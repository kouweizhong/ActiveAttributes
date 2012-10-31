﻿// Copyright (c) rubicon IT GmbH, www.rubicon.eu
//
// See the NOTICE file distributed with this work for additional information
// regarding copyright ownership.  rubicon licenses this file to you under 
// the Apache License, Version 2.0 (the "License"); you may not use this 
// file except in compliance with the License.  You may obtain a copy of the 
// License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the 
// License for the specific language governing permissions and limitations
// under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ActiveAttributes.Core.Aspects;
using ActiveAttributes.Core.Interception.Contexts;
using ActiveAttributes.Core.Interception.Invocations;
using Microsoft.Scripting.Ast;
using Remotion.Collections;
using Remotion.TypePipe.MutableReflection;
using Remotion.TypePipe.MutableReflection.BodyBuilding;

namespace ActiveAttributes.Core.Assembly.Old
{
  /// <summary>
  /// Patches a method so that it can be intercepted by applied aspects.
  /// </summary>
  /// <remarks>
  /// <code>
  /// var ctx = new InvocationContext(method, this, arg0, arg1, ...);
  /// var invocation0 = new Invocation(ctx, _originalMethodDelegate);
  /// var invocation1 = new OuterInvocation(ctx, Delegate.CreateDelegate (typeof (Action&lt;IInvocation&gt;), _aspects[1], method), invocation0);
  /// aspect[1].Intercept(invocation1);
  /// </code>
  /// </remarks>
  public class MethodPatcher : IMethodPatcher
  {
    private readonly MutableMethodInfo _mutableMethod;
    private readonly FieldInfo _propertyInfoFieldInfo;
    private readonly FieldInfo _eventInfoFieldInfo;
    private readonly FieldInfo _methodInfoFieldInfo;
    private readonly FieldInfo _delegateFieldInfo;
    private readonly IList<IExpressionGenerator> _aspects;
    private readonly IInvocationTypeProvider _invocationTypeProvider;


    private readonly MethodInfo _onInterceptMethodInfo;
    private readonly MethodInfo _onInterceptGetMethodInfo;
    private readonly MethodInfo _onInterceptSetMethodInfo;

    private readonly MethodInfo _createDelegateMethodInfo;

    public MethodPatcher (
        MutableMethodInfo mutableMethod,
        FieldInfo propertyInfoFieldInfo,
        FieldInfo eventInfoFieldInfo,
        FieldInfo methodInfoFieldInfo,
        FieldInfo delegateFieldInfo,
        IEnumerable<IExpressionGenerator> aspects,
        IInvocationTypeProvider invocationTypeProvider)
    {
      _mutableMethod = mutableMethod;
      _propertyInfoFieldInfo = propertyInfoFieldInfo;
      _eventInfoFieldInfo = eventInfoFieldInfo;
      _methodInfoFieldInfo = methodInfoFieldInfo;
      _delegateFieldInfo = delegateFieldInfo;
      _aspects = aspects.ToList();
      _invocationTypeProvider = invocationTypeProvider;

      _onInterceptMethodInfo = typeof (MethodInterceptionAspectAttribute).GetMethod ("OnIntercept");
      _onInterceptGetMethodInfo = typeof (PropertyInterceptionAspectAttribute).GetMethod ("OnInterceptGet");
      _onInterceptSetMethodInfo = typeof (PropertyInterceptionAspectAttribute).GetMethod ("OnInterceptSet");

      _createDelegateMethodInfo = typeof (Delegate).GetMethod (
          "CreateDelegate",
          new[] { typeof (Type), typeof (object), typeof (MethodInfo) });
    }

    public void AddMethodInterception ()
    {
      _mutableMethod.SetBody (GetPatchedBody);
    }

    private Expression GetPatchedBody (MethodBodyModificationContext ctx)
    {
      var methodInfoField = Expression.Field (ctx.This, _methodInfoFieldInfo);
      var delegateField = Expression.Field (ctx.This, _delegateFieldInfo);

      // InvocationContext<...> ctx = new InvocationContext<TInstance, TA1[, ...][, TR]> (_methodInfo, this, arg1, arg2[, ...]);
      var invocationContextType = _invocationTypeProvider.InvocationContextType;
      var invocationContext = Expression.Variable (invocationContextType, "ctx");
      var invocationContextCreateExpression = GetInvocationContextNewExpression (invocationContextType, methodInfoField, ctx.This, ctx.Parameters);
      var invocationContextAssignExpression = Expression.Assign (invocationContext, invocationContextCreateExpression);

      var invocationVariablesAndInitializations = GetInvocationVariablesAndAssignExpressions (invocationContext, delegateField, ctx.This);
      var invocations = invocationVariablesAndInitializations.Item1;
      var invocationInitExpressions = invocationVariablesAndInitializations.Item2;

      var outermostAspect = _aspects.Last().GetStorageExpression (ctx.This);
      var outermostAspectInterceptMethod = GetAspectInterceptMethod (_aspects.Last().AspectDescriptor.Type);
      var outermostInvocation = invocations.Last();
      var aspectCallExpression = GetOutermostAspectCallExpression (outermostAspect, outermostAspectInterceptMethod, outermostInvocation);

      var returnValueExpression = Expression.Property (invocationContext, "ReturnValue");

      return Expression.Block (
          new[] { invocationContext }.Concat (invocations),
          invocationContextAssignExpression,
          Expression.Block (invocationInitExpressions),
          aspectCallExpression,
          returnValueExpression);
    }

    private Tuple<ParameterExpression[], Expression[]> GetInvocationVariablesAndAssignExpressions (
        Expression invocationContext,
        Expression originalBodyDelegate,
        Expression thisExpression)
    {
      // var invocation0 = new InnerInvocation (invocationContext, _originalBodyDelegate);
      // var invocation1 = new OuterInvocation (invocationContext, Delegate.CreateDelegate (typeof (Action<IInvocation>), _aspects[0], method), invocation0);
      // var invocation2 = new OuterInvocation (invocationContext, Delegate.CreateDelegate (typeof (Action<IInvocation>), _aspects[1], method), invocation1);

      var invocations = new ParameterExpression[_aspects.Count];
      var invocationAssignExpressions = new Expression[_aspects.Count];

      for (var i = 0; i < _aspects.Count; i++)
      {
        Expression invocationCreateExpression;
        if (i == 0)
        {
          invocationCreateExpression = GetInnermostInvocationCreationExpression (invocationContext, originalBodyDelegate);
        }
        else
        {
          var innerAspectType = _aspects[i - 1].AspectDescriptor.Type;
          var innerAspect = _aspects[i - 1].GetStorageExpression (thisExpression);
          var innerAspectInterceptMethod = GetAspectInterceptMethod (innerAspectType);
          var innerInvocation = invocations[i - 1];

          invocationCreateExpression = GetOuterInvocationCreationExpression (
              invocationContext,
              innerAspect,
              innerAspectInterceptMethod,
              innerInvocation);
        }

        invocations[i] = Expression.Variable (invocationCreateExpression.Type, "invocation" + (i + 1));
        invocationAssignExpressions[i] = Expression.Assign (invocations[i], invocationCreateExpression);
      }

      return Tuple.Create (invocations, invocationAssignExpressions);
    }

    private NewExpression GetInvocationContextNewExpression (
        Type invocationContextType, Expression methodInfo, Expression thisExpression, IEnumerable<ParameterExpression> parameters)
    {
      var invocationContextConstructor = invocationContextType.GetConstructors().Single();

      var invocationContextArguments = new[] { methodInfo, thisExpression }.Concat (parameters.Cast<Expression>());
      if (typeof (IPropertyInvocationContext).IsAssignableFrom (invocationContextType))
      {
        invocationContextArguments = new[] { Expression.Field (thisExpression, _propertyInfoFieldInfo) }.Concat (invocationContextArguments);
      }

      var invocationContextCreateExpression = Expression.New (invocationContextConstructor, invocationContextArguments);
      return invocationContextCreateExpression;
    }

    private Expression GetInnermostInvocationCreationExpression (Expression invocationContext, Expression originalBodyDelegate)
    {
      var invocationType = _invocationTypeProvider.InvocationType;
      var invocationConstructor = invocationType.GetConstructors().Single();

      return Expression.New (invocationConstructor, invocationContext, originalBodyDelegate);
    }

    private Expression GetOuterInvocationCreationExpression (
        Expression invocationContext,
        Expression innerAspect,
        MethodInfo innerAspectInterceptMethod,
        Expression innerInvocation)
    {
      // new OuterInvocation (invocationContext, innerInvocationDelegate, innerInvocation)
      var invocationConstructor = typeof (OuterInvocation).GetConstructors().Single();
      var innerInvocationDelegate = GetInnerInvocationDelegateCreationExpression (innerAspect, innerAspectInterceptMethod);
      var outerInvocationCreateExpression = Expression.New (
          invocationConstructor, invocationContext, innerInvocationDelegate, innerInvocation);
      return outerInvocationCreateExpression;
    }

    private Expression GetInnerInvocationDelegateCreationExpression (Expression innerAspect, MethodInfo innerAspectInterceptMethod)
    {
      // (Action<IInvocation>) Delegate.CreateDelegate (typeof (Action<IInvocation>), innerAspect, innerAspectInterceptMethod)
      var innerInvocationDelegateType = Expression.Constant (typeof (Action<IInvocation>));
      var innerAspectInterceptMethod2 = Expression.Constant (innerAspectInterceptMethod);
      var innerInvocationUnconvertedDelegate = Expression.Call (
          null, _createDelegateMethodInfo, innerInvocationDelegateType, innerAspect, innerAspectInterceptMethod2);
      var innerInvocationDelegate = Expression.Convert (innerInvocationUnconvertedDelegate, typeof (Action<IInvocation>));
      return innerInvocationDelegate;
    }

    private Expression GetOutermostAspectCallExpression (
        Expression unconvertedOutermostAspect, MethodInfo aspectInterceptMethodInfo, Expression outermostInvocation)
    {
      var outermostAspect = Expression.Convert (unconvertedOutermostAspect, aspectInterceptMethodInfo.DeclaringType);
      var outermostAspectCall = Expression.Call (outermostAspect, aspectInterceptMethodInfo, new[] { outermostInvocation });
      return outermostAspectCall;
    }

    private MethodInfo GetAspectInterceptMethod (Type aspectType)
    {
      if (typeof (MethodInterceptionAspectAttribute).IsAssignableFrom (aspectType))
        return _onInterceptMethodInfo;
      else
      {
        if (_mutableMethod.Name.StartsWith ("set"))
          return _onInterceptSetMethodInfo;
        else
          return _onInterceptGetMethodInfo;
      }
    }
  }
}
// Copyright (c) rubicon IT GmbH, www.rubicon.eu
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
using ActiveAttributes.Core.Assembly.Old;
using ActiveAttributes.Core.Interception.Contexts;
using ActiveAttributes.Core.Interception.Invocations;
using Microsoft.Scripting.Ast;
using NUnit.Framework;
using Remotion.TypePipe.Expressions;
using Remotion.TypePipe.MutableReflection;
using Remotion.TypePipe.UnitTests.Expressions;
using Remotion.Utilities;
using Rhino.Mocks;

namespace ActiveAttributes.UnitTests.Assembly.Old
{
  [TestFixture]
  public class MethodPatcherTest : TestBase
  {
    private IExpressionGenerator _generator1;
    private IExpressionGenerator _generator2;

    private IAspectDescriptor _descriptor1;
    private IAspectDescriptor _descriptor2;

    private IEnumerable<IExpressionGenerator> _oneGenerator;
    private IEnumerable<IExpressionGenerator> _twoGenerators;

    private MethodInfo _createDelegate;
    private MethodInfo _onInterceptMethod;

    [SetUp]
    public override void SetUp ()
    {
      base.SetUp ();

      _descriptor1 = MockRepository.GenerateMock<IAspectDescriptor> ();
      _descriptor2 = MockRepository.GenerateMock<IAspectDescriptor> ();

      _descriptor1.Stub (x => x.Type).Return (typeof (MethodInterceptionAspectAttribute));
      _descriptor2.Stub (x => x.Type).Return (typeof (MethodInterceptionAspectAttribute));

      _generator1 = MockRepository.GenerateMock<IExpressionGenerator> ();
      _generator2 = MockRepository.GenerateMock<IExpressionGenerator>();

      _generator1.Stub (x => x.AspectDescriptor).Return (_descriptor1);
      _generator2.Stub (x => x.AspectDescriptor).Return (_descriptor2);

      var fieldInfo1 = MemberInfoFromExpressionUtility.GetField (() => DomainTypeBase.AspectField1);
      var fieldInfo2 = MemberInfoFromExpressionUtility.GetField (() => DomainTypeBase.AspectField2);

      var fieldExpression1 = Expression.Field (null, fieldInfo1);
      var fieldExpression2 = Expression.Field (null, fieldInfo2);

      _generator1.Stub (x => x.GetStorageExpression (null)).IgnoreArguments().Return (fieldExpression1);
      _generator2.Stub (x => x.GetStorageExpression (null)).IgnoreArguments().Return (fieldExpression2);
      
      _oneGenerator = new[] { _generator1 };
      _twoGenerators = new[] { _generator1, _generator2 };

      _createDelegate = typeof (Delegate).GetMethod ("CreateDelegate", new[] { typeof (Type), typeof (object), typeof (MethodInfo) });
      _onInterceptMethod = typeof (MethodInterceptionAspectAttribute).GetMethods().Single(x => x.Name == "OnIntercept");
    }

    public class DomainTypeBase
    {
      public static AspectAttribute AspectField1;
      public static AspectAttribute AspectField2;
      public PropertyInfo PropertyInfo;
      public EventInfo EventInfo;
      public MethodInfo MethodInfo;
    }

    public class DomainType : DomainTypeBase
    {
      public Action Delegate;
      public virtual void Method () { }
    }

    public class DomainType4 : DomainTypeBase
    {
      public Action<string> Delegate;
      public virtual string Property { get; set; }
    }

    [Test]
    public void ContainsInvocationContext ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // InvocationContext<TInstance, ...> ctx;
        var invocationContext = InvocationContext<ActionInvocationContext<DomainType>> ();
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, invocationContext);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.Method ()));
      PatchAndTest<DomainType> (methodInfo, _oneGenerator, test);
    }

    [Test]
    public void ContainsInvocationContextCreate_Method ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // new InvocationContext<TInstance> (this, method);
        var invocationContextCreate = InvocationContextCreate<ActionInvocationContext<DomainType>> (mutableMethod);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, invocationContextCreate);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.Method ()));
      PatchAndTest<DomainType> (methodInfo, _oneGenerator, test);
    }

    [Test]
    public void ContainsInvocationContextCreate_Property ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // new InvocationContext<TInstance> (this, method);
        var invocationContextCreate = InvocationContextCreate<PropertySetInvocationContext<DomainType4, string>> (mutableMethod);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, invocationContextCreate);
      };

      var methodInfo = typeof (DomainType4).GetMethods().Single (x => x.Name == "set_Property");
      PatchAndTest<DomainType4> (methodInfo, _oneGenerator, test);
    }

    [Test]
    public void ContainsInvocationContextAssign ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // var ctx = GetInvocationContext();
        var invocationContext = InvocationContext<ActionInvocationContext<DomainType>> ();
        var invocationContextCreate = InvocationContextCreate<ActionInvocationContext<DomainType>> (mutableMethod);
        var assign = Expression.Assign (invocationContext, invocationContextCreate);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, assign);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.Method ()));
      PatchAndTest<DomainType> (methodInfo, _oneGenerator, test);
    }

    [Test]
    public void ContainsFirstInvocation ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // Invocation invocation1;
        var invocation = InnerInvocation<ActionInvocation<DomainType>> (1);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, invocation);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.Method ()));
      PatchAndTest<DomainType> (methodInfo, _oneGenerator, test);
    }

    [Test]
    public void ContainsInnermostInvocationCreate_MethodInvocation ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // new Invocation(ctx, originalMethodDelegate)
        var invocationContext = InvocationContext<ActionInvocationContext<DomainType>> ();
        var invocationCreate = InnerInvocationCreate<ActionInvocation<DomainType>> (invocationContext);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, invocationCreate);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.Method ()));
      PatchAndTest<DomainType> (methodInfo, _oneGenerator, test);
    }

    [Test]
    public void ContainsInnermostInvocationCreate_PropertyInvocation ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // new Invocation(ctx, originalMethodDelegate)

        var invocationContext = InvocationContext<ActionInvocationContext<DomainType>>();
        var invocationCreate = InnerInvocationCreate<ActionInvocation<DomainType>> (invocationContext);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, invocationCreate);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.Method ()));
      PatchAndTest<DomainType> (methodInfo, _oneGenerator, test);
    }

    [Test]
    public void ContainsInnermostInvocationAssign ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // var invocation1 = GetInnermostInvocation();
        var invocationContext = InvocationContext<ActionInvocationContext<DomainType>> ();
        var invocation = InnerInvocation<ActionInvocation<DomainType>> (1);
        var invocationCreate = InnerInvocationCreate<ActionInvocation<DomainType>> (invocationContext);
        var expression = Expression.Assign (invocation, invocationCreate);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, expression);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.Method ()));
      PatchAndTest<DomainType> (methodInfo, _oneGenerator, test);
    }

    [Test]
    public void ContainsOuterInvocation ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // OuterInvocation invocation2;
        var invocation = OuterInvocation (2);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, invocation);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.Method ()));
      PatchAndTest<DomainType> (methodInfo, _twoGenerators, test);
    }

    [Test]
    public void ContainsOuterInvocationCreate ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // new OuterInvocation(ctx, innerInvocationDelegate, innerInvocation);
        var invocationContext = InvocationContext<ActionInvocationContext<DomainType>> ();
        var innerInvocation = InnerInvocation<ActionInvocation<DomainType>> (1);
        var outerInvocationCreate = OuterInvocationCreate (invocationContext, innerInvocation);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, outerInvocationCreate);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.Method ()));
      PatchAndTest<DomainType> (methodInfo, _twoGenerators, test);
    }

    [Test]
    public void ContainsOuterInvocationAssign ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // new OuterInvocation(ctx, innerInvocationDelegate, innerInvocation);
        var invocationContext = InvocationContext<ActionInvocationContext<DomainType>>();
        var innerInvocation = InnerInvocation<ActionInvocation<DomainType>> (1);
        var outerInvocation = OuterInvocation (2);
        var outerInvocationCreate = OuterInvocationCreate (invocationContext, innerInvocation);
        var assign = Expression.Assign (outerInvocation, outerInvocationCreate);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, assign);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.Method ()));
      PatchAndTest<DomainType> (methodInfo, _twoGenerators, test);
    }

    [Test]
    public void ContainsOutermostAspectCall ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // outermostAspect.Intercept(outermostInvocation);
        var outerInvocation = OuterInvocation (2);
        var outerAspectCall = OutermostAspectCall (_generator2, outerInvocation);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, outerAspectCall);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.Method ()));
      PatchAndTest<DomainType> (methodInfo, _twoGenerators, test);
    }

    [Test]
    public void ContainsPropertyAsReturnValue ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        var invocatonContext = InvocationContext<ActionInvocationContext<DomainType>> ();
        var propertyAsReturnValue = PropertyAsReturnValue (invocatonContext);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, propertyAsReturnValue);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.Method ()));
      PatchAndTest<DomainType> (methodInfo, _twoGenerators, test);
    }



    public class DomainType2 : DomainTypeBase
    {
      public Action<string, int> Delegate;
      public virtual void MethodWithArgs (string a, int b) { }
    }

    [Test]
    public void ContainsInvocationContextCreateWithArgs ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // new InvocationContext<TInstance, TA1, TA2> (this, method, arg1, arg2);
        var invocationContextCreate = InvocationContextCreate<ActionInvocationContext<DomainType2, string, int>> (mutableMethod);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, invocationContextCreate);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType2 obj) => obj.MethodWithArgs ("a", 1)));
      PatchAndTest<DomainType2> (methodInfo, _oneGenerator, test);
    }

    public class DomainType3 : DomainTypeBase
    {
      public Func<string> Delegate;
      public virtual string MethodWithReturn () { return ""; }
    }

    [Test]
    public void ContainsInvocationContextCreateWithReturn ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // new InvocationContext<TInstance, TA1, TA2> (this, method, arg1, arg2);
        var invocationContextCreate = InvocationContextCreate<FuncInvocationContext<DomainType3, string>> (mutableMethod);
        ExpressionTreeComparer2.CheckTreeContains (mutableMethod.Body, invocationContextCreate);
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType3 obj) => obj.MethodWithReturn()));
      PatchAndTest<DomainType3> (methodInfo, _oneGenerator, test);
    }


    [Test]
    public void FullTreeTest ()
    {
      Action<MutableMethodInfo> test = mutableMethod =>
      {
        // ActionInvocationContext ctx = new ActionInvocationContext<TInstance, TA1, TA2> (this, method);
        var invocationContext = InvocationContext<ActionInvocationContext<DomainType2, string, int>>();
        var invocationContextCreate = InvocationContextCreate<ActionInvocationContext<DomainType2, string, int>> (mutableMethod);
        var invocationContextAssign = Expression.Assign (invocationContext, invocationContextCreate);

        // ActionInvocation<TInstance, TA1, TA2> invocation1 = new ActionInvocation<TInstance, TA1, TA2> (ctx, methodDelegate);
        var innermostInvocation = InnerInvocation<ActionInvocation<DomainType2, string, int>> (1);
        var innermostInvocationCreate = InnerInvocationCreate<ActionInvocation<DomainType2, string, int>> (invocationContext);
        var innermostInvocationAssign = Expression.Assign (innermostInvocation, innermostInvocationCreate);

        // OuterInvocation invocation2 = new OuterInvocation(ctx, invocation1, aspect1);
        var outermostInvocation = OuterInvocation (2);
        var outermostInvocationCreate = OuterInvocationCreate (invocationContext, innermostInvocation);
        var outermostInvocationAssign = Expression.Assign (outermostInvocation, outermostInvocationCreate);

        // aspect2.OnIntercept
        var outermostAspectCall = OutermostAspectCall (_generator2, outermostInvocation);

        // return ctx.ReturnValue;
        var propertyAsReturnValue = PropertyAsReturnValue (invocationContext);

        var block = Expression.Block (
            new[] { invocationContext, innermostInvocation, outermostInvocation }.Cast<ParameterExpression>(),
            invocationContextAssign,
            Expression.Block (
                innermostInvocationAssign,
                outermostInvocationAssign),
            outermostAspectCall,
            propertyAsReturnValue);

        ExpressionTreeComparer.CheckAreEqualTrees (mutableMethod.Body, Expression.Block (typeof (void), block));
      };

      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType2 obj) => obj.MethodWithArgs ("a", 1)));
      PatchAndTest<DomainType2> (methodInfo, _twoGenerators, test);
    }



    private Expression OutermostAspectCall (IExpressionGenerator generator, Expression invocation)
    {
      var convertedAspect = Expression.Convert (generator.GetStorageExpression (null), generator.AspectDescriptor.Type);
      var call = Expression.Call (convertedAspect, _onInterceptMethod, new[] { invocation });
      return call;
    }

    private Expression OuterInvocationCreate (Expression invocationContext, Expression innerInvocation)
    {
      var declaringType = innerInvocation.Type.GetGenericArguments().First();
      var thisExpression = ThisExpression (declaringType);

      var innerInvocationDelegateType = Expression.Constant (typeof (Action<IInvocation>));
      var innerInvocationMethod = Expression.Constant (_onInterceptMethod);
      var innerInvocationDelgate = Expression.Call (
          null, _createDelegate, innerInvocationDelegateType, _generator1.GetStorageExpression (thisExpression), innerInvocationMethod);
      var create = Expression.New (
          typeof (OuterInvocation).GetConstructors().Single(),
          invocationContext,
          Expression.Convert (innerInvocationDelgate, typeof (Action<IInvocation>)),
          innerInvocation);

      return create;
    }

    private Expression InnerInvocationCreate<TInvocation> (Expression invocationContext) where TInvocation : IInvocation
    {
      var declaringType = typeof (TInvocation).GetGenericArguments().First();
      var delegate2 = Field (declaringType, "Delegate");

      var invocationCreate = Expression.New (
          typeof (TInvocation).GetConstructors().Single(),
          invocationContext,
          delegate2);
      return invocationCreate;
    }

    private Expression InvocationContext<TInvocationContext> ()
    {
      return Expression.Variable (typeof (TInvocationContext), "ctx");
    }

    private Expression InnerInvocation<TInvocation> (int index)
    {
      return Expression.Variable (typeof (TInvocation), "invocation" + index);
    }

    private Expression OuterInvocation (int index)
    {
      return Expression.Variable (typeof (OuterInvocation), "invocation" + index);
    }

    private Expression PropertyAsReturnValue (Expression ctx)
    {
      return Expression.Property (ctx, "ReturnValue");
    }

    private NewExpression InvocationContextCreate<TInvocationContext> (MutableMethodInfo mutableMethod) where TInvocationContext : IInvocationContext
    {
      var declaringType = typeof (TInvocationContext).GetGenericArguments().First();

      var propertyInfoField = Field (declaringType, "PropertyInfo");
      var eventInfo = Field (declaringType, "EventInfo");
      var methodInfoField = Field (declaringType, "MethodInfo");
      var this2 = ThisExpression (declaringType);
      var parameters = mutableMethod.GetParameters ().Select (x => Expression.Parameter (x.ParameterType, x.Name)).Cast<Expression> ();

      var arguments = default (IEnumerable<Expression>);
      if (declaringType == typeof (DomainType))
        arguments = new[] { methodInfoField, this2 }.Concat (parameters);
      else if (declaringType == typeof (DomainType2))
        arguments = new[] { methodInfoField, this2 }.Concat (parameters);
      else if (declaringType == typeof (DomainType3))
        arguments = new[] { methodInfoField, this2 }.Concat (parameters);
      else if (declaringType == typeof (DomainType4))
        arguments = new[] { propertyInfoField, methodInfoField, this2 }.Concat (parameters);

      var invocationContextCreate = Expression.New (typeof (TInvocationContext).GetConstructors().Single(), arguments);
      return invocationContextCreate;
    }

    private Expression Field (Type declaringType, string fieldName)
    {
      var fieldInfo = declaringType.GetFields().Single (x => x.Name == fieldName);
      return Expression.Field (ThisExpression (declaringType), fieldInfo);
    }

    private Expression ThisExpression (Type type)
    {
      return new ThisExpression (type);
    }

    private void PatchAndTest<T> (MethodInfo methodInfo, IEnumerable<IExpressionGenerator> aspects, Action<MutableMethodInfo> test)
    {
      AssembleType<T> (
          mutableType =>
          {
            var mutableMethod = mutableType.GetOrAddMutableMethod (methodInfo);

            var propertyInfoField = typeof (T).GetFields().Single(x => x.Name == "PropertyInfo");
            var eventInfoField = typeof (T).GetFields().Single(x => x.Name == "EventInfo");
            var methodInfoField = typeof (T).GetFields().Single(x => x.Name == "MethodInfo");
            var delegateField = typeof (T).GetFields().Single(x => x.Name == "Delegate");

            var typeProvider = new InvocationTypeProvider (mutableMethod.UnderlyingSystemMethodInfo);
            var patcher = new MethodPatcher (mutableMethod, propertyInfoField, eventInfoField, methodInfoField, delegateField, aspects, typeProvider);
            patcher.AddMethodInterception();

            test (mutableMethod);
          });
    }
  }
}
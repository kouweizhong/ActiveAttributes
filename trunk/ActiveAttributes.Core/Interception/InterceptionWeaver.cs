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
using System.Reflection;
using ActiveAttributes.Core.AdviceInfo;
using ActiveAttributes.Core.Assembly;
using ActiveAttributes.Core.Discovery;
using Microsoft.Scripting.Ast;
using Remotion.Collections;
using Remotion.TypePipe.MutableReflection;
using Remotion.Utilities;
using System.Linq;

namespace ActiveAttributes.Core.Interception
{
  public class InterceptionWeaver : IWeaver
  {
    private readonly IInterceptionExpressionHelperFactory _expressionHelperFactory;
    private readonly IInitializationService _initializationService;

    public InterceptionWeaver (
        IInterceptionExpressionHelperFactory expressionHelperFactory,
        IInitializationService initializationService)
    {
      ArgumentUtility.CheckNotNull ("expressionHelperFactory", expressionHelperFactory);
      ArgumentUtility.CheckNotNull ("initializationService", initializationService);

      _expressionHelperFactory = expressionHelperFactory;
      _initializationService = initializationService;
    }

    public void Weave (MethodInfo method, IEnumerable<Advice> advices)
    {
      ArgumentUtility.CheckNotNull ("method", method);
      ArgumentUtility.CheckNotNull ("advices", advices);
      var advicesAsList = advices.ToList ();
      Assertion.IsNotNull (method.DeclaringType);
      Assertion.IsTrue (method.DeclaringType is MutableType);
      Assertion.IsTrue (advicesAsList.All (x => x.Execution == AdviceExecution.Around));

      var mutableType = (MutableType) method.DeclaringType;
      var mutableMethod = mutableType.GetOrAddMutableMethod (method);

      var memberInfoField = _initializationService.AddMemberInfo (mutableMethod);
      var delegateField = _initializationService.AddDelegate (mutableMethod);
      var adviceTuples = advicesAsList.Select (x => Tuple.Create (x.Method, _initializationService.GetOrAddAspect (x, mutableType))).ToList();

      mutableMethod.SetBody (
          ctx =>
          {
            var helper = _expressionHelperFactory.Create (method, ctx, adviceTuples, memberInfoField, delegateField);

            var contextTuple = helper.CreateMethodInvocationExpressions();
            var methodInvocationParam = contextTuple.Item1;
            var methodInvocationAssign = contextTuple.Item2;
            var invocationTuples = helper.CreateAdviceInvocationExpressions (methodInvocationParam).ToList ();
            var invocationParams = invocationTuples.Select (x => x.Item1);
            var invocationAssigns = invocationTuples.Select (x => x.Item2).Cast<Expression>();
            var adviceCall = helper.CreateOutermostAdviceCallExpression (methodInvocationParam, invocationParams.Last());
            var returnValue = helper.CreateReturnValueExpression (methodInvocationParam);

            return Expression.Block (
                new[] { methodInvocationParam }.Concat (invocationParams),
                methodInvocationAssign,
                Expression.Block (invocationAssigns),
                adviceCall,
                returnValue);
          });
    }
  }
}
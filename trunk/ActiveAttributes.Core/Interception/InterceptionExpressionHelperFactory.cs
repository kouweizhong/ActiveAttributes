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
using System.Reflection;
using ActiveAttributes.Core.Assembly;
using ActiveAttributes.Core.Assembly.FieldWrapper;
using Microsoft.Scripting.Ast;
using Remotion.Collections;
using Remotion.TypePipe.MutableReflection.BodyBuilding;
using Remotion.Utilities;

namespace ActiveAttributes.Core.Interception
{
  public interface IInterceptionExpressionHelperFactory
  {
    IInterceptionExpressionHelper Create (
        MethodInfo method,
        BodyContextBase bodyContextBase,
        IEnumerable<Tuple<MethodInfo, IFieldWrapper>> advices,
        IFieldWrapper memberInfoField,
        IFieldWrapper delegateField);
  }

  public class InterceptionExpressionHelperFactory : IInterceptionExpressionHelperFactory
  {
    private readonly IInterceptionTypeProvider _interceptionTypeProvider;

    public InterceptionExpressionHelperFactory (IInterceptionTypeProvider interceptionTypeProvider)
    {
      ArgumentUtility.CheckNotNull ("interceptionTypeProvider", interceptionTypeProvider);

      _interceptionTypeProvider = interceptionTypeProvider;
    }

    public IInterceptionExpressionHelper Create (
        MethodInfo method,
        BodyContextBase bodyContextBase,
        IEnumerable<Tuple<MethodInfo, IFieldWrapper>> advices,
        IFieldWrapper memberInfoField,
        IFieldWrapper delegateField)
    {
      ArgumentUtility.CheckNotNull ("method", method);
      ArgumentUtility.CheckNotNull ("bodyContextBase", bodyContextBase);
      ArgumentUtility.CheckNotNull ("advices", advices);
      ArgumentUtility.CheckNotNull ("memberInfoField", memberInfoField);
      ArgumentUtility.CheckNotNull ("delegateField", delegateField);

      var invocationExpressionHelper = new InvocationExpressionHelper();
      Type invocationType;
      Type invocationContextType;
      _interceptionTypeProvider.GetTypes (method, out invocationType, out invocationContextType);

      return new InterceptionExpressionHelper (
          invocationExpressionHelper,
          method,
          bodyContextBase.This,
          bodyContextBase.Parameters,
          invocationType,
          invocationContextType,
          advices,
          memberInfoField,
          delegateField);
    }
  }
}
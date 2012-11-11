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
using System.Reflection;
using ActiveAttributes.Interception.Invocations;
using Microsoft.Scripting.Ast;
using Remotion.ServiceLocation;
using System.Linq;

namespace ActiveAttributes.Interception
{
  [ConcreteImplementation (typeof (CallExpressionHelper))]
  public interface ICallExpressionHelper
  {
    MethodCallExpression CreateAdviceCallExpression (
        Expression methodInvocation, Expression aspect, MethodInfo advice, Expression invocation);
  }

  public class CallExpressionHelper : ICallExpressionHelper
  {
    public MethodCallExpression CreateAdviceCallExpression (Expression methodInvocation, Expression aspect, MethodInfo advice, Expression invocation)
    {
      var arguments = advice.GetParameters().Select (x => GetParameterExpression (invocation, methodInvocation, x));
      return Expression.Call (aspect, advice, arguments.ToArray());
    }

    private Expression GetParameterExpression (Expression invocation, Expression methodInvocation, ParameterInfo parameterInfo)
    {
      if (parameterInfo.ParameterType == typeof (IInvocation))
        return invocation;

      try
      {
        var parameterType = parameterInfo.ParameterType;
        if (parameterType.IsByRef)
          parameterType = parameterType.GetElementType();

        var invocationType = methodInvocation.Type;
        var invocationField = invocationType.GetFields().Single (x => parameterType.IsAssignableFrom (x.FieldType));
        return Expression.Field (methodInvocation, invocationField);
      }
      catch (Exception)
      {
        // TODO exception
        throw;
      }
    }
  }
}
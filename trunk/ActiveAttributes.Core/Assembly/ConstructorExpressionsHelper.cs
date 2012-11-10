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
using System.Reflection;
using ActiveAttributes.Core.Assembly.Storages;
using ActiveAttributes.Core.Extensions;
using Microsoft.Scripting.Ast;
using Remotion.ServiceLocation;
using Remotion.TypePipe.MutableReflection.BodyBuilding;
using Remotion.Utilities;

namespace ActiveAttributes.Core.Assembly
{
  [ConcreteImplementation (typeof (ConstructorExpressionsHelper))]
  public interface IConstructorExpressionsHelper
  {
    BinaryExpression CreateMemberInfoAssignExpression (IStorage field, MemberInfo method);

    BinaryExpression CreateDelegateAssignExpression (IStorage field, MethodInfo method);
  }

  public class ConstructorExpressionsHelper : IConstructorExpressionsHelper
  {
    private readonly IAspectInitializationExpressionHelper _aspectInitializationExpressionHelper;
    private readonly BodyContextBase _context;

    public ConstructorExpressionsHelper (IAspectInitializationExpressionHelper aspectInitializationExpressionHelper, BodyContextBase context)
    {
      ArgumentUtility.CheckNotNull ("aspectInitializationExpressionHelper", aspectInitializationExpressionHelper);
      ArgumentUtility.CheckNotNull ("context", context);

      _aspectInitializationExpressionHelper = aspectInitializationExpressionHelper;
      _context = context;
    }

    public BinaryExpression CreateMemberInfoAssignExpression (IStorage field, MemberInfo member)
    {
      ArgumentUtility.CheckNotNull ("field", field);
      ArgumentUtility.CheckNotNull ("member", member);

      if (member is MethodInfo)
        return CreateMemberInfoAssignExpression (field, (MethodInfo) member);
      if (member is PropertyInfo)
        return CreateMemberInfoAssignExpression (field, (PropertyInfo) member);

      throw new NotSupportedException();
    }

    public BinaryExpression CreateDelegateAssignExpression (IStorage field, MethodInfo method)
    {
      ArgumentUtility.CheckNotNull ("field", field);
      ArgumentUtility.CheckNotNull ("method", method);

      var value = Expression.NewDelegate (method.GetDelegateType(), _context.This, method);

      return GetAssignExpression (field, value);
    }


    private BinaryExpression CreateMemberInfoAssignExpression (IStorage field, MethodInfo method)
    {
      ArgumentUtility.CheckNotNull ("field", field);
      ArgumentUtility.CheckNotNull ("method", method);

      return GetAssignExpression<MethodInfo> (field, method);
    }

    private BinaryExpression CreateMemberInfoAssignExpression (IStorage field, PropertyInfo property)
    {
      ArgumentUtility.CheckNotNull ("field", field);
      ArgumentUtility.CheckNotNull ("property", property);

      var value = Expression.Call (
          Expression.Constant (_context.DeclaringType),
          typeof (Type).GetMethod ("GetProperty", new[] { typeof (string), typeof (BindingFlags) }),
          Expression.Constant (property.Name),
          Expression.Constant (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));

      return GetAssignExpression (field, value);
    }


    private BinaryExpression GetAssignExpression<T> (IStorage field, object constant)
    {
      var constantExpression = Expression.Constant (constant, typeof (T));

      return GetAssignExpression (field, constantExpression);
    }

    private BinaryExpression GetAssignExpression (IStorage field, Expression value)
    {
      return Expression.Assign (GetFieldAccessExpression (field), value);
    }

    private Expression GetFieldAccessExpression (IStorage storage)
    {
      return storage.GetStorageExpression (_context.This);
    }
  }
}
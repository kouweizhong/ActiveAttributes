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
// 
using System;
using System.Linq;
using Microsoft.Scripting.Ast;

namespace ActiveAttributes.Core.Assembly
{
  public class AssemblyArrayAccessor : IArrayAccessor
  {
    private readonly Expression _accessExpression;

    public AssemblyArrayAccessor (string assemblyKey)
    {
      var dictionaryPropertyInfo = typeof (AssemblyAspectManager).GetProperties ().Single ();
      var dictionary = Expression.Property (null, dictionaryPropertyInfo);

      var assemblyFullName = Expression.Constant(assemblyKey);
      _accessExpression = Expression.Property (dictionary, "Item", assemblyFullName);
    }

    public Expression GetAccessExpression (Expression thisExpression)
    {
      return _accessExpression;
    }
  }
}
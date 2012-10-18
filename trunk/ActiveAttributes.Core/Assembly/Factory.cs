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
using ActiveAttributes.Core.Assembly.Accessors;
using Remotion.TypePipe.MutableReflection;

namespace ActiveAttributes.Core.Assembly
{
  public class Factory : IFactory
  {
    public ITypeProvider GetTypeProvider (MethodInfo methodInfo)
    {
      return new TypeProvider (methodInfo);
    }

    public IArrayAccessor GetAccessor (FieldInfo fieldInfo)
    {
      return fieldInfo.IsStatic
                 ? (IArrayAccessor) new StaticArrayAccessor (fieldInfo)
                 : new InstanceArrayAccessor (fieldInfo);
    }

    public IMethodPatcher GetMethodPatcher (
        MutableMethodInfo mutableMethod,
        FieldInfoContainer fieldInfoContainer,
        IEnumerable<IExpressionGenerator> aspects,
        ITypeProvider typeProvider)
    {
      return new MethodPatcher (
          mutableMethod,
          fieldInfoContainer.PropertyInfoField,
          fieldInfoContainer.EventInfoField,
          fieldInfoContainer.MethodInfoField,
          fieldInfoContainer.DelegateField,
          aspects,
          typeProvider);
    }

    public IExpressionGenerator GetGenerator (IArrayAccessor arrayAccessor, int index, IAspectDescriptor aspectDescriptor)
    {
      return new ExpressionGenerator (arrayAccessor, index, aspectDescriptor);
    }
  }
}
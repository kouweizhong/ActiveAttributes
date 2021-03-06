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

namespace ActiveAttributes.Weaving.Context
{
  public class PropertySetContext<TInstance, TValue> : ActionContext<TInstance, TValue>, IPropertyContext
  {
    private readonly PropertyInfo _propertyInfo;

    public PropertySetContext (PropertyInfo propertyInfo, TInstance instance, TValue arg0)
        : base (propertyInfo, instance, arg0)
    {
      _propertyInfo = propertyInfo;
    }

    public object Index
    {
      get { throw new NotSupportedException(); }
    }

    public bool IsIndexer
    {
      get { return false; }
    }

    public object Value
    {
      get { return this[0]; }
      set { this[0] = value; }
    }

    public PropertyInfo PropertyInfo
    {
      get { return _propertyInfo; }
    }
  }
}
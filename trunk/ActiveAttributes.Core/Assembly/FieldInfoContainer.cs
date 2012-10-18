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

namespace ActiveAttributes.Core.Assembly
{
  public struct FieldInfoContainer
  {
    public FieldInfo DelegateField;
    public FieldInfo MethodInfoField;
    public FieldInfo PropertyInfoField;
    public FieldInfo EventInfoField;

    // TODO Replace these:
    public FieldInfo StaticAspectsField;
    public FieldInfo InstanceAspectsField;

    // TODO With these:
    // IArrayAccessor StaticAspectsField - instead of FieldInfo
    // IArrayAccessor InstanceAspectsField - instead of FieldInfo

    // TODO Remove IFactory.GetArrayAccessor method, move implementation to FieldIntroducer - create StaticArrayAccessor/InstanceArrayAccessor there.
  }
}
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
using ActiveAttributes.Core.Assembly.Configuration;
using ActiveAttributes.Core.Assembly.Descriptors;
using Remotion.Utilities;

namespace ActiveAttributes.Core.Assembly.Providers
{
  public class MethodParameterAspectProvider : IMethodLevelAspectProvider
  {
    public IEnumerable<IAspectDescriptor> GetAspects (MemberInfo member)
    {
      ArgumentUtility.CheckNotNull ("member", member);
      Assertion.IsTrue (member is MethodBase);

      var method = (MethodBase) member;
      var parameters = method.GetParameters();
      var parameterAspects = parameters.SelectMany (x => x.GetCustomAttributes (true)).OfType<AspectAttribute>();
      var appliedAspects = parameterAspects.SelectMany (x => x.GetType ().GetCustomAttributes (true)).OfType<ApplyAspectAttribute> ();
      var distinctAspects = appliedAspects.Select (x => x.AspectType).Distinct ();
      return distinctAspects.Select (x => new TypeDescriptor (x, AspectScope.Static, 0)).Cast<IAspectDescriptor>();
    }
  }
}
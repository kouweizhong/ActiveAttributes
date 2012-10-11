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
using ActiveAttributes.Core.Assembly.Descriptors;
using ActiveAttributes.Core.Extensions;
using Remotion.FunctionalProgramming;

namespace ActiveAttributes.Core.Assembly.Providers
{
  public class AspectProvider
  {
    private static AspectProvider s_singleton;

    public static AspectProvider Singleton
    {
      get { return s_singleton = s_singleton ?? new AspectProvider(); }
      set { s_singleton = value; }
    }

    public static IEnumerable<IAspectDescriptor> GetAspects (MemberInfo baseMember, IEnumerable<MemberInfo> members)
    {
      return Singleton.GetAspectsInternal (baseMember, members);
    }

    public virtual IEnumerable<IAspectDescriptor> GetAspectsInternal (MemberInfo baseMember, IEnumerable<MemberInfo> members)
    {
      var aspectsData = new List<CustomAttributeData>();
      var membersAsCollection = members.ConvertToCollection();

      foreach (var member in membersAsCollection)
      {
        var isBase = member == baseMember;
        var customAttributeDatas = CustomAttributeData.GetCustomAttributes (member)
            .Where (x => x.IsAspectAttribute())
            .Where (x => isBase || x.IsInheriting());
        // TODO add AllowMultiple
        // TODO TypePipeCustomAttributeData: add parameter for including interfaces

        aspectsData.AddRange (customAttributeDatas);
      }

      return aspectsData.Select (x => new CustomDataDescriptor (x)).Cast<IAspectDescriptor>();
    }
  }
}
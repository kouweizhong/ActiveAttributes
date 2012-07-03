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
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using ActiveAttributes.Core.Aspects;

using Remotion.Reflection.MemberSignatures;
using Remotion.TypePipe.MutableReflection;

namespace ActiveAttributes.Core.Assembly
{
  public class AspectsProvider
  {
    private readonly IRelatedMethodFinder _relatedMethodFinder;

    public AspectsProvider ()
    {
      _relatedMethodFinder = new RelatedMethodFinder();
    }

    // TODO: support aspects on interfaces?
    public IEnumerable<CompileTimeAspectBase> GetAspects (MethodInfo methodInfo)
    {
      // first level method aspects
      foreach (var compileTimeAspect in GetMethodLevelAspects (methodInfo, isBaseType: false))
        yield return compileTimeAspect;

      var declaringType = methodInfo.DeclaringType;
      var applyAspectsAttributes = declaringType.GetCustomAttributes (typeof (ApplyAspectAttribute), false).Cast<ApplyAspectAttribute>();
      foreach (var applyAspectsAttribute in applyAspectsAttributes)
      {
        yield return new TypeArgsCompileTimeAspect (applyAspectsAttribute.AspectType, applyAspectsAttribute.Arguments);
      }

      // inherited level method aspects
      while ((methodInfo = _relatedMethodFinder.GetBaseMethod (methodInfo)) != null)
      {
        foreach (var compileTimeAspect in GetMethodLevelAspects (methodInfo, isBaseType: true))
          yield return compileTimeAspect;
      }
    }

    private IEnumerable<CompileTimeAspectBase> GetMethodLevelAspects (MethodInfo methodInfo, bool isBaseType)
    {
      var customDatas = CustomAttributeData.GetCustomAttributes (methodInfo);
      if (isBaseType)
        customDatas = customDatas.Where (x => x.IsInheriting()).ToArray();

      return customDatas.Select (customData => new CustomDataCompileTimeAspect (customData)).Cast<CompileTimeAspectBase>();
    }
  }

  public static class CustomAttributeDataExtensions
  {
    public static bool IsInheriting (this CustomAttributeData customAttributeData)
    {
      var attributeType = customAttributeData.Constructor.DeclaringType;
      var attributeUsageAttr = attributeType.GetCustomAttributes (typeof (AttributeUsageAttribute), true).Cast<AttributeUsageAttribute>().Single();
      return attributeUsageAttr.Inherited;
    }
  }
}
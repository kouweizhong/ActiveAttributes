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
using ActiveAttributes.Core.Extensions;
using ActiveAttributes.Core.Infrastructure;
using Remotion.FunctionalProgramming;
using Remotion.TypePipe.MutableReflection;
using Remotion.Utilities;

namespace ActiveAttributes.Core.Discovery.AttributedAspectDeclarationProviders
{
  public class PropertyAttributedAdviceDeclarationProvider : IMethodLevelAdviceDeclarationProvider
  {
    private readonly IAttributeDeclarationProvider _attributeDeclarationProvider;
    private readonly IRelatedPropertyFinder _relatedPropertyFinder;

    public PropertyAttributedAdviceDeclarationProvider (
        IAttributeDeclarationProvider attributeDeclarationProvider, IRelatedPropertyFinder relatedPropertyFinder)
    {
      ArgumentUtility.CheckNotNull ("attributeDeclarationProvider", attributeDeclarationProvider);
      ArgumentUtility.CheckNotNull ("relatedPropertyFinder", relatedPropertyFinder);

      _attributeDeclarationProvider = attributeDeclarationProvider;
      _relatedPropertyFinder = relatedPropertyFinder;
    }

    //public IEnumerable<AspectDeclaration> GetDeclarations (MethodInfo method)
    //{
    //  ArgumentUtility.CheckNotNull ("method", method);

    //  var property = method.GetRelatedPropertyInfo();
    //  if (property == null)
    //    return Enumerable.Empty<AspectDeclaration>();

    //  var propertySequence = property.CreateSequence (x => _relatedPropertyFinder.GetBaseProperty (x));
    //  return propertySequence.SelectMany (x => _attributeDeclarationProvider.GetAspectDeclarations (x));
    //}
    public IEnumerable<Advice> GetDeclarations (MethodInfo method)
    {
      throw new NotImplementedException();
    }
  }
}
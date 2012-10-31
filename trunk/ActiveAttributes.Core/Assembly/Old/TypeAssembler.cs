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
using System.Linq;
using ActiveAttributes.Core.Configuration2;
using Remotion.Logging;
using Remotion.TypePipe.MutableReflection;
using Remotion.TypePipe.TypeAssembly;

namespace ActiveAttributes.Core.Assembly.Old
{
  public class TypeAssembler : IActiveAttributesAssembler
  {
    private static readonly ILog s_log = LogManager.GetLogger (typeof (TypeAssembler));

    public static ITypeAssemblyParticipant Singleton { get; private set; }

    private readonly IEnumerable<ITypeLevelAspectDescriptorProvider> _typeLevelAspectDescriptorProviders;
    private readonly IFieldIntroducer _fieldIntroducer;
    private readonly IGiveMeSomeName _giveMeSomeName;
    private readonly IMethodAssembler _methodAssembler;

    public TypeAssembler (
        IEnumerable<ITypeLevelAspectDescriptorProvider> typeLevelAspectDescriptorProviders,
        IFieldIntroducer fieldIntroducer,
        IGiveMeSomeName giveMeSomeName,
        IMethodAssembler methodAssembler)
    {
      _typeLevelAspectDescriptorProviders = typeLevelAspectDescriptorProviders;
      _methodAssembler = methodAssembler;
      _fieldIntroducer = fieldIntroducer;
      _giveMeSomeName = giveMeSomeName;
    }

    public void ModifyType (MutableType mutableType)
    {
      var descriptors = _typeLevelAspectDescriptorProviders.SelectMany (x => x.GetDescriptors (mutableType)).ToArray ();
      var fields = _fieldIntroducer.IntroduceTypeFields (mutableType);
      var generators = _giveMeSomeName.IntroduceExpressionGenerators (mutableType, descriptors, fields).ToArray();

      foreach (var method in mutableType.GetMethods())
      {
        _methodAssembler.ModifyMethod (mutableType, method, generators);
      }
    }
  }
}
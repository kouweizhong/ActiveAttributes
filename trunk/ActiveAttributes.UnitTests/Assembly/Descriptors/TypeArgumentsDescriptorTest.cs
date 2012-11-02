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

using ActiveAttributes.Core.Aspects;
using ActiveAttributes.Core.Assembly.Descriptors;
using ActiveAttributes.Core.Configuration2;
using ActiveAttributes.Core.Infrastructure;
using ActiveAttributes.Core.Infrastructure.AdviceInfo;
using NUnit.Framework;

namespace ActiveAttributes.UnitTests.Assembly.Descriptors
{
  [TestFixture]
  public class TypeArgumentsDescriptorTest
  {
    public class Initialize
    {
      [Test]
      public void SetsInformation ()
      {
        var aspectType = typeof (AspectAttribute);
        var scope = AdviceScope.Instance;
        var priority = 5;
        var descriptor = new TypeAspectDescriptor (aspectType, scope, priority);

        Assert.That (descriptor.Type, Is.EqualTo (aspectType));
        Assert.That (descriptor.AdviceScope, Is.EqualTo (scope));
        Assert.That (descriptor.Priority, Is.EqualTo (priority));
      }
    } 
  }
}
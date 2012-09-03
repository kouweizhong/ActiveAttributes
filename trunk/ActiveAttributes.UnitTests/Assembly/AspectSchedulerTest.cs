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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ActiveAttributes.Core.Aspects;
using ActiveAttributes.Core.Assembly;
using ActiveAttributes.Core.Configuration;
using ActiveAttributes.Core.Configuration.Rules;
using NUnit.Framework;
using Rhino.Mocks;

namespace ActiveAttributes.UnitTests.Assembly
{
  [TestFixture]
  public class AspectSchedulerTest
  {
    private IAspectScheduler _scheduler;
    private IAspectConfiguration _configuration;

    private IAspectDescriptor _descriptor1;
    private IAspectDescriptor _descriptor2;
    private IAspectDescriptor _descriptor3;
    private IAspectDescriptor _descriptor4;

    [SetUp]
    public void SetUp ()
    {
      _configuration = MockRepository.GenerateMock<IAspectConfiguration>();
      _scheduler = new AspectScheduler (_configuration);

      _descriptor1 = MockRepository.GenerateMock<IAspectDescriptor>();
      _descriptor2 = MockRepository.GenerateMock<IAspectDescriptor>();
      _descriptor3 = MockRepository.GenerateMock<IAspectDescriptor>();
      _descriptor4 = MockRepository.GenerateMock<IAspectDescriptor>();

      _descriptor1.Expect (x => x.AspectType).Return (typeof (Aspect1));
      _descriptor2.Expect (x => x.AspectType).Return (typeof (Aspect2));
      _descriptor3.Expect (x => x.AspectType).Return (typeof (Aspect3));
      _descriptor4.Expect (x => x.AspectType).Return (typeof (Aspect4));
    }

    [Test]
    public void Orders ()
    {
      var rules = new List<IOrderRule>
                  {
                    new TypeOrderRule(typeof(Aspect2), typeof(Aspect1)),
                    new TypeOrderRule(typeof(Aspect1), typeof(Aspect3))
                  };
      _configuration.Expect (x => x.Rules).Return (rules);
      var aspects = new[] { _descriptor1, _descriptor2, _descriptor3 };

      var actual = _scheduler.GetOrdered (aspects);
      var expected = new[] { _descriptor2, _descriptor1, _descriptor3 };
      Assert.That (actual, Is.EqualTo (expected));
    }

    [Test]
    public void ThrowsForCircularDependency ()
    {
      var rules = new List<IOrderRule>
                  {
                    new TypeOrderRule(typeof(Aspect2), typeof(Aspect1)),
                    new TypeOrderRule(typeof(Aspect1), typeof(Aspect2))
                  };
      _configuration.Expect (x => x.Rules).Return (rules);
      var aspects = new[] { _descriptor1, _descriptor2, _descriptor3 };

      Assert.That (() => _scheduler.GetOrdered (aspects), Throws.InvalidOperationException);
    }

    [Test]
    public void ThrowsForUndefinedOrder ()
    {
      var rules = new List<IOrderRule>
                  {
                    new TypeOrderRule(typeof(Aspect2), typeof(Aspect1)),
                  };
      _configuration.Expect (x => x.Rules).Return (new ReadOnlyCollection<IOrderRule> (rules));
      var aspects = new[] { _descriptor1, _descriptor2, _descriptor3 };

      Assert.That (() => _scheduler.GetOrdered (aspects), Throws.InvalidOperationException);
    }

    [Test]
    public void PriorityOverRules ()
    {
      _descriptor3.Expect (x => x.Priority).Return (1);
      var rules = new List<IOrderRule>
                  {
                    new TypeOrderRule(typeof(Aspect2), typeof(Aspect1)),
                    new TypeOrderRule(typeof(Aspect1), typeof(Aspect3)),
                  };
      _configuration.Expect (x => x.Rules).Return (rules);
      var aspects = new[] { _descriptor1, _descriptor3, _descriptor2 };

      var actual = _scheduler.GetOrdered (aspects).ToArray();
      var expected = new[] { _descriptor3, _descriptor2, _descriptor1 };
      Assert.That (actual, Is.EqualTo (expected));
    }

    private class Aspect1 : AspectAttribute { }
    private class Aspect2 : AspectAttribute { }
    private class Aspect3 : AspectAttribute { }
    private class Aspect4 : AspectAttribute { }
  }
}
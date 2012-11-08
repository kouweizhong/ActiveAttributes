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
using ActiveAttributes.Core.Assembly;
using ActiveAttributes.Core.Attributes.AdviceInfo;
using ActiveAttributes.Core.Discovery;
using ActiveAttributes.Core.Infrastructure;
using ActiveAttributes.Core.Infrastructure.AdviceInfo;
using ActiveAttributes.Core.Infrastructure.Construction;
using ActiveAttributes.Core.Pointcuts;
using NUnit.Framework;
using Remotion.Development.UnitTesting.Reflection;
using Rhino.Mocks;

namespace ActiveAttributes.UnitTests.Discovery
{
  [TestFixture]
  public class CustomAttributeProviderTransformTest
  {
    private CustomAttributeProviderTransform _transform;

    private IAdviceBuilder _builderMock;
    private IAdviceBuilderFactory _adviceBuilderFactoryMock;

    [SetUp]
    public void SetUp ()
    {
      _builderMock = MockRepository.GenerateStrictMock<IAdviceBuilder>();
      _adviceBuilderFactoryMock = MockRepository.GenerateStrictMock<IAdviceBuilderFactory>();

      _transform = new CustomAttributeProviderTransform (_adviceBuilderFactoryMock);
    }

    [Test]
    public void GetAdviceBuilder ()
    {
      var method = NormalizingMemberInfoFromExpressionUtility.GetMethod ((DomainAspect obj) => obj.Method1());

      _adviceBuilderFactoryMock.Expect (x => x.Create ()).Return (_builderMock);
      _builderMock.Expect (x => x.SetMethod (method)).Return (_builderMock);
      _builderMock.Expect (x => x.SetName ("Name")).Return (_builderMock);
      _builderMock.Expect (x => x.SetRole ("Role")).Return (_builderMock);
      _builderMock.Expect (x => x.SetExecution (AdviceExecution.Around)).Return (_builderMock);
      _builderMock.Expect (x => x.SetScope (AdviceScope.Instance)).Return (_builderMock);
      _builderMock.Expect (x => x.SetPriority (10)).Return (_builderMock);

      var result = _transform.GetAdviceBuilder (method, null);

      Assert.That (result, Is.SameAs (_builderMock));
      _builderMock.VerifyAllExpectations();
    }

    [Test]
    public void GetAdviceBuilder_Pointcuts ()
    {
      var method = NormalizingMemberInfoFromExpressionUtility.GetMethod ((DomainAspect obj) => obj.Method2());

      _adviceBuilderFactoryMock.Expect (x => x.Create ()).Return (_builderMock);
      _builderMock.Expect (x => x.SetMethod (method)).Return (_builderMock);
      _builderMock.Expect (x => x.AddPointcut (Arg<TypePointcut>.Is.TypeOf)).Return (_builderMock);
      _builderMock.Expect (x => x.AddPointcut (Arg<MemberNamePointcut>.Is.TypeOf)).Return (_builderMock);

      var result = _transform.GetAdviceBuilder (method, null);

      var args = _builderMock.GetArgumentsForCallsMadeOn (x => x.AddPointcut (null));
      var pointcuts = new[] { (IPointcut) args[0][0], (IPointcut) args[1][0] };
      Assert.That (pointcuts, Has.Some.TypeOf<TypePointcut>().With.Property ("Type").EqualTo (typeof (string)));
      Assert.That (pointcuts, Has.Some.TypeOf<MemberNamePointcut>().With.Property ("MemberName").EqualTo ("MemberName"));
      _builderMock.VerifyAllExpectations();
    }

    [Test]
    public void GetAdviceBuilder_SetConstructionIfType ()
    {
      var type = typeof (DomainAspect);

      _builderMock.Expect (x => x.SetConstruction (Arg<TypeConstruction>.Is.TypeOf)).Return (_builderMock);
      _builderMock.Expect (x => x.SetMethod (null)).Return (_builderMock);

      _adviceBuilderFactoryMock.Expect (x => x.Create ()).Return (_builderMock);
      var result = _transform.GetAdviceBuilder (type, null);

      var args = _builderMock.GetArgumentsForCallsMadeOn (x => x.SetConstruction (null));
      Assert.That (((IConstruction) args[0][0]).ConstructorInfo.DeclaringType, Is.EqualTo (typeof (DomainAspect)));
      _builderMock.VerifyAllExpectations();
    }

    [Test]
    public void GetAdviceBuilder_CopyParent ()
    {
      var type = typeof (DomainAspect);
      var parentAdviceBuilderMock = MockRepository.GenerateStrictMock<IAdviceBuilder>();
      var fakeAdviceBuilder = ObjectMother2.GetAdviceBuilder();

      parentAdviceBuilderMock.Expect (x => x.Copy()).Return (fakeAdviceBuilder);

      var result = _transform.GetAdviceBuilder (type, parentAdviceBuilderMock);

      Assert.That (result, Is.SameAs (fakeAdviceBuilder));
    }

    private class DomainAspect : IAspect
    {
      [AdviceExecution (AdviceExecution.Around)]
      [AdviceScope (AdviceScope.Instance)]
      [AdviceName ("Name")]
      [AdviceRole ("Role")]
      [AdvicePriority (10)]
      public void Method1 () {}

      [MemberNamePointcut ("MemberName")]
      [TypePointcut (typeof (string))]
      public void Method2 () {}
    }
  }
}
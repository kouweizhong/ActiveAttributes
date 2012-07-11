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
using System.Linq;
using ActiveAttributes.Core.Aspects;
using ActiveAttributes.Core.Assembly;
using ActiveAttributes.UnitTests.Assembly;
using FluentAssertions;
using NUnit.Framework;
using Remotion.Utilities;
using Xunit;
using Assert = NUnit.Framework.Assert;

//[assembly: AspectsProviderTest.AssemblyAttribute]

[assembly: AspectsProviderTest.DomainAspectAttribute (IfType = "*AspectsProviderTest+DomainType3")]
[assembly: AspectsProviderTest.DomainAspectAttribute (IfType = typeof (AspectsProviderTest.DomainType4))]
[assembly: AspectsProviderTest.DomainAspectAttribute (IfType = "ActiveAttributes.UnitTests.Assembly.AspectsProviderTest+NestedClass+*")]

[assembly: AspectsProviderTest.AssemblyLevelAspect (IfType = typeof(object))]

namespace ActiveAttributes.UnitTests.Assembly
{
  [TestFixture]
  public class AspectsProviderTest
  {
    private AspectsProvider _provider;

    public AspectsProviderTest ()
    {
      _provider = new AspectsProvider();
    }
    [SetUp]
    public void SetUp ()
    {
      _provider = new AspectsProvider();
    }

    [Test]
    public void GetAspects_OneElement ()
    {
      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.Method ()));

      var result = _provider.GetAspects (methodInfo).ToArray ();

      Assert.That (result.Length, Is.EqualTo (1));
      Assert.That (result, Has.Some.Property ("AspectType").EqualTo (typeof (DomainAspectAttribute)));
    }

    [Test]
    public void GetAspects_MultipleElement ()
    {
      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.OtherMethod ()));

      var result = _provider.GetAspects (methodInfo).ToArray();

      Assert.That (result.Length, Is.EqualTo (2));
      Assert.That (result, Has.Some.Property ("Priority").EqualTo (5));
      Assert.That (result, Has.Some.Property ("Priority").EqualTo (10));
    }

    [Test]
    public void GetAspects_Derived_Inheriting ()
    {
      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DerivedType obj) => obj.Method1 ()));

      var result = _provider.GetAspects (methodInfo).ToArray ();

      Assert.That (result, Has.Length.EqualTo (1));
    }

    [Test]
    public void GetAspects_Derived_NonInheriting ()
    {
      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DerivedType obj) => obj.Method2 ()));

      var result = _provider.GetAspects (methodInfo).ToArray ();

      Assert.That (result, Has.Length.EqualTo (0));
    }

    [Test]
    public void GetAspects_Derived_Inheriting_Property ()
    {
      var methodInfo = typeof (DerivedType).GetMethods ().Where (x => x.Name == "get_Property1").First ();

      var result = _provider.GetAspects (methodInfo).ToArray ();

      Assert.That (result, Has.Length.EqualTo (1));
    }


    [Test]
    public void GetAspects_Derived_NotInheriting_Property ()
    {
      var methodInfo = typeof (DerivedType).GetMethods ().Where (x => x.Name == "get_Property2").First ();

      var result = _provider.GetAspects (methodInfo).ToArray ();

      Assert.That (result, Has.Length.EqualTo (0));
    }

    [Test]
    public void GetAspects_Base_NonInheriting ()
    {
      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((BaseType obj) => obj.Method2 ()));

      var result = _provider.GetAspects (methodInfo).ToArray ();

      Assert.That (result, Has.Length.EqualTo (1));
    }

    [Test]
    public void GetAspects_OnlyAspectAttributes ()
    {
      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType obj) => obj.AnotherMethod()));

      var result = _provider.GetAspects (methodInfo);

      Assert.That (result, Is.Empty);
    }

    [Test]
    public void GetAspects_FromProperties ()
    {
      var methodInfo = typeof (DomainType).GetMethods().Where (x => x.Name == "get_Property").First();

      var result = _provider.GetAspects (methodInfo).ToArray();

      Assert.That (result, Has.Length.EqualTo (1));
    }


    [Test]
    public void GetAspects_IfSignature_Match ()
    {
      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType2 obj) => obj.Method1 ()));

      var result = _provider.GetAspects (methodInfo).ToArray ();

      Assert.That (result, Has.Length.EqualTo (1));
    }

    [Test]
    public void GetAspects_IfSignature_NoMatch ()
    {
      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType2 obj) => obj.SkipMethod ()));

      var result = _provider.GetAspects (methodInfo).ToArray ();

      Assert.That (result, Has.Length.EqualTo (0));
    }

    [Test]
    public void GetAspects_AssemblyLevel_StringType ()
    {
      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType3 obj) => obj.Method ()));

      var result = _provider.GetAspects (methodInfo).ToArray ();

      Assert.That (result, Has.Length.EqualTo (1));
    }

    [Test]
    public void GetAspects_AssemblyLevel_StrongType ()
    {
      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((DomainType4 obj) => obj.Method ()));

      var result = _provider.GetAspects (methodInfo).ToArray ();

      Assert.That (result, Has.Length.EqualTo (1));
    }

    [Test]
    public void GetAspects_AssemblyLevel_Namespace ()
    {
      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((NestedClass.DomainType5 obj) => obj.Method ()));

      var result = _provider.GetAspects (methodInfo).ToArray ();

      Assert.That (result, Has.Length.EqualTo (1));
    }

    [Test]
    public void GetAspects_AssemblyLevel_Namespace_EscapeDot ()
    {
      var methodInfo = MemberInfoFromExpressionUtility.GetMethod (((NestedClassX.DomainType5 obj) => obj.Method ()));

      var result = _provider.GetAspects (methodInfo).ToArray ();

      Assert.That (result, Has.Length.EqualTo (0));
    }


    public class AspectAttribute : Core.Aspects.AspectAttribute { }

    public class NonAspectAttribute : Attribute { }

    [AttributeUsage (AttributeTargets.All, Inherited = false)]
    public class NonInheritingAspectAttribute2 : Core.Aspects.AspectAttribute { }

    [AttributeUsage (AttributeTargets.All, Inherited = true)]
    public class InheritingAspectAttribute2 : Core.Aspects.AspectAttribute { }



    [Fact]
    public void GetTypeLevelAspects ()
    {
      var type = typeof (TypeLevelAspectClass);

      var result = _provider.GetTypeLevelAspects (type);

      result.Should().HaveCount (1);
    }

    [Aspect]
    public class TypeLevelAspectClass { }



    [Fact]
    public void GetTypeLevelAspectsSkipsNonAspectAttributes ()
    {
      var type = typeof (TypeLevelAspectClassWithNonAspectAttribute);

      var result = _provider.GetTypeLevelAspects (type);

      result.Should().HaveCount (1);
    }

    [Aspect]
    [NonAspect]
    public class TypeLevelAspectClassWithNonAspectAttribute { }



    [Fact]
    public void GetTypeLevelAspectsRespectsInheritingAspect ()
    {
      var type = typeof (TypeLevelAspectClassWithInheritingAspect);

      var result = _provider.GetTypeLevelAspects (type);

      result.Should().HaveCount (1);
    }

    [InheritingAspectAttribute2]
    public class TypeLevelAspectClassWithInheritingAspectBase { }
    public class TypeLevelAspectClassWithInheritingAspect : TypeLevelAspectClassWithInheritingAspectBase { }



    [Fact]
    public void GetTypeLevelAspectsRespectsNonInheritingAspect ()
    {
      var type = typeof (TypeLevelAspectClassWithoutAspectButBase);

      var result = _provider.GetTypeLevelAspects (type);

      result.Should ().HaveCount (0);
    }

    [NonInheritingAspectAttribute2]
    public class TypeLevelAspectClassWithNonInheritingAspectBase { }
    public class TypeLevelAspectClassWithoutAspectButBase : TypeLevelAspectClassWithNonInheritingAspectBase { }



    [Fact]
    public void GetTypeLevelAspectWithInheritingOnSelf ()
    {
      var type = typeof (TypeLevelAspectClassWithInheritingAspectOnSelf);

      var result = _provider.GetTypeLevelAspects (type);

      result.Should ().HaveCount (1);
    }

    [InheritingAspectAttribute2]
    public class TypeLevelAspectClassWithInheritingAspectOnSelf { }



    [Fact]
    public void GetAssemblyLevelAspect ()
    {
      var assembly = System.Reflection.Assembly.GetExecutingAssembly ();

      var result = _provider.GetAssemblyLevelAspects (assembly);

      result.Should ().Contain (x => typeof (AssemblyLevelAspect).IsAssignableFrom (x.AspectType));
    }

    public class AssemblyLevelAspect : AspectAttribute { }






    public class DomainType
    {
      [DomainAspect]
      public void Method () { }

      [DomainAspect (Priority = 5)]
      [DomainAspect (Priority = 10)]
      public void OtherMethod () { }

      [Dummy]
      public void AnotherMethod () { }

      [DomainAspect]
      public string Property { get; set; }
    }

    public class DomainAspectAttribute : Core.Aspects.AspectAttribute
    {
    }

    public class DummyAttribute : Attribute
    {
    }

    public class BaseType
    {
      [InheritingAspect]
      public virtual void Method1 () { }

      [NotInheritingAspect]
      public virtual void Method2 () { }

      [InheritingAspect]
      public virtual string Property1 { get; set; }

      [NotInheritingAspect]
      public virtual string Property2 { get; set; }
    }

    public class DerivedType : BaseType
    {
      public override void Method1 () { }

      public override void Method2 () { }

      public override string Property1 { get; set; }

      public override string Property2 { get; set; }
    }

    [AttributeUsage (AttributeTargets.All, Inherited = true)]
    public class InheritingAspectAttribute : Core.Aspects.AspectAttribute
    {
    }

    [AttributeUsage (AttributeTargets.All, Inherited = false)]
    public class NotInheritingAspectAttribute : Core.Aspects.AspectAttribute
    {
    }

    [DomainAspect (IfSignature = "void Method*(*)")]
    public class DomainType2
    {
      public virtual void Method1 () { }

      public virtual void SkipMethod () { }
    }

    public class DomainType3
    {
      public virtual void Method () { }
    }

    public class DomainType4
    {
      public virtual void Method () { }
    }

    public class NestedClass
    {
      public class DomainType5
      {
        public virtual object Method ()
        {
          return null;
        }
      }
    }

    public class NestedClassX
    {
      public class DomainType5
      {
        public virtual object Method ()
        {
          return null;
        }
      }
    }
  }
}

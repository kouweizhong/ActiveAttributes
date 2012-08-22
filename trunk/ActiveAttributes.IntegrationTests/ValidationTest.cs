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
using System.Text;
using ActiveAttributes.Common.Validation;
using ActiveAttributes.Core.Assembly;
using NUnit.Framework;

namespace ActiveAttributes.IntegrationTests
{
  [TestFixture]
  public class ValidationTest : TestBase
  {
    private DomainType _instance;

    [SetUp]
    public override void SetUp ()
    {
      base.SetUp();

      var type = AssembleType<DomainType> (Assembler.Singleton.ModifyType);
      _instance = (DomainType) Activator.CreateInstance (type);
    }

    [Test]
    public void ThrowsForNullArgument ()
    {
      Assert.That (() => _instance.Method1 (null), Throws.ArgumentException);
    }

    public class DomainType
    {
      public void Method1 ([NotNull] string arg)
      {
      }

      public string Method2 ()
      {
        return null;
      }
    } 
  }
}

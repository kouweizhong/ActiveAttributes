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
using System.Reflection;
using ActiveAttributes.Core.Assembly.FieldWrapper;
using Microsoft.Scripting.Ast;
using NUnit.Framework;
using Remotion.TypePipe.Expressions;
using Remotion.TypePipe.UnitTests.Expressions;

namespace ActiveAttributes.UnitTests.Assembly.FieldWrappers
{
  [TestFixture]
  public class StaticFieldWrapperTest
  {
    private FieldInfo _fieldInfo;
    private ThisExpression _thisExpression;

    [SetUp]
    public void SetUp ()
    {
      _fieldInfo = ObjectMother.GetFieldInfo (attributes: FieldAttributes.Static);
      _thisExpression = ObjectMother.GetThisExpression();
    }

    [Test]
    public void Initialization ()
    {
      var fieldWrapper = new StaticFieldWrapper (_fieldInfo);

      Assert.That (fieldWrapper.Field, Is.SameAs (_fieldInfo));
    }

    [Test]
    public void GetAccessExpression ()
    {
      var fieldWrapper = new StaticFieldWrapper (_fieldInfo);

      var expected = Expression.Field (null, _fieldInfo);
      var actual = fieldWrapper.GetAccessExpression (_thisExpression);

      ExpressionTreeComparer.CheckAreEqualTrees (expected, actual);
    }
  }
}
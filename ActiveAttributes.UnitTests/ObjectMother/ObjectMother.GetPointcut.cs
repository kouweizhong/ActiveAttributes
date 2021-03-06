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
using ActiveAttributes.Pointcuts;
using Remotion.Utilities;
using System.Linq;

namespace ActiveAttributes.UnitTests
{
  public static partial class ObjectMother
  {
    private static readonly IPointcut[] s_pointcuts
        = new IPointcut[]
          {
              new TypePointcut (typeof(string)),
              new VisibilityPointcut (Visibility.Family),
              new NamespacePointcut ("namespace")
          };

    public static IPointcut GetPointcut (Type pointcutType = null)
    {
      Assertion.IsTrue (pointcutType == null || s_pointcuts.Any (x => x.GetType() == pointcutType));

      return pointcutType == null ? GetRandom (s_pointcuts) : s_pointcuts.Single (x => x.GetType() == pointcutType);
    }
  }
}
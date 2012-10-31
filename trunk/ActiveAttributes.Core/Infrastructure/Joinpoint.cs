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
using Microsoft.Scripting.Ast;
using Remotion.Utilities;

namespace ActiveAttributes.Core.Infrastructure
{
  public interface IJoinPoint
  {
    System.Reflection.Assembly Assembly { get; }
    Type Type { get; }
    MemberInfo Member { get; }
    Expression Expression { get; }
  }

  public class JoinPoint : IJoinPoint
  {
    private readonly System.Reflection.Assembly _assembly;
    private readonly Type _type;
    private readonly MemberInfo _member;
    private readonly Expression _expression;

    public JoinPoint (System.Reflection.Assembly assembly)
    {
      _assembly = assembly;
    }

    public JoinPoint (Type type)
        : this (type.Assembly)
    {
      _type = type;
    }

    public JoinPoint (MemberInfo member)
        : this (member.DeclaringType)
    {
      _member = member;
    }

    public JoinPoint (MemberInfo member, Expression expression)
        : this (member)
    {
      _expression = expression;
    }

    public System.Reflection.Assembly Assembly
    {
      get { return _assembly; }
    }

    public Type Type
    {
      get { return _type; }
    }

    public MemberInfo Member
    {
      get { return _member; }
    }

    public Expression Expression
    {
      get { return _expression; }
    }
  }
}
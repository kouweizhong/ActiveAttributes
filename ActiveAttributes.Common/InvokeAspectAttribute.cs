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
using System.Windows.Forms;
using ActiveAttributes.Core.Aspects;
using ActiveAttributes.Core.Interception.Invocations;

namespace ActiveAttributes.Common
{
  /// <summary>
  /// Invokes a method call if required.
  /// </summary>
  public sealed class InvokeAspectAttribute : MethodInterceptionAspectAttribute
  {
    public override void OnIntercept (IInvocation invocation)
    {
      var control = (Control) invocation.Context.Instance;
      if (control.InvokeRequired)
        control.BeginInvoke (new Action (invocation.Proceed));
      else
        invocation.Proceed();
    }
  }
}
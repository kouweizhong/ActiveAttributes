//------------------------------------------------------------------------------
// This code was generated by a tool.
//
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated.
//
//------------------------------------------------------------------------------
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
using ActiveAttributes.Core.Interception.Contexts;
// ReSharper disable RedundantUsingDirective
using Remotion;
// ReSharper restore RedundantUsingDirective

namespace ActiveAttributes.Core.Interception.Invocations
{
  public class ActionInvocation<TInstance> : Invocation, IInvocationContext
  {
    private readonly ActionInvocationContext<TInstance> _context;
    private readonly Action _action;

    public ActionInvocation (ActionInvocationContext<TInstance> context, Action action)
    {
      _context = context;
      _action = action;
    }

    public override IInvocationContext Context
    {
      get { return _context; }
    }

    public override void Proceed ()
    {
      _action ();
    }

    public MethodInfo MethodInfo
    {
      get { return _context.MethodInfo; }
    }

    public object Instance
    {
      get { return _context.Instance; }
    }

    public IArgumentCollection Arguments
    {
      get { return _context; }
    }

    public object ReturnValue
    {
      get { throw new NotSupportedException (); }
      set { throw new NotSupportedException (); }
    }
  }
  public class ActionInvocation<TInstance, TA1, TA2> : Invocation, IInvocationContext
  {
    private readonly ActionInvocationContext<TInstance, TA1, TA2> _context;
    private readonly Action<TA1, TA2> _action;

    public ActionInvocation (ActionInvocationContext<TInstance, TA1, TA2> context, Action<TA1, TA2> action)
    {
      _context = context;
      _action = action;
    }

    public override IInvocationContext Context
    {
      get { return _context; }
    }

    public override void Proceed ()
    {
      _action (_context.Arg1, _context.Arg2);
    }

    public MethodInfo MethodInfo
    {
      get { return _context.MethodInfo; }
    }

    public object Instance
    {
      get { return _context.Instance; }
    }

    public IArgumentCollection Arguments
    {
      get { return _context; }
    }

    public object ReturnValue
    {
      get { throw new NotSupportedException (); }
      set { throw new NotSupportedException (); }
    }
  }
  public class ActionInvocation<TInstance, TA1, TA2, TA3> : Invocation, IInvocationContext
  {
    private readonly ActionInvocationContext<TInstance, TA1, TA2, TA3> _context;
    private readonly Action<TA1, TA2, TA3> _action;

    public ActionInvocation (ActionInvocationContext<TInstance, TA1, TA2, TA3> context, Action<TA1, TA2, TA3> action)
    {
      _context = context;
      _action = action;
    }

    public override IInvocationContext Context
    {
      get { return _context; }
    }

    public override void Proceed ()
    {
      _action (_context.Arg1, _context.Arg2, _context.Arg3);
    }

    public MethodInfo MethodInfo
    {
      get { return _context.MethodInfo; }
    }

    public object Instance
    {
      get { return _context.Instance; }
    }

    public IArgumentCollection Arguments
    {
      get { return _context; }
    }

    public object ReturnValue
    {
      get { throw new NotSupportedException (); }
      set { throw new NotSupportedException (); }
    }
  }
  public class ActionInvocation<TInstance, TA1, TA2, TA3, TA4> : Invocation, IInvocationContext
  {
    private readonly ActionInvocationContext<TInstance, TA1, TA2, TA3, TA4> _context;
    private readonly Action<TA1, TA2, TA3, TA4> _action;

    public ActionInvocation (ActionInvocationContext<TInstance, TA1, TA2, TA3, TA4> context, Action<TA1, TA2, TA3, TA4> action)
    {
      _context = context;
      _action = action;
    }

    public override IInvocationContext Context
    {
      get { return _context; }
    }

    public override void Proceed ()
    {
      _action (_context.Arg1, _context.Arg2, _context.Arg3, _context.Arg4);
    }

    public MethodInfo MethodInfo
    {
      get { return _context.MethodInfo; }
    }

    public object Instance
    {
      get { return _context.Instance; }
    }

    public IArgumentCollection Arguments
    {
      get { return _context; }
    }

    public object ReturnValue
    {
      get { throw new NotSupportedException (); }
      set { throw new NotSupportedException (); }
    }
  }
  public class ActionInvocation<TInstance, TA1, TA2, TA3, TA4, TA5> : Invocation, IInvocationContext
  {
    private readonly ActionInvocationContext<TInstance, TA1, TA2, TA3, TA4, TA5> _context;
    private readonly Action<TA1, TA2, TA3, TA4, TA5> _action;

    public ActionInvocation (ActionInvocationContext<TInstance, TA1, TA2, TA3, TA4, TA5> context, Action<TA1, TA2, TA3, TA4, TA5> action)
    {
      _context = context;
      _action = action;
    }

    public override IInvocationContext Context
    {
      get { return _context; }
    }

    public override void Proceed ()
    {
      _action (_context.Arg1, _context.Arg2, _context.Arg3, _context.Arg4, _context.Arg5);
    }

    public MethodInfo MethodInfo
    {
      get { return _context.MethodInfo; }
    }

    public object Instance
    {
      get { return _context.Instance; }
    }

    public IArgumentCollection Arguments
    {
      get { return _context; }
    }

    public object ReturnValue
    {
      get { throw new NotSupportedException (); }
      set { throw new NotSupportedException (); }
    }
  }
  public class ActionInvocation<TInstance, TA1, TA2, TA3, TA4, TA5, TA6> : Invocation, IInvocationContext
  {
    private readonly ActionInvocationContext<TInstance, TA1, TA2, TA3, TA4, TA5, TA6> _context;
    private readonly Action<TA1, TA2, TA3, TA4, TA5, TA6> _action;

    public ActionInvocation (ActionInvocationContext<TInstance, TA1, TA2, TA3, TA4, TA5, TA6> context, Action<TA1, TA2, TA3, TA4, TA5, TA6> action)
    {
      _context = context;
      _action = action;
    }

    public override IInvocationContext Context
    {
      get { return _context; }
    }

    public override void Proceed ()
    {
      _action (_context.Arg1, _context.Arg2, _context.Arg3, _context.Arg4, _context.Arg5, _context.Arg6);
    }

    public MethodInfo MethodInfo
    {
      get { return _context.MethodInfo; }
    }

    public object Instance
    {
      get { return _context.Instance; }
    }

    public IArgumentCollection Arguments
    {
      get { return _context; }
    }

    public object ReturnValue
    {
      get { throw new NotSupportedException (); }
      set { throw new NotSupportedException (); }
    }
  }
  public class ActionInvocation<TInstance, TA1, TA2, TA3, TA4, TA5, TA6, TA7> : Invocation, IInvocationContext
  {
    private readonly ActionInvocationContext<TInstance, TA1, TA2, TA3, TA4, TA5, TA6, TA7> _context;
    private readonly Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7> _action;

    public ActionInvocation (ActionInvocationContext<TInstance, TA1, TA2, TA3, TA4, TA5, TA6, TA7> context, Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7> action)
    {
      _context = context;
      _action = action;
    }

    public override IInvocationContext Context
    {
      get { return _context; }
    }

    public override void Proceed ()
    {
      _action (_context.Arg1, _context.Arg2, _context.Arg3, _context.Arg4, _context.Arg5, _context.Arg6, _context.Arg7);
    }

    public MethodInfo MethodInfo
    {
      get { return _context.MethodInfo; }
    }

    public object Instance
    {
      get { return _context.Instance; }
    }

    public IArgumentCollection Arguments
    {
      get { return _context; }
    }

    public object ReturnValue
    {
      get { throw new NotSupportedException (); }
      set { throw new NotSupportedException (); }
    }
  }
  public class ActionInvocation<TInstance, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8> : Invocation, IInvocationContext
  {
    private readonly ActionInvocationContext<TInstance, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8> _context;
    private readonly Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8> _action;

    public ActionInvocation (ActionInvocationContext<TInstance, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8> context, Action<TA1, TA2, TA3, TA4, TA5, TA6, TA7, TA8> action)
    {
      _context = context;
      _action = action;
    }

    public override IInvocationContext Context
    {
      get { return _context; }
    }

    public override void Proceed ()
    {
      _action (_context.Arg1, _context.Arg2, _context.Arg3, _context.Arg4, _context.Arg5, _context.Arg6, _context.Arg7, _context.Arg8);
    }

    public MethodInfo MethodInfo
    {
      get { return _context.MethodInfo; }
    }

    public object Instance
    {
      get { return _context.Instance; }
    }

    public IArgumentCollection Arguments
    {
      get { return _context; }
    }

    public object ReturnValue
    {
      get { throw new NotSupportedException (); }
      set { throw new NotSupportedException (); }
    }
  }
}

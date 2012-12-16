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
// ReSharper disable RedundantUsingDirective
using Remotion;
// ReSharper restore RedundantUsingDirective

namespace ActiveAttributes.Weaving.Context
{
  public class FuncContext<TInstance, TReturn> : FuncContextBase<TInstance, TReturn>
  {

    public FuncContext (MemberInfo memberInfo, TInstance instance)
        : base (memberInfo, instance)
    {
    }

    public override int Count
    {
      get { return 0; }
    }

    public override object this [int idx]
    {
      get
      {
        switch (idx)
        {
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
      set
      {
        switch (idx)
        {
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
    }
  }
  public class FuncContext<TInstance, TA0, TA1, TReturn> : FuncContextBase<TInstance, TReturn>
  {
    public TA0 Arg0;
    public TA1 Arg1;

    public FuncContext (MemberInfo memberInfo, TInstance instance, TA0 arg0, TA1 arg1)
        : base (memberInfo, instance)
    {
      Arg0 = arg0;
      Arg1 = arg1;
    }

    public override int Count
    {
      get { return 2; }
    }

    public override object this [int idx]
    {
      get
      {
        switch (idx)
        {
          case 0: return Arg0;
          case 1: return Arg1;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
      set
      {
        switch (idx)
        {
          case 0: Arg0 = (TA0) value; break;
          case 1: Arg1 = (TA1) value; break;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
    }
  }
  public class FuncContext<TInstance, TA0, TA1, TA2, TReturn> : FuncContextBase<TInstance, TReturn>
  {
    public TA0 Arg0;
    public TA1 Arg1;
    public TA2 Arg2;

    public FuncContext (MemberInfo memberInfo, TInstance instance, TA0 arg0, TA1 arg1, TA2 arg2)
        : base (memberInfo, instance)
    {
      Arg0 = arg0;
      Arg1 = arg1;
      Arg2 = arg2;
    }

    public override int Count
    {
      get { return 3; }
    }

    public override object this [int idx]
    {
      get
      {
        switch (idx)
        {
          case 0: return Arg0;
          case 1: return Arg1;
          case 2: return Arg2;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
      set
      {
        switch (idx)
        {
          case 0: Arg0 = (TA0) value; break;
          case 1: Arg1 = (TA1) value; break;
          case 2: Arg2 = (TA2) value; break;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
    }
  }
  public class FuncContext<TInstance, TA0, TA1, TA2, TA3, TReturn> : FuncContextBase<TInstance, TReturn>
  {
    public TA0 Arg0;
    public TA1 Arg1;
    public TA2 Arg2;
    public TA3 Arg3;

    public FuncContext (MemberInfo memberInfo, TInstance instance, TA0 arg0, TA1 arg1, TA2 arg2, TA3 arg3)
        : base (memberInfo, instance)
    {
      Arg0 = arg0;
      Arg1 = arg1;
      Arg2 = arg2;
      Arg3 = arg3;
    }

    public override int Count
    {
      get { return 4; }
    }

    public override object this [int idx]
    {
      get
      {
        switch (idx)
        {
          case 0: return Arg0;
          case 1: return Arg1;
          case 2: return Arg2;
          case 3: return Arg3;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
      set
      {
        switch (idx)
        {
          case 0: Arg0 = (TA0) value; break;
          case 1: Arg1 = (TA1) value; break;
          case 2: Arg2 = (TA2) value; break;
          case 3: Arg3 = (TA3) value; break;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
    }
  }
  public class FuncContext<TInstance, TA0, TA1, TA2, TA3, TA4, TReturn> : FuncContextBase<TInstance, TReturn>
  {
    public TA0 Arg0;
    public TA1 Arg1;
    public TA2 Arg2;
    public TA3 Arg3;
    public TA4 Arg4;

    public FuncContext (MemberInfo memberInfo, TInstance instance, TA0 arg0, TA1 arg1, TA2 arg2, TA3 arg3, TA4 arg4)
        : base (memberInfo, instance)
    {
      Arg0 = arg0;
      Arg1 = arg1;
      Arg2 = arg2;
      Arg3 = arg3;
      Arg4 = arg4;
    }

    public override int Count
    {
      get { return 5; }
    }

    public override object this [int idx]
    {
      get
      {
        switch (idx)
        {
          case 0: return Arg0;
          case 1: return Arg1;
          case 2: return Arg2;
          case 3: return Arg3;
          case 4: return Arg4;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
      set
      {
        switch (idx)
        {
          case 0: Arg0 = (TA0) value; break;
          case 1: Arg1 = (TA1) value; break;
          case 2: Arg2 = (TA2) value; break;
          case 3: Arg3 = (TA3) value; break;
          case 4: Arg4 = (TA4) value; break;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
    }
  }
  public class FuncContext<TInstance, TA0, TA1, TA2, TA3, TA4, TA5, TReturn> : FuncContextBase<TInstance, TReturn>
  {
    public TA0 Arg0;
    public TA1 Arg1;
    public TA2 Arg2;
    public TA3 Arg3;
    public TA4 Arg4;
    public TA5 Arg5;

    public FuncContext (MemberInfo memberInfo, TInstance instance, TA0 arg0, TA1 arg1, TA2 arg2, TA3 arg3, TA4 arg4, TA5 arg5)
        : base (memberInfo, instance)
    {
      Arg0 = arg0;
      Arg1 = arg1;
      Arg2 = arg2;
      Arg3 = arg3;
      Arg4 = arg4;
      Arg5 = arg5;
    }

    public override int Count
    {
      get { return 6; }
    }

    public override object this [int idx]
    {
      get
      {
        switch (idx)
        {
          case 0: return Arg0;
          case 1: return Arg1;
          case 2: return Arg2;
          case 3: return Arg3;
          case 4: return Arg4;
          case 5: return Arg5;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
      set
      {
        switch (idx)
        {
          case 0: Arg0 = (TA0) value; break;
          case 1: Arg1 = (TA1) value; break;
          case 2: Arg2 = (TA2) value; break;
          case 3: Arg3 = (TA3) value; break;
          case 4: Arg4 = (TA4) value; break;
          case 5: Arg5 = (TA5) value; break;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
    }
  }
  public class FuncContext<TInstance, TA0, TA1, TA2, TA3, TA4, TA5, TA6, TReturn> : FuncContextBase<TInstance, TReturn>
  {
    public TA0 Arg0;
    public TA1 Arg1;
    public TA2 Arg2;
    public TA3 Arg3;
    public TA4 Arg4;
    public TA5 Arg5;
    public TA6 Arg6;

    public FuncContext (MemberInfo memberInfo, TInstance instance, TA0 arg0, TA1 arg1, TA2 arg2, TA3 arg3, TA4 arg4, TA5 arg5, TA6 arg6)
        : base (memberInfo, instance)
    {
      Arg0 = arg0;
      Arg1 = arg1;
      Arg2 = arg2;
      Arg3 = arg3;
      Arg4 = arg4;
      Arg5 = arg5;
      Arg6 = arg6;
    }

    public override int Count
    {
      get { return 7; }
    }

    public override object this [int idx]
    {
      get
      {
        switch (idx)
        {
          case 0: return Arg0;
          case 1: return Arg1;
          case 2: return Arg2;
          case 3: return Arg3;
          case 4: return Arg4;
          case 5: return Arg5;
          case 6: return Arg6;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
      set
      {
        switch (idx)
        {
          case 0: Arg0 = (TA0) value; break;
          case 1: Arg1 = (TA1) value; break;
          case 2: Arg2 = (TA2) value; break;
          case 3: Arg3 = (TA3) value; break;
          case 4: Arg4 = (TA4) value; break;
          case 5: Arg5 = (TA5) value; break;
          case 6: Arg6 = (TA6) value; break;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
    }
  }
  public class FuncContext<TInstance, TA0, TA1, TA2, TA3, TA4, TA5, TA6, TA7, TReturn> : FuncContextBase<TInstance, TReturn>
  {
    public TA0 Arg0;
    public TA1 Arg1;
    public TA2 Arg2;
    public TA3 Arg3;
    public TA4 Arg4;
    public TA5 Arg5;
    public TA6 Arg6;
    public TA7 Arg7;

    public FuncContext (MemberInfo memberInfo, TInstance instance, TA0 arg0, TA1 arg1, TA2 arg2, TA3 arg3, TA4 arg4, TA5 arg5, TA6 arg6, TA7 arg7)
        : base (memberInfo, instance)
    {
      Arg0 = arg0;
      Arg1 = arg1;
      Arg2 = arg2;
      Arg3 = arg3;
      Arg4 = arg4;
      Arg5 = arg5;
      Arg6 = arg6;
      Arg7 = arg7;
    }

    public override int Count
    {
      get { return 8; }
    }

    public override object this [int idx]
    {
      get
      {
        switch (idx)
        {
          case 0: return Arg0;
          case 1: return Arg1;
          case 2: return Arg2;
          case 3: return Arg3;
          case 4: return Arg4;
          case 5: return Arg5;
          case 6: return Arg6;
          case 7: return Arg7;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
      set
      {
        switch (idx)
        {
          case 0: Arg0 = (TA0) value; break;
          case 1: Arg1 = (TA1) value; break;
          case 2: Arg2 = (TA2) value; break;
          case 3: Arg3 = (TA3) value; break;
          case 4: Arg4 = (TA4) value; break;
          case 5: Arg5 = (TA5) value; break;
          case 6: Arg6 = (TA6) value; break;
          case 7: Arg7 = (TA7) value; break;
          default: throw new IndexOutOfRangeException ("idx");
        }
      }
    }
  }
}

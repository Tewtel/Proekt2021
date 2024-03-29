﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Design.ForwardingProxy`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace System.Data.Entity.Infrastructure.Design
{
  internal class ForwardingProxy<T> : RealProxy
  {
    private readonly MarshalByRefObject _target;

    public ForwardingProxy(object target)
      : base(typeof (T))
    {
      this._target = (MarshalByRefObject) target;
    }

    public override IMessage Invoke(IMessage msg)
    {
      new MethodCallMessageWrapper((IMethodCallMessage) msg).Uri = RemotingServices.GetObjectUri(this._target);
      return RemotingServices.GetEnvoyChainForProxy(this._target).SyncProcessMessage(msg);
    }

    public T GetTransparentProxy() => (T) base.GetTransparentProxy();
  }
}

// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ConfigFile.InterceptorsCollection
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;

namespace System.Data.Entity.Internal.ConfigFile
{
  internal class InterceptorsCollection : ConfigurationElementCollection
  {
    private const string ElementKey = "interceptor";
    private int _nextKey;

    protected override ConfigurationElement CreateNewElement() => (ConfigurationElement) new InterceptorElement(this._nextKey++);

    protected override object GetElementKey(ConfigurationElement element) => (object) ((InterceptorElement) element).Key;

    public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

    protected override string ElementName => "interceptor";

    public void AddElement(InterceptorElement element) => this.BaseAdd((ConfigurationElement) element);

    public virtual IEnumerable<IDbInterceptor> Interceptors => (IEnumerable<IDbInterceptor>) this.OfType<InterceptorElement>().Select<InterceptorElement, IDbInterceptor>((Func<InterceptorElement, IDbInterceptor>) (e => e.CreateInterceptor())).ToList<IDbInterceptor>();
  }
}

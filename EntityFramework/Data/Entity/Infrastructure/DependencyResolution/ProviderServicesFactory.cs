// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.ProviderServicesFactory
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;
using System.Reflection;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class ProviderServicesFactory
  {
    public virtual DbProviderServices TryGetInstance(string providerTypeName)
    {
      Type type = Type.GetType(providerTypeName, false);
      return !(type == (Type) null) ? ProviderServicesFactory.GetInstance(type) : (DbProviderServices) null;
    }

    public virtual DbProviderServices GetInstance(
      string providerTypeName,
      string providerInvariantName)
    {
      Type type = Type.GetType(providerTypeName, false);
      return !(type == (Type) null) ? ProviderServicesFactory.GetInstance(type) : throw new InvalidOperationException(Strings.EF6Providers_ProviderTypeMissing((object) providerTypeName, (object) providerInvariantName));
    }

    private static DbProviderServices GetInstance(Type providerType)
    {
      MemberInfo memberInfo = (MemberInfo) providerType.GetStaticProperty("Instance");
      if ((object) memberInfo == null)
        memberInfo = (MemberInfo) providerType.GetField("Instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      if (memberInfo == (MemberInfo) null)
        throw new InvalidOperationException(Strings.EF6Providers_InstanceMissing((object) providerType.AssemblyQualifiedName));
      return memberInfo.GetValue() is DbProviderServices providerServices ? providerServices : throw new InvalidOperationException(Strings.EF6Providers_NotDbProviderServices((object) providerType.AssemblyQualifiedName));
    }
  }
}

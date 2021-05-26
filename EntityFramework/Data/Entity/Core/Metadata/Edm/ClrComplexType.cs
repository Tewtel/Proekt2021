// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ClrComplexType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Utilities;
using System.Threading;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal sealed class ClrComplexType : ComplexType
  {
    private readonly Type _type;
    private Func<object> _constructor;
    private readonly string _cspaceTypeName;

    internal ClrComplexType(Type clrType, string cspaceNamespaceName, string cspaceTypeName)
      : base(System.Data.Entity.Utilities.Check.NotNull<Type>(clrType, nameof (clrType)).Name, clrType.NestingNamespace() ?? string.Empty, DataSpace.OSpace)
    {
      this._type = clrType;
      this._cspaceTypeName = cspaceNamespaceName + "." + cspaceTypeName;
      this.Abstract = clrType.IsAbstract();
    }

    internal static ClrComplexType CreateReadonlyClrComplexType(
      Type clrType,
      string cspaceNamespaceName,
      string cspaceTypeName)
    {
      ClrComplexType clrComplexType = new ClrComplexType(clrType, cspaceNamespaceName, cspaceTypeName);
      clrComplexType.SetReadOnly();
      return clrComplexType;
    }

    internal Func<object> Constructor
    {
      get => this._constructor;
      set => Interlocked.CompareExchange<Func<object>>(ref this._constructor, value, (Func<object>) null);
    }

    internal override Type ClrType => this._type;

    internal string CSpaceTypeName => this._cspaceTypeName;
  }
}

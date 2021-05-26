// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ClrEnumType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal sealed class ClrEnumType : EnumType
  {
    private readonly Type _type;
    private readonly string _cspaceTypeName;

    internal ClrEnumType(Type clrType, string cspaceNamespaceName, string cspaceTypeName)
      : base(clrType)
    {
      this._type = clrType;
      this._cspaceTypeName = cspaceNamespaceName + "." + cspaceTypeName;
    }

    internal override Type ClrType => this._type;

    internal string CSpaceTypeName => this._cspaceTypeName;
  }
}

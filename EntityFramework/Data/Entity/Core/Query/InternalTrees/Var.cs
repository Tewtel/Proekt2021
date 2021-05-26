// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.Var
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal abstract class Var
  {
    private readonly int _id;
    private readonly VarType _varType;
    private readonly TypeUsage _type;

    internal Var(int id, VarType varType, TypeUsage type)
    {
      this._id = id;
      this._varType = varType;
      this._type = type;
    }

    internal int Id => this._id;

    internal VarType VarType => this._varType;

    internal TypeUsage Type => this._type;

    internal virtual bool TryGetName(out string name)
    {
      name = (string) null;
      return false;
    }

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}", (object) this.Id);
  }
}

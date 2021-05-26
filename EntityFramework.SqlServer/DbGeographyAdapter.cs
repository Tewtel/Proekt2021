// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.DbGeographyAdapter
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Data.Entity.Core;
using System.Data.Entity.Spatial;
using System.Data.Entity.SqlServer.Resources;
using System.Data.Entity.SqlServer.Utilities;

namespace System.Data.Entity.SqlServer
{
  internal class DbGeographyAdapter : IDbSpatialValue
  {
    private readonly DbGeography _value;

    internal DbGeographyAdapter(DbGeography value) => this._value = value;

    public bool IsGeography => true;

    public object ProviderValue => ((Func<object>) (() => this._value.ProviderValue)).NullIfNotImplemented<object>();

    public int? CoordinateSystemId => ((Func<int?>) (() => new int?(this._value.CoordinateSystemId))).NullIfNotImplemented<int?>();

    public string WellKnownText => ((Func<string>) (() => this._value.Provider.AsTextIncludingElevationAndMeasure(this._value))).NullIfNotImplemented<string>() ?? ((Func<string>) (() => this._value.AsText())).NullIfNotImplemented<string>();

    public byte[] WellKnownBinary => ((Func<byte[]>) (() => this._value.AsBinary())).NullIfNotImplemented<byte[]>();

    public string GmlString => ((Func<string>) (() => this._value.AsGml())).NullIfNotImplemented<string>();

    public Exception NotSqlCompatible() => (Exception) new ProviderIncompatibleException(Strings.SqlProvider_GeographyValueNotSqlCompatible);
  }
}

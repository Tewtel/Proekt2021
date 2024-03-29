﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Spatial.DefaultSpatialServices
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;

namespace System.Data.Entity.Spatial
{
  [Serializable]
  internal sealed class DefaultSpatialServices : DbSpatialServices
  {
    internal static readonly DefaultSpatialServices Instance = new DefaultSpatialServices();

    private DefaultSpatialServices()
    {
    }

    private static Exception SpatialServicesUnavailable() => (Exception) new NotImplementedException(Strings.SpatialProviderNotUsable);

    private static DefaultSpatialServices.ReadOnlySpatialValues CheckProviderValue(
      object providerValue)
    {
      return providerValue is DefaultSpatialServices.ReadOnlySpatialValues onlySpatialValues ? onlySpatialValues : throw new ArgumentException(Strings.Spatial_ProviderValueNotCompatibleWithSpatialServices, nameof (providerValue));
    }

    private static DefaultSpatialServices.ReadOnlySpatialValues CheckCompatible(
      DbGeography geographyValue)
    {
      if (geographyValue != null && geographyValue.ProviderValue is DefaultSpatialServices.ReadOnlySpatialValues providerValue)
        return providerValue;
      throw new ArgumentException(Strings.Spatial_GeographyValueNotCompatibleWithSpatialServices, nameof (geographyValue));
    }

    private static DefaultSpatialServices.ReadOnlySpatialValues CheckCompatible(
      DbGeometry geometryValue)
    {
      if (geometryValue != null && geometryValue.ProviderValue is DefaultSpatialServices.ReadOnlySpatialValues providerValue)
        return providerValue;
      throw new ArgumentException(Strings.Spatial_GeometryValueNotCompatibleWithSpatialServices, nameof (geometryValue));
    }

    public override DbGeography GeographyFromProviderValue(object providerValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<object>(providerValue, nameof (providerValue));
      return DbSpatialServices.CreateGeography((DbSpatialServices) this, (object) DefaultSpatialServices.CheckProviderValue(providerValue));
    }

    public override object CreateProviderValue(DbGeographyWellKnownValue wellKnownValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGeographyWellKnownValue>(wellKnownValue, nameof (wellKnownValue));
      return (object) new DefaultSpatialServices.ReadOnlySpatialValues(wellKnownValue.CoordinateSystemId, wellKnownValue.WellKnownText, wellKnownValue.WellKnownBinary, (string) null);
    }

    public override DbGeographyWellKnownValue CreateWellKnownValue(
      DbGeography geographyValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGeography>(geographyValue, nameof (geographyValue));
      DefaultSpatialServices.ReadOnlySpatialValues onlySpatialValues = DefaultSpatialServices.CheckCompatible(geographyValue);
      return new DbGeographyWellKnownValue()
      {
        CoordinateSystemId = onlySpatialValues.CoordinateSystemId,
        WellKnownBinary = onlySpatialValues.CloneBinary(),
        WellKnownText = onlySpatialValues.Text
      };
    }

    public override DbGeography GeographyFromBinary(byte[] geographyBinary)
    {
      System.Data.Entity.Utilities.Check.NotNull<byte[]>(geographyBinary, nameof (geographyBinary));
      return DbSpatialServices.CreateGeography((DbSpatialServices) this, (object) new DefaultSpatialServices.ReadOnlySpatialValues(DbGeography.DefaultCoordinateSystemId, (string) null, geographyBinary, (string) null));
    }

    public override DbGeography GeographyFromBinary(
      byte[] geographyBinary,
      int spatialReferenceSystemId)
    {
      System.Data.Entity.Utilities.Check.NotNull<byte[]>(geographyBinary, nameof (geographyBinary));
      return DbSpatialServices.CreateGeography((DbSpatialServices) this, (object) new DefaultSpatialServices.ReadOnlySpatialValues(spatialReferenceSystemId, (string) null, geographyBinary, (string) null));
    }

    public override DbGeography GeographyLineFromBinary(
      byte[] geographyBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyPointFromBinary(
      byte[] geographyBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyPolygonFromBinary(
      byte[] geographyBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyMultiLineFromBinary(
      byte[] geographyBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyMultiPointFromBinary(
      byte[] geographyBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyMultiPolygonFromBinary(
      byte[] geographyBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyCollectionFromBinary(
      byte[] geographyBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyFromText(string geographyText)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(geographyText, nameof (geographyText));
      return DbSpatialServices.CreateGeography((DbSpatialServices) this, (object) new DefaultSpatialServices.ReadOnlySpatialValues(DbGeography.DefaultCoordinateSystemId, geographyText, (byte[]) null, (string) null));
    }

    public override DbGeography GeographyFromText(
      string geographyText,
      int spatialReferenceSystemId)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(geographyText, nameof (geographyText));
      return DbSpatialServices.CreateGeography((DbSpatialServices) this, (object) new DefaultSpatialServices.ReadOnlySpatialValues(spatialReferenceSystemId, geographyText, (byte[]) null, (string) null));
    }

    public override DbGeography GeographyLineFromText(
      string geographyText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyPointFromText(
      string geographyText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyPolygonFromText(
      string geographyText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyMultiLineFromText(
      string geographyText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyMultiPointFromText(
      string geographyText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyMultiPolygonFromText(
      string multiPolygonKnownText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyCollectionFromText(
      string geographyText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography GeographyFromGml(string geographyMarkup)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(geographyMarkup, nameof (geographyMarkup));
      return DbSpatialServices.CreateGeography((DbSpatialServices) this, (object) new DefaultSpatialServices.ReadOnlySpatialValues(DbGeography.DefaultCoordinateSystemId, (string) null, (byte[]) null, geographyMarkup));
    }

    public override DbGeography GeographyFromGml(
      string geographyMarkup,
      int spatialReferenceSystemId)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(geographyMarkup, nameof (geographyMarkup));
      return DbSpatialServices.CreateGeography((DbSpatialServices) this, (object) new DefaultSpatialServices.ReadOnlySpatialValues(spatialReferenceSystemId, (string) null, (byte[]) null, geographyMarkup));
    }

    public override int GetCoordinateSystemId(DbGeography geographyValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGeography>(geographyValue, nameof (geographyValue));
      return DefaultSpatialServices.CheckCompatible(geographyValue).CoordinateSystemId;
    }

    public override int GetDimension(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override string GetSpatialTypeName(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool GetIsEmpty(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override string AsText(DbGeography geographyValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGeography>(geographyValue, nameof (geographyValue));
      return DefaultSpatialServices.CheckCompatible(geographyValue).Text;
    }

    public override byte[] AsBinary(DbGeography geographyValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGeography>(geographyValue, nameof (geographyValue));
      return DefaultSpatialServices.CheckCompatible(geographyValue).CloneBinary();
    }

    public override string AsGml(DbGeography geographyValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGeography>(geographyValue, nameof (geographyValue));
      return DefaultSpatialServices.CheckCompatible(geographyValue).GML;
    }

    public override bool SpatialEquals(DbGeography geographyValue, DbGeography otherGeography) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool Disjoint(DbGeography geographyValue, DbGeography otherGeography) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool Intersects(DbGeography geographyValue, DbGeography otherGeography) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeography Buffer(DbGeography geographyValue, double distance) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double Distance(DbGeography geographyValue, DbGeography otherGeography) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeography Intersection(
      DbGeography geographyValue,
      DbGeography otherGeography)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography Union(
      DbGeography geographyValue,
      DbGeography otherGeography)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography Difference(
      DbGeography geographyValue,
      DbGeography otherGeography)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeography SymmetricDifference(
      DbGeography geographyValue,
      DbGeography otherGeography)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override int? GetElementCount(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeography ElementAt(DbGeography geographyValue, int index) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double? GetLatitude(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double? GetLongitude(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double? GetElevation(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double? GetMeasure(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double? GetLength(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeography GetEndPoint(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeography GetStartPoint(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool? GetIsClosed(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override int? GetPointCount(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeography PointAt(DbGeography geographyValue, int index) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double? GetArea(DbGeography geographyValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override object CreateProviderValue(DbGeometryWellKnownValue wellKnownValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGeometryWellKnownValue>(wellKnownValue, nameof (wellKnownValue));
      return (object) new DefaultSpatialServices.ReadOnlySpatialValues(wellKnownValue.CoordinateSystemId, wellKnownValue.WellKnownText, wellKnownValue.WellKnownBinary, (string) null);
    }

    public override DbGeometryWellKnownValue CreateWellKnownValue(
      DbGeometry geometryValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGeometry>(geometryValue, nameof (geometryValue));
      DefaultSpatialServices.ReadOnlySpatialValues onlySpatialValues = DefaultSpatialServices.CheckCompatible(geometryValue);
      return new DbGeometryWellKnownValue()
      {
        CoordinateSystemId = onlySpatialValues.CoordinateSystemId,
        WellKnownBinary = onlySpatialValues.CloneBinary(),
        WellKnownText = onlySpatialValues.Text
      };
    }

    public override DbGeometry GeometryFromProviderValue(object providerValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<object>(providerValue, nameof (providerValue));
      return DbSpatialServices.CreateGeometry((DbSpatialServices) this, (object) DefaultSpatialServices.CheckProviderValue(providerValue));
    }

    public override DbGeometry GeometryFromBinary(byte[] geometryBinary)
    {
      System.Data.Entity.Utilities.Check.NotNull<byte[]>(geometryBinary, nameof (geometryBinary));
      return DbSpatialServices.CreateGeometry((DbSpatialServices) this, (object) new DefaultSpatialServices.ReadOnlySpatialValues(DbGeometry.DefaultCoordinateSystemId, (string) null, geometryBinary, (string) null));
    }

    public override DbGeometry GeometryFromBinary(
      byte[] geometryBinary,
      int spatialReferenceSystemId)
    {
      System.Data.Entity.Utilities.Check.NotNull<byte[]>(geometryBinary, nameof (geometryBinary));
      return DbSpatialServices.CreateGeometry((DbSpatialServices) this, (object) new DefaultSpatialServices.ReadOnlySpatialValues(spatialReferenceSystemId, (string) null, geometryBinary, (string) null));
    }

    public override DbGeometry GeometryLineFromBinary(
      byte[] geometryBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryPointFromBinary(
      byte[] geometryBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryPolygonFromBinary(
      byte[] geometryBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryMultiLineFromBinary(
      byte[] geometryBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryMultiPointFromBinary(
      byte[] geometryBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryMultiPolygonFromBinary(
      byte[] geometryBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryCollectionFromBinary(
      byte[] geometryBinary,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryFromText(string geometryText)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(geometryText, nameof (geometryText));
      return DbSpatialServices.CreateGeometry((DbSpatialServices) this, (object) new DefaultSpatialServices.ReadOnlySpatialValues(DbGeometry.DefaultCoordinateSystemId, geometryText, (byte[]) null, (string) null));
    }

    public override DbGeometry GeometryFromText(
      string geometryText,
      int spatialReferenceSystemId)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(geometryText, nameof (geometryText));
      return DbSpatialServices.CreateGeometry((DbSpatialServices) this, (object) new DefaultSpatialServices.ReadOnlySpatialValues(spatialReferenceSystemId, geometryText, (byte[]) null, (string) null));
    }

    public override DbGeometry GeometryLineFromText(
      string geometryText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryPointFromText(
      string geometryText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryPolygonFromText(
      string geometryText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryMultiLineFromText(
      string geometryText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryMultiPointFromText(
      string geometryText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryMultiPolygonFromText(
      string geometryText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryCollectionFromText(
      string geometryText,
      int spatialReferenceSystemId)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry GeometryFromGml(string geometryMarkup)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(geometryMarkup, nameof (geometryMarkup));
      return DbSpatialServices.CreateGeometry((DbSpatialServices) this, (object) new DefaultSpatialServices.ReadOnlySpatialValues(DbGeometry.DefaultCoordinateSystemId, (string) null, (byte[]) null, geometryMarkup));
    }

    public override DbGeometry GeometryFromGml(
      string geometryMarkup,
      int spatialReferenceSystemId)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(geometryMarkup, nameof (geometryMarkup));
      return DbSpatialServices.CreateGeometry((DbSpatialServices) this, (object) new DefaultSpatialServices.ReadOnlySpatialValues(spatialReferenceSystemId, (string) null, (byte[]) null, geometryMarkup));
    }

    public override int GetCoordinateSystemId(DbGeometry geometryValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGeometry>(geometryValue, nameof (geometryValue));
      return DefaultSpatialServices.CheckCompatible(geometryValue).CoordinateSystemId;
    }

    public override DbGeometry GetBoundary(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override int GetDimension(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeometry GetEnvelope(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override string GetSpatialTypeName(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool GetIsEmpty(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool GetIsSimple(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool GetIsValid(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override string AsText(DbGeometry geometryValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGeometry>(geometryValue, nameof (geometryValue));
      return DefaultSpatialServices.CheckCompatible(geometryValue).Text;
    }

    public override byte[] AsBinary(DbGeometry geometryValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGeometry>(geometryValue, nameof (geometryValue));
      return DefaultSpatialServices.CheckCompatible(geometryValue).CloneBinary();
    }

    public override string AsGml(DbGeometry geometryValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbGeometry>(geometryValue, nameof (geometryValue));
      return DefaultSpatialServices.CheckCompatible(geometryValue).GML;
    }

    public override bool SpatialEquals(DbGeometry geometryValue, DbGeometry otherGeometry) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool Disjoint(DbGeometry geometryValue, DbGeometry otherGeometry) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool Intersects(DbGeometry geometryValue, DbGeometry otherGeometry) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool Touches(DbGeometry geometryValue, DbGeometry otherGeometry) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool Crosses(DbGeometry geometryValue, DbGeometry otherGeometry) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool Within(DbGeometry geometryValue, DbGeometry otherGeometry) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool Contains(DbGeometry geometryValue, DbGeometry otherGeometry) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool Overlaps(DbGeometry geometryValue, DbGeometry otherGeometry) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool Relate(DbGeometry geometryValue, DbGeometry otherGeometry, string matrix) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeometry Buffer(DbGeometry geometryValue, double distance) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double Distance(DbGeometry geometryValue, DbGeometry otherGeometry) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeometry GetConvexHull(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeometry Intersection(
      DbGeometry geometryValue,
      DbGeometry otherGeometry)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry Union(DbGeometry geometryValue, DbGeometry otherGeometry) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeometry Difference(
      DbGeometry geometryValue,
      DbGeometry otherGeometry)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override DbGeometry SymmetricDifference(
      DbGeometry geometryValue,
      DbGeometry otherGeometry)
    {
      throw DefaultSpatialServices.SpatialServicesUnavailable();
    }

    public override int? GetElementCount(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeometry ElementAt(DbGeometry geometryValue, int index) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double? GetXCoordinate(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double? GetYCoordinate(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double? GetElevation(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double? GetMeasure(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double? GetLength(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeometry GetEndPoint(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeometry GetStartPoint(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool? GetIsClosed(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override bool? GetIsRing(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override int? GetPointCount(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeometry PointAt(DbGeometry geometryValue, int index) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override double? GetArea(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeometry GetCentroid(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeometry GetPointOnSurface(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeometry GetExteriorRing(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override int? GetInteriorRingCount(DbGeometry geometryValue) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    public override DbGeometry InteriorRingAt(DbGeometry geometryValue, int index) => throw DefaultSpatialServices.SpatialServicesUnavailable();

    [Serializable]
    private sealed class ReadOnlySpatialValues
    {
      private readonly int srid;
      private readonly byte[] wkb;
      private readonly string wkt;
      private readonly string gml;

      internal ReadOnlySpatialValues(
        int spatialRefSysId,
        string textValue,
        byte[] binaryValue,
        string gmlValue)
      {
        this.srid = spatialRefSysId;
        this.wkb = binaryValue == null ? (byte[]) null : (byte[]) binaryValue.Clone();
        this.wkt = textValue;
        this.gml = gmlValue;
      }

      internal int CoordinateSystemId => this.srid;

      internal byte[] CloneBinary() => this.wkb != null ? (byte[]) this.wkb.Clone() : (byte[]) null;

      internal string Text => this.wkt;

      internal string GML => this.gml;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlTypesAssembly
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Data.Entity.Hierarchy;
using System.Data.Entity.Spatial;
using System.Data.Entity.SqlServer.Utilities;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace System.Data.Entity.SqlServer
{
  internal class SqlTypesAssembly
  {
    private readonly Func<object, bool> sqlBooleanToBoolean;
    private readonly Func<object, bool?> sqlBooleanToNullableBoolean;
    private readonly Func<byte[], object> sqlBytesFromByteArray;
    private readonly Func<object, byte[]> sqlBytesToByteArray;
    private readonly Func<string, object> sqlStringFromString;
    private readonly Func<string, object> sqlCharsFromString;
    private readonly Func<object, string> sqlCharsToString;
    private readonly Func<object, string> sqlStringToString;
    private readonly Func<object, double> sqlDoubleToDouble;
    private readonly Func<object, double?> sqlDoubleToNullableDouble;
    private readonly Func<object, int> sqlInt32ToInt;
    private readonly Func<object, int?> sqlInt32ToNullableInt;
    private readonly Func<XmlReader, object> sqlXmlFromXmlReader;
    private readonly Func<object, string> sqlXmlToString;
    private readonly Func<object, bool> isSqlGeographyNull;
    private readonly Func<object, bool> isSqlGeometryNull;
    private readonly Func<object, object> geographyAsTextZMAsSqlChars;
    private readonly Func<object, object> geometryAsTextZMAsSqlChars;
    private readonly Func<string, object> sqlHierarchyIdParse;
    private readonly Func<string, int, object> sqlGeographyFromWKTString;
    private readonly Func<byte[], int, object> sqlGeographyFromWKBByteArray;
    private readonly Func<XmlReader, int, object> sqlGeographyFromGMLReader;
    private readonly Func<string, int, object> sqlGeometryFromWKTString;
    private readonly Func<byte[], int, object> sqlGeometryFromWKBByteArray;
    private readonly Func<XmlReader, int, object> sqlGeometryFromGMLReader;
    private readonly Lazy<MethodInfo> _smiSqlGeographyParse;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStGeomFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStPointFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStLineFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStPolyFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStmPointFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStmLineFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStmPolyFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStGeomCollFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStGeomFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStPointFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStLineFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStPolyFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStmPointFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStmLineFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStmPolyFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeographyStGeomCollFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeographyGeomFromGml;
    private readonly Lazy<PropertyInfo> _ipiSqlGeographyStSrid;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStGeometryType;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStDimension;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStAsBinary;
    private readonly Lazy<MethodInfo> _imiSqlGeographyAsGml;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStAsText;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStIsEmpty;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStEquals;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStDisjoint;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStIntersects;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStBuffer;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStDistance;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStIntersection;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStUnion;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStDifference;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStSymDifference;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStNumGeometries;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStGeometryN;
    private readonly Lazy<PropertyInfo> _ipiSqlGeographyLat;
    private readonly Lazy<PropertyInfo> _ipiSqlGeographyLong;
    private readonly Lazy<PropertyInfo> _ipiSqlGeographyZ;
    private readonly Lazy<PropertyInfo> _ipiSqlGeographyM;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStLength;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStStartPoint;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStEndPoint;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStIsClosed;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStNumPoints;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStPointN;
    private readonly Lazy<MethodInfo> _imiSqlGeographyStArea;
    private readonly Lazy<MethodInfo> _smiSqlGeometryParse;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStGeomFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStPointFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStLineFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStPolyFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStmPointFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStmLineFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStmPolyFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStGeomCollFromText;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStGeomFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStPointFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStLineFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStPolyFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStmPointFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStmLineFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStmPolyFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeometryStGeomCollFromWkb;
    private readonly Lazy<MethodInfo> _smiSqlGeometryGeomFromGml;
    private readonly Lazy<PropertyInfo> _ipiSqlGeometryStSrid;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStGeometryType;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStDimension;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStEnvelope;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStAsBinary;
    private readonly Lazy<MethodInfo> _imiSqlGeometryAsGml;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStAsText;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStIsEmpty;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStIsSimple;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStBoundary;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStIsValid;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStEquals;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStDisjoint;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStIntersects;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStTouches;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStCrosses;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStWithin;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStContains;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStOverlaps;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStRelate;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStBuffer;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStDistance;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStConvexHull;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStIntersection;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStUnion;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStDifference;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStSymDifference;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStNumGeometries;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStGeometryN;
    private readonly Lazy<PropertyInfo> _ipiSqlGeometryStx;
    private readonly Lazy<PropertyInfo> _ipiSqlGeometrySty;
    private readonly Lazy<PropertyInfo> _ipiSqlGeometryZ;
    private readonly Lazy<PropertyInfo> _ipiSqlGeometryM;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStLength;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStStartPoint;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStEndPoint;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStIsClosed;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStIsRing;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStNumPoints;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStPointN;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStArea;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStCentroid;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStPointOnSurface;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStExteriorRing;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStNumInteriorRing;
    private readonly Lazy<MethodInfo> _imiSqlGeometryStInteriorRingN;

    public SqlTypesAssembly()
    {
    }

    public SqlTypesAssembly(Assembly sqlSpatialAssembly)
    {
      Type type1 = sqlSpatialAssembly.GetType("Microsoft.SqlServer.Types.SqlHierarchyId", true);
      Type type2 = sqlSpatialAssembly.GetType("Microsoft.SqlServer.Types.SqlGeography", true);
      Type type3 = sqlSpatialAssembly.GetType("Microsoft.SqlServer.Types.SqlGeometry", true);
      this.SqlHierarchyIdType = type1;
      this.sqlHierarchyIdParse = SqlTypesAssembly.CreateStaticConstructorDelegateHierarchyId<string>(type1, "Parse");
      this.SqlGeographyType = type2;
      this.sqlGeographyFromWKTString = SqlTypesAssembly.CreateStaticConstructorDelegate<string>(type2, "STGeomFromText");
      this.sqlGeographyFromWKBByteArray = SqlTypesAssembly.CreateStaticConstructorDelegate<byte[]>(type2, "STGeomFromWKB");
      this.sqlGeographyFromGMLReader = SqlTypesAssembly.CreateStaticConstructorDelegate<XmlReader>(type2, "GeomFromGml");
      this.SqlGeometryType = type3;
      this.sqlGeometryFromWKTString = SqlTypesAssembly.CreateStaticConstructorDelegate<string>(type3, "STGeomFromText");
      this.sqlGeometryFromWKBByteArray = SqlTypesAssembly.CreateStaticConstructorDelegate<byte[]>(type3, "STGeomFromWKB");
      this.sqlGeometryFromGMLReader = SqlTypesAssembly.CreateStaticConstructorDelegate<XmlReader>(type3, "GeomFromGml");
      this.SqlCharsType = this.SqlGeometryType.GetPublicInstanceMethod("STAsText").ReturnType;
      this.SqlStringType = this.SqlCharsType.Assembly().GetType("System.Data.SqlTypes.SqlString", true);
      this.SqlBooleanType = this.SqlCharsType.Assembly().GetType("System.Data.SqlTypes.SqlBoolean", true);
      this.SqlBytesType = this.SqlCharsType.Assembly().GetType("System.Data.SqlTypes.SqlBytes", true);
      this.SqlDoubleType = this.SqlCharsType.Assembly().GetType("System.Data.SqlTypes.SqlDouble", true);
      this.SqlInt32Type = this.SqlCharsType.Assembly().GetType("System.Data.SqlTypes.SqlInt32", true);
      this.SqlXmlType = this.SqlCharsType.Assembly().GetType("System.Data.SqlTypes.SqlXml", true);
      this.sqlBytesFromByteArray = System.Data.Entity.SqlServer.Expressions.Lambda<byte[], object>("binaryValue", (Func<ParameterExpression, Expression>) (bytesVal => SqlTypesAssembly.BuildConvertToSqlBytes((Expression) bytesVal, this.SqlBytesType))).Compile();
      this.sqlStringFromString = System.Data.Entity.SqlServer.Expressions.Lambda<string, object>("stringValue", (Func<ParameterExpression, Expression>) (stringVal => SqlTypesAssembly.BuildConvertToSqlString((Expression) stringVal, this.SqlStringType))).Compile();
      this.sqlCharsFromString = System.Data.Entity.SqlServer.Expressions.Lambda<string, object>("stringValue", (Func<ParameterExpression, Expression>) (stringVal => SqlTypesAssembly.BuildConvertToSqlChars((Expression) stringVal, this.SqlCharsType))).Compile();
      this.sqlXmlFromXmlReader = System.Data.Entity.SqlServer.Expressions.Lambda<XmlReader, object>("readerVaue", (Func<ParameterExpression, Expression>) (readerVal => SqlTypesAssembly.BuildConvertToSqlXml((Expression) readerVal, this.SqlXmlType))).Compile();
      this.sqlBooleanToBoolean = System.Data.Entity.SqlServer.Expressions.Lambda<object, bool>("sqlBooleanValue", (Func<ParameterExpression, Expression>) (sqlBoolVal => sqlBoolVal.ConvertTo(this.SqlBooleanType).ConvertTo<bool>())).Compile();
      this.sqlBooleanToNullableBoolean = System.Data.Entity.SqlServer.Expressions.Lambda<object, bool?>("sqlBooleanValue", (Func<ParameterExpression, Expression>) (sqlBoolVal => sqlBoolVal.ConvertTo(this.SqlBooleanType).Property<bool>("IsNull").IfTrueThen(System.Data.Entity.SqlServer.Expressions.Null<bool?>()).Else(sqlBoolVal.ConvertTo(this.SqlBooleanType).ConvertTo<bool>().ConvertTo<bool?>()))).Compile();
      this.sqlBytesToByteArray = System.Data.Entity.SqlServer.Expressions.Lambda<object, byte[]>("sqlBytesValue", (Func<ParameterExpression, Expression>) (sqlBytesVal => sqlBytesVal.ConvertTo(this.SqlBytesType).Property<byte[]>("Value"))).Compile();
      this.sqlCharsToString = System.Data.Entity.SqlServer.Expressions.Lambda<object, string>("sqlCharsValue", (Func<ParameterExpression, Expression>) (sqlCharsVal => sqlCharsVal.ConvertTo(this.SqlCharsType).Call("ToSqlString").Property<string>("Value"))).Compile();
      this.sqlStringToString = System.Data.Entity.SqlServer.Expressions.Lambda<object, string>("sqlStringValue", (Func<ParameterExpression, Expression>) (sqlStringVal => sqlStringVal.ConvertTo(this.SqlStringType).Property<string>("Value"))).Compile();
      this.sqlDoubleToDouble = System.Data.Entity.SqlServer.Expressions.Lambda<object, double>("sqlDoubleValue", (Func<ParameterExpression, Expression>) (sqlDoubleVal => sqlDoubleVal.ConvertTo(this.SqlDoubleType).ConvertTo<double>())).Compile();
      this.sqlDoubleToNullableDouble = System.Data.Entity.SqlServer.Expressions.Lambda<object, double?>("sqlDoubleValue", (Func<ParameterExpression, Expression>) (sqlDoubleVal => sqlDoubleVal.ConvertTo(this.SqlDoubleType).Property<bool>("IsNull").IfTrueThen(System.Data.Entity.SqlServer.Expressions.Null<double?>()).Else(sqlDoubleVal.ConvertTo(this.SqlDoubleType).ConvertTo<double>().ConvertTo<double?>()))).Compile();
      this.sqlInt32ToInt = System.Data.Entity.SqlServer.Expressions.Lambda<object, int>("sqlInt32Value", (Func<ParameterExpression, Expression>) (sqlInt32Val => sqlInt32Val.ConvertTo(this.SqlInt32Type).ConvertTo<int>())).Compile();
      this.sqlInt32ToNullableInt = System.Data.Entity.SqlServer.Expressions.Lambda<object, int?>("sqlInt32Value", (Func<ParameterExpression, Expression>) (sqlInt32Val => sqlInt32Val.ConvertTo(this.SqlInt32Type).Property<bool>("IsNull").IfTrueThen(System.Data.Entity.SqlServer.Expressions.Null<int?>()).Else(sqlInt32Val.ConvertTo(this.SqlInt32Type).ConvertTo<int>().ConvertTo<int?>()))).Compile();
      this.sqlXmlToString = System.Data.Entity.SqlServer.Expressions.Lambda<object, string>("sqlXmlValue", (Func<ParameterExpression, Expression>) (sqlXmlVal => sqlXmlVal.ConvertTo(this.SqlXmlType).Property<string>("Value"))).Compile();
      this.isSqlGeographyNull = System.Data.Entity.SqlServer.Expressions.Lambda<object, bool>("sqlGeographyValue", (Func<ParameterExpression, Expression>) (sqlGeographyValue => sqlGeographyValue.ConvertTo(this.SqlGeographyType).Property<bool>("IsNull"))).Compile();
      this.isSqlGeometryNull = System.Data.Entity.SqlServer.Expressions.Lambda<object, bool>("sqlGeometryValue", (Func<ParameterExpression, Expression>) (sqlGeometryValue => sqlGeometryValue.ConvertTo(this.SqlGeometryType).Property<bool>("IsNull"))).Compile();
      this.geographyAsTextZMAsSqlChars = System.Data.Entity.SqlServer.Expressions.Lambda<object, object>("sqlGeographyValue", (Func<ParameterExpression, Expression>) (sqlGeographyValue => sqlGeographyValue.ConvertTo(this.SqlGeographyType).Call("AsTextZM"))).Compile();
      this.geometryAsTextZMAsSqlChars = System.Data.Entity.SqlServer.Expressions.Lambda<object, object>("sqlGeometryValue", (Func<ParameterExpression, Expression>) (sqlGeometryValue => sqlGeometryValue.ConvertTo(this.SqlGeometryType).Call("AsTextZM"))).Compile();
      this._smiSqlGeographyParse = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("Parse", this.SqlStringType)), true);
      this._smiSqlGeographyStGeomFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STGeomFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeographyStPointFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STPointFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeographyStLineFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STLineFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeographyStPolyFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STPolyFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeographyStmPointFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STMPointFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeographyStmLineFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STMLineFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeographyStmPolyFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STMPolyFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeographyStGeomCollFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STGeomCollFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeographyStGeomFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STGeomFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeographyStPointFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STPointFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeographyStLineFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STLineFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeographyStPolyFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STPolyFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeographyStmPointFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STMPointFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeographyStmLineFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STMLineFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeographyStmPolyFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STMPolyFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeographyStGeomCollFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("STGeomCollFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeographyGeomFromGml = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyStaticMethod("GeomFromGml", this.SqlXmlType, typeof (int))), true);
      this._ipiSqlGeographyStSrid = new Lazy<PropertyInfo>((Func<PropertyInfo>) (() => this.FindSqlGeographyProperty("STSrid")), true);
      this._imiSqlGeographyStGeometryType = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STGeometryType")), true);
      this._imiSqlGeographyStDimension = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STDimension")), true);
      this._imiSqlGeographyStAsBinary = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STAsBinary")), true);
      this._imiSqlGeographyAsGml = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("AsGml")), true);
      this._imiSqlGeographyStAsText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STAsText")), true);
      this._imiSqlGeographyStIsEmpty = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STIsEmpty")), true);
      this._imiSqlGeographyStEquals = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STEquals", this.SqlGeographyType)), true);
      this._imiSqlGeographyStDisjoint = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STDisjoint", this.SqlGeographyType)), true);
      this._imiSqlGeographyStIntersects = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STIntersects", this.SqlGeographyType)), true);
      this._imiSqlGeographyStBuffer = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STBuffer", typeof (double))), true);
      this._imiSqlGeographyStDistance = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STDistance", this.SqlGeographyType)), true);
      this._imiSqlGeographyStIntersection = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STIntersection", this.SqlGeographyType)), true);
      this._imiSqlGeographyStUnion = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STUnion", this.SqlGeographyType)), true);
      this._imiSqlGeographyStDifference = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STDifference", this.SqlGeographyType)), true);
      this._imiSqlGeographyStSymDifference = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STSymDifference", this.SqlGeographyType)), true);
      this._imiSqlGeographyStNumGeometries = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STNumGeometries")), true);
      this._imiSqlGeographyStGeometryN = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STGeometryN", typeof (int))), true);
      this._ipiSqlGeographyLat = new Lazy<PropertyInfo>((Func<PropertyInfo>) (() => this.FindSqlGeographyProperty("Lat")), true);
      this._ipiSqlGeographyLong = new Lazy<PropertyInfo>((Func<PropertyInfo>) (() => this.FindSqlGeographyProperty("Long")), true);
      this._ipiSqlGeographyZ = new Lazy<PropertyInfo>((Func<PropertyInfo>) (() => this.FindSqlGeographyProperty("Z")), true);
      this._ipiSqlGeographyM = new Lazy<PropertyInfo>((Func<PropertyInfo>) (() => this.FindSqlGeographyProperty("M")), true);
      this._imiSqlGeographyStLength = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STLength")), true);
      this._imiSqlGeographyStStartPoint = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STStartPoint")), true);
      this._imiSqlGeographyStEndPoint = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STEndPoint")), true);
      this._imiSqlGeographyStIsClosed = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STIsClosed")), true);
      this._imiSqlGeographyStNumPoints = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STNumPoints")), true);
      this._imiSqlGeographyStPointN = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STPointN", typeof (int))), true);
      this._imiSqlGeographyStArea = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeographyMethod("STArea")), true);
      this._smiSqlGeometryParse = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("Parse", this.SqlStringType)), true);
      this._smiSqlGeometryStGeomFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STGeomFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeometryStPointFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STPointFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeometryStLineFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STLineFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeometryStPolyFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STPolyFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeometryStmPointFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STMPointFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeometryStmLineFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STMLineFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeometryStmPolyFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STMPolyFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeometryStGeomCollFromText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STGeomCollFromText", this.SqlCharsType, typeof (int))), true);
      this._smiSqlGeometryStGeomFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STGeomFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeometryStPointFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STPointFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeometryStLineFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STLineFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeometryStPolyFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STPolyFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeometryStmPointFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STMPointFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeometryStmLineFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STMLineFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeometryStmPolyFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STMPolyFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeometryStGeomCollFromWkb = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("STGeomCollFromWKB", this.SqlBytesType, typeof (int))), true);
      this._smiSqlGeometryGeomFromGml = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryStaticMethod("GeomFromGml", this.SqlXmlType, typeof (int))), true);
      this._ipiSqlGeometryStSrid = new Lazy<PropertyInfo>((Func<PropertyInfo>) (() => this.FindSqlGeometryProperty("STSrid")), true);
      this._imiSqlGeometryStGeometryType = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STGeometryType")), true);
      this._imiSqlGeometryStDimension = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STDimension")), true);
      this._imiSqlGeometryStEnvelope = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STEnvelope")), true);
      this._imiSqlGeometryStAsBinary = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STAsBinary")), true);
      this._imiSqlGeometryAsGml = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("AsGml")), true);
      this._imiSqlGeometryStAsText = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STAsText")), true);
      this._imiSqlGeometryStIsEmpty = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STIsEmpty")), true);
      this._imiSqlGeometryStIsSimple = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STIsSimple")), true);
      this._imiSqlGeometryStBoundary = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STBoundary")), true);
      this._imiSqlGeometryStIsValid = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STIsValid")), true);
      this._imiSqlGeometryStEquals = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STEquals", this.SqlGeometryType)), true);
      this._imiSqlGeometryStDisjoint = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STDisjoint", this.SqlGeometryType)), true);
      this._imiSqlGeometryStIntersects = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STIntersects", this.SqlGeometryType)), true);
      this._imiSqlGeometryStTouches = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STTouches", this.SqlGeometryType)), true);
      this._imiSqlGeometryStCrosses = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STCrosses", this.SqlGeometryType)), true);
      this._imiSqlGeometryStWithin = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STWithin", this.SqlGeometryType)), true);
      this._imiSqlGeometryStContains = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STContains", this.SqlGeometryType)), true);
      this._imiSqlGeometryStOverlaps = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STOverlaps", this.SqlGeometryType)), true);
      this._imiSqlGeometryStRelate = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STRelate", this.SqlGeometryType, typeof (string))), true);
      this._imiSqlGeometryStBuffer = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STBuffer", typeof (double))), true);
      this._imiSqlGeometryStDistance = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STDistance", this.SqlGeometryType)), true);
      this._imiSqlGeometryStConvexHull = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STConvexHull")), true);
      this._imiSqlGeometryStIntersection = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STIntersection", this.SqlGeometryType)), true);
      this._imiSqlGeometryStUnion = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STUnion", this.SqlGeometryType)), true);
      this._imiSqlGeometryStDifference = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STDifference", this.SqlGeometryType)), true);
      this._imiSqlGeometryStSymDifference = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STSymDifference", this.SqlGeometryType)), true);
      this._imiSqlGeometryStNumGeometries = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STNumGeometries")), true);
      this._imiSqlGeometryStGeometryN = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STGeometryN", typeof (int))), true);
      this._ipiSqlGeometryStx = new Lazy<PropertyInfo>((Func<PropertyInfo>) (() => this.FindSqlGeometryProperty("STX")), true);
      this._ipiSqlGeometrySty = new Lazy<PropertyInfo>((Func<PropertyInfo>) (() => this.FindSqlGeometryProperty("STY")), true);
      this._ipiSqlGeometryZ = new Lazy<PropertyInfo>((Func<PropertyInfo>) (() => this.FindSqlGeometryProperty("Z")), true);
      this._ipiSqlGeometryM = new Lazy<PropertyInfo>((Func<PropertyInfo>) (() => this.FindSqlGeometryProperty("M")), true);
      this._imiSqlGeometryStLength = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STLength")), true);
      this._imiSqlGeometryStStartPoint = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STStartPoint")), true);
      this._imiSqlGeometryStEndPoint = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STEndPoint")), true);
      this._imiSqlGeometryStIsClosed = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STIsClosed")), true);
      this._imiSqlGeometryStIsRing = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STIsRing")), true);
      this._imiSqlGeometryStNumPoints = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STNumPoints")), true);
      this._imiSqlGeometryStPointN = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STPointN", typeof (int))), true);
      this._imiSqlGeometryStArea = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STArea")), true);
      this._imiSqlGeometryStCentroid = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STCentroid")), true);
      this._imiSqlGeometryStPointOnSurface = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STPointOnSurface")), true);
      this._imiSqlGeometryStExteriorRing = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STExteriorRing")), true);
      this._imiSqlGeometryStNumInteriorRing = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STNumInteriorRing")), true);
      this._imiSqlGeometryStInteriorRingN = new Lazy<MethodInfo>((Func<MethodInfo>) (() => this.FindSqlGeometryMethod("STInteriorRingN", typeof (int))), true);
    }

    internal Type SqlBooleanType { get; private set; }

    internal Type SqlBytesType { get; private set; }

    internal Type SqlCharsType { get; private set; }

    internal Type SqlStringType { get; private set; }

    internal Type SqlDoubleType { get; private set; }

    internal Type SqlInt32Type { get; private set; }

    internal Type SqlXmlType { get; private set; }

    internal bool SqlBooleanToBoolean(object sqlBooleanValue) => this.sqlBooleanToBoolean(sqlBooleanValue);

    internal bool? SqlBooleanToNullableBoolean(object sqlBooleanValue) => this.sqlBooleanToBoolean == null ? new bool?() : this.sqlBooleanToNullableBoolean(sqlBooleanValue);

    internal object SqlBytesFromByteArray(byte[] binaryValue) => this.sqlBytesFromByteArray(binaryValue);

    internal byte[] SqlBytesToByteArray(object sqlBytesValue) => sqlBytesValue == null ? (byte[]) null : this.sqlBytesToByteArray(sqlBytesValue);

    internal object SqlStringFromString(string stringValue) => this.sqlStringFromString(stringValue);

    internal object SqlCharsFromString(string stringValue) => this.sqlCharsFromString(stringValue);

    internal string SqlCharsToString(object sqlCharsValue) => sqlCharsValue == null ? (string) null : this.sqlCharsToString(sqlCharsValue);

    internal string SqlStringToString(object sqlStringValue) => sqlStringValue == null ? (string) null : this.sqlStringToString(sqlStringValue);

    internal double SqlDoubleToDouble(object sqlDoubleValue) => this.sqlDoubleToDouble(sqlDoubleValue);

    internal double? SqlDoubleToNullableDouble(object sqlDoubleValue) => sqlDoubleValue == null ? new double?() : this.sqlDoubleToNullableDouble(sqlDoubleValue);

    internal int SqlInt32ToInt(object sqlInt32Value) => this.sqlInt32ToInt(sqlInt32Value);

    internal int? SqlInt32ToNullableInt(object sqlInt32Value) => sqlInt32Value == null ? new int?() : this.sqlInt32ToNullableInt(sqlInt32Value);

    internal object SqlXmlFromString(string stringValue) => this.sqlXmlFromXmlReader(SqlTypesAssembly.XmlReaderFromString(stringValue));

    internal string SqlXmlToString(object sqlXmlValue) => sqlXmlValue == null ? (string) null : this.sqlXmlToString(sqlXmlValue);

    internal bool IsSqlGeographyNull(object sqlGeographyValue) => sqlGeographyValue == null || this.isSqlGeographyNull(sqlGeographyValue);

    internal bool IsSqlGeometryNull(object sqlGeometryValue) => sqlGeometryValue == null || this.isSqlGeometryNull(sqlGeometryValue);

    internal string GeographyAsTextZM(DbGeography geographyValue) => geographyValue == null ? (string) null : this.SqlCharsToString(this.geographyAsTextZMAsSqlChars(this.ConvertToSqlTypesGeography(geographyValue)));

    internal string GeometryAsTextZM(DbGeometry geometryValue) => geometryValue == null ? (string) null : this.SqlCharsToString(this.geometryAsTextZMAsSqlChars(this.ConvertToSqlTypesGeometry(geometryValue)));

    internal Type SqlHierarchyIdType { get; private set; }

    internal Type SqlGeographyType { get; private set; }

    internal Type SqlGeometryType { get; private set; }

    internal object ConvertToSqlTypesHierarchyId(HierarchyId hierarchyIdValue) => this.GetSqlTypesHierarchyIdValue(hierarchyIdValue.ToString());

    internal object ConvertToSqlTypesGeography(DbGeography geographyValue) => this.GetSqlTypesSpatialValue(geographyValue.AsSpatialValue(), this.SqlGeographyType);

    internal object SqlTypesGeographyFromBinary(byte[] wellKnownBinary, int srid) => this.sqlGeographyFromWKBByteArray(wellKnownBinary, srid);

    internal object SqlTypesGeographyFromText(string wellKnownText, int srid) => this.sqlGeographyFromWKTString(wellKnownText, srid);

    internal object ConvertToSqlTypesGeometry(DbGeometry geometryValue) => this.GetSqlTypesSpatialValue(geometryValue.AsSpatialValue(), this.SqlGeometryType);

    internal object SqlTypesGeometryFromBinary(byte[] wellKnownBinary, int srid) => this.sqlGeometryFromWKBByteArray(wellKnownBinary, srid);

    internal object SqlTypesGeometryFromText(string wellKnownText, int srid) => this.sqlGeometryFromWKTString(wellKnownText, srid);

    private object GetSqlTypesHierarchyIdValue(string hierarchyIdValue) => this.sqlHierarchyIdParse(hierarchyIdValue);

    private object GetSqlTypesSpatialValue(
      IDbSpatialValue spatialValue,
      Type requiredProviderValueType)
    {
      object providerValue = spatialValue.ProviderValue;
      if (providerValue != null && providerValue.GetType() == requiredProviderValueType)
        return providerValue;
      int? coordinateSystemId = spatialValue.CoordinateSystemId;
      if (coordinateSystemId.HasValue)
      {
        byte[] wellKnownBinary = spatialValue.WellKnownBinary;
        if (wellKnownBinary != null)
          return !spatialValue.IsGeography ? this.sqlGeometryFromWKBByteArray(wellKnownBinary, coordinateSystemId.Value) : this.sqlGeographyFromWKBByteArray(wellKnownBinary, coordinateSystemId.Value);
        string wellKnownText = spatialValue.WellKnownText;
        if (wellKnownText != null)
          return !spatialValue.IsGeography ? this.sqlGeometryFromWKTString(wellKnownText, coordinateSystemId.Value) : this.sqlGeographyFromWKTString(wellKnownText, coordinateSystemId.Value);
        string gmlString = spatialValue.GmlString;
        if (gmlString != null)
        {
          XmlReader xmlReader = SqlTypesAssembly.XmlReaderFromString(gmlString);
          return !spatialValue.IsGeography ? this.sqlGeometryFromGMLReader(xmlReader, coordinateSystemId.Value) : this.sqlGeographyFromGMLReader(xmlReader, coordinateSystemId.Value);
        }
      }
      throw spatialValue.NotSqlCompatible();
    }

    private static XmlReader XmlReaderFromString(string stringValue) => XmlReader.Create((TextReader) new StringReader(stringValue));

    private static Func<TArg, object> CreateStaticConstructorDelegateHierarchyId<TArg>(
      Type hierarchyIdType,
      string methodName)
    {
      ParameterExpression parameterExpression = Expression.Parameter(typeof (TArg));
      MethodInfo method = hierarchyIdType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);
      Expression expression = SqlTypesAssembly.BuildSqlString((Expression) parameterExpression, method.GetParameters()[0].ParameterType);
      return Expression.Lambda<Func<TArg, object>>((Expression) Expression.Convert((Expression) Expression.Call((Expression) null, method, expression), typeof (object)), parameterExpression).Compile();
    }

    private static Func<TArg, int, object> CreateStaticConstructorDelegate<TArg>(
      Type spatialType,
      string methodName)
    {
      ParameterExpression parameterExpression3 = Expression.Parameter(typeof (TArg));
      ParameterExpression parameterExpression4 = Expression.Parameter(typeof (int));
      MethodInfo onlyDeclaredMethod = spatialType.GetOnlyDeclaredMethod(methodName);
      Expression sqlType = SqlTypesAssembly.BuildConvertToSqlType((Expression) parameterExpression3, onlyDeclaredMethod.GetParameters()[0].ParameterType);
      return ((Expression<Func<TArg, int, object>>) ((parameterExpression1, parameterExpression2) => Expression.Call((Expression) null, onlyDeclaredMethod, sqlType, parameterExpression2))).Compile();
    }

    private static Expression BuildConvertToSqlType(Expression toConvert, Type convertTo)
    {
      if (toConvert.Type == typeof (byte[]))
        return SqlTypesAssembly.BuildConvertToSqlBytes(toConvert, convertTo);
      return toConvert.Type == typeof (string) ? (convertTo.Name == "SqlString" ? SqlTypesAssembly.BuildConvertToSqlString(toConvert, convertTo) : SqlTypesAssembly.BuildConvertToSqlChars(toConvert, convertTo)) : (toConvert.Type == typeof (XmlReader) ? SqlTypesAssembly.BuildConvertToSqlXml(toConvert, convertTo) : toConvert);
    }

    private static Expression BuildConvertToSqlBytes(
      Expression toConvert,
      Type sqlBytesType)
    {
      return (Expression) Expression.New(sqlBytesType.GetDeclaredConstructor(toConvert.Type), toConvert);
    }

    private static Expression BuildConvertToSqlChars(
      Expression toConvert,
      Type sqlCharsType)
    {
      Type type = sqlCharsType.Assembly().GetType("System.Data.SqlTypes.SqlString", true);
      return (Expression) Expression.New(sqlCharsType.GetDeclaredConstructor(type), (Expression) Expression.New(type.GetDeclaredConstructor(typeof (string)), toConvert));
    }

    private static Expression BuildSqlString(Expression toConvert, Type sqlStringType) => (Expression) Expression.New(sqlStringType.GetConstructor(BindingFlags.Instance | BindingFlags.Public, (Binder) null, new Type[1]
    {
      typeof (string)
    }, (ParameterModifier[]) null), toConvert);

    private static Expression BuildConvertToSqlString(
      Expression toConvert,
      Type sqlStringType)
    {
      return (Expression) Expression.Convert((Expression) Expression.New(sqlStringType.GetDeclaredConstructor(typeof (string)), toConvert), typeof (object));
    }

    private static Expression BuildConvertToSqlXml(Expression toConvert, Type sqlXmlType) => (Expression) Expression.New(sqlXmlType.GetDeclaredConstructor(toConvert.Type), toConvert);

    public Lazy<MethodInfo> SmiSqlGeographyParse => this._smiSqlGeographyParse;

    public Lazy<MethodInfo> SmiSqlGeographyStGeomFromText => this._smiSqlGeographyStGeomFromText;

    public Lazy<MethodInfo> SmiSqlGeographyStPointFromText => this._smiSqlGeographyStPointFromText;

    public Lazy<MethodInfo> SmiSqlGeographyStLineFromText => this._smiSqlGeographyStLineFromText;

    public Lazy<MethodInfo> SmiSqlGeographyStPolyFromText => this._smiSqlGeographyStPolyFromText;

    public Lazy<MethodInfo> SmiSqlGeographyStmPointFromText => this._smiSqlGeographyStmPointFromText;

    public Lazy<MethodInfo> SmiSqlGeographyStmLineFromText => this._smiSqlGeographyStmLineFromText;

    public Lazy<MethodInfo> SmiSqlGeographyStmPolyFromText => this._smiSqlGeographyStmPolyFromText;

    public Lazy<MethodInfo> SmiSqlGeographyStGeomCollFromText => this._smiSqlGeographyStGeomCollFromText;

    public Lazy<MethodInfo> SmiSqlGeographyStGeomFromWkb => this._smiSqlGeographyStGeomFromWkb;

    public Lazy<MethodInfo> SmiSqlGeographyStPointFromWkb => this._smiSqlGeographyStPointFromWkb;

    public Lazy<MethodInfo> SmiSqlGeographyStLineFromWkb => this._smiSqlGeographyStLineFromWkb;

    public Lazy<MethodInfo> SmiSqlGeographyStPolyFromWkb => this._smiSqlGeographyStPolyFromWkb;

    public Lazy<MethodInfo> SmiSqlGeographyStmPointFromWkb => this._smiSqlGeographyStmPointFromWkb;

    public Lazy<MethodInfo> SmiSqlGeographyStmLineFromWkb => this._smiSqlGeographyStmLineFromWkb;

    public Lazy<MethodInfo> SmiSqlGeographyStmPolyFromWkb => this._smiSqlGeographyStmPolyFromWkb;

    public Lazy<MethodInfo> SmiSqlGeographyStGeomCollFromWkb => this._smiSqlGeographyStGeomCollFromWkb;

    public Lazy<MethodInfo> SmiSqlGeographyGeomFromGml => this._smiSqlGeographyGeomFromGml;

    public Lazy<PropertyInfo> IpiSqlGeographyStSrid => this._ipiSqlGeographyStSrid;

    public Lazy<MethodInfo> ImiSqlGeographyStGeometryType => this._imiSqlGeographyStGeometryType;

    public Lazy<MethodInfo> ImiSqlGeographyStDimension => this._imiSqlGeographyStDimension;

    public Lazy<MethodInfo> ImiSqlGeographyStAsBinary => this._imiSqlGeographyStAsBinary;

    public Lazy<MethodInfo> ImiSqlGeographyAsGml => this._imiSqlGeographyAsGml;

    public Lazy<MethodInfo> ImiSqlGeographyStAsText => this._imiSqlGeographyStAsText;

    public Lazy<MethodInfo> ImiSqlGeographyStIsEmpty => this._imiSqlGeographyStIsEmpty;

    public Lazy<MethodInfo> ImiSqlGeographyStEquals => this._imiSqlGeographyStEquals;

    public Lazy<MethodInfo> ImiSqlGeographyStDisjoint => this._imiSqlGeographyStDisjoint;

    public Lazy<MethodInfo> ImiSqlGeographyStIntersects => this._imiSqlGeographyStIntersects;

    public Lazy<MethodInfo> ImiSqlGeographyStBuffer => this._imiSqlGeographyStBuffer;

    public Lazy<MethodInfo> ImiSqlGeographyStDistance => this._imiSqlGeographyStDistance;

    public Lazy<MethodInfo> ImiSqlGeographyStIntersection => this._imiSqlGeographyStIntersection;

    public Lazy<MethodInfo> ImiSqlGeographyStUnion => this._imiSqlGeographyStUnion;

    public Lazy<MethodInfo> ImiSqlGeographyStDifference => this._imiSqlGeographyStDifference;

    public Lazy<MethodInfo> ImiSqlGeographyStSymDifference => this._imiSqlGeographyStSymDifference;

    public Lazy<MethodInfo> ImiSqlGeographyStNumGeometries => this._imiSqlGeographyStNumGeometries;

    public Lazy<MethodInfo> ImiSqlGeographyStGeometryN => this._imiSqlGeographyStGeometryN;

    public Lazy<PropertyInfo> IpiSqlGeographyLat => this._ipiSqlGeographyLat;

    public Lazy<PropertyInfo> IpiSqlGeographyLong => this._ipiSqlGeographyLong;

    public Lazy<PropertyInfo> IpiSqlGeographyZ => this._ipiSqlGeographyZ;

    public Lazy<PropertyInfo> IpiSqlGeographyM => this._ipiSqlGeographyM;

    public Lazy<MethodInfo> ImiSqlGeographyStLength => this._imiSqlGeographyStLength;

    public Lazy<MethodInfo> ImiSqlGeographyStStartPoint => this._imiSqlGeographyStStartPoint;

    public Lazy<MethodInfo> ImiSqlGeographyStEndPoint => this._imiSqlGeographyStEndPoint;

    public Lazy<MethodInfo> ImiSqlGeographyStIsClosed => this._imiSqlGeographyStIsClosed;

    public Lazy<MethodInfo> ImiSqlGeographyStNumPoints => this._imiSqlGeographyStNumPoints;

    public Lazy<MethodInfo> ImiSqlGeographyStPointN => this._imiSqlGeographyStPointN;

    public Lazy<MethodInfo> ImiSqlGeographyStArea => this._imiSqlGeographyStArea;

    public Lazy<MethodInfo> SmiSqlGeometryParse => this._smiSqlGeometryParse;

    public Lazy<MethodInfo> SmiSqlGeometryStGeomFromText => this._smiSqlGeometryStGeomFromText;

    public Lazy<MethodInfo> SmiSqlGeometryStPointFromText => this._smiSqlGeometryStPointFromText;

    public Lazy<MethodInfo> SmiSqlGeometryStLineFromText => this._smiSqlGeometryStLineFromText;

    public Lazy<MethodInfo> SmiSqlGeometryStPolyFromText => this._smiSqlGeometryStPolyFromText;

    public Lazy<MethodInfo> SmiSqlGeometryStmPointFromText => this._smiSqlGeometryStmPointFromText;

    public Lazy<MethodInfo> SmiSqlGeometryStmLineFromText => this._smiSqlGeometryStmLineFromText;

    public Lazy<MethodInfo> SmiSqlGeometryStmPolyFromText => this._smiSqlGeometryStmPolyFromText;

    public Lazy<MethodInfo> SmiSqlGeometryStGeomCollFromText => this._smiSqlGeometryStGeomCollFromText;

    public Lazy<MethodInfo> SmiSqlGeometryStGeomFromWkb => this._smiSqlGeometryStGeomFromWkb;

    public Lazy<MethodInfo> SmiSqlGeometryStPointFromWkb => this._smiSqlGeometryStPointFromWkb;

    public Lazy<MethodInfo> SmiSqlGeometryStLineFromWkb => this._smiSqlGeometryStLineFromWkb;

    public Lazy<MethodInfo> SmiSqlGeometryStPolyFromWkb => this._smiSqlGeometryStPolyFromWkb;

    public Lazy<MethodInfo> SmiSqlGeometryStmPointFromWkb => this._smiSqlGeometryStmPointFromWkb;

    public Lazy<MethodInfo> SmiSqlGeometryStmLineFromWkb => this._smiSqlGeometryStmLineFromWkb;

    public Lazy<MethodInfo> SmiSqlGeometryStmPolyFromWkb => this._smiSqlGeometryStmPolyFromWkb;

    public Lazy<MethodInfo> SmiSqlGeometryStGeomCollFromWkb => this._smiSqlGeometryStGeomCollFromWkb;

    public Lazy<MethodInfo> SmiSqlGeometryGeomFromGml => this._smiSqlGeometryGeomFromGml;

    public Lazy<PropertyInfo> IpiSqlGeometryStSrid => this._ipiSqlGeometryStSrid;

    public Lazy<MethodInfo> ImiSqlGeometryStGeometryType => this._imiSqlGeometryStGeometryType;

    public Lazy<MethodInfo> ImiSqlGeometryStDimension => this._imiSqlGeometryStDimension;

    public Lazy<MethodInfo> ImiSqlGeometryStEnvelope => this._imiSqlGeometryStEnvelope;

    public Lazy<MethodInfo> ImiSqlGeometryStAsBinary => this._imiSqlGeometryStAsBinary;

    public Lazy<MethodInfo> ImiSqlGeometryAsGml => this._imiSqlGeometryAsGml;

    public Lazy<MethodInfo> ImiSqlGeometryStAsText => this._imiSqlGeometryStAsText;

    public Lazy<MethodInfo> ImiSqlGeometryStIsEmpty => this._imiSqlGeometryStIsEmpty;

    public Lazy<MethodInfo> ImiSqlGeometryStIsSimple => this._imiSqlGeometryStIsSimple;

    public Lazy<MethodInfo> ImiSqlGeometryStBoundary => this._imiSqlGeometryStBoundary;

    public Lazy<MethodInfo> ImiSqlGeometryStIsValid => this._imiSqlGeometryStIsValid;

    public Lazy<MethodInfo> ImiSqlGeometryStEquals => this._imiSqlGeometryStEquals;

    public Lazy<MethodInfo> ImiSqlGeometryStDisjoint => this._imiSqlGeometryStDisjoint;

    public Lazy<MethodInfo> ImiSqlGeometryStIntersects => this._imiSqlGeometryStIntersects;

    public Lazy<MethodInfo> ImiSqlGeometryStTouches => this._imiSqlGeometryStTouches;

    public Lazy<MethodInfo> ImiSqlGeometryStCrosses => this._imiSqlGeometryStCrosses;

    public Lazy<MethodInfo> ImiSqlGeometryStWithin => this._imiSqlGeometryStWithin;

    public Lazy<MethodInfo> ImiSqlGeometryStContains => this._imiSqlGeometryStContains;

    public Lazy<MethodInfo> ImiSqlGeometryStOverlaps => this._imiSqlGeometryStOverlaps;

    public Lazy<MethodInfo> ImiSqlGeometryStRelate => this._imiSqlGeometryStRelate;

    public Lazy<MethodInfo> ImiSqlGeometryStBuffer => this._imiSqlGeometryStBuffer;

    public Lazy<MethodInfo> ImiSqlGeometryStDistance => this._imiSqlGeometryStDistance;

    public Lazy<MethodInfo> ImiSqlGeometryStConvexHull => this._imiSqlGeometryStConvexHull;

    public Lazy<MethodInfo> ImiSqlGeometryStIntersection => this._imiSqlGeometryStIntersection;

    public Lazy<MethodInfo> ImiSqlGeometryStUnion => this._imiSqlGeometryStUnion;

    public Lazy<MethodInfo> ImiSqlGeometryStDifference => this._imiSqlGeometryStDifference;

    public Lazy<MethodInfo> ImiSqlGeometryStSymDifference => this._imiSqlGeometryStSymDifference;

    public Lazy<MethodInfo> ImiSqlGeometryStNumGeometries => this._imiSqlGeometryStNumGeometries;

    public Lazy<MethodInfo> ImiSqlGeometryStGeometryN => this._imiSqlGeometryStGeometryN;

    public Lazy<PropertyInfo> IpiSqlGeometryStx => this._ipiSqlGeometryStx;

    public Lazy<PropertyInfo> IpiSqlGeometrySty => this._ipiSqlGeometrySty;

    public Lazy<PropertyInfo> IpiSqlGeometryZ => this._ipiSqlGeometryZ;

    public Lazy<PropertyInfo> IpiSqlGeometryM => this._ipiSqlGeometryM;

    public Lazy<MethodInfo> ImiSqlGeometryStLength => this._imiSqlGeometryStLength;

    public Lazy<MethodInfo> ImiSqlGeometryStStartPoint => this._imiSqlGeometryStStartPoint;

    public Lazy<MethodInfo> ImiSqlGeometryStEndPoint => this._imiSqlGeometryStEndPoint;

    public Lazy<MethodInfo> ImiSqlGeometryStIsClosed => this._imiSqlGeometryStIsClosed;

    public Lazy<MethodInfo> ImiSqlGeometryStIsRing => this._imiSqlGeometryStIsRing;

    public Lazy<MethodInfo> ImiSqlGeometryStNumPoints => this._imiSqlGeometryStNumPoints;

    public Lazy<MethodInfo> ImiSqlGeometryStPointN => this._imiSqlGeometryStPointN;

    public Lazy<MethodInfo> ImiSqlGeometryStArea => this._imiSqlGeometryStArea;

    public Lazy<MethodInfo> ImiSqlGeometryStCentroid => this._imiSqlGeometryStCentroid;

    public Lazy<MethodInfo> ImiSqlGeometryStPointOnSurface => this._imiSqlGeometryStPointOnSurface;

    public Lazy<MethodInfo> ImiSqlGeometryStExteriorRing => this._imiSqlGeometryStExteriorRing;

    public Lazy<MethodInfo> ImiSqlGeometryStNumInteriorRing => this._imiSqlGeometryStNumInteriorRing;

    public Lazy<MethodInfo> ImiSqlGeometryStInteriorRingN => this._imiSqlGeometryStInteriorRingN;

    private MethodInfo FindSqlGeographyMethod(string methodName, params Type[] argTypes) => this.SqlGeographyType.GetDeclaredMethod(methodName, argTypes);

    private MethodInfo FindSqlGeographyStaticMethod(
      string methodName,
      params Type[] argTypes)
    {
      return this.SqlGeographyType.GetDeclaredMethod(methodName, argTypes);
    }

    private PropertyInfo FindSqlGeographyProperty(string propertyName) => this.SqlGeographyType.GetRuntimeProperty(propertyName);

    private MethodInfo FindSqlGeometryStaticMethod(
      string methodName,
      params Type[] argTypes)
    {
      return this.SqlGeometryType.GetDeclaredMethod(methodName, argTypes);
    }

    private MethodInfo FindSqlGeometryMethod(string methodName, params Type[] argTypes) => this.SqlGeometryType.GetDeclaredMethod(methodName, argTypes);

    private PropertyInfo FindSqlGeometryProperty(string propertyName) => this.SqlGeometryType.GetRuntimeProperty(propertyName);
  }
}

// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlGen.SqlFunctionCallHandler
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Spatial;
using System.Data.Entity.SqlServer.Utilities;
using System.Linq;
using System.Text;

namespace System.Data.Entity.SqlServer.SqlGen
{
  internal static class SqlFunctionCallHandler
  {
    private static readonly Dictionary<string, SqlFunctionCallHandler.FunctionHandler> _storeFunctionHandlers = SqlFunctionCallHandler.InitializeStoreFunctionHandlers();
    private static readonly Dictionary<string, SqlFunctionCallHandler.FunctionHandler> _canonicalFunctionHandlers = SqlFunctionCallHandler.InitializeCanonicalFunctionHandlers();
    private static readonly Dictionary<string, string> _functionNameToOperatorDictionary = SqlFunctionCallHandler.InitializeFunctionNameToOperatorDictionary();
    private static readonly Dictionary<string, string> _dateAddFunctionNameToDatepartDictionary = SqlFunctionCallHandler.InitializeDateAddFunctionNameToDatepartDictionary();
    private static readonly Dictionary<string, string> _dateDiffFunctionNameToDatepartDictionary = SqlFunctionCallHandler.InitializeDateDiffFunctionNameToDatepartDictionary();
    private static readonly Dictionary<string, SqlFunctionCallHandler.FunctionHandler> _hierarchyIdFunctionNameToStaticMethodHandlerDictionary = SqlFunctionCallHandler.InitializeHierarchyIdStaticMethodFunctionsDictionary();
    private static readonly Dictionary<string, SqlFunctionCallHandler.FunctionHandler> _geographyFunctionNameToStaticMethodHandlerDictionary = SqlFunctionCallHandler.InitializeGeographyStaticMethodFunctionsDictionary();
    private static readonly Dictionary<string, string> _geographyFunctionNameToInstancePropertyNameDictionary = SqlFunctionCallHandler.InitializeGeographyInstancePropertyFunctionsDictionary();
    private static readonly Dictionary<string, string> _geographyRenamedInstanceMethodFunctionDictionary = SqlFunctionCallHandler.InitializeRenamedGeographyInstanceMethodFunctions();
    private static readonly Dictionary<string, SqlFunctionCallHandler.FunctionHandler> _geometryFunctionNameToStaticMethodHandlerDictionary = SqlFunctionCallHandler.InitializeGeometryStaticMethodFunctionsDictionary();
    private static readonly Dictionary<string, string> _geometryFunctionNameToInstancePropertyNameDictionary = SqlFunctionCallHandler.InitializeGeometryInstancePropertyFunctionsDictionary();
    private static readonly Dictionary<string, string> _geometryRenamedInstanceMethodFunctionDictionary = SqlFunctionCallHandler.InitializeRenamedGeometryInstanceMethodFunctions();
    private static readonly ISet<string> _datepartKeywords = (ISet<string>) new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      "year",
      "yy",
      "yyyy",
      "quarter",
      "qq",
      "q",
      "month",
      "mm",
      "m",
      "dayofyear",
      "dy",
      "y",
      "day",
      "dd",
      "d",
      "week",
      "wk",
      "ww",
      "weekday",
      "dw",
      "w",
      "hour",
      "hh",
      "minute",
      "mi",
      "n",
      "second",
      "ss",
      "s",
      "millisecond",
      "ms",
      "microsecond",
      "mcs",
      "nanosecond",
      "ns",
      "tzoffset",
      "tz",
      "iso_week",
      "isoww",
      "isowk"
    };
    private static readonly ISet<string> _functionRequiresReturnTypeCastToInt64 = (ISet<string>) new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      "SqlServer.CHARINDEX"
    };
    private static readonly ISet<string> _functionRequiresReturnTypeCastToInt32 = (ISet<string>) new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      "SqlServer.LEN",
      "SqlServer.PATINDEX",
      "SqlServer.DATALENGTH",
      "SqlServer.CHARINDEX",
      "Edm.IndexOf",
      "Edm.Length"
    };
    private static readonly ISet<string> _functionRequiresReturnTypeCastToInt16 = (ISet<string>) new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      "Edm.Abs"
    };
    private static readonly ISet<string> _functionRequiresReturnTypeCastToSingle = (ISet<string>) new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      "Edm.Abs",
      "Edm.Round",
      "Edm.Floor",
      "Edm.Ceiling"
    };
    private static readonly ISet<string> _maxTypeNames = (ISet<string>) new HashSet<string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
    {
      "varchar(max)",
      "nvarchar(max)",
      "text",
      "ntext",
      "varbinary(max)",
      "image",
      "xml"
    };
    private static readonly DbExpression _defaultGeographySridExpression = (DbExpression) DbExpressionBuilder.Constant((object) DbGeography.DefaultCoordinateSystemId);
    private static readonly DbExpression _defaultGeometrySridExpression = (DbExpression) DbExpressionBuilder.Constant((object) DbGeometry.DefaultCoordinateSystemId);

    private static Dictionary<string, SqlFunctionCallHandler.FunctionHandler> InitializeStoreFunctionHandlers() => new Dictionary<string, SqlFunctionCallHandler.FunctionHandler>(19, (IEqualityComparer<string>) StringComparer.Ordinal)
    {
      {
        "CONCAT",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleConcatFunction)
      },
      {
        "DATEADD",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleDatepartDateFunction)
      },
      {
        "DATEDIFF",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleDatepartDateFunction)
      },
      {
        "DATENAME",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleDatepartDateFunction)
      },
      {
        "DATEPART",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleDatepartDateFunction)
      },
      {
        "Parse",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "hierarchyid::Parse"))
      },
      {
        "GetRoot",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "hierarchyid::GetRoot"))
      },
      {
        "POINTGEOGRAPHY",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::Point"))
      },
      {
        "POINTGEOMETRY",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::Point"))
      },
      {
        "ASTEXTZM",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, "AsTextZM", functionExpression, false))
      },
      {
        "BUFFERWITHTOLERANCE",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, "BufferWithTolerance", functionExpression, false))
      },
      {
        "ENVELOPEANGLE",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, "EnvelopeAngle", functionExpression, false))
      },
      {
        "ENVELOPECENTER",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, "EnvelopeCenter", functionExpression, false))
      },
      {
        "INSTANCEOF",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, "InstanceOf", functionExpression, false))
      },
      {
        "FILTER",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, "Filter", functionExpression, false))
      },
      {
        "MAKEVALID",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, "MakeValid", functionExpression, false))
      },
      {
        "REDUCE",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, "Reduce", functionExpression, false))
      },
      {
        "NUMRINGS",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, "NumRings", functionExpression, false))
      },
      {
        "RINGN",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, "RingN", functionExpression, false))
      }
    };

    private static Dictionary<string, SqlFunctionCallHandler.FunctionHandler> InitializeCanonicalFunctionHandlers() => new Dictionary<string, SqlFunctionCallHandler.FunctionHandler>(16, (IEqualityComparer<string>) StringComparer.Ordinal)
    {
      {
        "IndexOf",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionIndexOf)
      },
      {
        "Length",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionLength)
      },
      {
        "NewGuid",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionNewGuid)
      },
      {
        "Round",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionRound)
      },
      {
        "Truncate",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionTruncate)
      },
      {
        "Abs",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionAbs)
      },
      {
        "ToLower",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionToLower)
      },
      {
        "ToUpper",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionToUpper)
      },
      {
        "Trim",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionTrim)
      },
      {
        "Contains",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionContains)
      },
      {
        "StartsWith",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionStartsWith)
      },
      {
        "EndsWith",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionEndsWith)
      },
      {
        "Year",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDatepart)
      },
      {
        "Month",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDatepart)
      },
      {
        "Day",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDatepart)
      },
      {
        "Hour",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDatepart)
      },
      {
        "Minute",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDatepart)
      },
      {
        "Second",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDatepart)
      },
      {
        "Millisecond",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDatepart)
      },
      {
        "DayOfYear",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDatepart)
      },
      {
        "CurrentDateTime",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionCurrentDateTime)
      },
      {
        "CurrentUtcDateTime",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionCurrentUtcDateTime)
      },
      {
        "CurrentDateTimeOffset",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionCurrentDateTimeOffset)
      },
      {
        "GetTotalOffsetMinutes",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionGetTotalOffsetMinutes)
      },
      {
        "LocalDateTime",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionLocalDateTime)
      },
      {
        "UtcDateTime",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionUtcDateTime)
      },
      {
        "TruncateTime",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionTruncateTime)
      },
      {
        "CreateDateTime",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionCreateDateTime)
      },
      {
        "CreateDateTimeOffset",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionCreateDateTimeOffset)
      },
      {
        "CreateTime",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionCreateTime)
      },
      {
        "AddYears",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateAdd)
      },
      {
        "AddMonths",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateAdd)
      },
      {
        "AddDays",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateAdd)
      },
      {
        "AddHours",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateAdd)
      },
      {
        "AddMinutes",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateAdd)
      },
      {
        "AddSeconds",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateAdd)
      },
      {
        "AddMilliseconds",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateAdd)
      },
      {
        "AddMicroseconds",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateAddKatmaiOrNewer)
      },
      {
        "AddNanoseconds",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateAddKatmaiOrNewer)
      },
      {
        "DiffYears",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateDiff)
      },
      {
        "DiffMonths",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateDiff)
      },
      {
        "DiffDays",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateDiff)
      },
      {
        "DiffHours",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateDiff)
      },
      {
        "DiffMinutes",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateDiff)
      },
      {
        "DiffSeconds",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateDiff)
      },
      {
        "DiffMilliseconds",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateDiff)
      },
      {
        "DiffMicroseconds",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateDiffKatmaiOrNewer)
      },
      {
        "DiffNanoseconds",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionDateDiffKatmaiOrNewer)
      },
      {
        "Concat",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleConcatFunction)
      },
      {
        "BitwiseAnd",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionBitwise)
      },
      {
        "BitwiseNot",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionBitwise)
      },
      {
        "BitwiseOr",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionBitwise)
      },
      {
        "BitwiseXor",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleCanonicalFunctionBitwise)
      }
    };

    private static Dictionary<string, string> InitializeFunctionNameToOperatorDictionary() => new Dictionary<string, string>(5, (IEqualityComparer<string>) StringComparer.Ordinal)
    {
      {
        "Concat",
        "+"
      },
      {
        "CONCAT",
        "+"
      },
      {
        "BitwiseAnd",
        "&"
      },
      {
        "BitwiseNot",
        "~"
      },
      {
        "BitwiseOr",
        "|"
      },
      {
        "BitwiseXor",
        "^"
      }
    };

    private static Dictionary<string, string> InitializeDateAddFunctionNameToDatepartDictionary() => new Dictionary<string, string>(5, (IEqualityComparer<string>) StringComparer.Ordinal)
    {
      {
        "AddYears",
        "year"
      },
      {
        "AddMonths",
        "month"
      },
      {
        "AddDays",
        "day"
      },
      {
        "AddHours",
        "hour"
      },
      {
        "AddMinutes",
        "minute"
      },
      {
        "AddSeconds",
        "second"
      },
      {
        "AddMilliseconds",
        "millisecond"
      },
      {
        "AddMicroseconds",
        "microsecond"
      },
      {
        "AddNanoseconds",
        "nanosecond"
      }
    };

    private static Dictionary<string, string> InitializeDateDiffFunctionNameToDatepartDictionary() => new Dictionary<string, string>(5, (IEqualityComparer<string>) StringComparer.Ordinal)
    {
      {
        "DiffYears",
        "year"
      },
      {
        "DiffMonths",
        "month"
      },
      {
        "DiffDays",
        "day"
      },
      {
        "DiffHours",
        "hour"
      },
      {
        "DiffMinutes",
        "minute"
      },
      {
        "DiffSeconds",
        "second"
      },
      {
        "DiffMilliseconds",
        "millisecond"
      },
      {
        "DiffMicroseconds",
        "microsecond"
      },
      {
        "DiffNanoseconds",
        "nanosecond"
      }
    };

    private static Dictionary<string, SqlFunctionCallHandler.FunctionHandler> InitializeHierarchyIdStaticMethodFunctionsDictionary() => new Dictionary<string, SqlFunctionCallHandler.FunctionHandler>()
    {
      {
        "HierarchyIdGetRoot",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "hierarchyid::GetRoot"))
      },
      {
        "HierarchyIdParse",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "hierarchyid::Parse"))
      }
    };

    private static Dictionary<string, SqlFunctionCallHandler.FunctionHandler> InitializeGeographyStaticMethodFunctionsDictionary() => new Dictionary<string, SqlFunctionCallHandler.FunctionHandler>()
    {
      {
        "GeographyFromText",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleSpatialFromTextFunction)
      },
      {
        "GeographyPointFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STPointFromText"))
      },
      {
        "GeographyLineFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STLineFromText"))
      },
      {
        "GeographyPolygonFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STPolyFromText"))
      },
      {
        "GeographyMultiPointFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STMPointFromText"))
      },
      {
        "GeographyMultiLineFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STMLineFromText"))
      },
      {
        "GeographyMultiPolygonFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STMPolyFromText"))
      },
      {
        "GeographyCollectionFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STGeomCollFromText"))
      },
      {
        "GeographyFromBinary",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleSpatialFromBinaryFunction)
      },
      {
        "GeographyPointFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STPointFromWKB"))
      },
      {
        "GeographyLineFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STLineFromWKB"))
      },
      {
        "GeographyPolygonFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STPolyFromWKB"))
      },
      {
        "GeographyMultiPointFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STMPointFromWKB"))
      },
      {
        "GeographyMultiLineFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STMLineFromWKB"))
      },
      {
        "GeographyMultiPolygonFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STMPolyFromWKB"))
      },
      {
        "GeographyCollectionFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geography::STGeomCollFromWKB"))
      },
      {
        "GeographyFromGml",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleSpatialFromGmlFunction)
      }
    };

    private static Dictionary<string, string> InitializeGeographyInstancePropertyFunctionsDictionary() => new Dictionary<string, string>()
    {
      {
        "CoordinateSystemId",
        "STSrid"
      },
      {
        "Latitude",
        "Lat"
      },
      {
        "Longitude",
        "Long"
      },
      {
        "Measure",
        "M"
      },
      {
        "Elevation",
        "Z"
      }
    };

    private static Dictionary<string, string> InitializeRenamedGeographyInstanceMethodFunctions() => new Dictionary<string, string>()
    {
      {
        "AsText",
        "STAsText"
      },
      {
        "AsBinary",
        "STAsBinary"
      },
      {
        "SpatialTypeName",
        "STGeometryType"
      },
      {
        "SpatialDimension",
        "STDimension"
      },
      {
        "IsEmptySpatial",
        "STIsEmpty"
      },
      {
        "SpatialEquals",
        "STEquals"
      },
      {
        "SpatialDisjoint",
        "STDisjoint"
      },
      {
        "SpatialIntersects",
        "STIntersects"
      },
      {
        "SpatialBuffer",
        "STBuffer"
      },
      {
        "Distance",
        "STDistance"
      },
      {
        "SpatialUnion",
        "STUnion"
      },
      {
        "SpatialIntersection",
        "STIntersection"
      },
      {
        "SpatialDifference",
        "STDifference"
      },
      {
        "SpatialSymmetricDifference",
        "STSymDifference"
      },
      {
        "SpatialElementCount",
        "STNumGeometries"
      },
      {
        "SpatialElementAt",
        "STGeometryN"
      },
      {
        "SpatialLength",
        "STLength"
      },
      {
        "StartPoint",
        "STStartPoint"
      },
      {
        "EndPoint",
        "STEndPoint"
      },
      {
        "IsClosedSpatial",
        "STIsClosed"
      },
      {
        "PointCount",
        "STNumPoints"
      },
      {
        "PointAt",
        "STPointN"
      },
      {
        "Area",
        "STArea"
      }
    };

    private static Dictionary<string, SqlFunctionCallHandler.FunctionHandler> InitializeGeometryStaticMethodFunctionsDictionary() => new Dictionary<string, SqlFunctionCallHandler.FunctionHandler>()
    {
      {
        "GeometryFromText",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleSpatialFromTextFunction)
      },
      {
        "GeometryPointFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STPointFromText"))
      },
      {
        "GeometryLineFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STLineFromText"))
      },
      {
        "GeometryPolygonFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STPolyFromText"))
      },
      {
        "GeometryMultiPointFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STMPointFromText"))
      },
      {
        "GeometryMultiLineFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STMLineFromText"))
      },
      {
        "GeometryMultiPolygonFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STMPolyFromText"))
      },
      {
        "GeometryCollectionFromText",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STGeomCollFromText"))
      },
      {
        "GeometryFromBinary",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleSpatialFromBinaryFunction)
      },
      {
        "GeometryPointFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STPointFromWKB"))
      },
      {
        "GeometryLineFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STLineFromWKB"))
      },
      {
        "GeometryPolygonFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STPolyFromWKB"))
      },
      {
        "GeometryMultiPointFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STMPointFromWKB"))
      },
      {
        "GeometryMultiLineFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STMLineFromWKB"))
      },
      {
        "GeometryMultiPolygonFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STMPolyFromWKB"))
      },
      {
        "GeometryCollectionFromBinary",
        (SqlFunctionCallHandler.FunctionHandler) ((sqlgen, functionExpression) => SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, "geometry::STGeomCollFromWKB"))
      },
      {
        "GeometryFromGml",
        new SqlFunctionCallHandler.FunctionHandler(SqlFunctionCallHandler.HandleSpatialFromGmlFunction)
      }
    };

    private static Dictionary<string, string> InitializeGeometryInstancePropertyFunctionsDictionary() => new Dictionary<string, string>()
    {
      {
        "CoordinateSystemId",
        "STSrid"
      },
      {
        "Measure",
        "M"
      },
      {
        "XCoordinate",
        "STX"
      },
      {
        "YCoordinate",
        "STY"
      },
      {
        "Elevation",
        "Z"
      }
    };

    private static Dictionary<string, string> InitializeRenamedGeometryInstanceMethodFunctions() => new Dictionary<string, string>()
    {
      {
        "AsText",
        "STAsText"
      },
      {
        "AsBinary",
        "STAsBinary"
      },
      {
        "SpatialTypeName",
        "STGeometryType"
      },
      {
        "SpatialDimension",
        "STDimension"
      },
      {
        "IsEmptySpatial",
        "STIsEmpty"
      },
      {
        "IsSimpleGeometry",
        "STIsSimple"
      },
      {
        "IsValidGeometry",
        "STIsValid"
      },
      {
        "SpatialBoundary",
        "STBoundary"
      },
      {
        "SpatialEnvelope",
        "STEnvelope"
      },
      {
        "SpatialEquals",
        "STEquals"
      },
      {
        "SpatialDisjoint",
        "STDisjoint"
      },
      {
        "SpatialIntersects",
        "STIntersects"
      },
      {
        "SpatialTouches",
        "STTouches"
      },
      {
        "SpatialCrosses",
        "STCrosses"
      },
      {
        "SpatialWithin",
        "STWithin"
      },
      {
        "SpatialContains",
        "STContains"
      },
      {
        "SpatialOverlaps",
        "STOverlaps"
      },
      {
        "SpatialRelate",
        "STRelate"
      },
      {
        "SpatialBuffer",
        "STBuffer"
      },
      {
        "SpatialConvexHull",
        "STConvexHull"
      },
      {
        "Distance",
        "STDistance"
      },
      {
        "SpatialUnion",
        "STUnion"
      },
      {
        "SpatialIntersection",
        "STIntersection"
      },
      {
        "SpatialDifference",
        "STDifference"
      },
      {
        "SpatialSymmetricDifference",
        "STSymDifference"
      },
      {
        "SpatialElementCount",
        "STNumGeometries"
      },
      {
        "SpatialElementAt",
        "STGeometryN"
      },
      {
        "SpatialLength",
        "STLength"
      },
      {
        "StartPoint",
        "STStartPoint"
      },
      {
        "EndPoint",
        "STEndPoint"
      },
      {
        "IsClosedSpatial",
        "STIsClosed"
      },
      {
        "IsRing",
        "STIsRing"
      },
      {
        "PointCount",
        "STNumPoints"
      },
      {
        "PointAt",
        "STPointN"
      },
      {
        "Area",
        "STArea"
      },
      {
        "Centroid",
        "STCentroid"
      },
      {
        "PointOnSurface",
        "STPointOnSurface"
      },
      {
        "ExteriorRing",
        "STExteriorRing"
      },
      {
        "InteriorRingCount",
        "STNumInteriorRing"
      },
      {
        "InteriorRingAt",
        "STInteriorRingN"
      }
    };

    private static ISqlFragment HandleSpatialFromTextFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression functionExpression)
    {
      string functionName1 = functionExpression.ResultType.IsPrimitiveType(PrimitiveTypeKind.Geometry) ? "geometry::STGeomFromText" : "geography::STGeomFromText";
      string functionName2 = functionExpression.ResultType.IsPrimitiveType(PrimitiveTypeKind.Geometry) ? "geometry::Parse" : "geography::Parse";
      return functionExpression.Arguments.Count == 2 ? SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, functionName1) : SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, functionName2);
    }

    private static ISqlFragment HandleSpatialFromGmlFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression functionExpression)
    {
      return SqlFunctionCallHandler.HandleSpatialStaticMethodFunctionAppendSrid(sqlgen, functionExpression, functionExpression.ResultType.IsPrimitiveType(PrimitiveTypeKind.Geometry) ? "geometry::GeomFromGml" : "geography::GeomFromGml");
    }

    private static ISqlFragment HandleSpatialFromBinaryFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression functionExpression)
    {
      return SqlFunctionCallHandler.HandleSpatialStaticMethodFunctionAppendSrid(sqlgen, functionExpression, functionExpression.ResultType.IsPrimitiveType(PrimitiveTypeKind.Geometry) ? "geometry::STGeomFromWKB" : "geography::STGeomFromWKB");
    }

    private static ISqlFragment HandleSpatialStaticMethodFunctionAppendSrid(
      SqlGenerator sqlgen,
      DbFunctionExpression functionExpression,
      string functionName)
    {
      if (functionExpression.Arguments.Count == 2)
        return SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, functionExpression, functionName);
      DbExpression dbExpression = functionExpression.ResultType.IsPrimitiveType(PrimitiveTypeKind.Geometry) ? SqlFunctionCallHandler._defaultGeometrySridExpression : SqlFunctionCallHandler._defaultGeographySridExpression;
      SqlBuilder result = new SqlBuilder();
      result.Append((object) functionName);
      SqlFunctionCallHandler.WriteFunctionArguments(sqlgen, functionExpression.Arguments.Concat<DbExpression>((IEnumerable<DbExpression>) new DbExpression[1]
      {
        dbExpression
      }), result);
      return (ISqlFragment) result;
    }

    internal static ISqlFragment GenerateFunctionCallSql(
      SqlGenerator sqlgen,
      DbFunctionExpression functionExpression)
    {
      if (SqlFunctionCallHandler.IsSpecialCanonicalFunction(functionExpression))
        return SqlFunctionCallHandler.HandleSpecialCanonicalFunction(sqlgen, functionExpression);
      if (SqlFunctionCallHandler.IsSpecialStoreFunction(functionExpression))
        return SqlFunctionCallHandler.HandleSpecialStoreFunction(sqlgen, functionExpression);
      PrimitiveTypeKind spatialTypeKind;
      if (SqlFunctionCallHandler.IsSpatialCanonicalFunction(functionExpression, out spatialTypeKind))
        return SqlFunctionCallHandler.HandleSpatialCanonicalFunction(sqlgen, functionExpression, spatialTypeKind);
      return SqlFunctionCallHandler.IsHierarchyCanonicalFunction(functionExpression) ? SqlFunctionCallHandler.HandleHierarchyIdCanonicalFunction(sqlgen, functionExpression) : SqlFunctionCallHandler.HandleFunctionDefault(sqlgen, functionExpression);
    }

    private static bool IsSpecialStoreFunction(DbFunctionExpression e) => SqlFunctionCallHandler.IsStoreFunction(e.Function) && SqlFunctionCallHandler._storeFunctionHandlers.ContainsKey(e.Function.Name);

    private static bool IsSpecialCanonicalFunction(DbFunctionExpression e) => e.Function.IsCanonicalFunction() && SqlFunctionCallHandler._canonicalFunctionHandlers.ContainsKey(e.Function.Name);

    private static bool IsHierarchyCanonicalFunction(DbFunctionExpression e)
    {
      if (e.Function.IsCanonicalFunction())
      {
        if (e.ResultType.IsHierarchyIdType())
          return true;
        foreach (FunctionParameter parameter in e.Function.Parameters)
        {
          if (parameter.TypeUsage.IsHierarchyIdType())
            return true;
        }
      }
      return false;
    }

    private static bool IsSpatialCanonicalFunction(
      DbFunctionExpression e,
      out PrimitiveTypeKind spatialTypeKind)
    {
      if (e.Function.IsCanonicalFunction())
      {
        if (e.ResultType.IsSpatialType(out spatialTypeKind))
          return true;
        foreach (FunctionParameter parameter in e.Function.Parameters)
        {
          if (parameter.TypeUsage.IsSpatialType(out spatialTypeKind))
            return true;
        }
      }
      spatialTypeKind = PrimitiveTypeKind.Binary;
      return false;
    }

    private static ISqlFragment HandleFunctionDefault(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, e, (string) null);
    }

    private static ISqlFragment HandleFunctionDefaultGivenName(
      SqlGenerator sqlgen,
      DbFunctionExpression e,
      string functionName)
    {
      if (SqlFunctionCallHandler.CastReturnTypeToInt64(e))
        return SqlFunctionCallHandler.HandleFunctionDefaultCastReturnValue(sqlgen, e, functionName, "bigint");
      if (SqlFunctionCallHandler.CastReturnTypeToInt32(sqlgen, e))
        return SqlFunctionCallHandler.HandleFunctionDefaultCastReturnValue(sqlgen, e, functionName, "int");
      if (SqlFunctionCallHandler.CastReturnTypeToInt16(e))
        return SqlFunctionCallHandler.HandleFunctionDefaultCastReturnValue(sqlgen, e, functionName, "smallint");
      return SqlFunctionCallHandler.CastReturnTypeToSingle(e) ? SqlFunctionCallHandler.HandleFunctionDefaultCastReturnValue(sqlgen, e, functionName, "real") : SqlFunctionCallHandler.HandleFunctionDefaultCastReturnValue(sqlgen, e, functionName, (string) null);
    }

    private static ISqlFragment HandleFunctionDefaultCastReturnValue(
      SqlGenerator sqlgen,
      DbFunctionExpression e,
      string functionName,
      string returnType)
    {
      return SqlFunctionCallHandler.WrapWithCast(returnType, (Action<SqlBuilder>) (result =>
      {
        if (functionName == null)
          SqlFunctionCallHandler.WriteFunctionName(result, e.Function);
        else
          result.Append((object) functionName);
        SqlFunctionCallHandler.HandleFunctionArgumentsDefault(sqlgen, e, result);
      }));
    }

    private static ISqlFragment WrapWithCast(
      string returnType,
      Action<SqlBuilder> toWrap)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      if (returnType != null)
        sqlBuilder.Append((object) " CAST(");
      toWrap(sqlBuilder);
      if (returnType != null)
      {
        sqlBuilder.Append((object) " AS ");
        sqlBuilder.Append((object) returnType);
        sqlBuilder.Append((object) ")");
      }
      return (ISqlFragment) sqlBuilder;
    }

    private static void HandleFunctionArgumentsDefault(
      SqlGenerator sqlgen,
      DbFunctionExpression e,
      SqlBuilder result)
    {
      int num = e.Function.NiladicFunctionAttribute ? 1 : 0;
      if (num != 0 && e.Arguments.Count > 0)
        throw new MetadataException(System.Data.Entity.SqlServer.Resources.Strings.SqlGen_NiladicFunctionsCannotHaveParameters);
      if (num != 0)
        return;
      SqlFunctionCallHandler.WriteFunctionArguments(sqlgen, (IEnumerable<DbExpression>) e.Arguments, result);
    }

    private static void WriteFunctionArguments(
      SqlGenerator sqlgen,
      IEnumerable<DbExpression> functionArguments,
      SqlBuilder result)
    {
      result.Append((object) "(");
      string str = "";
      foreach (DbExpression functionArgument in functionArguments)
      {
        result.Append((object) str);
        result.Append((object) functionArgument.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
        str = ", ";
      }
      result.Append((object) ")");
    }

    private static ISqlFragment HandleFunctionGivenNameBasedOnVersion(
      SqlGenerator sqlgen,
      DbFunctionExpression e,
      string preKatmaiName,
      string katmaiName)
    {
      return sqlgen.IsPreKatmai ? SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, e, preKatmaiName) : SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, e, katmaiName);
    }

    private static ISqlFragment HandleSpecialStoreFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleSpecialFunction(SqlFunctionCallHandler._storeFunctionHandlers, sqlgen, e);
    }

    private static ISqlFragment HandleHierarchyIdCanonicalFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression functionExpression)
    {
      SqlFunctionCallHandler.FunctionHandler functionHandler;
      if (SqlFunctionCallHandler._hierarchyIdFunctionNameToStaticMethodHandlerDictionary.TryGetValue(functionExpression.Function.Name, out functionHandler))
        return functionHandler(sqlgen, functionExpression);
      string name = functionExpression.Function.Name;
      return SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, name, functionExpression, false);
    }

    private static ISqlFragment HandleSpecialCanonicalFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleSpecialFunction(SqlFunctionCallHandler._canonicalFunctionHandlers, sqlgen, e);
    }

    private static ISqlFragment HandleSpecialFunction(
      Dictionary<string, SqlFunctionCallHandler.FunctionHandler> handlers,
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return handlers[e.Function.Name](sqlgen, e);
    }

    private static ISqlFragment HandleSpatialCanonicalFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression functionExpression,
      PrimitiveTypeKind spatialTypeKind)
    {
      return spatialTypeKind == PrimitiveTypeKind.Geography ? SqlFunctionCallHandler.HandleSpatialCanonicalFunction(sqlgen, functionExpression, SqlFunctionCallHandler._geographyFunctionNameToStaticMethodHandlerDictionary, SqlFunctionCallHandler._geographyFunctionNameToInstancePropertyNameDictionary, SqlFunctionCallHandler._geographyRenamedInstanceMethodFunctionDictionary) : SqlFunctionCallHandler.HandleSpatialCanonicalFunction(sqlgen, functionExpression, SqlFunctionCallHandler._geometryFunctionNameToStaticMethodHandlerDictionary, SqlFunctionCallHandler._geometryFunctionNameToInstancePropertyNameDictionary, SqlFunctionCallHandler._geometryRenamedInstanceMethodFunctionDictionary);
    }

    private static ISqlFragment HandleSpatialCanonicalFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression functionExpression,
      Dictionary<string, SqlFunctionCallHandler.FunctionHandler> staticMethodsMap,
      Dictionary<string, string> instancePropertiesMap,
      Dictionary<string, string> renamedInstanceMethodsMap)
    {
      SqlFunctionCallHandler.FunctionHandler functionHandler;
      if (staticMethodsMap.TryGetValue(functionExpression.Function.Name, out functionHandler))
        return functionHandler(sqlgen, functionExpression);
      string functionName;
      if (instancePropertiesMap.TryGetValue(functionExpression.Function.Name, out functionName))
        return SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, functionName, functionExpression, true, (string) null);
      string name;
      if (!renamedInstanceMethodsMap.TryGetValue(functionExpression.Function.Name, out name))
        name = functionExpression.Function.Name;
      string castReturnTypeTo = (string) null;
      if (name == "AsGml")
        castReturnTypeTo = "nvarchar(max)";
      return SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, name, functionExpression, false, castReturnTypeTo);
    }

    private static ISqlFragment WriteInstanceFunctionCall(
      SqlGenerator sqlgen,
      string functionName,
      DbFunctionExpression functionExpression,
      bool isPropertyAccess)
    {
      return SqlFunctionCallHandler.WriteInstanceFunctionCall(sqlgen, functionName, functionExpression, isPropertyAccess, (string) null);
    }

    private static ISqlFragment WriteInstanceFunctionCall(
      SqlGenerator sqlgen,
      string functionName,
      DbFunctionExpression functionExpression,
      bool isPropertyAccess,
      string castReturnTypeTo)
    {
      return SqlFunctionCallHandler.WrapWithCast(castReturnTypeTo, (Action<SqlBuilder>) (result =>
      {
        DbExpression e = functionExpression.Arguments[0];
        if (e.ExpressionKind != DbExpressionKind.Function)
          sqlgen.ParenthesizeExpressionIfNeeded(e, result);
        else
          result.Append((object) e.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
        result.Append((object) ".");
        result.Append((object) functionName);
        if (isPropertyAccess)
          return;
        SqlFunctionCallHandler.WriteFunctionArguments(sqlgen, functionExpression.Arguments.Skip<DbExpression>(1), result);
      }));
    }

    private static ISqlFragment HandleSpecialFunctionToOperator(
      SqlGenerator sqlgen,
      DbFunctionExpression e,
      bool parenthesizeArguments)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      if (e.Arguments.Count > 1)
      {
        if (parenthesizeArguments)
          sqlBuilder.Append((object) "(");
        sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
        if (parenthesizeArguments)
          sqlBuilder.Append((object) ")");
      }
      sqlBuilder.Append((object) " ");
      sqlBuilder.Append((object) SqlFunctionCallHandler._functionNameToOperatorDictionary[e.Function.Name]);
      sqlBuilder.Append((object) " ");
      if (parenthesizeArguments)
        sqlBuilder.Append((object) "(");
      sqlBuilder.Append((object) e.Arguments[e.Arguments.Count - 1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      if (parenthesizeArguments)
        sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleConcatFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleSpecialFunctionToOperator(sqlgen, e, false);
    }

    private static ISqlFragment HandleCanonicalFunctionBitwise(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleSpecialFunctionToOperator(sqlgen, e, true);
    }

    internal static ISqlFragment HandleDatepartDateFunction(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      if (!(e.Arguments[0] is DbConstantExpression constantExpression))
        throw new InvalidOperationException(System.Data.Entity.SqlServer.Resources.Strings.SqlGen_InvalidDatePartArgumentExpression((object) e.Function.NamespaceName, (object) e.Function.Name));
      if (!(constantExpression.Value is string str))
        throw new InvalidOperationException(System.Data.Entity.SqlServer.Resources.Strings.SqlGen_InvalidDatePartArgumentExpression((object) e.Function.NamespaceName, (object) e.Function.Name));
      if (!SqlFunctionCallHandler._datepartKeywords.Contains(str))
        throw new InvalidOperationException(System.Data.Entity.SqlServer.Resources.Strings.SqlGen_InvalidDatePartArgumentValue((object) str, (object) e.Function.NamespaceName, (object) e.Function.Name));
      SqlBuilder result = new SqlBuilder();
      SqlFunctionCallHandler.WriteFunctionName(result, e.Function);
      result.Append((object) "(");
      result.Append((object) str);
      for (int index = 1; index < e.Arguments.Count; ++index)
      {
        result.Append((object) ", ");
        result.Append((object) e.Arguments[index].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      }
      result.Append((object) ")");
      return (ISqlFragment) result;
    }

    private static ISqlFragment HandleCanonicalFunctionDatepart(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleCanonicalFunctionDatepart(sqlgen, e.Function.Name.ToLowerInvariant(), e);
    }

    private static ISqlFragment HandleCanonicalFunctionGetTotalOffsetMinutes(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleCanonicalFunctionDatepart(sqlgen, "tzoffset", e);
    }

    private static ISqlFragment HandleCanonicalFunctionLocalDateTime(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      sqlgen.AssertKatmaiOrNewer(e);
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "CAST (");
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) " AS DATETIME2)");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionUtcDateTime(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      sqlgen.AssertKatmaiOrNewer(e);
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "CONVERT (DATETIME2, ");
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ", 1)");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionDatepart(
      SqlGenerator sqlgen,
      string datepart,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "DATEPART (");
      sqlBuilder.Append((object) datepart);
      sqlBuilder.Append((object) ", ");
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionCurrentDateTime(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleFunctionGivenNameBasedOnVersion(sqlgen, e, "GetDate", "SysDateTime");
    }

    private static ISqlFragment HandleCanonicalFunctionCurrentUtcDateTime(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleFunctionGivenNameBasedOnVersion(sqlgen, e, "GetUtcDate", "SysUtcDateTime");
    }

    private static ISqlFragment HandleCanonicalFunctionCurrentDateTimeOffset(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      sqlgen.AssertKatmaiOrNewer(e);
      return SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, e, "SysDateTimeOffset");
    }

    private static ISqlFragment HandleCanonicalFunctionCreateDateTime(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      string typeName = sqlgen.IsPreKatmai ? "datetime" : "datetime2";
      return SqlFunctionCallHandler.HandleCanonicalFunctionDateTimeTypeCreation(sqlgen, typeName, e.Arguments, true, false);
    }

    private static ISqlFragment HandleCanonicalFunctionCreateDateTimeOffset(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      sqlgen.AssertKatmaiOrNewer(e);
      return SqlFunctionCallHandler.HandleCanonicalFunctionDateTimeTypeCreation(sqlgen, "datetimeoffset", e.Arguments, true, true);
    }

    private static ISqlFragment HandleCanonicalFunctionCreateTime(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      sqlgen.AssertKatmaiOrNewer(e);
      return SqlFunctionCallHandler.HandleCanonicalFunctionDateTimeTypeCreation(sqlgen, "time", e.Arguments, false, false);
    }

    private static ISqlFragment HandleCanonicalFunctionDateTimeTypeCreation(
      SqlGenerator sqlgen,
      string typeName,
      IList<DbExpression> args,
      bool hasDatePart,
      bool hasTimeZonePart)
    {
      SqlBuilder result1 = new SqlBuilder();
      int num1 = 0;
      result1.Append((object) "convert (");
      result1.Append((object) typeName);
      result1.Append((object) ",");
      if (hasDatePart)
      {
        result1.Append((object) "right('000' + ");
        SqlGenerator sqlgen1 = sqlgen;
        SqlBuilder result2 = result1;
        IList<DbExpression> dbExpressionList1 = args;
        int index1 = num1;
        int num2 = index1 + 1;
        DbExpression e1 = dbExpressionList1[index1];
        SqlFunctionCallHandler.AppendConvertToVarchar(sqlgen1, result2, e1);
        result1.Append((object) ", 4)");
        result1.Append((object) " + '-' + ");
        SqlGenerator sqlgen2 = sqlgen;
        SqlBuilder result3 = result1;
        IList<DbExpression> dbExpressionList2 = args;
        int index2 = num2;
        int num3 = index2 + 1;
        DbExpression e2 = dbExpressionList2[index2];
        SqlFunctionCallHandler.AppendConvertToVarchar(sqlgen2, result3, e2);
        result1.Append((object) " + '-' + ");
        SqlGenerator sqlgen3 = sqlgen;
        SqlBuilder result4 = result1;
        IList<DbExpression> dbExpressionList3 = args;
        int index3 = num3;
        num1 = index3 + 1;
        DbExpression e3 = dbExpressionList3[index3];
        SqlFunctionCallHandler.AppendConvertToVarchar(sqlgen3, result4, e3);
        result1.Append((object) " + ' ' + ");
      }
      SqlGenerator sqlgen4 = sqlgen;
      SqlBuilder result5 = result1;
      IList<DbExpression> dbExpressionList4 = args;
      int index4 = num1;
      int num4 = index4 + 1;
      DbExpression e4 = dbExpressionList4[index4];
      SqlFunctionCallHandler.AppendConvertToVarchar(sqlgen4, result5, e4);
      result1.Append((object) " + ':' + ");
      SqlGenerator sqlgen5 = sqlgen;
      SqlBuilder result6 = result1;
      IList<DbExpression> dbExpressionList5 = args;
      int index5 = num4;
      int num5 = index5 + 1;
      DbExpression e5 = dbExpressionList5[index5];
      SqlFunctionCallHandler.AppendConvertToVarchar(sqlgen5, result6, e5);
      result1.Append((object) " + ':' + str(");
      SqlBuilder sqlBuilder = result1;
      IList<DbExpression> dbExpressionList6 = args;
      int index6 = num5;
      int index7 = index6 + 1;
      ISqlFragment sqlFragment = dbExpressionList6[index6].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen);
      sqlBuilder.Append((object) sqlFragment);
      if (sqlgen.IsPreKatmai)
        result1.Append((object) ", 6, 3)");
      else
        result1.Append((object) ", 10, 7)");
      if (hasTimeZonePart)
      {
        result1.Append((object) " + (CASE WHEN ");
        sqlgen.ParenthesizeExpressionIfNeeded(args[index7], result1);
        result1.Append((object) " >= 0 THEN '+' ELSE '-' END) + convert(varchar(255), ABS(");
        sqlgen.ParenthesizeExpressionIfNeeded(args[index7], result1);
        result1.Append((object) "/60)) + ':' + convert(varchar(255), ABS(");
        sqlgen.ParenthesizeExpressionIfNeeded(args[index7], result1);
        result1.Append((object) "%60))");
      }
      result1.Append((object) ", 121)");
      return (ISqlFragment) result1;
    }

    private static void AppendConvertToVarchar(
      SqlGenerator sqlgen,
      SqlBuilder result,
      DbExpression e)
    {
      result.Append((object) "convert(varchar(255), ");
      result.Append((object) e.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      result.Append((object) ")");
    }

    private static ISqlFragment HandleCanonicalFunctionTruncateTime(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      bool flag = e.Arguments[0].ResultType.GetPrimitiveTypeKind() == PrimitiveTypeKind.DateTimeOffset;
      if (sqlgen.IsPreKatmai & flag)
        throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.SqlGen_CanonicalFunctionNotSupportedPriorSql10((object) e.Function.Name));
      SqlBuilder sqlBuilder = new SqlBuilder();
      ISqlFragment sqlFragment = e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen);
      if (sqlgen.IsPreKatmai)
      {
        sqlBuilder.Append((object) "dateadd(d, datediff(d, 0, ");
        sqlBuilder.Append((object) sqlFragment);
        sqlBuilder.Append((object) "), 0)");
      }
      else if (!flag)
      {
        sqlBuilder.Append((object) "cast(cast(");
        sqlBuilder.Append((object) sqlFragment);
        sqlBuilder.Append((object) " as date) as datetime2)");
      }
      else
      {
        sqlBuilder.Append((object) "todatetimeoffset(cast(");
        sqlBuilder.Append((object) sqlFragment);
        sqlBuilder.Append((object) " as date), datepart(tz, ");
        sqlBuilder.Append((object) sqlFragment);
        sqlBuilder.Append((object) "))");
      }
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionDateAddKatmaiOrNewer(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      sqlgen.AssertKatmaiOrNewer(e);
      return SqlFunctionCallHandler.HandleCanonicalFunctionDateAdd(sqlgen, e);
    }

    private static ISqlFragment HandleCanonicalFunctionDateAdd(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "DATEADD (");
      sqlBuilder.Append((object) SqlFunctionCallHandler._dateAddFunctionNameToDatepartDictionary[e.Function.Name]);
      sqlBuilder.Append((object) ", ");
      sqlBuilder.Append((object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ", ");
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionDateDiffKatmaiOrNewer(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      sqlgen.AssertKatmaiOrNewer(e);
      return SqlFunctionCallHandler.HandleCanonicalFunctionDateDiff(sqlgen, e);
    }

    private static ISqlFragment HandleCanonicalFunctionDateDiff(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "DATEDIFF (");
      sqlBuilder.Append((object) SqlFunctionCallHandler._dateDiffFunctionNameToDatepartDictionary[e.Function.Name]);
      sqlBuilder.Append((object) ", ");
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ", ");
      sqlBuilder.Append((object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ")");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionIndexOf(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, e, "CHARINDEX");
    }

    private static ISqlFragment HandleCanonicalFunctionNewGuid(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, e, "NEWID");
    }

    private static ISqlFragment HandleCanonicalFunctionLength(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, e, "LEN");
    }

    private static ISqlFragment HandleCanonicalFunctionRound(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleCanonicalFunctionRoundOrTruncate(sqlgen, e, true);
    }

    private static ISqlFragment HandleCanonicalFunctionTruncate(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleCanonicalFunctionRoundOrTruncate(sqlgen, e, false);
    }

    private static ISqlFragment HandleCanonicalFunctionRoundOrTruncate(
      SqlGenerator sqlgen,
      DbFunctionExpression e,
      bool round)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      bool flag = false;
      if (e.Arguments.Count == 1)
      {
        flag = SqlFunctionCallHandler.CastReturnTypeToSingle(e);
        if (flag)
          sqlBuilder.Append((object) " CAST(");
      }
      sqlBuilder.Append((object) "ROUND(");
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) ", ");
      if (e.Arguments.Count > 1)
        sqlBuilder.Append((object) e.Arguments[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      else
        sqlBuilder.Append((object) "0");
      if (!round)
        sqlBuilder.Append((object) ", 1");
      sqlBuilder.Append((object) ")");
      if (flag)
        sqlBuilder.Append((object) " AS real)");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionAbs(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      if (!e.Arguments[0].ResultType.IsPrimitiveType(PrimitiveTypeKind.Byte))
        return SqlFunctionCallHandler.HandleFunctionDefault(sqlgen, e);
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionTrim(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder = new SqlBuilder();
      sqlBuilder.Append((object) "LTRIM(RTRIM(");
      sqlBuilder.Append((object) e.Arguments[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      sqlBuilder.Append((object) "))");
      return (ISqlFragment) sqlBuilder;
    }

    private static ISqlFragment HandleCanonicalFunctionToLower(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, e, "LOWER");
    }

    private static ISqlFragment HandleCanonicalFunctionToUpper(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.HandleFunctionDefaultGivenName(sqlgen, e, "UPPER");
    }

    private static void TranslateConstantParameterForLike(
      SqlGenerator sqlgen,
      DbExpression targetExpression,
      DbConstantExpression constSearchParamExpression,
      SqlBuilder result,
      bool insertPercentStart,
      bool insertPercentEnd)
    {
      result.Append((object) targetExpression.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      result.Append((object) " LIKE ");
      StringBuilder stringBuilder = new StringBuilder();
      if (insertPercentStart)
        stringBuilder.Append("%");
      bool usedEscapeChar;
      stringBuilder.Append(SqlProviderManifest.EscapeLikeText(constSearchParamExpression.Value as string, false, out usedEscapeChar));
      if (insertPercentEnd)
        stringBuilder.Append("%");
      DbConstantExpression constantExpression = constSearchParamExpression.ResultType.Constant((object) stringBuilder.ToString());
      result.Append((object) constantExpression.Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
      if (!usedEscapeChar)
        return;
      result.Append((object) " ESCAPE '~'");
    }

    private static ISqlFragment HandleCanonicalFunctionContains(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.WrapPredicate(new Func<SqlGenerator, IList<DbExpression>, SqlBuilder, SqlBuilder>(SqlFunctionCallHandler.HandleCanonicalFunctionContains), sqlgen, e);
    }

    private static SqlBuilder HandleCanonicalFunctionContains(
      SqlGenerator sqlgen,
      IList<DbExpression> args,
      SqlBuilder result)
    {
      if (args[1] is DbConstantExpression constSearchParamExpression && !string.IsNullOrEmpty(constSearchParamExpression.Value as string))
      {
        SqlFunctionCallHandler.TranslateConstantParameterForLike(sqlgen, args[0], constSearchParamExpression, result, true, true);
      }
      else
      {
        result.Append((object) "CHARINDEX( ");
        result.Append((object) args[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
        result.Append((object) ", ");
        result.Append((object) args[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
        result.Append((object) ") > 0");
      }
      return result;
    }

    private static ISqlFragment HandleCanonicalFunctionStartsWith(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.WrapPredicate(new Func<SqlGenerator, IList<DbExpression>, SqlBuilder, SqlBuilder>(SqlFunctionCallHandler.HandleCanonicalFunctionStartsWith), sqlgen, e);
    }

    private static SqlBuilder HandleCanonicalFunctionStartsWith(
      SqlGenerator sqlgen,
      IList<DbExpression> args,
      SqlBuilder result)
    {
      if (args[1] is DbConstantExpression constSearchParamExpression && !string.IsNullOrEmpty(constSearchParamExpression.Value as string))
      {
        SqlFunctionCallHandler.TranslateConstantParameterForLike(sqlgen, args[0], constSearchParamExpression, result, false, true);
      }
      else
      {
        result.Append((object) "CHARINDEX( ");
        result.Append((object) args[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
        result.Append((object) ", ");
        result.Append((object) args[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
        result.Append((object) ") = 1");
      }
      return result;
    }

    private static ISqlFragment HandleCanonicalFunctionEndsWith(
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      return SqlFunctionCallHandler.WrapPredicate(new Func<SqlGenerator, IList<DbExpression>, SqlBuilder, SqlBuilder>(SqlFunctionCallHandler.HandleCanonicalFunctionEndsWith), sqlgen, e);
    }

    private static SqlBuilder HandleCanonicalFunctionEndsWith(
      SqlGenerator sqlgen,
      IList<DbExpression> args,
      SqlBuilder result)
    {
      DbConstantExpression constSearchParamExpression = args[1] as DbConstantExpression;
      DbPropertyExpression propertyExpression = args[0] as DbPropertyExpression;
      if (constSearchParamExpression != null && propertyExpression != null && !string.IsNullOrEmpty(constSearchParamExpression.Value as string))
      {
        SqlFunctionCallHandler.TranslateConstantParameterForLike(sqlgen, args[0], constSearchParamExpression, result, true, false);
      }
      else
      {
        result.Append((object) "CHARINDEX( REVERSE(");
        result.Append((object) args[1].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
        result.Append((object) "), REVERSE(");
        result.Append((object) args[0].Accept<ISqlFragment>((DbExpressionVisitor<ISqlFragment>) sqlgen));
        result.Append((object) ")) = 1");
      }
      return result;
    }

    private static ISqlFragment WrapPredicate(
      Func<SqlGenerator, IList<DbExpression>, SqlBuilder, SqlBuilder> predicateTranslator,
      SqlGenerator sqlgen,
      DbFunctionExpression e)
    {
      SqlBuilder sqlBuilder1 = new SqlBuilder();
      sqlBuilder1.Append((object) "CASE WHEN (");
      SqlBuilder sqlBuilder2 = predicateTranslator(sqlgen, e.Arguments, sqlBuilder1);
      sqlBuilder1.Append((object) ") THEN cast(1 as bit) WHEN ( NOT (");
      SqlBuilder sqlBuilder3 = predicateTranslator(sqlgen, e.Arguments, sqlBuilder1);
      sqlBuilder1.Append((object) ")) THEN cast(0 as bit) END");
      return (ISqlFragment) sqlBuilder1;
    }

    internal static void WriteFunctionName(SqlBuilder result, EdmFunction function)
    {
      string name = function.StoreFunctionNameAttribute == null ? function.Name : function.StoreFunctionNameAttribute;
      if (function.IsCanonicalFunction())
        result.Append((object) name.ToUpperInvariant());
      else if (SqlFunctionCallHandler.IsStoreFunction(function))
      {
        result.Append((object) name);
      }
      else
      {
        if (string.IsNullOrEmpty(function.Schema))
          result.Append((object) SqlGenerator.QuoteIdentifier(function.NamespaceName));
        else
          result.Append((object) SqlGenerator.QuoteIdentifier(function.Schema));
        result.Append((object) ".");
        result.Append((object) SqlGenerator.QuoteIdentifier(name));
      }
    }

    internal static bool IsStoreFunction(EdmFunction function) => function.BuiltInAttribute && !function.IsCanonicalFunction();

    internal static bool CastReturnTypeToInt64(DbFunctionExpression e) => SqlFunctionCallHandler.CastReturnTypeToGivenType(e, SqlFunctionCallHandler._functionRequiresReturnTypeCastToInt64, PrimitiveTypeKind.Int64);

    internal static bool CastReturnTypeToInt32(SqlGenerator sqlgen, DbFunctionExpression e) => SqlFunctionCallHandler._functionRequiresReturnTypeCastToInt32.Contains(e.Function.FullName) && e.Arguments.Select<DbExpression, TypeUsage>((Func<DbExpression, TypeUsage>) (t => sqlgen.StoreItemCollection.ProviderManifest.GetStoreType(t.ResultType))).Any<TypeUsage>((Func<TypeUsage, bool>) (storeType => SqlFunctionCallHandler._maxTypeNames.Contains(storeType.EdmType.Name)));

    internal static bool CastReturnTypeToInt16(DbFunctionExpression e) => SqlFunctionCallHandler.CastReturnTypeToGivenType(e, SqlFunctionCallHandler._functionRequiresReturnTypeCastToInt16, PrimitiveTypeKind.Int16);

    internal static bool CastReturnTypeToSingle(DbFunctionExpression e) => SqlFunctionCallHandler.CastReturnTypeToGivenType(e, SqlFunctionCallHandler._functionRequiresReturnTypeCastToSingle, PrimitiveTypeKind.Single);

    private static bool CastReturnTypeToGivenType(
      DbFunctionExpression e,
      ISet<string> functionsRequiringReturnTypeCast,
      PrimitiveTypeKind type)
    {
      return functionsRequiringReturnTypeCast.Contains(e.Function.FullName) && e.Arguments.Any<DbExpression>((Func<DbExpression, bool>) (t => t.ResultType.IsPrimitiveType(type)));
    }

    private delegate ISqlFragment FunctionHandler(
      SqlGenerator sqlgen,
      DbFunctionExpression functionExpr);
  }
}

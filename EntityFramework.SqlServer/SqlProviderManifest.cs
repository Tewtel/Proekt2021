// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlProviderManifest
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.SqlServer.Utilities;
using System.Linq;
using System.Text;
using System.Xml;

namespace System.Data.Entity.SqlServer
{
  internal class SqlProviderManifest : DbXmlEnabledProviderManifest
  {
    internal const string TokenSql8 = "2000";
    internal const string TokenSql9 = "2005";
    internal const string TokenSql10 = "2008";
    internal const string TokenSql11 = "2012";
    internal const string TokenAzure11 = "2012.Azure";
    internal const char LikeEscapeChar = '~';
    internal const string LikeEscapeCharToString = "~";
    private readonly SqlVersion _version = SqlVersion.Sql9;
    private const int varcharMaxSize = 8000;
    private const int nvarcharMaxSize = 4000;
    private const int binaryMaxSize = 8000;
    private ReadOnlyCollection<PrimitiveType> _primitiveTypes;
    private ReadOnlyCollection<EdmFunction> _functions;

    public SqlProviderManifest(string manifestToken)
      : base(SqlProviderManifest.GetProviderManifest())
    {
      this._version = SqlVersionUtils.GetSqlVersion(manifestToken);
      this.Initialize();
    }

    private void Initialize()
    {
      if (this._version == SqlVersion.Sql10 || this._version == SqlVersion.Sql11)
      {
        this._primitiveTypes = base.GetStoreTypes();
        this._functions = base.GetStoreFunctions();
      }
      else
      {
        List<PrimitiveType> primitiveTypeList = new List<PrimitiveType>((IEnumerable<PrimitiveType>) base.GetStoreTypes());
        primitiveTypeList.RemoveAll((Predicate<PrimitiveType>) (primitiveType => primitiveType.Name.Equals("time", StringComparison.OrdinalIgnoreCase) || primitiveType.Name.Equals("date", StringComparison.OrdinalIgnoreCase) || (primitiveType.Name.Equals("datetime2", StringComparison.OrdinalIgnoreCase) || primitiveType.Name.Equals("datetimeoffset", StringComparison.OrdinalIgnoreCase)) || (primitiveType.Name.Equals("hierarchyid", StringComparison.OrdinalIgnoreCase) || primitiveType.Name.Equals("geography", StringComparison.OrdinalIgnoreCase)) || primitiveType.Name.Equals("geometry", StringComparison.OrdinalIgnoreCase)));
        if (this._version == SqlVersion.Sql8)
          primitiveTypeList.RemoveAll((Predicate<PrimitiveType>) (primitiveType => primitiveType.Name.Equals("xml", StringComparison.OrdinalIgnoreCase) || primitiveType.Name.EndsWith("(max)", StringComparison.OrdinalIgnoreCase)));
        this._primitiveTypes = new ReadOnlyCollection<PrimitiveType>((IList<PrimitiveType>) primitiveTypeList);
        IEnumerable<EdmFunction> source = base.GetStoreFunctions().Where<EdmFunction>((Func<EdmFunction, bool>) (f => !SqlProviderManifest.IsKatmaiOrNewer(f)));
        if (this._version == SqlVersion.Sql8)
          source = source.Where<EdmFunction>((Func<EdmFunction, bool>) (f => !SqlProviderManifest.IsYukonOrNewer(f)));
        this._functions = new ReadOnlyCollection<EdmFunction>((IList<EdmFunction>) source.ToList<EdmFunction>());
      }
    }

    internal SqlVersion SqlVersion => this._version;

    private static XmlReader GetXmlResource(string resourceName) => XmlReader.Create(typeof (SqlProviderManifest).Assembly().GetManifestResourceStream(resourceName));

    internal static XmlReader GetProviderManifest() => SqlProviderManifest.GetXmlResource("System.Data.Resources.SqlClient.SqlProviderServices.ProviderManifest.xml");

    internal static XmlReader GetStoreSchemaMapping(string mslName) => SqlProviderManifest.GetXmlResource("System.Data.Resources.SqlClient.SqlProviderServices." + mslName + ".msl");

    internal XmlReader GetStoreSchemaDescription(string ssdlName) => this._version == SqlVersion.Sql8 ? SqlProviderManifest.GetXmlResource("System.Data.Resources.SqlClient.SqlProviderServices." + ssdlName + "_Sql8.ssdl") : SqlProviderManifest.GetXmlResource("System.Data.Resources.SqlClient.SqlProviderServices." + ssdlName + ".ssdl");

    internal static string EscapeLikeText(
      string text,
      bool alwaysEscapeEscapeChar,
      out bool usedEscapeChar)
    {
      usedEscapeChar = false;
      if (!text.Contains("%") && !text.Contains("_") && (!text.Contains("[") && !text.Contains("^")) && (!alwaysEscapeEscapeChar || !text.Contains("~")))
        return text;
      StringBuilder stringBuilder = new StringBuilder(text.Length);
      foreach (char ch in text)
      {
        switch (ch)
        {
          case '%':
          case '[':
          case '^':
          case '_':
          case '~':
            stringBuilder.Append('~');
            usedEscapeChar = true;
            break;
        }
        stringBuilder.Append(ch);
      }
      return stringBuilder.ToString();
    }

    protected override XmlReader GetDbInformation(string informationType)
    {
      if (informationType == "StoreSchemaDefinitionVersion3" || informationType == "StoreSchemaDefinition")
        return this.GetStoreSchemaDescription(informationType);
      if (informationType == "StoreSchemaMappingVersion3" || informationType == "StoreSchemaMapping")
        return SqlProviderManifest.GetStoreSchemaMapping(informationType);
      if (informationType == "ConceptualSchemaDefinitionVersion3" || informationType == "ConceptualSchemaDefinition")
        return (XmlReader) null;
      throw new ProviderIncompatibleException(System.Data.Entity.SqlServer.Resources.Strings.ProviderReturnedNullForGetDbInformation((object) informationType));
    }

    public override ReadOnlyCollection<PrimitiveType> GetStoreTypes() => this._primitiveTypes;

    public override ReadOnlyCollection<EdmFunction> GetStoreFunctions() => this._functions;

    private static bool IsKatmaiOrNewer(EdmFunction edmFunction)
    {
      if (edmFunction.ReturnParameter != null && edmFunction.ReturnParameter.TypeUsage.IsSpatialType() || edmFunction.Parameters.Any<FunctionParameter>((Func<FunctionParameter, bool>) (p => p.TypeUsage.IsSpatialType())))
        return true;
      ReadOnlyMetadataCollection<FunctionParameter> parameters = edmFunction.Parameters;
      switch (edmFunction.Name.ToUpperInvariant())
      {
        case "CHECKSUM":
        case "DATALENGTH":
        case "DAY":
        case "MONTH":
        case "YEAR":
          string name1 = parameters[0].TypeUsage.EdmType.Name;
          return name1.Equals("DateTimeOffset", StringComparison.OrdinalIgnoreCase) || name1.Equals("Time", StringComparison.OrdinalIgnoreCase);
        case "COUNT":
        case "COUNT_BIG":
        case "MAX":
        case "MIN":
          string name2 = ((CollectionType) parameters[0].TypeUsage.EdmType).TypeUsage.EdmType.Name;
          return name2.Equals("DateTimeOffset", StringComparison.OrdinalIgnoreCase) || name2.Equals("Time", StringComparison.OrdinalIgnoreCase);
        case "DATEADD":
        case "DATEDIFF":
          string name3 = parameters[1].TypeUsage.EdmType.Name;
          string name4 = parameters[2].TypeUsage.EdmType.Name;
          return name3.Equals("Time", StringComparison.OrdinalIgnoreCase) || name4.Equals("Time", StringComparison.OrdinalIgnoreCase) || name3.Equals("DateTimeOffset", StringComparison.OrdinalIgnoreCase) || name4.Equals("DateTimeOffset", StringComparison.OrdinalIgnoreCase);
        case "DATENAME":
        case "DATEPART":
          string name5 = parameters[1].TypeUsage.EdmType.Name;
          return name5.Equals("DateTimeOffset", StringComparison.OrdinalIgnoreCase) || name5.Equals("Time", StringComparison.OrdinalIgnoreCase);
        case "SYSDATETIME":
        case "SYSDATETIMEOFFSET":
        case "SYSUTCDATETIME":
          return true;
        default:
          return false;
      }
    }

    private static bool IsYukonOrNewer(EdmFunction edmFunction)
    {
      ReadOnlyMetadataCollection<FunctionParameter> parameters = edmFunction.Parameters;
      if (parameters == null || parameters.Count == 0)
        return false;
      switch (edmFunction.Name.ToUpperInvariant())
      {
        case "COUNT":
        case "COUNT_BIG":
          return ((CollectionType) parameters[0].TypeUsage.EdmType).TypeUsage.EdmType.Name.Equals("Guid", StringComparison.OrdinalIgnoreCase);
        case "CHARINDEX":
          using (ReadOnlyMetadataCollection<FunctionParameter>.Enumerator enumerator = parameters.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              if (enumerator.Current.TypeUsage.EdmType.Name.Equals("Int64", StringComparison.OrdinalIgnoreCase))
                return true;
            }
            break;
          }
      }
      return false;
    }

    public override TypeUsage GetEdmType(TypeUsage storeType)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<TypeUsage>(storeType, nameof (storeType));
      string lowerInvariant = storeType.EdmType.Name.ToLowerInvariant();
      PrimitiveType primitiveType = this.StoreTypeNameToEdmPrimitiveType.ContainsKey(lowerInvariant) ? this.StoreTypeNameToEdmPrimitiveType[lowerInvariant] : throw new ArgumentException(System.Data.Entity.SqlServer.Resources.Strings.ProviderDoesNotSupportType((object) lowerInvariant));
      int maxLength = 0;
      bool isUnicode = true;
      PrimitiveTypeKind primitiveTypeKind;
      bool flag;
      bool isFixedLength;
      switch (lowerInvariant)
      {
        case "bigint":
        case "bit":
        case "geography":
        case "geometry":
        case "hierarchyid":
        case "int":
        case "smallint":
        case "tinyint":
        case "uniqueidentifier":
          return TypeUsage.CreateDefaultTypeUsage((EdmType) primitiveType);
        case "binary":
          primitiveTypeKind = PrimitiveTypeKind.Binary;
          flag = !storeType.TryGetMaxLength(out maxLength);
          isFixedLength = true;
          break;
        case "char":
          primitiveTypeKind = PrimitiveTypeKind.String;
          flag = !storeType.TryGetMaxLength(out maxLength);
          isUnicode = false;
          isFixedLength = true;
          break;
        case "date":
          return TypeUsage.CreateDefaultTypeUsage((EdmType) primitiveType);
        case "datetime":
        case "datetime2":
        case "smalldatetime":
          return TypeUsage.CreateDateTimeTypeUsage(primitiveType, new byte?());
        case "datetimeoffset":
          return TypeUsage.CreateDateTimeOffsetTypeUsage(primitiveType, new byte?());
        case "decimal":
        case "numeric":
          byte precision;
          byte scale;
          return storeType.TryGetPrecision(out precision) && storeType.TryGetScale(out scale) ? TypeUsage.CreateDecimalTypeUsage(primitiveType, precision, scale) : TypeUsage.CreateDecimalTypeUsage(primitiveType);
        case "float":
        case "real":
          return TypeUsage.CreateDefaultTypeUsage((EdmType) primitiveType);
        case "image":
        case "varbinary(max)":
          primitiveTypeKind = PrimitiveTypeKind.Binary;
          flag = true;
          isFixedLength = false;
          break;
        case "money":
          return TypeUsage.CreateDecimalTypeUsage(primitiveType, (byte) 19, (byte) 4);
        case "nchar":
          primitiveTypeKind = PrimitiveTypeKind.String;
          flag = !storeType.TryGetMaxLength(out maxLength);
          isUnicode = true;
          isFixedLength = true;
          break;
        case "ntext":
        case "nvarchar(max)":
        case "xml":
          primitiveTypeKind = PrimitiveTypeKind.String;
          flag = true;
          isUnicode = true;
          isFixedLength = false;
          break;
        case "nvarchar":
          primitiveTypeKind = PrimitiveTypeKind.String;
          flag = !storeType.TryGetMaxLength(out maxLength);
          isUnicode = true;
          isFixedLength = false;
          break;
        case "rowversion":
        case "timestamp":
          return TypeUsage.CreateBinaryTypeUsage(primitiveType, true, 8);
        case "smallmoney":
          return TypeUsage.CreateDecimalTypeUsage(primitiveType, (byte) 10, (byte) 4);
        case "text":
        case "varchar(max)":
          primitiveTypeKind = PrimitiveTypeKind.String;
          flag = true;
          isUnicode = false;
          isFixedLength = false;
          break;
        case "time":
          return TypeUsage.CreateTimeTypeUsage(primitiveType, new byte?());
        case "varbinary":
          primitiveTypeKind = PrimitiveTypeKind.Binary;
          flag = !storeType.TryGetMaxLength(out maxLength);
          isFixedLength = false;
          break;
        case "varchar":
          primitiveTypeKind = PrimitiveTypeKind.String;
          flag = !storeType.TryGetMaxLength(out maxLength);
          isUnicode = false;
          isFixedLength = false;
          break;
        default:
          throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.ProviderDoesNotSupportType((object) lowerInvariant));
      }
      if (primitiveTypeKind != PrimitiveTypeKind.Binary)
      {
        if (primitiveTypeKind != PrimitiveTypeKind.String)
          throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.ProviderDoesNotSupportType((object) lowerInvariant));
        return !flag ? TypeUsage.CreateStringTypeUsage(primitiveType, isUnicode, isFixedLength, maxLength) : TypeUsage.CreateStringTypeUsage(primitiveType, isUnicode, isFixedLength);
      }
      return !flag ? TypeUsage.CreateBinaryTypeUsage(primitiveType, isFixedLength, maxLength) : TypeUsage.CreateBinaryTypeUsage(primitiveType, isFixedLength);
    }

    public override TypeUsage GetStoreType(TypeUsage edmType)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<TypeUsage>(edmType, nameof (edmType));
      if (!(edmType.EdmType is PrimitiveType edmType1))
        throw new ArgumentException(System.Data.Entity.SqlServer.Resources.Strings.ProviderDoesNotSupportType((object) edmType.EdmType.Name));
      ReadOnlyMetadataCollection<Facet> facets = edmType.Facets;
      switch (edmType1.PrimitiveTypeKind)
      {
        case PrimitiveTypeKind.Binary:
          int num = facets["FixedLength"].Value == null ? 0 : ((bool) facets["FixedLength"].Value ? 1 : 0);
          Facet facet1 = facets["MaxLength"];
          bool flag1 = facet1.IsUnbounded || facet1.Value == null || (int) facet1.Value > 8000;
          int maxLength1 = !flag1 ? (int) facet1.Value : int.MinValue;
          return num == 0 ? (!flag1 ? TypeUsage.CreateBinaryTypeUsage(this.StoreTypeNameToStorePrimitiveType["varbinary"], false, maxLength1) : (this._version == SqlVersion.Sql8 ? TypeUsage.CreateBinaryTypeUsage(this.StoreTypeNameToStorePrimitiveType["varbinary"], false, 8000) : TypeUsage.CreateBinaryTypeUsage(this.StoreTypeNameToStorePrimitiveType["varbinary(max)"], false))) : TypeUsage.CreateBinaryTypeUsage(this.StoreTypeNameToStorePrimitiveType["binary"], true, flag1 ? 8000 : maxLength1);
        case PrimitiveTypeKind.Boolean:
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["bit"]);
        case PrimitiveTypeKind.Byte:
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["tinyint"]);
        case PrimitiveTypeKind.DateTime:
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["datetime"]);
        case PrimitiveTypeKind.Decimal:
          byte precision;
          if (!edmType.TryGetPrecision(out precision))
            precision = (byte) 18;
          byte scale;
          if (!edmType.TryGetScale(out scale))
            scale = (byte) 0;
          return TypeUsage.CreateDecimalTypeUsage(this.StoreTypeNameToStorePrimitiveType["decimal"], precision, scale);
        case PrimitiveTypeKind.Double:
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["float"]);
        case PrimitiveTypeKind.Guid:
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["uniqueidentifier"]);
        case PrimitiveTypeKind.Single:
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["real"]);
        case PrimitiveTypeKind.Int16:
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["smallint"]);
        case PrimitiveTypeKind.Int32:
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["int"]);
        case PrimitiveTypeKind.Int64:
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["bigint"]);
        case PrimitiveTypeKind.String:
          bool flag2 = facets["Unicode"].Value == null || (bool) facets["Unicode"].Value;
          bool flag3 = facets["FixedLength"].Value != null && (bool) facets["FixedLength"].Value;
          Facet facet2 = facets["MaxLength"];
          bool flag4 = facet2.IsUnbounded || facet2.Value == null || (int) facet2.Value > (flag2 ? 4000 : 8000);
          int maxLength2 = !flag4 ? (int) facet2.Value : int.MinValue;
          return !flag2 ? (!flag3 ? (!flag4 ? TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["varchar"], false, false, maxLength2) : (this._version == SqlVersion.Sql8 ? TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["varchar"], false, false, 8000) : TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["varchar(max)"], false, false))) : TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["char"], false, true, flag4 ? 8000 : maxLength2)) : (!flag3 ? (!flag4 ? TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["nvarchar"], true, false, maxLength2) : (this._version == SqlVersion.Sql8 ? TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["nvarchar"], true, false, 4000) : TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["nvarchar(max)"], true, false))) : TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["nchar"], true, true, flag4 ? 4000 : maxLength2));
        case PrimitiveTypeKind.Time:
          return this.GetStorePrimitiveTypeIfPostSql9("time", edmType.EdmType.Name, edmType1.PrimitiveTypeKind);
        case PrimitiveTypeKind.DateTimeOffset:
          return this.GetStorePrimitiveTypeIfPostSql9("datetimeoffset", edmType.EdmType.Name, edmType1.PrimitiveTypeKind);
        case PrimitiveTypeKind.Geometry:
        case PrimitiveTypeKind.GeometryPoint:
        case PrimitiveTypeKind.GeometryLineString:
        case PrimitiveTypeKind.GeometryPolygon:
        case PrimitiveTypeKind.GeometryMultiPoint:
        case PrimitiveTypeKind.GeometryMultiLineString:
        case PrimitiveTypeKind.GeometryMultiPolygon:
        case PrimitiveTypeKind.GeometryCollection:
          return this.GetStorePrimitiveTypeIfPostSql9("geometry", edmType.EdmType.Name, edmType1.PrimitiveTypeKind);
        case PrimitiveTypeKind.Geography:
        case PrimitiveTypeKind.GeographyPoint:
        case PrimitiveTypeKind.GeographyLineString:
        case PrimitiveTypeKind.GeographyPolygon:
        case PrimitiveTypeKind.GeographyMultiPoint:
        case PrimitiveTypeKind.GeographyMultiLineString:
        case PrimitiveTypeKind.GeographyMultiPolygon:
        case PrimitiveTypeKind.GeographyCollection:
          return this.GetStorePrimitiveTypeIfPostSql9("geography", edmType.EdmType.Name, edmType1.PrimitiveTypeKind);
        case PrimitiveTypeKind.HierarchyId:
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["hierarchyid"]);
        default:
          throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.NoStoreTypeForEdmType((object) edmType.EdmType.Name, (object) edmType1.PrimitiveTypeKind));
      }
    }

    private TypeUsage GetStorePrimitiveTypeIfPostSql9(
      string storeTypeName,
      string nameForException,
      PrimitiveTypeKind primitiveTypeKind)
    {
      if (this.SqlVersion != SqlVersion.Sql8 && this.SqlVersion != SqlVersion.Sql9)
        return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType[storeTypeName]);
      throw new NotSupportedException(System.Data.Entity.SqlServer.Resources.Strings.NoStoreTypeForEdmType((object) nameForException, (object) primitiveTypeKind));
    }

    public override bool SupportsEscapingLikeArgument(out char escapeCharacter)
    {
      escapeCharacter = '~';
      return true;
    }

    public override string EscapeLikeArgument(string argument)
    {
      System.Data.Entity.SqlServer.Utilities.Check.NotNull<string>(argument, nameof (argument));
      return SqlProviderManifest.EscapeLikeText(argument, true, out bool _);
    }

    /// <summary>
    /// Indicates if the provider supports the parameter optimization described in EntityFramework6 GitHub issue #195.
    /// </summary>
    /// <returns><c>True</c> since this provider supports the parameter optimization.</returns>
    public override bool SupportsParameterOptimizationInSchemaQueries() => true;

    public override bool SupportsInExpression() => true;

    public override bool SupportsIntersectAndUnionAllFlattening() => true;
  }
}

// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.EF6.SQLiteProviderManifest
// Assembly: System.Data.SQLite.EF6, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 284EE9AD-5161-41AE-8341-10FDAF741756
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.EF6.dll

using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Reflection;
using System.Text;
using System.Xml;

namespace System.Data.SQLite.EF6
{
  internal sealed class SQLiteProviderManifest : DbXmlEnabledProviderManifest
  {
    internal SQLiteDateFormats _dateTimeFormat;
    internal DateTimeKind _dateTimeKind;
    internal string _dateTimeFormatString;
    internal bool _binaryGuid;

    public SQLiteProviderManifest(string manifestToken)
      : base(SQLiteProviderManifest.GetProviderManifest())
    {
      this.SetFromOptions(SQLiteProviderManifest.ParseProviderManifestToken(SQLiteProviderManifest.GetProviderManifestToken(manifestToken)));
    }

    private static XmlReader GetProviderManifest() => SQLiteProviderManifest.GetXmlResource("System.Data.SQLite.SQLiteProviderServices.ProviderManifest.xml");

    private static string GetProviderManifestToken(string manifestToken)
    {
      string settingValue = UnsafeNativeMethods.GetSettingValue("AppendManifestToken_SQLiteProviderManifest", (string) null);
      if (string.IsNullOrEmpty(settingValue))
        return manifestToken;
      int length = settingValue.Length;
      if (manifestToken != null)
        length += manifestToken.Length;
      StringBuilder stringBuilder = new StringBuilder(length);
      stringBuilder.Append(manifestToken);
      stringBuilder.Append(settingValue);
      return stringBuilder.ToString();
    }

    private static SortedList<string, string> ParseProviderManifestToken(
      string manifestToken)
    {
      return SQLiteConnection.ParseConnectionString(manifestToken, false, true);
    }

    internal void SetFromOptions(SortedList<string, string> opts)
    {
      this._dateTimeFormat = SQLiteDateFormats.ISO8601;
      this._dateTimeKind = DateTimeKind.Unspecified;
      this._dateTimeFormatString = (string) null;
      this._binaryGuid = false;
      if (opts == null)
        return;
      foreach (string name in Enum.GetNames(typeof (SQLiteDateFormats)))
      {
        if (!string.IsNullOrEmpty(name) && (object) SQLiteConnection.FindKey(opts, name, (string) null) != null)
          this._dateTimeFormat = (SQLiteDateFormats) Enum.Parse(typeof (SQLiteDateFormats), name, true);
      }
      if (SQLiteConnection.TryParseEnum(typeof (SQLiteDateFormats), SQLiteConnection.FindKey(opts, "DateTimeFormat", (string) null), true) is SQLiteDateFormats sqLiteDateFormats)
        this._dateTimeFormat = sqLiteDateFormats;
      if (SQLiteConnection.TryParseEnum(typeof (DateTimeKind), SQLiteConnection.FindKey(opts, "DateTimeKind", (string) null), true) is DateTimeKind dateTimeKind)
        this._dateTimeKind = dateTimeKind;
      string key1 = SQLiteConnection.FindKey(opts, "DateTimeFormatString", (string) null);
      if (key1 != null)
        this._dateTimeFormatString = key1;
      string key2 = SQLiteConnection.FindKey(opts, "BinaryGUID", (string) null);
      if (key2 == null)
        return;
      this._binaryGuid = SQLiteConvert.ToBoolean(key2);
    }

    protected override XmlReader GetDbInformation(string informationType)
    {
      if (informationType == "StoreSchemaDefinition")
        return this.GetStoreSchemaDescription();
      if (informationType == "StoreSchemaMapping")
        return this.GetStoreSchemaMapping();
      if (informationType == "ConceptualSchemaDefinition")
        return (XmlReader) null;
      throw new ProviderIncompatibleException(string.Format("SQLite does not support this information type '{0}'.", (object) informationType));
    }

    public override TypeUsage GetEdmType(TypeUsage storeType)
    {
      if (storeType == null)
        throw new ArgumentNullException(nameof (storeType));
      string lowerInvariant = storeType.EdmType.Name.ToLowerInvariant();
      PrimitiveType primitiveType;
      if (!this.StoreTypeNameToEdmPrimitiveType.TryGetValue(lowerInvariant, out primitiveType))
        throw new ArgumentException(string.Format("SQLite does not support the type '{0}'.", (object) lowerInvariant));
      int maxLength = 0;
      bool isUnicode = true;
      PrimitiveTypeKind primitiveTypeKind;
      bool flag;
      bool isFixedLength;
      switch (lowerInvariant)
      {
        case "tinyint":
        case "smallint":
        case "integer":
        case "bit":
        case "uniqueidentifier":
        case "int":
        case "float":
        case "real":
          return TypeUsage.CreateDefaultTypeUsage((EdmType) primitiveType);
        case "varchar":
          primitiveTypeKind = PrimitiveTypeKind.String;
          flag = !SQLiteProviderManifest.TypeHelpers.TryGetMaxLength(storeType, out maxLength);
          isUnicode = false;
          isFixedLength = false;
          break;
        case "char":
          primitiveTypeKind = PrimitiveTypeKind.String;
          flag = !SQLiteProviderManifest.TypeHelpers.TryGetMaxLength(storeType, out maxLength);
          isUnicode = false;
          isFixedLength = true;
          break;
        case "nvarchar":
          primitiveTypeKind = PrimitiveTypeKind.String;
          flag = !SQLiteProviderManifest.TypeHelpers.TryGetMaxLength(storeType, out maxLength);
          isUnicode = true;
          isFixedLength = false;
          break;
        case "nchar":
          primitiveTypeKind = PrimitiveTypeKind.String;
          flag = !SQLiteProviderManifest.TypeHelpers.TryGetMaxLength(storeType, out maxLength);
          isUnicode = true;
          isFixedLength = true;
          break;
        case "blob":
          primitiveTypeKind = PrimitiveTypeKind.Binary;
          flag = !SQLiteProviderManifest.TypeHelpers.TryGetMaxLength(storeType, out maxLength);
          isFixedLength = false;
          break;
        case "decimal":
          byte precision;
          byte scale;
          return SQLiteProviderManifest.TypeHelpers.TryGetPrecision(storeType, out precision) && SQLiteProviderManifest.TypeHelpers.TryGetScale(storeType, out scale) ? TypeUsage.CreateDecimalTypeUsage(primitiveType, precision, scale) : TypeUsage.CreateDecimalTypeUsage(primitiveType);
        case "datetime":
          return TypeUsage.CreateDateTimeTypeUsage(primitiveType, new byte?());
        default:
          throw new NotSupportedException(string.Format("SQLite does not support the type '{0}'.", (object) lowerInvariant));
      }
      switch (primitiveTypeKind)
      {
        case PrimitiveTypeKind.Binary:
          return !flag ? TypeUsage.CreateBinaryTypeUsage(primitiveType, isFixedLength, maxLength) : TypeUsage.CreateBinaryTypeUsage(primitiveType, isFixedLength);
        case PrimitiveTypeKind.String:
          return !flag ? TypeUsage.CreateStringTypeUsage(primitiveType, isUnicode, isFixedLength, maxLength) : TypeUsage.CreateStringTypeUsage(primitiveType, isUnicode, isFixedLength);
        default:
          throw new NotSupportedException(string.Format("SQLite does not support the type '{0}'.", (object) lowerInvariant));
      }
    }

    public override TypeUsage GetStoreType(TypeUsage edmType)
    {
      if (edmType == null)
        throw new ArgumentNullException(nameof (edmType));
      if (!(edmType.EdmType is PrimitiveType edmType1))
        throw new ArgumentException(string.Format("SQLite does not support the type '{0}'.", (object) edmType));
      ReadOnlyMetadataCollection<Facet> facets = edmType.Facets;
      switch (edmType1.PrimitiveTypeKind)
      {
        case PrimitiveTypeKind.Binary:
          bool flag1 = facets["FixedLength"].Value != null && (bool) facets["FixedLength"].Value;
          Facet facet1 = facets["MaxLength"];
          bool flag2 = facet1.IsUnbounded || facet1.Value == null || (int) facet1.Value > int.MaxValue;
          int maxLength1 = !flag2 ? (int) facet1.Value : int.MinValue;
          return !flag1 ? (!flag2 ? TypeUsage.CreateBinaryTypeUsage(this.StoreTypeNameToStorePrimitiveType["blob"], false, maxLength1) : TypeUsage.CreateBinaryTypeUsage(this.StoreTypeNameToStorePrimitiveType["blob"], false)) : TypeUsage.CreateBinaryTypeUsage(this.StoreTypeNameToStorePrimitiveType["blob"], true, maxLength1);
        case PrimitiveTypeKind.Boolean:
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["bit"]);
        case PrimitiveTypeKind.Byte:
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["tinyint"]);
        case PrimitiveTypeKind.DateTime:
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["datetime"]);
        case PrimitiveTypeKind.Decimal:
          byte precision;
          if (!SQLiteProviderManifest.TypeHelpers.TryGetPrecision(edmType, out precision))
            precision = (byte) 18;
          byte scale;
          if (!SQLiteProviderManifest.TypeHelpers.TryGetScale(edmType, out scale))
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
          return TypeUsage.CreateDefaultTypeUsage((EdmType) this.StoreTypeNameToStorePrimitiveType["integer"]);
        case PrimitiveTypeKind.String:
          bool flag3 = facets["Unicode"].Value == null || (bool) facets["Unicode"].Value;
          bool flag4 = facets["FixedLength"].Value != null && (bool) facets["FixedLength"].Value;
          Facet facet2 = facets["MaxLength"];
          bool flag5 = facet2.IsUnbounded || facet2.Value == null || (int) facet2.Value > (flag3 ? int.MaxValue : int.MaxValue);
          int maxLength2 = !flag5 ? (int) facet2.Value : int.MinValue;
          return !flag3 ? (!flag4 ? (!flag5 ? TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["varchar"], false, false, maxLength2) : TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["varchar"], false, false)) : TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["char"], false, true, maxLength2)) : (!flag4 ? (!flag5 ? TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["nvarchar"], true, false, maxLength2) : TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["nvarchar"], true, false)) : TypeUsage.CreateStringTypeUsage(this.StoreTypeNameToStorePrimitiveType["nchar"], true, true, maxLength2));
        default:
          throw new NotSupportedException(string.Format("There is no store type corresponding to the EDM type '{0}' of primitive type '{1}'.", (object) edmType, (object) edmType1.PrimitiveTypeKind));
      }
    }

    private XmlReader GetStoreSchemaMapping() => SQLiteProviderManifest.GetXmlResource("System.Data.SQLite.SQLiteProviderServices.StoreSchemaMapping.msl");

    private XmlReader GetStoreSchemaDescription() => SQLiteProviderManifest.GetXmlResource("System.Data.SQLite.SQLiteProviderServices.StoreSchemaDefinition.ssdl");

    internal static XmlReader GetXmlResource(string resourceName) => XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName));

    private static class TypeHelpers
    {
      public static bool TryGetPrecision(TypeUsage tu, out byte precision)
      {
        precision = (byte) 0;
        Facet facet;
        if (!tu.Facets.TryGetValue("Precision", false, out facet) || facet.IsUnbounded || facet.Value == null)
          return false;
        precision = (byte) facet.Value;
        return true;
      }

      public static bool TryGetMaxLength(TypeUsage tu, out int maxLength)
      {
        maxLength = 0;
        Facet facet;
        if (!tu.Facets.TryGetValue("MaxLength", false, out facet) || facet.IsUnbounded || facet.Value == null)
          return false;
        maxLength = (int) facet.Value;
        return true;
      }

      public static bool TryGetScale(TypeUsage tu, out byte scale)
      {
        scale = (byte) 0;
        Facet facet;
        if (!tu.Facets.TryGetValue("Scale", false, out facet) || facet.IsUnbounded || facet.Value == null)
          return false;
        scale = (byte) facet.Value;
        return true;
      }
    }
  }
}

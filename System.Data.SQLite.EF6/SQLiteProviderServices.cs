// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.EF6.SQLiteProviderServices
// Assembly: System.Data.SQLite.EF6, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 284EE9AD-5161-41AE-8341-10FDAF741756
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.EF6.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SQLite.EF6.Properties;
using System.Globalization;
using System.Text;

namespace System.Data.SQLite.EF6
{
  internal sealed class SQLiteProviderServices : DbProviderServices, ISQLiteSchemaExtensions
  {
    internal static readonly SQLiteProviderServices Instance = new SQLiteProviderServices();

    protected override DbCommandDefinition CreateDbCommandDefinition(
      DbProviderManifest manifest,
      DbCommandTree commandTree)
    {
      return this.CreateCommandDefinition(this.CreateCommand(manifest, commandTree));
    }

    private DbCommand CreateCommand(DbProviderManifest manifest, DbCommandTree commandTree)
    {
      if (manifest == null)
        throw new ArgumentNullException(nameof (manifest));
      if (commandTree == null)
        throw new ArgumentNullException(nameof (commandTree));
      SQLiteCommand sqLiteCommand = new SQLiteCommand();
      try
      {
        List<DbParameter> parameters;
        CommandType commandType;
        sqLiteCommand.CommandText = SqlGenerator.GenerateSql((SQLiteProviderManifest) manifest, commandTree, out parameters, out commandType);
        sqLiteCommand.CommandType = commandType;
        EdmFunction edmFunction = (EdmFunction) null;
        if (commandTree is DbFunctionCommandTree)
          edmFunction = ((DbFunctionCommandTree) commandTree).EdmFunction;
        foreach (KeyValuePair<string, TypeUsage> parameter1 in commandTree.Parameters)
        {
          FunctionParameter functionParameter;
          SQLiteParameter parameter2 = edmFunction == null || !edmFunction.Parameters.TryGetValue(parameter1.Key, false, out functionParameter) ? SQLiteProviderServices.CreateSqlParameter((SQLiteProviderManifest) manifest, parameter1.Key, parameter1.Value, ParameterMode.In, (object) DBNull.Value) : SQLiteProviderServices.CreateSqlParameter((SQLiteProviderManifest) manifest, functionParameter.Name, functionParameter.TypeUsage, functionParameter.Mode, (object) DBNull.Value);
          sqLiteCommand.Parameters.Add(parameter2);
        }
        if (parameters != null && 0 < parameters.Count)
        {
          switch (commandTree)
          {
            case DbInsertCommandTree _:
            case DbUpdateCommandTree _:
            case DbDeleteCommandTree _:
              using (List<DbParameter>.Enumerator enumerator = parameters.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  DbParameter current = enumerator.Current;
                  sqLiteCommand.Parameters.Add((object) current);
                }
                break;
              }
            default:
              throw new InvalidOperationException("SqlGenParametersNotPermitted");
          }
        }
        return (DbCommand) sqLiteCommand;
      }
      catch
      {
        sqLiteCommand.Dispose();
        throw;
      }
    }

    protected override string GetDbProviderManifestToken(DbConnection connection)
    {
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      return !string.IsNullOrEmpty(connection.ConnectionString) ? connection.ConnectionString : throw new ArgumentNullException("ConnectionString");
    }

    protected override DbProviderManifest GetDbProviderManifest(string versionHint) => (DbProviderManifest) new SQLiteProviderManifest(versionHint);

    internal static SQLiteParameter CreateSqlParameter(
      SQLiteProviderManifest manifest,
      string name,
      TypeUsage type,
      ParameterMode mode,
      object value)
    {
      if (manifest != null && !manifest._binaryGuid && MetadataHelpers.GetPrimitiveTypeKind(type) == PrimitiveTypeKind.Guid)
        type = TypeUsage.CreateStringTypeUsage(PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.String), false, true);
      SQLiteParameter sqLiteParameter = new SQLiteParameter(name, value);
      ParameterDirection parameterDirection = MetadataHelpers.ParameterModeToParameterDirection(mode);
      if (sqLiteParameter.Direction != parameterDirection)
        sqLiteParameter.Direction = parameterDirection;
      bool isOutParam = mode != ParameterMode.In;
      int? size;
      DbType sqlDbType = SQLiteProviderServices.GetSqlDbType(type, isOutParam, out size);
      if (sqLiteParameter.DbType != sqlDbType)
        sqLiteParameter.DbType = sqlDbType;
      if (size.HasValue && (isOutParam || sqLiteParameter.Size != size.Value))
        sqLiteParameter.Size = size.Value;
      bool flag = MetadataHelpers.IsNullable(type);
      if (isOutParam || flag != sqLiteParameter.IsNullable)
        sqLiteParameter.IsNullable = flag;
      return sqLiteParameter;
    }

    private static DbType GetSqlDbType(TypeUsage type, bool isOutParam, out int? size)
    {
      PrimitiveTypeKind primitiveTypeKind = MetadataHelpers.GetPrimitiveTypeKind(type);
      size = new int?();
      switch (primitiveTypeKind)
      {
        case PrimitiveTypeKind.Binary:
          size = SQLiteProviderServices.GetParameterSize(type, isOutParam);
          return SQLiteProviderServices.GetBinaryDbType(type);
        case PrimitiveTypeKind.Boolean:
          return DbType.Boolean;
        case PrimitiveTypeKind.Byte:
          return DbType.Byte;
        case PrimitiveTypeKind.DateTime:
          return DbType.DateTime;
        case PrimitiveTypeKind.Decimal:
          return DbType.Decimal;
        case PrimitiveTypeKind.Double:
          return DbType.Double;
        case PrimitiveTypeKind.Guid:
          return DbType.Guid;
        case PrimitiveTypeKind.Single:
          return DbType.Single;
        case PrimitiveTypeKind.SByte:
          return DbType.SByte;
        case PrimitiveTypeKind.Int16:
          return DbType.Int16;
        case PrimitiveTypeKind.Int32:
          return DbType.Int32;
        case PrimitiveTypeKind.Int64:
          return DbType.Int64;
        case PrimitiveTypeKind.String:
          size = SQLiteProviderServices.GetParameterSize(type, isOutParam);
          return SQLiteProviderServices.GetStringDbType(type);
        case PrimitiveTypeKind.Time:
          return DbType.Time;
        case PrimitiveTypeKind.DateTimeOffset:
          return DbType.DateTimeOffset;
        default:
          return DbType.Object;
      }
    }

    private static int? GetParameterSize(TypeUsage type, bool isOutParam)
    {
      int maxLength;
      if (MetadataHelpers.TryGetMaxLength(type, out maxLength))
        return new int?(maxLength);
      return isOutParam ? new int?(int.MaxValue) : new int?();
    }

    private static DbType GetStringDbType(TypeUsage type)
    {
      bool isFixedLength;
      if (!MetadataHelpers.TryGetIsFixedLength(type, out isFixedLength))
        isFixedLength = false;
      bool isUnicode;
      if (!MetadataHelpers.TryGetIsUnicode(type, out isUnicode))
        isUnicode = true;
      return !isFixedLength ? (isUnicode ? DbType.String : DbType.AnsiString) : (isUnicode ? DbType.StringFixedLength : DbType.AnsiStringFixedLength);
    }

    private static DbType GetBinaryDbType(TypeUsage type)
    {
      bool isFixedLength;
      if (!MetadataHelpers.TryGetIsFixedLength(type, out isFixedLength))
        isFixedLength = false;
      return DbType.Binary;
    }

    void ISQLiteSchemaExtensions.BuildTempSchema(SQLiteConnection cnn)
    {
      string[] strArray = new string[8]
      {
        "TABLES",
        "COLUMNS",
        "VIEWS",
        "VIEWCOLUMNS",
        "INDEXES",
        "INDEXCOLUMNS",
        "FOREIGNKEYS",
        "CATALOGS"
      };
      using (DataTable schema = cnn.GetSchema("Tables", new string[3]
      {
        "temp",
        null,
        string.Format("SCHEMA{0}", (object) strArray[0])
      }))
      {
        if (schema.Rows.Count > 0)
          return;
      }
      for (int index = 0; index < strArray.Length; ++index)
      {
        using (DataTable schema = cnn.GetSchema(strArray[index]))
          this.DataTableToTable(cnn, schema, string.Format("SCHEMA{0}", (object) strArray[index]));
      }
      using (SQLiteCommand command = cnn.CreateCommand())
      {
        command.CommandText = Resources.SQL_CONSTRAINTS;
        command.ExecuteNonQuery();
        command.CommandText = Resources.SQL_CONSTRAINTCOLUMNS;
        command.ExecuteNonQuery();
      }
    }

    private void DataTableToTable(SQLiteConnection cnn, DataTable table, string dest)
    {
      StringBuilder stringBuilder = new StringBuilder();
      SQLiteCommandBuilder liteCommandBuilder = new SQLiteCommandBuilder();
      using (SQLiteCommand command = cnn.CreateCommand())
      {
        using (DataTable dataTable = new DataTable())
        {
          stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "CREATE TEMP TABLE {0} (", (object) liteCommandBuilder.QuoteIdentifier(dest));
          string str = string.Empty;
          SQLiteConnectionFlags flags = cnn.Flags;
          foreach (DataColumn column in (InternalDataCollectionBase) table.Columns)
          {
            DbType dbType = SQLiteConvert.TypeToDbType(column.DataType);
            string typeName = SQLiteConvert.DbTypeToTypeName(cnn, dbType, flags);
            stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{2}{0} {1} COLLATE NOCASE", (object) liteCommandBuilder.QuoteIdentifier(column.ColumnName), (object) typeName, (object) str);
            str = ", ";
          }
          stringBuilder.Append(")");
          command.CommandText = stringBuilder.ToString();
          command.ExecuteNonQuery();
          command.CommandText = string.Format("SELECT * FROM TEMP.{0} WHERE 1=2", (object) liteCommandBuilder.QuoteIdentifier(dest));
          using (SQLiteDataAdapter sqLiteDataAdapter = new SQLiteDataAdapter(command))
          {
            liteCommandBuilder.DataAdapter = sqLiteDataAdapter;
            sqLiteDataAdapter.Fill(dataTable);
            foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
            {
              object[] itemArray = row.ItemArray;
              dataTable.Rows.Add(itemArray);
            }
            sqLiteDataAdapter.Update(dataTable);
          }
        }
      }
    }
  }
}

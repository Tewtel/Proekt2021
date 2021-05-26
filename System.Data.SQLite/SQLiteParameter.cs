// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteParameter
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.ComponentModel;
using System.Data.Common;

namespace System.Data.SQLite
{
  /// <summary>SQLite implementation of DbParameter.</summary>
  public sealed class SQLiteParameter : DbParameter, ICloneable
  {
    /// <summary>
    /// This value represents an "unknown" <see cref="P:System.Data.SQLite.SQLiteParameter.DbType" />.
    /// </summary>
    private const DbType UnknownDbType = ~DbType.AnsiString;
    /// <summary>The command associated with this parameter.</summary>
    private IDbCommand _command;
    /// <summary>The data type of the parameter</summary>
    internal DbType _dbType;
    /// <summary>The version information for mapping the parameter</summary>
    private DataRowVersion _rowVersion;
    /// <summary>The value of the data in the parameter</summary>
    private object _objValue;
    /// <summary>The source column for the parameter</summary>
    private string _sourceColumn;
    /// <summary>The column name</summary>
    private string _parameterName;
    /// <summary>The data size, unused by SQLite</summary>
    private int _dataSize;
    private bool _nullable;
    private bool _nullMapping;
    /// <summary>
    /// The database type name associated with this parameter, if any.
    /// </summary>
    private string _typeName;

    /// <summary>
    /// Constructor used when creating for use with a specific command.
    /// </summary>
    /// <param name="command">
    /// The command associated with this parameter.
    /// </param>
    internal SQLiteParameter(IDbCommand command)
      : this()
    {
      this._command = command;
    }

    /// <summary>Default constructor</summary>
    public SQLiteParameter()
      : this((string) null, ~DbType.AnsiString, 0, (string) null, DataRowVersion.Current)
    {
    }

    /// <summary>
    /// Constructs a named parameter given the specified parameter name
    /// </summary>
    /// <param name="parameterName">The parameter name</param>
    public SQLiteParameter(string parameterName)
      : this(parameterName, ~DbType.AnsiString, 0, (string) null, DataRowVersion.Current)
    {
    }

    /// <summary>
    /// Constructs a named parameter given the specified parameter name and initial value
    /// </summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="value">The initial value of the parameter</param>
    public SQLiteParameter(string parameterName, object value)
      : this(parameterName, ~DbType.AnsiString, 0, (string) null, DataRowVersion.Current)
    {
      this.Value = value;
    }

    /// <summary>Constructs a named parameter of the specified type</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="dbType">The datatype of the parameter</param>
    public SQLiteParameter(string parameterName, DbType dbType)
      : this(parameterName, dbType, 0, (string) null, DataRowVersion.Current)
    {
    }

    /// <summary>
    /// Constructs a named parameter of the specified type and source column reference
    /// </summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="dbType">The data type</param>
    /// <param name="sourceColumn">The source column</param>
    public SQLiteParameter(string parameterName, DbType dbType, string sourceColumn)
      : this(parameterName, dbType, 0, sourceColumn, DataRowVersion.Current)
    {
    }

    /// <summary>
    /// Constructs a named parameter of the specified type, source column and row version
    /// </summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="dbType">The data type</param>
    /// <param name="sourceColumn">The source column</param>
    /// <param name="rowVersion">The row version information</param>
    public SQLiteParameter(
      string parameterName,
      DbType dbType,
      string sourceColumn,
      DataRowVersion rowVersion)
      : this(parameterName, dbType, 0, sourceColumn, rowVersion)
    {
    }

    /// <summary>
    /// Constructs an unnamed parameter of the specified data type
    /// </summary>
    /// <param name="dbType">The datatype of the parameter</param>
    public SQLiteParameter(DbType dbType)
      : this((string) null, dbType, 0, (string) null, DataRowVersion.Current)
    {
    }

    /// <summary>
    /// Constructs an unnamed parameter of the specified data type and sets the initial value
    /// </summary>
    /// <param name="dbType">The datatype of the parameter</param>
    /// <param name="value">The initial value of the parameter</param>
    public SQLiteParameter(DbType dbType, object value)
      : this((string) null, dbType, 0, (string) null, DataRowVersion.Current)
    {
      this.Value = value;
    }

    /// <summary>
    /// Constructs an unnamed parameter of the specified data type and source column
    /// </summary>
    /// <param name="dbType">The datatype of the parameter</param>
    /// <param name="sourceColumn">The source column</param>
    public SQLiteParameter(DbType dbType, string sourceColumn)
      : this((string) null, dbType, 0, sourceColumn, DataRowVersion.Current)
    {
    }

    /// <summary>
    /// Constructs an unnamed parameter of the specified data type, source column and row version
    /// </summary>
    /// <param name="dbType">The data type</param>
    /// <param name="sourceColumn">The source column</param>
    /// <param name="rowVersion">The row version information</param>
    public SQLiteParameter(DbType dbType, string sourceColumn, DataRowVersion rowVersion)
      : this((string) null, dbType, 0, sourceColumn, rowVersion)
    {
    }

    /// <summary>
    /// Constructs a named parameter of the specified type and size
    /// </summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="parameterType">The data type</param>
    /// <param name="parameterSize">The size of the parameter</param>
    public SQLiteParameter(string parameterName, DbType parameterType, int parameterSize)
      : this(parameterName, parameterType, parameterSize, (string) null, DataRowVersion.Current)
    {
    }

    /// <summary>
    /// Constructs a named parameter of the specified type, size and source column
    /// </summary>
    /// <param name="parameterName">The name of the parameter</param>
    /// <param name="parameterType">The data type</param>
    /// <param name="parameterSize">The size of the parameter</param>
    /// <param name="sourceColumn">The source column</param>
    public SQLiteParameter(
      string parameterName,
      DbType parameterType,
      int parameterSize,
      string sourceColumn)
      : this(parameterName, parameterType, parameterSize, sourceColumn, DataRowVersion.Current)
    {
    }

    /// <summary>
    /// Constructs a named parameter of the specified type, size, source column and row version
    /// </summary>
    /// <param name="parameterName">The name of the parameter</param>
    /// <param name="parameterType">The data type</param>
    /// <param name="parameterSize">The size of the parameter</param>
    /// <param name="sourceColumn">The source column</param>
    /// <param name="rowVersion">The row version information</param>
    public SQLiteParameter(
      string parameterName,
      DbType parameterType,
      int parameterSize,
      string sourceColumn,
      DataRowVersion rowVersion)
    {
      this._parameterName = parameterName;
      this._dbType = parameterType;
      this._sourceColumn = sourceColumn;
      this._rowVersion = rowVersion;
      this._dataSize = parameterSize;
      this._nullable = true;
    }

    private SQLiteParameter(SQLiteParameter source)
      : this(source.ParameterName, source._dbType, 0, source.Direction, source.IsNullable, (byte) 0, (byte) 0, source.SourceColumn, source.SourceVersion, source.Value)
    {
      this._nullMapping = source._nullMapping;
    }

    /// <summary>
    /// Constructs a named parameter of the specified type, size, source column and row version
    /// </summary>
    /// <param name="parameterName">The name of the parameter</param>
    /// <param name="parameterType">The data type</param>
    /// <param name="parameterSize">The size of the parameter</param>
    /// <param name="direction">Only input parameters are supported in SQLite</param>
    /// <param name="isNullable">Ignored</param>
    /// <param name="precision">Ignored</param>
    /// <param name="scale">Ignored</param>
    /// <param name="sourceColumn">The source column</param>
    /// <param name="rowVersion">The row version information</param>
    /// <param name="value">The initial value to assign the parameter</param>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public SQLiteParameter(
      string parameterName,
      DbType parameterType,
      int parameterSize,
      ParameterDirection direction,
      bool isNullable,
      byte precision,
      byte scale,
      string sourceColumn,
      DataRowVersion rowVersion,
      object value)
      : this(parameterName, parameterType, parameterSize, sourceColumn, rowVersion)
    {
      this.Direction = direction;
      this.IsNullable = isNullable;
      this.Value = value;
    }

    /// <summary>Constructs a named parameter, yet another flavor</summary>
    /// <param name="parameterName">The name of the parameter</param>
    /// <param name="parameterType">The data type</param>
    /// <param name="parameterSize">The size of the parameter</param>
    /// <param name="direction">Only input parameters are supported in SQLite</param>
    /// <param name="precision">Ignored</param>
    /// <param name="scale">Ignored</param>
    /// <param name="sourceColumn">The source column</param>
    /// <param name="rowVersion">The row version information</param>
    /// <param name="sourceColumnNullMapping">Whether or not this parameter is for comparing NULL's</param>
    /// <param name="value">The intial value to assign the parameter</param>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public SQLiteParameter(
      string parameterName,
      DbType parameterType,
      int parameterSize,
      ParameterDirection direction,
      byte precision,
      byte scale,
      string sourceColumn,
      DataRowVersion rowVersion,
      bool sourceColumnNullMapping,
      object value)
      : this(parameterName, parameterType, parameterSize, sourceColumn, rowVersion)
    {
      this.Direction = direction;
      this.SourceColumnNullMapping = sourceColumnNullMapping;
      this.Value = value;
    }

    /// <summary>
    /// Constructs an unnamed parameter of the specified type and size
    /// </summary>
    /// <param name="parameterType">The data type</param>
    /// <param name="parameterSize">The size of the parameter</param>
    public SQLiteParameter(DbType parameterType, int parameterSize)
      : this((string) null, parameterType, parameterSize, (string) null, DataRowVersion.Current)
    {
    }

    /// <summary>
    /// Constructs an unnamed parameter of the specified type, size, and source column
    /// </summary>
    /// <param name="parameterType">The data type</param>
    /// <param name="parameterSize">The size of the parameter</param>
    /// <param name="sourceColumn">The source column</param>
    public SQLiteParameter(DbType parameterType, int parameterSize, string sourceColumn)
      : this((string) null, parameterType, parameterSize, sourceColumn, DataRowVersion.Current)
    {
    }

    /// <summary>
    /// Constructs an unnamed parameter of the specified type, size, source column and row version
    /// </summary>
    /// <param name="parameterType">The data type</param>
    /// <param name="parameterSize">The size of the parameter</param>
    /// <param name="sourceColumn">The source column</param>
    /// <param name="rowVersion">The row version information</param>
    public SQLiteParameter(
      DbType parameterType,
      int parameterSize,
      string sourceColumn,
      DataRowVersion rowVersion)
      : this((string) null, parameterType, parameterSize, sourceColumn, rowVersion)
    {
    }

    /// <summary>The command associated with this parameter.</summary>
    public IDbCommand Command
    {
      get => this._command;
      set => this._command = value;
    }

    /// <summary>Whether or not the parameter can contain a null value</summary>
    public override bool IsNullable
    {
      get => this._nullable;
      set => this._nullable = value;
    }

    /// <summary>Returns the datatype of the parameter</summary>
    [RefreshProperties(RefreshProperties.All)]
    [DbProviderSpecificTypeProperty(true)]
    public override DbType DbType
    {
      get
      {
        if (this._dbType != ~DbType.AnsiString)
          return this._dbType;
        return this._objValue != null && this._objValue != DBNull.Value ? SQLiteConvert.TypeToDbType(this._objValue.GetType()) : DbType.String;
      }
      set => this._dbType = value;
    }

    /// <summary>Supports only input parameters</summary>
    public override ParameterDirection Direction
    {
      get => ParameterDirection.Input;
      set
      {
        if (value != ParameterDirection.Input)
          throw new NotSupportedException();
      }
    }

    /// <summary>Returns the parameter name</summary>
    public override string ParameterName
    {
      get => this._parameterName;
      set => this._parameterName = value;
    }

    /// <summary>
    /// Resets the DbType of the parameter so it can be inferred from the value
    /// </summary>
    public override void ResetDbType() => this._dbType = ~DbType.AnsiString;

    /// <summary>Returns the size of the parameter</summary>
    [DefaultValue(0)]
    public override int Size
    {
      get => this._dataSize;
      set => this._dataSize = value;
    }

    /// <summary>Gets/sets the source column</summary>
    public override string SourceColumn
    {
      get => this._sourceColumn;
      set => this._sourceColumn = value;
    }

    /// <summary>
    /// Used by DbCommandBuilder to determine the mapping for nullable fields
    /// </summary>
    public override bool SourceColumnNullMapping
    {
      get => this._nullMapping;
      set => this._nullMapping = value;
    }

    /// <summary>Gets and sets the row version</summary>
    public override DataRowVersion SourceVersion
    {
      get => this._rowVersion;
      set => this._rowVersion = value;
    }

    /// <summary>
    /// Gets and sets the parameter value.  If no datatype was specified, the datatype will assume the type from the value given.
    /// </summary>
    [TypeConverter(typeof (StringConverter))]
    [RefreshProperties(RefreshProperties.All)]
    public override object Value
    {
      get => this._objValue;
      set
      {
        this._objValue = value;
        if (this._dbType != ~DbType.AnsiString || this._objValue == null || this._objValue == DBNull.Value)
          return;
        this._dbType = SQLiteConvert.TypeToDbType(this._objValue.GetType());
      }
    }

    /// <summary>
    /// The database type name associated with this parameter, if any.
    /// </summary>
    public string TypeName
    {
      get => this._typeName;
      set => this._typeName = value;
    }

    /// <summary>Clones a parameter</summary>
    /// <returns>A new, unassociated SQLiteParameter</returns>
    public object Clone() => (object) new SQLiteParameter(this);
  }
}

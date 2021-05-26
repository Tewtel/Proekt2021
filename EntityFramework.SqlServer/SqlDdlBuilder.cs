// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlDdlBuilder
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.SqlServer.Utilities;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace System.Data.Entity.SqlServer
{
  internal sealed class SqlDdlBuilder
  {
    private readonly StringBuilder unencodedStringBuilder = new StringBuilder();
    private readonly HashSet<EntitySet> ignoredEntitySets = new HashSet<EntitySet>();

    internal static string CreateObjectsScript(
      StoreItemCollection itemCollection,
      bool createSchemas)
    {
      SqlDdlBuilder sqlDdlBuilder = new SqlDdlBuilder();
      foreach (EntityContainer entityContainer in itemCollection.GetItems<EntityContainer>())
      {
        IOrderedEnumerable<EntitySet> source = entityContainer.BaseEntitySets.OfType<EntitySet>().OrderBy<EntitySet, string>((Func<EntitySet, string>) (s => s.Name));
        if (createSchemas)
        {
          foreach (string schema in (IEnumerable<string>) new HashSet<string>(source.Select<EntitySet, string>((Func<EntitySet, string>) (s => SqlDdlBuilder.GetSchemaName(s)))).OrderBy<string, string>((Func<string, string>) (s => s)))
          {
            if (schema != "dbo")
              sqlDdlBuilder.AppendCreateSchema(schema);
          }
        }
        foreach (EntitySet entitySet in (IEnumerable<EntitySet>) entityContainer.BaseEntitySets.OfType<EntitySet>().OrderBy<EntitySet, string>((Func<EntitySet, string>) (s => s.Name)))
          sqlDdlBuilder.AppendCreateTable(entitySet);
        foreach (AssociationSet associationSet in (IEnumerable<AssociationSet>) entityContainer.BaseEntitySets.OfType<AssociationSet>().OrderBy<AssociationSet, string>((Func<AssociationSet, string>) (s => s.Name)))
          sqlDdlBuilder.AppendCreateForeignKeys(associationSet);
      }
      return sqlDdlBuilder.GetCommandText();
    }

    internal static string CreateDatabaseScript(
      string databaseName,
      string dataFileName,
      string logFileName)
    {
      SqlDdlBuilder sqlDdlBuilder = new SqlDdlBuilder();
      sqlDdlBuilder.AppendSql("create database ");
      sqlDdlBuilder.AppendIdentifier(databaseName);
      if (dataFileName != null)
      {
        sqlDdlBuilder.AppendSql(" on primary ");
        sqlDdlBuilder.AppendFileName(dataFileName);
        sqlDdlBuilder.AppendSql(" log on ");
        sqlDdlBuilder.AppendFileName(logFileName);
      }
      return sqlDdlBuilder.unencodedStringBuilder.ToString();
    }

    internal static string SetDatabaseOptionsScript(SqlVersion sqlVersion, string databaseName)
    {
      if (sqlVersion < SqlVersion.Sql9)
        return string.Empty;
      SqlDdlBuilder sqlDdlBuilder = new SqlDdlBuilder();
      sqlDdlBuilder.AppendSql("if serverproperty('EngineEdition') <> 5 execute sp_executesql ");
      sqlDdlBuilder.AppendStringLiteral(SqlDdlBuilder.SetReadCommittedSnapshotScript(databaseName));
      return sqlDdlBuilder.unencodedStringBuilder.ToString();
    }

    private static string SetReadCommittedSnapshotScript(string databaseName)
    {
      SqlDdlBuilder sqlDdlBuilder = new SqlDdlBuilder();
      sqlDdlBuilder.AppendSql("alter database ");
      sqlDdlBuilder.AppendIdentifier(databaseName);
      sqlDdlBuilder.AppendSql(" set read_committed_snapshot on");
      return sqlDdlBuilder.unencodedStringBuilder.ToString();
    }

    internal static string CreateDatabaseExistsScript(string databaseName)
    {
      SqlDdlBuilder sqlDdlBuilder = new SqlDdlBuilder();
      sqlDdlBuilder.AppendSql("IF db_id(");
      sqlDdlBuilder.AppendStringLiteral(databaseName);
      sqlDdlBuilder.AppendSql(") IS NOT NULL SELECT 1 ELSE SELECT Count(*) FROM sys.databases WHERE [name]=");
      sqlDdlBuilder.AppendStringLiteral(databaseName);
      return sqlDdlBuilder.unencodedStringBuilder.ToString();
    }

    private static void AppendSysDatabases(SqlDdlBuilder builder, bool useDeprecatedSystemTable)
    {
      if (useDeprecatedSystemTable)
        builder.AppendSql("sysdatabases");
      else
        builder.AppendSql("sys.databases");
    }

    internal static string CreateGetDatabaseNamesBasedOnFileNameScript(
      string databaseFileName,
      bool useDeprecatedSystemTable)
    {
      SqlDdlBuilder builder = new SqlDdlBuilder();
      builder.AppendSql("SELECT [d].[name] FROM ");
      SqlDdlBuilder.AppendSysDatabases(builder, useDeprecatedSystemTable);
      builder.AppendSql(" AS [d] ");
      if (!useDeprecatedSystemTable)
        builder.AppendSql("INNER JOIN sys.master_files AS [f] ON [f].[database_id] = [d].[database_id]");
      builder.AppendSql(" WHERE [");
      if (useDeprecatedSystemTable)
        builder.AppendSql("filename");
      else
        builder.AppendSql("f].[physical_name");
      builder.AppendSql("]=");
      builder.AppendStringLiteral(databaseFileName);
      return builder.unencodedStringBuilder.ToString();
    }

    internal static string CreateCountDatabasesBasedOnFileNameScript(
      string databaseFileName,
      bool useDeprecatedSystemTable)
    {
      SqlDdlBuilder sqlDdlBuilder = new SqlDdlBuilder();
      sqlDdlBuilder.AppendSql("SELECT Count(*) FROM ");
      if (useDeprecatedSystemTable)
        sqlDdlBuilder.AppendSql("sysdatabases");
      if (!useDeprecatedSystemTable)
        sqlDdlBuilder.AppendSql("sys.master_files");
      sqlDdlBuilder.AppendSql(" WHERE [");
      if (useDeprecatedSystemTable)
        sqlDdlBuilder.AppendSql("filename");
      else
        sqlDdlBuilder.AppendSql("physical_name");
      sqlDdlBuilder.AppendSql("]=");
      sqlDdlBuilder.AppendStringLiteral(databaseFileName);
      return sqlDdlBuilder.unencodedStringBuilder.ToString();
    }

    internal static string DropDatabaseScript(string databaseName)
    {
      SqlDdlBuilder sqlDdlBuilder = new SqlDdlBuilder();
      sqlDdlBuilder.AppendSql("drop database ");
      sqlDdlBuilder.AppendIdentifier(databaseName);
      return sqlDdlBuilder.unencodedStringBuilder.ToString();
    }

    internal string GetCommandText() => this.unencodedStringBuilder.ToString();

    internal static string GetSchemaName(EntitySet entitySet) => entitySet.GetMetadataPropertyValue<string>("Schema") ?? entitySet.EntityContainer.Name;

    internal static string GetTableName(EntitySet entitySet) => entitySet.GetMetadataPropertyValue<string>("Table") ?? entitySet.Name;

    private void AppendCreateForeignKeys(AssociationSet associationSet)
    {
      ReferentialConstraint referentialConstraint = associationSet.ElementType.ReferentialConstraints.Single<ReferentialConstraint>();
      AssociationSetEnd associationSetEnd1 = associationSet.AssociationSetEnds[referentialConstraint.FromRole.Name];
      AssociationSetEnd associationSetEnd2 = associationSet.AssociationSetEnds[referentialConstraint.ToRole.Name];
      if (this.ignoredEntitySets.Contains(associationSetEnd1.EntitySet) || this.ignoredEntitySets.Contains(associationSetEnd2.EntitySet))
      {
        this.AppendSql("-- Ignoring association set with participating entity set with defining query: ");
        this.AppendIdentifierEscapeNewLine(associationSet.Name);
      }
      else
      {
        this.AppendSql("alter table ");
        this.AppendIdentifier(associationSetEnd2.EntitySet);
        this.AppendSql(" add constraint ");
        this.AppendIdentifier(associationSet.Name);
        this.AppendSql(" foreign key (");
        this.AppendIdentifiers((IEnumerable<EdmProperty>) referentialConstraint.ToProperties);
        this.AppendSql(") references ");
        this.AppendIdentifier(associationSetEnd1.EntitySet);
        this.AppendSql("(");
        this.AppendIdentifiers((IEnumerable<EdmProperty>) referentialConstraint.FromProperties);
        this.AppendSql(")");
        if (associationSetEnd1.CorrespondingAssociationEndMember.DeleteBehavior == OperationAction.Cascade)
          this.AppendSql(" on delete cascade");
        this.AppendSql(";");
      }
      this.AppendNewLine();
    }

    private void AppendCreateTable(EntitySet entitySet)
    {
      if (entitySet.GetMetadataPropertyValue<string>("DefiningQuery") != null)
      {
        this.AppendSql("-- Ignoring entity set with defining query: ");
        this.AppendIdentifier(entitySet, new Action<string>(this.AppendIdentifierEscapeNewLine));
        this.ignoredEntitySets.Add(entitySet);
      }
      else
      {
        this.AppendSql("create table ");
        this.AppendIdentifier(entitySet);
        this.AppendSql(" (");
        this.AppendNewLine();
        foreach (EdmProperty property in entitySet.ElementType.Properties)
        {
          this.AppendSql("    ");
          this.AppendIdentifier(property.Name);
          this.AppendSql(" ");
          this.AppendType(property);
          this.AppendSql(",");
          this.AppendNewLine();
        }
        this.AppendSql("    primary key (");
        this.AppendJoin<EdmMember>((IEnumerable<EdmMember>) entitySet.ElementType.KeyMembers, (Action<EdmMember>) (k => this.AppendIdentifier(k.Name)), ", ");
        this.AppendSql(")");
        this.AppendNewLine();
        this.AppendSql(");");
      }
      this.AppendNewLine();
    }

    private void AppendCreateSchema(string schema)
    {
      this.AppendSql("if (schema_id(");
      this.AppendStringLiteral(schema);
      this.AppendSql(") is null) exec(");
      SqlDdlBuilder sqlDdlBuilder = new SqlDdlBuilder();
      sqlDdlBuilder.AppendSql("create schema ");
      sqlDdlBuilder.AppendIdentifier(schema);
      this.AppendStringLiteral(sqlDdlBuilder.unencodedStringBuilder.ToString());
      this.AppendSql(");");
      this.AppendNewLine();
    }

    private void AppendIdentifier(EntitySet table) => this.AppendIdentifier(table, new Action<string>(this.AppendIdentifier));

    private void AppendIdentifier(EntitySet table, Action<string> AppendIdentifierEscape)
    {
      string schemaName = SqlDdlBuilder.GetSchemaName(table);
      string tableName = SqlDdlBuilder.GetTableName(table);
      if (schemaName != null)
      {
        AppendIdentifierEscape(schemaName);
        this.AppendSql(".");
      }
      AppendIdentifierEscape(tableName);
    }

    private void AppendStringLiteral(string literalValue) => this.AppendSql("N'" + literalValue.Replace("'", "''") + "'");

    private void AppendIdentifiers(IEnumerable<EdmProperty> properties) => this.AppendJoin<EdmProperty>(properties, (Action<EdmProperty>) (p => this.AppendIdentifier(p.Name)), ", ");

    private void AppendIdentifier(string identifier) => this.AppendSql("[" + identifier.Replace("]", "]]") + "]");

    private void AppendIdentifierEscapeNewLine(string identifier) => this.AppendIdentifier(identifier.Replace("\r", "\r--").Replace("\n", "\n--"));

    private void AppendFileName(string path)
    {
      this.AppendSql("(name=");
      this.AppendStringLiteral(Path.GetFileName(path));
      this.AppendSql(", filename=");
      this.AppendStringLiteral(path);
      this.AppendSql(")");
    }

    private void AppendJoin<T>(
      IEnumerable<T> elements,
      Action<T> appendElement,
      string unencodedSeparator)
    {
      bool flag = true;
      foreach (T element in elements)
      {
        if (flag)
          flag = false;
        else
          this.AppendSql(unencodedSeparator);
        appendElement(element);
      }
    }

    private void AppendType(EdmProperty column)
    {
      TypeUsage typeUsage = column.TypeUsage;
      bool flag = false;
      Facet facet;
      if (typeUsage.EdmType.Name == "binary" && 8 == typeUsage.GetMaxLength() && (column.TypeUsage.Facets.TryGetValue("StoreGeneratedPattern", false, out facet) && facet.Value != null) && StoreGeneratedPattern.Computed == (StoreGeneratedPattern) facet.Value)
      {
        flag = true;
        this.AppendIdentifier("rowversion");
      }
      else
      {
        string name = typeUsage.EdmType.Name;
        if (typeUsage.EdmType.BuiltInTypeKind == BuiltInTypeKind.PrimitiveType && name.EndsWith("(max)", StringComparison.Ordinal))
        {
          this.AppendIdentifier(name.Substring(0, name.Length - "(max)".Length));
          this.AppendSql("(max)");
        }
        else
          this.AppendIdentifier(name);
        switch (typeUsage.EdmType.Name)
        {
          case "binary":
          case "char":
          case "nchar":
          case "nvarchar":
          case "varbinary":
          case "varchar":
            this.AppendSqlInvariantFormat("({0})", (object) typeUsage.GetMaxLength());
            break;
          case "datetime2":
          case "datetimeoffset":
          case "time":
            this.AppendSqlInvariantFormat("({0})", (object) typeUsage.GetPrecision());
            break;
          case "decimal":
          case "numeric":
            this.AppendSqlInvariantFormat("({0}, {1})", (object) typeUsage.GetPrecision(), (object) typeUsage.GetScale());
            break;
        }
      }
      this.AppendSql(column.Nullable ? " null" : " not null");
      if (flag || !column.TypeUsage.Facets.TryGetValue("StoreGeneratedPattern", false, out facet) || (facet.Value == null || (StoreGeneratedPattern) facet.Value != StoreGeneratedPattern.Identity))
        return;
      if (typeUsage.EdmType.Name == "uniqueidentifier")
        this.AppendSql(" default newid()");
      else
        this.AppendSql(" identity");
    }

    private void AppendSql(string text) => this.unencodedStringBuilder.Append(text);

    private void AppendNewLine() => this.unencodedStringBuilder.Append("\r\n");

    private void AppendSqlInvariantFormat(string format, params object[] args) => this.unencodedStringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, format, args);
  }
}

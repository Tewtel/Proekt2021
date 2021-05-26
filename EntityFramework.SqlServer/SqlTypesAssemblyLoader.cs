// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlTypesAssemblyLoader
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.SqlServer
{
  internal class SqlTypesAssemblyLoader
  {
    private const string AssemblyNameTemplate = "Microsoft.SqlServer.Types, Version={0}.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91";
    private static readonly SqlTypesAssemblyLoader _instance = new SqlTypesAssemblyLoader();
    private readonly IEnumerable<string> _preferredSqlTypesAssemblies;
    private readonly Lazy<SqlTypesAssembly> _latestVersion;

    public static SqlTypesAssemblyLoader DefaultInstance => SqlTypesAssemblyLoader._instance;

    public SqlTypesAssemblyLoader(IEnumerable<string> assemblyNames = null)
    {
      if (assemblyNames != null)
      {
        this._preferredSqlTypesAssemblies = assemblyNames;
      }
      else
      {
        List<string> source = new List<string>()
        {
          SqlTypesAssemblyLoader.GenerateSqlServerTypesAssemblyName(11),
          SqlTypesAssemblyLoader.GenerateSqlServerTypesAssemblyName(10)
        };
        for (int version = 20; version > 11; --version)
          source.Add(SqlTypesAssemblyLoader.GenerateSqlServerTypesAssemblyName(version));
        this._preferredSqlTypesAssemblies = (IEnumerable<string>) source.ToList<string>();
      }
      this._latestVersion = new Lazy<SqlTypesAssembly>(new Func<SqlTypesAssembly>(this.BindToLatest), true);
    }

    private static string GenerateSqlServerTypesAssemblyName(int version) => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Microsoft.SqlServer.Types, Version={0}.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91", (object) version);

    public SqlTypesAssemblyLoader(SqlTypesAssembly assembly) => this._latestVersion = new Lazy<SqlTypesAssembly>((Func<SqlTypesAssembly>) (() => assembly), true);

    public virtual SqlTypesAssembly TryGetSqlTypesAssembly() => this._latestVersion.Value;

    public virtual SqlTypesAssembly GetSqlTypesAssembly() => this._latestVersion.Value ?? throw new InvalidOperationException(System.Data.Entity.SqlServer.Resources.Strings.SqlProvider_SqlTypesAssemblyNotFound);

    public virtual bool TryGetSqlTypesAssembly(Assembly assembly, out SqlTypesAssembly sqlAssembly)
    {
      if (this.IsKnownAssembly(assembly))
      {
        sqlAssembly = new SqlTypesAssembly(assembly);
        return true;
      }
      sqlAssembly = (SqlTypesAssembly) null;
      return false;
    }

    private SqlTypesAssembly BindToLatest()
    {
      Assembly sqlSpatialAssembly = (Assembly) null;
      IEnumerable<string> strings;
      if (SqlProviderServices.SqlServerTypesAssemblyName == null)
        strings = this._preferredSqlTypesAssemblies;
      else
        strings = (IEnumerable<string>) new string[1]
        {
          SqlProviderServices.SqlServerTypesAssemblyName
        };
      foreach (string assemblyName in strings)
      {
        AssemblyName assemblyRef = new AssemblyName(assemblyName);
        try
        {
          sqlSpatialAssembly = Assembly.Load(assemblyRef);
          break;
        }
        catch (FileNotFoundException ex)
        {
        }
        catch (FileLoadException ex)
        {
        }
      }
      return sqlSpatialAssembly != (Assembly) null ? new SqlTypesAssembly(sqlSpatialAssembly) : (SqlTypesAssembly) null;
    }

    private bool IsKnownAssembly(Assembly assembly)
    {
      foreach (string sqlTypesAssembly in this._preferredSqlTypesAssemblies)
      {
        if (SqlTypesAssemblyLoader.AssemblyNamesMatch(assembly.FullName, new AssemblyName(sqlTypesAssembly)))
          return true;
      }
      return false;
    }

    private static bool AssemblyNamesMatch(
      string infoRowProviderAssemblyName,
      AssemblyName targetAssemblyName)
    {
      if (string.IsNullOrWhiteSpace(infoRowProviderAssemblyName))
        return false;
      AssemblyName assemblyName;
      try
      {
        assemblyName = new AssemblyName(infoRowProviderAssemblyName);
      }
      catch (Exception ex)
      {
        return false;
      }
      if (!string.Equals(targetAssemblyName.Name, assemblyName.Name, StringComparison.OrdinalIgnoreCase) || targetAssemblyName.Version == (Version) null || (assemblyName.Version == (Version) null || targetAssemblyName.Version.Major != assemblyName.Version.Major) || targetAssemblyName.Version.Minor != assemblyName.Version.Minor)
        return false;
      byte[] publicKeyToken = targetAssemblyName.GetPublicKeyToken();
      return publicKeyToken != null && ((IEnumerable<byte>) publicKeyToken).SequenceEqual<byte>((IEnumerable<byte>) assemblyName.GetPublicKeyToken());
    }
  }
}

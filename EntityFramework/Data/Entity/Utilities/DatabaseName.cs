// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.DatabaseName
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;
using System.Globalization;
using System.Text.RegularExpressions;

namespace System.Data.Entity.Utilities
{
  internal class DatabaseName
  {
    private const string NamePartRegex = "(?:(?:\\[(?<part{0}>(?:(?:\\]\\])|[^\\]])+)\\])|(?<part{0}>[^\\.\\[\\]]+))";
    private static readonly Regex _partExtractor = new Regex(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "^{0}(?:\\.{1})?$", (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, "(?:(?:\\[(?<part{0}>(?:(?:\\]\\])|[^\\]])+)\\])|(?<part{0}>[^\\.\\[\\]]+))", (object) 1), (object) string.Format((IFormatProvider) CultureInfo.InvariantCulture, "(?:(?:\\[(?<part{0}>(?:(?:\\]\\])|[^\\]])+)\\])|(?<part{0}>[^\\.\\[\\]]+))", (object) 2)), RegexOptions.Compiled);
    private readonly string _name;
    private readonly string _schema;

    public static DatabaseName Parse(string name)
    {
      Match match = DatabaseName._partExtractor.Match(name.Trim());
      if (!match.Success)
        throw Error.InvalidDatabaseName((object) name);
      string str = match.Groups["part1"].Value.Replace("]]", "]");
      string name1 = match.Groups["part2"].Value.Replace("]]", "]");
      return string.IsNullOrWhiteSpace(name1) ? new DatabaseName(str) : new DatabaseName(name1, str);
    }

    public DatabaseName(string name)
      : this(name, (string) null)
    {
    }

    public DatabaseName(string name, string schema)
    {
      this._name = name;
      this._schema = !string.IsNullOrEmpty(schema) ? schema : (string) null;
    }

    public string Name => this._name;

    public string Schema => this._schema;

    public override string ToString()
    {
      string str = DatabaseName.Escape(this._name);
      if (this._schema != null)
        str = DatabaseName.Escape(this._schema) + "." + str;
      return str;
    }

    private static string Escape(string name) => name.IndexOfAny(new char[3]
    {
      ']',
      '[',
      '.'
    }) == -1 ? name : "[" + name.Replace("]", "]]") + "]";

    public bool Equals(DatabaseName other)
    {
      if (other == null)
        return false;
      if (this == other)
        return true;
      return string.Equals(other._name, this._name, StringComparison.Ordinal) && string.Equals(other._schema, this._schema, StringComparison.Ordinal);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return obj.GetType() == typeof (DatabaseName) && this.Equals((DatabaseName) obj);
    }

    public override int GetHashCode() => this._name.GetHashCode() * 397 ^ (this._schema != null ? this._schema.GetHashCode() : 0);
  }
}

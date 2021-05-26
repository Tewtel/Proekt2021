// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.Utilities.StringExtensions
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Collections.Generic;
using System.Data.Entity.SqlServer.Resources;
using System.Globalization;
using System.Text.RegularExpressions;

namespace System.Data.Entity.SqlServer.Utilities
{
  internal static class StringExtensions
  {
    private const string StartCharacterExp = "[\\p{L}\\p{Nl}_]";
    private const string OtherCharacterExp = "[\\p{L}\\p{Nl}\\p{Nd}\\p{Mn}\\p{Mc}\\p{Pc}\\p{Cf}]";
    private const string NameExp = "[\\p{L}\\p{Nl}_][\\p{L}\\p{Nl}\\p{Nd}\\p{Mn}\\p{Mc}\\p{Pc}\\p{Cf}]{0,}";
    private static readonly Regex _undottedNameValidator = new Regex("^[\\p{L}\\p{Nl}_][\\p{L}\\p{Nl}\\p{Nd}\\p{Mn}\\p{Mc}\\p{Pc}\\p{Cf}]{0,}$", RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex _migrationIdPattern = new Regex("\\d{15}_.+");
    private static readonly string[] _lineEndings = new string[2]
    {
      "\r\n",
      "\n"
    };

    public static bool EqualsIgnoreCase(this string s1, string s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);

    internal static bool EqualsOrdinal(this string s1, string s2) => string.Equals(s1, s2, StringComparison.Ordinal);

    public static string MigrationName(this string migrationId) => migrationId.Substring(16);

    public static string RestrictTo(this string s, int size) => string.IsNullOrEmpty(s) || s.Length <= size ? s : s.Substring(0, size);

    public static void EachLine(this string s, Action<string> action) => ((IEnumerable<string>) s.Split(StringExtensions._lineEndings, StringSplitOptions.None)).Each<string>(action);

    public static bool IsValidMigrationId(this string migrationId) => StringExtensions._migrationIdPattern.IsMatch(migrationId) || migrationId == "0";

    public static bool IsAutomaticMigration(this string migrationId) => migrationId.EndsWith(Strings.AutomaticMigration, StringComparison.Ordinal);

    public static string ToAutomaticMigrationId(this string migrationId) => (Convert.ToInt64(migrationId.Substring(0, 15), (IFormatProvider) CultureInfo.InvariantCulture) - 1L).ToString() + migrationId.Substring(15) + "_" + Strings.AutomaticMigration;

    public static bool IsValidUndottedName(this string name) => !string.IsNullOrEmpty(name) && StringExtensions._undottedNameValidator.IsMatch(name);
  }
}

// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.Structures.CqlIdentifiers
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Globalization;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.Structures
{
  internal class CqlIdentifiers : InternalBase
  {
    private readonly Set<string> m_identifiers;

    internal CqlIdentifiers() => this.m_identifiers = new Set<string>((IEqualityComparer<string>) StringComparer.Ordinal);

    internal string GetFromVariable(int num) => this.GetNonConflictingName("_from", num);

    internal string GetBlockAlias(int num) => this.GetNonConflictingName("T", num);

    internal string GetBlockAlias() => this.GetNonConflictingName("T", -1);

    internal void AddIdentifier(string identifier) => this.m_identifiers.Add(identifier.ToLower(CultureInfo.InvariantCulture));

    private string GetNonConflictingName(string prefix, int number)
    {
      string str1;
      if (number >= 0)
        str1 = StringUtil.FormatInvariant("{0}{1}", (object) prefix, (object) number);
      else
        str1 = prefix;
      string str2 = str1;
      if (!this.m_identifiers.Contains(str2.ToLower(CultureInfo.InvariantCulture)))
        return str2;
      for (int index = 0; index < int.MaxValue; ++index)
      {
        string str3;
        if (number < 0)
          str3 = StringUtil.FormatInvariant("{0}_{1}", (object) prefix, (object) index);
        else
          str3 = StringUtil.FormatInvariant("{0}_{1}_{2}", (object) prefix, (object) index, (object) number);
        if (!this.m_identifiers.Contains(str3.ToLower(CultureInfo.InvariantCulture)))
          return str3;
      }
      return (string) null;
    }

    internal override void ToCompactString(StringBuilder builder) => this.m_identifiers.ToCompactString(builder);
  }
}

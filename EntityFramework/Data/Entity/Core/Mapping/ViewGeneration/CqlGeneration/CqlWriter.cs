﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.CqlGeneration.CqlWriter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping.ViewGeneration.Utils;
using System.Data.Entity.Core.Metadata.Edm;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Data.Entity.Core.Mapping.ViewGeneration.CqlGeneration
{
  internal static class CqlWriter
  {
    private static readonly Regex _wordIdentifierRegex = new Regex("^[_A-Za-z]\\w*$", RegexOptions.Compiled | RegexOptions.ECMAScript);

    internal static string GetQualifiedName(string blockName, string field) => StringUtil.FormatInvariant("{0}.{1}", (object) blockName, (object) field);

    internal static void AppendEscapedTypeName(StringBuilder builder, EdmType type) => CqlWriter.AppendEscapedName(builder, CqlWriter.GetQualifiedName(type.NamespaceName, type.Name));

    internal static void AppendEscapedQualifiedName(
      StringBuilder builder,
      string name1,
      string name2)
    {
      CqlWriter.AppendEscapedName(builder, name1);
      builder.Append('.');
      CqlWriter.AppendEscapedName(builder, name2);
    }

    internal static void AppendEscapedName(StringBuilder builder, string name)
    {
      if (CqlWriter._wordIdentifierRegex.IsMatch(name) && !ExternalCalls.IsReservedKeyword(name))
      {
        builder.Append(name);
      }
      else
      {
        string str = name.Replace("]", "]]");
        builder.Append('[').Append(str).Append(']');
      }
    }
  }
}

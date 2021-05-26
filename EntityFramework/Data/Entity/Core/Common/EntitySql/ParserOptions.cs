// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.ParserOptions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class ParserOptions
  {
    internal ParserOptions.CompilationMode ParserCompilationMode;

    internal StringComparer NameComparer => !this.NameComparisonCaseInsensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;

    internal bool NameComparisonCaseInsensitive => this.ParserCompilationMode != ParserOptions.CompilationMode.RestrictedViewGenerationMode;

    internal enum CompilationMode
    {
      NormalMode,
      RestrictedViewGenerationMode,
      UserViewGenerationMode,
    }
  }
}

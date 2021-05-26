// Decompiled with JetBrains decompiler
// Type: System.Linq.Expressions.Internal.Error
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Linq.Expressions.Internal
{
  internal static class Error
  {
    internal static Exception UnhandledExpressionType(ExpressionType expressionType) => (Exception) new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_UnhandledExpressionType((object) expressionType));

    internal static Exception UnhandledBindingType(MemberBindingType memberBindingType) => (Exception) new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_UnhandledBindingType((object) memberBindingType));
  }
}

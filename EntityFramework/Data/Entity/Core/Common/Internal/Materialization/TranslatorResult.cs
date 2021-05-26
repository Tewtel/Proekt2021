// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Internal.Materialization.TranslatorResult
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Objects.Internal;
using System.Linq.Expressions;

namespace System.Data.Entity.Core.Common.Internal.Materialization
{
  internal class TranslatorResult
  {
    private readonly Expression ReturnedExpression;
    private readonly Type RequestedType;

    internal TranslatorResult(Expression returnedExpression, Type requestedType)
    {
      this.RequestedType = requestedType;
      this.ReturnedExpression = returnedExpression;
    }

    internal Expression Expression => CodeGenEmitter.Emit_EnsureType(this.ReturnedExpression, this.RequestedType);

    internal Expression UnconvertedExpression => this.ReturnedExpression;

    internal Expression UnwrappedExpression => !typeof (IEntityWrapper).IsAssignableFrom(this.ReturnedExpression.Type) ? this.ReturnedExpression : CodeGenEmitter.Emit_UnwrapAndEnsureType(this.ReturnedExpression, this.RequestedType);
  }
}

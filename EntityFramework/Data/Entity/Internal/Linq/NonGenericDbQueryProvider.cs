// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.Linq.NonGenericDbQueryProvider
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.Internal.Linq
{
  internal class NonGenericDbQueryProvider : DbQueryProvider
  {
    public NonGenericDbQueryProvider(InternalContext internalContext, IInternalQuery internalQuery)
      : base(internalContext, internalQuery)
    {
    }

    public override IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression>(expression, nameof (expression));
      ObjectQuery objectQuery = this.CreateObjectQuery(expression);
      return typeof (TElement) != ((IQueryable) objectQuery).ElementType ? (IQueryable<TElement>) this.CreateQuery(objectQuery) : (IQueryable<TElement>) new InternalDbQuery<TElement>((IInternalQuery<TElement>) new InternalQuery<TElement>(this.InternalContext, objectQuery));
    }

    public override IQueryable CreateQuery(Expression expression)
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression>(expression, nameof (expression));
      return this.CreateQuery(this.CreateObjectQuery(expression));
    }

    private IQueryable CreateQuery(ObjectQuery objectQuery)
    {
      IInternalQuery internalQuery = this.CreateInternalQuery(objectQuery);
      return (IQueryable) ((IEnumerable<ConstructorInfo>) typeof (InternalDbQuery<>).MakeGenericType(internalQuery.ElementType).GetConstructors(BindingFlags.Instance | BindingFlags.Public)).Single<ConstructorInfo>().Invoke(new object[1]
      {
        (object) internalQuery
      });
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Internal.Materialization.CompensatingCollection`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Data.Entity.Core.Common.Internal.Materialization
{
  internal class CompensatingCollection<TElement> : 
    IOrderedQueryable<TElement>,
    IQueryable<TElement>,
    IEnumerable<TElement>,
    IEnumerable,
    IQueryable,
    IOrderedQueryable,
    IOrderedEnumerable<TElement>
  {
    private readonly IEnumerable<TElement> _source;
    private readonly Expression _expression;

    public CompensatingCollection(IEnumerable<TElement> source)
    {
      this._source = source;
      this._expression = (Expression) Expression.Constant((object) source);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._source.GetEnumerator();

    IEnumerator<TElement> IEnumerable<TElement>.GetEnumerator() => this._source.GetEnumerator();

    IOrderedEnumerable<TElement> IOrderedEnumerable<TElement>.CreateOrderedEnumerable<K>(
      Func<TElement, K> keySelector,
      IComparer<K> comparer,
      bool descending)
    {
      throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_CreateOrderedEnumerableNotSupported);
    }

    Type IQueryable.ElementType => typeof (TElement);

    Expression IQueryable.Expression => this._expression;

    IQueryProvider IQueryable.Provider => throw new NotSupportedException(System.Data.Entity.Resources.Strings.ELinq_UnsupportedQueryableMethod);
  }
}

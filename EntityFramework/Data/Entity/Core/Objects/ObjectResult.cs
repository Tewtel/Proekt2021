// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ObjectResult
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.Core.Objects
{
  /// <summary>
  /// This class implements IEnumerable and IDisposable. Instance of this class
  /// is returned from ObjectQuery.Execute method.
  /// </summary>
  public abstract class ObjectResult : IEnumerable, IDisposable, IListSource, IDbAsyncEnumerable
  {
    /// <summary>
    ///     This constructor is intended only for use when creating test doubles that will override members
    ///     with mocked or faked behavior. Use of this constructor for other purposes may result in unexpected
    ///     behavior including but not limited to throwing <see cref="T:System.NullReferenceException" />.
    /// </summary>
    protected internal ObjectResult()
    {
    }

    /// <inheritdoc />
    IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator() => this.GetAsyncEnumeratorInternal();

    /// <summary>Returns an enumerator that iterates through the query results.</summary>
    /// <returns>An enumerator that iterates through the query results.</returns>
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumeratorInternal();

    /// <summary>
    /// IListSource.ContainsListCollection implementation. Always returns false.
    /// </summary>
    bool IListSource.ContainsListCollection => false;

    /// <summary>Returns the results in a format useful for data binding.</summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IList" /> of entity objects.
    /// </returns>
    IList IListSource.GetList() => this.GetIListSourceListInternal();

    /// <summary>
    /// When overridden in a derived class, gets the type of the generic
    /// <see cref="T:System.Data.Entity.Core.Objects.ObjectResult`1" />
    /// .
    /// </summary>
    /// <returns>
    /// The type of the generic <see cref="T:System.Data.Entity.Core.Objects.ObjectResult`1" />.
    /// </returns>
    public abstract Type ElementType { get; }

    /// <summary>Performs tasks associated with freeing, releasing, or resetting resources.</summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>Releases the resources used by the object result.</summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected abstract void Dispose(bool disposing);

    /// <summary>Gets the next result set of a stored procedure.</summary>
    /// <returns>An ObjectResult that enumerates the values of the next result set. Null, if there are no more, or if the ObjectResult is not the result of a stored procedure call.</returns>
    /// <typeparam name="TElement">The type of the element.</typeparam>
    public virtual ObjectResult<TElement> GetNextResult<TElement>() => this.GetNextResultInternal<TElement>();

    internal abstract IDbAsyncEnumerator GetAsyncEnumeratorInternal();

    internal abstract IEnumerator GetEnumeratorInternal();

    internal abstract IList GetIListSourceListInternal();

    internal abstract ObjectResult<TElement> GetNextResultInternal<TElement>();
  }
}

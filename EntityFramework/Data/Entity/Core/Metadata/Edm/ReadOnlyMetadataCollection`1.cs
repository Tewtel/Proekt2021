// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ReadOnlyMetadataCollection`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>
  /// Class representing a read-only wrapper around MetadataCollection
  /// </summary>
  /// <typeparam name="T"> The type of items in this collection </typeparam>
  public class ReadOnlyMetadataCollection<T> : ReadOnlyCollection<T> where T : MetadataItem
  {
    internal ReadOnlyMetadataCollection()
      : base((IList<T>) new MetadataCollection<T>())
    {
    }

    internal ReadOnlyMetadataCollection(MetadataCollection<T> collection)
      : base((IList<T>) collection)
    {
    }

    internal ReadOnlyMetadataCollection(List<T> list)
      : base((IList<T>) MetadataCollection<T>.Wrap(list))
    {
    }

    /// <summary>Gets a value indicating whether this collection is read-only.</summary>
    /// <returns>true if this collection is read-only; otherwise, false.</returns>
    public bool IsReadOnly => true;

    /// <summary>Gets an item from this collection by using the specified identity.</summary>
    /// <returns>An item from this collection.</returns>
    /// <param name="identity">The identity of the item to be searched for.</param>
    public virtual T this[string identity] => ((MetadataCollection<T>) this.Items)[identity];

    internal MetadataCollection<T> Source
    {
      get
      {
        try
        {
          return (MetadataCollection<T>) this.Items;
        }
        finally
        {
          EventHandler sourceAccessed = this.SourceAccessed;
          if (sourceAccessed != null)
            sourceAccessed((object) this, (EventArgs) null);
        }
      }
    }

    internal event EventHandler SourceAccessed;

    /// <summary>Retrieves an item from this collection by using the specified identity.</summary>
    /// <returns>An item from this collection.</returns>
    /// <param name="identity">The identity of the item to be searched for.</param>
    /// <param name="ignoreCase">true to perform the case-insensitive search; otherwise, false. </param>
    public virtual T GetValue(string identity, bool ignoreCase) => ((MetadataCollection<T>) this.Items).GetValue(identity, ignoreCase);

    /// <summary>Determines whether the collection contains an item with the specified identity.</summary>
    /// <returns>true if the collection contains the item to be searched for; otherwise, false. The default is false.</returns>
    /// <param name="identity">The identity of the item.</param>
    public virtual bool Contains(string identity) => ((MetadataCollection<T>) this.Items).ContainsIdentity(identity);

    /// <summary>Retrieves an item from this collection by using the specified identity.</summary>
    /// <returns>true if there is an item that matches the search criteria; otherwise, false. </returns>
    /// <param name="identity">The identity of the item to be searched for.</param>
    /// <param name="ignoreCase">true to perform the case-insensitive search; otherwise, false. </param>
    /// <param name="item">When this method returns, this output parameter contains an item from the collection. If there is no matched item, this output parameter contains null.</param>
    public virtual bool TryGetValue(string identity, bool ignoreCase, out T item) => ((MetadataCollection<T>) this.Items).TryGetValue(identity, ignoreCase, out item);

    /// <summary>Returns an enumerator that can iterate through this collection.</summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.ReadOnlyMetadataCollection`1.Enumerator" /> that can be used to iterate through this
    /// <see cref="T:System.Data.Metadata.Edm.ReadOnlyMetadataCollection" />
    /// .
    /// </returns>
    public ReadOnlyMetadataCollection<T>.Enumerator GetEnumerator() => new ReadOnlyMetadataCollection<T>.Enumerator(this.Items);

    /// <summary>Returns the index of the specified value in this collection.</summary>
    /// <returns>The index of the specified value in this collection.</returns>
    /// <param name="value">A value to seek.</param>
    public new virtual int IndexOf(T value) => base.IndexOf(value);

    /// <summary>The enumerator for MetadataCollection</summary>
    public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
    {
      private int _nextIndex;
      private readonly IList<T> _parent;
      private T _current;

      internal Enumerator(IList<T> collection)
      {
        this._parent = collection;
        this._nextIndex = 0;
        this._current = default (T);
      }

      /// <summary>Gets the member at the current position. </summary>
      /// <returns>The member at the current position.</returns>
      public T Current => this._current;

      /// <summary>Gets the member at the current position</summary>
      object IEnumerator.Current => (object) this.Current;

      /// <summary>Disposes of this enumerator.</summary>
      public void Dispose()
      {
      }

      /// <summary>
      /// Moves to the next member in the collection of type
      /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.ReadOnlyMetadataCollection`1.Enumerator" />
      /// .
      /// </summary>
      /// <returns>
      /// true if the enumerator is moved in the collection of type
      /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.ReadOnlyMetadataCollection`1.EnumeratorCollection" />
      /// ; otherwise, false.
      /// </returns>
      public bool MoveNext()
      {
        if ((uint) this._nextIndex < (uint) this._parent.Count)
        {
          this._current = this._parent[this._nextIndex];
          ++this._nextIndex;
          return true;
        }
        this._current = default (T);
        return false;
      }

      /// <summary>
      /// Positions the enumerator before the first position in the collection of type
      /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.ReadOnlyMetadataCollection`1" />
      /// .
      /// </summary>
      public void Reset()
      {
        this._current = default (T);
        this._nextIndex = 0;
      }
    }
  }
}

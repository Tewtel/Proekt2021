// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.RelationshipEndCollection
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal sealed class RelationshipEndCollection : 
    IList<IRelationshipEnd>,
    ICollection<IRelationshipEnd>,
    IEnumerable<IRelationshipEnd>,
    IEnumerable
  {
    private Dictionary<string, IRelationshipEnd> _endLookup;
    private List<string> _keysInDefOrder;

    public int Count => this.KeysInDefOrder.Count;

    public void Add(IRelationshipEnd end)
    {
      SchemaElement end1 = end as SchemaElement;
      if (!RelationshipEndCollection.IsEndValid(end) || !this.ValidateUniqueName(end1, end.Name))
        return;
      this.EndLookup.Add(end.Name, end);
      this.KeysInDefOrder.Add(end.Name);
    }

    private static bool IsEndValid(IRelationshipEnd end) => !string.IsNullOrEmpty(end.Name);

    private bool ValidateUniqueName(SchemaElement end, string name)
    {
      if (!this.EndLookup.ContainsKey(name))
        return true;
      end.AddError(ErrorCode.AlreadyDefined, EdmSchemaErrorSeverity.Error, (object) Strings.EndNameAlreadyDefinedDuplicate((object) name));
      return false;
    }

    public bool Remove(IRelationshipEnd end)
    {
      if (!RelationshipEndCollection.IsEndValid(end))
        return false;
      this.KeysInDefOrder.Remove(end.Name);
      return this.EndLookup.Remove(end.Name);
    }

    public bool Contains(string name) => this.EndLookup.ContainsKey(name);

    public bool Contains(IRelationshipEnd end) => this.Contains(end.Name);

    public IRelationshipEnd this[int index]
    {
      get => this.EndLookup[this.KeysInDefOrder[index]];
      set => throw new NotSupportedException();
    }

    public IEnumerator<IRelationshipEnd> GetEnumerator() => (IEnumerator<IRelationshipEnd>) new RelationshipEndCollection.Enumerator(this.EndLookup, this.KeysInDefOrder);

    public bool TryGetEnd(string name, out IRelationshipEnd end) => this.EndLookup.TryGetValue(name, out end);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new RelationshipEndCollection.Enumerator(this.EndLookup, this.KeysInDefOrder);

    private Dictionary<string, IRelationshipEnd> EndLookup
    {
      get
      {
        if (this._endLookup == null)
          this._endLookup = new Dictionary<string, IRelationshipEnd>((IEqualityComparer<string>) StringComparer.Ordinal);
        return this._endLookup;
      }
    }

    private List<string> KeysInDefOrder
    {
      get
      {
        if (this._keysInDefOrder == null)
          this._keysInDefOrder = new List<string>();
        return this._keysInDefOrder;
      }
    }

    public void Clear()
    {
      this.EndLookup.Clear();
      this.KeysInDefOrder.Clear();
    }

    public bool IsReadOnly => false;

    int IList<IRelationshipEnd>.IndexOf(IRelationshipEnd end) => throw new NotSupportedException();

    void IList<IRelationshipEnd>.Insert(int index, IRelationshipEnd end) => throw new NotSupportedException();

    void IList<IRelationshipEnd>.RemoveAt(int index) => throw new NotSupportedException();

    public void CopyTo(IRelationshipEnd[] ends, int index)
    {
      foreach (IRelationshipEnd relationshipEnd in this)
        ends[index++] = relationshipEnd;
    }

    private sealed class Enumerator : IEnumerator<IRelationshipEnd>, IDisposable, IEnumerator
    {
      private List<string>.Enumerator _Enumerator;
      private readonly Dictionary<string, IRelationshipEnd> _Data;

      public Enumerator(Dictionary<string, IRelationshipEnd> data, List<string> keysInDefOrder)
      {
        this._Enumerator = keysInDefOrder.GetEnumerator();
        this._Data = data;
      }

      public void Reset() => ((IEnumerator) this._Enumerator).Reset();

      public IRelationshipEnd Current => this._Data[this._Enumerator.Current];

      object IEnumerator.Current => (object) this._Data[this._Enumerator.Current];

      public bool MoveNext() => this._Enumerator.MoveNext();

      public void Dispose()
      {
      }
    }
  }
}

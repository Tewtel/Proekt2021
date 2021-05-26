// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.HtmlAttributeCollection
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace HtmlAgilityPack
{
  /// <summary>
  /// Represents a combined list and collection of HTML nodes.
  /// </summary>
  public class HtmlAttributeCollection : 
    IList<HtmlAttribute>,
    ICollection<HtmlAttribute>,
    IEnumerable<HtmlAttribute>,
    IEnumerable
  {
    internal Dictionary<string, HtmlAttribute> Hashitems = new Dictionary<string, HtmlAttribute>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private HtmlNode _ownernode;
    internal List<HtmlAttribute> items = new List<HtmlAttribute>();

    internal HtmlAttributeCollection(HtmlNode ownernode) => this._ownernode = ownernode;

    /// <summary>
    /// Gets the number of elements actually contained in the list.
    /// </summary>
    public int Count => this.items.Count;

    /// <summary>Gets readonly status of colelction</summary>
    public bool IsReadOnly => false;

    /// <summary>Gets the attribute at the specified index.</summary>
    public HtmlAttribute this[int index]
    {
      get => this.items[index];
      set
      {
        HtmlAttribute htmlAttribute = this.items[index];
        this.items[index] = value;
        if (htmlAttribute.Name != value.Name)
          this.Hashitems.Remove(htmlAttribute.Name);
        this.Hashitems[value.Name] = value;
        value._ownernode = this._ownernode;
        this._ownernode.SetChanged();
      }
    }

    /// <summary>Gets a given attribute from the list using its name.</summary>
    public HtmlAttribute this[string name]
    {
      get
      {
        if (name == null)
          throw new ArgumentNullException(nameof (name));
        HtmlAttribute htmlAttribute;
        return !this.Hashitems.TryGetValue(name, out htmlAttribute) ? (HtmlAttribute) null : htmlAttribute;
      }
      set
      {
        HtmlAttribute htmlAttribute;
        if (!this.Hashitems.TryGetValue(name, out htmlAttribute))
          this.Append(value);
        else
          this[this.items.IndexOf(htmlAttribute)] = value;
      }
    }

    /// <summary>
    /// Adds a new attribute to the collection with the given values
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void Add(string name, string value) => this.Append(name, value);

    /// <summary>Adds supplied item to collection</summary>
    /// <param name="item"></param>
    public void Add(HtmlAttribute item) => this.Append(item);

    /// <summary>Adds a range supplied items to collection.</summary>
    /// <param name="items">An IEnumerable&lt;HtmlAttribute&gt; of items to append to this.</param>
    public void AddRange(IEnumerable<HtmlAttribute> items)
    {
      foreach (HtmlAttribute newAttribute in items)
        this.Append(newAttribute);
    }

    /// <summary>Adds a range supplied items to collection using a dictionary.</summary>
    /// <param name="items">A Dictionary&lt;string,string&gt; of items to append to this.</param>
    public void AddRange(Dictionary<string, string> items)
    {
      foreach (KeyValuePair<string, string> keyValuePair in items)
        this.Add(keyValuePair.Key, keyValuePair.Value);
    }

    /// <summary>Explicit clear</summary>
    void ICollection<HtmlAttribute>.Clear() => this.items.Clear();

    /// <summary>Retreives existence of supplied item</summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(HtmlAttribute item) => this.items.Contains(item);

    /// <summary>Copies collection to array</summary>
    /// <param name="array"></param>
    /// <param name="arrayIndex"></param>
    public void CopyTo(HtmlAttribute[] array, int arrayIndex) => this.items.CopyTo(array, arrayIndex);

    /// <summary>Get Explicit enumerator</summary>
    /// <returns></returns>
    IEnumerator<HtmlAttribute> IEnumerable<HtmlAttribute>.GetEnumerator() => (IEnumerator<HtmlAttribute>) this.items.GetEnumerator();

    /// <summary>Explicit non-generic enumerator</summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.items.GetEnumerator();

    /// <summary>
    /// Retrieves the index for the supplied item, -1 if not found
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int IndexOf(HtmlAttribute item) => this.items.IndexOf(item);

    /// <summary>Inserts given item into collection at supplied index</summary>
    /// <param name="index"></param>
    /// <param name="item"></param>
    public void Insert(int index, HtmlAttribute item)
    {
      this.Hashitems[item.Name] = item != null ? item : throw new ArgumentNullException(nameof (item));
      item._ownernode = this._ownernode;
      this.items.Insert(index, item);
      this._ownernode.SetChanged();
    }

    /// <summary>Explicit collection remove</summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool ICollection<HtmlAttribute>.Remove(HtmlAttribute item) => this.items.Remove(item);

    /// <summary>Removes the attribute at the specified index.</summary>
    /// <param name="index">The index of the attribute to remove.</param>
    public void RemoveAt(int index)
    {
      this.Hashitems.Remove(this.items[index].Name);
      this.items.RemoveAt(index);
      this._ownernode.SetChanged();
    }

    /// <summary>
    /// Inserts the specified attribute as the last attribute in the collection.
    /// </summary>
    /// <param name="newAttribute">The attribute to insert. May not be null.</param>
    /// <returns>The appended attribute.</returns>
    public HtmlAttribute Append(HtmlAttribute newAttribute)
    {
      if (this._ownernode.NodeType == HtmlNodeType.Text || this._ownernode.NodeType == HtmlNodeType.Comment)
        throw new Exception("A Text or Comment node cannot have attributes.");
      this.Hashitems[newAttribute.Name] = newAttribute != null ? newAttribute : throw new ArgumentNullException(nameof (newAttribute));
      newAttribute._ownernode = this._ownernode;
      this.items.Add(newAttribute);
      this._ownernode.SetChanged();
      return newAttribute;
    }

    /// <summary>
    /// Creates and inserts a new attribute as the last attribute in the collection.
    /// </summary>
    /// <param name="name">The name of the attribute to insert.</param>
    /// <returns>The appended attribute.</returns>
    public HtmlAttribute Append(string name) => this.Append(this._ownernode._ownerdocument.CreateAttribute(name));

    /// <summary>
    /// Creates and inserts a new attribute as the last attribute in the collection.
    /// </summary>
    /// <param name="name">The name of the attribute to insert.</param>
    /// <param name="value">The value of the attribute to insert.</param>
    /// <returns>The appended attribute.</returns>
    public HtmlAttribute Append(string name, string value) => this.Append(this._ownernode._ownerdocument.CreateAttribute(name, value));

    /// <summary>Checks for existance of attribute with given name</summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool Contains(string name)
    {
      for (int index = 0; index < this.items.Count; ++index)
      {
        if (string.Equals(this.items[index].Name, name, StringComparison.OrdinalIgnoreCase))
          return true;
      }
      return false;
    }

    /// <summary>
    /// Inserts the specified attribute as the first node in the collection.
    /// </summary>
    /// <param name="newAttribute">The attribute to insert. May not be null.</param>
    /// <returns>The prepended attribute.</returns>
    public HtmlAttribute Prepend(HtmlAttribute newAttribute)
    {
      this.Insert(0, newAttribute);
      return newAttribute;
    }

    /// <summary>Removes a given attribute from the list.</summary>
    /// <param name="attribute">The attribute to remove. May not be null.</param>
    public void Remove(HtmlAttribute attribute)
    {
      int index = attribute != null ? this.GetAttributeIndex(attribute) : throw new ArgumentNullException(nameof (attribute));
      if (index == -1)
        throw new IndexOutOfRangeException();
      this.RemoveAt(index);
    }

    /// <summary>
    /// Removes an attribute from the list, using its name. If there are more than one attributes with this name, they will all be removed.
    /// </summary>
    /// <param name="name">The attribute's name. May not be null.</param>
    public void Remove(string name)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      for (int index = 0; index < this.items.Count; ++index)
      {
        if (string.Equals(this.items[index].Name, name, StringComparison.OrdinalIgnoreCase))
          this.RemoveAt(index);
      }
    }

    /// <summary>Remove all attributes in the list.</summary>
    public void RemoveAll()
    {
      this.Hashitems.Clear();
      this.items.Clear();
      this._ownernode.SetChanged();
    }

    /// <summary>
    /// Returns all attributes with specified name. Handles case insentivity
    /// </summary>
    /// <param name="attributeName">Name of the attribute</param>
    /// <returns></returns>
    public IEnumerable<HtmlAttribute> AttributesWithName(
      string attributeName)
    {
      for (int i = 0; i < this.items.Count; ++i)
      {
        if (string.Equals(this.items[i].Name, attributeName, StringComparison.OrdinalIgnoreCase))
          yield return this.items[i];
      }
    }

    /// <summary>Removes all attributes from the collection</summary>
    public void Remove() => this.items.Clear();

    /// <summary>Clears the attribute collection</summary>
    internal void Clear()
    {
      this.Hashitems.Clear();
      this.items.Clear();
    }

    internal int GetAttributeIndex(HtmlAttribute attribute)
    {
      if (attribute == null)
        throw new ArgumentNullException(nameof (attribute));
      for (int index = 0; index < this.items.Count; ++index)
      {
        if (this.items[index] == attribute)
          return index;
      }
      return -1;
    }

    internal int GetAttributeIndex(string name)
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      for (int index = 0; index < this.items.Count; ++index)
      {
        if (string.Equals(this.items[index].Name, name, StringComparison.OrdinalIgnoreCase))
          return index;
      }
      return -1;
    }
  }
}

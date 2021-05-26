// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.SchemaElementLookUpTable`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal sealed class SchemaElementLookUpTable<T> : 
    IEnumerable<T>,
    IEnumerable,
    ISchemaElementLookUpTable<T>
    where T : SchemaElement
  {
    private Dictionary<string, T> _keyToType;
    private readonly List<string> _keysInDefOrder = new List<string>();

    public int Count => this.KeyToType.Count;

    public bool ContainsKey(string key) => this.KeyToType.ContainsKey(SchemaElementLookUpTable<T>.KeyFromName(key));

    public T LookUpEquivalentKey(string key)
    {
      key = SchemaElementLookUpTable<T>.KeyFromName(key);
      T obj;
      return this.KeyToType.TryGetValue(key, out obj) ? obj : default (T);
    }

    public T this[string key] => this.KeyToType[SchemaElementLookUpTable<T>.KeyFromName(key)];

    public T GetElementAt(int index) => this.KeyToType[this._keysInDefOrder[index]];

    public IEnumerator<T> GetEnumerator() => (IEnumerator<T>) new SchemaElementLookUpTableEnumerator<T, T>(this.KeyToType, this._keysInDefOrder);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new SchemaElementLookUpTableEnumerator<T, T>(this.KeyToType, this._keysInDefOrder);

    public IEnumerator<S> GetFilteredEnumerator<S>() where S : T => (IEnumerator<S>) new SchemaElementLookUpTableEnumerator<S, T>(this.KeyToType, this._keysInDefOrder);

    public AddErrorKind TryAdd(T type)
    {
      if (string.IsNullOrEmpty(type.Identity))
        return AddErrorKind.MissingNameError;
      string key = SchemaElementLookUpTable<T>.KeyFromElement(type);
      if (this.KeyToType.TryGetValue(key, out T _))
        return AddErrorKind.DuplicateNameError;
      this.KeyToType.Add(key, type);
      this._keysInDefOrder.Add(key);
      return AddErrorKind.Succeeded;
    }

    public void Add(
      T type,
      bool doNotAddErrorForEmptyName,
      Func<object, string> duplicateKeyErrorFormat)
    {
      switch (this.TryAdd(type))
      {
        case AddErrorKind.MissingNameError:
          if (doNotAddErrorForEmptyName)
            break;
          type.AddError(ErrorCode.InvalidName, EdmSchemaErrorSeverity.Error, (object) Strings.MissingName);
          break;
        case AddErrorKind.DuplicateNameError:
          type.AddError(ErrorCode.AlreadyDefined, EdmSchemaErrorSeverity.Error, (object) duplicateKeyErrorFormat((object) type.FQName));
          break;
      }
    }

    private static string KeyFromElement(T type) => SchemaElementLookUpTable<T>.KeyFromName(type.Identity);

    private static string KeyFromName(string unnormalizedKey) => unnormalizedKey;

    private Dictionary<string, T> KeyToType
    {
      get
      {
        if (this._keyToType == null)
          this._keyToType = new Dictionary<string, T>((IEqualityComparer<string>) StringComparer.Ordinal);
        return this._keyToType;
      }
    }
  }
}

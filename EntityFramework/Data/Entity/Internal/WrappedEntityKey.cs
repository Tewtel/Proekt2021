// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.WrappedEntityKey
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Internal
{
  internal class WrappedEntityKey
  {
    private readonly IEnumerable<KeyValuePair<string, object>> _keyValuePairs;
    private readonly EntityKey _key;

    public WrappedEntityKey(
      EntitySet entitySet,
      string entitySetName,
      object[] keyValues,
      string keyValuesParamName)
    {
      if (keyValues == null)
        keyValues = new object[1];
      List<string> list = entitySet.ElementType.KeyMembers.Select<EdmMember, string>((Func<EdmMember, string>) (m => m.Name)).ToList<string>();
      if (list.Count != keyValues.Length)
        throw new ArgumentException(System.Data.Entity.Resources.Strings.DbSet_WrongNumberOfKeyValuesPassed, keyValuesParamName);
      this._keyValuePairs = list.Zip<string, object, KeyValuePair<string, object>>((IEnumerable<object>) keyValues, (Func<string, object, KeyValuePair<string, object>>) ((name, value) => new KeyValuePair<string, object>(name, value)));
      if (!((IEnumerable<object>) keyValues).All<object>((Func<object, bool>) (v => v != null)))
        return;
      this._key = new EntityKey(entitySetName, this.KeyValuePairs);
    }

    public bool HasNullValues => this._key == (EntityKey) null;

    public EntityKey EntityKey => this._key;

    public IEnumerable<KeyValuePair<string, object>> KeyValuePairs => this._keyValuePairs;
  }
}

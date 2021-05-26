// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.EntityKeyMember
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Data.Entity.Core
{
  /// <summary>
  /// Information about a key that is part of an EntityKey.
  /// A key member contains the key name and value.
  /// </summary>
  [DataContract]
  [Serializable]
  public class EntityKeyMember
  {
    private string _keyName;
    private object _keyValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.EntityKeyMember" /> class.
    /// </summary>
    public EntityKeyMember()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.EntityKeyMember" /> class with the specified entity key pair.
    /// </summary>
    /// <param name="keyName">The name of the key.</param>
    /// <param name="keyValue">The key value.</param>
    public EntityKeyMember(string keyName, object keyValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(keyName, nameof (keyName));
      System.Data.Entity.Utilities.Check.NotNull<object>(keyValue, nameof (keyValue));
      this._keyName = keyName;
      this._keyValue = keyValue;
    }

    /// <summary>Gets or sets the name of the entity key.</summary>
    /// <returns>The key name.</returns>
    [DataMember]
    public string Key
    {
      get => this._keyName;
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<string>(value, nameof (value));
        EntityKeyMember.ValidateWritable((object) this._keyName);
        this._keyName = value;
      }
    }

    /// <summary>Gets or sets the value of the entity key.</summary>
    /// <returns>The key value.</returns>
    [DataMember]
    public object Value
    {
      get => this._keyValue;
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<object>(value, nameof (value));
        EntityKeyMember.ValidateWritable(this._keyValue);
        this._keyValue = value;
      }
    }

    /// <summary>Returns a string representation of the entity key.</summary>
    /// <returns>A string representation of the entity key.</returns>
    public override string ToString() => string.Format((IFormatProvider) CultureInfo.CurrentCulture, "[{0}, {1}]", (object) this._keyName, this._keyValue);

    private static void ValidateWritable(object instance)
    {
      if (instance != null)
        throw new InvalidOperationException(Strings.EntityKey_CannotChangeKey);
    }
  }
}

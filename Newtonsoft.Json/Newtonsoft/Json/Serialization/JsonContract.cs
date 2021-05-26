// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonContract
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Contract details for a <see cref="T:System.Type" /> used by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  public abstract class JsonContract
  {
    internal bool IsNullable;
    internal bool IsConvertable;
    internal bool IsEnum;
    internal Type NonNullableUnderlyingType;
    internal ReadType InternalReadType;
    internal JsonContractType ContractType;
    internal bool IsReadOnlyOrFixedSize;
    internal bool IsSealed;
    internal bool IsInstantiable;
    private List<SerializationCallback>? _onDeserializedCallbacks;
    private List<SerializationCallback>? _onDeserializingCallbacks;
    private List<SerializationCallback>? _onSerializedCallbacks;
    private List<SerializationCallback>? _onSerializingCallbacks;
    private List<SerializationErrorCallback>? _onErrorCallbacks;
    private Type _createdType;

    /// <summary>Gets the underlying type for the contract.</summary>
    /// <value>The underlying type for the contract.</value>
    public Type UnderlyingType { get; }

    /// <summary>Gets or sets the type created during deserialization.</summary>
    /// <value>The type created during deserialization.</value>
    public Type CreatedType
    {
      get => this._createdType;
      set
      {
        ValidationUtils.ArgumentNotNull((object) value, nameof (value));
        this._createdType = value;
        this.IsSealed = this._createdType.IsSealed();
        this.IsInstantiable = !this._createdType.IsInterface() && !this._createdType.IsAbstract();
      }
    }

    /// <summary>
    /// Gets or sets whether this type contract is serialized as a reference.
    /// </summary>
    /// <value>Whether this type contract is serialized as a reference.</value>
    public bool? IsReference { get; set; }

    /// <summary>
    /// Gets or sets the default <see cref="T:Newtonsoft.Json.JsonConverter" /> for this contract.
    /// </summary>
    /// <value>The converter.</value>
    public JsonConverter? Converter { get; set; }

    /// <summary>
    /// Gets the internally resolved <see cref="T:Newtonsoft.Json.JsonConverter" /> for the contract's type.
    /// This converter is used as a fallback converter when no other converter is resolved.
    /// Setting <see cref="P:Newtonsoft.Json.Serialization.JsonContract.Converter" /> will always override this converter.
    /// </summary>
    public JsonConverter? InternalConverter { get; internal set; }

    /// <summary>
    /// Gets or sets all methods called immediately after deserialization of the object.
    /// </summary>
    /// <value>The methods called immediately after deserialization of the object.</value>
    public IList<SerializationCallback> OnDeserializedCallbacks
    {
      get
      {
        if (this._onDeserializedCallbacks == null)
          this._onDeserializedCallbacks = new List<SerializationCallback>();
        return (IList<SerializationCallback>) this._onDeserializedCallbacks;
      }
    }

    /// <summary>
    /// Gets or sets all methods called during deserialization of the object.
    /// </summary>
    /// <value>The methods called during deserialization of the object.</value>
    public IList<SerializationCallback> OnDeserializingCallbacks
    {
      get
      {
        if (this._onDeserializingCallbacks == null)
          this._onDeserializingCallbacks = new List<SerializationCallback>();
        return (IList<SerializationCallback>) this._onDeserializingCallbacks;
      }
    }

    /// <summary>
    /// Gets or sets all methods called after serialization of the object graph.
    /// </summary>
    /// <value>The methods called after serialization of the object graph.</value>
    public IList<SerializationCallback> OnSerializedCallbacks
    {
      get
      {
        if (this._onSerializedCallbacks == null)
          this._onSerializedCallbacks = new List<SerializationCallback>();
        return (IList<SerializationCallback>) this._onSerializedCallbacks;
      }
    }

    /// <summary>
    /// Gets or sets all methods called before serialization of the object.
    /// </summary>
    /// <value>The methods called before serialization of the object.</value>
    public IList<SerializationCallback> OnSerializingCallbacks
    {
      get
      {
        if (this._onSerializingCallbacks == null)
          this._onSerializingCallbacks = new List<SerializationCallback>();
        return (IList<SerializationCallback>) this._onSerializingCallbacks;
      }
    }

    /// <summary>
    /// Gets or sets all method called when an error is thrown during the serialization of the object.
    /// </summary>
    /// <value>The methods called when an error is thrown during the serialization of the object.</value>
    public IList<SerializationErrorCallback> OnErrorCallbacks
    {
      get
      {
        if (this._onErrorCallbacks == null)
          this._onErrorCallbacks = new List<SerializationErrorCallback>();
        return (IList<SerializationErrorCallback>) this._onErrorCallbacks;
      }
    }

    /// <summary>
    /// Gets or sets the default creator method used to create the object.
    /// </summary>
    /// <value>The default creator method used to create the object.</value>
    public Func<object>? DefaultCreator { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the default creator is non-public.
    /// </summary>
    /// <value><c>true</c> if the default object creator is non-public; otherwise, <c>false</c>.</value>
    public bool DefaultCreatorNonPublic { get; set; }

    internal JsonContract(Type underlyingType)
    {
      ValidationUtils.ArgumentNotNull((object) underlyingType, nameof (underlyingType));
      this.UnderlyingType = underlyingType;
      underlyingType = ReflectionUtils.EnsureNotByRefType(underlyingType);
      this.IsNullable = ReflectionUtils.IsNullable(underlyingType);
      this.NonNullableUnderlyingType = !this.IsNullable || !ReflectionUtils.IsNullableType(underlyingType) ? underlyingType : Nullable.GetUnderlyingType(underlyingType);
      this._createdType = this.CreatedType = this.NonNullableUnderlyingType;
      this.IsConvertable = ConvertUtils.IsConvertible(this.NonNullableUnderlyingType);
      this.IsEnum = this.NonNullableUnderlyingType.IsEnum();
      this.InternalReadType = ReadType.Read;
    }

    internal void InvokeOnSerializing(object o, StreamingContext context)
    {
      if (this._onSerializingCallbacks == null)
        return;
      foreach (SerializationCallback serializingCallback in this._onSerializingCallbacks)
        serializingCallback(o, context);
    }

    internal void InvokeOnSerialized(object o, StreamingContext context)
    {
      if (this._onSerializedCallbacks == null)
        return;
      foreach (SerializationCallback serializedCallback in this._onSerializedCallbacks)
        serializedCallback(o, context);
    }

    internal void InvokeOnDeserializing(object o, StreamingContext context)
    {
      if (this._onDeserializingCallbacks == null)
        return;
      foreach (SerializationCallback deserializingCallback in this._onDeserializingCallbacks)
        deserializingCallback(o, context);
    }

    internal void InvokeOnDeserialized(object o, StreamingContext context)
    {
      if (this._onDeserializedCallbacks == null)
        return;
      foreach (SerializationCallback deserializedCallback in this._onDeserializedCallbacks)
        deserializedCallback(o, context);
    }

    internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
    {
      if (this._onErrorCallbacks == null)
        return;
      foreach (SerializationErrorCallback onErrorCallback in this._onErrorCallbacks)
        onErrorCallback(o, context, errorContext);
    }

    internal static SerializationCallback CreateSerializationCallback(
      MethodInfo callbackMethodInfo)
    {
      return (SerializationCallback) ((o, context) => callbackMethodInfo.Invoke(o, new object[1]
      {
        (object) context
      }));
    }

    internal static SerializationErrorCallback CreateSerializationErrorCallback(
      MethodInfo callbackMethodInfo)
    {
      return (SerializationErrorCallback) ((o, context, econtext) => callbackMethodInfo.Invoke(o, new object[2]
      {
        (object) context,
        (object) econtext
      }));
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonDictionaryContract
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Contract details for a <see cref="T:System.Type" /> used by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  public class JsonDictionaryContract : JsonContainerContract
  {
    private readonly Type? _genericCollectionDefinitionType;
    private Type? _genericWrapperType;
    private ObjectConstructor<object>? _genericWrapperCreator;
    private Func<object>? _genericTemporaryDictionaryCreator;
    private readonly ConstructorInfo? _parameterizedConstructor;
    private ObjectConstructor<object>? _overrideCreator;
    private ObjectConstructor<object>? _parameterizedCreator;

    /// <summary>Gets or sets the dictionary key resolver.</summary>
    /// <value>The dictionary key resolver.</value>
    public Func<string, string>? DictionaryKeyResolver { get; set; }

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the dictionary keys.
    /// </summary>
    /// <value>The <see cref="T:System.Type" /> of the dictionary keys.</value>
    public Type? DictionaryKeyType { get; }

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the dictionary values.
    /// </summary>
    /// <value>The <see cref="T:System.Type" /> of the dictionary values.</value>
    public Type? DictionaryValueType { get; }

    internal JsonContract? KeyContract { get; set; }

    internal bool ShouldCreateWrapper { get; }

    internal ObjectConstructor<object>? ParameterizedCreator
    {
      get
      {
        if (this._parameterizedCreator == null && this._parameterizedConstructor != (ConstructorInfo) null)
          this._parameterizedCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) this._parameterizedConstructor);
        return this._parameterizedCreator;
      }
    }

    /// <summary>
    /// Gets or sets the function used to create the object. When set this function will override <see cref="P:Newtonsoft.Json.Serialization.JsonContract.DefaultCreator" />.
    /// </summary>
    /// <value>The function used to create the object.</value>
    public ObjectConstructor<object>? OverrideCreator
    {
      get => this._overrideCreator;
      set => this._overrideCreator = value;
    }

    /// <summary>
    /// Gets a value indicating whether the creator has a parameter with the dictionary values.
    /// </summary>
    /// <value><c>true</c> if the creator has a parameter with the dictionary values; otherwise, <c>false</c>.</value>
    public bool HasParameterizedCreator { get; set; }

    internal bool HasParameterizedCreatorInternal => this.HasParameterizedCreator || this._parameterizedCreator != null || this._parameterizedConstructor != (ConstructorInfo) null;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.JsonDictionaryContract" /> class.
    /// </summary>
    /// <param name="underlyingType">The underlying type for the contract.</param>
    public JsonDictionaryContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Dictionary;
      Type keyType;
      Type valueType;
      if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (IDictionary<,>), out this._genericCollectionDefinitionType))
      {
        keyType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
        valueType = this._genericCollectionDefinitionType.GetGenericArguments()[1];
        if (ReflectionUtils.IsGenericDefinition(this.UnderlyingType, typeof (IDictionary<,>)))
          this.CreatedType = typeof (Dictionary<,>).MakeGenericType(keyType, valueType);
        else if (underlyingType.IsGenericType() && underlyingType.GetGenericTypeDefinition().FullName == "System.Collections.Concurrent.ConcurrentDictionary`2")
          this.ShouldCreateWrapper = true;
        this.IsReadOnlyOrFixedSize = ReflectionUtils.InheritsGenericDefinition(underlyingType, typeof (ReadOnlyDictionary<,>));
      }
      else if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof (IReadOnlyDictionary<,>), out this._genericCollectionDefinitionType))
      {
        keyType = this._genericCollectionDefinitionType.GetGenericArguments()[0];
        valueType = this._genericCollectionDefinitionType.GetGenericArguments()[1];
        if (ReflectionUtils.IsGenericDefinition(this.UnderlyingType, typeof (IReadOnlyDictionary<,>)))
          this.CreatedType = typeof (ReadOnlyDictionary<,>).MakeGenericType(keyType, valueType);
        this.IsReadOnlyOrFixedSize = true;
      }
      else
      {
        ReflectionUtils.GetDictionaryKeyValueTypes(this.UnderlyingType, out keyType, out valueType);
        if (this.UnderlyingType == typeof (IDictionary))
          this.CreatedType = typeof (Dictionary<object, object>);
      }
      if (keyType != (Type) null && valueType != (Type) null)
      {
        this._parameterizedConstructor = CollectionUtils.ResolveEnumerableCollectionConstructor(this.CreatedType, typeof (KeyValuePair<,>).MakeGenericType(keyType, valueType), typeof (IDictionary<,>).MakeGenericType(keyType, valueType));
        if (!this.HasParameterizedCreatorInternal && underlyingType.Name == "FSharpMap`2")
        {
          FSharpUtils.EnsureInitialized(underlyingType.Assembly());
          this._parameterizedCreator = FSharpUtils.Instance.CreateMap(keyType, valueType);
        }
      }
      if (!typeof (IDictionary).IsAssignableFrom(this.CreatedType))
        this.ShouldCreateWrapper = true;
      this.DictionaryKeyType = keyType;
      this.DictionaryValueType = valueType;
      Type createdType;
      ObjectConstructor<object> parameterizedCreator;
      if (!(this.DictionaryKeyType != (Type) null) || !(this.DictionaryValueType != (Type) null) || !ImmutableCollectionsUtils.TryBuildImmutableForDictionaryContract(underlyingType, this.DictionaryKeyType, this.DictionaryValueType, out createdType, out parameterizedCreator))
        return;
      this.CreatedType = createdType;
      this._parameterizedCreator = parameterizedCreator;
      this.IsReadOnlyOrFixedSize = true;
    }

    internal IWrappedDictionary CreateWrapper(object dictionary)
    {
      if (this._genericWrapperCreator == null)
      {
        this._genericWrapperType = typeof (DictionaryWrapper<,>).MakeGenericType(this.DictionaryKeyType, this.DictionaryValueType);
        this._genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateParameterizedConstructor((MethodBase) this._genericWrapperType.GetConstructor(new Type[1]
        {
          this._genericCollectionDefinitionType
        }));
      }
      return (IWrappedDictionary) this._genericWrapperCreator(new object[1]
      {
        dictionary
      });
    }

    internal IDictionary CreateTemporaryDictionary()
    {
      if (this._genericTemporaryDictionaryCreator == null)
      {
        Type type1 = typeof (Dictionary<,>);
        Type[] typeArray = new Type[2];
        Type type2 = this.DictionaryKeyType;
        if ((object) type2 == null)
          type2 = typeof (object);
        typeArray[0] = type2;
        Type type3 = this.DictionaryValueType;
        if ((object) type3 == null)
          type3 = typeof (object);
        typeArray[1] = type3;
        this._genericTemporaryDictionaryCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateDefaultConstructor<object>(type1.MakeGenericType(typeArray));
      }
      return (IDictionary) this._genericTemporaryDictionaryCreator();
    }
  }
}

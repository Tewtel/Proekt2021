// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.JsonObjectContract
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Contract details for a <see cref="T:System.Type" /> used by the <see cref="T:Newtonsoft.Json.JsonSerializer" />.
  /// </summary>
  public class JsonObjectContract : JsonContainerContract
  {
    internal bool ExtensionDataIsJToken;
    private bool? _hasRequiredOrDefaultValueProperties;
    private ObjectConstructor<object>? _overrideCreator;
    private ObjectConstructor<object>? _parameterizedCreator;
    private JsonPropertyCollection? _creatorParameters;
    private Type? _extensionDataValueType;

    /// <summary>Gets or sets the object member serialization.</summary>
    /// <value>The member object serialization.</value>
    public MemberSerialization MemberSerialization { get; set; }

    /// <summary>
    /// Gets or sets the missing member handling used when deserializing this object.
    /// </summary>
    /// <value>The missing member handling.</value>
    public Newtonsoft.Json.MissingMemberHandling? MissingMemberHandling { get; set; }

    /// <summary>
    /// Gets or sets a value that indicates whether the object's properties are required.
    /// </summary>
    /// <value>
    /// 	A value indicating whether the object's properties are required.
    /// </value>
    public Required? ItemRequired { get; set; }

    /// <summary>
    /// Gets or sets how the object's properties with null values are handled during serialization and deserialization.
    /// </summary>
    /// <value>How the object's properties with null values are handled during serialization and deserialization.</value>
    public NullValueHandling? ItemNullValueHandling { get; set; }

    /// <summary>Gets the object's properties.</summary>
    /// <value>The object's properties.</value>
    public JsonPropertyCollection Properties { get; }

    /// <summary>
    /// Gets a collection of <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> instances that define the parameters used with <see cref="P:Newtonsoft.Json.Serialization.JsonObjectContract.OverrideCreator" />.
    /// </summary>
    public JsonPropertyCollection CreatorParameters
    {
      get
      {
        if (this._creatorParameters == null)
          this._creatorParameters = new JsonPropertyCollection(this.UnderlyingType);
        return this._creatorParameters;
      }
    }

    /// <summary>
    /// Gets or sets the function used to create the object. When set this function will override <see cref="P:Newtonsoft.Json.Serialization.JsonContract.DefaultCreator" />.
    /// This function is called with a collection of arguments which are defined by the <see cref="P:Newtonsoft.Json.Serialization.JsonObjectContract.CreatorParameters" /> collection.
    /// </summary>
    /// <value>The function used to create the object.</value>
    public ObjectConstructor<object>? OverrideCreator
    {
      get => this._overrideCreator;
      set => this._overrideCreator = value;
    }

    internal ObjectConstructor<object>? ParameterizedCreator
    {
      get => this._parameterizedCreator;
      set => this._parameterizedCreator = value;
    }

    /// <summary>Gets or sets the extension data setter.</summary>
    public ExtensionDataSetter? ExtensionDataSetter { get; set; }

    /// <summary>Gets or sets the extension data getter.</summary>
    public ExtensionDataGetter? ExtensionDataGetter { get; set; }

    /// <summary>Gets or sets the extension data value type.</summary>
    public Type? ExtensionDataValueType
    {
      get => this._extensionDataValueType;
      set
      {
        this._extensionDataValueType = value;
        this.ExtensionDataIsJToken = value != (Type) null && typeof (JToken).IsAssignableFrom(value);
      }
    }

    /// <summary>Gets or sets the extension data name resolver.</summary>
    /// <value>The extension data name resolver.</value>
    public Func<string, string>? ExtensionDataNameResolver { get; set; }

    internal bool HasRequiredOrDefaultValueProperties
    {
      get
      {
        if (!this._hasRequiredOrDefaultValueProperties.HasValue)
        {
          this._hasRequiredOrDefaultValueProperties = new bool?(false);
          if (this.ItemRequired.GetValueOrDefault(Required.Default) != Required.Default)
          {
            this._hasRequiredOrDefaultValueProperties = new bool?(true);
          }
          else
          {
            foreach (JsonProperty property in (Collection<JsonProperty>) this.Properties)
            {
              if (property.Required == Required.Default)
              {
                DefaultValueHandling? defaultValueHandling1 = property.DefaultValueHandling;
                DefaultValueHandling? nullable = defaultValueHandling1.HasValue ? new DefaultValueHandling?(defaultValueHandling1.GetValueOrDefault() & DefaultValueHandling.Populate) : new DefaultValueHandling?();
                DefaultValueHandling defaultValueHandling2 = DefaultValueHandling.Populate;
                if (!(nullable.GetValueOrDefault() == defaultValueHandling2 & nullable.HasValue))
                  continue;
              }
              this._hasRequiredOrDefaultValueProperties = new bool?(true);
              break;
            }
          }
        }
        return this._hasRequiredOrDefaultValueProperties.GetValueOrDefault();
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.JsonObjectContract" /> class.
    /// </summary>
    /// <param name="underlyingType">The underlying type for the contract.</param>
    public JsonObjectContract(Type underlyingType)
      : base(underlyingType)
    {
      this.ContractType = JsonContractType.Object;
      this.Properties = new JsonPropertyCollection(this.UnderlyingType);
    }

    [SecuritySafeCritical]
    internal object GetUninitializedObject()
    {
      if (!JsonTypeReflector.FullyTrusted)
        throw new JsonException("Insufficient permissions. Creating an uninitialized '{0}' type requires full trust.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.NonNullableUnderlyingType));
      return FormatterServices.GetUninitializedObject(this.NonNullableUnderlyingType);
    }
  }
}

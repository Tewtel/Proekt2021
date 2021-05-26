// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.JsonContractResolver
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Web.Http;

namespace System.Net.Http.Formatting
{
  /// <summary>Represents the default <see cref="T:Newtonsoft.Json.Serialization.IContractResolver" /> used by <see cref="T:System.Net.Http.Formatting.JsonMediaTypeFormatter" />. It uses the formatter's <see cref="T:System.Net.Http.Formatting.IRequiredMemberSelector" /> to select required members and recognizes the <see cref="T:System.SerializableAttribute" /> type annotation.</summary>
  public class JsonContractResolver : DefaultContractResolver
  {
    private readonly MediaTypeFormatter _formatter;

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.Http.Formatting.JsonContractResolver" /> class.</summary>
    /// <param name="formatter">The formatter to use for resolving required members.</param>
    public JsonContractResolver(MediaTypeFormatter formatter)
    {
      this._formatter = formatter != null ? formatter : throw Error.ArgumentNull(nameof (formatter));
      this.IgnoreSerializableAttribute = false;
    }

    private void ConfigureProperty(MemberInfo member, JsonProperty property)
    {
      if (this._formatter.RequiredMemberSelector != null && this._formatter.RequiredMemberSelector.IsRequiredMember(member))
      {
        property.Required = Required.AllowNull;
        property.DefaultValueHandling = new DefaultValueHandling?(DefaultValueHandling.Include);
        property.NullValueHandling = new NullValueHandling?(NullValueHandling.Include);
      }
      else
        property.Required = Required.Default;
    }

    /// <summary>Creates a property on the specified class by using the specified parameters.</summary>
    /// <returns>A <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> to create on the specified class by using the specified parameters.</returns>
    /// <param name="member">The member info.</param>
    /// <param name="memberSerialization">The member serialization.</param>
    protected override JsonProperty CreateProperty(
      MemberInfo member,
      MemberSerialization memberSerialization)
    {
      JsonProperty property = base.CreateProperty(member, memberSerialization);
      this.ConfigureProperty(member, property);
      return property;
    }
  }
}

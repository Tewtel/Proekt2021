// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.DataSetConverter
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using System;
using System.Data;


#nullable enable
namespace Newtonsoft.Json.Converters
{
  /// <summary>
  /// Converts a <see cref="T:System.Data.DataSet" /> to and from JSON.
  /// </summary>
  public class DataSetConverter : JsonConverter
  {
    /// <summary>Writes the JSON representation of the object.</summary>
    /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
    /// <param name="value">The value.</param>
    /// <param name="serializer">The calling serializer.</param>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
      }
      else
      {
        DataSet dataSet = (DataSet) value;
        DefaultContractResolver contractResolver = serializer.ContractResolver as DefaultContractResolver;
        DataTableConverter dataTableConverter = new DataTableConverter();
        writer.WriteStartObject();
        foreach (DataTable table in (InternalDataCollectionBase) dataSet.Tables)
        {
          writer.WritePropertyName(contractResolver != null ? contractResolver.GetResolvedPropertyName(table.TableName) : table.TableName);
          dataTableConverter.WriteJson(writer, (object) table, serializer);
        }
        writer.WriteEndObject();
      }
    }

    /// <summary>Reads the JSON representation of the object.</summary>
    /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
    /// <param name="objectType">Type of the object.</param>
    /// <param name="existingValue">The existing value of object being read.</param>
    /// <param name="serializer">The calling serializer.</param>
    /// <returns>The object value.</returns>
    public override object? ReadJson(
      JsonReader reader,
      Type objectType,
      object? existingValue,
      JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
        return (object) null;
      DataSet dataSet = objectType == typeof (DataSet) ? new DataSet() : (DataSet) Activator.CreateInstance(objectType);
      DataTableConverter dataTableConverter = new DataTableConverter();
      reader.ReadAndAssert();
      while (reader.TokenType == JsonToken.PropertyName)
      {
        DataTable table1 = dataSet.Tables[(string) reader.Value];
        int num = table1 != null ? 1 : 0;
        DataTable table2 = (DataTable) dataTableConverter.ReadJson(reader, typeof (DataTable), (object) table1, serializer);
        if (num == 0)
          dataSet.Tables.Add(table2);
        reader.ReadAndAssert();
      }
      return (object) dataSet;
    }

    /// <summary>
    /// Determines whether this instance can convert the specified value type.
    /// </summary>
    /// <param name="valueType">Type of the value.</param>
    /// <returns>
    /// 	<c>true</c> if this instance can convert the specified value type; otherwise, <c>false</c>.
    /// </returns>
    public override bool CanConvert(Type valueType) => typeof (DataSet).IsAssignableFrom(valueType);
  }
}

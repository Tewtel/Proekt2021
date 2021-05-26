// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JValue
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.Linq.Expressions;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;


#nullable enable
namespace Newtonsoft.Json.Linq
{
  /// <summary>
  /// Represents a value in JSON (string, integer, date, etc).
  /// </summary>
  public class JValue : 
    JToken,
    IEquatable<JValue>,
    IFormattable,
    IComparable,
    IComparable<JValue>,
    IConvertible
  {
    private JTokenType _valueType;
    private object? _value;

    /// <summary>
    /// Writes this token to a <see cref="T:Newtonsoft.Json.JsonWriter" /> asynchronously.
    /// </summary>
    /// <param name="writer">A <see cref="T:Newtonsoft.Json.JsonWriter" /> into which this method will write.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <param name="converters">A collection of <see cref="T:Newtonsoft.Json.JsonConverter" /> which will be used when writing the token.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous write operation.</returns>
    public override Task WriteToAsync(
      JsonWriter writer,
      CancellationToken cancellationToken,
      params JsonConverter[] converters)
    {
      if (converters != null && converters.Length != 0 && this._value != null)
      {
        JsonConverter matchingConverter = JsonSerializer.GetMatchingConverter((IList<JsonConverter>) converters, this._value.GetType());
        if (matchingConverter != null && matchingConverter.CanWrite)
        {
          matchingConverter.WriteJson(writer, this._value, JsonSerializer.CreateDefault());
          return AsyncUtils.CompletedTask;
        }
      }
      switch (this._valueType)
      {
        case JTokenType.Comment:
          return writer.WriteCommentAsync(this._value?.ToString(), cancellationToken);
        case JTokenType.Integer:
          if (this._value is int num7)
            return writer.WriteValueAsync(num7, cancellationToken);
          if (this._value is long num8)
            return writer.WriteValueAsync(num8, cancellationToken);
          if (this._value is ulong num9)
            return writer.WriteValueAsync(num9, cancellationToken);
          return this._value is BigInteger bigInteger2 ? writer.WriteValueAsync((object) bigInteger2, cancellationToken) : writer.WriteValueAsync(Convert.ToInt64(this._value, (IFormatProvider) CultureInfo.InvariantCulture), cancellationToken);
        case JTokenType.Float:
          if (this._value is Decimal num10)
            return writer.WriteValueAsync(num10, cancellationToken);
          if (this._value is double num11)
            return writer.WriteValueAsync(num11, cancellationToken);
          return this._value is float num12 ? writer.WriteValueAsync(num12, cancellationToken) : writer.WriteValueAsync(Convert.ToDouble(this._value, (IFormatProvider) CultureInfo.InvariantCulture), cancellationToken);
        case JTokenType.String:
          return writer.WriteValueAsync(this._value?.ToString(), cancellationToken);
        case JTokenType.Boolean:
          return writer.WriteValueAsync(Convert.ToBoolean(this._value, (IFormatProvider) CultureInfo.InvariantCulture), cancellationToken);
        case JTokenType.Null:
          return writer.WriteNullAsync(cancellationToken);
        case JTokenType.Undefined:
          return writer.WriteUndefinedAsync(cancellationToken);
        case JTokenType.Date:
          return this._value is DateTimeOffset dateTimeOffset2 ? writer.WriteValueAsync(dateTimeOffset2, cancellationToken) : writer.WriteValueAsync(Convert.ToDateTime(this._value, (IFormatProvider) CultureInfo.InvariantCulture), cancellationToken);
        case JTokenType.Raw:
          return writer.WriteRawValueAsync(this._value?.ToString(), cancellationToken);
        case JTokenType.Bytes:
          return writer.WriteValueAsync((byte[]) this._value, cancellationToken);
        case JTokenType.Guid:
          return writer.WriteValueAsync(this._value != null ? (Guid?) this._value : new Guid?(), cancellationToken);
        case JTokenType.Uri:
          return writer.WriteValueAsync((Uri) this._value, cancellationToken);
        case JTokenType.TimeSpan:
          return writer.WriteValueAsync(this._value != null ? (TimeSpan?) this._value : new TimeSpan?(), cancellationToken);
        default:
          throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", (object) this._valueType, "Unexpected token type.");
      }
    }

    internal JValue(object? value, JTokenType type)
    {
      this._value = value;
      this._valueType = type;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class from another <see cref="T:Newtonsoft.Json.Linq.JValue" /> object.
    /// </summary>
    /// <param name="other">A <see cref="T:Newtonsoft.Json.Linq.JValue" /> object to copy from.</param>
    public JValue(JValue other)
      : this(other.Value, other.Type)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public JValue(long value)
      : this((object) value, JTokenType.Integer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public JValue(Decimal value)
      : this((object) value, JTokenType.Float)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public JValue(char value)
      : this((object) value, JTokenType.String)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    [CLSCompliant(false)]
    public JValue(ulong value)
      : this((object) value, JTokenType.Integer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public JValue(double value)
      : this((object) value, JTokenType.Float)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public JValue(float value)
      : this((object) value, JTokenType.Float)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public JValue(DateTime value)
      : this((object) value, JTokenType.Date)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public JValue(DateTimeOffset value)
      : this((object) value, JTokenType.Date)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public JValue(bool value)
      : this((object) value, JTokenType.Boolean)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public JValue(string? value)
      : this((object) value, JTokenType.String)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public JValue(Guid value)
      : this((object) value, JTokenType.Guid)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public JValue(Uri? value)
      : this((object) value, value != (Uri) null ? JTokenType.Uri : JTokenType.Null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public JValue(TimeSpan value)
      : this((object) value, JTokenType.TimeSpan)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Linq.JValue" /> class with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    public JValue(object? value)
      : this(value, JValue.GetValueType(new JTokenType?(), value))
    {
    }

    internal override bool DeepEquals(JToken node)
    {
      if (!(node is JValue v2))
        return false;
      return v2 == this || JValue.ValuesEquals(this, v2);
    }

    /// <summary>
    /// Gets a value indicating whether this token has child tokens.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this token has child values; otherwise, <c>false</c>.
    /// </value>
    public override bool HasValues => false;

    private static int CompareBigInteger(BigInteger i1, object i2)
    {
      int num = i1.CompareTo(ConvertUtils.ToBigInteger(i2));
      if (num != 0)
        return num;
      switch (i2)
      {
        case Decimal d2:
          return 0M.CompareTo(Math.Abs(d2 - Math.Truncate(d2)));
        case double _:
        case float _:
          double d1 = Convert.ToDouble(i2, (IFormatProvider) CultureInfo.InvariantCulture);
          return 0.0.CompareTo(Math.Abs(d1 - Math.Truncate(d1)));
        default:
          return num;
      }
    }

    internal static int Compare(JTokenType valueType, object? objA, object? objB)
    {
      if (objA == objB)
        return 0;
      if (objB == null)
        return 1;
      if (objA == null)
        return -1;
      switch (valueType)
      {
        case JTokenType.Comment:
        case JTokenType.String:
        case JTokenType.Raw:
          return string.CompareOrdinal(Convert.ToString(objA, (IFormatProvider) CultureInfo.InvariantCulture), Convert.ToString(objB, (IFormatProvider) CultureInfo.InvariantCulture));
        case JTokenType.Integer:
          if (objA is BigInteger i1_5)
            return JValue.CompareBigInteger(i1_5, objB);
          if (objB is BigInteger i1_6)
            return -JValue.CompareBigInteger(i1_6, objA);
          if (objA is ulong || objB is ulong || (objA is Decimal || objB is Decimal))
            return Convert.ToDecimal(objA, (IFormatProvider) CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, (IFormatProvider) CultureInfo.InvariantCulture));
          return objA is float || objB is float || (objA is double || objB is double) ? JValue.CompareFloat(objA, objB) : Convert.ToInt64(objA, (IFormatProvider) CultureInfo.InvariantCulture).CompareTo(Convert.ToInt64(objB, (IFormatProvider) CultureInfo.InvariantCulture));
        case JTokenType.Float:
          if (objA is BigInteger i1_7)
            return JValue.CompareBigInteger(i1_7, objB);
          if (objB is BigInteger i1_8)
            return -JValue.CompareBigInteger(i1_8, objA);
          return objA is ulong || objB is ulong || (objA is Decimal || objB is Decimal) ? Convert.ToDecimal(objA, (IFormatProvider) CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, (IFormatProvider) CultureInfo.InvariantCulture)) : JValue.CompareFloat(objA, objB);
        case JTokenType.Boolean:
          return Convert.ToBoolean(objA, (IFormatProvider) CultureInfo.InvariantCulture).CompareTo(Convert.ToBoolean(objB, (IFormatProvider) CultureInfo.InvariantCulture));
        case JTokenType.Date:
          if (objA is DateTime dateTime2)
          {
            DateTime dateTime = !(objB is DateTimeOffset dateTimeOffset4) ? Convert.ToDateTime(objB, (IFormatProvider) CultureInfo.InvariantCulture) : dateTimeOffset4.DateTime;
            return dateTime2.CompareTo(dateTime);
          }
          DateTimeOffset dateTimeOffset3 = (DateTimeOffset) objA;
          if (!(objB is DateTimeOffset other2))
            other2 = new DateTimeOffset(Convert.ToDateTime(objB, (IFormatProvider) CultureInfo.InvariantCulture));
          return dateTimeOffset3.CompareTo(other2);
        case JTokenType.Bytes:
          return objB is byte[] a2_2 ? MiscellaneousUtils.ByteArrayCompare(objA as byte[], a2_2) : throw new ArgumentException("Object must be of type byte[].");
        case JTokenType.Guid:
          return objB is Guid guid2 ? ((Guid) objA).CompareTo(guid2) : throw new ArgumentException("Object must be of type Guid.");
        case JTokenType.Uri:
          Uri uri = objB as Uri;
          if (uri == (Uri) null)
            throw new ArgumentException("Object must be of type Uri.");
          return Comparer<string>.Default.Compare(((Uri) objA).ToString(), uri.ToString());
        case JTokenType.TimeSpan:
          return objB is TimeSpan timeSpan2 ? ((TimeSpan) objA).CompareTo(timeSpan2) : throw new ArgumentException("Object must be of type TimeSpan.");
        default:
          throw MiscellaneousUtils.CreateArgumentOutOfRangeException(nameof (valueType), (object) valueType, "Unexpected value type: {0}".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) valueType));
      }
    }

    private static int CompareFloat(object objA, object objB)
    {
      double d1 = Convert.ToDouble(objA, (IFormatProvider) CultureInfo.InvariantCulture);
      double d2 = Convert.ToDouble(objB, (IFormatProvider) CultureInfo.InvariantCulture);
      return MathUtils.ApproxEquals(d1, d2) ? 0 : d1.CompareTo(d2);
    }

    private static bool Operation(
      ExpressionType operation,
      object? objA,
      object? objB,
      out object? result)
    {
      if ((objA is string || objB is string) && (operation == ExpressionType.Add || operation == ExpressionType.AddAssign))
      {
        result = (object) (objA?.ToString() + objB?.ToString());
        return true;
      }
      if (objA is BigInteger || objB is BigInteger)
      {
        if (objA == null || objB == null)
        {
          result = (object) null;
          return true;
        }
        BigInteger bigInteger1 = ConvertUtils.ToBigInteger(objA);
        BigInteger bigInteger2 = ConvertUtils.ToBigInteger(objB);
        switch (operation)
        {
          case ExpressionType.Add:
          case ExpressionType.AddAssign:
            result = (object) (bigInteger1 + bigInteger2);
            return true;
          case ExpressionType.Divide:
          case ExpressionType.DivideAssign:
            result = (object) (bigInteger1 / bigInteger2);
            return true;
          case ExpressionType.Multiply:
          case ExpressionType.MultiplyAssign:
            result = (object) (bigInteger1 * bigInteger2);
            return true;
          case ExpressionType.Subtract:
          case ExpressionType.SubtractAssign:
            result = (object) (bigInteger1 - bigInteger2);
            return true;
        }
      }
      else if (objA is ulong || objB is ulong || (objA is Decimal || objB is Decimal))
      {
        if (objA == null || objB == null)
        {
          result = (object) null;
          return true;
        }
        Decimal num1 = Convert.ToDecimal(objA, (IFormatProvider) CultureInfo.InvariantCulture);
        Decimal num2 = Convert.ToDecimal(objB, (IFormatProvider) CultureInfo.InvariantCulture);
        switch (operation)
        {
          case ExpressionType.Add:
          case ExpressionType.AddAssign:
            result = (object) (num1 + num2);
            return true;
          case ExpressionType.Divide:
          case ExpressionType.DivideAssign:
            result = (object) (num1 / num2);
            return true;
          case ExpressionType.Multiply:
          case ExpressionType.MultiplyAssign:
            result = (object) (num1 * num2);
            return true;
          case ExpressionType.Subtract:
          case ExpressionType.SubtractAssign:
            result = (object) (num1 - num2);
            return true;
        }
      }
      else if (objA is float || objB is float || (objA is double || objB is double))
      {
        if (objA == null || objB == null)
        {
          result = (object) null;
          return true;
        }
        double num1 = Convert.ToDouble(objA, (IFormatProvider) CultureInfo.InvariantCulture);
        double num2 = Convert.ToDouble(objB, (IFormatProvider) CultureInfo.InvariantCulture);
        switch (operation)
        {
          case ExpressionType.Add:
          case ExpressionType.AddAssign:
            result = (object) (num1 + num2);
            return true;
          case ExpressionType.Divide:
          case ExpressionType.DivideAssign:
            result = (object) (num1 / num2);
            return true;
          case ExpressionType.Multiply:
          case ExpressionType.MultiplyAssign:
            result = (object) (num1 * num2);
            return true;
          case ExpressionType.Subtract:
          case ExpressionType.SubtractAssign:
            result = (object) (num1 - num2);
            return true;
        }
      }
      else
      {
        switch (objA)
        {
          case int _:
          case uint _:
          case long _:
          case short _:
          case ushort _:
          case sbyte _:
          case byte _:
label_28:
            if (objA == null || objB == null)
            {
              result = (object) null;
              return true;
            }
            long int64_1 = Convert.ToInt64(objA, (IFormatProvider) CultureInfo.InvariantCulture);
            long int64_2 = Convert.ToInt64(objB, (IFormatProvider) CultureInfo.InvariantCulture);
            switch (operation)
            {
              case ExpressionType.Add:
              case ExpressionType.AddAssign:
                result = (object) (int64_1 + int64_2);
                return true;
              case ExpressionType.Divide:
              case ExpressionType.DivideAssign:
                result = (object) (int64_1 / int64_2);
                return true;
              case ExpressionType.Multiply:
              case ExpressionType.MultiplyAssign:
                result = (object) (int64_1 * int64_2);
                return true;
              case ExpressionType.Subtract:
              case ExpressionType.SubtractAssign:
                result = (object) (int64_1 - int64_2);
                return true;
            }
            break;
          default:
            switch (objB)
            {
              case int _:
              case uint _:
              case long _:
              case short _:
              case ushort _:
              case sbyte _:
              case byte _:
                goto label_28;
            }
            break;
        }
      }
      result = (object) null;
      return false;
    }

    internal override JToken CloneToken() => (JToken) new JValue(this);

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JValue" /> comment with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JValue" /> comment with the given value.</returns>
    public static JValue CreateComment(string? value) => new JValue((object) value, JTokenType.Comment);

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JValue" /> string with the given value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JValue" /> string with the given value.</returns>
    public static JValue CreateString(string? value) => new JValue((object) value, JTokenType.String);

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JValue" /> null value.
    /// </summary>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JValue" /> null value.</returns>
    public static JValue CreateNull() => new JValue((object) null, JTokenType.Null);

    /// <summary>
    /// Creates a <see cref="T:Newtonsoft.Json.Linq.JValue" /> undefined value.
    /// </summary>
    /// <returns>A <see cref="T:Newtonsoft.Json.Linq.JValue" /> undefined value.</returns>
    public static JValue CreateUndefined() => new JValue((object) null, JTokenType.Undefined);

    private static JTokenType GetValueType(JTokenType? current, object? value)
    {
      if (value == null || value == DBNull.Value)
        return JTokenType.Null;
      switch (value)
      {
        case string _:
          return JValue.GetStringValueType(current);
        case long _:
        case int _:
        case short _:
        case sbyte _:
        case ulong _:
        case uint _:
        case ushort _:
        case byte _:
          return JTokenType.Integer;
        case Enum _:
          return JTokenType.Integer;
        case BigInteger _:
          return JTokenType.Integer;
        case double _:
        case float _:
        case Decimal _:
          return JTokenType.Float;
        case DateTime _:
          return JTokenType.Date;
        case DateTimeOffset _:
          return JTokenType.Date;
        case byte[] _:
          return JTokenType.Bytes;
        case bool _:
          return JTokenType.Boolean;
        case Guid _:
          return JTokenType.Guid;
        default:
          if ((object) (value as Uri) != null)
            return JTokenType.Uri;
          if (value is TimeSpan)
            return JTokenType.TimeSpan;
          throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()));
      }
    }

    private static JTokenType GetStringValueType(JTokenType? current)
    {
      if (!current.HasValue)
        return JTokenType.String;
      switch (current.GetValueOrDefault())
      {
        case JTokenType.Comment:
        case JTokenType.String:
        case JTokenType.Raw:
          return current.GetValueOrDefault();
        default:
          return JTokenType.String;
      }
    }

    /// <summary>
    /// Gets the node type for this <see cref="T:Newtonsoft.Json.Linq.JToken" />.
    /// </summary>
    /// <value>The type.</value>
    public override JTokenType Type => this._valueType;

    /// <summary>Gets or sets the underlying token value.</summary>
    /// <value>The underlying token value.</value>
    public object? Value
    {
      get => this._value;
      set
      {
        if (this._value?.GetType() != value?.GetType())
          this._valueType = JValue.GetValueType(new JTokenType?(this._valueType), value);
        this._value = value;
      }
    }

    /// <summary>
    /// Writes this token to a <see cref="T:Newtonsoft.Json.JsonWriter" />.
    /// </summary>
    /// <param name="writer">A <see cref="T:Newtonsoft.Json.JsonWriter" /> into which this method will write.</param>
    /// <param name="converters">A collection of <see cref="T:Newtonsoft.Json.JsonConverter" />s which will be used when writing the token.</param>
    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      if (converters != null && converters.Length != 0 && this._value != null)
      {
        JsonConverter matchingConverter = JsonSerializer.GetMatchingConverter((IList<JsonConverter>) converters, this._value.GetType());
        if (matchingConverter != null && matchingConverter.CanWrite)
        {
          matchingConverter.WriteJson(writer, this._value, JsonSerializer.CreateDefault());
          return;
        }
      }
      switch (this._valueType)
      {
        case JTokenType.Comment:
          writer.WriteComment(this._value?.ToString());
          break;
        case JTokenType.Integer:
          if (this._value is int num7)
          {
            writer.WriteValue(num7);
            break;
          }
          if (this._value is long num8)
          {
            writer.WriteValue(num8);
            break;
          }
          if (this._value is ulong num9)
          {
            writer.WriteValue(num9);
            break;
          }
          if (this._value is BigInteger bigInteger2)
          {
            writer.WriteValue((object) bigInteger2);
            break;
          }
          writer.WriteValue(Convert.ToInt64(this._value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case JTokenType.Float:
          if (this._value is Decimal num10)
          {
            writer.WriteValue(num10);
            break;
          }
          if (this._value is double num11)
          {
            writer.WriteValue(num11);
            break;
          }
          if (this._value is float num12)
          {
            writer.WriteValue(num12);
            break;
          }
          writer.WriteValue(Convert.ToDouble(this._value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case JTokenType.String:
          writer.WriteValue(this._value?.ToString());
          break;
        case JTokenType.Boolean:
          writer.WriteValue(Convert.ToBoolean(this._value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case JTokenType.Null:
          writer.WriteNull();
          break;
        case JTokenType.Undefined:
          writer.WriteUndefined();
          break;
        case JTokenType.Date:
          if (this._value is DateTimeOffset dateTimeOffset2)
          {
            writer.WriteValue(dateTimeOffset2);
            break;
          }
          writer.WriteValue(Convert.ToDateTime(this._value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case JTokenType.Raw:
          writer.WriteRawValue(this._value?.ToString());
          break;
        case JTokenType.Bytes:
          writer.WriteValue((byte[]) this._value);
          break;
        case JTokenType.Guid:
          writer.WriteValue(this._value != null ? (Guid?) this._value : new Guid?());
          break;
        case JTokenType.Uri:
          writer.WriteValue((Uri) this._value);
          break;
        case JTokenType.TimeSpan:
          writer.WriteValue(this._value != null ? (TimeSpan?) this._value : new TimeSpan?());
          break;
        default:
          throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", (object) this._valueType, "Unexpected token type.");
      }
    }

    internal override int GetDeepHashCode()
    {
      int num = this._value != null ? this._value.GetHashCode() : 0;
      return ((int) this._valueType).GetHashCode() ^ num;
    }

    private static bool ValuesEquals(JValue v1, JValue v2)
    {
      if (v1 == v2)
        return true;
      return v1._valueType == v2._valueType && JValue.Compare(v1._valueType, v1._value, v2._value) == 0;
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
    /// </returns>
    /// <param name="other">An object to compare with this object.</param>
    public bool Equals([AllowNull] JValue other) => other != null && JValue.ValuesEquals(this, other);

    /// <summary>
    /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
    /// </summary>
    /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj) => obj is JValue other && this.Equals(other);

    /// <summary>Serves as a hash function for a particular type.</summary>
    /// <returns>
    /// A hash code for the current <see cref="T:System.Object" />.
    /// </returns>
    public override int GetHashCode() => this._value == null ? 0 : this._value.GetHashCode();

    /// <summary>
    /// Returns a <see cref="T:System.String" /> that represents this instance.
    /// </summary>
    /// <remarks>
    /// <c>ToString()</c> returns a non-JSON string value for tokens with a type of <see cref="F:Newtonsoft.Json.Linq.JTokenType.String" />.
    /// If you want the JSON for all token types then you should use <see cref="M:Newtonsoft.Json.Linq.JValue.WriteTo(Newtonsoft.Json.JsonWriter,Newtonsoft.Json.JsonConverter[])" />.
    /// </remarks>
    /// <returns>
    /// A <see cref="T:System.String" /> that represents this instance.
    /// </returns>
    public override string ToString() => this._value == null ? string.Empty : this._value.ToString();

    /// <summary>
    /// Returns a <see cref="T:System.String" /> that represents this instance.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <returns>
    /// A <see cref="T:System.String" /> that represents this instance.
    /// </returns>
    public string ToString(string format) => this.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture);

    /// <summary>
    /// Returns a <see cref="T:System.String" /> that represents this instance.
    /// </summary>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>
    /// A <see cref="T:System.String" /> that represents this instance.
    /// </returns>
    public string ToString(IFormatProvider formatProvider) => this.ToString((string) null, formatProvider);

    /// <summary>
    /// Returns a <see cref="T:System.String" /> that represents this instance.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="formatProvider">The format provider.</param>
    /// <returns>
    /// A <see cref="T:System.String" /> that represents this instance.
    /// </returns>
    public string ToString(string? format, IFormatProvider formatProvider)
    {
      if (this._value == null)
        return string.Empty;
      return this._value is IFormattable formattable ? formattable.ToString(format, formatProvider) : this._value.ToString();
    }

    /// <summary>
    /// Returns the <see cref="T:System.Dynamic.DynamicMetaObject" /> responsible for binding operations performed on this object.
    /// </summary>
    /// <param name="parameter">The expression tree representation of the runtime value.</param>
    /// <returns>
    /// The <see cref="T:System.Dynamic.DynamicMetaObject" /> to bind this object.
    /// </returns>
    protected override DynamicMetaObject GetMetaObject(Expression parameter) => (DynamicMetaObject) new DynamicProxyMetaObject<JValue>(parameter, this, (DynamicProxy<JValue>) new JValue.JValueDynamicProxy());

    int IComparable.CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      object objB;
      JTokenType valueType;
      if (obj is JValue jvalue)
      {
        objB = jvalue.Value;
        valueType = this._valueType != JTokenType.String || this._valueType == jvalue._valueType ? this._valueType : jvalue._valueType;
      }
      else
      {
        objB = obj;
        valueType = this._valueType;
      }
      return JValue.Compare(valueType, this._value, objB);
    }

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings:
    /// Value
    /// Meaning
    /// Less than zero
    /// This instance is less than <paramref name="obj" />.
    /// Zero
    /// This instance is equal to <paramref name="obj" />.
    /// Greater than zero
    /// This instance is greater than <paramref name="obj" />.
    /// </returns>
    /// <exception cref="T:System.ArgumentException">
    /// 	<paramref name="obj" /> is not of the same type as this instance.
    /// </exception>
    public int CompareTo(JValue obj) => obj == null ? 1 : JValue.Compare(this._valueType != JTokenType.String || this._valueType == obj._valueType ? this._valueType : obj._valueType, this._value, obj._value);

    TypeCode IConvertible.GetTypeCode()
    {
      if (this._value == null)
        return TypeCode.Empty;
      return this._value is IConvertible convertible ? convertible.GetTypeCode() : TypeCode.Object;
    }

    bool IConvertible.ToBoolean(IFormatProvider provider) => (bool) (JToken) this;

    char IConvertible.ToChar(IFormatProvider provider) => (char) (JToken) this;

    sbyte IConvertible.ToSByte(IFormatProvider provider) => (sbyte) (JToken) this;

    byte IConvertible.ToByte(IFormatProvider provider) => (byte) (JToken) this;

    short IConvertible.ToInt16(IFormatProvider provider) => (short) (JToken) this;

    ushort IConvertible.ToUInt16(IFormatProvider provider) => (ushort) (JToken) this;

    int IConvertible.ToInt32(IFormatProvider provider) => (int) (JToken) this;

    uint IConvertible.ToUInt32(IFormatProvider provider) => (uint) (JToken) this;

    long IConvertible.ToInt64(IFormatProvider provider) => (long) (JToken) this;

    ulong IConvertible.ToUInt64(IFormatProvider provider) => (ulong) (JToken) this;

    float IConvertible.ToSingle(IFormatProvider provider) => (float) (JToken) this;

    double IConvertible.ToDouble(IFormatProvider provider) => (double) (JToken) this;

    Decimal IConvertible.ToDecimal(IFormatProvider provider) => (Decimal) (JToken) this;

    DateTime IConvertible.ToDateTime(IFormatProvider provider) => (DateTime) (JToken) this;

    object? IConvertible.ToType(System.Type conversionType, IFormatProvider provider) => this.ToObject(conversionType);

    private class JValueDynamicProxy : DynamicProxy<JValue>
    {
      public override bool TryConvert(JValue instance, ConvertBinder binder, [NotNullWhen(true)] out object? result)
      {
        if (binder.Type == typeof (JValue) || binder.Type == typeof (JToken))
        {
          result = (object) instance;
          return true;
        }
        object initialValue = instance.Value;
        if (initialValue == null)
        {
          result = (object) null;
          return ReflectionUtils.IsNullable(binder.Type);
        }
        result = ConvertUtils.Convert(initialValue, CultureInfo.InvariantCulture, binder.Type);
        return true;
      }

      public override bool TryBinaryOperation(
        JValue instance,
        BinaryOperationBinder binder,
        object arg,
        [NotNullWhen(true)] out object? result)
      {
        object objB = arg is JValue jvalue ? jvalue.Value : arg;
        switch (binder.Operation)
        {
          case ExpressionType.Add:
          case ExpressionType.Divide:
          case ExpressionType.Multiply:
          case ExpressionType.Subtract:
          case ExpressionType.AddAssign:
          case ExpressionType.DivideAssign:
          case ExpressionType.MultiplyAssign:
          case ExpressionType.SubtractAssign:
            if (JValue.Operation(binder.Operation, instance.Value, objB, out result))
            {
              result = (object) new JValue(result);
              return true;
            }
            break;
          case ExpressionType.Equal:
            result = (object) (JValue.Compare(instance.Type, instance.Value, objB) == 0);
            return true;
          case ExpressionType.GreaterThan:
            result = (object) (JValue.Compare(instance.Type, instance.Value, objB) > 0);
            return true;
          case ExpressionType.GreaterThanOrEqual:
            result = (object) (JValue.Compare(instance.Type, instance.Value, objB) >= 0);
            return true;
          case ExpressionType.LessThan:
            result = (object) (JValue.Compare(instance.Type, instance.Value, objB) < 0);
            return true;
          case ExpressionType.LessThanOrEqual:
            result = (object) (JValue.Compare(instance.Type, instance.Value, objB) <= 0);
            return true;
          case ExpressionType.NotEqual:
            result = (object) ((uint) JValue.Compare(instance.Type, instance.Value, objB) > 0U);
            return true;
        }
        result = (object) null;
        return false;
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Linq.StringUtil
// Assembly: System.Data.SQLite.Linq, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: CD270981-C76C-415D-A29C-2F81F5D54EA4
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.Linq.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime;
using System.Text;

namespace System.Data.SQLite.Linq
{
  internal static class StringUtil
  {
    private const string s_defaultDelimiter = ", ";

    internal static string BuildDelimitedList<T>(
      IEnumerable<T> values,
      StringUtil.ToStringConverter<T> converter,
      string delimiter)
    {
      if (values == null)
        return string.Empty;
      if (converter == null)
        converter = new StringUtil.ToStringConverter<T>(StringUtil.InvariantConvertToString<T>);
      if (delimiter == null)
        delimiter = ", ";
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      foreach (T obj in values)
      {
        if (flag)
          flag = false;
        else
          stringBuilder.Append(delimiter);
        stringBuilder.Append(converter(obj));
      }
      return stringBuilder.ToString();
    }

    internal static string FormatIndex(string arrayVarName, int index) => new StringBuilder(arrayVarName.Length + 10 + 2).Append(arrayVarName).Append('[').Append(index).Append(']').ToString();

    internal static string FormatInvariant(string format, params object[] args) => string.Format((IFormatProvider) CultureInfo.InvariantCulture, format, args);

    internal static StringBuilder FormatStringBuilder(
      StringBuilder builder,
      string format,
      params object[] args)
    {
      builder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, format, args);
      return builder;
    }

    internal static StringBuilder IndentNewLine(StringBuilder builder, int indent)
    {
      builder.AppendLine();
      for (int index = 0; index < indent; ++index)
        builder.Append("    ");
      return builder;
    }

    private static string InvariantConvertToString<T>(T value) => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}", (object) value);

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal static bool IsNullOrEmptyOrWhiteSpace(string value) => StringUtil.IsNullOrEmptyOrWhiteSpace(value, 0);

    internal static bool IsNullOrEmptyOrWhiteSpace(string value, int offset)
    {
      if (value != null)
      {
        for (int index = offset; index < value.Length; ++index)
        {
          if (!char.IsWhiteSpace(value[index]))
            return false;
        }
      }
      return true;
    }

    internal static bool IsNullOrEmptyOrWhiteSpace(string value, int offset, int length)
    {
      if (value != null)
      {
        length = Math.Min(value.Length, length);
        for (int index = offset; index < length; ++index)
        {
          if (!char.IsWhiteSpace(value[index]))
            return false;
        }
      }
      return true;
    }

    internal static string MembersToCommaSeparatedString(IEnumerable members)
    {
      StringBuilder builder = new StringBuilder();
      builder.Append("{");
      StringUtil.ToCommaSeparatedString(builder, members);
      builder.Append("}");
      return builder.ToString();
    }

    internal static string ToCommaSeparatedString(IEnumerable list) => StringUtil.ToSeparatedString(list, ", ", string.Empty);

    internal static void ToCommaSeparatedString(StringBuilder builder, IEnumerable list) => StringUtil.ToSeparatedStringPrivate(builder, list, ", ", string.Empty, false);

    internal static string ToCommaSeparatedStringSorted(IEnumerable list) => StringUtil.ToSeparatedStringSorted(list, ", ", string.Empty);

    internal static void ToCommaSeparatedStringSorted(StringBuilder builder, IEnumerable list) => StringUtil.ToSeparatedStringPrivate(builder, list, ", ", string.Empty, true);

    internal static string ToSeparatedString(IEnumerable list, string separator, string nullValue)
    {
      StringBuilder stringBuilder = new StringBuilder();
      StringUtil.ToSeparatedString(stringBuilder, list, separator, nullValue);
      return stringBuilder.ToString();
    }

    internal static void ToSeparatedString(
      StringBuilder builder,
      IEnumerable list,
      string separator)
    {
      StringUtil.ToSeparatedStringPrivate(builder, list, separator, string.Empty, false);
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal static void ToSeparatedString(
      StringBuilder stringBuilder,
      IEnumerable list,
      string separator,
      string nullValue)
    {
      StringUtil.ToSeparatedStringPrivate(stringBuilder, list, separator, nullValue, false);
    }

    private static void ToSeparatedStringPrivate(
      StringBuilder stringBuilder,
      IEnumerable list,
      string separator,
      string nullValue,
      bool toSort)
    {
      if (list == null)
        return;
      bool flag = true;
      List<string> stringList = new List<string>();
      foreach (object obj in list)
      {
        string str;
        if (obj == null)
          str = nullValue;
        else
          str = StringUtil.FormatInvariant("{0}", obj);
        stringList.Add(str);
      }
      if (toSort)
        stringList.Sort((IComparer<string>) StringComparer.Ordinal);
      foreach (string str in stringList)
      {
        if (!flag)
          stringBuilder.Append(separator);
        stringBuilder.Append(str);
        flag = false;
      }
    }

    internal static string ToSeparatedStringSorted(
      IEnumerable list,
      string separator,
      string nullValue)
    {
      StringBuilder stringBuilder = new StringBuilder();
      StringUtil.ToSeparatedStringPrivate(stringBuilder, list, separator, nullValue, true);
      return stringBuilder.ToString();
    }

    internal static void ToSeparatedStringSorted(
      StringBuilder builder,
      IEnumerable list,
      string separator)
    {
      StringUtil.ToSeparatedStringPrivate(builder, list, separator, string.Empty, true);
    }

    internal delegate string ToStringConverter<T>(T value);
  }
}

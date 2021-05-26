// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.FormUrlEncodedJson
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Properties;
using System.Text;

namespace System.Net.Http.Formatting
{
  internal static class FormUrlEncodedJson
  {
    private const string ApplicationFormUrlEncoded = "application/x-www-form-urlencoded";
    private const int MinDepth = 0;
    private static readonly string[] _emptyPath = new string[1]
    {
      string.Empty
    };

    public static JObject Parse(
      IEnumerable<KeyValuePair<string, string>> nameValuePairs)
    {
      return FormUrlEncodedJson.ParseInternal(nameValuePairs, int.MaxValue, true);
    }

    public static JObject Parse(
      IEnumerable<KeyValuePair<string, string>> nameValuePairs,
      int maxDepth)
    {
      return FormUrlEncodedJson.ParseInternal(nameValuePairs, maxDepth, true);
    }

    public static bool TryParse(
      IEnumerable<KeyValuePair<string, string>> nameValuePairs,
      out JObject value)
    {
      return (value = FormUrlEncodedJson.ParseInternal(nameValuePairs, int.MaxValue, false)) != null;
    }

    public static bool TryParse(
      IEnumerable<KeyValuePair<string, string>> nameValuePairs,
      int maxDepth,
      out JObject value)
    {
      return (value = FormUrlEncodedJson.ParseInternal(nameValuePairs, maxDepth, false)) != null;
    }

    private static JObject ParseInternal(
      IEnumerable<KeyValuePair<string, string>> nameValuePairs,
      int maxDepth,
      bool throwOnError)
    {
      if (nameValuePairs == null)
        throw System.Web.Http.Error.ArgumentNull(nameof (nameValuePairs));
      if (maxDepth <= 0)
        throw System.Web.Http.Error.ArgumentMustBeGreaterThanOrEqualTo(nameof (maxDepth), (object) maxDepth, (object) 1);
      JObject root = new JObject();
      foreach (KeyValuePair<string, string> nameValuePair in nameValuePairs)
      {
        string key = nameValuePair.Key;
        string str = nameValuePair.Value;
        if (key == null)
        {
          if (string.IsNullOrEmpty(str))
          {
            if (throwOnError)
              throw System.Web.Http.Error.Argument(nameof (nameValuePairs), Resources.QueryStringNameShouldNotNull);
            return (JObject) null;
          }
          string[] path = new string[1]{ str };
          if (!FormUrlEncodedJson.Insert(root, path, (string) null, throwOnError))
            return (JObject) null;
        }
        else
        {
          string[] path = FormUrlEncodedJson.GetPath(key, maxDepth, throwOnError);
          if (path == null || !FormUrlEncodedJson.Insert(root, path, str, throwOnError))
            return (JObject) null;
        }
      }
      FormUrlEncodedJson.FixContiguousArrays((JToken) root);
      return root;
    }

    private static string[] GetPath(string key, int maxDepth, bool throwOnError)
    {
      if (string.IsNullOrWhiteSpace(key))
        return FormUrlEncodedJson._emptyPath;
      if (!FormUrlEncodedJson.ValidateQueryString(key, throwOnError))
        return (string[]) null;
      string[] strArray = key.Split('[');
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index].EndsWith("]", StringComparison.Ordinal))
          strArray[index] = strArray[index].Substring(0, strArray[index].Length - 1);
      }
      if (strArray.Length < maxDepth)
        return strArray;
      if (throwOnError)
        throw System.Web.Http.Error.Argument(Resources.MaxDepthExceeded, (object) maxDepth);
      return (string[]) null;
    }

    private static bool ValidateQueryString(string key, bool throwOnError)
    {
      bool flag = false;
      for (int index = 0; index < key.Length; ++index)
      {
        switch (key[index])
        {
          case '[':
            if (!flag)
            {
              flag = true;
              break;
            }
            if (throwOnError)
              throw System.Web.Http.Error.Argument(Resources.NestedBracketNotValid, "application/x-www-form-urlencoded", (object) index);
            return false;
          case ']':
            if (flag)
            {
              flag = false;
              break;
            }
            if (throwOnError)
              throw System.Web.Http.Error.Argument(Resources.UnMatchedBracketNotValid, "application/x-www-form-urlencoded", (object) index);
            return false;
        }
      }
      if (!flag)
        return true;
      if (throwOnError)
        throw System.Web.Http.Error.Argument(Resources.NestedBracketNotValid, "application/x-www-form-urlencoded", (object) key.LastIndexOf('['));
      return false;
    }

    private static bool Insert(JObject root, string[] path, string value, bool throwOnError)
    {
      JObject jobject = root;
      JObject parent = (JObject) null;
      for (int i = 0; i < path.Length - 1; ++i)
      {
        if (string.IsNullOrEmpty(path[i]))
        {
          if (throwOnError)
            throw System.Web.Http.Error.Argument(Resources.InvalidArrayInsert, FormUrlEncodedJson.BuildPathString(path, i));
          return false;
        }
        if (!jobject.ContainsKey(path[i]))
          jobject[path[i]] = (JToken) new JObject();
        else if (jobject[path[i]] == null || jobject[path[i]] is JValue)
        {
          if (throwOnError)
            throw System.Web.Http.Error.Argument(Resources.FormUrlEncodedMismatchingTypes, FormUrlEncodedJson.BuildPathString(path, i));
          return false;
        }
        parent = jobject;
        jobject = jobject[path[i]] as JObject;
      }
      if (string.IsNullOrEmpty(path[path.Length - 1]) && path.Length > 1)
      {
        if (!FormUrlEncodedJson.AddToArray(parent, path, value, throwOnError))
          return false;
      }
      else
      {
        if (jobject == null)
        {
          if (throwOnError)
            throw System.Web.Http.Error.Argument(Resources.FormUrlEncodedMismatchingTypes, FormUrlEncodedJson.BuildPathString(path, path.Length - 1));
          return false;
        }
        if (!FormUrlEncodedJson.AddToObject(jobject, path, value, throwOnError))
          return false;
      }
      return true;
    }

    private static bool AddToObject(JObject obj, string[] path, string value, bool throwOnError)
    {
      int i = path.Length - 1;
      string str1 = path[i];
      if (obj.ContainsKey(str1))
      {
        if (obj[str1] == null || obj[str1].Type == JTokenType.Null)
        {
          if (throwOnError)
            throw System.Web.Http.Error.Argument(Resources.FormUrlEncodedMismatchingTypes, FormUrlEncodedJson.BuildPathString(path, i));
          return false;
        }
        if (path.Length == 1)
        {
          if (obj[str1].Type == JTokenType.String)
          {
            string str2 = obj[str1].ToObject<string>();
            obj[str1] = (JToken) new JObject()
            {
              {
                "0",
                (JToken) str2
              },
              {
                "1",
                (JToken) value
              }
            };
          }
          else if (obj[str1] is JObject)
          {
            JObject jsonObject = obj[str1] as JObject;
            string index = FormUrlEncodedJson.GetIndex(jsonObject, throwOnError);
            if (index == null)
              return false;
            jsonObject.Add(index, (JToken) value);
          }
        }
        else
        {
          if (throwOnError)
            throw System.Web.Http.Error.Argument(Resources.JQuery13CompatModeNotSupportNestedJson, FormUrlEncodedJson.BuildPathString(path, i));
          return false;
        }
      }
      else
        obj[str1] = value != null ? (JToken) value : (JToken) null;
      return true;
    }

    private static bool AddToArray(JObject parent, string[] path, string value, bool throwOnError)
    {
      string propertyName = path[path.Length - 2];
      if (!(parent[propertyName] is JObject jsonObject))
      {
        if (throwOnError)
          throw System.Web.Http.Error.Argument(Resources.FormUrlEncodedMismatchingTypes, FormUrlEncodedJson.BuildPathString(path, path.Length - 1));
        return false;
      }
      string index = FormUrlEncodedJson.GetIndex(jsonObject, throwOnError);
      if (index == null)
        return false;
      jsonObject.Add(index, (JToken) value);
      return true;
    }

    private static string GetIndex(JObject jsonObject, bool throwOnError)
    {
      int num = -1;
      if (jsonObject.Count > 0)
      {
        foreach (string key in (IEnumerable<string>) ((IDictionary<string, JToken>) jsonObject).Keys)
        {
          int result;
          if (int.TryParse(key, NumberStyles.Integer, (IFormatProvider) NumberFormatInfo.InvariantInfo, out result) && result > num)
          {
            num = result;
          }
          else
          {
            if (throwOnError)
              throw System.Web.Http.Error.Argument(Resources.FormUrlEncodedMismatchingTypes, key);
            return (string) null;
          }
        }
      }
      return (num + 1).ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    private static void FixContiguousArrays(JToken jv)
    {
      switch (jv)
      {
        case JArray jarray:
          for (int index = 0; index < jarray.Count; ++index)
          {
            if (jarray[index] != null)
            {
              jarray[index] = FormUrlEncodedJson.FixSingleContiguousArray(jarray[index]);
              FormUrlEncodedJson.FixContiguousArrays(jarray[index]);
            }
          }
          break;
        case JObject jobject:
          if (jobject.Count <= 0)
            break;
          using (List<string>.Enumerator enumerator = new List<string>((IEnumerable<string>) ((IDictionary<string, JToken>) jobject).Keys).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              string current = enumerator.Current;
              if (jobject[current] != null)
              {
                jobject[current] = FormUrlEncodedJson.FixSingleContiguousArray(jobject[current]);
                FormUrlEncodedJson.FixContiguousArrays(jobject[current]);
              }
            }
            break;
          }
      }
    }

    private static JToken FixSingleContiguousArray(JToken original)
    {
      List<string> sortedKeys;
      if (!(original is JObject jobject) || jobject.Count <= 0 || !FormUrlEncodedJson.CanBecomeArray(new List<string>((IEnumerable<string>) ((IDictionary<string, JToken>) jobject).Keys), out sortedKeys))
        return original;
      JArray jarray = new JArray();
      foreach (string propertyName in sortedKeys)
        jarray.Add(jobject[propertyName]);
      return (JToken) jarray;
    }

    private static bool CanBecomeArray(List<string> keys, out List<string> sortedKeys)
    {
      List<FormUrlEncodedJson.ArrayCandidate> source = new List<FormUrlEncodedJson.ArrayCandidate>();
      sortedKeys = (List<string>) null;
      bool flag = true;
      foreach (string key in keys)
      {
        int result;
        if (!int.TryParse(key, NumberStyles.None, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        {
          flag = false;
          break;
        }
        string str = result.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        if (!str.Equals(key, StringComparison.Ordinal))
        {
          flag = false;
          break;
        }
        source.Add(new FormUrlEncodedJson.ArrayCandidate(result, str));
      }
      if (flag)
      {
        source.Sort((Comparison<FormUrlEncodedJson.ArrayCandidate>) ((x, y) => x.Key - y.Key));
        for (int index = 0; index < source.Count; ++index)
        {
          if (source[index].Key != index)
          {
            flag = false;
            break;
          }
        }
      }
      if (flag)
        sortedKeys = new List<string>(source.Select<FormUrlEncodedJson.ArrayCandidate, string>((Func<FormUrlEncodedJson.ArrayCandidate, string>) (x => x.Value)));
      return flag;
    }

    private static string BuildPathString(string[] path, int i)
    {
      StringBuilder stringBuilder = new StringBuilder(path[0]);
      for (int index = 1; index <= i; ++index)
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "[{0}]", (object) path[index]);
      return stringBuilder.ToString();
    }

    private class ArrayCandidate
    {
      public ArrayCandidate(int key, string value)
      {
        this.Key = key;
        this.Value = value;
      }

      public int Key { get; set; }

      public string Value { get; set; }
    }
  }
}

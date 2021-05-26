// Decompiled with JetBrains decompiler
// Type: HtmlAgilityPack.NameValuePairList
// Assembly: HtmlAgilityPack, Version=1.11.24.0, Culture=neutral, PublicKeyToken=bd319b19eaf3b43a
// MVID: 0D2121F1-AAF8-4C0B-8205-7FF2BEA3525B
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\HtmlAgilityPack.dll

using System;
using System.Collections.Generic;

namespace HtmlAgilityPack
{
  internal class NameValuePairList
  {
    internal readonly string Text;
    private List<KeyValuePair<string, string>> _allPairs;
    private Dictionary<string, List<KeyValuePair<string, string>>> _pairsWithName;

    internal NameValuePairList()
      : this((string) null)
    {
    }

    internal NameValuePairList(string text)
    {
      this.Text = text;
      this._allPairs = new List<KeyValuePair<string, string>>();
      this._pairsWithName = new Dictionary<string, List<KeyValuePair<string, string>>>();
      this.Parse(text);
    }

    internal static string GetNameValuePairsValue(string text, string name) => new NameValuePairList(text).GetNameValuePairValue(name);

    internal List<KeyValuePair<string, string>> GetNameValuePairs(string name)
    {
      if (name == null)
        return this._allPairs;
      return !this._pairsWithName.ContainsKey(name) ? new List<KeyValuePair<string, string>>() : this._pairsWithName[name];
    }

    internal string GetNameValuePairValue(string name)
    {
      List<KeyValuePair<string, string>> keyValuePairList = name != null ? this.GetNameValuePairs(name) : throw new ArgumentNullException();
      return keyValuePairList.Count == 0 ? string.Empty : keyValuePairList[0].Value.Trim();
    }

    private void Parse(string text)
    {
      this._allPairs.Clear();
      this._pairsWithName.Clear();
      if (text == null)
        return;
      string str1 = text;
      char[] chArray = new char[1]{ ';' };
      foreach (string str2 in str1.Split(chArray))
      {
        if (str2.Length != 0)
        {
          string[] strArray = str2.Split(new char[1]{ '=' }, 2);
          if (strArray.Length != 0)
          {
            KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(strArray[0].Trim().ToLowerInvariant(), strArray.Length < 2 ? "" : strArray[1]);
            this._allPairs.Add(keyValuePair);
            List<KeyValuePair<string, string>> keyValuePairList;
            if (!this._pairsWithName.TryGetValue(keyValuePair.Key, out keyValuePairList))
            {
              keyValuePairList = new List<KeyValuePair<string, string>>();
              this._pairsWithName.Add(keyValuePair.Key, keyValuePairList);
            }
            keyValuePairList.Add(keyValuePair);
          }
        }
      }
    }
  }
}

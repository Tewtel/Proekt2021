// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.FieldNameLookup
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace System.Data.Entity.Core
{
  internal sealed class FieldNameLookup
  {
    private readonly Dictionary<string, int> _fieldNameLookup = new Dictionary<string, int>();
    private readonly string[] _fieldNames;

    public FieldNameLookup(ReadOnlyCollection<string> columnNames)
    {
      int count = columnNames.Count;
      this._fieldNames = new string[count];
      for (int index = 0; index < count; ++index)
        this._fieldNames[index] = columnNames[index];
      this.GenerateLookup();
    }

    public FieldNameLookup(IDataRecord reader)
    {
      int fieldCount = reader.FieldCount;
      this._fieldNames = new string[fieldCount];
      for (int i = 0; i < fieldCount; ++i)
        this._fieldNames[i] = reader.GetName(i);
      this.GenerateLookup();
    }

    public int GetOrdinal(string fieldName)
    {
      System.Data.Entity.Utilities.Check.NotNull<string>(fieldName, nameof (fieldName));
      int num = this.IndexOf(fieldName);
      return num != -1 ? num : throw new IndexOutOfRangeException(fieldName);
    }

    private int IndexOf(string fieldName)
    {
      int num;
      if (!this._fieldNameLookup.TryGetValue(fieldName, out num))
      {
        num = this.LinearIndexOf(fieldName, CompareOptions.IgnoreCase);
        if (num == -1)
          num = this.LinearIndexOf(fieldName, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
      }
      return num;
    }

    private int LinearIndexOf(string fieldName, CompareOptions compareOptions)
    {
      for (int index = 0; index < this._fieldNames.Length; ++index)
      {
        if (CultureInfo.InvariantCulture.CompareInfo.Compare(fieldName, this._fieldNames[index], compareOptions) == 0)
        {
          this._fieldNameLookup[fieldName] = index;
          return index;
        }
      }
      return -1;
    }

    private void GenerateLookup()
    {
      for (int index = this._fieldNames.Length - 1; 0 <= index; --index)
        this._fieldNameLookup[this._fieldNames[index]] = index;
    }
  }
}

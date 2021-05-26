// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.SchemaEnumMember
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Globalization;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal class SchemaEnumMember : SchemaElement
  {
    private long? _value;

    public SchemaEnumMember(SchemaElement parentElement)
      : base(parentElement)
    {
    }

    public long? Value
    {
      get => this._value;
      set => this._value = value;
    }

    protected override bool HandleAttribute(XmlReader reader)
    {
      bool flag = base.HandleAttribute(reader);
      if (!flag && (flag = SchemaElement.CanHandleAttribute(reader, "Value")))
        this.HandleValueAttribute(reader);
      return flag;
    }

    private void HandleValueAttribute(XmlReader reader)
    {
      long result;
      if (!long.TryParse(reader.Value, NumberStyles.AllowLeadingSign, (IFormatProvider) CultureInfo.InvariantCulture, out result))
        return;
      this._value = new long?(result);
    }
  }
}

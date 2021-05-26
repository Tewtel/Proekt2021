﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.FunctionCommandText
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;
using System.Xml;

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal sealed class FunctionCommandText : SchemaElement
  {
    private string _commandText;

    public FunctionCommandText(Function parentElement)
      : base((SchemaElement) parentElement)
    {
    }

    public string CommandText => this._commandText;

    protected override bool HandleText(XmlReader reader)
    {
      this._commandText = reader.Value;
      return true;
    }

    internal override void Validate()
    {
      base.Validate();
      if (!string.IsNullOrEmpty(this._commandText))
        return;
      this.AddError(ErrorCode.EmptyCommandText, EdmSchemaErrorSeverity.Error, (object) Strings.EmptyCommandText);
    }
  }
}

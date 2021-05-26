// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.SchemaType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal abstract class SchemaType : SchemaElement
  {
    public string Namespace => this.Schema.Namespace;

    public override string Identity => this.Namespace + "." + this.Name;

    public override string FQName => this.Namespace + "." + this.Name;

    internal SchemaType(Schema parentElement)
      : base((SchemaElement) parentElement)
    {
    }
  }
}

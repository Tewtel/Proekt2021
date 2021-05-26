// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.SchemaObjectModel.PropertyRefElement
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.SchemaObjectModel
{
  internal sealed class PropertyRefElement : SchemaElement
  {
    private StructuredProperty _property;

    public PropertyRefElement(SchemaElement parentElement)
      : base(parentElement)
    {
    }

    public StructuredProperty Property => this._property;

    internal override void ResolveTopLevelNames()
    {
    }

    internal bool ResolveNames(SchemaEntityType entityType)
    {
      if (string.IsNullOrEmpty(this.Name))
        return true;
      this._property = entityType.FindProperty(this.Name);
      return this._property != null;
    }
  }
}

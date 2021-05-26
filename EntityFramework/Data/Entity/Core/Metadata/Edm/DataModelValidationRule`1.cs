// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.DataModelValidationRule`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal abstract class DataModelValidationRule<TItem> : DataModelValidationRule
    where TItem : class
  {
    protected Action<EdmModelValidationContext, TItem> _validate;

    internal DataModelValidationRule(Action<EdmModelValidationContext, TItem> validate) => this._validate = validate;

    internal override Type ValidatedType => typeof (TItem);

    internal override void Evaluate(EdmModelValidationContext context, MetadataItem item) => this._validate(context, item as TItem);
  }
}

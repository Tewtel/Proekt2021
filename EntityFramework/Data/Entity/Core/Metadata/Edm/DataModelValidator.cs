// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.DataModelValidator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class DataModelValidator
  {
    public event EventHandler<DataModelErrorEventArgs> OnError;

    public void Validate(EdmModel model, bool validateSyntax)
    {
      EdmModelValidationContext context = new EdmModelValidationContext(model, validateSyntax);
      context.OnError += this.OnError;
      new EdmModelValidationVisitor(context, EdmModelRuleSet.CreateEdmModelRuleSet(model.SchemaVersion, validateSyntax)).Visit(model);
    }
  }
}

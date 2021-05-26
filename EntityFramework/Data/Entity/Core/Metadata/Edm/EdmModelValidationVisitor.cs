// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.EdmModelValidationVisitor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Edm;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal sealed class EdmModelValidationVisitor : EdmModelVisitor
  {
    private readonly EdmModelValidationContext _context;
    private readonly EdmModelRuleSet _ruleSet;
    private readonly HashSet<MetadataItem> _visitedItems = new HashSet<MetadataItem>();

    internal EdmModelValidationVisitor(EdmModelValidationContext context, EdmModelRuleSet ruleSet)
    {
      this._context = context;
      this._ruleSet = ruleSet;
    }

    protected internal override void VisitMetadataItem(MetadataItem item)
    {
      if (!this._visitedItems.Add(item))
        return;
      this.EvaluateItem(item);
    }

    private void EvaluateItem(MetadataItem item)
    {
      foreach (DataModelValidationRule rule in this._ruleSet.GetRules(item))
        rule.Evaluate(this._context, item);
    }

    internal void Visit(EdmModel model)
    {
      this.EvaluateItem((MetadataItem) model);
      this.VisitEdmModel(model);
    }
  }
}

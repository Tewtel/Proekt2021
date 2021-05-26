// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.DataModelValidationRuleSet
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal abstract class DataModelValidationRuleSet
  {
    private readonly List<DataModelValidationRule> _rules = new List<DataModelValidationRule>();

    protected void AddRule(DataModelValidationRule rule) => this._rules.Add(rule);

    protected void RemoveRule(DataModelValidationRule rule) => this._rules.Remove(rule);

    internal IEnumerable<DataModelValidationRule> GetRules(
      MetadataItem itemToValidate)
    {
      return this._rules.Where<DataModelValidationRule>((Func<DataModelValidationRule, bool>) (r => r.ValidatedType.IsInstanceOfType((object) itemToValidate)));
    }
  }
}

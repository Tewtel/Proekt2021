// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Mapping.PropertyMappingSpecification
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.ModelConfiguration.Configuration.Mapping
{
  internal class PropertyMappingSpecification
  {
    private readonly EntityType _entityType;
    private readonly IList<EdmProperty> _propertyPath;
    private readonly IList<ConditionPropertyMapping> _conditions;
    private readonly bool _isDefaultDiscriminatorCondition;

    public PropertyMappingSpecification(
      EntityType entityType,
      IList<EdmProperty> propertyPath,
      IList<ConditionPropertyMapping> conditions,
      bool isDefaultDiscriminatorCondition)
    {
      this._entityType = entityType;
      this._propertyPath = propertyPath;
      this._conditions = conditions;
      this._isDefaultDiscriminatorCondition = isDefaultDiscriminatorCondition;
    }

    public EntityType EntityType => this._entityType;

    public IList<EdmProperty> PropertyPath => this._propertyPath;

    public IList<ConditionPropertyMapping> Conditions => this._conditions;

    public bool IsDefaultDiscriminatorCondition => this._isDefaultDiscriminatorCondition;
  }
}

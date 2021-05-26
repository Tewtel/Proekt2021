﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ExplicitDiscriminatorMap
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Mapping.ViewGeneration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class ExplicitDiscriminatorMap
  {
    private readonly ReadOnlyCollection<KeyValuePair<object, EntityType>> m_typeMap;
    private readonly EdmMember m_discriminatorProperty;
    private readonly ReadOnlyCollection<EdmProperty> m_properties;

    internal ExplicitDiscriminatorMap(DiscriminatorMap template)
    {
      this.m_typeMap = template.TypeMap;
      this.m_discriminatorProperty = template.Discriminator.Property;
      this.m_properties = new ReadOnlyCollection<EdmProperty>((IList<EdmProperty>) template.PropertyMap.Select<KeyValuePair<EdmProperty, DbExpression>, EdmProperty>((Func<KeyValuePair<EdmProperty, DbExpression>, EdmProperty>) (propertyValuePair => propertyValuePair.Key)).ToList<EdmProperty>());
    }

    internal ReadOnlyCollection<KeyValuePair<object, EntityType>> TypeMap => this.m_typeMap;

    internal EdmMember DiscriminatorProperty => this.m_discriminatorProperty;

    internal ReadOnlyCollection<EdmProperty> Properties => this.m_properties;

    internal object GetTypeId(EntityType entityType)
    {
      object obj = (object) null;
      foreach (KeyValuePair<object, EntityType> type in this.TypeMap)
      {
        if (type.Value.EdmEquals((MetadataItem) entityType))
        {
          obj = type.Key;
          break;
        }
      }
      return obj;
    }
  }
}

﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ExpensiveOSpaceLoader
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Utilities;
using System.Reflection;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class ExpensiveOSpaceLoader
  {
    public virtual Dictionary<string, EdmType> LoadTypesExpensiveWay(
      Assembly assembly)
    {
      KnownAssembliesSet knownAssemblies = new KnownAssembliesSet();
      Dictionary<string, EdmType> typesInLoading;
      List<EdmItemError> errors;
      AssemblyCache.LoadAssembly(assembly, false, knownAssemblies, out typesInLoading, out errors);
      if (errors.Count != 0)
        throw EntityUtil.InvalidSchemaEncountered(Helper.CombineErrorMessage((IEnumerable<EdmItemError>) errors));
      return typesInLoading;
    }

    public virtual AssociationType GetRelationshipTypeExpensiveWay(
      Type entityClrType,
      string relationshipName)
    {
      Dictionary<string, EdmType> dictionary = this.LoadTypesExpensiveWay(entityClrType.Assembly());
      EdmType type;
      return dictionary != null && dictionary.TryGetValue(relationshipName, out type) && Helper.IsRelationshipType(type) ? (AssociationType) type : (AssociationType) null;
    }

    public virtual IEnumerable<AssociationType> GetAllRelationshipTypesExpensiveWay(
      Assembly assembly)
    {
      Dictionary<string, EdmType> dictionary = this.LoadTypesExpensiveWay(assembly);
      if (dictionary != null)
      {
        foreach (EdmType type in dictionary.Values)
        {
          if (Helper.IsAssociationType(type))
            yield return (AssociationType) type;
        }
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.FunctionImportMappingNonComposable
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Represents a mapping from a model function import to a store non-composable function.
  /// </summary>
  public sealed class FunctionImportMappingNonComposable : FunctionImportMapping
  {
    private readonly ReadOnlyCollection<FunctionImportResultMapping> _resultMappings;
    private readonly bool noExplicitResultMappings;
    private readonly ReadOnlyCollection<FunctionImportStructuralTypeMappingKB> _internalResultMappings;

    /// <summary>
    /// Initializes a new FunctionImportMappingNonComposable instance.
    /// </summary>
    /// <param name="functionImport">The model function import.</param>
    /// <param name="targetFunction">The store non-composable function.</param>
    /// <param name="resultMappings">The function import result mappings.</param>
    /// <param name="containerMapping">The parent container mapping.</param>
    public FunctionImportMappingNonComposable(
      EdmFunction functionImport,
      EdmFunction targetFunction,
      IEnumerable<FunctionImportResultMapping> resultMappings,
      EntityContainerMapping containerMapping)
      : base(System.Data.Entity.Utilities.Check.NotNull<EdmFunction>(functionImport, nameof (functionImport)), System.Data.Entity.Utilities.Check.NotNull<EdmFunction>(targetFunction, nameof (targetFunction)))
    {
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<FunctionImportResultMapping>>(resultMappings, nameof (resultMappings));
      System.Data.Entity.Utilities.Check.NotNull<EntityContainerMapping>(containerMapping, nameof (containerMapping));
      if (!resultMappings.Any<FunctionImportResultMapping>())
      {
        this._internalResultMappings = new ReadOnlyCollection<FunctionImportStructuralTypeMappingKB>((IList<FunctionImportStructuralTypeMappingKB>) new FunctionImportStructuralTypeMappingKB[1]
        {
          new FunctionImportStructuralTypeMappingKB((IEnumerable<FunctionImportStructuralTypeMapping>) new List<FunctionImportStructuralTypeMapping>(), (ItemCollection) (containerMapping.StorageMappingItemCollection != null ? containerMapping.StorageMappingItemCollection.EdmItemCollection : new EdmItemCollection(new EdmModel(DataSpace.CSpace))))
        });
        this.noExplicitResultMappings = true;
      }
      else
      {
        this._internalResultMappings = new ReadOnlyCollection<FunctionImportStructuralTypeMappingKB>((IList<FunctionImportStructuralTypeMappingKB>) resultMappings.Select<FunctionImportResultMapping, FunctionImportStructuralTypeMappingKB>((Func<FunctionImportResultMapping, FunctionImportStructuralTypeMappingKB>) (resultMapping => new FunctionImportStructuralTypeMappingKB((IEnumerable<FunctionImportStructuralTypeMapping>) resultMapping.TypeMappings, (ItemCollection) containerMapping.StorageMappingItemCollection.EdmItemCollection))).ToArray<FunctionImportStructuralTypeMappingKB>());
        this.noExplicitResultMappings = false;
      }
      this._resultMappings = new ReadOnlyCollection<FunctionImportResultMapping>((IList<FunctionImportResultMapping>) resultMappings.ToList<FunctionImportResultMapping>());
    }

    internal FunctionImportMappingNonComposable(
      EdmFunction functionImport,
      EdmFunction targetFunction,
      List<List<FunctionImportStructuralTypeMapping>> structuralTypeMappingsList,
      ItemCollection itemCollection)
      : base(functionImport, targetFunction)
    {
      if (structuralTypeMappingsList.Count == 0)
      {
        this._internalResultMappings = new ReadOnlyCollection<FunctionImportStructuralTypeMappingKB>((IList<FunctionImportStructuralTypeMappingKB>) new FunctionImportStructuralTypeMappingKB[1]
        {
          new FunctionImportStructuralTypeMappingKB((IEnumerable<FunctionImportStructuralTypeMapping>) new List<FunctionImportStructuralTypeMapping>(), itemCollection)
        });
        this.noExplicitResultMappings = true;
      }
      else
      {
        this._internalResultMappings = new ReadOnlyCollection<FunctionImportStructuralTypeMappingKB>((IList<FunctionImportStructuralTypeMappingKB>) structuralTypeMappingsList.Select<List<FunctionImportStructuralTypeMapping>, FunctionImportStructuralTypeMappingKB>((Func<List<FunctionImportStructuralTypeMapping>, FunctionImportStructuralTypeMappingKB>) (structuralTypeMappings => new FunctionImportStructuralTypeMappingKB((IEnumerable<FunctionImportStructuralTypeMapping>) structuralTypeMappings, itemCollection))).ToArray<FunctionImportStructuralTypeMappingKB>());
        this.noExplicitResultMappings = false;
      }
    }

    internal ReadOnlyCollection<FunctionImportStructuralTypeMappingKB> InternalResultMappings => this._internalResultMappings;

    /// <summary>Gets the function import result mappings.</summary>
    public ReadOnlyCollection<FunctionImportResultMapping> ResultMappings => this._resultMappings;

    internal override void SetReadOnly()
    {
      MappingItem.SetReadOnly((IEnumerable<MappingItem>) this._resultMappings);
      base.SetReadOnly();
    }

    internal FunctionImportStructuralTypeMappingKB GetResultMapping(
      int resultSetIndex)
    {
      if (this.noExplicitResultMappings)
        return this.InternalResultMappings[0];
      if (this.InternalResultMappings.Count <= resultSetIndex)
        throw new ArgumentOutOfRangeException(nameof (resultSetIndex));
      return this.InternalResultMappings[resultSetIndex];
    }

    internal IList<string> GetDiscriminatorColumns(int resultSetIndex) => (IList<string>) this.GetResultMapping(resultSetIndex).DiscriminatorColumns;

    internal EntityType Discriminate(object[] discriminatorValues, int resultSetIndex)
    {
      FunctionImportStructuralTypeMappingKB resultMapping = this.GetResultMapping(resultSetIndex);
      BitArray bitArray = new BitArray(resultMapping.MappedEntityTypes.Count, true);
      foreach (FunctionImportNormalizedEntityTypeMapping entityTypeMapping in resultMapping.NormalizedEntityTypeMappings)
      {
        bool flag = true;
        ReadOnlyCollection<FunctionImportEntityTypeMappingCondition> columnConditions = entityTypeMapping.ColumnConditions;
        for (int index = 0; index < columnConditions.Count; ++index)
        {
          if (columnConditions[index] != null && !columnConditions[index].ColumnValueMatchesCondition(discriminatorValues[index]))
          {
            flag = false;
            break;
          }
        }
        bitArray = !flag ? bitArray.And(entityTypeMapping.ComplementImpliedEntityTypes) : bitArray.And(entityTypeMapping.ImpliedEntityTypes);
      }
      EntityType entityType = (EntityType) null;
      for (int index = 0; index < bitArray.Length; ++index)
      {
        if (bitArray[index])
        {
          if (entityType != null)
            throw new EntityCommandExecutionException(System.Data.Entity.Resources.Strings.ADP_InvalidDataReaderUnableToDetermineType);
          entityType = resultMapping.MappedEntityTypes[index];
        }
      }
      return entityType != null ? entityType : throw new EntityCommandExecutionException(System.Data.Entity.Resources.Strings.ADP_InvalidDataReaderUnableToDetermineType);
    }

    internal TypeUsage GetExpectedTargetResultType(int resultSetIndex)
    {
      FunctionImportStructuralTypeMappingKB resultMapping = this.GetResultMapping(resultSetIndex);
      Dictionary<string, TypeUsage> source = new Dictionary<string, TypeUsage>();
      IEnumerable<StructuralType> structuralTypes;
      if (resultMapping.NormalizedEntityTypeMappings.Count == 0)
      {
        StructuralType returnType;
        MetadataHelper.TryGetFunctionImportReturnType<StructuralType>(this.FunctionImport, resultSetIndex, out returnType);
        structuralTypes = (IEnumerable<StructuralType>) new StructuralType[1]
        {
          returnType
        };
      }
      else
        structuralTypes = resultMapping.MappedEntityTypes.Cast<StructuralType>();
      foreach (EdmType edmType in structuralTypes)
      {
        foreach (EdmProperty structuralMember in (IEnumerable) TypeHelpers.GetAllStructuralMembers(edmType))
          source[structuralMember.Name] = structuralMember.TypeUsage;
      }
      foreach (string discriminatorColumn in (IEnumerable<string>) this.GetDiscriminatorColumns(resultSetIndex))
      {
        if (!source.ContainsKey(discriminatorColumn))
        {
          TypeUsage stringTypeUsage = TypeUsage.CreateStringTypeUsage(MetadataWorkspace.GetModelPrimitiveType(PrimitiveTypeKind.String), true, false);
          source.Add(discriminatorColumn, stringTypeUsage);
        }
      }
      return TypeUsage.Create((EdmType) new CollectionType(TypeUsage.Create((EdmType) new RowType(source.Select<KeyValuePair<string, TypeUsage>, EdmProperty>((Func<KeyValuePair<string, TypeUsage>, EdmProperty>) (c => new EdmProperty(c.Key, c.Value)))))));
    }
  }
}

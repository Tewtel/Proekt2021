// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.ColumnMapTranslator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.InternalTrees;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class ColumnMapTranslator : 
    ColumnMapVisitorWithResults<ColumnMap, ColumnMapTranslatorTranslationDelegate>
  {
    private static readonly ColumnMapTranslator _instance = new ColumnMapTranslator();

    private ColumnMapTranslator()
    {
    }

    private static Var GetReplacementVar(
      Var originalVar,
      IDictionary<Var, Var> replacementVarMap)
    {
      Var key = originalVar;
      while (replacementVarMap.TryGetValue(key, out originalVar) && originalVar != key)
        key = originalVar;
      return key;
    }

    internal static ColumnMap Translate(
      ColumnMap columnMap,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
    {
      return columnMap.Accept<ColumnMap, ColumnMapTranslatorTranslationDelegate>((ColumnMapVisitorWithResults<ColumnMap, ColumnMapTranslatorTranslationDelegate>) ColumnMapTranslator._instance, translationDelegate);
    }

    internal static ColumnMap Translate(
      ColumnMap columnMapToTranslate,
      Dictionary<Var, ColumnMap> varToColumnMap)
    {
      return ColumnMapTranslator.Translate(columnMapToTranslate, (ColumnMapTranslatorTranslationDelegate) (columnMap =>
      {
        if (columnMap is VarRefColumnMap varRefColumnMap2)
        {
          if (varToColumnMap.TryGetValue(varRefColumnMap2.Var, out columnMap))
          {
            if (!columnMap.IsNamed && varRefColumnMap2.IsNamed)
              columnMap.Name = varRefColumnMap2.Name;
            if (Helper.IsEnumType(varRefColumnMap2.Type.EdmType) && varRefColumnMap2.Type.EdmType != columnMap.Type.EdmType)
              columnMap.Type = varRefColumnMap2.Type;
          }
          else
            columnMap = (ColumnMap) varRefColumnMap2;
        }
        return columnMap;
      }));
    }

    internal static ColumnMap Translate(
      ColumnMap columnMapToTranslate,
      IDictionary<Var, Var> varToVarMap)
    {
      return ColumnMapTranslator.Translate(columnMapToTranslate, (ColumnMapTranslatorTranslationDelegate) (columnMap =>
      {
        if (columnMap is VarRefColumnMap varRefColumnMap2)
        {
          Var replacementVar = ColumnMapTranslator.GetReplacementVar(varRefColumnMap2.Var, varToVarMap);
          if (varRefColumnMap2.Var != replacementVar)
            columnMap = (ColumnMap) new VarRefColumnMap(varRefColumnMap2.Type, varRefColumnMap2.Name, replacementVar);
        }
        return columnMap;
      }));
    }

    internal static ColumnMap Translate(
      ColumnMap columnMapToTranslate,
      Dictionary<Var, KeyValuePair<int, int>> varToCommandColumnMap)
    {
      return ColumnMapTranslator.Translate(columnMapToTranslate, (ColumnMapTranslatorTranslationDelegate) (columnMap =>
      {
        if (columnMap is VarRefColumnMap varRefColumnMap2)
        {
          KeyValuePair<int, int> keyValuePair;
          if (!varToCommandColumnMap.TryGetValue(varRefColumnMap2.Var, out keyValuePair))
            throw EntityUtil.InternalError(EntityUtil.InternalErrorCode.UnknownVar, 1, (object) varRefColumnMap2.Var.Id);
          columnMap = (ColumnMap) new ScalarColumnMap(varRefColumnMap2.Type, varRefColumnMap2.Name, keyValuePair.Key, keyValuePair.Value);
        }
        if (!columnMap.IsNamed)
          columnMap.Name = "Value";
        return columnMap;
      }));
    }

    private void VisitList<TResultType>(
      TResultType[] tList,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
      where TResultType : ColumnMap
    {
      for (int index = 0; index < tList.Length; ++index)
        tList[index] = (TResultType) tList[index].Accept<ColumnMap, ColumnMapTranslatorTranslationDelegate>((ColumnMapVisitorWithResults<ColumnMap, ColumnMapTranslatorTranslationDelegate>) this, translationDelegate);
    }

    protected override EntityIdentity VisitEntityIdentity(
      DiscriminatedEntityIdentity entityIdentity,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
    {
      ColumnMap columnMap = entityIdentity.EntitySetColumnMap.Accept<ColumnMap, ColumnMapTranslatorTranslationDelegate>((ColumnMapVisitorWithResults<ColumnMap, ColumnMapTranslatorTranslationDelegate>) this, translationDelegate);
      this.VisitList<SimpleColumnMap>(entityIdentity.Keys, translationDelegate);
      if (columnMap != entityIdentity.EntitySetColumnMap)
        entityIdentity = new DiscriminatedEntityIdentity((SimpleColumnMap) columnMap, entityIdentity.EntitySetMap, entityIdentity.Keys);
      return (EntityIdentity) entityIdentity;
    }

    protected override EntityIdentity VisitEntityIdentity(
      SimpleEntityIdentity entityIdentity,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
    {
      this.VisitList<SimpleColumnMap>(entityIdentity.Keys, translationDelegate);
      return (EntityIdentity) entityIdentity;
    }

    internal override ColumnMap Visit(
      ComplexTypeColumnMap columnMap,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
    {
      SimpleColumnMap nullSentinel = columnMap.NullSentinel;
      if (nullSentinel != null)
        nullSentinel = (SimpleColumnMap) translationDelegate((ColumnMap) nullSentinel);
      this.VisitList<ColumnMap>(columnMap.Properties, translationDelegate);
      if (columnMap.NullSentinel != nullSentinel)
        columnMap = new ComplexTypeColumnMap(columnMap.Type, columnMap.Name, columnMap.Properties, nullSentinel);
      return translationDelegate((ColumnMap) columnMap);
    }

    internal override ColumnMap Visit(
      DiscriminatedCollectionColumnMap columnMap,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
    {
      ColumnMap columnMap1 = columnMap.Discriminator.Accept<ColumnMap, ColumnMapTranslatorTranslationDelegate>((ColumnMapVisitorWithResults<ColumnMap, ColumnMapTranslatorTranslationDelegate>) this, translationDelegate);
      this.VisitList<SimpleColumnMap>(columnMap.ForeignKeys, translationDelegate);
      this.VisitList<SimpleColumnMap>(columnMap.Keys, translationDelegate);
      ColumnMap elementMap = columnMap.Element.Accept<ColumnMap, ColumnMapTranslatorTranslationDelegate>((ColumnMapVisitorWithResults<ColumnMap, ColumnMapTranslatorTranslationDelegate>) this, translationDelegate);
      if (columnMap1 != columnMap.Discriminator || elementMap != columnMap.Element)
        columnMap = new DiscriminatedCollectionColumnMap(columnMap.Type, columnMap.Name, elementMap, columnMap.Keys, columnMap.ForeignKeys, (SimpleColumnMap) columnMap1, columnMap.DiscriminatorValue);
      return translationDelegate((ColumnMap) columnMap);
    }

    internal override ColumnMap Visit(
      EntityColumnMap columnMap,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
    {
      EntityIdentity entityIdentity = this.VisitEntityIdentity(columnMap.EntityIdentity, translationDelegate);
      this.VisitList<ColumnMap>(columnMap.Properties, translationDelegate);
      if (entityIdentity != columnMap.EntityIdentity)
        columnMap = new EntityColumnMap(columnMap.Type, columnMap.Name, columnMap.Properties, entityIdentity);
      return translationDelegate((ColumnMap) columnMap);
    }

    internal override ColumnMap Visit(
      SimplePolymorphicColumnMap columnMap,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
    {
      ColumnMap columnMap1 = columnMap.TypeDiscriminator.Accept<ColumnMap, ColumnMapTranslatorTranslationDelegate>((ColumnMapVisitorWithResults<ColumnMap, ColumnMapTranslatorTranslationDelegate>) this, translationDelegate);
      Dictionary<object, TypedColumnMap> typeChoices = columnMap.TypeChoices;
      foreach (KeyValuePair<object, TypedColumnMap> typeChoice in columnMap.TypeChoices)
      {
        TypedColumnMap typedColumnMap = (TypedColumnMap) typeChoice.Value.Accept<ColumnMap, ColumnMapTranslatorTranslationDelegate>((ColumnMapVisitorWithResults<ColumnMap, ColumnMapTranslatorTranslationDelegate>) this, translationDelegate);
        if (typedColumnMap != typeChoice.Value)
        {
          if (typeChoices == columnMap.TypeChoices)
            typeChoices = new Dictionary<object, TypedColumnMap>((IDictionary<object, TypedColumnMap>) columnMap.TypeChoices);
          typeChoices[typeChoice.Key] = typedColumnMap;
        }
      }
      this.VisitList<ColumnMap>(columnMap.Properties, translationDelegate);
      if (columnMap1 != columnMap.TypeDiscriminator || typeChoices != columnMap.TypeChoices)
        columnMap = new SimplePolymorphicColumnMap(columnMap.Type, columnMap.Name, columnMap.Properties, (SimpleColumnMap) columnMap1, typeChoices);
      return translationDelegate((ColumnMap) columnMap);
    }

    internal override ColumnMap Visit(
      MultipleDiscriminatorPolymorphicColumnMap columnMap,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(false, "unexpected MultipleDiscriminatorPolymorphicColumnMap in ColumnMapTranslator");
      return (ColumnMap) null;
    }

    internal override ColumnMap Visit(
      RecordColumnMap columnMap,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
    {
      SimpleColumnMap nullSentinel = columnMap.NullSentinel;
      if (nullSentinel != null)
        nullSentinel = (SimpleColumnMap) translationDelegate((ColumnMap) nullSentinel);
      this.VisitList<ColumnMap>(columnMap.Properties, translationDelegate);
      if (columnMap.NullSentinel != nullSentinel)
        columnMap = new RecordColumnMap(columnMap.Type, columnMap.Name, columnMap.Properties, nullSentinel);
      return translationDelegate((ColumnMap) columnMap);
    }

    internal override ColumnMap Visit(
      RefColumnMap columnMap,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
    {
      EntityIdentity entityIdentity = this.VisitEntityIdentity(columnMap.EntityIdentity, translationDelegate);
      if (entityIdentity != columnMap.EntityIdentity)
        columnMap = new RefColumnMap(columnMap.Type, columnMap.Name, entityIdentity);
      return translationDelegate((ColumnMap) columnMap);
    }

    internal override ColumnMap Visit(
      ScalarColumnMap columnMap,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
    {
      return translationDelegate((ColumnMap) columnMap);
    }

    internal override ColumnMap Visit(
      SimpleCollectionColumnMap columnMap,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
    {
      this.VisitList<SimpleColumnMap>(columnMap.ForeignKeys, translationDelegate);
      this.VisitList<SimpleColumnMap>(columnMap.Keys, translationDelegate);
      ColumnMap elementMap = columnMap.Element.Accept<ColumnMap, ColumnMapTranslatorTranslationDelegate>((ColumnMapVisitorWithResults<ColumnMap, ColumnMapTranslatorTranslationDelegate>) this, translationDelegate);
      if (elementMap != columnMap.Element)
        columnMap = new SimpleCollectionColumnMap(columnMap.Type, columnMap.Name, elementMap, columnMap.Keys, columnMap.ForeignKeys);
      return translationDelegate((ColumnMap) columnMap);
    }

    internal override ColumnMap Visit(
      VarRefColumnMap columnMap,
      ColumnMapTranslatorTranslationDelegate translationDelegate)
    {
      return translationDelegate((ColumnMap) columnMap);
    }
  }
}

﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.ColumnMapVisitor`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal abstract class ColumnMapVisitor<TArgType>
  {
    protected void VisitList<TListType>(TListType[] columnMaps, TArgType arg) where TListType : ColumnMap
    {
      foreach (TListType columnMap in columnMaps)
        columnMap.Accept<TArgType>(this, arg);
    }

    protected void VisitEntityIdentity(EntityIdentity entityIdentity, TArgType arg)
    {
      if (entityIdentity is DiscriminatedEntityIdentity entityIdentity1)
        this.VisitEntityIdentity(entityIdentity1, arg);
      else
        this.VisitEntityIdentity((SimpleEntityIdentity) entityIdentity, arg);
    }

    protected virtual void VisitEntityIdentity(
      DiscriminatedEntityIdentity entityIdentity,
      TArgType arg)
    {
      entityIdentity.EntitySetColumnMap.Accept<TArgType>(this, arg);
      foreach (ColumnMap key in entityIdentity.Keys)
        key.Accept<TArgType>(this, arg);
    }

    protected virtual void VisitEntityIdentity(SimpleEntityIdentity entityIdentity, TArgType arg)
    {
      foreach (ColumnMap key in entityIdentity.Keys)
        key.Accept<TArgType>(this, arg);
    }

    internal virtual void Visit(ComplexTypeColumnMap columnMap, TArgType arg)
    {
      columnMap.NullSentinel?.Accept<TArgType>(this, arg);
      foreach (ColumnMap property in columnMap.Properties)
        property.Accept<TArgType>(this, arg);
    }

    internal virtual void Visit(DiscriminatedCollectionColumnMap columnMap, TArgType arg)
    {
      columnMap.Discriminator.Accept<TArgType>(this, arg);
      foreach (ColumnMap foreignKey in columnMap.ForeignKeys)
        foreignKey.Accept<TArgType>(this, arg);
      foreach (ColumnMap key in columnMap.Keys)
        key.Accept<TArgType>(this, arg);
      columnMap.Element.Accept<TArgType>(this, arg);
    }

    internal virtual void Visit(EntityColumnMap columnMap, TArgType arg)
    {
      this.VisitEntityIdentity(columnMap.EntityIdentity, arg);
      foreach (ColumnMap property in columnMap.Properties)
        property.Accept<TArgType>(this, arg);
    }

    internal virtual void Visit(SimplePolymorphicColumnMap columnMap, TArgType arg)
    {
      columnMap.TypeDiscriminator.Accept<TArgType>(this, arg);
      foreach (ColumnMap columnMap1 in columnMap.TypeChoices.Values)
        columnMap1.Accept<TArgType>(this, arg);
      foreach (ColumnMap property in columnMap.Properties)
        property.Accept<TArgType>(this, arg);
    }

    internal virtual void Visit(
      MultipleDiscriminatorPolymorphicColumnMap columnMap,
      TArgType arg)
    {
      foreach (ColumnMap typeDiscriminator in columnMap.TypeDiscriminators)
        typeDiscriminator.Accept<TArgType>(this, arg);
      foreach (ColumnMap columnMap1 in columnMap.TypeChoices.Values)
        columnMap1.Accept<TArgType>(this, arg);
      foreach (ColumnMap property in columnMap.Properties)
        property.Accept<TArgType>(this, arg);
    }

    internal virtual void Visit(RecordColumnMap columnMap, TArgType arg)
    {
      columnMap.NullSentinel?.Accept<TArgType>(this, arg);
      foreach (ColumnMap property in columnMap.Properties)
        property.Accept<TArgType>(this, arg);
    }

    internal virtual void Visit(RefColumnMap columnMap, TArgType arg) => this.VisitEntityIdentity(columnMap.EntityIdentity, arg);

    internal virtual void Visit(ScalarColumnMap columnMap, TArgType arg)
    {
    }

    internal virtual void Visit(SimpleCollectionColumnMap columnMap, TArgType arg)
    {
      foreach (ColumnMap foreignKey in columnMap.ForeignKeys)
        foreignKey.Accept<TArgType>(this, arg);
      foreach (ColumnMap key in columnMap.Keys)
        key.Accept<TArgType>(this, arg);
      columnMap.Element.Accept<TArgType>(this, arg);
    }

    internal virtual void Visit(VarRefColumnMap columnMap, TArgType arg)
    {
    }
  }
}

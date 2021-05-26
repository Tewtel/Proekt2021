// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.Converter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm.Provider;
using System.Data.Entity.Core.SchemaObjectModel;
using System.Globalization;
using System.Linq;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal static class Converter
  {
    internal static readonly FacetDescription ConcurrencyModeFacet;
    internal static readonly FacetDescription StoreGeneratedPatternFacet;
    internal static readonly FacetDescription CollationFacet;

    static Converter()
    {
      EnumType enumType1 = new EnumType("ConcurrencyMode", "Edm", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Int32), false, DataSpace.CSpace);
      foreach (string name in Enum.GetNames(typeof (ConcurrencyMode)))
        enumType1.AddMember(new EnumMember(name, (object) (int) Enum.Parse(typeof (ConcurrencyMode), name, false)));
      EnumType enumType2 = new EnumType("StoreGeneratedPattern", "Edm", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Int32), false, DataSpace.CSpace);
      foreach (string name in Enum.GetNames(typeof (StoreGeneratedPattern)))
        enumType2.AddMember(new EnumMember(name, (object) (int) Enum.Parse(typeof (StoreGeneratedPattern), name, false)));
      Converter.ConcurrencyModeFacet = new FacetDescription("ConcurrencyMode", (EdmType) enumType1, new int?(), new int?(), (object) ConcurrencyMode.None);
      Converter.StoreGeneratedPatternFacet = new FacetDescription("StoreGeneratedPattern", (EdmType) enumType2, new int?(), new int?(), (object) StoreGeneratedPattern.None);
      Converter.CollationFacet = new FacetDescription("Collation", (EdmType) MetadataItem.EdmProviderManifest.GetPrimitiveType(PrimitiveTypeKind.String), new int?(), new int?(), (object) string.Empty);
    }

    internal static IEnumerable<GlobalItem> ConvertSchema(
      Schema somSchema,
      DbProviderManifest providerManifest,
      ItemCollection itemCollection)
    {
      Dictionary<SchemaElement, GlobalItem> newGlobalItems = new Dictionary<SchemaElement, GlobalItem>();
      Converter.ConvertSchema(somSchema, providerManifest, new Converter.ConversionCache(itemCollection), newGlobalItems);
      return (IEnumerable<GlobalItem>) newGlobalItems.Values;
    }

    internal static IEnumerable<GlobalItem> ConvertSchema(
      IList<Schema> somSchemas,
      DbProviderManifest providerManifest,
      ItemCollection itemCollection)
    {
      Dictionary<SchemaElement, GlobalItem> newGlobalItems = new Dictionary<SchemaElement, GlobalItem>();
      Converter.ConversionCache convertedItemCache = new Converter.ConversionCache(itemCollection);
      foreach (Schema somSchema in (IEnumerable<Schema>) somSchemas)
        Converter.ConvertSchema(somSchema, providerManifest, convertedItemCache, newGlobalItems);
      return (IEnumerable<GlobalItem>) newGlobalItems.Values;
    }

    private static void ConvertSchema(
      Schema somSchema,
      DbProviderManifest providerManifest,
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      List<System.Data.Entity.Core.SchemaObjectModel.Function> functionList = new List<System.Data.Entity.Core.SchemaObjectModel.Function>();
      foreach (System.Data.Entity.Core.SchemaObjectModel.SchemaType schemaType in somSchema.SchemaTypes)
      {
        if (Converter.LoadSchemaElement(schemaType, providerManifest, convertedItemCache, newGlobalItems) == null && schemaType is System.Data.Entity.Core.SchemaObjectModel.Function function1)
          functionList.Add(function1);
      }
      foreach (SchemaEntityType element in somSchema.SchemaTypes.OfType<SchemaEntityType>())
        Converter.LoadEntityTypePhase2(element, providerManifest, convertedItemCache, newGlobalItems);
      foreach (System.Data.Entity.Core.SchemaObjectModel.SchemaType element in functionList)
        Converter.LoadSchemaElement(element, providerManifest, convertedItemCache, newGlobalItems);
      if (convertedItemCache.ItemCollection.DataSpace == DataSpace.CSpace)
      {
        ((EdmItemCollection) convertedItemCache.ItemCollection).EdmVersion = somSchema.SchemaVersion;
      }
      else
      {
        if (!(convertedItemCache.ItemCollection is StoreItemCollection itemCollection2))
          return;
        itemCollection2.StoreSchemaVersion = somSchema.SchemaVersion;
      }
    }

    internal static MetadataItem LoadSchemaElement(
      System.Data.Entity.Core.SchemaObjectModel.SchemaType element,
      DbProviderManifest providerManifest,
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      GlobalItem globalItem;
      if (newGlobalItems.TryGetValue((SchemaElement) element, out globalItem))
        return (MetadataItem) globalItem;
      switch (element)
      {
        case System.Data.Entity.Core.SchemaObjectModel.EntityContainer element1:
          return (MetadataItem) Converter.ConvertToEntityContainer(element1, providerManifest, convertedItemCache, newGlobalItems);
        case SchemaEntityType _:
          return (MetadataItem) Converter.ConvertToEntityType((SchemaEntityType) element, providerManifest, convertedItemCache, newGlobalItems);
        case Relationship _:
          return (MetadataItem) Converter.ConvertToAssociationType((Relationship) element, providerManifest, convertedItemCache, newGlobalItems);
        case SchemaComplexType _:
          return (MetadataItem) Converter.ConvertToComplexType((SchemaComplexType) element, providerManifest, convertedItemCache, newGlobalItems);
        case System.Data.Entity.Core.SchemaObjectModel.Function _:
          return (MetadataItem) Converter.ConvertToFunction((System.Data.Entity.Core.SchemaObjectModel.Function) element, providerManifest, convertedItemCache, (EntityContainer) null, newGlobalItems);
        case SchemaEnumType _:
          return (MetadataItem) Converter.ConvertToEnumType((SchemaEnumType) element, newGlobalItems);
        default:
          return (MetadataItem) null;
      }
    }

    private static EntityContainer ConvertToEntityContainer(
      System.Data.Entity.Core.SchemaObjectModel.EntityContainer element,
      DbProviderManifest providerManifest,
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      EntityContainer entityContainer = new EntityContainer(element.Name, Converter.GetDataSpace(providerManifest));
      newGlobalItems.Add((SchemaElement) element, (GlobalItem) entityContainer);
      foreach (EntityContainerEntitySet entitySet in element.EntitySets)
        entityContainer.AddEntitySetBase((EntitySetBase) Converter.ConvertToEntitySet(entitySet, providerManifest, convertedItemCache, newGlobalItems));
      foreach (EntityContainerRelationshipSet relationshipSet in element.RelationshipSets)
        entityContainer.AddEntitySetBase((EntitySetBase) Converter.ConvertToAssociationSet(relationshipSet, providerManifest, convertedItemCache, entityContainer, newGlobalItems));
      foreach (System.Data.Entity.Core.SchemaObjectModel.Function functionImport in element.FunctionImports)
        entityContainer.AddFunctionImport(Converter.ConvertToFunction(functionImport, providerManifest, convertedItemCache, entityContainer, newGlobalItems));
      if (element.Documentation != null)
        entityContainer.Documentation = Converter.ConvertToDocumentation(element.Documentation);
      Converter.AddOtherContent((SchemaElement) element, (MetadataItem) entityContainer);
      return entityContainer;
    }

    private static EntityType ConvertToEntityType(
      SchemaEntityType element,
      DbProviderManifest providerManifest,
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      string[] strArray = (string[]) null;
      if (element.DeclaredKeyProperties.Count != 0)
      {
        strArray = new string[element.DeclaredKeyProperties.Count];
        for (int index = 0; index < strArray.Length; ++index)
          strArray[index] = element.DeclaredKeyProperties[index].Property.Name;
      }
      EdmProperty[] edmPropertyArray = new EdmProperty[element.Properties.Count];
      int num = 0;
      foreach (StructuredProperty property in element.Properties)
        edmPropertyArray[num++] = Converter.ConvertToProperty(property, providerManifest, convertedItemCache, newGlobalItems);
      EntityType entityType = new EntityType(element.Name, element.Namespace, Converter.GetDataSpace(providerManifest), (IEnumerable<string>) strArray, (IEnumerable<EdmMember>) edmPropertyArray);
      if (element.BaseType != null)
        entityType.BaseType = (EdmType) Converter.LoadSchemaElement((System.Data.Entity.Core.SchemaObjectModel.SchemaType) element.BaseType, providerManifest, convertedItemCache, newGlobalItems);
      entityType.Abstract = element.IsAbstract;
      if (element.Documentation != null)
        entityType.Documentation = Converter.ConvertToDocumentation(element.Documentation);
      Converter.AddOtherContent((SchemaElement) element, (MetadataItem) entityType);
      newGlobalItems.Add((SchemaElement) element, (GlobalItem) entityType);
      return entityType;
    }

    private static void LoadEntityTypePhase2(
      SchemaEntityType element,
      DbProviderManifest providerManifest,
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      EntityType newGlobalItem = (EntityType) newGlobalItems[(SchemaElement) element];
      foreach (System.Data.Entity.Core.SchemaObjectModel.NavigationProperty navigationProperty in element.NavigationProperties)
        newGlobalItem.AddMember((EdmMember) Converter.ConvertToNavigationProperty(newGlobalItem, navigationProperty, providerManifest, convertedItemCache, newGlobalItems));
    }

    private static ComplexType ConvertToComplexType(
      SchemaComplexType element,
      DbProviderManifest providerManifest,
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      ComplexType complexType = new ComplexType(element.Name, element.Namespace, Converter.GetDataSpace(providerManifest));
      newGlobalItems.Add((SchemaElement) element, (GlobalItem) complexType);
      foreach (StructuredProperty property in element.Properties)
        complexType.AddMember((EdmMember) Converter.ConvertToProperty(property, providerManifest, convertedItemCache, newGlobalItems));
      complexType.Abstract = element.IsAbstract;
      if (element.BaseType != null)
        complexType.BaseType = (EdmType) Converter.LoadSchemaElement((System.Data.Entity.Core.SchemaObjectModel.SchemaType) element.BaseType, providerManifest, convertedItemCache, newGlobalItems);
      if (element.Documentation != null)
        complexType.Documentation = Converter.ConvertToDocumentation(element.Documentation);
      Converter.AddOtherContent((SchemaElement) element, (MetadataItem) complexType);
      return complexType;
    }

    private static AssociationType ConvertToAssociationType(
      Relationship element,
      DbProviderManifest providerManifest,
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      AssociationType associationType = new AssociationType(element.Name, element.Namespace, element.IsForeignKey, Converter.GetDataSpace(providerManifest));
      newGlobalItems.Add((SchemaElement) element, (GlobalItem) associationType);
      foreach (RelationshipEnd end in (IEnumerable<IRelationshipEnd>) element.Ends)
      {
        EntityType endMemberType = (EntityType) Converter.LoadSchemaElement((System.Data.Entity.Core.SchemaObjectModel.SchemaType) end.Type, providerManifest, convertedItemCache, newGlobalItems);
        AssociationEndMember associationEndMember = Converter.InitializeAssociationEndMember(associationType, (IRelationshipEnd) end, endMemberType);
        Converter.AddOtherContent((SchemaElement) end, (MetadataItem) associationEndMember);
        foreach (OnOperation operation in (IEnumerable<OnOperation>) end.Operations)
        {
          if (operation.Operation == Operation.Delete)
          {
            OperationAction operationAction = OperationAction.None;
            switch (operation.Action)
            {
              case System.Data.Entity.Core.SchemaObjectModel.Action.None:
                operationAction = OperationAction.None;
                break;
              case System.Data.Entity.Core.SchemaObjectModel.Action.Cascade:
                operationAction = OperationAction.Cascade;
                break;
            }
            associationEndMember.DeleteBehavior = operationAction;
          }
        }
        if (end.Documentation != null)
          associationEndMember.Documentation = Converter.ConvertToDocumentation(end.Documentation);
      }
      for (int index = 0; index < element.Constraints.Count; ++index)
      {
        System.Data.Entity.Core.SchemaObjectModel.ReferentialConstraint constraint = element.Constraints[index];
        AssociationEndMember member1 = (AssociationEndMember) associationType.Members[constraint.PrincipalRole.Name];
        AssociationEndMember member2 = (AssociationEndMember) associationType.Members[constraint.DependentRole.Name];
        EntityTypeBase elementType1 = ((RefType) member1.TypeUsage.EdmType).ElementType;
        EntityTypeBase elementType2 = ((RefType) member2.TypeUsage.EdmType).ElementType;
        ReferentialConstraint referentialConstraint = new ReferentialConstraint((RelationshipEndMember) member1, (RelationshipEndMember) member2, (IEnumerable<EdmProperty>) Converter.GetProperties(elementType1, constraint.PrincipalRole.RoleProperties), (IEnumerable<EdmProperty>) Converter.GetProperties(elementType2, constraint.DependentRole.RoleProperties));
        if (constraint.Documentation != null)
          referentialConstraint.Documentation = Converter.ConvertToDocumentation(constraint.Documentation);
        if (constraint.PrincipalRole.Documentation != null)
          referentialConstraint.FromRole.Documentation = Converter.ConvertToDocumentation(constraint.PrincipalRole.Documentation);
        if (constraint.DependentRole.Documentation != null)
          referentialConstraint.ToRole.Documentation = Converter.ConvertToDocumentation(constraint.DependentRole.Documentation);
        associationType.AddReferentialConstraint(referentialConstraint);
        Converter.AddOtherContent((SchemaElement) element.Constraints[index], (MetadataItem) referentialConstraint);
      }
      if (element.Documentation != null)
        associationType.Documentation = Converter.ConvertToDocumentation(element.Documentation);
      Converter.AddOtherContent((SchemaElement) element, (MetadataItem) associationType);
      return associationType;
    }

    private static AssociationEndMember InitializeAssociationEndMember(
      AssociationType associationType,
      IRelationshipEnd end,
      EntityType endMemberType)
    {
      EdmMember edmMember;
      AssociationEndMember associationEndMember;
      if (!associationType.Members.TryGetValue(end.Name, false, out edmMember))
      {
        associationEndMember = new AssociationEndMember(end.Name, endMemberType.GetReferenceType(), end.Multiplicity.Value);
        associationType.AddKeyMember((EdmMember) associationEndMember);
      }
      else
        associationEndMember = (AssociationEndMember) edmMember;
      if (end is RelationshipEnd relationshipEnd && relationshipEnd.Documentation != null)
        associationEndMember.Documentation = Converter.ConvertToDocumentation(relationshipEnd.Documentation);
      return associationEndMember;
    }

    private static EdmProperty[] GetProperties(
      EntityTypeBase entityType,
      IList<PropertyRefElement> properties)
    {
      EdmProperty[] edmPropertyArray = new EdmProperty[properties.Count];
      for (int index = 0; index < properties.Count; ++index)
        edmPropertyArray[index] = (EdmProperty) entityType.Members[properties[index].Name];
      return edmPropertyArray;
    }

    private static void AddOtherContent(SchemaElement element, MetadataItem item)
    {
      if (element.OtherContent.Count <= 0)
        return;
      item.AddMetadataProperties((IEnumerable<MetadataProperty>) element.OtherContent);
    }

    private static EntitySet ConvertToEntitySet(
      EntityContainerEntitySet set,
      DbProviderManifest providerManifest,
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      EntitySet entitySet = new EntitySet(set.Name, set.DbSchema, set.Table, set.DefiningQuery, (EntityType) Converter.LoadSchemaElement((System.Data.Entity.Core.SchemaObjectModel.SchemaType) set.EntityType, providerManifest, convertedItemCache, newGlobalItems));
      if (set.Documentation != null)
        entitySet.Documentation = Converter.ConvertToDocumentation(set.Documentation);
      Converter.AddOtherContent((SchemaElement) set, (MetadataItem) entitySet);
      return entitySet;
    }

    private static EntitySet GetEntitySet(
      EntityContainerEntitySet set,
      EntityContainer container)
    {
      return container.GetEntitySetByName(set.Name, false);
    }

    private static AssociationSet ConvertToAssociationSet(
      EntityContainerRelationshipSet relationshipSet,
      DbProviderManifest providerManifest,
      Converter.ConversionCache convertedItemCache,
      EntityContainer container,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      AssociationType associationType = (AssociationType) Converter.LoadSchemaElement((System.Data.Entity.Core.SchemaObjectModel.SchemaType) relationshipSet.Relationship, providerManifest, convertedItemCache, newGlobalItems);
      AssociationSet parentSet = new AssociationSet(relationshipSet.Name, associationType);
      foreach (EntityContainerRelationshipSetEnd end in relationshipSet.Ends)
      {
        AssociationEndMember member = (AssociationEndMember) associationType.Members[end.Name];
        AssociationSetEnd associationSetEnd = new AssociationSetEnd(Converter.GetEntitySet(end.EntitySet, container), parentSet, member);
        Converter.AddOtherContent((SchemaElement) end, (MetadataItem) associationSetEnd);
        parentSet.AddAssociationSetEnd(associationSetEnd);
        if (end.Documentation != null)
          associationSetEnd.Documentation = Converter.ConvertToDocumentation(end.Documentation);
      }
      if (relationshipSet.Documentation != null)
        parentSet.Documentation = Converter.ConvertToDocumentation(relationshipSet.Documentation);
      Converter.AddOtherContent((SchemaElement) relationshipSet, (MetadataItem) parentSet);
      return parentSet;
    }

    private static EdmProperty ConvertToProperty(
      StructuredProperty somProperty,
      DbProviderManifest providerManifest,
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      TypeUsage typeUsage;
      if (somProperty.Type is ScalarType type && somProperty.Schema.DataModel != SchemaDataModelOption.EntityDataModel)
      {
        typeUsage = somProperty.TypeUsage;
        Converter.UpdateSentinelValuesInFacets(ref typeUsage);
      }
      else
      {
        EdmType edmType = type == null ? (EdmType) Converter.LoadSchemaElement(somProperty.Type, providerManifest, convertedItemCache, newGlobalItems) : (EdmType) convertedItemCache.ItemCollection.GetItem<PrimitiveType>(somProperty.TypeUsage.EdmType.FullName);
        if (somProperty.CollectionKind != CollectionKind.None)
        {
          typeUsage = TypeUsage.Create((EdmType) new CollectionType(edmType));
        }
        else
        {
          SchemaEnumType schemaEnumType = type == null ? somProperty.Type as SchemaEnumType : (SchemaEnumType) null;
          typeUsage = TypeUsage.Create(edmType);
          if (schemaEnumType != null)
            somProperty.EnsureEnumTypeFacets(convertedItemCache, newGlobalItems);
          if (somProperty.TypeUsage != null)
            Converter.ApplyTypePropertyFacets(somProperty.TypeUsage, ref typeUsage);
        }
      }
      Converter.PopulateGeneralFacets(somProperty, ref typeUsage);
      EdmProperty edmProperty = new EdmProperty(somProperty.Name, typeUsage);
      if (somProperty.Documentation != null)
        edmProperty.Documentation = Converter.ConvertToDocumentation(somProperty.Documentation);
      Converter.AddOtherContent((SchemaElement) somProperty, (MetadataItem) edmProperty);
      return edmProperty;
    }

    private static NavigationProperty ConvertToNavigationProperty(
      EntityType declaringEntityType,
      System.Data.Entity.Core.SchemaObjectModel.NavigationProperty somNavigationProperty,
      DbProviderManifest providerManifest,
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      EntityType endMemberType = (EntityType) Converter.LoadSchemaElement(somNavigationProperty.Type, providerManifest, convertedItemCache, newGlobalItems);
      AssociationType associationType = (AssociationType) Converter.LoadSchemaElement((System.Data.Entity.Core.SchemaObjectModel.SchemaType) somNavigationProperty.Relationship, providerManifest, convertedItemCache, newGlobalItems);
      IRelationshipEnd end = (IRelationshipEnd) null;
      somNavigationProperty.Relationship.TryGetEnd(somNavigationProperty.ToEnd.Name, out end);
      RelationshipMultiplicity? multiplicity1 = end.Multiplicity;
      RelationshipMultiplicity relationshipMultiplicity1 = RelationshipMultiplicity.Many;
      EdmType edmType = !(multiplicity1.GetValueOrDefault() == relationshipMultiplicity1 & multiplicity1.HasValue) ? (EdmType) endMemberType : (EdmType) endMemberType.GetCollectionType();
      RelationshipMultiplicity? multiplicity2 = end.Multiplicity;
      RelationshipMultiplicity relationshipMultiplicity2 = RelationshipMultiplicity.One;
      TypeUsage typeUsage;
      if (multiplicity2.GetValueOrDefault() == relationshipMultiplicity2 & multiplicity2.HasValue)
        typeUsage = TypeUsage.Create(edmType, new FacetValues()
        {
          Nullable = (FacetValueContainer<bool?>) new bool?(false)
        });
      else
        typeUsage = TypeUsage.Create(edmType);
      Converter.InitializeAssociationEndMember(associationType, somNavigationProperty.ToEnd, endMemberType);
      Converter.InitializeAssociationEndMember(associationType, somNavigationProperty.FromEnd, declaringEntityType);
      NavigationProperty navigationProperty = new NavigationProperty(somNavigationProperty.Name, typeUsage);
      navigationProperty.RelationshipType = (RelationshipType) associationType;
      navigationProperty.ToEndMember = (RelationshipEndMember) associationType.Members[somNavigationProperty.ToEnd.Name];
      navigationProperty.FromEndMember = (RelationshipEndMember) associationType.Members[somNavigationProperty.FromEnd.Name];
      if (somNavigationProperty.Documentation != null)
        navigationProperty.Documentation = Converter.ConvertToDocumentation(somNavigationProperty.Documentation);
      Converter.AddOtherContent((SchemaElement) somNavigationProperty, (MetadataItem) navigationProperty);
      return navigationProperty;
    }

    private static EdmFunction ConvertToFunction(
      System.Data.Entity.Core.SchemaObjectModel.Function somFunction,
      DbProviderManifest providerManifest,
      Converter.ConversionCache convertedItemCache,
      EntityContainer functionImportEntityContainer,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      GlobalItem globalItem = (GlobalItem) null;
      if (!somFunction.IsFunctionImport && newGlobalItems.TryGetValue((SchemaElement) somFunction, out globalItem))
        return (EdmFunction) globalItem;
      bool areConvertingForProviderManifest = somFunction.Schema.DataModel == SchemaDataModelOption.ProviderManifestModel;
      List<FunctionParameter> functionParameterList1 = new List<FunctionParameter>();
      if (somFunction.ReturnTypeList != null)
      {
        int num = 0;
        foreach (ReturnType returnType in (IEnumerable<ReturnType>) somFunction.ReturnTypeList)
        {
          TypeUsage functionTypeUsage = Converter.GetFunctionTypeUsage(somFunction is ModelFunction, somFunction, (FacetEnabledSchemaElement) returnType, providerManifest, areConvertingForProviderManifest, returnType.Type, returnType.CollectionKind, returnType.IsRefType, convertedItemCache, newGlobalItems);
          if (functionTypeUsage == null)
            return (EdmFunction) null;
          string str = num == 0 ? string.Empty : num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          ++num;
          FunctionParameter functionParameter = new FunctionParameter("ReturnType" + str, functionTypeUsage, ParameterMode.ReturnValue);
          Converter.AddOtherContent((SchemaElement) returnType, (MetadataItem) functionParameter);
          functionParameterList1.Add(functionParameter);
        }
      }
      else if (somFunction.Type != null)
      {
        TypeUsage functionTypeUsage = Converter.GetFunctionTypeUsage(somFunction is ModelFunction, somFunction, (FacetEnabledSchemaElement) null, providerManifest, areConvertingForProviderManifest, somFunction.Type, somFunction.CollectionKind, somFunction.IsReturnAttributeReftype, convertedItemCache, newGlobalItems);
        if (functionTypeUsage == null)
          return (EdmFunction) null;
        functionParameterList1.Add(new FunctionParameter("ReturnType", functionTypeUsage, ParameterMode.ReturnValue));
      }
      EntitySet[] entitySetArray = (EntitySet[]) null;
      string name;
      if (somFunction.IsFunctionImport)
      {
        FunctionImportElement functionImportElement = (FunctionImportElement) somFunction;
        name = functionImportElement.Container.Name;
        if (functionImportElement.EntitySet != null)
        {
          EntityContainer container = functionImportEntityContainer;
          entitySetArray = new EntitySet[1]
          {
            Converter.GetEntitySet(functionImportElement.EntitySet, container)
          };
        }
        else if (functionImportElement.ReturnTypeList != null)
          entitySetArray = functionImportElement.ReturnTypeList.Select<ReturnType, EntitySet>((Func<ReturnType, EntitySet>) (returnType => returnType.EntitySet == null ? (EntitySet) null : Converter.GetEntitySet(returnType.EntitySet, functionImportEntityContainer))).ToArray<EntitySet>();
      }
      else
        name = somFunction.Namespace;
      List<FunctionParameter> functionParameterList2 = new List<FunctionParameter>();
      foreach (Parameter parameter in somFunction.Parameters)
      {
        TypeUsage functionTypeUsage = Converter.GetFunctionTypeUsage(somFunction is ModelFunction, somFunction, (FacetEnabledSchemaElement) parameter, providerManifest, areConvertingForProviderManifest, parameter.Type, parameter.CollectionKind, parameter.IsRefType, convertedItemCache, newGlobalItems);
        if (functionTypeUsage == null)
          return (EdmFunction) null;
        FunctionParameter functionParameter = new FunctionParameter(parameter.Name, functionTypeUsage, Converter.GetParameterMode(parameter.ParameterDirection));
        Converter.AddOtherContent((SchemaElement) parameter, (MetadataItem) functionParameter);
        if (parameter.Documentation != null)
          functionParameter.Documentation = Converter.ConvertToDocumentation(parameter.Documentation);
        functionParameterList2.Add(functionParameter);
      }
      EdmFunction edmFunction = new EdmFunction(somFunction.Name, name, Converter.GetDataSpace(providerManifest), new EdmFunctionPayload()
      {
        Schema = somFunction.DbSchema,
        StoreFunctionName = somFunction.StoreFunctionName,
        CommandText = somFunction.CommandText,
        EntitySets = (IList<EntitySet>) entitySetArray,
        IsAggregate = new bool?(somFunction.IsAggregate),
        IsBuiltIn = new bool?(somFunction.IsBuiltIn),
        IsNiladic = new bool?(somFunction.IsNiladicFunction),
        IsComposable = new bool?(somFunction.IsComposable),
        IsFromProviderManifest = new bool?(areConvertingForProviderManifest),
        IsFunctionImport = new bool?(somFunction.IsFunctionImport),
        ReturnParameters = (IList<FunctionParameter>) functionParameterList1.ToArray(),
        Parameters = (IList<FunctionParameter>) functionParameterList2.ToArray(),
        ParameterTypeSemantics = new ParameterTypeSemantics?(somFunction.ParameterTypeSemantics)
      });
      if (!somFunction.IsFunctionImport)
        newGlobalItems.Add((SchemaElement) somFunction, (GlobalItem) edmFunction);
      if (somFunction.Documentation != null)
        edmFunction.Documentation = Converter.ConvertToDocumentation(somFunction.Documentation);
      Converter.AddOtherContent((SchemaElement) somFunction, (MetadataItem) edmFunction);
      return edmFunction;
    }

    private static EnumType ConvertToEnumType(
      SchemaEnumType somEnumType,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      ScalarType underlyingType = (ScalarType) somEnumType.UnderlyingType;
      EnumType enumType = new EnumType(somEnumType.Name, somEnumType.Namespace, underlyingType.Type, somEnumType.IsFlags, DataSpace.CSpace);
      Type clrEquivalentType = underlyingType.Type.ClrEquivalentType;
      foreach (SchemaEnumMember enumMember1 in somEnumType.EnumMembers)
      {
        EnumMember enumMember2 = new EnumMember(enumMember1.Name, Convert.ChangeType((object) enumMember1.Value, clrEquivalentType, (IFormatProvider) CultureInfo.InvariantCulture));
        if (enumMember1.Documentation != null)
          enumMember2.Documentation = Converter.ConvertToDocumentation(enumMember1.Documentation);
        Converter.AddOtherContent((SchemaElement) enumMember1, (MetadataItem) enumMember2);
        enumType.AddMember(enumMember2);
      }
      if (somEnumType.Documentation != null)
        enumType.Documentation = Converter.ConvertToDocumentation(somEnumType.Documentation);
      Converter.AddOtherContent((SchemaElement) somEnumType, (MetadataItem) enumType);
      newGlobalItems.Add((SchemaElement) somEnumType, (GlobalItem) enumType);
      return enumType;
    }

    private static Documentation ConvertToDocumentation(DocumentationElement element) => element.MetadataDocumentation;

    private static TypeUsage GetFunctionTypeUsage(
      bool isModelFunction,
      System.Data.Entity.Core.SchemaObjectModel.Function somFunction,
      FacetEnabledSchemaElement somParameter,
      DbProviderManifest providerManifest,
      bool areConvertingForProviderManifest,
      System.Data.Entity.Core.SchemaObjectModel.SchemaType type,
      CollectionKind collectionKind,
      bool isRefType,
      Converter.ConversionCache convertedItemCache,
      Dictionary<SchemaElement, GlobalItem> newGlobalItems)
    {
      if (somParameter != null & areConvertingForProviderManifest && somParameter.HasUserDefinedFacets)
        return somParameter.TypeUsage;
      if (type == null)
      {
        if (isModelFunction && somParameter != null && somParameter is Parameter)
        {
          ((Parameter) somParameter).ResolveNestedTypeNames(convertedItemCache, newGlobalItems);
          return somParameter.TypeUsage;
        }
        if (somParameter == null || !(somParameter is ReturnType))
          return (TypeUsage) null;
        ((ReturnType) somParameter).ResolveNestedTypeNames(convertedItemCache, newGlobalItems);
        return somParameter.TypeUsage;
      }
      EdmType edmType;
      if (!areConvertingForProviderManifest)
      {
        if (type is ScalarType scalarType2)
        {
          if (isModelFunction && somParameter != null)
          {
            if (somParameter.TypeUsage == null)
              somParameter.ValidateAndSetTypeUsage(scalarType2);
            return somParameter.TypeUsage;
          }
          if (isModelFunction)
          {
            ModelFunction modelFunction = somFunction as ModelFunction;
            if (modelFunction.TypeUsage == null)
              modelFunction.ValidateAndSetTypeUsage(scalarType2);
            return modelFunction.TypeUsage;
          }
          if (somParameter != null && somParameter.HasUserDefinedFacets && somFunction.Schema.DataModel == SchemaDataModelOption.ProviderDataModel)
          {
            somParameter.ValidateAndSetTypeUsage(scalarType2);
            return somParameter.TypeUsage;
          }
          edmType = (EdmType) Converter.GetPrimitiveType(scalarType2, providerManifest);
        }
        else
        {
          edmType = (EdmType) Converter.LoadSchemaElement(type, providerManifest, convertedItemCache, newGlobalItems);
          if (isModelFunction && type is SchemaEnumType)
          {
            if (somParameter != null)
            {
              somParameter.ValidateAndSetTypeUsage(edmType);
              return somParameter.TypeUsage;
            }
            if (somFunction != null)
            {
              ModelFunction modelFunction = (ModelFunction) somFunction;
              modelFunction.ValidateAndSetTypeUsage(edmType);
              return modelFunction.TypeUsage;
            }
          }
        }
      }
      else
        edmType = !(type is TypeElement) ? (EdmType) (type as ScalarType).Type : (EdmType) (type as TypeElement).PrimitiveType;
      TypeUsage typeUsage;
      if (collectionKind != CollectionKind.None)
      {
        typeUsage = convertedItemCache.GetCollectionTypeUsageWithNullFacets(edmType);
      }
      else
      {
        EntityType entityType = edmType as EntityType;
        typeUsage = !(entityType != null & isRefType) ? convertedItemCache.GetTypeUsageWithNullFacets(edmType) : TypeUsage.Create((EdmType) new RefType(entityType));
      }
      return typeUsage;
    }

    private static ParameterMode GetParameterMode(ParameterDirection parameterDirection)
    {
      switch (parameterDirection)
      {
        case ParameterDirection.Input:
          return ParameterMode.In;
        case ParameterDirection.Output:
          return ParameterMode.Out;
        default:
          return ParameterMode.InOut;
      }
    }

    private static void ApplyTypePropertyFacets(TypeUsage sourceType, ref TypeUsage targetType)
    {
      Dictionary<string, Facet> dictionary = targetType.Facets.ToDictionary<Facet, string>((Func<Facet, string>) (f => f.Name));
      bool flag = false;
      foreach (Facet facet1 in sourceType.Facets)
      {
        Facet facet2;
        if (dictionary.TryGetValue(facet1.Name, out facet2))
        {
          if (!facet2.Description.IsConstant)
          {
            flag = true;
            dictionary[facet2.Name] = Facet.Create(facet2.Description, facet1.Value);
          }
        }
        else
        {
          flag = true;
          dictionary.Add(facet1.Name, facet1);
        }
      }
      if (!flag)
        return;
      targetType = TypeUsage.Create(targetType.EdmType, (IEnumerable<Facet>) dictionary.Values);
    }

    private static void PopulateGeneralFacets(
      StructuredProperty somProperty,
      ref TypeUsage propertyTypeUsage)
    {
      bool flag = false;
      Dictionary<string, Facet> dictionary = propertyTypeUsage.Facets.ToDictionary<Facet, string>((Func<Facet, string>) (f => f.Name));
      if (!somProperty.Nullable)
      {
        dictionary["Nullable"] = Facet.Create(MetadataItem.NullableFacetDescription, (object) false);
        flag = true;
      }
      if (somProperty.Default != null)
      {
        dictionary["DefaultValue"] = Facet.Create(MetadataItem.DefaultValueFacetDescription, somProperty.DefaultAsObject);
        flag = true;
      }
      if (somProperty.Schema.SchemaVersion == 1.1)
      {
        Facet facet = Facet.Create(MetadataItem.CollectionKindFacetDescription, (object) somProperty.CollectionKind);
        dictionary.Add(facet.Name, facet);
        flag = true;
      }
      if (!flag)
        return;
      propertyTypeUsage = TypeUsage.Create(propertyTypeUsage.EdmType, (IEnumerable<Facet>) dictionary.Values);
    }

    private static DataSpace GetDataSpace(DbProviderManifest providerManifest) => providerManifest is EdmProviderManifest ? DataSpace.CSpace : DataSpace.SSpace;

    private static PrimitiveType GetPrimitiveType(
      ScalarType scalarType,
      DbProviderManifest providerManifest)
    {
      PrimitiveType primitiveType = (PrimitiveType) null;
      string name = scalarType.Name;
      foreach (PrimitiveType storeType in providerManifest.GetStoreTypes())
      {
        if (storeType.Name == name)
        {
          primitiveType = storeType;
          break;
        }
      }
      return primitiveType;
    }

    private static void UpdateSentinelValuesInFacets(ref TypeUsage typeUsage)
    {
      PrimitiveType edmType = (PrimitiveType) typeUsage.EdmType;
      if (edmType.PrimitiveTypeKind != PrimitiveTypeKind.String && edmType.PrimitiveTypeKind != PrimitiveTypeKind.Binary || !Helper.IsUnboundedFacetValue(typeUsage.Facets["MaxLength"]))
        return;
      typeUsage = typeUsage.ShallowCopy(new FacetValues()
      {
        MaxLength = (FacetValueContainer<int?>) Helper.GetFacet((IEnumerable<FacetDescription>) edmType.FacetDescriptions, "MaxLength").MaxValue
      });
    }

    internal class ConversionCache
    {
      internal readonly ItemCollection ItemCollection;
      private readonly Dictionary<EdmType, TypeUsage> _nullFacetsTypeUsage;
      private readonly Dictionary<EdmType, TypeUsage> _nullFacetsCollectionTypeUsage;

      internal ConversionCache(ItemCollection itemCollection)
      {
        this.ItemCollection = itemCollection;
        this._nullFacetsTypeUsage = new Dictionary<EdmType, TypeUsage>();
        this._nullFacetsCollectionTypeUsage = new Dictionary<EdmType, TypeUsage>();
      }

      internal TypeUsage GetTypeUsageWithNullFacets(EdmType edmType)
      {
        TypeUsage typeUsage1;
        if (this._nullFacetsTypeUsage.TryGetValue(edmType, out typeUsage1))
          return typeUsage1;
        TypeUsage typeUsage2 = TypeUsage.Create(edmType, FacetValues.NullFacetValues);
        this._nullFacetsTypeUsage.Add(edmType, typeUsage2);
        return typeUsage2;
      }

      internal TypeUsage GetCollectionTypeUsageWithNullFacets(EdmType edmType)
      {
        TypeUsage typeUsage1;
        if (this._nullFacetsCollectionTypeUsage.TryGetValue(edmType, out typeUsage1))
          return typeUsage1;
        TypeUsage typeUsage2 = TypeUsage.Create((EdmType) new CollectionType(this.GetTypeUsageWithNullFacets(edmType)), FacetValues.NullFacetValues);
        this._nullFacetsCollectionTypeUsage.Add(edmType, typeUsage2);
        return typeUsage2;
      }
    }
  }
}

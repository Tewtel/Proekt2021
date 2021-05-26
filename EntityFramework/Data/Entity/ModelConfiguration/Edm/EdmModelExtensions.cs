// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.EdmModelExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Edm.Services;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace System.Data.Entity.ModelConfiguration.Edm
{
  internal static class EdmModelExtensions
  {
    public const string DefaultSchema = "dbo";
    public const string DefaultModelNamespace = "CodeFirstNamespace";
    public const string DefaultStoreNamespace = "CodeFirstDatabaseSchema";

    public static System.Data.Entity.Core.Metadata.Edm.EntityType AddTable(
      this EdmModel database,
      string name)
    {
      string str = ((IEnumerable<INamedDataModelItem>) database.EntityTypes).UniquifyName(name);
      System.Data.Entity.Core.Metadata.Edm.EntityType elementType = new System.Data.Entity.Core.Metadata.Edm.EntityType(str, "CodeFirstDatabaseSchema", DataSpace.SSpace);
      database.AddItem(elementType);
      database.AddEntitySet(elementType.Name, elementType, str);
      return elementType;
    }

    public static System.Data.Entity.Core.Metadata.Edm.EntityType AddTable(
      this EdmModel database,
      string name,
      System.Data.Entity.Core.Metadata.Edm.EntityType pkSource)
    {
      System.Data.Entity.Core.Metadata.Edm.EntityType entityType = database.AddTable(name);
      foreach (EdmProperty keyProperty in pkSource.KeyProperties)
        entityType.AddKeyMember((EdmMember) keyProperty.Clone());
      return entityType;
    }

    public static EdmFunction AddFunction(
      this EdmModel database,
      string name,
      EdmFunctionPayload functionPayload)
    {
      EdmFunction edmFunction = new EdmFunction(((IEnumerable<INamedDataModelItem>) database.Functions).UniquifyName(name), "CodeFirstDatabaseSchema", DataSpace.SSpace, functionPayload);
      database.AddItem(edmFunction);
      return edmFunction;
    }

    public static System.Data.Entity.Core.Metadata.Edm.EntityType FindTableByName(
      this EdmModel database,
      DatabaseName tableName)
    {
      if (!(database.EntityTypes is IList<System.Data.Entity.Core.Metadata.Edm.EntityType> entityTypeList1))
        entityTypeList1 = (IList<System.Data.Entity.Core.Metadata.Edm.EntityType>) database.EntityTypes.ToList<System.Data.Entity.Core.Metadata.Edm.EntityType>();
      IList<System.Data.Entity.Core.Metadata.Edm.EntityType> entityTypeList2 = entityTypeList1;
      for (int index = 0; index < entityTypeList2.Count; ++index)
      {
        System.Data.Entity.Core.Metadata.Edm.EntityType table = entityTypeList2[index];
        DatabaseName tableName1 = table.GetTableName();
        if ((tableName1 != null ? (tableName1.Equals(tableName) ? 1 : 0) : (!string.Equals(table.Name, tableName.Name, StringComparison.Ordinal) ? 0 : (tableName.Schema == null ? 1 : 0))) != 0)
          return table;
      }
      return (System.Data.Entity.Core.Metadata.Edm.EntityType) null;
    }

    public static bool HasCascadeDeletePath(
      this EdmModel model,
      System.Data.Entity.Core.Metadata.Edm.EntityType sourceEntityType,
      System.Data.Entity.Core.Metadata.Edm.EntityType targetEntityType)
    {
      return model.AssociationTypes.SelectMany((Func<AssociationType, IEnumerable<AssociationEndMember>>) (a => a.Members.Cast<AssociationEndMember>()), (a, ae) => new
      {
        a = a,
        ae = ae
      }).Where(_param1 => _param1.ae.GetEntityType() == sourceEntityType && _param1.ae.DeleteBehavior == OperationAction.Cascade).Select(_param1 => _param1.a.GetOtherEnd(_param1.ae).GetEntityType()).Any<System.Data.Entity.Core.Metadata.Edm.EntityType>((Func<System.Data.Entity.Core.Metadata.Edm.EntityType, bool>) (et => et == targetEntityType || model.HasCascadeDeletePath(et, targetEntityType)));
    }

    public static IEnumerable<Type> GetClrTypes(this EdmModel model) => model.EntityTypes.Select<System.Data.Entity.Core.Metadata.Edm.EntityType, Type>((Func<System.Data.Entity.Core.Metadata.Edm.EntityType, Type>) (e => EntityTypeExtensions.GetClrType(e))).Union<Type>(model.ComplexTypes.Select<ComplexType, Type>((Func<ComplexType, Type>) (ct => ComplexTypeExtensions.GetClrType(ct))));

    public static NavigationProperty GetNavigationProperty(
      this EdmModel model,
      PropertyInfo propertyInfo)
    {
      if (!(model.EntityTypes is IList<System.Data.Entity.Core.Metadata.Edm.EntityType> entityTypeList1))
        entityTypeList1 = (IList<System.Data.Entity.Core.Metadata.Edm.EntityType>) model.EntityTypes.ToList<System.Data.Entity.Core.Metadata.Edm.EntityType>();
      IList<System.Data.Entity.Core.Metadata.Edm.EntityType> entityTypeList2 = entityTypeList1;
      for (int index = 0; index < entityTypeList2.Count; ++index)
      {
        NavigationProperty navigationProperty = entityTypeList2[index].GetNavigationProperty(propertyInfo);
        if (navigationProperty != null)
          return navigationProperty;
      }
      return (NavigationProperty) null;
    }

    public static void ValidateAndSerializeCsdl(this EdmModel model, XmlWriter writer)
    {
      List<DataModelErrorEventArgs> csdlErrors = model.SerializeAndGetCsdlErrors(writer);
      if (csdlErrors.Count > 0)
        throw new ModelValidationException((IEnumerable<DataModelErrorEventArgs>) csdlErrors);
    }

    private static List<DataModelErrorEventArgs> SerializeAndGetCsdlErrors(
      this EdmModel model,
      XmlWriter writer)
    {
      List<DataModelErrorEventArgs> validationErrors = new List<DataModelErrorEventArgs>();
      CsdlSerializer csdlSerializer = new CsdlSerializer();
      csdlSerializer.OnError += (EventHandler<DataModelErrorEventArgs>) ((s, e) => validationErrors.Add(e));
      csdlSerializer.Serialize(model, writer);
      return validationErrors;
    }

    public static DbDatabaseMapping GenerateDatabaseMapping(
      this EdmModel model,
      DbProviderInfo providerInfo,
      DbProviderManifest providerManifest)
    {
      return new DatabaseMappingGenerator(providerInfo, providerManifest).Generate(model);
    }

    public static EdmType GetStructuralOrEnumType(this EdmModel model, string name) => model.GetStructuralType(name) ?? (EdmType) model.GetEnumType(name);

    public static EdmType GetStructuralType(this EdmModel model, string name) => (EdmType) model.GetEntityType(name) ?? (EdmType) model.GetComplexType(name);

    public static System.Data.Entity.Core.Metadata.Edm.EntityType GetEntityType(
      this EdmModel model,
      string name)
    {
      return model.EntityTypes.SingleOrDefault<System.Data.Entity.Core.Metadata.Edm.EntityType>((Func<System.Data.Entity.Core.Metadata.Edm.EntityType, bool>) (e => e.Name == name));
    }

    public static System.Data.Entity.Core.Metadata.Edm.EntityType GetEntityType(
      this EdmModel model,
      Type clrType)
    {
      if (!(model.EntityTypes is IList<System.Data.Entity.Core.Metadata.Edm.EntityType> entityTypeList1))
        entityTypeList1 = (IList<System.Data.Entity.Core.Metadata.Edm.EntityType>) model.EntityTypes.ToList<System.Data.Entity.Core.Metadata.Edm.EntityType>();
      IList<System.Data.Entity.Core.Metadata.Edm.EntityType> entityTypeList2 = entityTypeList1;
      for (int index = 0; index < entityTypeList2.Count; ++index)
      {
        System.Data.Entity.Core.Metadata.Edm.EntityType entityType = entityTypeList2[index];
        if (EntityTypeExtensions.GetClrType(entityType) == clrType)
          return entityType;
      }
      return (System.Data.Entity.Core.Metadata.Edm.EntityType) null;
    }

    public static ComplexType GetComplexType(this EdmModel model, string name) => model.ComplexTypes.SingleOrDefault<ComplexType>((Func<ComplexType, bool>) (e => e.Name == name));

    public static ComplexType GetComplexType(this EdmModel model, Type clrType) => model.ComplexTypes.SingleOrDefault<ComplexType>((Func<ComplexType, bool>) (e => ComplexTypeExtensions.GetClrType(e) == clrType));

    public static EnumType GetEnumType(this EdmModel model, string name) => model.EnumTypes.SingleOrDefault<EnumType>((Func<EnumType, bool>) (e => e.Name == name));

    public static System.Data.Entity.Core.Metadata.Edm.EntityType AddEntityType(
      this EdmModel model,
      string name,
      string modelNamespace = null)
    {
      System.Data.Entity.Core.Metadata.Edm.EntityType entityType = new System.Data.Entity.Core.Metadata.Edm.EntityType(name, modelNamespace ?? "CodeFirstNamespace", DataSpace.CSpace);
      model.AddItem(entityType);
      return entityType;
    }

    public static EntitySet GetEntitySet(this EdmModel model, System.Data.Entity.Core.Metadata.Edm.EntityType entityType) => model.GetEntitySets().SingleOrDefault<EntitySet>((Func<EntitySet, bool>) (e => e.ElementType == entityType.GetRootType()));

    public static AssociationSet GetAssociationSet(
      this EdmModel model,
      AssociationType associationType)
    {
      return model.Containers.Single<EntityContainer>().AssociationSets.SingleOrDefault<AssociationSet>((Func<AssociationSet, bool>) (a => a.ElementType == associationType));
    }

    public static IEnumerable<EntitySet> GetEntitySets(this EdmModel model) => (IEnumerable<EntitySet>) model.Containers.Single<EntityContainer>().EntitySets;

    public static EntitySet AddEntitySet(
      this EdmModel model,
      string name,
      System.Data.Entity.Core.Metadata.Edm.EntityType elementType,
      string table = null)
    {
      EntitySet entitySet = new EntitySet(name, (string) null, table, (string) null, elementType);
      model.Containers.Single<EntityContainer>().AddEntitySetBase((EntitySetBase) entitySet);
      return entitySet;
    }

    public static ComplexType AddComplexType(
      this EdmModel model,
      string name,
      string modelNamespace = null)
    {
      ComplexType complexType = new ComplexType(name, modelNamespace ?? "CodeFirstNamespace", DataSpace.CSpace);
      model.AddItem(complexType);
      return complexType;
    }

    public static EnumType AddEnumType(
      this EdmModel model,
      string name,
      string modelNamespace = null)
    {
      EnumType enumType = new EnumType(name, modelNamespace ?? "CodeFirstNamespace", PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Int32), false, DataSpace.CSpace);
      model.AddItem(enumType);
      return enumType;
    }

    public static AssociationType GetAssociationType(
      this EdmModel model,
      string name)
    {
      return model.AssociationTypes.SingleOrDefault<AssociationType>((Func<AssociationType, bool>) (a => a.Name == name));
    }

    public static IEnumerable<AssociationType> GetAssociationTypesBetween(
      this EdmModel model,
      System.Data.Entity.Core.Metadata.Edm.EntityType first,
      System.Data.Entity.Core.Metadata.Edm.EntityType second)
    {
      return model.AssociationTypes.Where<AssociationType>((Func<AssociationType, bool>) (a =>
      {
        if (a.SourceEnd.GetEntityType() == first && a.TargetEnd.GetEntityType() == second)
          return true;
        return a.SourceEnd.GetEntityType() == second && a.TargetEnd.GetEntityType() == first;
      }));
    }

    public static AssociationType AddAssociationType(
      this EdmModel model,
      string name,
      System.Data.Entity.Core.Metadata.Edm.EntityType sourceEntityType,
      RelationshipMultiplicity sourceAssociationEndKind,
      System.Data.Entity.Core.Metadata.Edm.EntityType targetEntityType,
      RelationshipMultiplicity targetAssociationEndKind,
      string modelNamespace = null)
    {
      AssociationType associationType = new AssociationType(name, modelNamespace ?? "CodeFirstNamespace", false, DataSpace.CSpace)
      {
        SourceEnd = new AssociationEndMember(name + "_Source", sourceEntityType.GetReferenceType(), sourceAssociationEndKind),
        TargetEnd = new AssociationEndMember(name + "_Target", targetEntityType.GetReferenceType(), targetAssociationEndKind)
      };
      model.AddAssociationType(associationType);
      return associationType;
    }

    public static void AddAssociationType(this EdmModel model, AssociationType associationType) => model.AddItem(associationType);

    public static void AddAssociationSet(this EdmModel model, AssociationSet associationSet) => model.Containers.Single<EntityContainer>().AddEntitySetBase((EntitySetBase) associationSet);

    public static void RemoveEntityType(this EdmModel model, System.Data.Entity.Core.Metadata.Edm.EntityType entityType)
    {
      model.RemoveItem(entityType);
      EntityContainer entityContainer = model.Containers.Single<EntityContainer>();
      EntitySet entitySet = entityContainer.EntitySets.SingleOrDefault<EntitySet>((Func<EntitySet, bool>) (a => a.ElementType == entityType));
      if (entitySet == null)
        return;
      entityContainer.RemoveEntitySetBase((EntitySetBase) entitySet);
    }

    public static void ReplaceEntitySet(
      this EdmModel model,
      System.Data.Entity.Core.Metadata.Edm.EntityType entityType,
      EntitySet newSet)
    {
      EntityContainer entityContainer = model.Containers.Single<EntityContainer>();
      EntitySet entitySet = entityContainer.EntitySets.SingleOrDefault<EntitySet>((Func<EntitySet, bool>) (a => a.ElementType == entityType));
      if (entitySet == null)
        return;
      entityContainer.RemoveEntitySetBase((EntitySetBase) entitySet);
      if (newSet == null)
        return;
      foreach (AssociationSet associationSet in model.Containers.Single<EntityContainer>().AssociationSets)
      {
        if (associationSet.SourceSet == entitySet)
          associationSet.SourceSet = newSet;
        if (associationSet.TargetSet == entitySet)
          associationSet.TargetSet = newSet;
      }
    }

    public static void RemoveAssociationType(this EdmModel model, AssociationType associationType)
    {
      model.RemoveItem(associationType);
      EntityContainer entityContainer = model.Containers.Single<EntityContainer>();
      AssociationSet associationSet = entityContainer.AssociationSets.SingleOrDefault<AssociationSet>((Func<AssociationSet, bool>) (a => a.ElementType == associationType));
      if (associationSet == null)
        return;
      entityContainer.RemoveEntitySetBase((EntitySetBase) associationSet);
    }

    public static AssociationSet AddAssociationSet(
      this EdmModel model,
      string name,
      AssociationType associationType)
    {
      AssociationSet associationSet = new AssociationSet(name, associationType)
      {
        SourceSet = model.GetEntitySet(associationType.SourceEnd.GetEntityType()),
        TargetSet = model.GetEntitySet(associationType.TargetEnd.GetEntityType())
      };
      model.Containers.Single<EntityContainer>().AddEntitySetBase((EntitySetBase) associationSet);
      return associationSet;
    }

    public static IEnumerable<System.Data.Entity.Core.Metadata.Edm.EntityType> GetDerivedTypes(
      this EdmModel model,
      System.Data.Entity.Core.Metadata.Edm.EntityType entityType)
    {
      return model.EntityTypes.Where<System.Data.Entity.Core.Metadata.Edm.EntityType>((Func<System.Data.Entity.Core.Metadata.Edm.EntityType, bool>) (et => et.BaseType == entityType));
    }

    public static IEnumerable<System.Data.Entity.Core.Metadata.Edm.EntityType> GetSelfAndAllDerivedTypes(
      this EdmModel model,
      System.Data.Entity.Core.Metadata.Edm.EntityType entityType)
    {
      List<System.Data.Entity.Core.Metadata.Edm.EntityType> entityTypes = new List<System.Data.Entity.Core.Metadata.Edm.EntityType>();
      EdmModelExtensions.AddSelfAndAllDerivedTypes(model, entityType, entityTypes);
      return (IEnumerable<System.Data.Entity.Core.Metadata.Edm.EntityType>) entityTypes;
    }

    private static void AddSelfAndAllDerivedTypes(
      EdmModel model,
      System.Data.Entity.Core.Metadata.Edm.EntityType entityType,
      List<System.Data.Entity.Core.Metadata.Edm.EntityType> entityTypes)
    {
      entityTypes.Add(entityType);
      foreach (System.Data.Entity.Core.Metadata.Edm.EntityType entityType1 in model.EntityTypes.Where<System.Data.Entity.Core.Metadata.Edm.EntityType>((Func<System.Data.Entity.Core.Metadata.Edm.EntityType, bool>) (et => et.BaseType == entityType)))
        EdmModelExtensions.AddSelfAndAllDerivedTypes(model, entityType1, entityTypes);
    }
  }
}

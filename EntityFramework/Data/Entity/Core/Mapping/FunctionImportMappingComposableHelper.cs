// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.FunctionImportMappingComposableHelper
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
using System.Xml;

namespace System.Data.Entity.Core.Mapping
{
  internal class FunctionImportMappingComposableHelper
  {
    private readonly EntityContainerMapping _entityContainerMapping;
    private readonly string m_sourceLocation;
    private readonly List<EdmSchemaError> m_parsingErrors;

    internal FunctionImportMappingComposableHelper(
      EntityContainerMapping entityContainerMapping,
      string sourceLocation,
      List<EdmSchemaError> parsingErrors)
    {
      this._entityContainerMapping = entityContainerMapping;
      this.m_sourceLocation = sourceLocation;
      this.m_parsingErrors = parsingErrors;
    }

    internal bool TryCreateFunctionImportMappingComposableWithStructuralResult(
      EdmFunction functionImport,
      EdmFunction cTypeTargetFunction,
      List<FunctionImportStructuralTypeMapping> typeMappings,
      RowType cTypeTvfElementType,
      RowType sTypeTvfElementType,
      IXmlLineInfo lineInfo,
      out FunctionImportMappingComposable mapping)
    {
      mapping = (FunctionImportMappingComposable) null;
      StructuralType returnType1;
      if (typeMappings.Count == 0 && MetadataHelper.TryGetFunctionImportReturnType<StructuralType>(functionImport, 0, out returnType1))
      {
        if (returnType1.Abstract)
        {
          FunctionImportMappingComposableHelper.AddToSchemaErrorWithMemberAndStructure(new Func<object, object, string>(System.Data.Entity.Resources.Strings.Mapping_FunctionImport_ImplicitMappingForAbstractReturnType), returnType1.FullName, functionImport.Identity, MappingErrorCode.MappingOfAbstractType, this.m_sourceLocation, lineInfo, (IList<EdmSchemaError>) this.m_parsingErrors);
          return false;
        }
        if (returnType1.BuiltInTypeKind == BuiltInTypeKind.EntityType)
          typeMappings.Add((FunctionImportStructuralTypeMapping) new FunctionImportEntityTypeMapping(Enumerable.Empty<System.Data.Entity.Core.Metadata.Edm.EntityType>(), (IEnumerable<System.Data.Entity.Core.Metadata.Edm.EntityType>) new System.Data.Entity.Core.Metadata.Edm.EntityType[1]
          {
            (System.Data.Entity.Core.Metadata.Edm.EntityType) returnType1
          }, Enumerable.Empty<FunctionImportEntityTypeMappingCondition>(), new Collection<FunctionImportReturnTypePropertyMapping>(), new LineInfo(lineInfo)));
        else
          typeMappings.Add((FunctionImportStructuralTypeMapping) new FunctionImportComplexTypeMapping((ComplexType) returnType1, new Collection<FunctionImportReturnTypePropertyMapping>(), new LineInfo(lineInfo)));
      }
      EdmItemCollection edmItemCollection = this._entityContainerMapping.StorageMappingItemCollection != null ? this._entityContainerMapping.StorageMappingItemCollection.EdmItemCollection : new EdmItemCollection(new EdmModel(DataSpace.CSpace));
      FunctionImportStructuralTypeMappingKB functionImportKB = new FunctionImportStructuralTypeMappingKB((IEnumerable<FunctionImportStructuralTypeMapping>) typeMappings, (ItemCollection) edmItemCollection);
      List<Tuple<StructuralType, List<ConditionPropertyMapping>, List<PropertyMapping>>> structuralTypeMappings = new List<Tuple<StructuralType, List<ConditionPropertyMapping>, List<PropertyMapping>>>();
      EdmProperty[] keys = (EdmProperty[]) null;
      if (functionImportKB.MappedEntityTypes.Count > 0)
      {
        if (!functionImportKB.ValidateTypeConditions(true, (IList<EdmSchemaError>) this.m_parsingErrors, this.m_sourceLocation))
          return false;
        for (int index = 0; index < functionImportKB.MappedEntityTypes.Count; ++index)
        {
          List<ConditionPropertyMapping> typeConditions;
          List<PropertyMapping> propertyMappings;
          if (this.TryConvertToEntityTypeConditionsAndPropertyMappings(functionImport, functionImportKB, index, cTypeTvfElementType, sTypeTvfElementType, lineInfo, out typeConditions, out propertyMappings))
            structuralTypeMappings.Add(Tuple.Create<StructuralType, List<ConditionPropertyMapping>, List<PropertyMapping>>((StructuralType) functionImportKB.MappedEntityTypes[index], typeConditions, propertyMappings));
        }
        if (structuralTypeMappings.Count < functionImportKB.MappedEntityTypes.Count)
          return false;
        if (!FunctionImportMappingComposableHelper.TryInferTVFKeys(structuralTypeMappings, out keys))
        {
          FunctionImportMappingComposableHelper.AddToSchemaErrorsWithMemberInfo(new Func<object, string>(System.Data.Entity.Resources.Strings.Mapping_FunctionImport_CannotInferTargetFunctionKeys), functionImport.Identity, MappingErrorCode.MappingFunctionImportCannotInferTargetFunctionKeys, this.m_sourceLocation, lineInfo, (IList<EdmSchemaError>) this.m_parsingErrors);
          return false;
        }
      }
      else
      {
        ComplexType returnType2;
        if (MetadataHelper.TryGetFunctionImportReturnType<ComplexType>(functionImport, 0, out returnType2))
        {
          List<PropertyMapping> propertyMappings;
          if (!this.TryConvertToPropertyMappings((StructuralType) returnType2, cTypeTvfElementType, sTypeTvfElementType, functionImport, functionImportKB, lineInfo, out propertyMappings))
            return false;
          structuralTypeMappings.Add(Tuple.Create<StructuralType, List<ConditionPropertyMapping>, List<PropertyMapping>>((StructuralType) returnType2, new List<ConditionPropertyMapping>(), propertyMappings));
        }
      }
      mapping = new FunctionImportMappingComposable(functionImport, cTypeTargetFunction, structuralTypeMappings, keys, this._entityContainerMapping);
      return true;
    }

    internal bool TryCreateFunctionImportMappingComposableWithScalarResult(
      EdmFunction functionImport,
      EdmFunction cTypeTargetFunction,
      EdmFunction sTypeTargetFunction,
      EdmType scalarResultType,
      RowType cTypeTvfElementType,
      IXmlLineInfo lineInfo,
      out FunctionImportMappingComposable mapping)
    {
      mapping = (FunctionImportMappingComposable) null;
      if (cTypeTvfElementType.Properties.Count > 1)
      {
        FunctionImportMappingComposableHelper.AddToSchemaErrors(System.Data.Entity.Resources.Strings.Mapping_FunctionImport_ScalarMappingToMulticolumnTVF((object) functionImport.Identity, (object) sTypeTargetFunction.Identity), MappingErrorCode.MappingFunctionImportScalarMappingToMulticolumnTVF, this.m_sourceLocation, lineInfo, (IList<EdmSchemaError>) this.m_parsingErrors);
        return false;
      }
      if (!FunctionImportMappingComposableHelper.ValidateFunctionImportMappingResultTypeCompatibility(TypeUsage.Create(scalarResultType), cTypeTvfElementType.Properties[0].TypeUsage))
      {
        FunctionImportMappingComposableHelper.AddToSchemaErrors(System.Data.Entity.Resources.Strings.Mapping_FunctionImport_ScalarMappingTypeMismatch((object) functionImport.ReturnParameter.TypeUsage.EdmType.FullName, (object) functionImport.Identity, (object) sTypeTargetFunction.ReturnParameter.TypeUsage.EdmType.FullName, (object) sTypeTargetFunction.Identity), MappingErrorCode.MappingFunctionImportScalarMappingTypeMismatch, this.m_sourceLocation, lineInfo, (IList<EdmSchemaError>) this.m_parsingErrors);
        return false;
      }
      mapping = new FunctionImportMappingComposable(functionImport, cTypeTargetFunction, (List<Tuple<StructuralType, List<ConditionPropertyMapping>, List<PropertyMapping>>>) null, (EdmProperty[]) null, this._entityContainerMapping);
      return true;
    }

    private bool TryConvertToEntityTypeConditionsAndPropertyMappings(
      EdmFunction functionImport,
      FunctionImportStructuralTypeMappingKB functionImportKB,
      int typeID,
      RowType cTypeTvfElementType,
      RowType sTypeTvfElementType,
      IXmlLineInfo navLineInfo,
      out List<ConditionPropertyMapping> typeConditions,
      out List<PropertyMapping> propertyMappings)
    {
      System.Data.Entity.Core.Metadata.Edm.EntityType mappedEntityType = functionImportKB.MappedEntityTypes[typeID];
      typeConditions = new List<ConditionPropertyMapping>();
      bool flag = false;
      foreach (FunctionImportNormalizedEntityTypeMapping entityTypeMapping in functionImportKB.NormalizedEntityTypeMappings.Where<FunctionImportNormalizedEntityTypeMapping>((Func<FunctionImportNormalizedEntityTypeMapping, bool>) (f => f.ImpliedEntityTypes[typeID])))
      {
        foreach (FunctionImportEntityTypeMappingCondition mappingCondition in entityTypeMapping.ColumnConditions.Where<FunctionImportEntityTypeMappingCondition>((Func<FunctionImportEntityTypeMappingCondition, bool>) (c => c != null)))
        {
          FunctionImportEntityTypeMappingCondition condition = mappingCondition;
          EdmProperty column;
          if (sTypeTvfElementType.Properties.TryGetValue(condition.ColumnName, false, out column))
          {
            object obj;
            bool? nullable;
            if (condition.ConditionValue.IsSentinel)
            {
              obj = (object) null;
              nullable = condition.ConditionValue != ValueCondition.IsNull ? new bool?(false) : new bool?(true);
            }
            else
            {
              PrimitiveType edmType = (PrimitiveType) cTypeTvfElementType.Properties[column.Name].TypeUsage.EdmType;
              obj = ((FunctionImportEntityTypeMappingConditionValue) condition).GetConditionValue(edmType.ClrEquivalentType, (Action) (() => FunctionImportMappingComposableHelper.AddToSchemaErrorWithMemberAndStructure(new Func<object, object, string>(System.Data.Entity.Resources.Strings.Mapping_InvalidContent_ConditionMapping_InvalidPrimitiveTypeKind), column.Name, column.TypeUsage.EdmType.FullName, MappingErrorCode.ConditionError, this.m_sourceLocation, (IXmlLineInfo) condition.LineInfo, (IList<EdmSchemaError>) this.m_parsingErrors)), (Action) (() => FunctionImportMappingComposableHelper.AddToSchemaErrors(System.Data.Entity.Resources.Strings.Mapping_ConditionValueTypeMismatch, MappingErrorCode.ConditionError, this.m_sourceLocation, (IXmlLineInfo) condition.LineInfo, (IList<EdmSchemaError>) this.m_parsingErrors)));
              if (obj == null)
              {
                flag = true;
                continue;
              }
              nullable = new bool?();
            }
            typeConditions.Add(obj != null ? (ConditionPropertyMapping) new ValueConditionMapping(column, obj) : (ConditionPropertyMapping) new IsNullConditionMapping(column, nullable.Value));
          }
          else
            FunctionImportMappingComposableHelper.AddToSchemaErrorsWithMemberInfo(new Func<object, string>(System.Data.Entity.Resources.Strings.Mapping_InvalidContent_Column), condition.ColumnName, MappingErrorCode.InvalidStorageMember, this.m_sourceLocation, (IXmlLineInfo) condition.LineInfo, (IList<EdmSchemaError>) this.m_parsingErrors);
        }
      }
      return !(flag | !this.TryConvertToPropertyMappings((StructuralType) mappedEntityType, cTypeTvfElementType, sTypeTvfElementType, functionImport, functionImportKB, navLineInfo, out propertyMappings));
    }

    private bool TryConvertToPropertyMappings(
      StructuralType structuralType,
      RowType cTypeTvfElementType,
      RowType sTypeTvfElementType,
      EdmFunction functionImport,
      FunctionImportStructuralTypeMappingKB functionImportKB,
      IXmlLineInfo navLineInfo,
      out List<PropertyMapping> propertyMappings)
    {
      propertyMappings = new List<PropertyMapping>();
      bool flag1 = false;
      foreach (EdmProperty structuralMember in (IEnumerable) TypeHelpers.GetAllStructuralMembers((EdmType) structuralType))
      {
        if (!Helper.IsScalarType(structuralMember.TypeUsage.EdmType))
        {
          this.m_parsingErrors.Add(new EdmSchemaError(System.Data.Entity.Resources.Strings.Mapping_Invalid_CSide_ScalarProperty((object) structuralMember.Name), 2085, EdmSchemaErrorSeverity.Error, this.m_sourceLocation, navLineInfo.LineNumber, navLineInfo.LinePosition));
          flag1 = true;
        }
        else
        {
          IXmlLineInfo lineInfo = (IXmlLineInfo) null;
          FunctionImportReturnTypeStructuralTypeColumnRenameMapping columnRenameMapping;
          bool flag2;
          string str;
          if (functionImportKB.ReturnTypeColumnsRenameMapping.TryGetValue(structuralMember.Name, out columnRenameMapping))
          {
            flag2 = true;
            str = columnRenameMapping.GetRename((EdmType) structuralType, out lineInfo);
          }
          else
          {
            flag2 = false;
            str = structuralMember.Name;
          }
          lineInfo = lineInfo == null || !lineInfo.HasLineInfo() ? navLineInfo : lineInfo;
          EdmProperty column;
          if (sTypeTvfElementType.Properties.TryGetValue(str, false, out column))
          {
            EdmProperty property = cTypeTvfElementType.Properties[str];
            if (FunctionImportMappingComposableHelper.ValidateFunctionImportMappingResultTypeCompatibility(structuralMember.TypeUsage, property.TypeUsage))
            {
              propertyMappings.Add((PropertyMapping) new ScalarPropertyMapping(structuralMember, column));
            }
            else
            {
              this.m_parsingErrors.Add(new EdmSchemaError(FunctionImportMappingComposableHelper.GetInvalidMemberMappingErrorMessage((EdmMember) structuralMember, (EdmMember) column), 2019, EdmSchemaErrorSeverity.Error, this.m_sourceLocation, lineInfo.LineNumber, lineInfo.LinePosition));
              flag1 = true;
            }
          }
          else if (flag2)
          {
            FunctionImportMappingComposableHelper.AddToSchemaErrorsWithMemberInfo(new Func<object, string>(System.Data.Entity.Resources.Strings.Mapping_InvalidContent_Column), str, MappingErrorCode.InvalidStorageMember, this.m_sourceLocation, lineInfo, (IList<EdmSchemaError>) this.m_parsingErrors);
            flag1 = true;
          }
          else
          {
            this.m_parsingErrors.Add(new EdmSchemaError(System.Data.Entity.Resources.Strings.Mapping_FunctionImport_PropertyNotMapped((object) structuralMember.Name, (object) structuralType.FullName, (object) functionImport.Identity), 2104, EdmSchemaErrorSeverity.Error, this.m_sourceLocation, lineInfo.LineNumber, lineInfo.LinePosition));
            flag1 = true;
          }
        }
      }
      return !flag1;
    }

    private static bool TryInferTVFKeys(
      List<Tuple<StructuralType, List<ConditionPropertyMapping>, List<PropertyMapping>>> structuralTypeMappings,
      out EdmProperty[] keys)
    {
      keys = (EdmProperty[]) null;
      foreach (Tuple<StructuralType, List<ConditionPropertyMapping>, List<PropertyMapping>> structuralTypeMapping in structuralTypeMappings)
      {
        EdmProperty[] keys1;
        if (!FunctionImportMappingComposableHelper.TryInferTVFKeysForEntityType((System.Data.Entity.Core.Metadata.Edm.EntityType) structuralTypeMapping.Item1, structuralTypeMapping.Item3, out keys1))
        {
          keys = (EdmProperty[]) null;
          return false;
        }
        if (keys == null)
        {
          keys = keys1;
        }
        else
        {
          for (int index = 0; index < keys.Length; ++index)
          {
            if (!keys[index].EdmEquals((MetadataItem) keys1[index]))
            {
              keys = (EdmProperty[]) null;
              return false;
            }
          }
        }
      }
      for (int index = 0; index < keys.Length; ++index)
      {
        if (keys[index].Nullable)
        {
          keys = (EdmProperty[]) null;
          return false;
        }
      }
      return true;
    }

    private static bool TryInferTVFKeysForEntityType(
      System.Data.Entity.Core.Metadata.Edm.EntityType entityType,
      List<PropertyMapping> propertyMappings,
      out EdmProperty[] keys)
    {
      keys = new EdmProperty[entityType.KeyMembers.Count];
      for (int index = 0; index < keys.Length; ++index)
      {
        if (!(propertyMappings[entityType.Properties.IndexOf((EdmProperty) entityType.KeyMembers[index])] is ScalarPropertyMapping propertyMapping2))
        {
          keys = (EdmProperty[]) null;
          return false;
        }
        keys[index] = propertyMapping2.Column;
      }
      return true;
    }

    private static bool ValidateFunctionImportMappingResultTypeCompatibility(
      TypeUsage cSpaceMemberType,
      TypeUsage sSpaceMemberType)
    {
      TypeUsage typeUsage1 = sSpaceMemberType;
      TypeUsage typeUsage2 = FunctionImportMappingComposableHelper.ResolveTypeUsageForEnums(cSpaceMemberType);
      return TypeSemantics.IsStructurallyEqualOrPromotableTo(typeUsage1, typeUsage2) | TypeSemantics.IsStructurallyEqualOrPromotableTo(typeUsage2, typeUsage1);
    }

    private static TypeUsage ResolveTypeUsageForEnums(TypeUsage typeUsage) => MappingItemLoader.ResolveTypeUsageForEnums(typeUsage);

    private static void AddToSchemaErrors(
      string message,
      MappingErrorCode errorCode,
      string location,
      IXmlLineInfo lineInfo,
      IList<EdmSchemaError> parsingErrors)
    {
      MappingItemLoader.AddToSchemaErrors(message, errorCode, location, lineInfo, parsingErrors);
    }

    private static void AddToSchemaErrorsWithMemberInfo(
      Func<object, string> messageFormat,
      string errorMember,
      MappingErrorCode errorCode,
      string location,
      IXmlLineInfo lineInfo,
      IList<EdmSchemaError> parsingErrors)
    {
      MappingItemLoader.AddToSchemaErrorsWithMemberInfo(messageFormat, errorMember, errorCode, location, lineInfo, parsingErrors);
    }

    private static void AddToSchemaErrorWithMemberAndStructure(
      Func<object, object, string> messageFormat,
      string errorMember,
      string errorStructure,
      MappingErrorCode errorCode,
      string location,
      IXmlLineInfo lineInfo,
      IList<EdmSchemaError> parsingErrors)
    {
      MappingItemLoader.AddToSchemaErrorWithMemberAndStructure(messageFormat, errorMember, errorStructure, errorCode, location, lineInfo, parsingErrors);
    }

    private static string GetInvalidMemberMappingErrorMessage(
      EdmMember cSpaceMember,
      EdmMember sSpaceMember)
    {
      return MappingItemLoader.GetInvalidMemberMappingErrorMessage(cSpaceMember, sSpaceMember);
    }
  }
}

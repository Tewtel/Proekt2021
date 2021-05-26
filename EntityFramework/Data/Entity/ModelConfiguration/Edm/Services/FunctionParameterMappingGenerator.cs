// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Edm.Services.FunctionParameterMappingGenerator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Mapping.Update.Internal;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Edm.Services
{
  internal class FunctionParameterMappingGenerator : StructuralTypeMappingGenerator
  {
    public FunctionParameterMappingGenerator(DbProviderManifest providerManifest)
      : base(providerManifest)
    {
    }

    public IEnumerable<ModificationFunctionParameterBinding> Generate(
      ModificationOperator modificationOperator,
      IEnumerable<EdmProperty> properties,
      IList<ColumnMappingBuilder> columnMappings,
      IList<EdmProperty> propertyPath,
      bool useOriginalValues = false)
    {
      foreach (EdmProperty property1 in properties)
      {
        EdmProperty property = property1;
        if (property.IsComplexType && propertyPath.Any<EdmProperty>((Func<EdmProperty, bool>) (p => p.IsComplexType && p.ComplexType == property.ComplexType)))
          throw System.Data.Entity.Resources.Error.CircularComplexTypeHierarchy();
        propertyPath.Add(property);
        if (property.IsComplexType)
        {
          foreach (ModificationFunctionParameterBinding parameterBinding in this.Generate(modificationOperator, (IEnumerable<EdmProperty>) property.ComplexType.Properties, columnMappings, propertyPath, useOriginalValues))
            yield return parameterBinding;
        }
        else
        {
          StoreGeneratedPattern? generatedPattern1 = property.GetStoreGeneratedPattern();
          StoreGeneratedPattern generatedPattern2 = StoreGeneratedPattern.Identity;
          if (!(generatedPattern1.GetValueOrDefault() == generatedPattern2 & generatedPattern1.HasValue) || modificationOperator != ModificationOperator.Insert)
          {
            EdmProperty columnProperty = columnMappings.First<ColumnMappingBuilder>((Func<ColumnMappingBuilder, bool>) (cm => cm.PropertyPath.SequenceEqual<EdmProperty>((IEnumerable<EdmProperty>) propertyPath))).ColumnProperty;
            generatedPattern1 = property.GetStoreGeneratedPattern();
            StoreGeneratedPattern generatedPattern3 = StoreGeneratedPattern.Computed;
            if (!(generatedPattern1.GetValueOrDefault() == generatedPattern3 & generatedPattern1.HasValue) && (modificationOperator != ModificationOperator.Delete || property.IsKeyMember))
              yield return new ModificationFunctionParameterBinding(new FunctionParameter(columnProperty.Name, columnProperty.TypeUsage, ParameterMode.In), new ModificationFunctionMemberPath((IEnumerable<EdmMember>) propertyPath, (AssociationSet) null), !useOriginalValues);
            if (modificationOperator != ModificationOperator.Insert && property.ConcurrencyMode == ConcurrencyMode.Fixed)
              yield return new ModificationFunctionParameterBinding(new FunctionParameter(columnProperty.Name + "_Original", columnProperty.TypeUsage, ParameterMode.In), new ModificationFunctionMemberPath((IEnumerable<EdmMember>) propertyPath, (AssociationSet) null), false);
            columnProperty = (EdmProperty) null;
          }
        }
        propertyPath.Remove(property);
      }
    }

    public IEnumerable<ModificationFunctionParameterBinding> Generate(
      IEnumerable<Tuple<ModificationFunctionMemberPath, EdmProperty>> iaFkProperties,
      bool useOriginalValues = false)
    {
      return iaFkProperties.Select(iaFkProperty => new
      {
        iaFkProperty = iaFkProperty,
        functionParameter = new FunctionParameter(iaFkProperty.Item2.Name, iaFkProperty.Item2.TypeUsage, ParameterMode.In)
      }).Select(_param1 => new ModificationFunctionParameterBinding(_param1.functionParameter, _param1.iaFkProperty.Item1, !useOriginalValues));
    }
  }
}

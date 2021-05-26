// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive.PrimitivePropertyConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Internal;
using System.Data.Entity.ModelConfiguration.Configuration.Types;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Configuration.Properties.Primitive
{
  internal class PrimitivePropertyConfiguration : PropertyConfiguration
  {
    private readonly IDictionary<string, object> _annotations = (IDictionary<string, object>) new Dictionary<string, object>();

    public PrimitivePropertyConfiguration() => this.OverridableConfigurationParts = OverridableConfigurationParts.OverridableInCSpace | OverridableConfigurationParts.OverridableInSSpace;

    protected PrimitivePropertyConfiguration(PrimitivePropertyConfiguration source)
    {
      System.Data.Entity.Utilities.Check.NotNull<PrimitivePropertyConfiguration>(source, nameof (source));
      this.TypeConfiguration = source.TypeConfiguration;
      this.IsNullable = source.IsNullable;
      this.ConcurrencyMode = source.ConcurrencyMode;
      this.DatabaseGeneratedOption = source.DatabaseGeneratedOption;
      this.ColumnType = source.ColumnType;
      this.ColumnName = source.ColumnName;
      this.ParameterName = source.ParameterName;
      this.ColumnOrder = source.ColumnOrder;
      this.OverridableConfigurationParts = source.OverridableConfigurationParts;
      foreach (KeyValuePair<string, object> annotation in (IEnumerable<KeyValuePair<string, object>>) source._annotations)
        this._annotations.Add(annotation);
    }

    internal virtual PrimitivePropertyConfiguration Clone() => new PrimitivePropertyConfiguration(this);

    public bool? IsNullable { get; set; }

    public System.Data.Entity.Core.Metadata.Edm.ConcurrencyMode? ConcurrencyMode { get; set; }

    public System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption? DatabaseGeneratedOption { get; set; }

    public string ColumnType { get; set; }

    public string ColumnName { get; set; }

    public IDictionary<string, object> Annotations => this._annotations;

    public virtual void SetAnnotation(string name, object value)
    {
      if (!name.IsValidUndottedName())
        throw new ArgumentException(System.Data.Entity.Resources.Strings.BadAnnotationName((object) name));
      this._annotations[name] = value;
    }

    public string ParameterName { get; set; }

    public int? ColumnOrder { get; set; }

    internal OverridableConfigurationParts OverridableConfigurationParts { get; set; }

    internal StructuralTypeConfiguration TypeConfiguration { get; set; }

    internal virtual void Configure(EdmProperty property) => this.Clone().MergeWithExistingConfiguration(property, (Func<string, Exception>) (errorMessage =>
    {
      PropertyInfo clrPropertyInfo = property.GetClrPropertyInfo();
      return System.Data.Entity.Resources.Error.ConflictingPropertyConfiguration((object) property.Name, clrPropertyInfo == (PropertyInfo) null ? (object) string.Empty : (object) ObjectContextTypeCache.GetObjectType(clrPropertyInfo.DeclaringType).FullNameWithNesting(), (object) errorMessage);
    }), true, false).ConfigureProperty(property);

    private PrimitivePropertyConfiguration MergeWithExistingConfiguration(
      EdmProperty property,
      Func<string, Exception> getConflictException,
      bool inCSpace,
      bool fillFromExistingConfiguration)
    {
      if (!(property.GetConfiguration() is PrimitivePropertyConfiguration configuration))
        return this;
      OverridableConfigurationParts configurationParts = inCSpace ? OverridableConfigurationParts.OverridableInCSpace : OverridableConfigurationParts.OverridableInSSpace;
      if (configuration.OverridableConfigurationParts.HasFlag((Enum) configurationParts) | fillFromExistingConfiguration)
        return configuration.OverrideFrom(this, inCSpace);
      string errorMessage;
      if (this.OverridableConfigurationParts.HasFlag((Enum) configurationParts) || configuration.IsCompatible(this, inCSpace, out errorMessage))
        return this.OverrideFrom(configuration, inCSpace);
      throw getConflictException(errorMessage);
    }

    private PrimitivePropertyConfiguration OverrideFrom(
      PrimitivePropertyConfiguration overridingConfiguration,
      bool inCSpace)
    {
      if (overridingConfiguration.GetType().IsAssignableFrom(this.GetType()))
      {
        this.MakeCompatibleWith(overridingConfiguration, inCSpace);
        this.FillFrom(overridingConfiguration, inCSpace);
        return this;
      }
      overridingConfiguration.FillFrom(this, inCSpace);
      return overridingConfiguration;
    }

    protected virtual void ConfigureProperty(EdmProperty property)
    {
      if (this.IsNullable.HasValue)
        property.Nullable = this.IsNullable.Value;
      if (this.ConcurrencyMode.HasValue)
        property.ConcurrencyMode = this.ConcurrencyMode.Value;
      if (this.DatabaseGeneratedOption.HasValue)
      {
        property.SetStoreGeneratedPattern((StoreGeneratedPattern) this.DatabaseGeneratedOption.Value);
        if (this.DatabaseGeneratedOption.Value == System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)
          property.Nullable = false;
      }
      property.SetConfiguration((object) this);
    }

    internal void Configure(
      IEnumerable<Tuple<ColumnMappingBuilder, EntityType>> propertyMappings,
      DbProviderManifest providerManifest,
      bool allowOverride = false,
      bool fillFromExistingConfiguration = false)
    {
      propertyMappings.Each<Tuple<ColumnMappingBuilder, EntityType>>((Action<Tuple<ColumnMappingBuilder, EntityType>>) (pm => this.Configure(pm.Item1.ColumnProperty, pm.Item2, providerManifest, allowOverride, fillFromExistingConfiguration)));
    }

    internal void ConfigureFunctionParameters(IEnumerable<FunctionParameter> parameters) => parameters.Each<FunctionParameter>(new Action<FunctionParameter>(this.ConfigureParameterName));

    private void ConfigureParameterName(FunctionParameter parameter)
    {
      if (string.IsNullOrWhiteSpace(this.ParameterName) || string.Equals(this.ParameterName, parameter.Name, StringComparison.Ordinal))
        return;
      parameter.Name = this.ParameterName;
      IEnumerable<FunctionParameter> ts = parameter.DeclaringFunction.Parameters.Select(p => new
      {
        p = p,
        configuration = p.GetConfiguration() as PrimitivePropertyConfiguration
      }).Where(_param1 =>
      {
        if (_param1.p == parameter || !string.Equals(this.ParameterName, _param1.p.Name, StringComparison.Ordinal))
          return false;
        return _param1.configuration == null || _param1.configuration.ParameterName == null;
      }).Select(_param1 => _param1.p);
      List<FunctionParameter> renamedParameters = new List<FunctionParameter>()
      {
        parameter
      };
      Action<FunctionParameter> action = (Action<FunctionParameter>) (c =>
      {
        c.Name = ((IEnumerable<INamedDataModelItem>) renamedParameters).UniquifyName(this.ParameterName);
        renamedParameters.Add(c);
      });
      ts.Each<FunctionParameter>(action);
      parameter.SetConfiguration((object) this);
    }

    internal void Configure(
      EdmProperty column,
      EntityType table,
      DbProviderManifest providerManifest,
      bool allowOverride = false,
      bool fillFromExistingConfiguration = false)
    {
      PrimitivePropertyConfiguration propertyConfiguration = this.Clone();
      if (allowOverride)
        propertyConfiguration.OverridableConfigurationParts |= OverridableConfigurationParts.OverridableInSSpace;
      propertyConfiguration.MergeWithExistingConfiguration(column, (Func<string, Exception>) (errorMessage => System.Data.Entity.Resources.Error.ConflictingColumnConfiguration((object) column.Name, (object) table.Name, (object) errorMessage)), false, fillFromExistingConfiguration).ConfigureColumn(column, table, providerManifest);
    }

    protected virtual void ConfigureColumn(
      EdmProperty column,
      EntityType table,
      DbProviderManifest providerManifest)
    {
      this.ConfigureColumnName(column, table);
      this.ConfigureAnnotations(column);
      if (!string.IsNullOrWhiteSpace(this.ColumnType))
        column.PrimitiveType = providerManifest.GetStoreTypeFromName(this.ColumnType);
      if (this.ColumnOrder.HasValue)
        column.SetOrder(this.ColumnOrder.Value);
      PrimitiveType primitiveType = providerManifest.GetStoreTypes().SingleOrDefault<PrimitiveType>((Func<PrimitiveType, bool>) (t => t.Name.Equals(column.TypeName, StringComparison.OrdinalIgnoreCase)));
      if (primitiveType != null)
        primitiveType.FacetDescriptions.Each<FacetDescription>((Action<FacetDescription>) (f => this.Configure(column, f)));
      column.SetConfiguration((object) this);
    }

    private void ConfigureColumnName(EdmProperty column, EntityType table)
    {
      if (string.IsNullOrWhiteSpace(this.ColumnName) || string.Equals(this.ColumnName, column.Name, StringComparison.Ordinal))
        return;
      column.Name = this.ColumnName;
      IEnumerable<EdmProperty> ts = table.Properties.Select(c => new
      {
        c = c,
        configuration = c.GetConfiguration() as PrimitivePropertyConfiguration
      }).Where(_param1 =>
      {
        if (_param1.c == column || !string.Equals(this.ColumnName, _param1.c.GetPreferredName(), StringComparison.Ordinal))
          return false;
        return _param1.configuration == null || _param1.configuration.ColumnName == null;
      }).Select(_param1 => _param1.c);
      List<EdmProperty> renamedColumns = new List<EdmProperty>()
      {
        column
      };
      Action<EdmProperty> action = (Action<EdmProperty>) (c =>
      {
        c.Name = ((IEnumerable<INamedDataModelItem>) renamedColumns).UniquifyName(this.ColumnName);
        renamedColumns.Add(c);
      });
      ts.Each<EdmProperty>(action);
    }

    private void ConfigureAnnotations(EdmProperty column)
    {
      foreach (KeyValuePair<string, object> annotation in (IEnumerable<KeyValuePair<string, object>>) this._annotations)
        column.AddAnnotation("http://schemas.microsoft.com/ado/2013/11/edm/customannotation:" + annotation.Key, annotation.Value);
    }

    internal virtual void Configure(EdmProperty column, FacetDescription facetDescription)
    {
    }

    internal virtual void CopyFrom(PrimitivePropertyConfiguration other)
    {
      if (this == other)
        return;
      this.ColumnName = other.ColumnName;
      this.ParameterName = other.ParameterName;
      this.ColumnOrder = other.ColumnOrder;
      this.ColumnType = other.ColumnType;
      this.ConcurrencyMode = other.ConcurrencyMode;
      this.DatabaseGeneratedOption = other.DatabaseGeneratedOption;
      this.IsNullable = other.IsNullable;
      this.OverridableConfigurationParts = other.OverridableConfigurationParts;
      this._annotations.Clear();
      foreach (KeyValuePair<string, object> annotation in (IEnumerable<KeyValuePair<string, object>>) other._annotations)
        this._annotations[annotation.Key] = annotation.Value;
    }

    internal virtual void FillFrom(PrimitivePropertyConfiguration other, bool inCSpace)
    {
      if (this == other)
        return;
      if (inCSpace)
      {
        if (!this.ConcurrencyMode.HasValue)
          this.ConcurrencyMode = other.ConcurrencyMode;
        if (!this.DatabaseGeneratedOption.HasValue)
          this.DatabaseGeneratedOption = other.DatabaseGeneratedOption;
        if (!this.IsNullable.HasValue)
          this.IsNullable = other.IsNullable;
        if (other.OverridableConfigurationParts.HasFlag((Enum) OverridableConfigurationParts.OverridableInCSpace))
          return;
        this.OverridableConfigurationParts &= ~OverridableConfigurationParts.OverridableInCSpace;
      }
      else
      {
        if (this.ColumnName == null)
          this.ColumnName = other.ColumnName;
        if (this.ParameterName == null)
          this.ParameterName = other.ParameterName;
        if (!this.ColumnOrder.HasValue)
          this.ColumnOrder = other.ColumnOrder;
        if (this.ColumnType == null)
          this.ColumnType = other.ColumnType;
        foreach (KeyValuePair<string, object> annotation1 in (IEnumerable<KeyValuePair<string, object>>) other._annotations)
        {
          if (this._annotations.ContainsKey(annotation1.Key))
          {
            if (this._annotations[annotation1.Key] is IMergeableAnnotation annotation5)
              this._annotations[annotation1.Key] = annotation5.MergeWith(annotation1.Value);
          }
          else
            this._annotations[annotation1.Key] = annotation1.Value;
        }
        if (other.OverridableConfigurationParts.HasFlag((Enum) OverridableConfigurationParts.OverridableInSSpace))
          return;
        this.OverridableConfigurationParts &= ~OverridableConfigurationParts.OverridableInSSpace;
      }
    }

    internal virtual void MakeCompatibleWith(PrimitivePropertyConfiguration other, bool inCSpace)
    {
      if (this == other)
        return;
      if (inCSpace)
      {
        System.Data.Entity.Core.Metadata.Edm.ConcurrencyMode? nullable1 = other.ConcurrencyMode;
        if (nullable1.HasValue)
        {
          nullable1 = new System.Data.Entity.Core.Metadata.Edm.ConcurrencyMode?();
          this.ConcurrencyMode = nullable1;
        }
        System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption? nullable2 = other.DatabaseGeneratedOption;
        if (nullable2.HasValue)
        {
          nullable2 = new System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption?();
          this.DatabaseGeneratedOption = nullable2;
        }
        bool? nullable3 = other.IsNullable;
        if (!nullable3.HasValue)
          return;
        nullable3 = new bool?();
        this.IsNullable = nullable3;
      }
      else
      {
        if (other.ColumnName != null)
          this.ColumnName = (string) null;
        if (other.ParameterName != null)
          this.ParameterName = (string) null;
        int? nullable = other.ColumnOrder;
        if (nullable.HasValue)
        {
          nullable = new int?();
          this.ColumnOrder = nullable;
        }
        if (other.ColumnType != null)
          this.ColumnType = (string) null;
        foreach (string key in (IEnumerable<string>) other._annotations.Keys)
        {
          if (this._annotations.ContainsKey(key) && (!(this._annotations[key] is IMergeableAnnotation annotation3) || !(bool) annotation3.IsCompatibleWith(other._annotations[key])))
            this._annotations.Remove(key);
        }
      }
    }

    internal virtual bool IsCompatible(
      PrimitivePropertyConfiguration other,
      bool inCSpace,
      out string errorMessage)
    {
      errorMessage = string.Empty;
      if (other == null || this == other)
        return true;
      int num1;
      if (inCSpace)
        num1 = this.IsCompatible<bool, PrimitivePropertyConfiguration>((Expression<Func<PrimitivePropertyConfiguration, bool?>>) (c => c.IsNullable), other, ref errorMessage) ? 1 : 0;
      else
        num1 = 1;
      bool flag1 = num1 != 0;
      int num2;
      if (inCSpace)
        num2 = this.IsCompatible<System.Data.Entity.Core.Metadata.Edm.ConcurrencyMode, PrimitivePropertyConfiguration>((Expression<Func<PrimitivePropertyConfiguration, System.Data.Entity.Core.Metadata.Edm.ConcurrencyMode?>>) (c => c.ConcurrencyMode), other, ref errorMessage) ? 1 : 0;
      else
        num2 = 1;
      bool flag2 = num2 != 0;
      int num3;
      if (inCSpace)
        num3 = this.IsCompatible<System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption, PrimitivePropertyConfiguration>((Expression<Func<PrimitivePropertyConfiguration, System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption?>>) (c => c.DatabaseGeneratedOption), other, ref errorMessage) ? 1 : 0;
      else
        num3 = 1;
      bool flag3 = num3 != 0;
      int num4;
      if (!inCSpace)
        num4 = this.IsCompatible<PrimitivePropertyConfiguration>((Expression<Func<PrimitivePropertyConfiguration, string>>) (c => c.ColumnName), other, ref errorMessage) ? 1 : 0;
      else
        num4 = 1;
      bool flag4 = num4 != 0;
      int num5;
      if (!inCSpace)
        num5 = this.IsCompatible<PrimitivePropertyConfiguration>((Expression<Func<PrimitivePropertyConfiguration, string>>) (c => c.ParameterName), other, ref errorMessage) ? 1 : 0;
      else
        num5 = 1;
      bool flag5 = num5 != 0;
      int num6;
      if (!inCSpace)
        num6 = this.IsCompatible<int, PrimitivePropertyConfiguration>((Expression<Func<PrimitivePropertyConfiguration, int?>>) (c => c.ColumnOrder), other, ref errorMessage) ? 1 : 0;
      else
        num6 = 1;
      bool flag6 = num6 != 0;
      int num7;
      if (!inCSpace)
        num7 = this.IsCompatible<PrimitivePropertyConfiguration>((Expression<Func<PrimitivePropertyConfiguration, string>>) (c => c.ColumnType), other, ref errorMessage) ? 1 : 0;
      else
        num7 = 1;
      bool flag7 = num7 != 0;
      bool flag8 = inCSpace || this.AnnotationsAreCompatible(other, ref errorMessage);
      return flag1 & flag2 & flag3 & flag4 & flag5 & flag6 & flag7 & flag8;
    }

    private bool AnnotationsAreCompatible(
      PrimitivePropertyConfiguration other,
      ref string errorMessage)
    {
      bool flag = true;
      foreach (KeyValuePair<string, object> annotation1 in (IEnumerable<KeyValuePair<string, object>>) this.Annotations)
      {
        if (other.Annotations.ContainsKey(annotation1.Key))
        {
          object objA = annotation1.Value;
          object annotation2 = other.Annotations[annotation1.Key];
          if (objA is IMergeableAnnotation mergeableAnnotation3)
          {
            CompatibilityResult compatibilityResult = mergeableAnnotation3.IsCompatibleWith(annotation2);
            if (!(bool) compatibilityResult)
            {
              flag = false;
              errorMessage = errorMessage + Environment.NewLine + "\t" + compatibilityResult.ErrorMessage;
            }
          }
          else if (!object.Equals(objA, annotation2))
          {
            flag = false;
            errorMessage = errorMessage + Environment.NewLine + "\t" + System.Data.Entity.Resources.Strings.ConflictingAnnotationValue((object) annotation1.Key, (object) objA.ToString(), (object) annotation2.ToString());
          }
        }
      }
      return flag;
    }

    protected bool IsCompatible<TProperty, TConfiguration>(
      Expression<Func<TConfiguration, TProperty?>> propertyExpression,
      TConfiguration other,
      ref string errorMessage)
      where TProperty : struct
      where TConfiguration : PrimitivePropertyConfiguration
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TConfiguration, TProperty?>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotNull<TConfiguration>(other, nameof (other));
      PropertyInfo propertyInfo = propertyExpression.GetSimplePropertyAccess().Single<PropertyInfo>();
      TProperty? thisConfiguration = (TProperty?) propertyInfo.GetValue((object) this, (object[]) null);
      TProperty? other1 = (TProperty?) propertyInfo.GetValue((object) other, (object[]) null);
      if (PrimitivePropertyConfiguration.IsCompatible<TProperty>(thisConfiguration, other1))
        return true;
      errorMessage = errorMessage + Environment.NewLine + "\t" + System.Data.Entity.Resources.Strings.ConflictingConfigurationValue((object) propertyInfo.Name, (object) thisConfiguration, (object) propertyInfo.Name, (object) other1);
      return false;
    }

    protected bool IsCompatible<TConfiguration>(
      Expression<Func<TConfiguration, string>> propertyExpression,
      TConfiguration other,
      ref string errorMessage)
      where TConfiguration : PrimitivePropertyConfiguration
    {
      System.Data.Entity.Utilities.Check.NotNull<Expression<Func<TConfiguration, string>>>(propertyExpression, nameof (propertyExpression));
      System.Data.Entity.Utilities.Check.NotNull<TConfiguration>(other, nameof (other));
      PropertyInfo propertyInfo = propertyExpression.GetSimplePropertyAccess().Single<PropertyInfo>();
      string thisConfiguration = (string) propertyInfo.GetValue((object) this, (object[]) null);
      string other1 = (string) propertyInfo.GetValue((object) other, (object[]) null);
      if (PrimitivePropertyConfiguration.IsCompatible(thisConfiguration, other1))
        return true;
      errorMessage = errorMessage + Environment.NewLine + "\t" + System.Data.Entity.Resources.Strings.ConflictingConfigurationValue((object) propertyInfo.Name, (object) thisConfiguration, (object) propertyInfo.Name, (object) other1);
      return false;
    }

    protected static bool IsCompatible<T>(T? thisConfiguration, T? other) where T : struct => !thisConfiguration.HasValue || !other.HasValue || object.Equals((object) thisConfiguration.Value, (object) other.Value);

    protected static bool IsCompatible(string thisConfiguration, string other) => thisConfiguration == null || other == null || object.Equals((object) thisConfiguration, (object) other);
  }
}

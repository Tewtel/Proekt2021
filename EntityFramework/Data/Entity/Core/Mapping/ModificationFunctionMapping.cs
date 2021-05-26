// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ModificationFunctionMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;
using System.Linq;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Describes modification function binding for change processing of entities or associations.
  /// </summary>
  public sealed class ModificationFunctionMapping : MappingItem
  {
    private FunctionParameter _rowsAffectedParameter;
    private readonly EdmFunction _function;
    private readonly ReadOnlyCollection<ModificationFunctionParameterBinding> _parameterBindings;
    private readonly ReadOnlyCollection<AssociationSetEnd> _collocatedAssociationSetEnds;
    private readonly ReadOnlyCollection<ModificationFunctionResultBinding> _resultBindings;

    /// <summary>
    /// Initializes a new ModificationFunctionMapping instance.
    /// </summary>
    /// <param name="entitySet">The entity or association set.</param>
    /// <param name="entityType">The entity or association type.</param>
    /// <param name="function">The metadata of function to which we should bind.</param>
    /// <param name="parameterBindings">Bindings for function parameters.</param>
    /// <param name="rowsAffectedParameter">The output parameter producing number of rows affected.</param>
    /// <param name="resultBindings">Bindings for the results of function evaluation</param>
    public ModificationFunctionMapping(
      EntitySetBase entitySet,
      EntityTypeBase entityType,
      EdmFunction function,
      IEnumerable<ModificationFunctionParameterBinding> parameterBindings,
      FunctionParameter rowsAffectedParameter,
      IEnumerable<ModificationFunctionResultBinding> resultBindings)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntitySetBase>(entitySet, nameof (entitySet));
      System.Data.Entity.Utilities.Check.NotNull<EdmFunction>(function, nameof (function));
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<ModificationFunctionParameterBinding>>(parameterBindings, nameof (parameterBindings));
      this._function = function;
      this._rowsAffectedParameter = rowsAffectedParameter;
      this._parameterBindings = new ReadOnlyCollection<ModificationFunctionParameterBinding>((IList<ModificationFunctionParameterBinding>) parameterBindings.ToList<ModificationFunctionParameterBinding>());
      if (resultBindings != null)
      {
        List<ModificationFunctionResultBinding> list = resultBindings.ToList<ModificationFunctionResultBinding>();
        if (0 < list.Count)
          this._resultBindings = new ReadOnlyCollection<ModificationFunctionResultBinding>((IList<ModificationFunctionResultBinding>) list);
      }
      this._collocatedAssociationSetEnds = new ReadOnlyCollection<AssociationSetEnd>((IList<AssociationSetEnd>) ModificationFunctionMapping.GetReferencedAssociationSetEnds(entitySet as EntitySet, entityType as EntityType, parameterBindings).ToList<AssociationSetEnd>());
    }

    /// <summary>
    /// Gets output parameter producing number of rows affected. May be null.
    /// </summary>
    public FunctionParameter RowsAffectedParameter
    {
      get => this._rowsAffectedParameter;
      internal set => this._rowsAffectedParameter = value;
    }

    internal string RowsAffectedParameterName => this.RowsAffectedParameter == null ? (string) null : this.RowsAffectedParameter.Name;

    /// <summary>Gets Metadata of function to which we should bind.</summary>
    public EdmFunction Function => this._function;

    /// <summary>Gets bindings for function parameters.</summary>
    public ReadOnlyCollection<ModificationFunctionParameterBinding> ParameterBindings => this._parameterBindings;

    internal ReadOnlyCollection<AssociationSetEnd> CollocatedAssociationSetEnds => this._collocatedAssociationSetEnds;

    /// <summary>Gets bindings for the results of function evaluation.</summary>
    public ReadOnlyCollection<ModificationFunctionResultBinding> ResultBindings => this._resultBindings;

    /// <inheritdoc />
    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Func{{{0}}}: Prm={{{1}}}, Result={{{2}}}", (object) this.Function, (object) StringUtil.ToCommaSeparatedStringSorted((IEnumerable) this.ParameterBindings), (object) StringUtil.ToCommaSeparatedStringSorted((IEnumerable) this.ResultBindings));

    internal override void SetReadOnly()
    {
      MappingItem.SetReadOnly((IEnumerable<MappingItem>) this._parameterBindings);
      MappingItem.SetReadOnly((IEnumerable<MappingItem>) this._resultBindings);
      base.SetReadOnly();
    }

    private static IEnumerable<AssociationSetEnd> GetReferencedAssociationSetEnds(
      EntitySet entitySet,
      EntityType entityType,
      IEnumerable<ModificationFunctionParameterBinding> parameterBindings)
    {
      HashSet<AssociationSetEnd> associationSetEndSet = new HashSet<AssociationSetEnd>();
      if (entitySet != null && entityType != null)
      {
        foreach (ModificationFunctionParameterBinding parameterBinding in parameterBindings)
        {
          AssociationSetEnd associationSetEnd = parameterBinding.MemberPath.AssociationSetEnd;
          if (associationSetEnd != null)
            associationSetEndSet.Add(associationSetEnd);
        }
        foreach (AssociationSet associationSet in entitySet.AssociationSets)
        {
          ReadOnlyMetadataCollection<ReferentialConstraint> referentialConstraints = associationSet.ElementType.ReferentialConstraints;
          if (referentialConstraints != null)
          {
            foreach (ReferentialConstraint referentialConstraint in referentialConstraints)
            {
              if (associationSet.AssociationSetEnds[referentialConstraint.ToRole.Name].EntitySet == entitySet && referentialConstraint.ToRole.GetEntityType().IsAssignableFrom((EdmType) entityType))
                associationSetEndSet.Add(associationSet.AssociationSetEnds[referentialConstraint.FromRole.Name]);
            }
          }
        }
      }
      return (IEnumerable<AssociationSetEnd>) associationSetEndSet;
    }
  }
}

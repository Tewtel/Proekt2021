// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.AssociationSetModificationFunctionMapping
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Describes modification function mappings for an association set.
  /// </summary>
  public sealed class AssociationSetModificationFunctionMapping : MappingItem
  {
    private readonly AssociationSet _associationSet;
    private readonly ModificationFunctionMapping _deleteFunctionMapping;
    private readonly ModificationFunctionMapping _insertFunctionMapping;

    /// <summary>
    /// Initializes a new AssociationSetModificationFunctionMapping instance.
    /// </summary>
    /// <param name="associationSet">An association set.</param>
    /// <param name="deleteFunctionMapping">A delete function mapping.</param>
    /// <param name="insertFunctionMapping">An insert function mapping.</param>
    public AssociationSetModificationFunctionMapping(
      AssociationSet associationSet,
      ModificationFunctionMapping deleteFunctionMapping,
      ModificationFunctionMapping insertFunctionMapping)
    {
      System.Data.Entity.Utilities.Check.NotNull<AssociationSet>(associationSet, nameof (associationSet));
      this._associationSet = associationSet;
      this._deleteFunctionMapping = deleteFunctionMapping;
      this._insertFunctionMapping = insertFunctionMapping;
    }

    /// <summary>Gets the association set.</summary>
    public AssociationSet AssociationSet => this._associationSet;

    /// <summary>Gets the delete function mapping.</summary>
    public ModificationFunctionMapping DeleteFunctionMapping => this._deleteFunctionMapping;

    /// <summary>Gets the insert function mapping.</summary>
    public ModificationFunctionMapping InsertFunctionMapping => this._insertFunctionMapping;

    /// <inheritdoc />
    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "AS{{{0}}}:{3}DFunc={{{1}}},{3}IFunc={{{2}}}", (object) this.AssociationSet, (object) this.DeleteFunctionMapping, (object) this.InsertFunctionMapping, (object) (Environment.NewLine + "  "));

    internal override void SetReadOnly()
    {
      MappingItem.SetReadOnly((MappingItem) this._deleteFunctionMapping);
      MappingItem.SetReadOnly((MappingItem) this._insertFunctionMapping);
      base.SetReadOnly();
    }
  }
}

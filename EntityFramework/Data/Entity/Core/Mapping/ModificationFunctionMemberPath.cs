// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ModificationFunctionMemberPath
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Describes the location of a member within an entity or association type structure.
  /// </summary>
  public sealed class ModificationFunctionMemberPath : MappingItem
  {
    private readonly ReadOnlyCollection<EdmMember> _members;
    private readonly AssociationSetEnd _associationSetEnd;

    /// <summary>
    /// Initializes a new ModificationFunctionMemberPath instance.
    /// </summary>
    /// <param name="members">Gets the members in the path from the leaf (the member being bound)
    /// to the root of the structure.</param>
    /// <param name="associationSet">Gets the association set to which we are navigating
    /// via this member. If the value is null, this is not a navigation member path.</param>
    public ModificationFunctionMemberPath(
      IEnumerable<EdmMember> members,
      AssociationSet associationSet)
    {
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<EdmMember>>(members, nameof (members));
      this._members = new ReadOnlyCollection<EdmMember>((IList<EdmMember>) new List<EdmMember>(members));
      if (associationSet == null)
        return;
      this._associationSetEnd = associationSet.AssociationSetEnds[this.Members[1].Name];
    }

    /// <summary>
    /// Gets the members in the path from the leaf (the member being bound)
    /// to the Root of the structure.
    /// </summary>
    public ReadOnlyCollection<EdmMember> Members => this._members;

    /// <summary>
    /// Gets the association set to which we are navigating via this member. If the value
    /// is null, this is not a navigation member path.
    /// </summary>
    public AssociationSetEnd AssociationSetEnd => this._associationSetEnd;

    /// <inheritdoc />
    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}{1}", this.AssociationSetEnd == null ? (object) string.Empty : (object) ("[" + this.AssociationSetEnd.ParentAssociationSet?.ToString() + "]"), (object) StringUtil.BuildDelimitedList<EdmMember>((IEnumerable<EdmMember>) this.Members, (StringUtil.ToStringConverter<EdmMember>) null, "."));
  }
}

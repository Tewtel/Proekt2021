// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.RowType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Objects.ELinq;
using System.Data.Entity.Resources;
using System.Text;
using System.Threading;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Represents the Edm Row Type</summary>
  public class RowType : StructuralType
  {
    private ReadOnlyMetadataCollection<EdmProperty> _properties;
    private readonly InitializerMetadata _initializerMetadata;

    internal RowType()
    {
    }

    internal RowType(IEnumerable<EdmProperty> properties)
      : this(properties, (InitializerMetadata) null)
    {
    }

    internal RowType(IEnumerable<EdmProperty> properties, InitializerMetadata initializerMetadata)
      : base(RowType.GetRowTypeIdentityFromProperties(RowType.CheckProperties(properties), initializerMetadata), "Transient", ~DataSpace.OSpace)
    {
      if (properties != null)
      {
        foreach (EdmProperty property in properties)
          this.AddProperty(property);
      }
      this._initializerMetadata = initializerMetadata;
      this.SetReadOnly();
    }

    internal InitializerMetadata InitializerMetadata => this._initializerMetadata;

    /// <summary>
    /// Gets the built-in type kind for this <see cref="T:System.Data.Entity.Core.Metadata.Edm.RowType" />.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Metadata.Edm.BuiltInTypeKind" /> object that represents the built-in type kind for this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.RowType" />
    /// .
    /// </returns>
    public override BuiltInTypeKind BuiltInTypeKind => BuiltInTypeKind.RowType;

    /// <summary>
    /// Gets the list of properties on this <see cref="T:System.Data.Entity.Core.Metadata.Edm.RowType" />.
    /// </summary>
    /// <returns>
    /// A collection of type <see cref="T:System.Data.Entity.Core.Metadata.Edm.ReadOnlyMetadataCollection`1" /> that contains the list of properties on this
    /// <see cref="T:System.Data.Entity.Core.Metadata.Edm.RowType" />
    /// .
    /// </returns>
    public virtual ReadOnlyMetadataCollection<EdmProperty> Properties
    {
      get
      {
        if (this._properties == null)
          Interlocked.CompareExchange<ReadOnlyMetadataCollection<EdmProperty>>(ref this._properties, (ReadOnlyMetadataCollection<EdmProperty>) new FilteredReadOnlyMetadataCollection<EdmProperty, EdmMember>(this.Members, new Predicate<EdmMember>(Helper.IsEdmProperty)), (ReadOnlyMetadataCollection<EdmProperty>) null);
        return this._properties;
      }
    }

    /// <summary>Gets a collection of the properties defined by the current type.</summary>
    /// <returns>A collection of the properties defined by the current type.</returns>
    public ReadOnlyMetadataCollection<EdmProperty> DeclaredProperties => this.GetDeclaredOnlyMembers<EdmProperty>();

    private void AddProperty(EdmProperty property)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmProperty>(property, nameof (property));
      this.AddMember((EdmMember) property);
    }

    internal override void ValidateMemberForAdd(EdmMember member)
    {
    }

    private static string GetRowTypeIdentityFromProperties(
      IEnumerable<EdmProperty> properties,
      InitializerMetadata initializerMetadata)
    {
      StringBuilder builder = new StringBuilder("rowtype[");
      if (properties != null)
      {
        int num = 0;
        foreach (EdmProperty property in properties)
        {
          if (num > 0)
            builder.Append(",");
          builder.Append("(");
          builder.Append(property.Name);
          builder.Append(",");
          property.TypeUsage.BuildIdentity(builder);
          builder.Append(")");
          ++num;
        }
      }
      builder.Append("]");
      if (initializerMetadata != null)
        builder.Append(",").Append(initializerMetadata.Identity);
      return builder.ToString();
    }

    private static IEnumerable<EdmProperty> CheckProperties(
      IEnumerable<EdmProperty> properties)
    {
      if (properties != null)
      {
        int num = 0;
        foreach (EdmProperty property in properties)
        {
          if (property == null)
            throw new ArgumentException(Strings.ADP_CollectionParameterElementIsNull((object) nameof (properties)));
          ++num;
        }
      }
      return properties;
    }

    internal override bool EdmEquals(MetadataItem item)
    {
      if (this == item)
        return true;
      if (item == null || BuiltInTypeKind.RowType != item.BuiltInTypeKind)
        return false;
      RowType rowType = (RowType) item;
      if (this.Members.Count != rowType.Members.Count)
        return false;
      for (int index = 0; index < this.Members.Count; ++index)
      {
        EdmMember member1 = this.Members[index];
        EdmMember member2 = rowType.Members[index];
        if (!member1.EdmEquals((MetadataItem) member2) || !member1.TypeUsage.EdmEquals((MetadataItem) member2.TypeUsage))
          return false;
      }
      return true;
    }

    /// <summary>
    /// The factory method for constructing the <see cref="T:System.Data.Entity.Core.Metadata.Edm.RowType" /> object.
    /// </summary>
    /// <param name="properties">Properties of the row type object.</param>
    /// <param name="metadataProperties">Metadata properties that will be added to the function. Can be null.</param>
    /// <returns>
    /// A new, read-only instance of the <see cref="T:System.Data.Entity.Core.Metadata.Edm.RowType" /> object.
    /// </returns>
    public static RowType Create(
      IEnumerable<EdmProperty> properties,
      IEnumerable<MetadataProperty> metadataProperties)
    {
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<EdmProperty>>(properties, nameof (properties));
      RowType rowType = new RowType(properties);
      if (metadataProperties != null)
        rowType.AddMetadataProperties(metadataProperties);
      rowType.SetReadOnly();
      return rowType;
    }
  }
}

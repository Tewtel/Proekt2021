// Decompiled with JetBrains decompiler
// Type: System.ComponentModel.DataAnnotations.Schema.IndexAttribute
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Infrastructure.Annotations;
using System.Runtime.CompilerServices;

namespace System.ComponentModel.DataAnnotations.Schema
{
  /// <summary>
  /// When this attribute is placed on a property it indicates that the database column to which the
  /// property is mapped has an index.
  /// </summary>
  /// <remarks>
  /// This attribute is used by Entity Framework Migrations to create indexes on mapped database columns.
  /// Multi-column indexes are created by using the same index name in multiple attributes. The information
  /// in these attributes is then merged together to specify the actual database index.
  /// </remarks>
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
  public class IndexAttribute : Attribute
  {
    private string _name;
    private int _order = -1;
    private bool? _isClustered;
    private bool? _isUnique;

    /// <summary>
    /// Creates a <see cref="T:System.ComponentModel.DataAnnotations.Schema.IndexAttribute" /> instance for an index that will be named by convention and
    /// has no column order, clustering, or uniqueness specified.
    /// </summary>
    public IndexAttribute()
    {
    }

    /// <summary>
    /// Creates a <see cref="T:System.ComponentModel.DataAnnotations.Schema.IndexAttribute" /> instance for an index with the given name and
    /// has no column order, clustering, or uniqueness specified.
    /// </summary>
    /// <param name="name">The index name.</param>
    public IndexAttribute(string name)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      this._name = name;
    }

    /// <summary>
    /// Creates a <see cref="T:System.ComponentModel.DataAnnotations.Schema.IndexAttribute" /> instance for an index with the given name and column order,
    /// but with no clustering or uniqueness specified.
    /// </summary>
    /// <remarks>
    /// Multi-column indexes are created by using the same index name in multiple attributes. The information
    /// in these attributes is then merged together to specify the actual database index.
    /// </remarks>
    /// <param name="name">The index name.</param>
    /// <param name="order">A number which will be used to determine column ordering for multi-column indexes.</param>
    public IndexAttribute(string name, int order)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      if (order < 0)
        throw new ArgumentOutOfRangeException(nameof (order));
      this._name = name;
      this._order = order;
    }

    internal IndexAttribute(string name, bool? isClustered, bool? isUnique)
    {
      this._name = name;
      this._isClustered = isClustered;
      this._isUnique = isUnique;
    }

    internal IndexAttribute(string name, int order, bool? isClustered, bool? isUnique)
    {
      this._name = name;
      this._order = order;
      this._isClustered = isClustered;
      this._isUnique = isUnique;
    }

    /// <summary>The index name.</summary>
    /// <remarks>
    /// Multi-column indexes are created by using the same index name in multiple attributes. The information
    /// in these attributes is then merged together to specify the actual database index.
    /// </remarks>
    public virtual string Name
    {
      get => this._name;
      internal set => this._name = value;
    }

    /// <summary>
    /// A number which will be used to determine column ordering for multi-column indexes. This will be -1 if no
    /// column order has been specified.
    /// </summary>
    /// <remarks>
    /// Multi-column indexes are created by using the same index name in multiple attributes. The information
    /// in these attributes is then merged together to specify the actual database index.
    /// </remarks>
    public virtual int Order
    {
      get => this._order;
      set => this._order = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }

    /// <summary>
    /// Set this property to true to define a clustered index. Set this property to false to define a
    /// non-clustered index.
    /// </summary>
    /// <remarks>
    /// The value of this property is only relevant if <see cref="P:System.ComponentModel.DataAnnotations.Schema.IndexAttribute.IsClusteredConfigured" /> returns true.
    /// If <see cref="P:System.ComponentModel.DataAnnotations.Schema.IndexAttribute.IsClusteredConfigured" /> returns false, then the value of this property is meaningless.
    /// </remarks>
    public virtual bool IsClustered
    {
      get => this._isClustered.HasValue && this._isClustered.Value;
      set => this._isClustered = new bool?(value);
    }

    /// <summary>
    /// Returns true if <see cref="P:System.ComponentModel.DataAnnotations.Schema.IndexAttribute.IsClustered" /> has been set to a value.
    /// </summary>
    public virtual bool IsClusteredConfigured => this._isClustered.HasValue;

    /// <summary>
    /// Set this property to true to define a unique index. Set this property to false to define a
    /// non-unique index.
    /// </summary>
    /// <remarks>
    /// The value of this property is only relevant if <see cref="P:System.ComponentModel.DataAnnotations.Schema.IndexAttribute.IsUniqueConfigured" /> returns true.
    /// If <see cref="P:System.ComponentModel.DataAnnotations.Schema.IndexAttribute.IsUniqueConfigured" /> returns false, then the value of this property is meaningless.
    /// </remarks>
    public virtual bool IsUnique
    {
      get => this._isUnique.HasValue && this._isUnique.Value;
      set => this._isUnique = new bool?(value);
    }

    /// <summary>
    /// Returns true if <see cref="P:System.ComponentModel.DataAnnotations.Schema.IndexAttribute.IsUnique" /> has been set to a value.
    /// </summary>
    public virtual bool IsUniqueConfigured => this._isUnique.HasValue;

    /// <summary>
    /// Returns a different ID for each object instance such that type descriptors won't
    /// attempt to combine all IndexAttribute instances into a single instance.
    /// </summary>
    public override object TypeId => (object) RuntimeHelpers.GetHashCode((object) this);

    /// <summary>
    /// Returns true if this attribute specifies the same name and configuration as the given attribute.
    /// </summary>
    /// <param name="other">The attribute to compare.</param>
    /// <returns>True if the other object is equal to this object; otherwise false.</returns>
    protected virtual bool Equals(IndexAttribute other) => this._name == other._name && this._order == other._order && this._isClustered.Equals((object) other._isClustered) && this._isUnique.Equals((object) other._isUnique);

    /// <inheritdoc />
    public override string ToString() => IndexAnnotationSerializer.SerializeIndexAttribute(this);

    /// <summary>
    /// Returns true if this attribute specifies the same name and configuration as the given attribute.
    /// </summary>
    /// <param name="obj">The attribute to compare.</param>
    /// <returns>True if the other object is equal to this object; otherwise false.</returns>
    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (this == obj)
        return true;
      return !(obj.GetType() != this.GetType()) && this.Equals((IndexAttribute) obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() => (((base.GetHashCode() * 397 ^ (this._name != null ? this._name.GetHashCode() : 0)) * 397 ^ this._order) * 397 ^ this._isClustered.GetHashCode()) * 397 ^ this._isUnique.GetHashCode();
  }
}

﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Annotations.IndexAnnotation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.Infrastructure.Annotations
{
  /// <summary>
  /// Instances of this class are used as custom annotations for representing database indexes in an
  /// Entity Framework model.
  /// </summary>
  /// <remarks>
  /// An index annotation is added to a Code First model when an <see cref="T:System.ComponentModel.DataAnnotations.Schema.IndexAttribute" /> is placed on
  /// a mapped property of that model. This is used by Entity Framework Migrations to create indexes on
  /// mapped database columns. Note that multiple index attributes on a property will be merged into a
  /// single annotation for the column. Similarly, index attributes on multiple properties that map to the
  /// same column will be merged into a single annotation for the column. This means that one index
  /// annotation can represent multiple indexes. Within an annotation there can be only one index with any
  /// given name.
  /// </remarks>
  public class IndexAnnotation : IMergeableAnnotation
  {
    /// <summary>
    /// The name used when this annotation is stored in Entity Framework metadata or serialized into
    /// an SSDL/EDMX file.
    /// </summary>
    public const string AnnotationName = "Index";
    private readonly IList<IndexAttribute> _indexes = (IList<IndexAttribute>) new List<IndexAttribute>();

    /// <summary>Creates a new annotation for the given index.</summary>
    /// <param name="indexAttribute">An index attributes representing an index.</param>
    public IndexAnnotation(IndexAttribute indexAttribute)
    {
      System.Data.Entity.Utilities.Check.NotNull<IndexAttribute>(indexAttribute, nameof (indexAttribute));
      this._indexes.Add(indexAttribute);
    }

    /// <summary>
    /// Creates a new annotation for the given collection of indexes.
    /// </summary>
    /// <param name="indexAttributes">Index attributes representing one or more indexes.</param>
    public IndexAnnotation(IEnumerable<IndexAttribute> indexAttributes)
    {
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<IndexAttribute>>(indexAttributes, nameof (indexAttributes));
      IndexAnnotation.MergeLists((ICollection<IndexAttribute>) this._indexes, indexAttributes, (PropertyInfo) null);
    }

    internal IndexAnnotation(PropertyInfo propertyInfo, IEnumerable<IndexAttribute> indexAttributes)
    {
      System.Data.Entity.Utilities.Check.NotNull<IEnumerable<IndexAttribute>>(indexAttributes, nameof (indexAttributes));
      IndexAnnotation.MergeLists((ICollection<IndexAttribute>) this._indexes, indexAttributes, propertyInfo);
    }

    private static void MergeLists(
      ICollection<IndexAttribute> existingIndexes,
      IEnumerable<IndexAttribute> newIndexes,
      PropertyInfo propertyInfo)
    {
      foreach (IndexAttribute newIndex in newIndexes)
      {
        IndexAttribute index = newIndex;
        if (index == null)
          throw new ArgumentNullException("indexAttribute");
        IndexAttribute other = existingIndexes.SingleOrDefault<IndexAttribute>((Func<IndexAttribute, bool>) (i => i.Name == index.Name));
        if (other == null)
        {
          existingIndexes.Add(index);
        }
        else
        {
          CompatibilityResult compatibilityResult = index.IsCompatibleWith(other);
          if ((bool) compatibilityResult)
          {
            existingIndexes.Remove(other);
            existingIndexes.Add(index.MergeWith(other));
          }
          else
          {
            string str = Environment.NewLine + "\t" + compatibilityResult.ErrorMessage;
            throw new InvalidOperationException(propertyInfo == (PropertyInfo) null ? System.Data.Entity.Resources.Strings.ConflictingIndexAttribute((object) other.Name, (object) str) : System.Data.Entity.Resources.Strings.ConflictingIndexAttributesOnProperty((object) propertyInfo.Name, (object) propertyInfo.ReflectedType.Name, (object) other.Name, (object) str));
          }
        }
      }
    }

    /// <summary>Gets the indexes represented by this annotation.</summary>
    public virtual IEnumerable<IndexAttribute> Indexes => (IEnumerable<IndexAttribute>) this._indexes;

    /// <summary>
    /// Returns true if this annotation does not conflict with the given annotation such that
    /// the two can be combined together using the <see cref="M:System.Data.Entity.Infrastructure.Annotations.IndexAnnotation.MergeWith(System.Object)" /> method.
    /// </summary>
    /// <remarks>
    /// Each index annotation contains at most one <see cref="T:System.ComponentModel.DataAnnotations.Schema.IndexAttribute" /> with a given name.
    /// Two annotations are considered compatible if each IndexAttribute with a given name is only
    /// contained in one annotation or the other, or if both annotations contain an IndexAttribute
    /// with the given name.
    /// </remarks>
    /// <param name="other">The annotation to compare.</param>
    /// <returns>A CompatibilityResult indicating whether or not this annotation is compatible with the other.</returns>
    public virtual CompatibilityResult IsCompatibleWith(object other)
    {
      if (this == other || other == null)
        return new CompatibilityResult(true, (string) null);
      if (!(other is IndexAnnotation indexAnnotation))
        return new CompatibilityResult(false, System.Data.Entity.Resources.Strings.IncompatibleTypes((object) other.GetType().Name, (object) typeof (IndexAnnotation).Name));
      foreach (IndexAttribute index in (IEnumerable<IndexAttribute>) indexAnnotation._indexes)
      {
        IndexAttribute newIndex = index;
        IndexAttribute me = this._indexes.SingleOrDefault<IndexAttribute>((Func<IndexAttribute, bool>) (i => i.Name == newIndex.Name));
        if (me != null)
        {
          CompatibilityResult compatibilityResult = me.IsCompatibleWith(newIndex);
          if (!(bool) compatibilityResult)
            return compatibilityResult;
        }
      }
      return new CompatibilityResult(true, (string) null);
    }

    /// <summary>
    /// Merges this annotation with the given annotation and returns a new annotation containing the merged indexes.
    /// </summary>
    /// <remarks>
    /// Each index annotation contains at most one <see cref="T:System.ComponentModel.DataAnnotations.Schema.IndexAttribute" /> with a given name.
    /// The merged annotation will contain IndexAttributes from both this and the other annotation.
    /// If both annotations contain an IndexAttribute with the same name, then the merged annotation
    /// will contain one IndexAttribute with that name.
    /// </remarks>
    /// <param name="other">The annotation to merge with this one.</param>
    /// <returns>A new annotation with indexes from both annotations merged.</returns>
    /// <exception cref="T:System.InvalidOperationException">
    /// The other annotation contains indexes that are not compatible with indexes in this annotation.
    /// </exception>
    public virtual object MergeWith(object other)
    {
      if (this == other || other == null)
        return (object) this;
      if (!(other is IndexAnnotation indexAnnotation))
        throw new ArgumentException(System.Data.Entity.Resources.Strings.IncompatibleTypes((object) other.GetType().Name, (object) typeof (IndexAnnotation).Name));
      List<IndexAttribute> list = this._indexes.ToList<IndexAttribute>();
      IndexAnnotation.MergeLists((ICollection<IndexAttribute>) list, (IEnumerable<IndexAttribute>) indexAnnotation._indexes, (PropertyInfo) null);
      return (object) new IndexAnnotation((IEnumerable<IndexAttribute>) list);
    }

    /// <inheritdoc />
    public override string ToString() => "IndexAnnotation: " + new IndexAnnotationSerializer().Serialize("Index", (object) this);
  }
}

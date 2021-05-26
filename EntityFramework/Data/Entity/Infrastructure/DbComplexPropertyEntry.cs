// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbComplexPropertyEntry
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Internal;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// A non-generic version of the <see cref="T:System.Data.Entity.Infrastructure.DbComplexPropertyEntry`2" /> class.
  /// </summary>
  public class DbComplexPropertyEntry : DbPropertyEntry
  {
    internal static DbComplexPropertyEntry Create(
      InternalPropertyEntry internalPropertyEntry)
    {
      return (DbComplexPropertyEntry) internalPropertyEntry.CreateDbMemberEntry();
    }

    internal DbComplexPropertyEntry(InternalPropertyEntry internalPropertyEntry)
      : base(internalPropertyEntry)
    {
    }

    /// <summary>
    /// Gets an object that represents a nested property of this property.
    /// This method can be used for both scalar or complex properties.
    /// </summary>
    /// <param name="propertyName"> The name of the nested property. </param>
    /// <returns> An object representing the nested property. </returns>
    public DbPropertyEntry Property(string propertyName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      return DbPropertyEntry.Create(((InternalPropertyEntry) this.InternalMemberEntry).Property(propertyName));
    }

    /// <summary>
    /// Gets an object that represents a nested complex property of this property.
    /// </summary>
    /// <param name="propertyName"> The name of the nested property. </param>
    /// <returns> An object representing the nested property. </returns>
    public DbComplexPropertyEntry ComplexProperty(string propertyName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(propertyName, nameof (propertyName));
      return DbComplexPropertyEntry.Create(((InternalPropertyEntry) this.InternalMemberEntry).Property(propertyName, requireComplex: true));
    }

    /// <summary>
    /// Returns the equivalent generic <see cref="T:System.Data.Entity.Infrastructure.DbComplexPropertyEntry`2" /> object.
    /// </summary>
    /// <typeparam name="TEntity"> The type of entity on which the member is declared. </typeparam>
    /// <typeparam name="TComplexProperty"> The type of the complex property. </typeparam>
    /// <returns> The equivalent generic object. </returns>
    public DbComplexPropertyEntry<TEntity, TComplexProperty> Cast<TEntity, TComplexProperty>() where TEntity : class
    {
      MemberEntryMetadata entryMetadata = this.InternalMemberEntry.EntryMetadata;
      if (!typeof (TEntity).IsAssignableFrom(entryMetadata.DeclaringType) || !typeof (TComplexProperty).IsAssignableFrom(entryMetadata.ElementType))
        throw Error.DbMember_BadTypeForCast((object) typeof (DbComplexPropertyEntry).Name, (object) typeof (TEntity).Name, (object) typeof (TComplexProperty).Name, (object) entryMetadata.DeclaringType.Name, (object) entryMetadata.MemberType.Name);
      return DbComplexPropertyEntry<TEntity, TComplexProperty>.Create((InternalPropertyEntry) this.InternalMemberEntry);
    }
  }
}

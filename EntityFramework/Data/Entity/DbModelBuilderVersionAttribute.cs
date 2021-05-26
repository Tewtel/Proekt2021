// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.DbModelBuilderVersionAttribute
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity
{
  /// <summary>
  /// This attribute can be applied to a class derived from <see cref="T:System.Data.Entity.DbContext" /> to set which
  /// version of the DbContext and <see cref="T:System.Data.Entity.DbModelBuilder" /> conventions should be used when building
  /// a model from code--also known as "Code First". See the <see cref="T:System.Data.Entity.DbModelBuilderVersion" />
  /// enumeration for details about DbModelBuilder versions.
  /// </summary>
  /// <remarks>
  /// If the attribute is missing from DbContextthen DbContext will always use the latest
  /// version of the conventions.  This is equivalent to using DbModelBuilderVersion.Latest.
  /// </remarks>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public sealed class DbModelBuilderVersionAttribute : Attribute
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.DbModelBuilderVersionAttribute" /> class.
    /// </summary>
    /// <param name="version">
    /// The <see cref="T:System.Data.Entity.DbModelBuilder" /> conventions version to use.
    /// </param>
    public DbModelBuilderVersionAttribute(DbModelBuilderVersion version) => this.Version = Enum.IsDefined(typeof (DbModelBuilderVersion), (object) version) ? version : throw new ArgumentOutOfRangeException(nameof (version));

    /// <summary>
    /// Gets the <see cref="T:System.Data.Entity.DbModelBuilder" /> conventions version.
    /// </summary>
    /// <value>
    /// The <see cref="T:System.Data.Entity.DbModelBuilder" /> conventions version.
    /// </value>
    public DbModelBuilderVersion Version { get; private set; }
  }
}

﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.DbModelBuilderVersion
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity
{
  /// <summary>
  /// A value from this enumeration can be provided directly to the <see cref="T:System.Data.Entity.DbModelBuilder" />
  /// class or can be used in the <see cref="T:System.Data.Entity.DbModelBuilderVersionAttribute" /> applied to
  /// a class derived from <see cref="T:System.Data.Entity.DbContext" />. The value used defines which version of
  /// the DbContext and DbModelBuilder conventions should be used when building a model from
  /// code--also known as "Code First".
  /// </summary>
  /// <remarks>
  /// Using DbModelBuilderVersion.Latest ensures that all the latest functionality is available
  /// when upgrading to a new release of the Entity Framework. However, it may result in an
  /// application behaving differently with the new release than it did with a previous release.
  /// This can be avoided by using a specific version of the conventions, but if a version
  /// other than the latest is set then not all the latest functionality will be available.
  /// </remarks>
  public enum DbModelBuilderVersion
  {
    /// <summary>
    /// Indicates that the latest version of the <see cref="T:System.Data.Entity.DbModelBuilder" /> and
    /// <see cref="T:System.Data.Entity.DbContext" /> conventions should be used.
    /// </summary>
    Latest,
    /// <summary>
    /// Indicates that the version of the <see cref="T:System.Data.Entity.DbModelBuilder" /> and
    /// <see cref="T:System.Data.Entity.DbContext" /> conventions shipped with Entity Framework v4.1
    /// should be used.
    /// </summary>
    V4_1,
    /// <summary>
    /// Indicates that the version of the <see cref="T:System.Data.Entity.DbModelBuilder" /> and
    /// <see cref="T:System.Data.Entity.DbContext" /> conventions shipped with Entity Framework v5.0
    /// when targeting .Net Framework 4 should be used.
    /// </summary>
    V5_0_Net4,
    /// <summary>
    /// Indicates that the version of the <see cref="T:System.Data.Entity.DbModelBuilder" /> and
    /// <see cref="T:System.Data.Entity.DbContext" /> conventions shipped with Entity Framework v5.0
    /// should be used.
    /// </summary>
    V5_0,
    /// <summary>
    /// Indicates that the version of the <see cref="T:System.Data.Entity.DbModelBuilder" /> and
    /// <see cref="T:System.Data.Entity.DbContext" /> conventions shipped with Entity Framework v6.0
    /// should be used.
    /// </summary>
    V6_0,
  }
}

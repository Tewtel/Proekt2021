// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.SuppressDbSetInitializationAttribute
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// This attribute can be applied to either an entire derived <see cref="T:System.Data.Entity.DbContext" /> class or to
  /// individual <see cref="T:System.Data.Entity.DbSet`1" /> or <see cref="T:System.Data.Entity.IDbSet`1" /> properties on that class.  When applied
  /// any discovered <see cref="T:System.Data.Entity.DbSet`1" /> or <see cref="T:System.Data.Entity.IDbSet`1" /> properties will still be included
  /// in the model but will not be automatically initialized.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false)]
  public sealed class SuppressDbSetInitializationAttribute : Attribute
  {
  }
}

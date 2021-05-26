// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.IStoreModelConvention`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// A convention that operates on the database section of the model after the model is created.
  /// </summary>
  /// <typeparam name="T">The type of metadata item that this convention operates on.</typeparam>
  public interface IStoreModelConvention<T> : IConvention where T : MetadataItem
  {
    /// <summary>Applies this convention to an item in the model.</summary>
    /// <param name="item">The item to apply the convention to.</param>
    /// <param name="model">The model.</param>
    void Apply(T item, DbModel model);
  }
}

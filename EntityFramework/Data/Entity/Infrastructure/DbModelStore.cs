// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DbModelStore
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Xml.Linq;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>Base class for persisted model cache.</summary>
  public abstract class DbModelStore
  {
    /// <summary>Loads a model from the store.</summary>
    /// <param name="contextType">The type of context representing the model.</param>
    /// <returns>The loaded metadata model.</returns>
    public abstract DbCompiledModel TryLoad(Type contextType);

    /// <summary>
    /// Retrieves an edmx XDocument version of the model from the store.
    /// </summary>
    /// <param name="contextType">The type of context representing the model.</param>
    /// <returns>The loaded XDocument edmx.</returns>
    public abstract XDocument TryGetEdmx(Type contextType);

    /// <summary>Saves a model to the store.</summary>
    /// <param name="contextType">The type of context representing the model.</param>
    /// <param name="model">The metadata model to save.</param>
    public abstract void Save(Type contextType, DbModel model);

    /// <summary>Gets the default database schema used by a model.</summary>
    /// <param name="contextType">The type of context representing the model.</param>
    /// <returns>The default database schema.</returns>
    protected virtual string GetDefaultSchema(Type contextType) => "dbo";
  }
}

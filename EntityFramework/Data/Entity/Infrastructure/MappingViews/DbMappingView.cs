// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.MappingViews.DbMappingView
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.MappingViews
{
  /// <summary>Represents a mapping view.</summary>
  public class DbMappingView
  {
    private readonly string _entitySql;

    /// <summary>
    /// Creates a <see cref="T:System.Data.Entity.Infrastructure.MappingViews.DbMappingView" /> instance having the specified entity SQL.
    /// </summary>
    /// <param name="entitySql">A string that specifies the entity SQL.</param>
    public DbMappingView(string entitySql)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(entitySql, nameof (entitySql));
      this._entitySql = entitySql;
    }

    /// <summary>Gets the entity SQL.</summary>
    public string EntitySql => this._entitySql;
  }
}

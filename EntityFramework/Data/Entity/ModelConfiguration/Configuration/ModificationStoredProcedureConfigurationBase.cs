﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.ModificationStoredProcedureConfigurationBase
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Performs configuration of a stored procedure uses to modify an entity in the database.
  /// </summary>
  public abstract class ModificationStoredProcedureConfigurationBase
  {
    private readonly ModificationStoredProcedureConfiguration _configuration = new ModificationStoredProcedureConfiguration();

    internal ModificationStoredProcedureConfigurationBase()
    {
    }

    internal ModificationStoredProcedureConfiguration Configuration => this._configuration;
  }
}

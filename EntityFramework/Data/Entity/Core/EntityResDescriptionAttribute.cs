﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.EntityResDescriptionAttribute
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core
{
  [AttributeUsage(AttributeTargets.All)]
  internal sealed class EntityResDescriptionAttribute : DescriptionAttribute
  {
    private bool _replaced;

    public override string Description
    {
      get
      {
        if (!this._replaced)
        {
          this._replaced = true;
          this.DescriptionValue = EntityRes.GetString(base.Description);
        }
        return base.Description;
      }
    }

    public EntityResDescriptionAttribute(string description)
      : base(description)
    {
    }
  }
}

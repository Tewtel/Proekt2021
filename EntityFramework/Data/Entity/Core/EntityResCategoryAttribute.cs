// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.EntityResCategoryAttribute
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core
{
  [AttributeUsage(AttributeTargets.All)]
  internal sealed class EntityResCategoryAttribute : CategoryAttribute
  {
    public EntityResCategoryAttribute(string category)
      : base(category)
    {
    }

    protected override string GetLocalizedString(string value) => EntityRes.GetString(value);
  }
}

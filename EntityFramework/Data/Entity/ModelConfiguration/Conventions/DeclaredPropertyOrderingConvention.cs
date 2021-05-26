// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.DeclaredPropertyOrderingConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Data.Entity.ModelConfiguration.Mappers;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to move primary key properties to appear first.
  /// </summary>
  public class DeclaredPropertyOrderingConvention : 
    IConceptualModelConvention<EntityType>,
    IConvention
  {
    /// <inheritdoc />
    public virtual void Apply(EntityType item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityType>(item, nameof (item));
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      if (item.BaseType != null)
        return;
      foreach (EdmProperty keyProperty in item.KeyProperties)
      {
        item.RemoveMember((EdmMember) keyProperty);
        item.AddKeyMember((EdmMember) keyProperty);
      }
      foreach (PropertyInfo property in new PropertyFilter().GetProperties(EntityTypeExtensions.GetClrType(item), false, includePrivate: true))
      {
        PropertyInfo p = property;
        EdmProperty edmProperty = item.DeclaredProperties.SingleOrDefault<EdmProperty>((Func<EdmProperty, bool>) (ep => ep.Name == p.Name));
        if (edmProperty != null && !item.KeyProperties.Contains(edmProperty))
        {
          item.RemoveMember((EdmMember) edmProperty);
          item.AddMember((EdmMember) edmProperty);
        }
      }
    }
  }
}

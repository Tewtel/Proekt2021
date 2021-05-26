// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation;
using System.Data.Entity.ModelConfiguration.Edm;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// Convention to enable cascade delete for any required relationships.
  /// </summary>
  public class OneToManyCascadeDeleteConvention : 
    IConceptualModelConvention<AssociationType>,
    IConvention
  {
    /// <inheritdoc />
    public virtual void Apply(AssociationType item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<AssociationType>(item, nameof (item));
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      if (item.IsSelfReferencing() || item.GetConfiguration() is NavigationPropertyConfiguration configuration && configuration.DeleteAction.HasValue)
        return;
      AssociationEndMember associationEndMember = (AssociationEndMember) null;
      if (item.IsRequiredToMany())
        associationEndMember = item.SourceEnd;
      else if (item.IsManyToRequired())
        associationEndMember = item.TargetEnd;
      if (associationEndMember == null)
        return;
      associationEndMember.DeleteBehavior = OperationAction.Cascade;
    }
  }
}

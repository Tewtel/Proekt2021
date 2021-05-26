// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.ForeignKeyIndexConvention
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>Convention to introduce indexes for foreign keys.</summary>
  public class ForeignKeyIndexConvention : IStoreModelConvention<AssociationType>, IConvention
  {
    /// <inheritdoc />
    public virtual void Apply(AssociationType item, DbModel model)
    {
      System.Data.Entity.Utilities.Check.NotNull<AssociationType>(item, nameof (item));
      if (item.Constraint == null)
        return;
      IEnumerable<ConsolidatedIndex> source = ConsolidatedIndex.BuildIndexes(item.Name, item.Constraint.ToProperties.Select<EdmProperty, Tuple<string, EdmProperty>>((Func<EdmProperty, Tuple<string, EdmProperty>>) (p => Tuple.Create<string, EdmProperty>(p.Name, p))));
      IEnumerable<string> dependentColumnNames = item.Constraint.ToProperties.Select<EdmProperty, string>((Func<EdmProperty, string>) (p => p.Name));
      Func<ConsolidatedIndex, bool> predicate = (Func<ConsolidatedIndex, bool>) (c => c.Columns.SequenceEqual<string>(dependentColumnNames));
      if (source.Any<ConsolidatedIndex>(predicate))
        return;
      string name = IndexOperation.BuildDefaultName(dependentColumnNames);
      int num = 0;
      foreach (EdmProperty toProperty in item.Constraint.ToProperties)
      {
        IndexAnnotation indexAnnotation = new IndexAnnotation(new IndexAttribute(name, num++));
        object annotation = toProperty.Annotations.GetAnnotation("http://schemas.microsoft.com/ado/2013/11/edm/customannotation:Index");
        if (annotation != null)
          indexAnnotation = (IndexAnnotation) ((IndexAnnotation) annotation).MergeWith((object) indexAnnotation);
        toProperty.AddAnnotation("http://schemas.microsoft.com/ado/2013/11/edm/customannotation:Index", (object) indexAnnotation);
      }
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Conventions.AttributeToTableAnnotationConvention`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Utilities;
using System.Linq;

namespace System.Data.Entity.ModelConfiguration.Conventions
{
  /// <summary>
  /// A general purpose class for Code First conventions that read attributes from .NET types
  /// and generate table annotations based on those attributes.
  /// </summary>
  /// <typeparam name="TAttribute">The type of attribute to discover.</typeparam>
  /// <typeparam name="TAnnotation">The type of annotation that will be created.</typeparam>
  public class AttributeToTableAnnotationConvention<TAttribute, TAnnotation> : Convention where TAttribute : Attribute
  {
    /// <summary>
    /// Constructs a convention that will create table annotations with the given name and
    /// using the given factory delegate.
    /// </summary>
    /// <param name="annotationName">The name of the annotations to create.</param>
    /// <param name="annotationFactory">A factory for creating the annotation on each table.</param>
    public AttributeToTableAnnotationConvention(
      string annotationName,
      Func<Type, IList<TAttribute>, TAnnotation> annotationFactory)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(annotationName, nameof (annotationName));
      System.Data.Entity.Utilities.Check.NotNull<Func<Type, IList<TAttribute>, TAnnotation>>(annotationFactory, nameof (annotationFactory));
      AttributeProvider attributeProvider = DbConfiguration.DependencyResolver.GetService<AttributeProvider>();
      this.Types().Having<List<TAttribute>>((Func<Type, List<TAttribute>>) (t => attributeProvider.GetAttributes(t).OfType<TAttribute>().ToList<TAttribute>())).Configure((Action<ConventionTypeConfiguration, List<TAttribute>>) ((c, a) =>
      {
        if (!a.Any<TAttribute>())
          return;
        c.HasTableAnnotation(annotationName, (object) annotationFactory(c.ClrType, (IList<TAttribute>) a));
      }));
    }
  }
}

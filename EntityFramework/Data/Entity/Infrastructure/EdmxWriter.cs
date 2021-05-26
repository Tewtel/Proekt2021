// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.EdmxWriter
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Infrastructure.DependencyResolution;
using System.Data.Entity.Internal;
using System.Data.Entity.ModelConfiguration.Edm.Serialization;
using System.Data.Entity.Resources;
using System.Xml;
using System.Xml.Linq;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Contains methods used to access the Entity Data Model created by Code First in the EDMX form.
  /// These methods are typically used for debugging when there is a need to look at the model that
  /// Code First creates internally.
  /// </summary>
  public static class EdmxWriter
  {
    /// <summary>
    /// Uses Code First with the given context and writes the resulting Entity Data Model to the given
    /// writer in EDMX form.  This method can only be used with context instances that use Code First
    /// and create the model internally.  The method cannot be used for contexts created using Database
    /// First or Model First, for contexts created using a pre-existing <see cref="T:System.Data.Entity.Core.Objects.ObjectContext" />, or
    /// for contexts created using a pre-existing <see cref="T:System.Data.Entity.Infrastructure.DbCompiledModel" />.
    /// </summary>
    /// <param name="context"> The context. </param>
    /// <param name="writer"> The writer. </param>
    public static void WriteEdmx(DbContext context, XmlWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbContext>(context, nameof (context));
      System.Data.Entity.Utilities.Check.NotNull<XmlWriter>(writer, nameof (writer));
      InternalContext internalContext = context.InternalContext;
      DbModel model = !(internalContext is EagerInternalContext) ? internalContext.ModelBeingInitialized : throw Error.EdmxWriter_EdmxFromObjectContextNotSupported();
      if (model != null)
      {
        EdmxWriter.WriteEdmx(model, writer);
      }
      else
      {
        DbCompiledModel codeFirstModel = internalContext.CodeFirstModel;
        if (codeFirstModel == null)
          throw Error.EdmxWriter_EdmxFromModelFirstNotSupported();
        DbModelStore service = DbConfiguration.DependencyResolver.GetService<DbModelStore>();
        if (service != null)
        {
          XDocument edmx = service.TryGetEdmx(context.GetType());
          if (edmx != null)
          {
            edmx.WriteTo(writer);
            return;
          }
        }
        DbModelBuilder dbModelBuilder = (codeFirstModel.CachedModelBuilder ?? throw Error.EdmxWriter_EdmxFromRawCompiledModelNotSupported()).Clone();
        EdmxWriter.WriteEdmx(internalContext.ModelProviderInfo == null ? dbModelBuilder.Build(internalContext.Connection) : dbModelBuilder.Build(internalContext.ModelProviderInfo), writer);
      }
    }

    /// <summary>
    /// Writes the Entity Data Model represented by the given <see cref="T:System.Data.Entity.Infrastructure.DbModel" /> to the
    /// given writer in EDMX form.
    /// </summary>
    /// <param name="model"> An object representing the EDM. </param>
    /// <param name="writer"> The writer. </param>
    public static void WriteEdmx(DbModel model, XmlWriter writer)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbModel>(model, nameof (model));
      System.Data.Entity.Utilities.Check.NotNull<XmlWriter>(writer, nameof (writer));
      new EdmxSerializer().Serialize(model.DatabaseMapping, writer);
    }
  }
}

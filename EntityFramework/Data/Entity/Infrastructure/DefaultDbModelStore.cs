// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DefaultDbModelStore
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Loads or saves models from/into .edmx files at a specified location.
  /// </summary>
  public class DefaultDbModelStore : DbModelStore
  {
    private const string FileExtension = ".edmx";
    private readonly string _directory;

    /// <summary>Initializes a new DefaultDbModelStore instance.</summary>
    /// <param name="directory">The parent directory for the .edmx files.</param>
    public DefaultDbModelStore(string directory)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(directory, nameof (directory));
      this._directory = directory;
    }

    /// <summary>Gets the location of the .edmx files.</summary>
    public string Directory => this._directory;

    /// <summary>Loads a model from the store.</summary>
    /// <param name="contextType">The type of context representing the model.</param>
    /// <returns>The loaded metadata model.</returns>
    public override DbCompiledModel TryLoad(Type contextType) => this.LoadXml<DbCompiledModel>(contextType, (Func<XmlReader, DbCompiledModel>) (reader =>
    {
      string defaultSchema = this.GetDefaultSchema(contextType);
      return EdmxReader.Read(reader, defaultSchema);
    }));

    /// <summary>
    /// Retrieves an edmx XDocument version of the model from the store.
    /// </summary>
    /// <param name="contextType">The type of context representing the model.</param>
    /// <returns>The loaded XDocument edmx.</returns>
    public override XDocument TryGetEdmx(Type contextType) => this.LoadXml<XDocument>(contextType, new Func<XmlReader, XDocument>(XDocument.Load));

    internal T LoadXml<T>(Type contextType, Func<XmlReader, T> xmlReaderDelegate)
    {
      string filePath = this.GetFilePath(contextType);
      if (!File.Exists(filePath))
        return default (T);
      if (!this.FileIsValid(contextType, filePath))
      {
        File.Delete(filePath);
        return default (T);
      }
      using (XmlReader xmlReader = XmlReader.Create(filePath))
        return xmlReaderDelegate(xmlReader);
    }

    /// <summary>Saves a model to the store.</summary>
    /// <param name="contextType">The type of context representing the model.</param>
    /// <param name="model">The metadata model to save.</param>
    public override void Save(Type contextType, DbModel model)
    {
      string filePath = this.GetFilePath(contextType);
      using (XmlWriter writer = XmlWriter.Create(filePath, new XmlWriterSettings()
      {
        Indent = true
      }))
        EdmxWriter.WriteEdmx(model, writer);
    }

    /// <summary>
    /// Gets the path of the .edmx file corresponding to the specified context type.
    /// </summary>
    /// <param name="contextType">A context type.</param>
    /// <returns>The .edmx file path.</returns>
    protected virtual string GetFilePath(Type contextType) => Path.Combine(this._directory, contextType.FullName + ".edmx");

    /// <summary>
    /// Validates the model store is valid.
    /// The default implementation verifies that the .edmx file was last
    /// written after the context assembly was last written.
    /// </summary>
    /// <param name="contextType">The type of context representing the model.</param>
    /// <param name="filePath">The path of the stored model.</param>
    /// <returns>Whether the edmx file should be invalidated.</returns>
    protected virtual bool FileIsValid(Type contextType, string filePath)
    {
      DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(contextType.Assembly.Location);
      return File.GetLastWriteTimeUtc(filePath) >= lastWriteTimeUtc;
    }
  }
}

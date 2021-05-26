// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.ModelHashCalculator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace System.Data.Entity.Internal
{
  internal class ModelHashCalculator
  {
    public virtual string Calculate(DbCompiledModel compiledModel)
    {
      DbProviderInfo providerInfo = compiledModel.ProviderInfo;
      DbModelBuilder dbModelBuilder = compiledModel.CachedModelBuilder.Clone();
      EdmMetadataContext.ConfigureEdmMetadata(dbModelBuilder.ModelConfiguration);
      EdmModel database = dbModelBuilder.Build(providerInfo).DatabaseMapping.Database;
      database.SchemaVersion = 2.0;
      StringBuilder stringBuilder = new StringBuilder();
      StringBuilder output = stringBuilder;
      using (XmlWriter xmlWriter = XmlWriter.Create(output, new XmlWriterSettings()
      {
        Indent = true
      }))
        new SsdlSerializer().Serialize(database, providerInfo.ProviderInvariantName, providerInfo.ProviderManifestToken, xmlWriter);
      return ModelHashCalculator.ComputeSha256Hash(stringBuilder.ToString());
    }

    private static string ComputeSha256Hash(string input)
    {
      byte[] hash = ModelHashCalculator.GetSha256HashAlgorithm().ComputeHash(Encoding.ASCII.GetBytes(input));
      StringBuilder stringBuilder = new StringBuilder(hash.Length * 2);
      foreach (byte num in hash)
        stringBuilder.Append(num.ToString("X2", (IFormatProvider) CultureInfo.InvariantCulture));
      return stringBuilder.ToString();
    }

    private static SHA256 GetSha256HashAlgorithm()
    {
      try
      {
        return (SHA256) new SHA256CryptoServiceProvider();
      }
      catch (PlatformNotSupportedException ex)
      {
        return (SHA256) new SHA256Managed();
      }
    }
  }
}

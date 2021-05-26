// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.CompressingHashBuilder
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;

namespace System.Data.Entity.Core.Mapping
{
  internal class CompressingHashBuilder : StringHashBuilder
  {
    private const int HashCharacterCompressionThreshold = 2048;
    private const int SpacesPerIndent = 4;
    private int _indent;
    private static readonly Dictionary<Type, string> _legacyTypeNames = CompressingHashBuilder.InitializeLegacyTypeNames();

    internal CompressingHashBuilder(HashAlgorithm hashAlgorithm)
      : base(hashAlgorithm, 6144)
    {
    }

    internal override void Append(string content)
    {
      base.Append(string.Empty.PadLeft(4 * this._indent, ' '));
      base.Append(content);
      this.CompressHash();
    }

    internal override void AppendLine(string content)
    {
      base.Append(string.Empty.PadLeft(4 * this._indent, ' '));
      base.AppendLine(content);
      this.CompressHash();
    }

    private static Dictionary<Type, string> InitializeLegacyTypeNames() => new Dictionary<Type, string>()
    {
      {
        typeof (AssociationSetMapping),
        "System.Data.Entity.Core.Mapping.StorageAssociationSetMapping"
      },
      {
        typeof (AssociationSetModificationFunctionMapping),
        "System.Data.Entity.Core.Mapping.StorageAssociationSetModificationFunctionMapping"
      },
      {
        typeof (AssociationTypeMapping),
        "System.Data.Entity.Core.Mapping.StorageAssociationTypeMapping"
      },
      {
        typeof (ComplexPropertyMapping),
        "System.Data.Entity.Core.Mapping.StorageComplexPropertyMapping"
      },
      {
        typeof (ComplexTypeMapping),
        "System.Data.Entity.Core.Mapping.StorageComplexTypeMapping"
      },
      {
        typeof (ConditionPropertyMapping),
        "System.Data.Entity.Core.Mapping.StorageConditionPropertyMapping"
      },
      {
        typeof (EndPropertyMapping),
        "System.Data.Entity.Core.Mapping.StorageEndPropertyMapping"
      },
      {
        typeof (EntityContainerMapping),
        "System.Data.Entity.Core.Mapping.StorageEntityContainerMapping"
      },
      {
        typeof (EntitySetMapping),
        "System.Data.Entity.Core.Mapping.StorageEntitySetMapping"
      },
      {
        typeof (EntityTypeMapping),
        "System.Data.Entity.Core.Mapping.StorageEntityTypeMapping"
      },
      {
        typeof (EntityTypeModificationFunctionMapping),
        "System.Data.Entity.Core.Mapping.StorageEntityTypeModificationFunctionMapping"
      },
      {
        typeof (MappingFragment),
        "System.Data.Entity.Core.Mapping.StorageMappingFragment"
      },
      {
        typeof (ModificationFunctionMapping),
        "System.Data.Entity.Core.Mapping.StorageModificationFunctionMapping"
      },
      {
        typeof (ModificationFunctionMemberPath),
        "System.Data.Entity.Core.Mapping.StorageModificationFunctionMemberPath"
      },
      {
        typeof (ModificationFunctionParameterBinding),
        "System.Data.Entity.Core.Mapping.StorageModificationFunctionParameterBinding"
      },
      {
        typeof (ModificationFunctionResultBinding),
        "System.Data.Entity.Core.Mapping.StorageModificationFunctionResultBinding"
      },
      {
        typeof (PropertyMapping),
        "System.Data.Entity.Core.Mapping.StoragePropertyMapping"
      },
      {
        typeof (ScalarPropertyMapping),
        "System.Data.Entity.Core.Mapping.StorageScalarPropertyMapping"
      },
      {
        typeof (EntitySetBaseMapping),
        "System.Data.Entity.Core.Mapping.StorageSetMapping"
      },
      {
        typeof (TypeMapping),
        "System.Data.Entity.Core.Mapping.StorageTypeMapping"
      }
    };

    internal void AppendObjectStartDump(object o, int objectIndex)
    {
      base.Append(string.Empty.PadLeft(4 * this._indent, ' '));
      string s;
      if (!CompressingHashBuilder._legacyTypeNames.TryGetValue(o.GetType(), out s))
        s = o.GetType().ToString();
      base.Append(s);
      base.Append(" Instance#");
      base.AppendLine(objectIndex.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      this.CompressHash();
      ++this._indent;
    }

    internal void AppendObjectEndDump() => --this._indent;

    private void CompressHash()
    {
      if (this.CharCount < 2048)
        return;
      string hash = this.ComputeHash();
      this.Clear();
      base.Append(hash);
    }
  }
}

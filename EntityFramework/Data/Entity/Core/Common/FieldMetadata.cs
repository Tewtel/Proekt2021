// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.FieldMetadata
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Core.Common
{
  /// <summary>
  /// FieldMetadata class providing the correlation between the column ordinals and MemberMetadata.
  /// </summary>
  public struct FieldMetadata
  {
    private readonly EdmMember _fieldType;
    private readonly int _ordinal;

    /// <summary>
    /// Initializes a new <see cref="T:System.Data.Entity.Core.Common.FieldMetadata" /> object with the specified ordinal value and field type.
    /// </summary>
    /// <param name="ordinal">An integer specified the location of the metadata.</param>
    /// <param name="fieldType">The field type.</param>
    public FieldMetadata(int ordinal, EdmMember fieldType)
    {
      if (ordinal < 0)
        throw new ArgumentOutOfRangeException(nameof (ordinal));
      System.Data.Entity.Utilities.Check.NotNull<EdmMember>(fieldType, nameof (fieldType));
      this._fieldType = fieldType;
      this._ordinal = ordinal;
    }

    /// <summary>
    /// Gets the type of field for this <see cref="T:System.Data.Entity.Core.Common.FieldMetadata" /> object.
    /// </summary>
    /// <returns>
    /// The type of field for this <see cref="T:System.Data.Entity.Core.Common.FieldMetadata" /> object.
    /// </returns>
    public EdmMember FieldType => this._fieldType;

    /// <summary>
    /// Gets the ordinal for this <see cref="T:System.Data.Entity.Core.Common.FieldMetadata" /> object.
    /// </summary>
    /// <returns>An integer representing the ordinal value.</returns>
    public int Ordinal => this._ordinal;
  }
}

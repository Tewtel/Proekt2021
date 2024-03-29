﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.EnumType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm.Provider;
using System.Data.Entity.Utilities;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.Core.Metadata.Edm
{
  /// <summary>Represents an enumeration type.</summary>
  public class EnumType : SimpleType
  {
    private readonly ReadOnlyMetadataCollection<EnumMember> _members = new ReadOnlyMetadataCollection<EnumMember>(new MetadataCollection<EnumMember>());
    private PrimitiveType _underlyingType;
    private bool _isFlags;

    internal EnumType()
    {
      this._underlyingType = PrimitiveType.GetEdmPrimitiveType(PrimitiveTypeKind.Int32);
      this._isFlags = false;
    }

    internal EnumType(
      string name,
      string namespaceName,
      PrimitiveType underlyingType,
      bool isFlags,
      DataSpace dataSpace)
      : base(name, namespaceName, dataSpace)
    {
      this._isFlags = isFlags;
      this._underlyingType = underlyingType;
    }

    internal EnumType(Type clrType)
      : base(clrType.Name, clrType.NestingNamespace() ?? string.Empty, DataSpace.OSpace)
    {
      ClrProviderManifest.Instance.TryGetPrimitiveType(clrType.GetEnumUnderlyingType(), out this._underlyingType);
      this._isFlags = clrType.GetCustomAttributes<FlagsAttribute>(false).Any<FlagsAttribute>();
      foreach (string name in Enum.GetNames(clrType))
        this.AddMember(new EnumMember(name, Convert.ChangeType(Enum.Parse(clrType, name), clrType.GetEnumUnderlyingType(), (IFormatProvider) CultureInfo.InvariantCulture)));
    }

    /// <summary> Returns the kind of the type </summary>
    public override BuiltInTypeKind BuiltInTypeKind => BuiltInTypeKind.EnumType;

    /// <summary> Gets a collection of enumeration members for this enumeration type. </summary>
    [MetadataProperty(BuiltInTypeKind.EnumMember, true)]
    public ReadOnlyMetadataCollection<EnumMember> Members => this._members;

    /// <summary> Gets a value indicating whether the enum type is defined as flags (i.e. can be treated as a bit field) </summary>
    [MetadataProperty(PrimitiveTypeKind.Boolean, false)]
    public bool IsFlags
    {
      get => this._isFlags;
      internal set
      {
        Util.ThrowIfReadOnly((MetadataItem) this);
        this._isFlags = value;
      }
    }

    /// <summary> Gets the underlying type for this enumeration type. </summary>
    [MetadataProperty(BuiltInTypeKind.PrimitiveType, false)]
    public PrimitiveType UnderlyingType
    {
      get => this._underlyingType;
      internal set
      {
        Util.ThrowIfReadOnly((MetadataItem) this);
        this._underlyingType = value;
      }
    }

    internal override void SetReadOnly()
    {
      if (this.IsReadOnly)
        return;
      base.SetReadOnly();
      this.Members.Source.SetReadOnly();
    }

    internal void AddMember(EnumMember enumMember) => this.Members.Source.Add(enumMember);

    /// <summary>Creates a read-only EnumType instance.</summary>
    /// <param name="name">The name of the enumeration type.</param>
    /// <param name="namespaceName">The namespace of the enumeration type.</param>
    /// <param name="underlyingType">The underlying type of the enumeration type.</param>
    /// <param name="isFlags">Indicates whether the enumeration type can be treated as a bit field; that is, a set of flags.</param>
    /// <param name="members">The members of the enumeration type.</param>
    /// <param name="metadataProperties">Metadata properties to be associated with the enumeration type.</param>
    /// <returns>The newly created EnumType instance.</returns>
    /// <exception cref="T:System.ArgumentNullException">underlyingType is null.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// name is null or empty.
    /// -or-
    /// namespaceName is null or empty.
    /// -or-
    /// underlyingType is not a supported underlying type.
    /// -or-
    /// The specified members do not have unique names.
    /// -or-
    /// The value of a specified member is not in the range of the underlying type.
    /// </exception>
    public static EnumType Create(
      string name,
      string namespaceName,
      PrimitiveType underlyingType,
      bool isFlags,
      IEnumerable<EnumMember> members,
      IEnumerable<MetadataProperty> metadataProperties)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(name, nameof (name));
      System.Data.Entity.Utilities.Check.NotEmpty(namespaceName, nameof (namespaceName));
      System.Data.Entity.Utilities.Check.NotNull<PrimitiveType>(underlyingType, nameof (underlyingType));
      if (!Helper.IsSupportedEnumUnderlyingType(underlyingType.PrimitiveTypeKind))
        throw new ArgumentException(System.Data.Entity.Resources.Strings.InvalidEnumUnderlyingType, nameof (underlyingType));
      EnumType enumType = new EnumType(name, namespaceName, underlyingType, isFlags, DataSpace.CSpace);
      if (members != null)
      {
        foreach (EnumMember member in members)
        {
          if (!Helper.IsEnumMemberValueInRange(underlyingType.PrimitiveTypeKind, Convert.ToInt64(member.Value, (IFormatProvider) CultureInfo.InvariantCulture)))
            throw new ArgumentException(System.Data.Entity.Resources.Strings.EnumMemberValueOutOfItsUnderylingTypeRange(member.Value, (object) member.Name, (object) underlyingType.Name), nameof (members));
          enumType.AddMember(member);
        }
      }
      if (metadataProperties != null)
        enumType.AddMetadataProperties(metadataProperties);
      enumType.SetReadOnly();
      return enumType;
    }
  }
}

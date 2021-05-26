// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.EntitySql.MetadataEnumMember
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Common.EntitySql
{
  internal sealed class MetadataEnumMember : MetadataMember
  {
    internal readonly TypeUsage EnumType;
    internal readonly EnumMember EnumMember;

    internal MetadataEnumMember(string name, TypeUsage enumType, EnumMember enumMember)
      : base(MetadataMemberClass.EnumMember, name)
    {
      this.EnumType = enumType;
      this.EnumMember = enumMember;
    }

    internal override string MetadataMemberClassName => MetadataEnumMember.EnumMemberClassName;

    internal static string EnumMemberClassName => Strings.LocalizedEnumMember;
  }
}

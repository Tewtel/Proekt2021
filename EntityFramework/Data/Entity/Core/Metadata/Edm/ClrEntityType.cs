// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ClrEntityType
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Utilities;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal sealed class ClrEntityType : EntityType
  {
    private readonly Type _type;
    private Func<object> _constructor;
    private readonly string _cspaceTypeName;
    private readonly string _cspaceNamespaceName;
    private string _hash;

    internal ClrEntityType(Type type, string cspaceNamespaceName, string cspaceTypeName)
      : base(System.Data.Entity.Utilities.Check.NotNull<Type>(type, nameof (type)).Name, type.NestingNamespace() ?? string.Empty, DataSpace.OSpace)
    {
      this._type = type;
      this._cspaceNamespaceName = cspaceNamespaceName;
      this._cspaceTypeName = cspaceNamespaceName + "." + cspaceTypeName;
      this.Abstract = type.IsAbstract();
    }

    internal Func<object> Constructor
    {
      get => this._constructor;
      set => Interlocked.CompareExchange<Func<object>>(ref this._constructor, value, (Func<object>) null);
    }

    internal override Type ClrType => this._type;

    internal string CSpaceTypeName => this._cspaceTypeName;

    internal string CSpaceNamespaceName => this._cspaceNamespaceName;

    internal string HashedDescription
    {
      get
      {
        if (this._hash == null)
          Interlocked.CompareExchange<string>(ref this._hash, this.BuildEntityTypeHash(), (string) null);
        return this._hash;
      }
    }

    private string BuildEntityTypeHash()
    {
      using (SHA256 a256HashAlgorithm = MetadataHelper.CreateSHA256HashAlgorithm())
      {
        byte[] hash = a256HashAlgorithm.ComputeHash(Encoding.ASCII.GetBytes(this.BuildEntityTypeDescription()));
        StringBuilder stringBuilder = new StringBuilder(hash.Length * 2);
        foreach (byte num in hash)
          stringBuilder.Append(num.ToString("X2", (IFormatProvider) CultureInfo.InvariantCulture));
        return stringBuilder.ToString();
      }
    }

    private string BuildEntityTypeDescription()
    {
      StringBuilder stringBuilder = new StringBuilder(512);
      stringBuilder.Append("CLR:").Append(this.ClrType.FullName);
      stringBuilder.Append("Conceptual:").Append(this.CSpaceTypeName);
      SortedSet<string> sortedSet1 = new SortedSet<string>();
      foreach (NavigationProperty navigationProperty in this.NavigationProperties)
        sortedSet1.Add(navigationProperty.Name + "*" + navigationProperty.FromEndMember.Name + "*" + navigationProperty.FromEndMember.RelationshipMultiplicity.ToString() + "*" + navigationProperty.ToEndMember.Name + "*" + navigationProperty.ToEndMember.RelationshipMultiplicity.ToString() + "*");
      stringBuilder.Append("NavProps:");
      foreach (string str in sortedSet1)
        stringBuilder.Append(str);
      SortedSet<string> sortedSet2 = new SortedSet<string>();
      foreach (string keyMemberName in this.KeyMemberNames)
        sortedSet2.Add(keyMemberName);
      stringBuilder.Append("Keys:");
      foreach (string str in sortedSet2)
        stringBuilder.Append(str + "*");
      SortedSet<string> sortedSet3 = new SortedSet<string>();
      foreach (EdmMember member in this.Members)
      {
        if (!sortedSet2.Contains(member.Name))
          sortedSet3.Add(member.Name + "*");
      }
      stringBuilder.Append("Scalars:");
      foreach (string str in sortedSet3)
        stringBuilder.Append(str + "*");
      return stringBuilder.ToString();
    }
  }
}

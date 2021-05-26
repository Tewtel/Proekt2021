// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Formatting.IRequiredMemberSelector
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Reflection;

namespace System.Net.Http.Formatting
{
  /// <summary>Defines method that determines whether a given member is required on deserialization.</summary>
  public interface IRequiredMemberSelector
  {
    /// <summary>Determines whether a given member is required on deserialization.</summary>
    /// <returns>true if <paramref name="member" /> should be treated as a required member; otherwise false.</returns>
    /// <param name="member">The <see cref="T:System.Reflection.MemberInfo" /> to be deserialized.</param>
    bool IsRequiredMember(MemberInfo member);
  }
}

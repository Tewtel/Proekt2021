// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.AssemblySourceTimeStampAttribute
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// Defines a source code time-stamp custom attribute for an assembly
  /// manifest.
  /// </summary>
  [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
  public sealed class AssemblySourceTimeStampAttribute : Attribute
  {
    private string sourceTimeStamp;

    /// <summary>
    /// Constructs an instance of this attribute class using the specified
    /// source code time-stamp value.
    /// </summary>
    /// <param name="value">The source code time-stamp value to use.</param>
    public AssemblySourceTimeStampAttribute(string value) => this.sourceTimeStamp = value;

    /// <summary>Gets the source code time-stamp value.</summary>
    public string SourceTimeStamp => this.sourceTimeStamp;
  }
}

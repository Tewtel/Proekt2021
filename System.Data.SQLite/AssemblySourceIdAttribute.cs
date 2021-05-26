// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.AssemblySourceIdAttribute
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// Defines a source code identifier custom attribute for an assembly
  /// manifest.
  /// </summary>
  [AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
  public sealed class AssemblySourceIdAttribute : Attribute
  {
    private string sourceId;

    /// <summary>
    /// Constructs an instance of this attribute class using the specified
    /// source code identifier value.
    /// </summary>
    /// <param name="value">The source code identifier value to use.</param>
    public AssemblySourceIdAttribute(string value) => this.sourceId = value;

    /// <summary>Gets the source code identifier value.</summary>
    public string SourceId => this.sourceId;
  }
}

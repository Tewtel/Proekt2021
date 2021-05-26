// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.DataClasses.EdmTypeAttribute
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects.DataClasses
{
  /// <summary>Base attribute for schematized types</summary>
  public abstract class EdmTypeAttribute : Attribute
  {
    internal EdmTypeAttribute()
    {
    }

    /// <summary>The name of the type in the conceptual schema that maps to the class to which this attribute is applied.</summary>
    /// <returns>
    /// A <see cref="T:System.String" /> that is the name.
    /// </returns>
    public string Name { get; set; }

    /// <summary>The namespace name of the entity object type or complex type in the conceptual schema that maps to this type.</summary>
    /// <returns>
    /// A <see cref="T:System.String" /> that is the namespace name.
    /// </returns>
    public string NamespaceName { get; set; }
  }
}

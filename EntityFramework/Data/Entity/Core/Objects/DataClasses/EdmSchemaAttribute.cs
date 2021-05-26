// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.DataClasses.EdmSchemaAttribute
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects.DataClasses
{
  /// <summary>Attribute for static types</summary>
  [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class, AllowMultiple = true)]
  public sealed class EdmSchemaAttribute : Attribute
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.Objects.DataClasses.EdmSchemaAttribute" /> class.
    /// </summary>
    public EdmSchemaAttribute()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Data.Entity.Core.Objects.DataClasses.EdmSchemaAttribute" /> class with a unique value for each model referenced by the assembly.
    /// </summary>
    /// <remarks>
    /// Setting this parameter to a unique value for each model file in a Visual Basic
    /// assembly will prevent the following error:
    /// "'System.Data.Entity.Core.Objects.DataClasses.EdmSchemaAttribute' cannot be specified more than once in this project, even with identical parameter values."
    /// </remarks>
    /// <param name="assemblyGuid">A string that is a unique GUID value for the model in the assembly.</param>
    public EdmSchemaAttribute(string assemblyGuid) => System.Data.Entity.Utilities.Check.NotNull<string>(assemblyGuid, nameof (assemblyGuid));
  }
}

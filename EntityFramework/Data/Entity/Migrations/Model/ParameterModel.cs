﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Migrations.Model.ParameterModel
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;

namespace System.Data.Entity.Migrations.Model
{
  /// <summary>
  /// Represents information about a parameter.
  /// 
  /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
  /// (such as the end user of an application). If input is accepted from such sources it should be validated
  /// before being passed to these APIs to protect against SQL injection attacks etc.
  /// </summary>
  public class ParameterModel : PropertyModel
  {
    /// <summary>
    /// Initializes a new instance of the ParameterModel class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="type"> The data type for this parameter. </param>
    public ParameterModel(PrimitiveTypeKind type)
      : this(type, (TypeUsage) null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the ParameterModel class.
    /// 
    /// Entity Framework Migrations APIs are not designed to accept input provided by untrusted sources
    /// (such as the end user of an application). If input is accepted from such sources it should be validated
    /// before being passed to these APIs to protect against SQL injection attacks etc.
    /// </summary>
    /// <param name="type"> The data type for this parameter. </param>
    /// <param name="typeUsage"> Additional details about the data type. This includes details such as maximum length, nullability etc. </param>
    public ParameterModel(PrimitiveTypeKind type, TypeUsage typeUsage)
      : base(type, typeUsage)
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is out parameter.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is out parameter; otherwise, <c>false</c>.
    /// </value>
    public bool IsOutParameter { get; set; }
  }
}

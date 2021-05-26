// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ModificationFunctionParameterBinding
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;

namespace System.Data.Entity.Core.Mapping
{
  /// <summary>
  /// Binds a modification function parameter to a member of the entity or association being modified.
  /// </summary>
  public sealed class ModificationFunctionParameterBinding : MappingItem
  {
    private readonly FunctionParameter _parameter;
    private readonly ModificationFunctionMemberPath _memberPath;
    private readonly bool _isCurrent;

    /// <summary>
    /// Initializes a new ModificationFunctionParameterBinding instance.
    /// </summary>
    /// <param name="parameter">The parameter taking the value.</param>
    /// <param name="memberPath">The path to the entity or association member defining the value.</param>
    /// <param name="isCurrent">A flag indicating whether the current or original member value is being bound.</param>
    public ModificationFunctionParameterBinding(
      FunctionParameter parameter,
      ModificationFunctionMemberPath memberPath,
      bool isCurrent)
    {
      System.Data.Entity.Utilities.Check.NotNull<FunctionParameter>(parameter, nameof (parameter));
      System.Data.Entity.Utilities.Check.NotNull<ModificationFunctionMemberPath>(memberPath, nameof (memberPath));
      this._parameter = parameter;
      this._memberPath = memberPath;
      this._isCurrent = isCurrent;
    }

    /// <summary>Gets the parameter taking the value.</summary>
    public FunctionParameter Parameter => this._parameter;

    /// <summary>
    /// Gets the path to the entity or association member defining the value.
    /// </summary>
    public ModificationFunctionMemberPath MemberPath => this._memberPath;

    /// <summary>
    /// Gets a flag indicating whether the current or original
    /// member value is being bound.
    /// </summary>
    public bool IsCurrent => this._isCurrent;

    /// <inheritdoc />
    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "@{0}->{1}{2}", (object) this.Parameter, this.IsCurrent ? (object) "+" : (object) "-", (object) this.MemberPath);

    internal override void SetReadOnly()
    {
      MappingItem.SetReadOnly((MappingItem) this._memberPath);
      base.SetReadOnly();
    }
  }
}

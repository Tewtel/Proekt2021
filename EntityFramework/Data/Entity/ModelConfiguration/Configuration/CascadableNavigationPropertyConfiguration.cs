// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.CascadableNavigationPropertyConfiguration
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Configuration.Properties.Navigation;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Configures a relationship that can support cascade on delete functionality.
  /// </summary>
  public abstract class CascadableNavigationPropertyConfiguration
  {
    private readonly NavigationPropertyConfiguration _navigationPropertyConfiguration;

    internal CascadableNavigationPropertyConfiguration(
      NavigationPropertyConfiguration navigationPropertyConfiguration)
    {
      this._navigationPropertyConfiguration = navigationPropertyConfiguration;
    }

    /// <summary>
    /// Configures cascade delete to be on for the relationship.
    /// </summary>
    public void WillCascadeOnDelete() => this.WillCascadeOnDelete(true);

    /// <summary>
    /// Configures whether or not cascade delete is on for the relationship.
    /// </summary>
    /// <param name="value"> Value indicating if cascade delete is on or not. </param>
    public void WillCascadeOnDelete(bool value) => this._navigationPropertyConfiguration.DeleteAction = new OperationAction?(value ? OperationAction.Cascade : OperationAction.None);

    internal NavigationPropertyConfiguration NavigationPropertyConfiguration => this._navigationPropertyConfiguration;

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string ToString() => base.ToString();

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object obj) => base.Equals(obj);

    /// <inheritdoc />
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <summary>
    /// Gets the <see cref="T:System.Type" /> of the current instance.
    /// </summary>
    /// <returns>The exact runtime type of the current instance.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public new Type GetType() => base.GetType();
  }
}

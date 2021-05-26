// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Configuration.TypeConventionWithHavingConfiguration`2
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace System.Data.Entity.ModelConfiguration.Configuration
{
  /// <summary>
  /// Allows configuration to be performed for a lightweight convention based on
  /// the entity types in a model that inherit from a common, specified type and a
  /// captured value.
  /// </summary>
  /// <typeparam name="T"> The common type of the entity types that this convention applies to. </typeparam>
  /// <typeparam name="TValue"> Type of the captured value. </typeparam>
  public class TypeConventionWithHavingConfiguration<T, TValue>
    where T : class
    where TValue : class
  {
    private readonly ConventionsConfiguration _conventionsConfiguration;
    private readonly IEnumerable<Func<Type, bool>> _predicates;
    private readonly Func<Type, TValue> _capturingPredicate;

    internal TypeConventionWithHavingConfiguration(
      ConventionsConfiguration conventionsConfiguration,
      IEnumerable<Func<Type, bool>> predicates,
      Func<Type, TValue> capturingPredicate)
    {
      this._conventionsConfiguration = conventionsConfiguration;
      this._predicates = predicates;
      this._capturingPredicate = capturingPredicate;
    }

    internal ConventionsConfiguration ConventionsConfiguration => this._conventionsConfiguration;

    internal IEnumerable<Func<Type, bool>> Predicates => this._predicates;

    internal Func<Type, TValue> CapturingPredicate => this._capturingPredicate;

    /// <summary>
    /// Allows configuration of the entity types that this convention applies to.
    /// </summary>
    /// <param name="entityConfigurationAction">
    /// An action that performs configuration against a <see cref="T:System.Data.Entity.ModelConfiguration.Configuration.ConventionTypeConfiguration`1" />
    /// using a captured value.
    /// </param>
    public void Configure(
      Action<ConventionTypeConfiguration<T>, TValue> entityConfigurationAction)
    {
      System.Data.Entity.Utilities.Check.NotNull<Action<ConventionTypeConfiguration<T>, TValue>>(entityConfigurationAction, nameof (entityConfigurationAction));
      this._conventionsConfiguration.Add((IConvention) new TypeConventionWithHaving<T, TValue>(this._predicates, this._capturingPredicate, entityConfigurationAction));
    }

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

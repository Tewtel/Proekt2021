// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Annotations.AnnotationValues
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure.Annotations
{
  /// <summary>
  /// Represents a pair of annotation values in a scaffolded or hand-coded <see cref="T:System.Data.Entity.Migrations.DbMigration" />.
  /// </summary>
  /// <remarks>
  /// Code First allows for custom annotations to be associated with columns and tables in the
  /// generated model. This class represents a pair of annotation values in a migration such
  /// that when the Code First model changes the old annotation value and the new annotation
  /// value can be provided to the migration and used in SQL generation.
  /// </remarks>
  public sealed class AnnotationValues
  {
    private readonly object _oldValue;
    private readonly object _newValue;

    /// <summary>Creates a new pair of annotation values.</summary>
    /// <param name="oldValue">The old value of the annotation, which may be null if the annotation has just been created.</param>
    /// <param name="newValue">The new value of the annotation, which may be null if the annotation has been deleted.</param>
    public AnnotationValues(object oldValue, object newValue)
    {
      this._oldValue = oldValue;
      this._newValue = newValue;
    }

    /// <summary>
    /// Gets the old value of the annotation, which may be null if the annotation has just been created.
    /// </summary>
    public object OldValue => this._oldValue;

    /// <summary>
    /// Gets the new value of the annotation, which may be null if the annotation has been deleted.
    /// </summary>
    public object NewValue => this._newValue;

    private bool Equals(AnnotationValues other) => object.Equals(this._oldValue, other._oldValue) && object.Equals(this._newValue, other._newValue);

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if ((object) this == obj)
        return true;
      return (object) (obj as AnnotationValues) != null && this.Equals((AnnotationValues) obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() => (this._oldValue != null ? this._oldValue.GetHashCode() : 0) * 397 ^ (this._newValue != null ? this._newValue.GetHashCode() : 0);

    /// <summary>
    /// Returns true if both annotation pairs contain the same values, otherwise false.
    /// </summary>
    /// <param name="left">A pair of annotation values.</param>
    /// <param name="right">A pair of annotation values.</param>
    /// <returns>True if both pairs contain the same values.</returns>
    public static bool operator ==(AnnotationValues left, AnnotationValues right) => object.Equals((object) left, (object) right);

    /// <summary>
    /// Returns true if the two annotation pairs contain different values, otherwise false.
    /// </summary>
    /// <param name="left">A pair of annotation values.</param>
    /// <param name="right">A pair of annotation values.</param>
    /// <returns>True if the pairs contain different values.</returns>
    public static bool operator !=(AnnotationValues left, AnnotationValues right) => !object.Equals((object) left, (object) right);
  }
}

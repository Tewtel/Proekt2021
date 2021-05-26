// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.DataClasses.ComplexObject
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;
using System.Runtime.Serialization;

namespace System.Data.Entity.Core.Objects.DataClasses
{
  /// <summary>
  /// This is the interface that represent the minimum interface required
  /// to be an entity in ADO.NET.
  /// </summary>
  [DataContract(IsReference = true)]
  [Serializable]
  public abstract class ComplexObject : StructuralObject
  {
    private StructuralObject _parent;
    private string _parentPropertyName;

    internal void AttachToParent(StructuralObject parent, string parentPropertyName)
    {
      this._parent = this._parent == null ? parent : throw new InvalidOperationException(Strings.ComplexObject_ComplexObjectAlreadyAttachedToParent);
      this._parentPropertyName = parentPropertyName;
    }

    internal void DetachFromParent()
    {
      this._parent = (StructuralObject) null;
      this._parentPropertyName = (string) null;
    }

    /// <summary>Notifies the change tracker that a property change is pending on a complex object.</summary>
    /// <param name="property">The name of the changing property.</param>
    /// <exception cref="T:System.ArgumentNullException"> property  is null.</exception>
    protected override sealed void ReportPropertyChanging(string property)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(property, nameof (property));
      base.ReportPropertyChanging(property);
      this.ReportComplexPropertyChanging((string) null, this, property);
    }

    /// <summary>Notifies the change tracker that a property of a complex object has changed.</summary>
    /// <param name="property">The name of the changed property.</param>
    /// <exception cref="T:System.ArgumentNullException"> property  is null.</exception>
    protected override sealed void ReportPropertyChanged(string property)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(property, nameof (property));
      this.ReportComplexPropertyChanged((string) null, this, property);
      base.ReportPropertyChanged(property);
    }

    internal override sealed bool IsChangeTracked => this._parent != null && this._parent.IsChangeTracked;

    internal override sealed void ReportComplexPropertyChanging(
      string entityMemberName,
      ComplexObject complexObject,
      string complexMemberName)
    {
      if (this._parent == null)
        return;
      this._parent.ReportComplexPropertyChanging(this._parentPropertyName, complexObject, complexMemberName);
    }

    internal override sealed void ReportComplexPropertyChanged(
      string entityMemberName,
      ComplexObject complexObject,
      string complexMemberName)
    {
      if (this._parent == null)
        return;
      this._parent.ReportComplexPropertyChanged(this._parentPropertyName, complexObject, complexMemberName);
    }
  }
}

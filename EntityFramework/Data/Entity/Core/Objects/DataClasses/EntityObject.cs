// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.DataClasses.EntityObject
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.ComponentModel;
using System.Data.Entity.Resources;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace System.Data.Entity.Core.Objects.DataClasses
{
  /// <summary>
  /// This is the class is the basis for all perscribed EntityObject classes.
  /// </summary>
  [DataContract(IsReference = true)]
  [Serializable]
  public abstract class EntityObject : 
    StructuralObject,
    IEntityWithKey,
    IEntityWithChangeTracker,
    IEntityWithRelationships
  {
    private RelationshipManager _relationships;
    private EntityKey _entityKey;
    [NonSerialized]
    private IEntityChangeTracker _entityChangeTracker = (IEntityChangeTracker) EntityObject._detachedEntityChangeTracker;
    [NonSerialized]
    private static readonly EntityObject.DetachedEntityChangeTracker _detachedEntityChangeTracker = new EntityObject.DetachedEntityChangeTracker();

    private IEntityChangeTracker EntityChangeTracker
    {
      get
      {
        if (this._entityChangeTracker == null)
          this._entityChangeTracker = (IEntityChangeTracker) EntityObject._detachedEntityChangeTracker;
        return this._entityChangeTracker;
      }
      set => this._entityChangeTracker = value;
    }

    /// <summary>Gets the entity state of the object.</summary>
    /// <returns>
    /// The <see cref="T:System.Data.Entity.EntityState" /> of this object.
    /// </returns>
    [Browsable(false)]
    [XmlIgnore]
    public EntityState EntityState => this.EntityChangeTracker.EntityState;

    /// <summary>Gets or sets the key for this object.</summary>
    /// <returns>
    /// The <see cref="T:System.Data.Entity.Core.EntityKey" /> for this object.
    /// </returns>
    [Browsable(false)]
    [DataMember]
    public EntityKey EntityKey
    {
      get => this._entityKey;
      set
      {
        this.EntityChangeTracker.EntityMemberChanging("-EntityKey-");
        this._entityKey = value;
        this.EntityChangeTracker.EntityMemberChanged("-EntityKey-");
      }
    }

    /// <summary>
    /// Used by the ObjectStateManager to attach or detach this EntityObject to the cache.
    /// </summary>
    /// <param name="changeTracker"> Reference to the ObjectStateEntry that contains this entity </param>
    void IEntityWithChangeTracker.SetChangeTracker(
      IEntityChangeTracker changeTracker)
    {
      if (changeTracker != null && this.EntityChangeTracker != EntityObject._detachedEntityChangeTracker && changeTracker != this.EntityChangeTracker && (!(this.EntityChangeTracker is EntityEntry entityChangeTracker) || !entityChangeTracker.ObjectStateManager.IsDisposed))
        throw new InvalidOperationException(Strings.Entity_EntityCantHaveMultipleChangeTrackers);
      this.EntityChangeTracker = changeTracker;
    }

    /// <summary>
    /// Returns the container for the lazily created relationship
    /// navigation property objects, collections and refs.
    /// </summary>
    RelationshipManager IEntityWithRelationships.RelationshipManager
    {
      get
      {
        if (this._relationships == null)
          this._relationships = RelationshipManager.Create((IEntityWithRelationships) this);
        return this._relationships;
      }
    }

    /// <summary>Notifies the change tracker that a property change is pending.</summary>
    /// <param name="property">The name of the changing property.</param>
    /// <exception cref="T:System.ArgumentNullException"> property  is null.</exception>
    protected override sealed void ReportPropertyChanging(string property)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(property, nameof (property));
      base.ReportPropertyChanging(property);
      this.EntityChangeTracker.EntityMemberChanging(property);
    }

    /// <summary>Notifies the change tracker that a property has changed.</summary>
    /// <param name="property">The name of the changed property.</param>
    /// <exception cref="T:System.ArgumentNullException"> property  is null.</exception>
    protected override sealed void ReportPropertyChanged(string property)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(property, nameof (property));
      this.EntityChangeTracker.EntityMemberChanged(property);
      base.ReportPropertyChanged(property);
    }

    internal override sealed bool IsChangeTracked => this.EntityState != EntityState.Detached;

    internal override sealed void ReportComplexPropertyChanging(
      string entityMemberName,
      ComplexObject complexObject,
      string complexMemberName)
    {
      this.EntityChangeTracker.EntityComplexMemberChanging(entityMemberName, (object) complexObject, complexMemberName);
    }

    internal override sealed void ReportComplexPropertyChanged(
      string entityMemberName,
      ComplexObject complexObject,
      string complexMemberName)
    {
      this.EntityChangeTracker.EntityComplexMemberChanged(entityMemberName, (object) complexObject, complexMemberName);
    }

    private class DetachedEntityChangeTracker : IEntityChangeTracker
    {
      void IEntityChangeTracker.EntityMemberChanging(string entityMemberName)
      {
      }

      void IEntityChangeTracker.EntityMemberChanged(string entityMemberName)
      {
      }

      void IEntityChangeTracker.EntityComplexMemberChanging(
        string entityMemberName,
        object complexObject,
        string complexMemberName)
      {
      }

      void IEntityChangeTracker.EntityComplexMemberChanged(
        string entityMemberName,
        object complexObject,
        string complexMemberName)
      {
      }

      EntityState IEntityChangeTracker.EntityState => EntityState.Detached;
    }
  }
}

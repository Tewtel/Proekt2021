// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.ForeignKeyBuilder
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Edm;
using System.Linq;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class ForeignKeyBuilder : MetadataItem, INamedDataModelItem
  {
    private const string SelfRefSuffix = "Self";
    private readonly EdmModel _database;
    private readonly AssociationType _associationType;
    private readonly AssociationSet _associationSet;

    internal ForeignKeyBuilder()
    {
    }

    public ForeignKeyBuilder(EdmModel database, string name)
    {
      System.Data.Entity.Utilities.Check.NotNull<EdmModel>(database, nameof (database));
      this._database = database;
      this._associationType = new AssociationType(name, "CodeFirstDatabaseSchema", true, DataSpace.SSpace);
      this._associationSet = new AssociationSet(this._associationType.Name, this._associationType);
    }

    public string Name
    {
      get => this._associationType.Name;
      set
      {
        this._associationType.Name = value;
        this._associationSet.Name = value;
      }
    }

    public virtual EntityType PrincipalTable
    {
      get => this._associationType.SourceEnd.GetEntityType();
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<EntityType>(value, nameof (value));
        Util.ThrowIfReadOnly((MetadataItem) this);
        this._associationType.SourceEnd = new AssociationEndMember(value.Name, value);
        this._associationSet.SourceSet = this._database.GetEntitySet(value);
        if (this._associationType.TargetEnd == null || !(value.Name == this._associationType.TargetEnd.Name))
          return;
        this._associationType.TargetEnd.Name = value.Name + "Self";
      }
    }

    public virtual void SetOwner(EntityType owner)
    {
      Util.ThrowIfReadOnly((MetadataItem) this);
      if (owner == null)
      {
        this._database.RemoveAssociationType(this._associationType);
      }
      else
      {
        this._associationType.TargetEnd = new AssociationEndMember(owner != this.PrincipalTable ? owner.Name : owner.Name + "Self", owner);
        this._associationSet.TargetSet = this._database.GetEntitySet(owner);
        if (this._database.AssociationTypes.Contains<AssociationType>(this._associationType))
          return;
        this._database.AddAssociationType(this._associationType);
        this._database.AddAssociationSet(this._associationSet);
      }
    }

    public virtual IEnumerable<EdmProperty> DependentColumns
    {
      get => this._associationType.Constraint == null ? Enumerable.Empty<EdmProperty>() : (IEnumerable<EdmProperty>) this._associationType.Constraint.ToProperties;
      set
      {
        System.Data.Entity.Utilities.Check.NotNull<IEnumerable<EdmProperty>>(value, nameof (value));
        Util.ThrowIfReadOnly((MetadataItem) this);
        this._associationType.Constraint = new ReferentialConstraint((RelationshipEndMember) this._associationType.SourceEnd, (RelationshipEndMember) this._associationType.TargetEnd, (IEnumerable<EdmProperty>) this.PrincipalTable.KeyProperties, value);
        this.SetMultiplicities();
      }
    }

    public OperationAction DeleteAction
    {
      get => this._associationType.SourceEnd == null ? OperationAction.None : this._associationType.SourceEnd.DeleteBehavior;
      set
      {
        Util.ThrowIfReadOnly((MetadataItem) this);
        this._associationType.SourceEnd.DeleteBehavior = value;
      }
    }

    private void SetMultiplicities()
    {
      this._associationType.SourceEnd.RelationshipMultiplicity = RelationshipMultiplicity.ZeroOrOne;
      this._associationType.TargetEnd.RelationshipMultiplicity = RelationshipMultiplicity.Many;
      EntityType dependentTable = this._associationType.TargetEnd.GetEntityType();
      List<EdmProperty> list = dependentTable.KeyProperties.Where<EdmProperty>((Func<EdmProperty, bool>) (key => dependentTable.DeclaredMembers.Contains((EdmMember) key))).ToList<EdmProperty>();
      if (list.Count == this.DependentColumns.Count<EdmProperty>() && list.All<EdmProperty>(new Func<EdmProperty, bool>(((Enumerable) this.DependentColumns).Contains<EdmProperty>)))
      {
        this._associationType.SourceEnd.RelationshipMultiplicity = RelationshipMultiplicity.One;
        this._associationType.TargetEnd.RelationshipMultiplicity = RelationshipMultiplicity.ZeroOrOne;
      }
      else
      {
        if (this.DependentColumns.Any<EdmProperty>((Func<EdmProperty, bool>) (p => p.Nullable)))
          return;
        this._associationType.SourceEnd.RelationshipMultiplicity = RelationshipMultiplicity.One;
      }
    }

    public override BuiltInTypeKind BuiltInTypeKind => throw new NotImplementedException();

    string INamedDataModelItem.Identity => this.Identity;

    internal override string Identity => throw new NotImplementedException();
  }
}

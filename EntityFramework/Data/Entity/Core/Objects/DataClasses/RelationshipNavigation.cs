// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.DataClasses.RelationshipNavigation
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;

namespace System.Data.Entity.Core.Objects.DataClasses
{
  [Serializable]
  internal class RelationshipNavigation
  {
    private readonly string _relationshipName;
    private readonly string _from;
    private readonly string _to;
    [NonSerialized]
    private RelationshipNavigation _reverse;
    [NonSerialized]
    private NavigationPropertyAccessor _fromAccessor;
    [NonSerialized]
    private NavigationPropertyAccessor _toAccessor;
    [NonSerialized]
    private readonly AssociationType _associationType;

    internal RelationshipNavigation(
      string relationshipName,
      string from,
      string to,
      NavigationPropertyAccessor fromAccessor,
      NavigationPropertyAccessor toAccessor)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(relationshipName, nameof (relationshipName));
      System.Data.Entity.Utilities.Check.NotEmpty(from, nameof (from));
      System.Data.Entity.Utilities.Check.NotEmpty(to, nameof (to));
      this._relationshipName = relationshipName;
      this._from = from;
      this._to = to;
      this._fromAccessor = fromAccessor;
      this._toAccessor = toAccessor;
    }

    internal RelationshipNavigation(
      AssociationType associationType,
      string from,
      string to,
      NavigationPropertyAccessor fromAccessor,
      NavigationPropertyAccessor toAccessor)
    {
      this._associationType = associationType;
      this._relationshipName = associationType.FullName;
      this._from = from;
      this._to = to;
      this._fromAccessor = fromAccessor;
      this._toAccessor = toAccessor;
    }

    internal AssociationType AssociationType => this._associationType;

    internal string RelationshipName => this._relationshipName;

    internal string From => this._from;

    internal string To => this._to;

    internal NavigationPropertyAccessor ToPropertyAccessor => this._toAccessor;

    internal bool IsInitialized => this._toAccessor != null && this._fromAccessor != null;

    internal void InitializeAccessors(
      NavigationPropertyAccessor fromAccessor,
      NavigationPropertyAccessor toAccessor)
    {
      this._fromAccessor = fromAccessor;
      this._toAccessor = toAccessor;
    }

    internal RelationshipNavigation Reverse
    {
      get
      {
        if (this._reverse == null || !this._reverse.IsInitialized)
          this._reverse = this._associationType != null ? new RelationshipNavigation(this._associationType, this._to, this._from, this._toAccessor, this._fromAccessor) : new RelationshipNavigation(this._relationshipName, this._to, this._from, this._toAccessor, this._fromAccessor);
        return this._reverse;
      }
    }

    public override bool Equals(object obj)
    {
      RelationshipNavigation relationshipNavigation = obj as RelationshipNavigation;
      if (this == relationshipNavigation)
        return true;
      return this != null && relationshipNavigation != null && (this.RelationshipName == relationshipNavigation.RelationshipName && this.From == relationshipNavigation.From) && this.To == relationshipNavigation.To;
    }

    public override int GetHashCode() => this.RelationshipName.GetHashCode();

    public override string ToString() => string.Format((IFormatProvider) CultureInfo.InvariantCulture, "RelationshipNavigation: ({0},{1},{2})", (object) this._relationshipName, (object) this._from, (object) this._to);
  }
}

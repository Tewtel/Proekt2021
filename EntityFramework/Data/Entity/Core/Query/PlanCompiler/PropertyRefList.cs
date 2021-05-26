// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.PropertyRefList
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class PropertyRefList
  {
    private readonly Dictionary<PropertyRef, PropertyRef> m_propertyReferences;
    private bool m_allProperties;
    internal static PropertyRefList All = new PropertyRefList(true);

    internal PropertyRefList()
      : this(false)
    {
    }

    private PropertyRefList(bool allProps)
    {
      this.m_propertyReferences = new Dictionary<PropertyRef, PropertyRef>();
      if (!allProps)
        return;
      this.MakeAllProperties();
    }

    private void MakeAllProperties()
    {
      this.m_allProperties = true;
      this.m_propertyReferences.Clear();
      this.m_propertyReferences.Add((PropertyRef) AllPropertyRef.Instance, (PropertyRef) AllPropertyRef.Instance);
    }

    internal void Add(PropertyRef property)
    {
      if (this.m_allProperties)
        return;
      if (property is AllPropertyRef)
        this.MakeAllProperties();
      else
        this.m_propertyReferences[property] = property;
    }

    internal void Append(PropertyRefList propertyRefs)
    {
      if (this.m_allProperties)
        return;
      foreach (PropertyRef key in propertyRefs.m_propertyReferences.Keys)
        this.Add(key);
    }

    internal bool AllProperties => this.m_allProperties;

    internal PropertyRefList Clone()
    {
      PropertyRefList propertyRefList = new PropertyRefList(this.m_allProperties);
      foreach (PropertyRef key in this.m_propertyReferences.Keys)
        propertyRefList.Add(key);
      return propertyRefList;
    }

    internal bool Contains(PropertyRef p) => this.m_allProperties || this.m_propertyReferences.ContainsKey(p);

    internal IEnumerable<PropertyRef> Properties => (IEnumerable<PropertyRef>) this.m_propertyReferences.Keys;

    public override string ToString()
    {
      string str = "{";
      foreach (PropertyRef key in this.m_propertyReferences.Keys)
        str = str + key?.ToString() + ",";
      return str + "}";
    }
  }
}

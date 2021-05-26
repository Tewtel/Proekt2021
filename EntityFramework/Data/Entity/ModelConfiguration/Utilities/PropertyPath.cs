// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Utilities.PropertyPath
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System.Data.Entity.ModelConfiguration.Utilities
{
  internal class PropertyPath : IEnumerable<PropertyInfo>, IEnumerable
  {
    private static readonly PropertyPath _empty = new PropertyPath();
    private readonly List<PropertyInfo> _components = new List<PropertyInfo>();

    public PropertyPath(IEnumerable<PropertyInfo> components) => this._components.AddRange(components);

    public PropertyPath(PropertyInfo component) => this._components.Add(component);

    private PropertyPath()
    {
    }

    public int Count => this._components.Count;

    public static PropertyPath Empty => PropertyPath._empty;

    public PropertyInfo this[int index] => this._components[index];

    public override string ToString()
    {
      StringBuilder propertyPathName = new StringBuilder();
      this._components.Each<PropertyInfo>((Action<PropertyInfo>) (pi =>
      {
        propertyPathName.Append(pi.Name);
        propertyPathName.Append('.');
      }));
      return propertyPathName.ToString(0, propertyPathName.Length - 1);
    }

    public bool Equals(PropertyPath other)
    {
      if ((object) other == null)
        return false;
      return (object) this == (object) other || this._components.SequenceEqual<PropertyInfo>((IEnumerable<PropertyInfo>) other._components, (Func<PropertyInfo, PropertyInfo, bool>) ((p1, p2) => p1.IsSameAs(p2)));
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if ((object) this == obj)
        return true;
      return !(obj.GetType() != typeof (PropertyPath)) && this.Equals((PropertyPath) obj);
    }

    public override int GetHashCode() => this._components.Aggregate<PropertyInfo, int>(0, (Func<int, PropertyInfo, int>) ((t, n) => t ^ n.DeclaringType.GetHashCode() * n.Name.GetHashCode() * 397));

    public static bool operator ==(PropertyPath left, PropertyPath right) => object.Equals((object) left, (object) right);

    public static bool operator !=(PropertyPath left, PropertyPath right) => !object.Equals((object) left, (object) right);

    IEnumerator<PropertyInfo> IEnumerable<PropertyInfo>.GetEnumerator() => (IEnumerator<PropertyInfo>) this._components.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._components.GetEnumerator();
  }
}

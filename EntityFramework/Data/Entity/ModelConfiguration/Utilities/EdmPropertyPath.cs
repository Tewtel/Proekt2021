// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Utilities.EdmPropertyPath
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Text;

namespace System.Data.Entity.ModelConfiguration.Utilities
{
  internal class EdmPropertyPath : IEnumerable<EdmProperty>, IEnumerable
  {
    private static readonly EdmPropertyPath _empty = new EdmPropertyPath();
    private readonly List<EdmProperty> _components = new List<EdmProperty>();

    public EdmPropertyPath(IEnumerable<EdmProperty> components) => this._components.AddRange(components);

    public EdmPropertyPath(EdmProperty component) => this._components.Add(component);

    private EdmPropertyPath()
    {
    }

    public static EdmPropertyPath Empty => EdmPropertyPath._empty;

    public override string ToString()
    {
      StringBuilder propertyPathName = new StringBuilder();
      this._components.Each<EdmProperty>((Action<EdmProperty>) (pi =>
      {
        propertyPathName.Append(pi.Name);
        propertyPathName.Append('.');
      }));
      return propertyPathName.ToString(0, propertyPathName.Length - 1);
    }

    public bool Equals(EdmPropertyPath other)
    {
      if ((object) other == null)
        return false;
      return (object) this == (object) other || this._components.SequenceEqual<EdmProperty>((IEnumerable<EdmProperty>) other._components, (Func<EdmProperty, EdmProperty, bool>) ((p1, p2) => p1 == p2));
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if ((object) this == obj)
        return true;
      return !(obj.GetType() != typeof (EdmPropertyPath)) && this.Equals((EdmPropertyPath) obj);
    }

    public override int GetHashCode() => this._components.Aggregate<EdmProperty, int>(0, (Func<int, EdmProperty, int>) ((t, n) => t + n.GetHashCode()));

    public static bool operator ==(EdmPropertyPath left, EdmPropertyPath right) => object.Equals((object) left, (object) right);

    public static bool operator !=(EdmPropertyPath left, EdmPropertyPath right) => !object.Equals((object) left, (object) right);

    IEnumerator<EdmProperty> IEnumerable<EdmProperty>.GetEnumerator() => (IEnumerator<EdmProperty>) this._components.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._components.GetEnumerator();
  }
}

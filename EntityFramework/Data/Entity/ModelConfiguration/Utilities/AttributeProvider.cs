// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ModelConfiguration.Utilities.AttributeProvider
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace System.Data.Entity.ModelConfiguration.Utilities
{
  internal class AttributeProvider
  {
    private readonly ConcurrentDictionary<PropertyInfo, IEnumerable<Attribute>> _discoveredAttributes = new ConcurrentDictionary<PropertyInfo, IEnumerable<Attribute>>();

    public virtual IEnumerable<Attribute> GetAttributes(MemberInfo memberInfo)
    {
      Type type = memberInfo as Type;
      return type != (Type) null ? this.GetAttributes(type) : this.GetAttributes((PropertyInfo) memberInfo);
    }

    public virtual IEnumerable<Attribute> GetAttributes(Type type)
    {
      List<Attribute> attrs = new List<Attribute>(AttributeProvider.GetTypeDescriptor(type).GetAttributes().Cast<Attribute>());
      foreach (Attribute attribute in type.GetCustomAttributes<Attribute>(true).Where<Attribute>((Func<Attribute, bool>) (a => a.GetType().FullName.Equals("System.Data.Services.Common.EntityPropertyMappingAttribute", StringComparison.Ordinal) && !attrs.Contains(a))))
        attrs.Add(attribute);
      return (IEnumerable<Attribute>) attrs;
    }

    public virtual IEnumerable<Attribute> GetAttributes(
      PropertyInfo propertyInfo)
    {
      return this._discoveredAttributes.GetOrAdd(propertyInfo, (Func<PropertyInfo, IEnumerable<Attribute>>) (pi =>
      {
        PropertyDescriptor property = AttributeProvider.GetTypeDescriptor(pi.DeclaringType).GetProperties()[pi.Name];
        IEnumerable<Attribute> attributes1 = property != null ? property.Attributes.Cast<Attribute>() : pi.GetCustomAttributes<Attribute>(true);
        ICollection<Attribute> attributes2 = (ICollection<Attribute>) this.GetAttributes(pi.PropertyType);
        if (attributes2.Count > 0)
          attributes1 = attributes1.Except<Attribute>((IEnumerable<Attribute>) attributes2);
        return (IEnumerable<Attribute>) attributes1.ToList<Attribute>();
      }));
    }

    private static ICustomTypeDescriptor GetTypeDescriptor(Type type) => new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);

    public virtual void ClearCache() => this._discoveredAttributes.Clear();
  }
}

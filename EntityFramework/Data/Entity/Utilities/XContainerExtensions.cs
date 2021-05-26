﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Utilities.XContainerExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace System.Data.Entity.Utilities
{
  internal static class XContainerExtensions
  {
    public static XElement GetOrAddElement(this XContainer container, XName name)
    {
      XElement xelement = container.Element(name);
      if (xelement == null)
      {
        xelement = new XElement(name);
        container.Add((object) xelement);
      }
      return xelement;
    }

    public static IEnumerable<XElement> Descendants(
      this XContainer container,
      IEnumerable<XName> name)
    {
      return name.SelectMany<XName, XElement>(new Func<XName, IEnumerable<XElement>>(container.Descendants));
    }

    public static IEnumerable<XElement> Elements(
      this XContainer container,
      IEnumerable<XName> name)
    {
      return name.SelectMany<XName, XElement>(new Func<XName, IEnumerable<XElement>>(container.Elements));
    }

    public static IEnumerable<XElement> Descendants<T>(
      this IEnumerable<T> source,
      IEnumerable<XName> name)
      where T : XContainer
    {
      return name.SelectMany<XName, XElement>((Func<XName, IEnumerable<XElement>>) (n => source.SelectMany<T, XElement>((Func<T, IEnumerable<XElement>>) (c => c.Descendants(n)))));
    }
  }
}

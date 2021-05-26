// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.ModifiableIteratorCollection`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System.Data.Entity.Core.Common.Utils
{
  internal class ModifiableIteratorCollection<TElement> : InternalBase
  {
    private readonly List<TElement> m_elements;
    private int m_currentIteratorIndex;

    internal ModifiableIteratorCollection(IEnumerable<TElement> elements)
    {
      this.m_elements = new List<TElement>(elements);
      this.m_currentIteratorIndex = -1;
    }

    internal bool IsEmpty => this.m_elements.Count == 0;

    internal TElement RemoveOneElement() => this.Remove(this.m_elements.Count - 1);

    internal void ResetIterator() => this.m_currentIteratorIndex = -1;

    internal void RemoveCurrentOfIterator()
    {
      this.Remove(this.m_currentIteratorIndex);
      --this.m_currentIteratorIndex;
    }

    internal IEnumerable<TElement> Elements()
    {
      for (this.m_currentIteratorIndex = 0; this.m_currentIteratorIndex < this.m_elements.Count; ++this.m_currentIteratorIndex)
        yield return this.m_elements[this.m_currentIteratorIndex];
    }

    internal override void ToCompactString(StringBuilder builder) => StringUtil.ToCommaSeparatedString(builder, (IEnumerable) this.m_elements);

    private TElement Remove(int index)
    {
      TElement element = this.m_elements[index];
      int index1 = this.m_elements.Count - 1;
      this.m_elements[index] = this.m_elements[index1];
      this.m_elements.RemoveAt(index1);
      return element;
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.EF6.InternalBase
// Assembly: System.Data.SQLite.EF6, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 284EE9AD-5161-41AE-8341-10FDAF741756
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.EF6.dll

using System.Runtime;
using System.Text;

namespace System.Data.SQLite.EF6
{
  internal abstract class InternalBase
  {
    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    protected InternalBase()
    {
    }

    internal abstract void ToCompactString(StringBuilder builder);

    internal virtual string ToFullString()
    {
      StringBuilder builder = new StringBuilder();
      this.ToFullString(builder);
      return builder.ToString();
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    internal virtual void ToFullString(StringBuilder builder) => this.ToCompactString(builder);

    public override string ToString()
    {
      StringBuilder builder = new StringBuilder();
      this.ToCompactString(builder);
      return builder.ToString();
    }
  }
}

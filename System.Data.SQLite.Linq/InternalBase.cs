// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Linq.InternalBase
// Assembly: System.Data.SQLite.Linq, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: CD270981-C76C-415D-A29C-2F81F5D54EA4
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.Linq.dll

using System.Runtime;
using System.Text;

namespace System.Data.SQLite.Linq
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

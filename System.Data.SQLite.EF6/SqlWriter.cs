// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.EF6.SqlWriter
// Assembly: System.Data.SQLite.EF6, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 284EE9AD-5161-41AE-8341-10FDAF741756
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.EF6.dll

using System.Globalization;
using System.IO;
using System.Text;

namespace System.Data.SQLite.EF6
{
  internal class SqlWriter : StringWriter
  {
    private int indent = -1;
    private bool atBeginningOfLine = true;

    internal int Indent
    {
      get => this.indent;
      set => this.indent = value;
    }

    public SqlWriter(StringBuilder b)
      : base(b, (IFormatProvider) CultureInfo.InvariantCulture)
    {
    }

    public override void Write(string value)
    {
      if (value == "\r\n")
      {
        base.WriteLine();
        this.atBeginningOfLine = true;
      }
      else
      {
        if (this.atBeginningOfLine)
        {
          if (this.indent > 0)
            base.Write(new string('\t', this.indent));
          this.atBeginningOfLine = false;
        }
        base.Write(value);
      }
    }

    public override void WriteLine()
    {
      base.WriteLine();
      this.atBeginningOfLine = true;
    }
  }
}

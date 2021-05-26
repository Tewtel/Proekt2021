// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.SqlServer.SqlGen.SqlWriter
// Assembly: EntityFramework.SqlServer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: E0770E1D-BF74-466D-ABBB-FAC31C88F959
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.SqlServer.dll

using System.Data.Entity.Migrations.Utilities;
using System.IO;
using System.Text;

namespace System.Data.Entity.SqlServer.SqlGen
{
  internal class SqlWriter : IndentedTextWriter
  {
    public SqlWriter(StringBuilder b)
      : base((TextWriter) new StringWriter(b, (IFormatProvider) IndentedTextWriter.Culture))
    {
    }
  }
}

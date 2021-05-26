// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.Linq.Properties.Resources
// Assembly: System.Data.SQLite.Linq, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: CD270981-C76C-415D-A29C-2F81F5D54EA4
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.Linq.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace System.Data.SQLite.Linq.Properties
{
  [CompilerGenerated]
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  internal sealed class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (object.ReferenceEquals((object) System.Data.SQLite.Linq.Properties.Resources.resourceMan, (object) null))
          System.Data.SQLite.Linq.Properties.Resources.resourceMan = new ResourceManager("System.Data.SQLite.Properties", typeof (System.Data.SQLite.Linq.Properties.Resources).Assembly);
        return System.Data.SQLite.Linq.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => System.Data.SQLite.Linq.Properties.Resources.resourceCulture;
      set => System.Data.SQLite.Linq.Properties.Resources.resourceCulture = value;
    }

    internal static string SQL_CONSTRAINTCOLUMNS => System.Data.SQLite.Linq.Properties.Resources.ResourceManager.GetString(nameof (SQL_CONSTRAINTCOLUMNS), System.Data.SQLite.Linq.Properties.Resources.resourceCulture);

    internal static string SQL_CONSTRAINTS => System.Data.SQLite.Linq.Properties.Resources.ResourceManager.GetString(nameof (SQL_CONSTRAINTS), System.Data.SQLite.Linq.Properties.Resources.resourceCulture);
  }
}

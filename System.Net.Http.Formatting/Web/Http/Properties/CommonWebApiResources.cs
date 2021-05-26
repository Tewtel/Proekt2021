// Decompiled with JetBrains decompiler
// Type: System.Web.Http.Properties.CommonWebApiResources
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace System.Web.Http.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class CommonWebApiResources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal CommonWebApiResources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (CommonWebApiResources.resourceMan == null)
        {
          Assembly assembly = typeof (CommonWebApiResources).Assembly;
          string str = ((IEnumerable<string>) assembly.GetManifestResourceNames()).Where<string>((Func<string, bool>) (s => s.EndsWith("CommonWebApiResources.resources", StringComparison.OrdinalIgnoreCase))).Single<string>();
          CommonWebApiResources.resourceMan = new ResourceManager(str.Substring(0, str.Length - 10), assembly);
        }
        return CommonWebApiResources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => CommonWebApiResources.resourceCulture;
      set => CommonWebApiResources.resourceCulture = value;
    }

    internal static string ArgumentInvalidAbsoluteUri => CommonWebApiResources.ResourceManager.GetString(nameof (ArgumentInvalidAbsoluteUri), CommonWebApiResources.resourceCulture);

    internal static string ArgumentInvalidHttpUriScheme => CommonWebApiResources.ResourceManager.GetString(nameof (ArgumentInvalidHttpUriScheme), CommonWebApiResources.resourceCulture);

    internal static string ArgumentMustBeGreaterThanOrEqualTo => CommonWebApiResources.ResourceManager.GetString(nameof (ArgumentMustBeGreaterThanOrEqualTo), CommonWebApiResources.resourceCulture);

    internal static string ArgumentMustBeLessThanOrEqualTo => CommonWebApiResources.ResourceManager.GetString(nameof (ArgumentMustBeLessThanOrEqualTo), CommonWebApiResources.resourceCulture);

    internal static string ArgumentNullOrEmpty => CommonWebApiResources.ResourceManager.GetString(nameof (ArgumentNullOrEmpty), CommonWebApiResources.resourceCulture);

    internal static string ArgumentUriHasQueryOrFragment => CommonWebApiResources.ResourceManager.GetString(nameof (ArgumentUriHasQueryOrFragment), CommonWebApiResources.resourceCulture);

    internal static string InvalidEnumArgument => CommonWebApiResources.ResourceManager.GetString(nameof (InvalidEnumArgument), CommonWebApiResources.resourceCulture);
  }
}

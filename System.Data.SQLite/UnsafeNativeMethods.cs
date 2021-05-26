// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.UnsafeNativeMethods
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security;
using System.Text;
using System.Xml;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class declares P/Invoke methods to call native SQLite APIs.
  /// </summary>
  [SuppressUnmanagedCodeSecurity]
  internal static class UnsafeNativeMethods
  {
    public const string ExceptionMessageFormat = "Caught exception in \"{0}\" method: {1}";
    internal const string SQLITE_DLL = "SQLite.Interop.dll";
    /// <summary>The file extension used for dynamic link libraries.</summary>
    private static readonly string DllFileExtension = ".dll";
    /// <summary>
    /// The file extension used for the XML configuration file.
    /// </summary>
    private static readonly string ConfigFileExtension = ".config";
    /// <summary>
    /// This is the name of the XML configuration file specific to the
    /// System.Data.SQLite assembly.
    /// </summary>
    private static readonly string XmlConfigFileName = typeof (UnsafeNativeMethods).Namespace + UnsafeNativeMethods.DllFileExtension + UnsafeNativeMethods.ConfigFileExtension;
    /// <summary>
    /// This is the XML configuratrion file token that will be replaced with
    /// the qualified path to the directory containing the XML configuration
    /// file.
    /// </summary>
    private static readonly string XmlConfigDirectoryToken = "%PreLoadSQLite_XmlConfigDirectory%";
    /// <summary>
    /// This is the environment variable token that will be replaced with
    /// the qualified path to the directory containing this assembly.
    /// </summary>
    private static readonly string AssemblyDirectoryToken = "%PreLoadSQLite_AssemblyDirectory%";
    /// <summary>
    /// This is the environment variable token that will be replaced with an
    /// abbreviation of the target framework attribute value associated with
    /// this assembly.
    /// </summary>
    private static readonly string TargetFrameworkToken = "%PreLoadSQLite_TargetFramework%";
    /// <summary>
    /// This lock is used to protect the static _SQLiteNativeModuleFileName,
    /// _SQLiteNativeModuleHandle, and processorArchitecturePlatforms fields.
    /// </summary>
    private static readonly object staticSyncRoot = new object();
    /// <summary>
    /// This dictionary stores the mappings between target framework names
    /// and their associated (NuGet) abbreviations.  These mappings are only
    /// used by the <see cref="M:System.Data.SQLite.UnsafeNativeMethods.AbbreviateTargetFramework(System.String)" /> method.
    /// </summary>
    private static Dictionary<string, string> targetFrameworkAbbreviations;
    /// <summary>
    /// This dictionary stores the mappings between processor architecture
    /// names and platform names.  These mappings are now used for two
    /// purposes.  First, they are used to determine if the assembly code
    /// base should be used instead of the location, based upon whether one
    /// or more of the named sub-directories exist within the assembly code
    /// base.  Second, they are used to assist in loading the appropriate
    /// SQLite interop assembly into the current process.
    /// </summary>
    private static Dictionary<string, string> processorArchitecturePlatforms;
    /// <summary>
    /// This is the cached return value from the
    /// <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetAssemblyDirectory" /> method -OR- null if that method
    /// has never returned a valid value.
    /// </summary>
    private static string cachedAssemblyDirectory;
    /// <summary>
    /// When this field is non-zero, it indicates the
    /// <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetAssemblyDirectory" /> method was not able to locate a
    /// suitable assembly directory.  The
    /// <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetCachedAssemblyDirectory" /> method will check this
    /// field and skips calls into the <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetAssemblyDirectory" />
    /// method whenever it is non-zero.
    /// </summary>
    private static bool noAssemblyDirectory;
    /// <summary>
    /// This is the cached return value from the
    /// <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetXmlConfigFileName" /> method -OR- null if that method
    /// has never returned a valid value.
    /// </summary>
    private static string cachedXmlConfigFileName;
    /// <summary>
    /// When this field is non-zero, it indicates the
    /// <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetXmlConfigFileName" /> method was not able to locate a
    /// suitable XML configuration file name.  The
    /// <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetCachedXmlConfigFileName" /> method will check this
    /// field and skips calls into the <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetXmlConfigFileName" />
    /// method whenever it is non-zero.
    /// </summary>
    private static bool noXmlConfigFileName;
    /// <summary>
    /// The name of the environment variable containing the processor
    /// architecture of the current process.
    /// </summary>
    private static readonly string PROCESSOR_ARCHITECTURE = nameof (PROCESSOR_ARCHITECTURE);
    /// <summary>
    /// The native module file name for the native SQLite library or null.
    /// </summary>
    internal static string _SQLiteNativeModuleFileName = (string) null;
    /// <summary>
    /// The native module handle for the native SQLite library or the value
    /// IntPtr.Zero.
    /// </summary>
    private static IntPtr _SQLiteNativeModuleHandle = IntPtr.Zero;

    /// <summary>
    /// For now, this method simply calls the Initialize method.
    /// </summary>
    static UnsafeNativeMethods() => UnsafeNativeMethods.Initialize();

    /// <summary>
    /// Attempts to initialize this class by pre-loading the native SQLite
    /// library for the processor architecture of the current process.
    /// </summary>
    internal static void Initialize()
    {
      HelperMethods.MaybeBreakIntoDebugger();
      if (UnsafeNativeMethods.GetSettingValue("No_PreLoadSQLite", (string) null) != null)
        return;
      lock (UnsafeNativeMethods.staticSyncRoot)
      {
        if (UnsafeNativeMethods.targetFrameworkAbbreviations == null)
        {
          UnsafeNativeMethods.targetFrameworkAbbreviations = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v2.0", "net20");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v3.5", "net35");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.0", "net40");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.5", "net45");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.5.1", "net451");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.5.2", "net452");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.6", "net46");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.6.1", "net461");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.6.2", "net462");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.7", "net47");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.7.1", "net471");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.7.2", "net472");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETFramework,Version=v4.8", "net48");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETStandard,Version=v2.0", "netstandard2.0");
          UnsafeNativeMethods.targetFrameworkAbbreviations.Add(".NETStandard,Version=v2.1", "netstandard2.1");
        }
        if (UnsafeNativeMethods.processorArchitecturePlatforms == null)
        {
          UnsafeNativeMethods.processorArchitecturePlatforms = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
          UnsafeNativeMethods.processorArchitecturePlatforms.Add("x86", "Win32");
          UnsafeNativeMethods.processorArchitecturePlatforms.Add("x86_64", "x64");
          UnsafeNativeMethods.processorArchitecturePlatforms.Add("AMD64", "x64");
          UnsafeNativeMethods.processorArchitecturePlatforms.Add("IA64", "Itanium");
          UnsafeNativeMethods.processorArchitecturePlatforms.Add("ARM", "WinCE");
        }
        if (!(UnsafeNativeMethods._SQLiteNativeModuleHandle == IntPtr.Zero))
          return;
        string baseDirectory = (string) null;
        string processorArchitecture = (string) null;
        bool allowBaseDirectoryOnly = false;
        UnsafeNativeMethods.SearchForDirectory(ref baseDirectory, ref processorArchitecture, ref allowBaseDirectoryOnly);
        UnsafeNativeMethods.PreLoadSQLiteDll(baseDirectory, processorArchitecture, allowBaseDirectoryOnly, ref UnsafeNativeMethods._SQLiteNativeModuleFileName, ref UnsafeNativeMethods._SQLiteNativeModuleHandle);
      }
    }

    /// <summary>Combines two path strings.</summary>
    /// <param name="path1">The first path -OR- null.</param>
    /// <param name="path2">The second path -OR- null.</param>
    /// <returns>
    /// The combined path string -OR- null if both of the original path
    /// strings are null.
    /// </returns>
    private static string MaybeCombinePath(string path1, string path2) => path1 != null ? (path2 != null ? Path.Combine(path1, path2) : path1) : (path2 != null ? path2 : (string) null);

    /// <summary>
    /// Resets the cached XML configuration file name value, thus forcing the
    /// next call to <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetCachedXmlConfigFileName" /> method to rely
    /// upon the <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetXmlConfigFileName" /> method to fetch the
    /// XML configuration file name.
    /// </summary>
    private static void ResetCachedXmlConfigFileName()
    {
      lock (UnsafeNativeMethods.staticSyncRoot)
      {
        UnsafeNativeMethods.cachedXmlConfigFileName = (string) null;
        UnsafeNativeMethods.noXmlConfigFileName = false;
      }
    }

    /// <summary>
    /// Queries and returns the cached XML configuration file name for the
    /// assembly containing the managed System.Data.SQLite components, if
    /// available.  If the cached XML configuration file name value is not
    /// available, the <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetXmlConfigFileName" /> method will
    /// be used to obtain the XML configuration file name.
    /// </summary>
    /// <returns>
    /// The XML configuration file name -OR- null if it cannot be determined
    /// or does not exist.
    /// </returns>
    private static string GetCachedXmlConfigFileName()
    {
      lock (UnsafeNativeMethods.staticSyncRoot)
      {
        if (UnsafeNativeMethods.cachedXmlConfigFileName != null)
          return UnsafeNativeMethods.cachedXmlConfigFileName;
        if (UnsafeNativeMethods.noXmlConfigFileName)
          return (string) null;
      }
      return UnsafeNativeMethods.GetXmlConfigFileName();
    }

    /// <summary>
    /// Queries and returns the XML configuration file name for the assembly
    /// containing the managed System.Data.SQLite components.
    /// </summary>
    /// <returns>
    /// The XML configuration file name -OR- null if it cannot be determined
    /// or does not exist.
    /// </returns>
    private static string GetXmlConfigFileName()
    {
      string path1 = UnsafeNativeMethods.MaybeCombinePath(AppDomain.CurrentDomain.BaseDirectory, UnsafeNativeMethods.XmlConfigFileName);
      if (File.Exists(path1))
      {
        lock (UnsafeNativeMethods.staticSyncRoot)
          UnsafeNativeMethods.cachedXmlConfigFileName = path1;
        return path1;
      }
      string path2 = UnsafeNativeMethods.MaybeCombinePath(UnsafeNativeMethods.GetCachedAssemblyDirectory(), UnsafeNativeMethods.XmlConfigFileName);
      if (File.Exists(path2))
      {
        lock (UnsafeNativeMethods.staticSyncRoot)
          UnsafeNativeMethods.cachedXmlConfigFileName = path2;
        return path2;
      }
      lock (UnsafeNativeMethods.staticSyncRoot)
        UnsafeNativeMethods.noXmlConfigFileName = true;
      return (string) null;
    }

    /// <summary>
    /// If necessary, replaces all supported XML configuration file tokens
    /// with their associated values.
    /// </summary>
    /// <param name="fileName">
    /// The name of the XML configuration file being read.
    /// </param>
    /// <param name="value">
    /// A setting value read from the XML configuration file.
    /// </param>
    /// <returns>
    /// The value of the <paramref name="value" /> will all supported XML
    /// configuration file tokens replaced.  No return value is reserved
    /// to indicate an error.  This method cannot fail.
    /// </returns>
    private static string ReplaceXmlConfigFileTokens(string fileName, string value)
    {
      if (!string.IsNullOrEmpty(value))
      {
        if (!string.IsNullOrEmpty(fileName))
        {
          try
          {
            string directoryName = Path.GetDirectoryName(fileName);
            if (!string.IsNullOrEmpty(directoryName))
              value = value.Replace(UnsafeNativeMethods.XmlConfigDirectoryToken, directoryName);
          }
          catch (Exception ex)
          {
            try
            {
              Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader failed to replace XML configuration file \"{0}\" tokens: {1}", (object) fileName, (object) ex));
            }
            catch
            {
            }
          }
        }
      }
      return value;
    }

    /// <summary>
    /// Queries and returns the value of the specified setting, using the
    /// specified XML configuration file.
    /// </summary>
    /// <param name="fileName">
    /// The name of the XML configuration file to read.
    /// </param>
    /// <param name="name">The name of the setting.</param>
    /// <param name="default">
    /// The value to be returned if the setting has not been set explicitly
    /// or cannot be determined.
    /// </param>
    /// <param name="expand">
    /// Non-zero to expand any environment variable references contained in
    /// the setting value to be returned.  This has no effect on the .NET
    /// Compact Framework.
    /// </param>
    /// <returns>
    /// The value of the setting -OR- the default value specified by
    /// <paramref name="default" /> if it has not been set explicitly or
    /// cannot be determined.
    /// </returns>
    private static string GetSettingValueViaXmlConfigFile(
      string fileName,
      string name,
      string @default,
      bool expand)
    {
      try
      {
        if (fileName == null || name == null)
          return @default;
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(fileName);
        if (xmlDocument.SelectSingleNode(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "/configuration/appSettings/add[@key='{0}']", (object) name)) is XmlElement xmlElement2)
        {
          string name1 = (string) null;
          if (xmlElement2.HasAttribute("value"))
            name1 = xmlElement2.GetAttribute("value");
          if (!string.IsNullOrEmpty(name1))
          {
            if (expand)
              name1 = Environment.ExpandEnvironmentVariables(name1);
            string str = UnsafeNativeMethods.ReplaceEnvironmentVariableTokens(name1);
            name1 = UnsafeNativeMethods.ReplaceXmlConfigFileTokens(fileName, str);
          }
          if (name1 != null)
            return name1;
        }
      }
      catch (Exception ex)
      {
        try
        {
          Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader failed to get setting \"{0}\" value from XML configuration file \"{1}\": {2}", (object) name, (object) fileName, (object) ex));
        }
        catch
        {
        }
      }
      return @default;
    }

    /// <summary>
    /// Attempts to determine the target framework attribute value that is
    /// associated with the specified managed assembly, if applicable.
    /// </summary>
    /// <param name="assembly">
    /// The managed assembly to read the target framework attribute value
    /// from.
    /// </param>
    /// <returns>
    /// The value of the target framework attribute value for the specified
    /// managed assembly -OR- null if it cannot be determined.  If this
    /// assembly was compiled with a version of the .NET Framework prior to
    /// version 4.0, the value returned MAY reflect that version of the .NET
    /// Framework instead of the one associated with the specified managed
    /// assembly.
    /// </returns>
    private static string GetAssemblyTargetFramework(Assembly assembly)
    {
      if (assembly != (Assembly) null)
      {
        try
        {
          if (assembly.IsDefined(typeof (TargetFrameworkAttribute), false))
            return ((TargetFrameworkAttribute) assembly.GetCustomAttributes(typeof (TargetFrameworkAttribute), false)[0]).FrameworkName;
        }
        catch
        {
        }
      }
      return (string) null;
    }

    /// <summary>
    /// Accepts a long target framework attribute value and makes it into a
    /// much shorter version, suitable for use with NuGet packages.
    /// </summary>
    /// <param name="targetFramework">
    /// The long target framework attribute value to convert.
    /// </param>
    /// <returns>
    /// The short target framework attribute value -OR- null if it cannot
    /// be determined or converted.
    /// </returns>
    private static string AbbreviateTargetFramework(string targetFramework)
    {
      if (!string.IsNullOrEmpty(targetFramework))
      {
        lock (UnsafeNativeMethods.staticSyncRoot)
        {
          if (UnsafeNativeMethods.targetFrameworkAbbreviations != null)
          {
            string str;
            if (UnsafeNativeMethods.targetFrameworkAbbreviations.TryGetValue(targetFramework, out str))
              return str;
          }
        }
        if (targetFramework.IndexOf(".NETFramework,Version=v") != -1)
        {
          string str = targetFramework.Replace(".NETFramework,Version=v", "net").Replace(".", string.Empty);
          int length = str.IndexOf(',');
          return length != -1 ? str.Substring(0, length) : str;
        }
      }
      return targetFramework;
    }

    /// <summary>
    /// If necessary, replaces all supported environment variable tokens
    /// with their associated values.
    /// </summary>
    /// <param name="value">
    /// A setting value read from an environment variable.
    /// </param>
    /// <returns>
    /// The value of the <paramref name="value" /> will all supported
    /// environment variable tokens replaced.  No return value is reserved
    /// to indicate an error.  This method cannot fail.
    /// </returns>
    private static string ReplaceEnvironmentVariableTokens(string value)
    {
      if (!string.IsNullOrEmpty(value))
      {
        string assemblyDirectory = UnsafeNativeMethods.GetCachedAssemblyDirectory();
        if (!string.IsNullOrEmpty(assemblyDirectory))
        {
          try
          {
            value = value.Replace(UnsafeNativeMethods.AssemblyDirectoryToken, assemblyDirectory);
          }
          catch (Exception ex)
          {
            try
            {
              Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader failed to replace assembly directory token: {0}", (object) ex));
            }
            catch
            {
            }
          }
        }
        Assembly assembly = (Assembly) null;
        try
        {
          assembly = Assembly.GetExecutingAssembly();
        }
        catch (Exception ex)
        {
          try
          {
            Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader failed to obtain executing assembly: {0}", (object) ex));
          }
          catch
          {
          }
        }
        string newValue = UnsafeNativeMethods.AbbreviateTargetFramework(UnsafeNativeMethods.GetAssemblyTargetFramework(assembly));
        if (!string.IsNullOrEmpty(newValue))
        {
          try
          {
            value = value.Replace(UnsafeNativeMethods.TargetFrameworkToken, newValue);
          }
          catch (Exception ex)
          {
            try
            {
              Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader failed to replace target framework token: {0}", (object) ex));
            }
            catch
            {
            }
          }
        }
      }
      return value;
    }

    /// <summary>
    /// Queries and returns the value of the specified setting, using the XML
    /// configuration file and/or the environment variables for the current
    /// process and/or the current system, when available.
    /// </summary>
    /// <param name="name">The name of the setting.</param>
    /// <param name="default">
    /// The value to be returned if the setting has not been set explicitly
    /// or cannot be determined.
    /// </param>
    /// <returns>
    /// The value of the setting -OR- the default value specified by
    /// <paramref name="default" /> if it has not been set explicitly or
    /// cannot be determined.  By default, all references to existing
    /// environment variables will be expanded to their corresponding values
    /// within the value to be returned unless either the "No_Expand" or
    /// "No_Expand_<paramref name="name" />" environment variable is set [to
    /// anything].
    /// </returns>
    internal static string GetSettingValue(string name, string @default)
    {
      if (Environment.GetEnvironmentVariable("No_SQLiteGetSettingValue") != null || name == null)
        return @default;
      bool expand = true;
      if (Environment.GetEnvironmentVariable("No_Expand") != null)
        expand = false;
      else if (Environment.GetEnvironmentVariable(HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, "No_Expand_{0}", (object) name)) != null)
        expand = false;
      string name1 = Environment.GetEnvironmentVariable(name);
      if (!string.IsNullOrEmpty(name1))
      {
        if (expand)
          name1 = Environment.ExpandEnvironmentVariables(name1);
        name1 = UnsafeNativeMethods.ReplaceEnvironmentVariableTokens(name1);
      }
      if (name1 != null)
        return name1;
      return Environment.GetEnvironmentVariable("No_SQLiteXmlConfigFile") != null ? @default : UnsafeNativeMethods.GetSettingValueViaXmlConfigFile(UnsafeNativeMethods.GetCachedXmlConfigFileName(), name, @default, expand);
    }

    private static string ListToString(IList<string> list)
    {
      if (list == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string str in (IEnumerable<string>) list)
      {
        if (str != null)
        {
          if (stringBuilder.Length > 0)
            stringBuilder.Append(' ');
          stringBuilder.Append(str);
        }
      }
      return stringBuilder.ToString();
    }

    private static int CheckForArchitecturesAndPlatforms(string directory, ref List<string> matches)
    {
      int num = 0;
      if (matches == null)
        matches = new List<string>();
      lock (UnsafeNativeMethods.staticSyncRoot)
      {
        if (!string.IsNullOrEmpty(directory))
        {
          if (UnsafeNativeMethods.processorArchitecturePlatforms != null)
          {
            foreach (KeyValuePair<string, string> architecturePlatform in UnsafeNativeMethods.processorArchitecturePlatforms)
            {
              if (Directory.Exists(UnsafeNativeMethods.MaybeCombinePath(directory, architecturePlatform.Key)))
              {
                matches.Add(architecturePlatform.Key);
                ++num;
              }
              string path2 = architecturePlatform.Value;
              if (path2 != null && Directory.Exists(UnsafeNativeMethods.MaybeCombinePath(directory, path2)))
              {
                matches.Add(path2);
                ++num;
              }
            }
          }
        }
      }
      return num;
    }

    private static bool CheckAssemblyCodeBase(Assembly assembly, ref string fileName)
    {
      try
      {
        if (assembly == (Assembly) null)
          return false;
        string codeBase = assembly.CodeBase;
        if (string.IsNullOrEmpty(codeBase))
          return false;
        string localPath = new Uri(codeBase).LocalPath;
        if (!File.Exists(localPath))
          return false;
        string directoryName = Path.GetDirectoryName(localPath);
        if (File.Exists(UnsafeNativeMethods.MaybeCombinePath(directoryName, UnsafeNativeMethods.XmlConfigFileName)))
        {
          fileName = localPath;
          return true;
        }
        List<string> matches = (List<string>) null;
        if (UnsafeNativeMethods.CheckForArchitecturesAndPlatforms(directoryName, ref matches) <= 0)
          return false;
        fileName = localPath;
        return true;
      }
      catch (Exception ex)
      {
        try
        {
          Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader failed to check code base for currently executing assembly: {0}", (object) ex));
        }
        catch
        {
        }
      }
      return false;
    }

    /// <summary>
    /// Resets the cached assembly directory value, thus forcing the next
    /// call to <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetCachedAssemblyDirectory" /> method to rely
    /// upon the <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetAssemblyDirectory" /> method to fetch the
    /// assembly directory.
    /// </summary>
    private static void ResetCachedAssemblyDirectory()
    {
      lock (UnsafeNativeMethods.staticSyncRoot)
      {
        UnsafeNativeMethods.cachedAssemblyDirectory = (string) null;
        UnsafeNativeMethods.noAssemblyDirectory = false;
      }
    }

    /// <summary>
    /// Queries and returns the cached directory for the assembly currently
    /// being executed, if available.  If the cached assembly directory value
    /// is not available, the <see cref="M:System.Data.SQLite.UnsafeNativeMethods.GetAssemblyDirectory" /> method will
    /// be used to obtain the assembly directory.
    /// </summary>
    /// <returns>
    /// The directory for the assembly currently being executed -OR- null if
    /// it cannot be determined.
    /// </returns>
    private static string GetCachedAssemblyDirectory()
    {
      lock (UnsafeNativeMethods.staticSyncRoot)
      {
        if (UnsafeNativeMethods.cachedAssemblyDirectory != null)
          return UnsafeNativeMethods.cachedAssemblyDirectory;
        if (UnsafeNativeMethods.noAssemblyDirectory)
          return (string) null;
      }
      return UnsafeNativeMethods.GetAssemblyDirectory();
    }

    /// <summary>
    /// Queries and returns the directory for the assembly currently being
    /// executed.
    /// </summary>
    /// <returns>
    /// The directory for the assembly currently being executed -OR- null if
    /// it cannot be determined.
    /// </returns>
    private static string GetAssemblyDirectory()
    {
      try
      {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        if (executingAssembly == (Assembly) null)
        {
          lock (UnsafeNativeMethods.staticSyncRoot)
            UnsafeNativeMethods.noAssemblyDirectory = true;
          return (string) null;
        }
        string fileName = (string) null;
        if (!UnsafeNativeMethods.CheckAssemblyCodeBase(executingAssembly, ref fileName))
          fileName = executingAssembly.Location;
        if (string.IsNullOrEmpty(fileName))
        {
          lock (UnsafeNativeMethods.staticSyncRoot)
            UnsafeNativeMethods.noAssemblyDirectory = true;
          return (string) null;
        }
        string directoryName = Path.GetDirectoryName(fileName);
        if (string.IsNullOrEmpty(directoryName))
        {
          lock (UnsafeNativeMethods.staticSyncRoot)
            UnsafeNativeMethods.noAssemblyDirectory = true;
          return (string) null;
        }
        lock (UnsafeNativeMethods.staticSyncRoot)
          UnsafeNativeMethods.cachedAssemblyDirectory = directoryName;
        return directoryName;
      }
      catch (Exception ex)
      {
        try
        {
          Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader failed to get directory for currently executing assembly: {0}", (object) ex));
        }
        catch
        {
        }
      }
      lock (UnsafeNativeMethods.staticSyncRoot)
        UnsafeNativeMethods.noAssemblyDirectory = true;
      return (string) null;
    }

    /// <summary>
    /// Determines the base file name (without any directory information)
    /// for the native SQLite library to be pre-loaded by this class.
    /// </summary>
    /// <returns>
    /// The base file name for the native SQLite library to be pre-loaded by
    /// this class -OR- null if its value cannot be determined.
    /// </returns>
    internal static string GetNativeLibraryFileNameOnly() => UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_LibraryFileNameOnly", (string) null) ?? "SQLite.Interop.dll";

    /// <summary>
    /// Searches for the native SQLite library in the directory containing
    /// the assembly currently being executed as well as the base directory
    /// for the current application domain.
    /// </summary>
    /// <param name="baseDirectory">
    /// Upon success, this parameter will be modified to refer to the base
    /// directory containing the native SQLite library.
    /// </param>
    /// <param name="processorArchitecture">
    /// Upon success, this parameter will be modified to refer to the name
    /// of the immediate directory (i.e. the offset from the base directory)
    /// containing the native SQLite library.
    /// </param>
    /// <param name="allowBaseDirectoryOnly">
    /// Upon success, this parameter will be modified to non-zero only if
    /// the base directory itself should be allowed for loading the native
    /// library.
    /// </param>
    /// <returns>
    /// Non-zero (success) if the native SQLite library was found; otherwise,
    /// zero (failure).
    /// </returns>
    private static bool SearchForDirectory(
      ref string baseDirectory,
      ref string processorArchitecture,
      ref bool allowBaseDirectoryOnly)
    {
      if (UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_NoSearchForDirectory", (string) null) != null)
        return false;
      string libraryFileNameOnly = UnsafeNativeMethods.GetNativeLibraryFileNameOnly();
      if (libraryFileNameOnly == null)
        return false;
      string[] strArray1 = new string[2]
      {
        UnsafeNativeMethods.GetAssemblyDirectory(),
        AppDomain.CurrentDomain.BaseDirectory
      };
      string str = (string) null;
      if (UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_AllowBaseDirectoryOnly", (string) null) != null || HelperMethods.IsDotNetCore() && !HelperMethods.IsWindows())
        str = string.Empty;
      string[] strArray2 = new string[3]
      {
        UnsafeNativeMethods.GetProcessorArchitecture(),
        UnsafeNativeMethods.GetPlatformName((string) null),
        str
      };
      foreach (string path1 in strArray1)
      {
        if (path1 != null)
        {
          foreach (string path2 in strArray2)
          {
            if (path2 != null && File.Exists(UnsafeNativeMethods.FixUpDllFileName(UnsafeNativeMethods.MaybeCombinePath(UnsafeNativeMethods.MaybeCombinePath(path1, path2), libraryFileNameOnly))))
            {
              baseDirectory = path1;
              processorArchitecture = path2;
              allowBaseDirectoryOnly = path2.Length == 0;
              return true;
            }
          }
        }
      }
      return false;
    }

    /// <summary>
    /// Queries and returns the base directory of the current application
    /// domain.
    /// </summary>
    /// <returns>
    /// The base directory for the current application domain -OR- null if it
    /// cannot be determined.
    /// </returns>
    private static string GetBaseDirectory()
    {
      string settingValue = UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_BaseDirectory", (string) null);
      if (settingValue != null)
        return settingValue;
      if (UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_UseAssemblyDirectory", (string) null) != null)
      {
        string assemblyDirectory = UnsafeNativeMethods.GetAssemblyDirectory();
        if (assemblyDirectory != null)
          return assemblyDirectory;
      }
      return AppDomain.CurrentDomain.BaseDirectory;
    }

    /// <summary>
    /// Determines if the dynamic link library file name requires a suffix
    /// and adds it if necessary.
    /// </summary>
    /// <param name="fileName">
    /// The original dynamic link library file name to inspect.
    /// </param>
    /// <returns>
    /// The dynamic link library file name, possibly modified to include an
    /// extension.
    /// </returns>
    private static string FixUpDllFileName(string fileName) => !string.IsNullOrEmpty(fileName) && HelperMethods.IsWindows() && !fileName.EndsWith(UnsafeNativeMethods.DllFileExtension, StringComparison.OrdinalIgnoreCase) ? fileName + UnsafeNativeMethods.DllFileExtension : fileName;

    /// <summary>
    /// Queries and returns the processor architecture of the current
    /// process.
    /// </summary>
    /// <returns>
    /// The processor architecture of the current process -OR- null if it
    /// cannot be determined.
    /// </returns>
    private static string GetProcessorArchitecture()
    {
      string settingValue = UnsafeNativeMethods.GetSettingValue("PreLoadSQLite_ProcessorArchitecture", (string) null);
      if (settingValue != null)
        return settingValue;
      string a = UnsafeNativeMethods.GetSettingValue(UnsafeNativeMethods.PROCESSOR_ARCHITECTURE, (string) null);
      if (IntPtr.Size == 4 && string.Equals(a, "AMD64", StringComparison.OrdinalIgnoreCase))
        a = "x86";
      if (a == null)
        a = NativeLibraryHelper.GetMachine() ?? string.Empty;
      return a;
    }

    /// <summary>
    /// Given the processor architecture, returns the name of the platform.
    /// </summary>
    /// <param name="processorArchitecture">
    /// The processor architecture to be translated to a platform name.
    /// </param>
    /// <returns>
    /// The platform name for the specified processor architecture -OR- null
    /// if it cannot be determined.
    /// </returns>
    private static string GetPlatformName(string processorArchitecture)
    {
      if (processorArchitecture == null)
        processorArchitecture = UnsafeNativeMethods.GetProcessorArchitecture();
      if (string.IsNullOrEmpty(processorArchitecture))
        return (string) null;
      lock (UnsafeNativeMethods.staticSyncRoot)
      {
        if (UnsafeNativeMethods.processorArchitecturePlatforms == null)
          return (string) null;
        string str;
        if (UnsafeNativeMethods.processorArchitecturePlatforms.TryGetValue(processorArchitecture, out str))
          return str;
      }
      return (string) null;
    }

    /// <summary>
    /// Attempts to load the native SQLite library based on the specified
    /// directory and processor architecture.
    /// </summary>
    /// <param name="baseDirectory">
    /// The base directory to use, null for default (the base directory of
    /// the current application domain).  This directory should contain the
    /// processor architecture specific sub-directories.
    /// </param>
    /// <param name="processorArchitecture">
    /// The requested processor architecture, null for default (the
    /// processor architecture of the current process).  This caller should
    /// almost always specify null for this parameter.
    /// </param>
    /// <param name="allowBaseDirectoryOnly">
    /// Non-zero indicates that the native SQLite library can be loaded
    /// from the base directory itself.
    /// </param>
    /// <param name="nativeModuleFileName">
    /// The candidate native module file name to load will be stored here,
    /// if necessary.
    /// </param>
    /// <param name="nativeModuleHandle">
    /// The native module handle as returned by LoadLibrary will be stored
    /// here, if necessary.  This value will be IntPtr.Zero if the call to
    /// LoadLibrary fails.
    /// </param>
    /// <returns>
    /// Non-zero if the native module was loaded successfully; otherwise,
    /// zero.
    /// </returns>
    private static bool PreLoadSQLiteDll(
      string baseDirectory,
      string processorArchitecture,
      bool allowBaseDirectoryOnly,
      ref string nativeModuleFileName,
      ref IntPtr nativeModuleHandle)
    {
      if (baseDirectory == null)
        baseDirectory = UnsafeNativeMethods.GetBaseDirectory();
      if (baseDirectory == null)
        return false;
      string libraryFileNameOnly = UnsafeNativeMethods.GetNativeLibraryFileNameOnly();
      if (libraryFileNameOnly == null)
        return false;
      string str = UnsafeNativeMethods.FixUpDllFileName(UnsafeNativeMethods.MaybeCombinePath(baseDirectory, libraryFileNameOnly));
      if (File.Exists(str))
      {
        if (allowBaseDirectoryOnly)
        {
          if (string.IsNullOrEmpty(processorArchitecture))
            goto label_19;
        }
        return false;
      }
      if (processorArchitecture == null)
        processorArchitecture = UnsafeNativeMethods.GetProcessorArchitecture();
      if (processorArchitecture == null)
        return false;
      str = UnsafeNativeMethods.FixUpDllFileName(UnsafeNativeMethods.MaybeCombinePath(UnsafeNativeMethods.MaybeCombinePath(baseDirectory, processorArchitecture), libraryFileNameOnly));
      if (!File.Exists(str))
      {
        string platformName = UnsafeNativeMethods.GetPlatformName(processorArchitecture);
        if (platformName == null)
          return false;
        str = UnsafeNativeMethods.FixUpDllFileName(UnsafeNativeMethods.MaybeCombinePath(UnsafeNativeMethods.MaybeCombinePath(baseDirectory, platformName), libraryFileNameOnly));
        if (!File.Exists(str))
          return false;
      }
label_19:
      try
      {
        try
        {
          Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader is trying to load native SQLite library \"{0}\"...", (object) str));
        }
        catch
        {
        }
        nativeModuleFileName = str;
        nativeModuleHandle = NativeLibraryHelper.LoadLibrary(str);
        return nativeModuleHandle != IntPtr.Zero;
      }
      catch (Exception ex)
      {
        try
        {
          int lastWin32Error = Marshal.GetLastWin32Error();
          Trace.WriteLine(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Native library pre-loader failed to load native SQLite library \"{0}\" (getLastError = {1}): {2}", (object) str, (object) lastWin32Error, (object) ex));
        }
        catch
        {
        }
      }
      return false;
    }

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_bind_parameter_name_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_database_name_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_database_name16_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_decltype_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_decltype16_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_name_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_name16_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_origin_name_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_origin_name16_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_table_name_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_table_name16_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_text_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_column_text16_interop(
      IntPtr stmt,
      int index,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_errmsg_interop(IntPtr db, ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_prepare_interop(
      IntPtr db,
      IntPtr pSql,
      int nBytes,
      ref IntPtr stmt,
      ref IntPtr ptrRemain,
      ref int nRemain);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_table_column_metadata_interop(
      IntPtr db,
      byte[] dbName,
      byte[] tblName,
      byte[] colName,
      ref IntPtr ptrDataType,
      ref IntPtr ptrCollSeq,
      ref int notNull,
      ref int primaryKey,
      ref int autoInc,
      ref int dtLen,
      ref int csLen);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_value_text_interop(IntPtr p, ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_value_text16_interop(IntPtr p, ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern int sqlite3_malloc_size_interop(IntPtr p);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr interop_libversion();

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr interop_sourceid();

    [DllImport("SQLite.Interop.dll")]
    internal static extern int interop_compileoption_used(IntPtr zOptName);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr interop_compileoption_get(int N);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_close_interop(IntPtr db);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_create_function_interop(
      IntPtr db,
      byte[] strName,
      int nArgs,
      int nType,
      IntPtr pvUser,
      SQLiteCallback func,
      SQLiteCallback fstep,
      SQLiteFinalCallback ffinal,
      int needCollSeq);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_finalize_interop(IntPtr stmt);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_backup_finish_interop(
      IntPtr backup);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_blob_close_interop(IntPtr blob);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_open_interop(
      byte[] utf8Filename,
      byte[] vfsName,
      SQLiteOpenFlagsEnum flags,
      int extFuncs,
      ref IntPtr db);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_open16_interop(
      byte[] utf8Filename,
      byte[] vfsName,
      SQLiteOpenFlagsEnum flags,
      int extFuncs,
      ref IntPtr db);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_reset_interop(IntPtr stmt);

    [DllImport("SQLite.Interop.dll")]
    internal static extern int sqlite3_changes_interop(IntPtr db);

    [DllImport("SQLite.Interop.dll")]
    internal static extern IntPtr sqlite3_context_collseq_interop(
      IntPtr context,
      ref int type,
      ref int enc,
      ref int len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern int sqlite3_context_collcompare_interop(
      IntPtr context,
      byte[] p1,
      int p1len,
      byte[] p2,
      int p2len);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_cursor_rowid_interop(
      IntPtr stmt,
      int cursor,
      ref long rowid);

    [DllImport("SQLite.Interop.dll")]
    internal static extern SQLiteErrorCode sqlite3_index_column_info_interop(
      IntPtr db,
      byte[] catalog,
      byte[] IndexName,
      byte[] ColumnName,
      ref int sortOrder,
      ref int onError,
      ref IntPtr Collation,
      ref int colllen);

    [DllImport("SQLite.Interop.dll")]
    internal static extern int sqlite3_table_cursor_interop(IntPtr stmt, int db, int tableRootPage);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_libversion();

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_libversion_number();

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_sourceid();

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_compileoption_used(IntPtr zOptName);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_compileoption_get(int N);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_enable_shared_cache(int enable);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_enable_load_extension(
      IntPtr db,
      int enable);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_load_extension(
      IntPtr db,
      byte[] fileName,
      byte[] procName,
      ref IntPtr pError);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_overload_function(
      IntPtr db,
      IntPtr zName,
      int nArgs);

    [DllImport("SQLite.Interop.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_win32_set_directory(
      uint type,
      string value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_win32_reset_heap();

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_win32_compact_heap(
      ref uint largest);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_malloc(int n);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_malloc64(ulong n);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_realloc(IntPtr p, int n);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_realloc64(IntPtr p, ulong n);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ulong sqlite3_msize(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_free(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_open_v2(
      byte[] utf8Filename,
      ref IntPtr db,
      SQLiteOpenFlagsEnum flags,
      byte[] vfsName);

    [DllImport("SQLite.Interop.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_open16(
      string fileName,
      ref IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_interrupt(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern long sqlite3_last_insert_rowid(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_changes(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern long sqlite3_memory_used();

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern long sqlite3_memory_highwater(int resetFlag);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_shutdown();

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_busy_timeout(IntPtr db, int ms);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_clear_bindings(IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_blob(
      IntPtr stmt,
      int index,
      byte[] value,
      int nSize,
      IntPtr nTransient);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_double(
      IntPtr stmt,
      int index,
      double value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_int(
      IntPtr stmt,
      int index,
      int value);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_bind_int", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_uint(
      IntPtr stmt,
      int index,
      uint value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_int64(
      IntPtr stmt,
      int index,
      long value);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_bind_int64", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_uint64(
      IntPtr stmt,
      int index,
      ulong value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_null(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_text(
      IntPtr stmt,
      int index,
      byte[] value,
      int nlen,
      IntPtr pvReserved);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_bind_parameter_count(IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_bind_parameter_index(IntPtr stmt, byte[] strName);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_column_count(IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_step(IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_stmt_readonly(IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern double sqlite3_column_double(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_column_int(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern long sqlite3_column_int64(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_column_blob(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_column_bytes(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_column_bytes16(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern TypeAffinity sqlite3_column_type(IntPtr stmt, int index);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_create_collation(
      IntPtr db,
      byte[] strName,
      int nType,
      IntPtr pvUser,
      SQLiteCollation func);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_aggregate_count(IntPtr context);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_value_blob(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_value_bytes(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_value_bytes16(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern double sqlite3_value_double(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_value_int(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern long sqlite3_value_int64(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern TypeAffinity sqlite3_value_type(IntPtr p);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_blob(
      IntPtr context,
      byte[] value,
      int nSize,
      IntPtr pvReserved);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_double(IntPtr context, double value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_error(IntPtr context, byte[] strErr, int nLen);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_error_code(IntPtr context, SQLiteErrorCode value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_error_toobig(IntPtr context);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_error_nomem(IntPtr context);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_value(IntPtr context, IntPtr value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_zeroblob(IntPtr context, int nLen);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_int(IntPtr context, int value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_int64(IntPtr context, long value);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_null(IntPtr context);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_text(
      IntPtr context,
      byte[] value,
      int nLen,
      IntPtr pvReserved);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_aggregate_context(IntPtr context, int nBytes);

    [DllImport("SQLite.Interop.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_bind_text16(
      IntPtr stmt,
      int index,
      string value,
      int nlen,
      IntPtr pvReserved);

    [DllImport("SQLite.Interop.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_error16(IntPtr context, string strName, int nLen);

    [DllImport("SQLite.Interop.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_result_text16(
      IntPtr context,
      string strName,
      int nLen,
      IntPtr pvReserved);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_key(
      IntPtr db,
      byte[] key,
      int keylen);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_rekey(
      IntPtr db,
      byte[] key,
      int keylen);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_progress_handler(
      IntPtr db,
      int ops,
      SQLiteProgressCallback func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_set_authorizer(
      IntPtr db,
      SQLiteAuthorizerCallback func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_update_hook(
      IntPtr db,
      SQLiteUpdateCallback func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_commit_hook(
      IntPtr db,
      SQLiteCommitCallback func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_trace(
      IntPtr db,
      SQLiteTraceCallback func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_trace_v2(
      IntPtr db,
      SQLiteTraceFlags mask,
      SQLiteTraceCallback2 func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_limit(IntPtr db, SQLiteLimitOpsEnum op, int value);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_config", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_config_none(SQLiteConfigOpsEnum op);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_config", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_config_int(
      SQLiteConfigOpsEnum op,
      int value);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_config", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_config_log(
      SQLiteConfigOpsEnum op,
      SQLiteLogCallback func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_db_config", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_db_config_charptr(
      IntPtr db,
      SQLiteConfigDbOpsEnum op,
      IntPtr charPtr);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_db_config", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_db_config_int_refint(
      IntPtr db,
      SQLiteConfigDbOpsEnum op,
      int value,
      ref int result);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_db_config", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_db_config_intptr_two_ints(
      IntPtr db,
      SQLiteConfigDbOpsEnum op,
      IntPtr ptr,
      int int0,
      int int1);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_db_status(
      IntPtr db,
      SQLiteStatusOpsEnum op,
      ref int current,
      ref int highwater,
      int resetFlag);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_rollback_hook(
      IntPtr db,
      SQLiteRollbackCallback func,
      IntPtr pvUser);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_db_handle(IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_db_release_memory(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_db_filename(IntPtr db, IntPtr dbName);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_db_readonly(IntPtr db, IntPtr dbName);

    [DllImport("SQLite.Interop.dll", EntryPoint = "sqlite3_db_filename", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_db_filename_bytes(IntPtr db, byte[] dbName);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_next_stmt(IntPtr db, IntPtr stmt);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_exec(
      IntPtr db,
      byte[] strSql,
      IntPtr pvCallback,
      IntPtr pvParam,
      ref IntPtr errMsg);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_release_memory(int nBytes);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_get_autocommit(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_extended_result_codes(
      IntPtr db,
      int onoff);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_errcode(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_extended_errcode(IntPtr db);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_errstr(SQLiteErrorCode rc);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_log(SQLiteErrorCode iErrCode, byte[] zFormat);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_file_control(
      IntPtr db,
      byte[] zDbName,
      int op,
      IntPtr pArg);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_backup_init(
      IntPtr destDb,
      byte[] zDestName,
      IntPtr sourceDb,
      byte[] zSourceName);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_backup_step(
      IntPtr backup,
      int nPage);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_backup_remaining(IntPtr backup);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_backup_pagecount(IntPtr backup);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_blob_close(IntPtr blob);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3_blob_bytes(IntPtr blob);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_blob_open(
      IntPtr db,
      byte[] dbName,
      byte[] tblName,
      byte[] colName,
      long rowId,
      int flags,
      ref IntPtr ptrBlob);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_blob_read(
      IntPtr blob,
      [MarshalAs(UnmanagedType.LPArray)] byte[] buffer,
      int count,
      int offset);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_blob_reopen(
      IntPtr blob,
      long rowId);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_blob_write(
      IntPtr blob,
      [MarshalAs(UnmanagedType.LPArray)] byte[] buffer,
      int count,
      int offset);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3_declare_vtab(
      IntPtr db,
      IntPtr zSQL);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_mprintf(IntPtr format, __arglist);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr sqlite3_create_disposable_module(
      IntPtr db,
      IntPtr name,
      ref UnsafeNativeMethods.sqlite3_module module,
      IntPtr pClientData,
      UnsafeNativeMethods.xDestroyModule xDestroy);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3_dispose_module(IntPtr pModule);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3session_create(
      IntPtr db,
      byte[] dbName,
      ref IntPtr session);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3session_delete(IntPtr session);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3session_enable(IntPtr session, int enable);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3session_indirect(IntPtr session, int indirect);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3session_attach(
      IntPtr session,
      byte[] tblName);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3session_table_filter(
      IntPtr session,
      UnsafeNativeMethods.xSessionFilter xFilter,
      IntPtr context);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3session_changeset(
      IntPtr session,
      ref int nChangeSet,
      ref IntPtr pChangeSet);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3session_diff(
      IntPtr session,
      byte[] fromDbName,
      byte[] tblName,
      ref IntPtr errMsg);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3session_patchset(
      IntPtr session,
      ref int nPatchSet,
      ref IntPtr pPatchSet);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int sqlite3session_isempty(IntPtr session);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_start(
      ref IntPtr iterator,
      int nChangeSet,
      IntPtr pChangeSet);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_start_v2(
      ref IntPtr iterator,
      int nChangeSet,
      IntPtr pChangeSet,
      SQLiteChangeSetStartFlags flags);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_next(IntPtr iterator);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_op(
      IntPtr iterator,
      ref IntPtr pTblName,
      ref int nColumns,
      ref SQLiteAuthorizerActionCode op,
      ref int bIndirect);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_pk(
      IntPtr iterator,
      ref IntPtr pPrimaryKeys,
      ref int nColumns);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_old(
      IntPtr iterator,
      int columnIndex,
      ref IntPtr pValue);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_new(
      IntPtr iterator,
      int columnIndex,
      ref IntPtr pValue);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_conflict(
      IntPtr iterator,
      int columnIndex,
      ref IntPtr pValue);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_fk_conflicts(
      IntPtr iterator,
      ref int conflicts);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_finalize(IntPtr iterator);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_invert(
      int nIn,
      IntPtr pIn,
      ref int nOut,
      ref IntPtr pOut);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_concat(
      int nA,
      IntPtr pA,
      int nB,
      IntPtr pB,
      ref int nOut,
      ref IntPtr pOut);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changegroup_new(
      ref IntPtr changeGroup);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changegroup_add(
      IntPtr changeGroup,
      int nData,
      IntPtr pData);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changegroup_output(
      IntPtr changeGroup,
      ref int nData,
      ref IntPtr pData);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void sqlite3changegroup_delete(IntPtr changeGroup);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_apply(
      IntPtr db,
      int nChangeSet,
      IntPtr pChangeSet,
      UnsafeNativeMethods.xSessionFilter xFilter,
      UnsafeNativeMethods.xSessionConflict xConflict,
      IntPtr context);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_apply_strm(
      IntPtr db,
      UnsafeNativeMethods.xSessionInput xInput,
      IntPtr pIn,
      UnsafeNativeMethods.xSessionFilter xFilter,
      UnsafeNativeMethods.xSessionConflict xConflict,
      IntPtr context);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_concat_strm(
      UnsafeNativeMethods.xSessionInput xInputA,
      IntPtr pInA,
      UnsafeNativeMethods.xSessionInput xInputB,
      IntPtr pInB,
      UnsafeNativeMethods.xSessionOutput xOutput,
      IntPtr pOut);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_invert_strm(
      UnsafeNativeMethods.xSessionInput xInput,
      IntPtr pIn,
      UnsafeNativeMethods.xSessionOutput xOutput,
      IntPtr pOut);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_start_strm(
      ref IntPtr iterator,
      UnsafeNativeMethods.xSessionInput xInput,
      IntPtr pIn);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changeset_start_v2_strm(
      ref IntPtr iterator,
      UnsafeNativeMethods.xSessionInput xInput,
      IntPtr pIn,
      SQLiteChangeSetStartFlags flags);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3session_changeset_strm(
      IntPtr session,
      UnsafeNativeMethods.xSessionOutput xOutput,
      IntPtr pOut);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3session_patchset_strm(
      IntPtr session,
      UnsafeNativeMethods.xSessionOutput xOutput,
      IntPtr pOut);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changegroup_add_strm(
      IntPtr changeGroup,
      UnsafeNativeMethods.xSessionInput xInput,
      IntPtr pIn);

    [DllImport("SQLite.Interop.dll", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SQLiteErrorCode sqlite3changegroup_output_strm(
      IntPtr changeGroup,
      UnsafeNativeMethods.xSessionOutput xOutput,
      IntPtr pOut);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int xSessionFilter(IntPtr context, IntPtr pTblName);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate SQLiteChangeSetConflictResult xSessionConflict(
      IntPtr context,
      SQLiteChangeSetConflictType type,
      IntPtr iterator);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate SQLiteErrorCode xSessionInput(
      IntPtr context,
      IntPtr pData,
      ref int nData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate SQLiteErrorCode xSessionOutput(
      IntPtr context,
      IntPtr pData,
      int nData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xCreate(
      IntPtr pDb,
      IntPtr pAux,
      int argc,
      IntPtr argv,
      ref IntPtr pVtab,
      ref IntPtr pError);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xConnect(
      IntPtr pDb,
      IntPtr pAux,
      int argc,
      IntPtr argv,
      ref IntPtr pVtab,
      ref IntPtr pError);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xBestIndex(IntPtr pVtab, IntPtr pIndex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xDisconnect(IntPtr pVtab);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xDestroy(IntPtr pVtab);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xOpen(IntPtr pVtab, ref IntPtr pCursor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xClose(IntPtr pCursor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xFilter(
      IntPtr pCursor,
      int idxNum,
      IntPtr idxStr,
      int argc,
      IntPtr argv);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xNext(IntPtr pCursor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xEof(IntPtr pCursor);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xColumn(
      IntPtr pCursor,
      IntPtr pContext,
      int index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xRowId(IntPtr pCursor, ref long rowId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xUpdate(
      IntPtr pVtab,
      int argc,
      IntPtr argv,
      ref long rowId);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xBegin(IntPtr pVtab);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xSync(IntPtr pVtab);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xCommit(IntPtr pVtab);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xRollback(IntPtr pVtab);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int xFindFunction(
      IntPtr pVtab,
      int nArg,
      IntPtr zName,
      ref SQLiteCallback callback,
      ref IntPtr pUserData);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xRename(IntPtr pVtab, IntPtr zNew);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xSavepoint(IntPtr pVtab, int iSavepoint);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xRelease(IntPtr pVtab, int iSavepoint);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate SQLiteErrorCode xRollbackTo(IntPtr pVtab, int iSavepoint);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void xDestroyModule(IntPtr pClientData);

    internal struct sqlite3_module
    {
      public int iVersion;
      public UnsafeNativeMethods.xCreate xCreate;
      public UnsafeNativeMethods.xConnect xConnect;
      public UnsafeNativeMethods.xBestIndex xBestIndex;
      public UnsafeNativeMethods.xDisconnect xDisconnect;
      public UnsafeNativeMethods.xDestroy xDestroy;
      public UnsafeNativeMethods.xOpen xOpen;
      public UnsafeNativeMethods.xClose xClose;
      public UnsafeNativeMethods.xFilter xFilter;
      public UnsafeNativeMethods.xNext xNext;
      public UnsafeNativeMethods.xEof xEof;
      public UnsafeNativeMethods.xColumn xColumn;
      public UnsafeNativeMethods.xRowId xRowId;
      public UnsafeNativeMethods.xUpdate xUpdate;
      public UnsafeNativeMethods.xBegin xBegin;
      public UnsafeNativeMethods.xSync xSync;
      public UnsafeNativeMethods.xCommit xCommit;
      public UnsafeNativeMethods.xRollback xRollback;
      public UnsafeNativeMethods.xFindFunction xFindFunction;
      public UnsafeNativeMethods.xRename xRename;
      public UnsafeNativeMethods.xSavepoint xSavepoint;
      public UnsafeNativeMethods.xRelease xRelease;
      public UnsafeNativeMethods.xRollbackTo xRollbackTo;
    }

    internal struct sqlite3_vtab
    {
      public IntPtr pModule;
      public int nRef;
      public IntPtr zErrMsg;
    }

    internal struct sqlite3_vtab_cursor
    {
      public IntPtr pVTab;
    }

    internal struct sqlite3_index_constraint
    {
      public int iColumn;
      public SQLiteIndexConstraintOp op;
      public byte usable;
      public int iTermOffset;

      public sqlite3_index_constraint(SQLiteIndexConstraint constraint)
        : this()
      {
        if (constraint == null)
          return;
        this.iColumn = constraint.iColumn;
        this.op = constraint.op;
        this.usable = constraint.usable;
        this.iTermOffset = constraint.iTermOffset;
      }
    }

    internal struct sqlite3_index_orderby
    {
      public int iColumn;
      public byte desc;

      public sqlite3_index_orderby(SQLiteIndexOrderBy orderBy)
        : this()
      {
        if (orderBy == null)
          return;
        this.iColumn = orderBy.iColumn;
        this.desc = orderBy.desc;
      }
    }

    internal struct sqlite3_index_constraint_usage
    {
      public int argvIndex;
      public byte omit;

      public sqlite3_index_constraint_usage(SQLiteIndexConstraintUsage constraintUsage)
        : this()
      {
        if (constraintUsage == null)
          return;
        this.argvIndex = constraintUsage.argvIndex;
        this.omit = constraintUsage.omit;
      }
    }

    internal struct sqlite3_index_info
    {
      public int nConstraint;
      public IntPtr aConstraint;
      public int nOrderBy;
      public IntPtr aOrderBy;
      public IntPtr aConstraintUsage;
      public int idxNum;
      public string idxStr;
      public int needToFreeIdxStr;
      public int orderByConsumed;
      public double estimatedCost;
      public long estimatedRows;
      public SQLiteIndexFlags idxFlags;
      public long colUsed;
    }
  }
}

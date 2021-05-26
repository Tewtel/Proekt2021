// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.DbConfigurationTypeAttribute
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Resources;

namespace System.Data.Entity
{
  /// <summary>
  /// This attribute can be placed on a subclass of <see cref="T:System.Data.Entity.DbContext" /> to indicate that the subclass of
  /// <see cref="T:System.Data.Entity.DbConfiguration" /> representing the code-based configuration for the application is in a different
  /// assembly than the context type.
  /// </summary>
  /// <remarks>
  /// Normally a subclass of <see cref="T:System.Data.Entity.DbConfiguration" /> should be placed in the same assembly as
  /// the subclass of <see cref="T:System.Data.Entity.DbContext" /> used by the application. It will then be discovered automatically.
  /// However, if this is not possible or if the application contains multiple context types in different
  /// assemblies, then this attribute can be used to direct DbConfiguration discovery to the appropriate type.
  /// An alternative to using this attribute is to specify the DbConfiguration type to use in the application's
  /// config file. See http://go.microsoft.com/fwlink/?LinkId=260883 for more information.
  /// </remarks>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class DbConfigurationTypeAttribute : Attribute
  {
    private readonly Type _configurationType;

    /// <summary>
    /// Indicates that the given subclass of <see cref="T:System.Data.Entity.DbConfiguration" /> should be used for code-based configuration
    /// for this application.
    /// </summary>
    /// <param name="configurationType">
    /// The <see cref="T:System.Data.Entity.DbConfiguration" /> type to use.
    /// </param>
    public DbConfigurationTypeAttribute(Type configurationType)
    {
      System.Data.Entity.Utilities.Check.NotNull<Type>(configurationType, nameof (configurationType));
      this._configurationType = configurationType;
    }

    /// <summary>
    /// Indicates that the subclass of <see cref="T:System.Data.Entity.DbConfiguration" /> represented by the given assembly-qualified
    /// name should be used for code-based configuration for this application.
    /// </summary>
    /// <param name="configurationTypeName">
    /// The <see cref="T:System.Data.Entity.DbConfiguration" /> type to use.
    /// </param>
    public DbConfigurationTypeAttribute(string configurationTypeName)
    {
      System.Data.Entity.Utilities.Check.NotEmpty(configurationTypeName, nameof (configurationTypeName));
      try
      {
        this._configurationType = Type.GetType(configurationTypeName, true);
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException(Strings.DbConfigurationTypeInAttributeNotFound((object) configurationTypeName), ex);
      }
    }

    /// <summary>
    /// Gets the subclass of <see cref="T:System.Data.Entity.DbConfiguration" /> that should be used for code-based configuration
    /// for this application.
    /// </summary>
    public Type ConfigurationType => this._configurationType;
  }
}

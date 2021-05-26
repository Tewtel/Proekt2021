// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.EntityProxyTypeInfo
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Utilities;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal sealed class EntityProxyTypeInfo
  {
    private readonly Type _proxyType;
    private readonly ClrEntityType _entityType;
    internal const string EntityWrapperFieldName = "_entityWrapper";
    private const string InitializeEntityCollectionsName = "InitializeEntityCollections";
    private readonly DynamicMethod _initializeCollections;
    private readonly Func<object, string, object> _baseGetter;
    private readonly HashSet<string> _propertiesWithBaseGetter;
    private readonly Action<object, string, object> _baseSetter;
    private readonly HashSet<string> _propertiesWithBaseSetter;
    private readonly Func<object, object> Proxy_GetEntityWrapper;
    private readonly Func<object, object, object> Proxy_SetEntityWrapper;
    private readonly Func<object> _createObject;
    private readonly Dictionary<string, AssociationType> _navigationPropertyAssociationTypes = new Dictionary<string, AssociationType>();

    internal EntityProxyTypeInfo(
      Type proxyType,
      ClrEntityType ospaceEntityType,
      DynamicMethod initializeCollections,
      List<PropertyInfo> baseGetters,
      List<PropertyInfo> baseSetters,
      MetadataWorkspace workspace)
    {
      this._proxyType = proxyType;
      this._entityType = ospaceEntityType;
      this._initializeCollections = initializeCollections;
      foreach (AssociationType associationType in EntityProxyTypeInfo.GetAllRelationshipsForType(workspace, proxyType))
      {
        this._navigationPropertyAssociationTypes.Add(associationType.FullName, associationType);
        if (associationType.Name != associationType.FullName)
          this._navigationPropertyAssociationTypes.Add(associationType.Name, associationType);
      }
      FieldInfo field = proxyType.GetField("_entityWrapper", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      ParameterExpression parameterExpression4 = Expression.Parameter(typeof (object), "proxy");
      ParameterExpression parameterExpression5 = Expression.Parameter(typeof (object), "value");
      Func<object, object> getEntityWrapperDelegate = Expression.Lambda<Func<object, object>>((Expression) Expression.Field((Expression) Expression.Convert((Expression) parameterExpression4, field.DeclaringType), field), parameterExpression4).Compile();
      this.Proxy_GetEntityWrapper = (Func<object, object>) (proxy =>
      {
        IEntityWrapper entityWrapper = (IEntityWrapper) getEntityWrapperDelegate(proxy);
        if (entityWrapper != null && entityWrapper.Entity != proxy)
          throw new InvalidOperationException(System.Data.Entity.Resources.Strings.EntityProxyTypeInfo_ProxyHasWrongWrapper);
        return (object) entityWrapper;
      });
      this.Proxy_SetEntityWrapper = ((Expression<Func<object, object, object>>) ((parameterExpression1, parameterExpression2) => Expression.Assign((Expression) Expression.Field((Expression) Expression.Convert((Expression) parameterExpression4, field.DeclaringType), field), parameterExpression2))).Compile();
      ParameterExpression parameterExpression6 = Expression.Parameter(typeof (string), "propertyName");
      MethodInfo publicInstanceMethod1 = proxyType.GetPublicInstanceMethod("GetBasePropertyValue", typeof (string));
      if (publicInstanceMethod1 != (MethodInfo) null)
        this._baseGetter = Expression.Lambda<Func<object, string, object>>((Expression) Expression.Call((Expression) Expression.Convert((Expression) parameterExpression4, proxyType), publicInstanceMethod1, (Expression) parameterExpression6), parameterExpression4, parameterExpression6).Compile();
      ParameterExpression parameterExpression7 = Expression.Parameter(typeof (object), "propertyName");
      MethodInfo publicInstanceMethod2 = proxyType.GetPublicInstanceMethod("SetBasePropertyValue", typeof (string), typeof (object));
      if (publicInstanceMethod2 != (MethodInfo) null)
        this._baseSetter = ((Expression<Action<object, string, object>>) ((parameterExpression1, parameterExpression2, parameterExpression3) => Expression.Call((Expression) Expression.Convert(parameterExpression1, proxyType), publicInstanceMethod2, parameterExpression2, parameterExpression3))).Compile();
      this._propertiesWithBaseGetter = new HashSet<string>(baseGetters.Select<PropertyInfo, string>((Func<PropertyInfo, string>) (p => p.Name)));
      this._propertiesWithBaseSetter = new HashSet<string>(baseSetters.Select<PropertyInfo, string>((Func<PropertyInfo, string>) (p => p.Name)));
      this._createObject = DelegateFactory.CreateConstructor(proxyType);
    }

    internal static IEnumerable<AssociationType> GetAllRelationshipsForType(
      MetadataWorkspace workspace,
      Type clrType)
    {
      return workspace.GetItemCollection(DataSpace.OSpace).GetItems<AssociationType>().Where<AssociationType>((Func<AssociationType, bool>) (a => EntityProxyTypeInfo.IsEndMemberForType(a.AssociationEndMembers[0], clrType) || EntityProxyTypeInfo.IsEndMemberForType(a.AssociationEndMembers[1], clrType)));
    }

    private static bool IsEndMemberForType(AssociationEndMember end, Type clrType) => end.TypeUsage.EdmType is RefType edmType && edmType.ElementType.ClrType.IsAssignableFrom(clrType);

    internal object CreateProxyObject() => this._createObject();

    internal Type ProxyType => this._proxyType;

    internal DynamicMethod InitializeEntityCollections => this._initializeCollections;

    public Func<object, string, object> BaseGetter => this._baseGetter;

    public bool ContainsBaseGetter(string propertyName) => this.BaseGetter != null && this._propertiesWithBaseGetter.Contains(propertyName);

    public bool ContainsBaseSetter(string propertyName) => this.BaseSetter != null && this._propertiesWithBaseSetter.Contains(propertyName);

    public Action<object, string, object> BaseSetter => this._baseSetter;

    public bool TryGetNavigationPropertyAssociationType(
      string relationshipName,
      out AssociationType associationType)
    {
      return this._navigationPropertyAssociationTypes.TryGetValue(relationshipName, out associationType);
    }

    public IEnumerable<AssociationType> GetAllAssociationTypes() => this._navigationPropertyAssociationTypes.Values.Distinct<AssociationType>();

    public void ValidateType(ClrEntityType ospaceEntityType)
    {
      if (ospaceEntityType != this._entityType && ospaceEntityType.HashedDescription != this._entityType.HashedDescription)
        throw new InvalidOperationException(System.Data.Entity.Resources.Strings.EntityProxyTypeInfo_DuplicateOSpaceType((object) ospaceEntityType.ClrType.FullName));
    }

    internal IEntityWrapper SetEntityWrapper(IEntityWrapper wrapper) => this.Proxy_SetEntityWrapper(wrapper.Entity, (object) wrapper) as IEntityWrapper;

    internal IEntityWrapper GetEntityWrapper(object entity) => this.Proxy_GetEntityWrapper(entity) as IEntityWrapper;

    internal Func<object, object> EntityWrapperDelegate => this.Proxy_GetEntityWrapper;
  }
}

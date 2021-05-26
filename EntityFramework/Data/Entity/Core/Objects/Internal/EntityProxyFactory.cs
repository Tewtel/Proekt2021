// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.EntityProxyFactory
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Utilities;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml.Serialization;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal class EntityProxyFactory
  {
    internal const string ResetFKSetterFlagFieldName = "_resetFKSetterFlag";
    internal const string CompareByteArraysFieldName = "_compareByteArrays";
    private static readonly Dictionary<Tuple<Type, string>, EntityProxyTypeInfo> _proxyNameMap = new Dictionary<Tuple<Type, string>, EntityProxyTypeInfo>();
    private static readonly Dictionary<Type, EntityProxyTypeInfo> _proxyTypeMap = new Dictionary<Type, EntityProxyTypeInfo>();
    private static readonly Dictionary<Assembly, ModuleBuilder> _moduleBuilders = new Dictionary<Assembly, ModuleBuilder>();
    private static readonly ReaderWriterLockSlim _typeMapLock = new ReaderWriterLockSlim();
    private static readonly HashSet<Assembly> _proxyRuntimeAssemblies = new HashSet<Assembly>();
    internal static readonly MethodInfo GetInterceptorDelegateMethod = typeof (LazyLoadBehavior).GetOnlyDeclaredMethod("GetInterceptorDelegate");

    private static ModuleBuilder GetDynamicModule(EntityType ospaceEntityType)
    {
      Assembly key = ospaceEntityType.ClrType.Assembly();
      ModuleBuilder moduleBuilder;
      if (!EntityProxyFactory._moduleBuilders.TryGetValue(key, out moduleBuilder))
      {
        moduleBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "EntityFrameworkDynamicProxies-{0}", (object) key.FullName))
        {
          Version = new Version(1, 0, 0, 0)
        }, AssemblyBuilderAccess.Run).DefineDynamicModule("EntityProxyModule");
        EntityProxyFactory._moduleBuilders.Add(key, moduleBuilder);
      }
      return moduleBuilder;
    }

    private static void DiscardDynamicModule(EntityType ospaceEntityType) => EntityProxyFactory._moduleBuilders.Remove(ospaceEntityType.ClrType.Assembly());

    internal static bool TryGetProxyType(
      Type clrType,
      string entityTypeName,
      out EntityProxyTypeInfo proxyTypeInfo)
    {
      EntityProxyFactory._typeMapLock.EnterReadLock();
      try
      {
        return EntityProxyFactory._proxyNameMap.TryGetValue(new Tuple<Type, string>(clrType, entityTypeName), out proxyTypeInfo);
      }
      finally
      {
        EntityProxyFactory._typeMapLock.ExitReadLock();
      }
    }

    internal static bool TryGetProxyType(Type proxyType, out EntityProxyTypeInfo proxyTypeInfo)
    {
      EntityProxyFactory._typeMapLock.EnterReadLock();
      try
      {
        return EntityProxyFactory._proxyTypeMap.TryGetValue(proxyType, out proxyTypeInfo);
      }
      finally
      {
        EntityProxyFactory._typeMapLock.ExitReadLock();
      }
    }

    internal static bool TryGetProxyWrapper(object instance, out IEntityWrapper wrapper)
    {
      wrapper = (IEntityWrapper) null;
      EntityProxyTypeInfo proxyTypeInfo;
      if (EntityProxyFactory.IsProxyType(instance.GetType()) && EntityProxyFactory.TryGetProxyType(instance.GetType(), out proxyTypeInfo))
        wrapper = proxyTypeInfo.GetEntityWrapper(instance);
      return wrapper != null;
    }

    internal static EntityProxyTypeInfo GetProxyType(
      ClrEntityType ospaceEntityType,
      MetadataWorkspace workspace)
    {
      EntityProxyTypeInfo proxyTypeInfo = (EntityProxyTypeInfo) null;
      if (EntityProxyFactory.TryGetProxyType(ospaceEntityType.ClrType, ospaceEntityType.CSpaceTypeName, out proxyTypeInfo))
      {
        proxyTypeInfo?.ValidateType(ospaceEntityType);
        return proxyTypeInfo;
      }
      EntityProxyFactory._typeMapLock.EnterUpgradeableReadLock();
      try
      {
        return EntityProxyFactory.TryCreateProxyType((EntityType) ospaceEntityType, workspace);
      }
      finally
      {
        EntityProxyFactory._typeMapLock.ExitUpgradeableReadLock();
      }
    }

    internal static bool TryGetAssociationTypeFromProxyInfo(
      IEntityWrapper wrappedEntity,
      string relationshipName,
      out AssociationType associationType)
    {
      associationType = (AssociationType) null;
      EntityProxyTypeInfo proxyTypeInfo;
      return EntityProxyFactory.TryGetProxyType(wrappedEntity.Entity.GetType(), out proxyTypeInfo) && proxyTypeInfo != null && proxyTypeInfo.TryGetNavigationPropertyAssociationType(relationshipName, out associationType);
    }

    internal static IEnumerable<AssociationType> TryGetAllAssociationTypesFromProxyInfo(
      IEntityWrapper wrappedEntity)
    {
      EntityProxyTypeInfo proxyTypeInfo;
      return !EntityProxyFactory.TryGetProxyType(wrappedEntity.Entity.GetType(), out proxyTypeInfo) ? (IEnumerable<AssociationType>) null : proxyTypeInfo.GetAllAssociationTypes();
    }

    internal static void TryCreateProxyTypes(
      IEnumerable<EntityType> ospaceEntityTypes,
      MetadataWorkspace workspace)
    {
      EntityProxyFactory._typeMapLock.EnterUpgradeableReadLock();
      try
      {
        foreach (EntityType ospaceEntityType in ospaceEntityTypes)
          EntityProxyFactory.TryCreateProxyType(ospaceEntityType, workspace);
      }
      finally
      {
        EntityProxyFactory._typeMapLock.ExitUpgradeableReadLock();
      }
    }

    private static EntityProxyTypeInfo TryCreateProxyType(
      EntityType ospaceEntityType,
      MetadataWorkspace workspace)
    {
      ClrEntityType ospaceEntityType1 = (ClrEntityType) ospaceEntityType;
      Tuple<Type, string> key = new Tuple<Type, string>(ospaceEntityType1.ClrType, ospaceEntityType1.HashedDescription);
      EntityProxyTypeInfo entityProxyTypeInfo;
      if (!EntityProxyFactory._proxyNameMap.TryGetValue(key, out entityProxyTypeInfo))
      {
        if (EntityProxyFactory.CanProxyType(ospaceEntityType))
        {
          try
          {
            entityProxyTypeInfo = EntityProxyFactory.BuildType(EntityProxyFactory.GetDynamicModule(ospaceEntityType), ospaceEntityType1, workspace);
            EntityProxyFactory._typeMapLock.EnterWriteLock();
            try
            {
              EntityProxyFactory._proxyNameMap[key] = entityProxyTypeInfo;
              if (entityProxyTypeInfo != null)
                EntityProxyFactory._proxyTypeMap[entityProxyTypeInfo.ProxyType] = entityProxyTypeInfo;
            }
            finally
            {
              EntityProxyFactory._typeMapLock.ExitWriteLock();
            }
          }
          catch
          {
            EntityProxyFactory.DiscardDynamicModule(ospaceEntityType);
            throw;
          }
        }
      }
      return entityProxyTypeInfo;
    }

    internal static bool IsProxyType(Type type) => type != (Type) null && EntityProxyFactory._proxyRuntimeAssemblies.Contains(type.Assembly());

    internal static IEnumerable<Type> GetKnownProxyTypes()
    {
      EntityProxyFactory._typeMapLock.EnterReadLock();
      try
      {
        return (IEnumerable<Type>) EntityProxyFactory._proxyNameMap.Values.Where<EntityProxyTypeInfo>((Func<EntityProxyTypeInfo, bool>) (info => info != null)).Select<EntityProxyTypeInfo, Type>((Func<EntityProxyTypeInfo, Type>) (info => info.ProxyType)).ToArray<Type>();
      }
      finally
      {
        EntityProxyFactory._typeMapLock.ExitReadLock();
      }
    }

    public virtual Func<object, object> CreateBaseGetter(
      Type declaringType,
      PropertyInfo propertyInfo)
    {
      ParameterExpression parameterExpression;
      Func<object, object> nonProxyGetter = Expression.Lambda<Func<object, object>>((Expression) Expression.Property((Expression) Expression.Convert((Expression) parameterExpression, declaringType), propertyInfo), parameterExpression).Compile();
      string propertyName = propertyInfo.Name;
      return (Func<object, object>) (entity =>
      {
        Type type = entity.GetType();
        object obj;
        return EntityProxyFactory.IsProxyType(type) && EntityProxyFactory.TryGetBasePropertyValue(type, propertyName, entity, out obj) ? obj : nonProxyGetter(entity);
      });
    }

    private static bool TryGetBasePropertyValue(
      Type proxyType,
      string propertyName,
      object entity,
      out object value)
    {
      value = (object) null;
      EntityProxyTypeInfo proxyTypeInfo;
      if (!EntityProxyFactory.TryGetProxyType(proxyType, out proxyTypeInfo) || !proxyTypeInfo.ContainsBaseGetter(propertyName))
        return false;
      value = proxyTypeInfo.BaseGetter(entity, propertyName);
      return true;
    }

    public virtual Action<object, object> CreateBaseSetter(
      Type declaringType,
      PropertyInfo propertyInfo)
    {
      Action<object, object> nonProxySetter = DelegateFactory.CreateNavigationPropertySetter(declaringType, propertyInfo);
      string propertyName = propertyInfo.Name;
      return (Action<object, object>) ((entity, value) =>
      {
        Type type = entity.GetType();
        if (EntityProxyFactory.IsProxyType(type) && EntityProxyFactory.TrySetBasePropertyValue(type, propertyName, entity, value))
          return;
        nonProxySetter(entity, value);
      });
    }

    private static bool TrySetBasePropertyValue(
      Type proxyType,
      string propertyName,
      object entity,
      object value)
    {
      EntityProxyTypeInfo proxyTypeInfo;
      if (!EntityProxyFactory.TryGetProxyType(proxyType, out proxyTypeInfo) || !proxyTypeInfo.ContainsBaseSetter(propertyName))
        return false;
      proxyTypeInfo.BaseSetter(entity, propertyName, value);
      return true;
    }

    private static EntityProxyTypeInfo BuildType(
      ModuleBuilder moduleBuilder,
      ClrEntityType ospaceEntityType,
      MetadataWorkspace workspace)
    {
      EntityProxyFactory.ProxyTypeBuilder proxyTypeBuilder = new EntityProxyFactory.ProxyTypeBuilder(ospaceEntityType);
      Type type = proxyTypeBuilder.CreateType(moduleBuilder);
      EntityProxyTypeInfo proxyTypeInfo;
      if (type != (Type) null)
      {
        Assembly assembly = type.Assembly();
        if (!EntityProxyFactory._proxyRuntimeAssemblies.Contains(assembly))
        {
          EntityProxyFactory._proxyRuntimeAssemblies.Add(assembly);
          EntityProxyFactory.AddAssemblyToResolveList(assembly);
        }
        proxyTypeInfo = new EntityProxyTypeInfo(type, ospaceEntityType, proxyTypeBuilder.CreateInitializeCollectionMethod(type), proxyTypeBuilder.BaseGetters, proxyTypeBuilder.BaseSetters, workspace);
        foreach (EdmMember lazyLoadMember in proxyTypeBuilder.LazyLoadMembers)
          EntityProxyFactory.InterceptMember(lazyLoadMember, type, proxyTypeInfo);
        EntityProxyFactory.SetResetFKSetterFlagDelegate(type, proxyTypeInfo);
        EntityProxyFactory.SetCompareByteArraysDelegate(type);
      }
      else
        proxyTypeInfo = (EntityProxyTypeInfo) null;
      return proxyTypeInfo;
    }

    private static void AddAssemblyToResolveList(Assembly assembly)
    {
      try
      {
        AppDomain.CurrentDomain.AssemblyResolve += (ResolveEventHandler) ((_, args) => !(args.Name == assembly.FullName) ? (Assembly) null : assembly);
      }
      catch (MethodAccessException ex)
      {
      }
    }

    private static void InterceptMember(
      EdmMember member,
      Type proxyType,
      EntityProxyTypeInfo proxyTypeInfo)
    {
      PropertyInfo topProperty = proxyType.GetTopProperty(member.Name);
      FieldInfo field = proxyType.GetField(LazyLoadImplementor.GetInterceptorFieldName(member.Name), BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
      EntityProxyFactory.AssignInterceptionDelegate(EntityProxyFactory.GetInterceptorDelegateMethod.MakeGenericMethod(proxyType, topProperty.PropertyType).Invoke((object) null, new object[2]
      {
        (object) member,
        (object) proxyTypeInfo.EntityWrapperDelegate
      }) as Delegate, field);
    }

    private static void AssignInterceptionDelegate(
      Delegate interceptorDelegate,
      FieldInfo interceptorField)
    {
      interceptorField.SetValue((object) null, (object) interceptorDelegate);
    }

    private static void SetResetFKSetterFlagDelegate(
      Type proxyType,
      EntityProxyTypeInfo proxyTypeInfo)
    {
      FieldInfo field = proxyType.GetField("_resetFKSetterFlag", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
      EntityProxyFactory.AssignInterceptionDelegate((Delegate) EntityProxyFactory.GetResetFKSetterFlagDelegate(proxyTypeInfo.EntityWrapperDelegate), field);
    }

    private static Action<object> GetResetFKSetterFlagDelegate(
      Func<object, object> getEntityWrapperDelegate)
    {
      return (Action<object>) (proxy => EntityProxyFactory.ResetFKSetterFlag(getEntityWrapperDelegate(proxy)));
    }

    private static void ResetFKSetterFlag(object wrappedEntityAsObject)
    {
      IEntityWrapper entityWrapper = (IEntityWrapper) wrappedEntityAsObject;
      if (entityWrapper == null || entityWrapper.Context == null)
        return;
      entityWrapper.Context.ObjectStateManager.EntityInvokingFKSetter = (object) null;
    }

    private static void SetCompareByteArraysDelegate(Type proxyType)
    {
      FieldInfo field = proxyType.GetField("_compareByteArrays", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.NonPublic);
      EntityProxyFactory.AssignInterceptionDelegate((Delegate) new Func<object, object, bool>(ByValueEqualityComparer.Default.Equals), field);
    }

    private static bool CanProxyType(EntityType ospaceEntityType)
    {
      Type clrType = ospaceEntityType.ClrType;
      if (!clrType.IsPublic() || clrType.IsSealed() || (typeof (IEntityWithRelationships).IsAssignableFrom(clrType) || ospaceEntityType.Abstract))
        return false;
      ConstructorInfo declaredConstructor = clrType.GetDeclaredConstructor();
      if (!(declaredConstructor != (ConstructorInfo) null))
        return false;
      return (declaredConstructor.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public || (declaredConstructor.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Family || (declaredConstructor.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.FamORAssem;
    }

    private static bool CanProxyMethod(MethodInfo method)
    {
      bool flag = false;
      if (method != (MethodInfo) null)
      {
        MethodAttributes methodAttributes = method.Attributes & MethodAttributes.MemberAccessMask;
        flag = method.IsVirtual && !method.IsFinal && (methodAttributes == MethodAttributes.Public || methodAttributes == MethodAttributes.Family || methodAttributes == MethodAttributes.FamORAssem);
      }
      return flag;
    }

    internal static bool CanProxyGetter(PropertyInfo clrProperty) => EntityProxyFactory.CanProxyMethod(clrProperty.Getter());

    internal static bool CanProxySetter(PropertyInfo clrProperty) => EntityProxyFactory.CanProxyMethod(clrProperty.Setter());

    internal class ProxyTypeBuilder
    {
      private TypeBuilder _typeBuilder;
      private readonly BaseProxyImplementor _baseImplementor;
      private readonly IPocoImplementor _ipocoImplementor;
      private readonly LazyLoadImplementor _lazyLoadImplementor;
      private readonly DataContractImplementor _dataContractImplementor;
      private readonly SerializableImplementor _iserializableImplementor;
      private readonly ClrEntityType _ospaceEntityType;
      private ModuleBuilder _moduleBuilder;
      private readonly List<FieldBuilder> _serializedFields = new List<FieldBuilder>(3);
      private static readonly ConstructorInfo _nonSerializedAttributeConstructor = typeof (NonSerializedAttribute).GetDeclaredConstructor();
      private static readonly ConstructorInfo _ignoreDataMemberAttributeConstructor = typeof (IgnoreDataMemberAttribute).GetDeclaredConstructor();
      private static readonly ConstructorInfo _xmlIgnoreAttributeConstructor = typeof (XmlIgnoreAttribute).GetDeclaredConstructor();
      private static readonly Lazy<ConstructorInfo> _scriptIgnoreAttributeConstructor = new Lazy<ConstructorInfo>(new Func<ConstructorInfo>(EntityProxyFactory.ProxyTypeBuilder.TryGetScriptIgnoreAttributeConstructor));

      public ProxyTypeBuilder(ClrEntityType ospaceEntityType)
      {
        this._ospaceEntityType = ospaceEntityType;
        this._baseImplementor = new BaseProxyImplementor();
        this._ipocoImplementor = new IPocoImplementor((EntityType) ospaceEntityType);
        this._lazyLoadImplementor = new LazyLoadImplementor((EntityType) ospaceEntityType);
        this._dataContractImplementor = new DataContractImplementor((EntityType) ospaceEntityType);
        this._iserializableImplementor = new SerializableImplementor((EntityType) ospaceEntityType);
      }

      public Type BaseType => this._ospaceEntityType.ClrType;

      public DynamicMethod CreateInitializeCollectionMethod(Type proxyType) => this._ipocoImplementor.CreateInitializeCollectionMethod(proxyType);

      public List<PropertyInfo> BaseGetters => this._baseImplementor.BaseGetters;

      public List<PropertyInfo> BaseSetters => this._baseImplementor.BaseSetters;

      public IEnumerable<EdmMember> LazyLoadMembers => this._lazyLoadImplementor.Members;

      public Type CreateType(ModuleBuilder moduleBuilder)
      {
        this._moduleBuilder = moduleBuilder;
        bool flag = false;
        if (this._iserializableImplementor.TypeIsSuitable)
        {
          foreach (EdmMember member in this._ospaceEntityType.Members)
          {
            if (this._ipocoImplementor.CanProxyMember(member) || this._lazyLoadImplementor.CanProxyMember(member))
            {
              PropertyInfo topProperty = this.BaseType.GetTopProperty(member.Name);
              PropertyBuilder propertyBuilder = this.TypeBuilder.DefineProperty(member.Name, System.Reflection.PropertyAttributes.None, topProperty.PropertyType, Type.EmptyTypes);
              if (!this._ipocoImplementor.EmitMember(this.TypeBuilder, member, propertyBuilder, topProperty, this._baseImplementor))
                EntityProxyFactory.ProxyTypeBuilder.EmitBaseSetter(this.TypeBuilder, propertyBuilder, topProperty);
              if (!this._lazyLoadImplementor.EmitMember(this.TypeBuilder, member, propertyBuilder, topProperty, this._baseImplementor))
                EntityProxyFactory.ProxyTypeBuilder.EmitBaseGetter(this.TypeBuilder, propertyBuilder, topProperty);
              flag = true;
            }
          }
          if ((Type) this._typeBuilder != (Type) null)
          {
            this._baseImplementor.Implement(this.TypeBuilder);
            this._iserializableImplementor.Implement(this.TypeBuilder, (IEnumerable<FieldBuilder>) this._serializedFields);
          }
        }
        return !flag ? (Type) null : this.TypeBuilder.CreateType();
      }

      private TypeBuilder TypeBuilder
      {
        get
        {
          if ((Type) this._typeBuilder == (Type) null)
          {
            TypeAttributes attr = TypeAttributes.Public | TypeAttributes.Sealed;
            if ((this.BaseType.Attributes() & TypeAttributes.Serializable) == TypeAttributes.Serializable)
              attr |= TypeAttributes.Serializable;
            this._typeBuilder = this._moduleBuilder.DefineType(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "System.Data.Entity.DynamicProxies.{0}_{1}", (object) (this.BaseType.Name.Length <= 20 ? this.BaseType.Name : this.BaseType.Name.Substring(0, 20)), (object) this._ospaceEntityType.HashedDescription), attr, this.BaseType, this._ipocoImplementor.Interfaces);
            this._typeBuilder.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
            Action<FieldBuilder, bool> registerField = new Action<FieldBuilder, bool>(this.RegisterInstanceField);
            this._ipocoImplementor.Implement(this._typeBuilder, registerField);
            this._lazyLoadImplementor.Implement(this._typeBuilder, registerField);
            if (!this._iserializableImplementor.TypeImplementsISerializable)
              this._dataContractImplementor.Implement(this._typeBuilder);
          }
          return this._typeBuilder;
        }
      }

      private static void EmitBaseGetter(
        TypeBuilder typeBuilder,
        PropertyBuilder propertyBuilder,
        PropertyInfo baseProperty)
      {
        if (!EntityProxyFactory.CanProxyGetter(baseProperty))
          return;
        MethodInfo meth = baseProperty.Getter();
        MethodAttributes methodAttributes = meth.Attributes & MethodAttributes.MemberAccessMask;
        MethodBuilder mdBuilder = typeBuilder.DefineMethod("get_" + baseProperty.Name, methodAttributes | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, baseProperty.PropertyType, Type.EmptyTypes);
        ILGenerator ilGenerator = mdBuilder.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Call, meth);
        ilGenerator.Emit(OpCodes.Ret);
        propertyBuilder.SetGetMethod(mdBuilder);
      }

      private static void EmitBaseSetter(
        TypeBuilder typeBuilder,
        PropertyBuilder propertyBuilder,
        PropertyInfo baseProperty)
      {
        if (!EntityProxyFactory.CanProxySetter(baseProperty))
          return;
        MethodInfo meth = baseProperty.Setter();
        MethodAttributes methodAttributes = meth.Attributes & MethodAttributes.MemberAccessMask;
        MethodBuilder mdBuilder = typeBuilder.DefineMethod("set_" + baseProperty.Name, methodAttributes | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, (Type) null, new Type[1]
        {
          baseProperty.PropertyType
        });
        ILGenerator ilGenerator = mdBuilder.GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldarg_0);
        ilGenerator.Emit(OpCodes.Ldarg_1);
        ilGenerator.Emit(OpCodes.Call, meth);
        ilGenerator.Emit(OpCodes.Ret);
        propertyBuilder.SetSetMethod(mdBuilder);
      }

      private void RegisterInstanceField(FieldBuilder field, bool serializable)
      {
        if (serializable)
          this._serializedFields.Add(field);
        else
          EntityProxyFactory.ProxyTypeBuilder.MarkAsNotSerializable(field);
      }

      private static ConstructorInfo TryGetScriptIgnoreAttributeConstructor()
      {
        try
        {
          if (AspProxy.IsSystemWebLoaded())
          {
            Type type = Assembly.Load("System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35").GetType("System.Web.Script.Serialization.ScriptIgnoreAttribute");
            if (type != (Type) null)
              return type.GetDeclaredConstructor();
          }
        }
        catch
        {
        }
        return (ConstructorInfo) null;
      }

      public static void MarkAsNotSerializable(FieldBuilder field)
      {
        object[] constructorArgs = new object[0];
        field.SetCustomAttribute(new CustomAttributeBuilder(EntityProxyFactory.ProxyTypeBuilder._nonSerializedAttributeConstructor, constructorArgs));
        if (!field.IsPublic)
          return;
        field.SetCustomAttribute(new CustomAttributeBuilder(EntityProxyFactory.ProxyTypeBuilder._ignoreDataMemberAttributeConstructor, constructorArgs));
        field.SetCustomAttribute(new CustomAttributeBuilder(EntityProxyFactory.ProxyTypeBuilder._xmlIgnoreAttributeConstructor, constructorArgs));
        if (!(EntityProxyFactory.ProxyTypeBuilder._scriptIgnoreAttributeConstructor.Value != (ConstructorInfo) null))
          return;
        field.SetCustomAttribute(new CustomAttributeBuilder(EntityProxyFactory.ProxyTypeBuilder._scriptIgnoreAttributeConstructor.Value, constructorArgs));
      }
    }
  }
}

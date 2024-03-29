﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ELinq.InitializerMetadata
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.Internal.Materialization;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core.Objects.Internal;
using System.Data.Entity.Utilities;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace System.Data.Entity.Core.Objects.ELinq
{
  internal abstract class InitializerMetadata : IEquatable<InitializerMetadata>
  {
    internal readonly Type ClrType;
    private static long s_identifier;
    internal readonly string Identity;
    private static readonly string _identifierPrefix = typeof (InitializerMetadata).Name;

    private InitializerMetadata(Type clrType)
    {
      this.ClrType = clrType;
      this.Identity = InitializerMetadata._identifierPrefix + Interlocked.Increment(ref InitializerMetadata.s_identifier).ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    internal abstract InitializerMetadataKind Kind { get; }

    internal static bool TryGetInitializerMetadata(
      TypeUsage typeUsage,
      out InitializerMetadata initializerMetadata)
    {
      initializerMetadata = (InitializerMetadata) null;
      if (BuiltInTypeKind.RowType == typeUsage.EdmType.BuiltInTypeKind)
        initializerMetadata = ((RowType) typeUsage.EdmType).InitializerMetadata;
      return initializerMetadata != null;
    }

    internal static InitializerMetadata CreateGroupingInitializer(
      EdmItemCollection itemCollection,
      Type resultType)
    {
      return itemCollection.GetCanonicalInitializerMetadata((InitializerMetadata) new InitializerMetadata.GroupingInitializerMetadata(resultType));
    }

    internal static InitializerMetadata CreateProjectionInitializer(
      EdmItemCollection itemCollection,
      MemberInitExpression initExpression)
    {
      return itemCollection.GetCanonicalInitializerMetadata((InitializerMetadata) new InitializerMetadata.ProjectionInitializerMetadata(initExpression));
    }

    internal static InitializerMetadata CreateProjectionInitializer(
      EdmItemCollection itemCollection,
      NewExpression newExpression)
    {
      return itemCollection.GetCanonicalInitializerMetadata((InitializerMetadata) new InitializerMetadata.ProjectionNewMetadata(newExpression));
    }

    internal static InitializerMetadata CreateEmptyProjectionInitializer(
      EdmItemCollection itemCollection,
      NewExpression newExpression)
    {
      return itemCollection.GetCanonicalInitializerMetadata((InitializerMetadata) new InitializerMetadata.EmptyProjectionNewMetadata(newExpression));
    }

    internal static InitializerMetadata CreateEntityCollectionInitializer(
      EdmItemCollection itemCollection,
      Type type,
      NavigationProperty navigationProperty)
    {
      return itemCollection.GetCanonicalInitializerMetadata((InitializerMetadata) new InitializerMetadata.EntityCollectionInitializerMetadata(type, navigationProperty));
    }

    internal virtual void AppendColumnMapKey(ColumnMapKeyBuilder builder) => builder.Append("CLR-", this.ClrType);

    public override bool Equals(object obj) => this.Equals(obj as InitializerMetadata);

    public bool Equals(InitializerMetadata other)
    {
      if (this == other)
        return true;
      return this.Kind == other.Kind && this.ClrType.Equals(other.ClrType) && this.IsStructurallyEquivalent(other);
    }

    public override int GetHashCode() => this.ClrType.GetHashCode();

    protected virtual bool IsStructurallyEquivalent(InitializerMetadata other) => true;

    internal abstract Expression Emit(List<TranslatorResult> propertyTranslatorResults);

    internal abstract IEnumerable<Type> GetChildTypes();

    protected static List<Expression> GetPropertyReaders(
      List<TranslatorResult> propertyTranslatorResults)
    {
      return propertyTranslatorResults.Select<TranslatorResult, Expression>((Func<TranslatorResult, Expression>) (s => s.UnwrappedExpression)).ToList<Expression>();
    }

    private class Grouping<K, T> : IGrouping<K, T>, IEnumerable<T>, IEnumerable
    {
      private readonly K _key;
      private readonly IEnumerable<T> _group;

      public Grouping(K key, IEnumerable<T> group)
      {
        this._key = key;
        this._group = group;
      }

      public K Key => this._key;

      public IEnumerable<T> Group => this._group;

      IEnumerator<T> IEnumerable<T>.GetEnumerator()
      {
        if (this._group != null)
        {
          foreach (T obj in this._group)
            yield return obj;
        }
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) ((IEnumerable<T>) this).GetEnumerator();
    }

    private class GroupingInitializerMetadata : InitializerMetadata
    {
      internal GroupingInitializerMetadata(Type type)
        : base(type)
      {
      }

      internal override InitializerMetadataKind Kind => InitializerMetadataKind.Grouping;

      internal override Expression Emit(List<TranslatorResult> propertyTranslatorResults) => (Expression) Expression.Convert((Expression) Expression.New(((IEnumerable<ConstructorInfo>) typeof (InitializerMetadata.Grouping<,>).MakeGenericType(this.ClrType.GetGenericArguments()[0], this.ClrType.GetGenericArguments()[1]).GetConstructors()).Single<ConstructorInfo>(), (IEnumerable<Expression>) InitializerMetadata.GetPropertyReaders(propertyTranslatorResults)), this.ClrType);

      internal override IEnumerable<Type> GetChildTypes()
      {
        InitializerMetadata.GroupingInitializerMetadata initializerMetadata = this;
        Type genericArgument = initializerMetadata.ClrType.GetGenericArguments()[0];
        Type groupElementType = initializerMetadata.ClrType.GetGenericArguments()[1];
        yield return genericArgument;
        yield return typeof (IEnumerable<>).MakeGenericType(groupElementType);
      }
    }

    private class ProjectionNewMetadata : InitializerMetadata
    {
      private readonly NewExpression _newExpression;

      internal ProjectionNewMetadata(NewExpression newExpression)
        : base(newExpression.Type)
      {
        this._newExpression = newExpression;
      }

      internal override InitializerMetadataKind Kind => InitializerMetadataKind.ProjectionNew;

      protected override bool IsStructurallyEquivalent(InitializerMetadata other)
      {
        InitializerMetadata.ProjectionNewMetadata projectionNewMetadata = (InitializerMetadata.ProjectionNewMetadata) other;
        if (this._newExpression.Members == null && projectionNewMetadata._newExpression.Members == null)
          return true;
        if (this._newExpression.Members == null || projectionNewMetadata._newExpression.Members == null || this._newExpression.Members.Count != projectionNewMetadata._newExpression.Members.Count)
          return false;
        for (int index = 0; index < this._newExpression.Members.Count; ++index)
        {
          if (!this._newExpression.Members[index].Equals((object) projectionNewMetadata._newExpression.Members[index]))
            return false;
        }
        return true;
      }

      internal override Expression Emit(List<TranslatorResult> propertyTranslatorResults) => (Expression) Expression.New(this._newExpression.Constructor, (IEnumerable<Expression>) InitializerMetadata.GetPropertyReaders(propertyTranslatorResults));

      internal override IEnumerable<Type> GetChildTypes() => this._newExpression.Arguments.Select<Expression, Type>((Func<Expression, Type>) (arg => arg.Type));

      internal override void AppendColumnMapKey(ColumnMapKeyBuilder builder)
      {
        base.AppendColumnMapKey(builder);
        builder.Append(this._newExpression.Constructor.ToString());
        foreach (MemberInfo memberInfo in (IEnumerable<MemberInfo>) this._newExpression.Members ?? Enumerable.Empty<MemberInfo>())
        {
          builder.Append("DT", memberInfo.DeclaringType);
          builder.Append("." + memberInfo.Name);
        }
      }
    }

    private class EmptyProjectionNewMetadata : InitializerMetadata.ProjectionNewMetadata
    {
      internal EmptyProjectionNewMetadata(NewExpression newExpression)
        : base(newExpression)
      {
      }

      internal override Expression Emit(List<TranslatorResult> propertyReaders) => base.Emit(new List<TranslatorResult>());

      internal override IEnumerable<Type> GetChildTypes()
      {
        yield return (Type) null;
      }
    }

    private class ProjectionInitializerMetadata : InitializerMetadata
    {
      private readonly MemberInitExpression _initExpression;

      internal ProjectionInitializerMetadata(MemberInitExpression initExpression)
        : base(initExpression.Type)
      {
        this._initExpression = initExpression;
      }

      internal override InitializerMetadataKind Kind => InitializerMetadataKind.ProjectionInitializer;

      protected override bool IsStructurallyEquivalent(InitializerMetadata other)
      {
        InitializerMetadata.ProjectionInitializerMetadata initializerMetadata = (InitializerMetadata.ProjectionInitializerMetadata) other;
        if (this._initExpression.Bindings.Count != initializerMetadata._initExpression.Bindings.Count)
          return false;
        for (int index = 0; index < this._initExpression.Bindings.Count; ++index)
        {
          if (!this._initExpression.Bindings[index].Member.Equals((object) initializerMetadata._initExpression.Bindings[index].Member))
            return false;
        }
        return true;
      }

      internal override Expression Emit(List<TranslatorResult> propertyReaders)
      {
        MemberBinding[] memberBindingArray1 = new MemberBinding[this._initExpression.Bindings.Count];
        MemberBinding[] memberBindingArray2 = new MemberBinding[memberBindingArray1.Length];
        for (int index = 0; index < memberBindingArray1.Length; ++index)
        {
          MemberBinding binding = this._initExpression.Bindings[index];
          Expression unwrappedExpression = propertyReaders[index].UnwrappedExpression;
          MemberBinding memberBinding1 = (MemberBinding) Expression.Bind(binding.Member, unwrappedExpression);
          MemberBinding memberBinding2 = (MemberBinding) Expression.Bind(binding.Member, (Expression) Expression.Constant(TypeSystem.GetDefaultValue(unwrappedExpression.Type), unwrappedExpression.Type));
          memberBindingArray1[index] = memberBinding1;
          memberBindingArray2[index] = memberBinding2;
        }
        return (Expression) Expression.MemberInit(this._initExpression.NewExpression, memberBindingArray1);
      }

      internal override IEnumerable<Type> GetChildTypes()
      {
        foreach (MemberBinding binding in this._initExpression.Bindings)
        {
          Type type;
          TypeSystem.PropertyOrField(binding.Member, out string _, out type);
          yield return type;
        }
      }

      internal override void AppendColumnMapKey(ColumnMapKeyBuilder builder)
      {
        base.AppendColumnMapKey(builder);
        foreach (MemberBinding binding in this._initExpression.Bindings)
        {
          builder.Append(",", binding.Member.DeclaringType);
          builder.Append("." + binding.Member.Name);
        }
      }
    }

    internal class EntityCollectionInitializerMetadata : InitializerMetadata
    {
      private readonly NavigationProperty _navigationProperty;
      internal static readonly MethodInfo CreateEntityCollectionMethod = typeof (InitializerMetadata.EntityCollectionInitializerMetadata).GetOnlyDeclaredMethod("CreateEntityCollection");

      internal EntityCollectionInitializerMetadata(Type type, NavigationProperty navigationProperty)
        : base(type)
      {
        this._navigationProperty = navigationProperty;
      }

      internal override InitializerMetadataKind Kind => InitializerMetadataKind.EntityCollection;

      protected override bool IsStructurallyEquivalent(InitializerMetadata other) => this._navigationProperty.Equals((object) ((InitializerMetadata.EntityCollectionInitializerMetadata) other)._navigationProperty);

      internal override Expression Emit(List<TranslatorResult> propertyTranslatorResults)
      {
        Type elementType = this.GetElementType();
        MethodInfo method = InitializerMetadata.EntityCollectionInitializerMetadata.CreateEntityCollectionMethod.MakeGenericMethod(elementType);
        Expression expression1 = propertyTranslatorResults[0].Expression;
        Expression toGetCoordinator = (propertyTranslatorResults[1] as CollectionTranslatorResult).ExpressionToGetCoordinator;
        Expression expression2 = expression1;
        Expression expression3 = toGetCoordinator;
        ConstantExpression constantExpression1 = Expression.Constant((object) this._navigationProperty.RelationshipType.FullName);
        ConstantExpression constantExpression2 = Expression.Constant((object) this._navigationProperty.ToEndMember.Name);
        return (Expression) Expression.Call(method, expression2, expression3, (Expression) constantExpression1, (Expression) constantExpression2);
      }

      public static EntityCollection<T> CreateEntityCollection<T>(
        IEntityWrapper wrappedOwner,
        Coordinator<T> coordinator,
        string relationshipName,
        string targetRoleName)
        where T : class
      {
        if (wrappedOwner.Entity == null)
          return (EntityCollection<T>) null;
        EntityCollection<T> result = wrappedOwner.RelationshipManager.GetRelatedCollection<T>(relationshipName, targetRoleName);
        coordinator.RegisterCloseHandler((Action<Shaper, List<IEntityWrapper>>) ((readerState, elements) => result.Load(elements, readerState.MergeOption)));
        return result;
      }

      internal override IEnumerable<Type> GetChildTypes()
      {
        Type elementType = this.GetElementType();
        yield return (Type) null;
        yield return typeof (IEnumerable<>).MakeGenericType(elementType);
      }

      internal override void AppendColumnMapKey(ColumnMapKeyBuilder builder)
      {
        base.AppendColumnMapKey(builder);
        builder.Append(",NP" + this._navigationProperty.Name);
        builder.Append(",AT", (EdmType) this._navigationProperty.DeclaringType);
      }

      private Type GetElementType()
      {
        Type elementType = this.ClrType.TryGetElementType(typeof (ICollection<>));
        return !(elementType == (Type) null) ? elementType : throw new InvalidOperationException(System.Data.Entity.Resources.Strings.ELinq_UnexpectedTypeForNavigationProperty((object) this._navigationProperty, (object) typeof (EntityCollection<>), (object) typeof (ICollection<>), (object) this.ClrType));
      }
    }
  }
}

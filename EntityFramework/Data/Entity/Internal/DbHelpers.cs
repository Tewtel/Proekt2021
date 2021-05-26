// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Internal.DbHelpers
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Utilities;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Entity.Internal
{
  internal static class DbHelpers
  {
    public static readonly MethodInfo ConvertAndSetMethod = typeof (DbHelpers).GetOnlyDeclaredMethod("ConvertAndSet");
    private static readonly ConcurrentDictionary<Type, IDictionary<string, Type>> _propertyTypes = new ConcurrentDictionary<Type, IDictionary<string, Type>>();
    private static readonly ConcurrentDictionary<Type, IDictionary<string, Action<object, object>>> _propertySetters = new ConcurrentDictionary<Type, IDictionary<string, Action<object, object>>>();
    private static readonly ConcurrentDictionary<Type, IDictionary<string, Func<object, object>>> _propertyGetters = new ConcurrentDictionary<Type, IDictionary<string, Func<object, object>>>();
    private static readonly ConcurrentDictionary<Type, Type> _collectionTypes = new ConcurrentDictionary<Type, Type>();

    public static bool KeyValuesEqual(object x, object y)
    {
      if (x is DBNull)
        x = (object) null;
      if (y is DBNull)
        y = (object) null;
      if (object.Equals(x, y))
        return true;
      byte[] numArray1 = x as byte[];
      byte[] numArray2 = y as byte[];
      if (numArray1 == null || numArray2 == null || numArray1.Length != numArray2.Length)
        return false;
      for (int index = 0; index < numArray1.Length; ++index)
      {
        if ((int) numArray1[index] != (int) numArray2[index])
          return false;
      }
      return true;
    }

    public static bool PropertyValuesEqual(object x, object y)
    {
      if (x is DBNull)
        x = (object) null;
      if (y is DBNull)
        y = (object) null;
      if (x == null)
        return y == null;
      if (x.GetType().IsValueType() && object.Equals(x, y))
        return true;
      switch (x)
      {
        case string str:
          return str.Equals(y as string, StringComparison.Ordinal);
        case byte[] numArray2:
          if (!(y is byte[] numArray3) || numArray2.Length != numArray3.Length)
            return false;
          for (int index = 0; index < numArray2.Length; ++index)
          {
            if ((int) numArray2[index] != (int) numArray3[index])
              return false;
          }
          return true;
        default:
          return x == y;
      }
    }

    public static string QuoteIdentifier(string identifier) => "[" + identifier.Replace("]", "]]") + "]";

    public static bool TreatAsConnectionString(string nameOrConnectionString) => nameOrConnectionString.IndexOf('=') >= 0;

    public static bool TryGetConnectionName(string nameOrConnectionString, out string name)
    {
      int length = nameOrConnectionString.IndexOf('=');
      if (length < 0)
      {
        name = nameOrConnectionString;
        return true;
      }
      if (nameOrConnectionString.IndexOf('=', length + 1) >= 0)
      {
        name = (string) null;
        return false;
      }
      if (nameOrConnectionString.Substring(0, length).Trim().Equals(nameof (name), StringComparison.OrdinalIgnoreCase))
      {
        name = nameOrConnectionString.Substring(length + 1).Trim();
        return true;
      }
      name = (string) null;
      return false;
    }

    public static bool IsFullEFConnectionString(string nameOrConnectionString)
    {
      IEnumerable<string> source = ((IEnumerable<string>) nameOrConnectionString.ToUpperInvariant().Split('=', ';')).Select<string, string>((Func<string, string>) (t => t.Trim()));
      return source.Contains<string>("PROVIDER") && source.Contains<string>("PROVIDER CONNECTION STRING") && source.Contains<string>("METADATA");
    }

    public static string ParsePropertySelector<TEntity, TProperty>(
      Expression<Func<TEntity, TProperty>> property,
      string methodName,
      string paramName)
    {
      string path;
      if (!DbHelpers.TryParsePath(property.Body, out path) || path == null)
        throw new ArgumentException(System.Data.Entity.Resources.Strings.DbEntityEntry_BadPropertyExpression((object) methodName, (object) typeof (TEntity).Name), paramName);
      return path;
    }

    public static bool TryParsePath(Expression expression, out string path)
    {
      path = (string) null;
      Expression expression1 = expression.RemoveConvert();
      MemberExpression memberExpression = expression1 as MemberExpression;
      MethodCallExpression methodCallExpression = expression1 as MethodCallExpression;
      if (memberExpression != null)
      {
        string name = memberExpression.Member.Name;
        string path1;
        if (!DbHelpers.TryParsePath(memberExpression.Expression, out path1))
          return false;
        path = path1 == null ? name : path1 + "." + name;
      }
      else if (methodCallExpression != null)
      {
        string path1;
        string path2;
        if (!(methodCallExpression.Method.Name == "Select") || methodCallExpression.Arguments.Count != 2 || (!DbHelpers.TryParsePath(methodCallExpression.Arguments[0], out path1) || path1 == null) || (!(methodCallExpression.Arguments[1] is LambdaExpression lambdaExpression3) || !DbHelpers.TryParsePath(lambdaExpression3.Body, out path2) || path2 == null))
          return false;
        path = path1 + "." + path2;
        return true;
      }
      return true;
    }

    public static IDictionary<string, Type> GetPropertyTypes(Type type)
    {
      IDictionary<string, Type> dictionary;
      if (!DbHelpers._propertyTypes.TryGetValue(type, out dictionary))
      {
        IEnumerable<PropertyInfo> source = type.GetInstanceProperties().Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.GetIndexParameters().Length == 0));
        dictionary = (IDictionary<string, Type>) new Dictionary<string, Type>(source.Count<PropertyInfo>());
        foreach (PropertyInfo propertyInfo in source)
          dictionary[propertyInfo.Name] = propertyInfo.PropertyType;
        DbHelpers._propertyTypes.TryAdd(type, dictionary);
      }
      return dictionary;
    }

    public static IDictionary<string, Action<object, object>> GetPropertySetters(
      Type type)
    {
      IDictionary<string, Action<object, object>> dictionary;
      if (!DbHelpers._propertySetters.TryGetValue(type, out dictionary))
      {
        IEnumerable<PropertyInfo> source = type.GetInstanceProperties().Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.GetIndexParameters().Length == 0));
        dictionary = (IDictionary<string, Action<object, object>>) new Dictionary<string, Action<object, object>>(source.Count<PropertyInfo>());
        foreach (PropertyInfo property in source.Select<PropertyInfo, PropertyInfo>((Func<PropertyInfo, PropertyInfo>) (p => p.GetPropertyInfoForSet())))
        {
          MethodInfo method = property.Setter();
          if (method != (MethodInfo) null)
          {
            ParameterExpression parameterExpression1;
            ParameterExpression parameterExpression2;
            Action<object, object> setter = Expression.Lambda<Action<object, object>>((Expression) Expression.Call((Expression) Expression.Convert((Expression) parameterExpression2, type), method, (Expression) Expression.Convert((Expression) parameterExpression1, property.PropertyType)), parameterExpression2, parameterExpression1).Compile();
            Action<object, object, Action<object, object>, string, string> convertAndSet = (Action<object, object, Action<object, object>, string, string>) Delegate.CreateDelegate(typeof (Action<object, object, Action<object, object>, string, string>), DbHelpers.ConvertAndSetMethod.MakeGenericMethod(property.PropertyType));
            string propertyName = property.Name;
            dictionary[property.Name] = (Action<object, object>) ((i, v) => convertAndSet(i, v, setter, propertyName, type.Name));
          }
        }
        DbHelpers._propertySetters.TryAdd(type, dictionary);
      }
      return dictionary;
    }

    private static void ConvertAndSet<T>(
      object instance,
      object value,
      Action<object, object> setter,
      string propertyName,
      string typeName)
    {
      if (value == null && typeof (T).IsValueType() && Nullable.GetUnderlyingType(typeof (T)) == (Type) null)
        throw System.Data.Entity.Resources.Error.DbPropertyValues_CannotSetNullValue((object) propertyName, (object) typeof (T).Name, (object) typeName);
      setter(instance, (object) (T) value);
    }

    public static IDictionary<string, Func<object, object>> GetPropertyGetters(
      Type type)
    {
      IDictionary<string, Func<object, object>> dictionary;
      if (!DbHelpers._propertyGetters.TryGetValue(type, out dictionary))
      {
        IEnumerable<PropertyInfo> source = type.GetInstanceProperties().Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.GetIndexParameters().Length == 0));
        dictionary = (IDictionary<string, Func<object, object>>) new Dictionary<string, Func<object, object>>(source.Count<PropertyInfo>());
        foreach (PropertyInfo property in source)
        {
          MethodInfo method = property.Getter();
          if (method != (MethodInfo) null)
          {
            ParameterExpression parameterExpression = Expression.Parameter(typeof (object), "instance");
            UnaryExpression unaryExpression = Expression.Convert((Expression) Expression.Call((Expression) Expression.Convert((Expression) parameterExpression, type), method), typeof (object));
            dictionary[property.Name] = Expression.Lambda<Func<object, object>>((Expression) unaryExpression, parameterExpression).Compile();
          }
        }
        DbHelpers._propertyGetters.TryAdd(type, dictionary);
      }
      return dictionary;
    }

    public static IQueryable CreateNoTrackingQuery(ObjectQuery query)
    {
      IQueryable queryable = (IQueryable) query;
      ObjectQuery query1 = (ObjectQuery) queryable.Provider.CreateQuery(queryable.Expression);
      query1.ExecutionStrategy = query.ExecutionStrategy;
      query1.MergeOption = MergeOption.NoTracking;
      query1.Streaming = query.Streaming;
      return (IQueryable) query1;
    }

    public static IQueryable CreateStreamingQuery(ObjectQuery query)
    {
      IQueryable queryable = (IQueryable) query;
      ObjectQuery query1 = (ObjectQuery) queryable.Provider.CreateQuery(queryable.Expression);
      query1.ExecutionStrategy = query.ExecutionStrategy;
      query1.Streaming = true;
      query1.MergeOption = query.MergeOption;
      return (IQueryable) query1;
    }

    public static IQueryable CreateQueryWithExecutionStrategy(
      ObjectQuery query,
      IDbExecutionStrategy executionStrategy)
    {
      IQueryable queryable = (IQueryable) query;
      ObjectQuery query1 = (ObjectQuery) queryable.Provider.CreateQuery(queryable.Expression);
      query1.ExecutionStrategy = executionStrategy;
      query1.MergeOption = query.MergeOption;
      query1.Streaming = query.Streaming;
      return (IQueryable) query1;
    }

    public static IEnumerable<DbValidationError> SplitValidationResults(
      string propertyName,
      IEnumerable<ValidationResult> validationResults)
    {
      foreach (ValidationResult validationResult1 in validationResults)
      {
        ValidationResult validationResult = validationResult1;
        if (validationResult != null)
        {
          foreach (string str in validationResult.MemberNames == null || !validationResult.MemberNames.Any<string>() ? (IEnumerable<string>) new string[1] : validationResult.MemberNames)
            yield return new DbValidationError(str ?? propertyName, validationResult.ErrorMessage);
          validationResult = (ValidationResult) null;
        }
      }
    }

    public static string GetPropertyPath(InternalMemberEntry property) => string.Join(".", DbHelpers.GetPropertyPathSegments(property).Reverse<string>());

    private static IEnumerable<string> GetPropertyPathSegments(
      InternalMemberEntry property)
    {
      do
      {
        yield return property.Name;
        property = property is InternalNestedPropertyEntry ? (InternalMemberEntry) ((InternalPropertyEntry) property).ParentPropertyEntry : (InternalMemberEntry) null;
      }
      while (property != null);
    }

    public static Type CollectionType(Type elementType) => DbHelpers._collectionTypes.GetOrAdd(elementType, (Func<Type, Type>) (t => typeof (ICollection<>).MakeGenericType(t)));

    public static string DatabaseName(this Type contextType) => contextType.ToString();
  }
}

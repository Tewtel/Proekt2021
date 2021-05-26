// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder.Hierarchy.HierarchyIdEdmFunctions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder.Hierarchy
{
  /// <summary>
  ///     Provides an API to construct <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbExpression" />s that invoke hierarchyid realted canonical EDM functions, and, where appropriate, allows that API to be accessed as extension methods on the expression type itself.
  /// </summary>
  public static class HierarchyIdEdmFunctions
  {
    /// <summary>
    ///     Creates a <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbFunctionExpression" /> that invokes the canonical 'HierarchyIdParse' function with the
    ///     specified argument, which must have a string result type.
    ///     The result type of the expression is Edm.HierarchyId.
    /// </summary>
    /// <param name="input"> An expression that provides the canonical representation of the hierarchyid value. </param>
    /// <returns> A new DbFunctionExpression that returns a new hierarchyid value based on the specified value. </returns>
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="input" />
    ///     is null.
    /// </exception>
    /// <exception cref="T:System.ArgumentException">
    ///     No overload of the canonical 'HierarchyIdParse' function accept an argument with the result type of
    ///     <paramref name="input" />
    ///     .
    /// </exception>
    public static DbFunctionExpression HierarchyIdParse(DbExpression input)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(input, nameof (input));
      return EdmFunctions.InvokeCanonicalFunction(nameof (HierarchyIdParse), input);
    }

    /// <summary>
    ///     Creates a <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbFunctionExpression" /> that invokes the canonical 'HierarchyIdGetRoot' function.
    ///     The result type of the expression is Edm.HierarchyId.
    /// </summary>
    /// <returns> A new DbFunctionExpression that returns a new root hierarchyid value. </returns>
    public static DbFunctionExpression HierarchyIdGetRoot() => EdmFunctions.InvokeCanonicalFunction(nameof (HierarchyIdGetRoot));

    /// <summary>
    ///     Creates a <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbFunctionExpression" /> that invokes the canonical 'GetAncestor' function with the
    ///     specified argument, which must have an Int32 result type.
    ///     The result type of the expression is Edm.HierarchyId.
    /// </summary>
    /// <param name="hierarchyIdValue"> An expression that specifies the hierarchyid value. </param>
    /// <param name="n"> An expression that provides an integer value. </param>
    /// <returns> A new DbFunctionExpression that returns a hierarchyid. </returns>
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="hierarchyIdValue" />
    ///     or
    ///     <paramref name="n" />
    ///     is null.
    /// </exception>
    /// <exception cref="T:System.ArgumentException">
    ///     No overload of the canonical 'GetAncestor' function accept an argument with the result type of
    ///     <paramref name="n" />
    ///     .
    /// </exception>
    public static DbFunctionExpression GetAncestor(
      this DbExpression hierarchyIdValue,
      DbExpression n)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(hierarchyIdValue, nameof (hierarchyIdValue));
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(n, nameof (n));
      return EdmFunctions.InvokeCanonicalFunction(nameof (GetAncestor), hierarchyIdValue, n);
    }

    /// <summary>
    ///     Creates a <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbFunctionExpression" /> that invokes the canonical 'GetDescendant' function with the
    ///     specified argument, which must have a HierarchyId result type.
    ///     The result type of the expression is Edm.HierarchyId.
    /// </summary>
    /// <param name="hierarchyIdValue"> An expression that specifies the hierarchyid value. </param>
    /// <param name="child1"> An expression that provides a hierarchyid value. </param>
    /// <param name="child2"> An expression that provides a hierarchyid value. </param>
    /// <returns> A new DbFunctionExpression that returns a hierarchyid. </returns>
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="hierarchyIdValue" />
    ///     or
    ///     <paramref name="child1" />
    ///     or
    ///     <paramref name="child2" />
    ///     is null.
    /// </exception>
    /// <exception cref="T:System.ArgumentException">
    ///     No overload of the canonical 'GetDescendant' function accept an argument with the result type of
    ///     <paramref name="child1" />
    ///     and
    ///     <paramref name="child2" />
    ///     .
    /// </exception>
    public static DbFunctionExpression GetDescendant(
      this DbExpression hierarchyIdValue,
      DbExpression child1,
      DbExpression child2)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(hierarchyIdValue, nameof (hierarchyIdValue));
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(child1, nameof (child1));
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(child2, nameof (child2));
      return EdmFunctions.InvokeCanonicalFunction(nameof (GetDescendant), hierarchyIdValue, child1, child2);
    }

    /// <summary>
    ///     Creates a <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbFunctionExpression" /> that invokes the canonical 'GetLevel' function.
    ///     The result type of the expression is Int32.
    /// </summary>
    /// <param name="hierarchyIdValue"> An expression that specifies the hierarchyid value. </param>
    /// <returns> A new DbFunctionExpression that returns the level of the given hierarchyid. </returns>
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="hierarchyIdValue" />
    ///     is null.
    /// </exception>
    public static DbFunctionExpression GetLevel(
      this DbExpression hierarchyIdValue)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(hierarchyIdValue, nameof (hierarchyIdValue));
      return EdmFunctions.InvokeCanonicalFunction(nameof (GetLevel), hierarchyIdValue);
    }

    /// <summary>
    ///     Creates a <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbFunctionExpression" /> that invokes the canonical 'IsDescendantOf' function with the
    ///     specified argument, which must have a HierarchyId result type.
    ///     The result type of the expression is Int32.
    /// </summary>
    /// <param name="hierarchyIdValue"> An expression that specifies the hierarchyid value. </param>
    /// <param name="parent"> An expression that provides a hierarchyid value. </param>
    /// <returns> A new DbFunctionExpression that returns an integer value. </returns>
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="hierarchyIdValue" />
    ///     or
    ///     <paramref name="parent" />
    ///     is null.
    /// </exception>
    /// <exception cref="T:System.ArgumentException">
    ///     No overload of the canonical 'IsDescendantOf' function accept an argument with the result type of
    ///     <paramref name="parent" />
    ///     .
    /// </exception>
    public static DbFunctionExpression IsDescendantOf(
      this DbExpression hierarchyIdValue,
      DbExpression parent)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(hierarchyIdValue, nameof (hierarchyIdValue));
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(parent, nameof (parent));
      return EdmFunctions.InvokeCanonicalFunction(nameof (IsDescendantOf), hierarchyIdValue, parent);
    }

    /// <summary>
    ///     Creates a <see cref="T:System.Data.Entity.Core.Common.CommandTrees.DbFunctionExpression" /> that invokes the canonical 'GetReparentedValue' function with the
    ///     specified arguments, which must have a HierarchyId result type.
    ///     The result type of the expression is Edm.HierarchyId.
    /// </summary>
    /// <param name="hierarchyIdValue"> An expression that specifies the hierarchyid value. </param>
    /// <param name="oldRoot"> An expression that provides a hierarchyid value. </param>
    /// <param name="newRoot"> An expression that provides a hierarchyid value. </param>
    /// <returns> A new DbFunctionExpression that returns a hierarchyid. </returns>
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="hierarchyIdValue" />
    ///     or
    ///     <paramref name="oldRoot" />
    ///     or
    ///     <paramref name="newRoot" />
    ///     is null.
    /// </exception>
    /// <exception cref="T:System.ArgumentException">
    ///     No overload of the canonical 'GetReparentedValue' function accept an argument with the result type of
    ///     <paramref name="oldRoot" />
    ///     and
    ///     <paramref name="newRoot" />
    ///     .
    /// </exception>
    public static DbFunctionExpression GetReparentedValue(
      this DbExpression hierarchyIdValue,
      DbExpression oldRoot,
      DbExpression newRoot)
    {
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(hierarchyIdValue, nameof (hierarchyIdValue));
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(oldRoot, nameof (oldRoot));
      System.Data.Entity.Utilities.Check.NotNull<DbExpression>(newRoot, nameof (newRoot));
      return EdmFunctions.InvokeCanonicalFunction(nameof (GetReparentedValue), hierarchyIdValue, oldRoot, newRoot);
    }
  }
}

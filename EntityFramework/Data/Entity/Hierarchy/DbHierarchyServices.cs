// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Hierarchy.DbHierarchyServices
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Hierarchy
{
  /// <summary>
  ///     A provider-independent service API for HierarchyId type support.
  /// </summary>
  [Serializable]
  public abstract class DbHierarchyServices
  {
    /// <summary>
    ///     Returns a hierarchyid representing the nth ancestor of this.
    /// </summary>
    /// <returns>A hierarchyid representing the nth ancestor of this.</returns>
    /// <param name="n">n</param>
    public abstract HierarchyId GetAncestor(int n);

    /// <summary>Returns a child node of the parent.</summary>
    /// <param name="child1"> null or the hierarchyid of a child of the current node. </param>
    /// <param name="child2"> null or the hierarchyid of a child of the current node. </param>
    /// <returns>
    /// Returns one child node that is a descendant of the parent.
    /// If parent is null, returns null.
    /// If parent is not null, and both child1 and child2 are null, returns a child of parent.
    /// If parent and child1 are not null, and child2 is null, returns a child of parent greater than child1.
    /// If parent and child2 are not null and child1 is null, returns a child of parent less than child2.
    /// If parent, child1, and child2 are not null, returns a child of parent greater than child1 and less than child2.
    /// If child1 is not null and not a child of parent, an exception is raised.
    /// If child2 is not null and not a child of parent, an exception is raised.
    /// If child1 &gt;= child2, an exception is raised.
    /// </returns>
    public abstract HierarchyId GetDescendant(HierarchyId child1, HierarchyId child2);

    /// <summary>
    ///     Returns an integer that represents the depth of the node this in the tree.
    /// </summary>
    /// <returns>An integer that represents the depth of the node this in the tree.</returns>
    public abstract short GetLevel();

    /// <summary>Returns the root of the hierarchy tree.</summary>
    /// <returns>The root of the hierarchy tree.</returns>
    public static HierarchyId GetRoot() => new HierarchyId("/");

    /// <summary>Returns true if this is a descendant of parent.</summary>
    /// <returns>True if this is a descendant of parent.</returns>
    /// <param name="parent">parent</param>
    public abstract bool IsDescendantOf(HierarchyId parent);

    /// <summary>
    ///     Returns a node whose path from the root is the path to newRoot, followed by the path from oldRoot to this.
    /// </summary>
    /// <returns>Hierarchyid value.</returns>
    /// <param name="oldRoot">oldRoot</param>
    /// <param name="newRoot">newRoot</param>
    public abstract HierarchyId GetReparentedValue(
      HierarchyId oldRoot,
      HierarchyId newRoot);

    /// <summary>
    ///     Converts the canonical string representation of a hierarchyid to a hierarchyid value.
    /// </summary>
    /// <returns>Hierarchyid value.</returns>
    /// <param name="input">input</param>
    public static HierarchyId Parse(string input) => new HierarchyId(input);
  }
}

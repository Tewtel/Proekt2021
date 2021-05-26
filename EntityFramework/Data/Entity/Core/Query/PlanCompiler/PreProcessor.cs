// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.PlanCompiler.PreProcessor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Mapping.ViewGeneration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.InternalTrees;
using System.Data.Entity.Resources;
using System.Runtime.CompilerServices;

namespace System.Data.Entity.Core.Query.PlanCompiler
{
  internal class PreProcessor : SubqueryTrackingVisitor
  {
    private readonly Stack<EntitySet> m_entityTypeScopes = new Stack<EntitySet>();
    private readonly HashSet<EntityContainer> m_referencedEntityContainers = new HashSet<EntityContainer>();
    private readonly HashSet<EntitySet> m_referencedEntitySets = new HashSet<EntitySet>();
    private readonly HashSet<TypeUsage> m_referencedTypes = new HashSet<TypeUsage>();
    private readonly HashSet<EntityType> m_freeFloatingEntityConstructorTypes = new HashSet<EntityType>();
    private readonly HashSet<string> m_typesNeedingNullSentinel = new HashSet<string>();
    private readonly Dictionary<EdmFunction, EdmProperty[]> m_tvfResultKeys = new Dictionary<EdmFunction, EdmProperty[]>();
    private readonly RelPropertyHelper m_relPropertyHelper;
    private bool m_suppressDiscriminatorMaps;
    private readonly Dictionary<EntitySetBase, DiscriminatorMapInfo> m_discriminatorMaps = new Dictionary<EntitySetBase, DiscriminatorMapInfo>();
    private readonly Dictionary<PreProcessor.NavigationPropertyOpInfo, System.Data.Entity.Core.Query.InternalTrees.Node> _navigationPropertyOpRewrites = new Dictionary<PreProcessor.NavigationPropertyOpInfo, System.Data.Entity.Core.Query.InternalTrees.Node>();

    private PreProcessor(System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler planCompilerState)
      : base(planCompilerState)
    {
      this.m_relPropertyHelper = new RelPropertyHelper(this.m_command.MetadataWorkspace, this.m_command.ReferencedRelProperties);
    }

    internal static void Process(
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler planCompilerState,
      out StructuredTypeInfo typeInfo,
      out Dictionary<EdmFunction, EdmProperty[]> tvfResultKeys)
    {
      PreProcessor preProcessor = new PreProcessor(planCompilerState);
      preProcessor.Process(out tvfResultKeys);
      StructuredTypeInfo.Process(planCompilerState.Command, preProcessor.m_referencedTypes, preProcessor.m_referencedEntitySets, preProcessor.m_freeFloatingEntityConstructorTypes, preProcessor.m_suppressDiscriminatorMaps ? (Dictionary<EntitySetBase, DiscriminatorMapInfo>) null : preProcessor.m_discriminatorMaps, preProcessor.m_relPropertyHelper, preProcessor.m_typesNeedingNullSentinel, out typeInfo);
    }

    internal void Process(
      out Dictionary<EdmFunction, EdmProperty[]> tvfResultKeys)
    {
      this.m_command.Root = this.VisitNode(this.m_command.Root);
      foreach (Var var in this.m_command.Vars)
        this.AddTypeReference(var.Type);
      if (this.m_referencedTypes.Count > 0)
      {
        this.m_compilerState.MarkPhaseAsNeeded(PlanCompilerPhase.NTE);
        ((PhysicalProjectOp) this.m_command.Root.Op).ColumnMap.Accept<HashSet<string>>((ColumnMapVisitor<HashSet<string>>) StructuredTypeNullabilityAnalyzer.Instance, this.m_typesNeedingNullSentinel);
      }
      tvfResultKeys = this.m_tvfResultKeys;
    }

    private void AddEntitySetReference(EntitySet entitySet)
    {
      this.m_referencedEntitySets.Add(entitySet);
      if (this.m_referencedEntityContainers.Contains(entitySet.EntityContainer))
        return;
      this.m_referencedEntityContainers.Add(entitySet.EntityContainer);
    }

    private void AddTypeReference(TypeUsage type)
    {
      if (!TypeUtils.IsStructuredType(type) && !TypeUtils.IsCollectionType(type) && !TypeUtils.IsEnumerationType(type))
        return;
      this.m_referencedTypes.Add(type);
    }

    private List<RelationshipSet> GetRelationshipSets(RelationshipType relType)
    {
      List<RelationshipSet> relationshipSetList = new List<RelationshipSet>();
      foreach (EntityContainer referencedEntityContainer in this.m_referencedEntityContainers)
      {
        foreach (EntitySetBase baseEntitySet in referencedEntityContainer.BaseEntitySets)
        {
          if (baseEntitySet is RelationshipSet relationshipSet2 && relationshipSet2.ElementType.Equals((object) relType))
            relationshipSetList.Add(relationshipSet2);
        }
      }
      return relationshipSetList;
    }

    private List<EntitySet> GetEntitySets(TypeUsage entityType)
    {
      List<EntitySet> entitySetList = new List<EntitySet>();
      foreach (EntityContainer referencedEntityContainer in this.m_referencedEntityContainers)
      {
        foreach (EntitySetBase baseEntitySet in referencedEntityContainer.BaseEntitySets)
        {
          if (baseEntitySet is EntitySet entitySet2 && (entitySet2.ElementType.Equals((object) entityType.EdmType) || TypeSemantics.IsSubTypeOf(entityType.EdmType, (EdmType) entitySet2.ElementType) || TypeSemantics.IsSubTypeOf((EdmType) entitySet2.ElementType, entityType.EdmType)))
            entitySetList.Add(entitySet2);
        }
      }
      return entitySetList;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node ExpandView(
      ScanTableOp scanTableOp,
      ref IsOfOp typeFilter)
    {
      EntitySetBase extent = scanTableOp.Table.TableMetadata.Extent;
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(extent != null, "The target of a ScanTableOp must reference an EntitySet to be used with ExpandView");
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(extent.EntityContainer.DataSpace == DataSpace.CSpace, "Store entity sets cannot have Query Mapping Views and should not be used with ExpandView");
      if (typeFilter != null && !typeFilter.IsOfOnly && TypeSemantics.IsSubTypeOf((EdmType) extent.ElementType, typeFilter.IsOfType.EdmType))
        typeFilter = (IsOfOp) null;
      GeneratedView generatedView = (GeneratedView) null;
      EntityTypeBase entityTypeBase = scanTableOp.Table.TableMetadata.Extent.ElementType;
      bool includeSubtypes = true;
      if (typeFilter != null)
      {
        entityTypeBase = (EntityTypeBase) typeFilter.IsOfType.EdmType;
        includeSubtypes = !typeFilter.IsOfOnly;
        if (this.m_command.MetadataWorkspace.TryGetGeneratedViewOfType(extent, entityTypeBase, includeSubtypes, out generatedView))
          typeFilter = (IsOfOp) null;
      }
      if (generatedView == null)
        generatedView = this.m_command.MetadataWorkspace.GetGeneratedView(extent);
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(generatedView != null, Strings.ADP_NoQueryMappingView((object) extent.EntityContainer.Name, (object) extent.Name));
      System.Data.Entity.Core.Query.InternalTrees.Node internalTree = generatedView.GetInternalTree(this.m_command);
      this.DetermineDiscriminatorMapUsage(internalTree, extent, entityTypeBase, includeSubtypes);
      return this.m_command.CreateNode((Op) this.m_command.CreateScanViewOp(scanTableOp.Table), internalTree);
    }

    private void DetermineDiscriminatorMapUsage(
      System.Data.Entity.Core.Query.InternalTrees.Node viewNode,
      EntitySetBase entitySet,
      EntityTypeBase rootEntityType,
      bool includeSubtypes)
    {
      ExplicitDiscriminatorMap discriminatorMap = (ExplicitDiscriminatorMap) null;
      if (viewNode.Op.OpType == OpType.Project && viewNode.Child1.Child0.Child0.Op is DiscriminatedNewEntityOp op)
        discriminatorMap = op.DiscriminatorMap;
      DiscriminatorMapInfo discriminatorMapInfo;
      if (!this.m_discriminatorMaps.TryGetValue(entitySet, out discriminatorMapInfo))
      {
        if (rootEntityType == null)
        {
          rootEntityType = entitySet.ElementType;
          includeSubtypes = true;
        }
        discriminatorMapInfo = new DiscriminatorMapInfo(rootEntityType, includeSubtypes, discriminatorMap);
        this.m_discriminatorMaps.Add(entitySet, discriminatorMapInfo);
      }
      else
        discriminatorMapInfo.Merge(rootEntityType, includeSubtypes, discriminatorMap);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node RewriteNavigateOp(
      System.Data.Entity.Core.Query.InternalTrees.Node navigateOpNode,
      NavigateOp navigateOp,
      out Var outputVar)
    {
      outputVar = (Var) null;
      if (!Helper.IsAssociationType((EdmType) navigateOp.Relationship))
        throw new NotSupportedException(Strings.Cqt_RelNav_NoCompositions);
      if (navigateOpNode.Child0.Op.OpType == OpType.GetEntityRef && (navigateOp.ToEnd.RelationshipMultiplicity == RelationshipMultiplicity.ZeroOrOne || navigateOp.ToEnd.RelationshipMultiplicity == RelationshipMultiplicity.One))
      {
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(this.m_command.IsRelPropertyReferenced(navigateOp.RelProperty), "Unreferenced rel property? " + navigateOp.RelProperty?.ToString());
        return this.m_command.CreateNode((Op) this.m_command.CreateRelPropertyOp(navigateOp.RelProperty), navigateOpNode.Child0.Child0);
      }
      List<RelationshipSet> relationshipSets = this.GetRelationshipSets(navigateOp.Relationship);
      if (relationshipSets.Count == 0)
        return navigateOp.ToEnd.RelationshipMultiplicity != RelationshipMultiplicity.Many ? this.m_command.CreateNode((Op) this.m_command.CreateNullOp(navigateOp.Type)) : this.m_command.CreateNode((Op) this.m_command.CreateNewMultisetOp(navigateOp.Type));
      List<System.Data.Entity.Core.Query.InternalTrees.Node> nodeList = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      List<Var> varList = new List<Var>();
      foreach (EntitySetBase extent in relationshipSets)
      {
        ScanTableOp scanTableOp = this.m_command.CreateScanTableOp(Command.CreateTableDefinition(extent));
        System.Data.Entity.Core.Query.InternalTrees.Node node = this.m_command.CreateNode((Op) scanTableOp);
        Var column = scanTableOp.Table.Columns[0];
        varList.Add(column);
        nodeList.Add(node);
      }
      System.Data.Entity.Core.Query.InternalTrees.Node resultNode = (System.Data.Entity.Core.Query.InternalTrees.Node) null;
      Var resultVar;
      this.m_command.BuildUnionAllLadder((IList<System.Data.Entity.Core.Query.InternalTrees.Node>) nodeList, (IList<Var>) varList, out resultNode, out resultVar);
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.m_command.CreateNode((Op) this.m_command.CreatePropertyOp((EdmMember) navigateOp.ToEnd), this.m_command.CreateNode((Op) this.m_command.CreateVarRefOp(resultVar)));
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.m_command.CreateNode((Op) this.m_command.CreatePropertyOp((EdmMember) navigateOp.FromEnd), this.m_command.CreateNode((Op) this.m_command.CreateVarRefOp(resultVar)));
      System.Data.Entity.Core.Query.InternalTrees.Node node3 = this.m_command.BuildComparison(OpType.EQ, navigateOpNode.Child0, node2, true);
      Var projectVar;
      System.Data.Entity.Core.Query.InternalTrees.Node relOpNode = this.m_command.BuildProject(this.m_command.CreateNode((Op) this.m_command.CreateFilterOp(), resultNode, node3), node1, out projectVar);
      System.Data.Entity.Core.Query.InternalTrees.Node node4;
      if (navigateOp.ToEnd.RelationshipMultiplicity == RelationshipMultiplicity.Many)
      {
        node4 = this.m_command.BuildCollect(relOpNode, projectVar);
      }
      else
      {
        node4 = relOpNode;
        outputVar = projectVar;
      }
      return node4;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node BuildOfTypeTable(
      EntitySetBase entitySet,
      TypeUsage ofType,
      out Var resultVar)
    {
      ScanTableOp scanTableOp = this.m_command.CreateScanTableOp(Command.CreateTableDefinition(entitySet));
      System.Data.Entity.Core.Query.InternalTrees.Node node = this.m_command.CreateNode((Op) scanTableOp);
      Var column = scanTableOp.Table.Columns[0];
      System.Data.Entity.Core.Query.InternalTrees.Node resultNode;
      if (ofType != null && !entitySet.ElementType.EdmEquals((MetadataItem) ofType.EdmType))
      {
        this.m_command.BuildOfTypeTree(node, column, ofType, true, out resultNode, out resultVar);
      }
      else
      {
        resultNode = node;
        resultVar = column;
      }
      return resultNode;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node RewriteDerefOp(
      System.Data.Entity.Core.Query.InternalTrees.Node derefOpNode,
      DerefOp derefOp,
      out Var outputVar)
    {
      TypeUsage type = derefOp.Type;
      List<EntitySet> entitySets = this.GetEntitySets(type);
      if (entitySets.Count == 0)
      {
        outputVar = (Var) null;
        return this.m_command.CreateNode((Op) this.m_command.CreateNullOp(type));
      }
      List<System.Data.Entity.Core.Query.InternalTrees.Node> nodeList = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      List<Var> varList = new List<Var>();
      foreach (EntitySetBase entitySet in entitySets)
      {
        Var resultVar;
        System.Data.Entity.Core.Query.InternalTrees.Node node = this.BuildOfTypeTable(entitySet, type, out resultVar);
        nodeList.Add(node);
        varList.Add(resultVar);
      }
      System.Data.Entity.Core.Query.InternalTrees.Node resultNode;
      Var resultVar1;
      this.m_command.BuildUnionAllLadder((IList<System.Data.Entity.Core.Query.InternalTrees.Node>) nodeList, (IList<Var>) varList, out resultNode, out resultVar1);
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.m_command.CreateNode((Op) this.m_command.CreateGetEntityRefOp(derefOpNode.Child0.Op.Type), this.m_command.CreateNode((Op) this.m_command.CreateVarRefOp(resultVar1)));
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.m_command.BuildComparison(OpType.EQ, derefOpNode.Child0, node1, true);
      System.Data.Entity.Core.Query.InternalTrees.Node node3 = this.m_command.CreateNode((Op) this.m_command.CreateFilterOp(), resultNode, node2);
      outputVar = resultVar1;
      return node3;
    }

    private static EntitySetBase FindTargetEntitySet(
      RelationshipSet relationshipSet,
      RelationshipEndMember targetEnd)
    {
      AssociationSet associationSet = (AssociationSet) relationshipSet;
      EntitySetBase entitySetBase = (EntitySetBase) null;
      foreach (AssociationSetEnd associationSetEnd in associationSet.AssociationSetEnds)
      {
        if (associationSetEnd.CorrespondingAssociationEndMember.EdmEquals((MetadataItem) targetEnd))
        {
          entitySetBase = (EntitySetBase) associationSetEnd.EntitySet;
          break;
        }
      }
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(entitySetBase != null, "Could not find entity set for relationship set " + relationshipSet?.ToString() + ";association end " + targetEnd?.ToString());
      return entitySetBase;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node BuildJoinForNavProperty(
      RelationshipSet relSet,
      RelationshipEndMember end,
      out Var rsVar,
      out Var esVar)
    {
      EntitySetBase targetEntitySet = PreProcessor.FindTargetEntitySet(relSet, end);
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.BuildOfTypeTable((EntitySetBase) relSet, (TypeUsage) null, out rsVar);
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.BuildOfTypeTable(targetEntitySet, TypeHelpers.GetElementTypeUsage(end.TypeUsage), out esVar);
      System.Data.Entity.Core.Query.InternalTrees.Node node3 = this.m_command.BuildComparison(OpType.EQ, this.m_command.CreateNode((Op) this.m_command.CreateGetEntityRefOp(end.TypeUsage), this.m_command.CreateNode((Op) this.m_command.CreateVarRefOp(esVar))), this.m_command.CreateNode((Op) this.m_command.CreatePropertyOp((EdmMember) end), this.m_command.CreateNode((Op) this.m_command.CreateVarRefOp(rsVar))), true);
      return this.m_command.CreateNode((Op) this.m_command.CreateInnerJoinOp(), node1, node2, node3);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node RewriteManyToOneNavigationProperty(
      RelProperty relProperty,
      System.Data.Entity.Core.Query.InternalTrees.Node sourceEntityNode,
      TypeUsage resultType)
    {
      System.Data.Entity.Core.Query.InternalTrees.Node node = this.m_command.CreateNode((Op) this.m_command.CreateRelPropertyOp(relProperty), sourceEntityNode);
      return this.m_command.CreateNode((Op) this.m_command.CreateDerefOp(resultType), node);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node RewriteOneToManyNavigationProperty(
      RelProperty relProperty,
      List<RelationshipSet> relationshipSets,
      System.Data.Entity.Core.Query.InternalTrees.Node sourceRefNode)
    {
      Var outputVar;
      return this.m_command.BuildCollect(this.RewriteFromOneNavigationProperty(relProperty, relationshipSets, sourceRefNode, out outputVar), outputVar);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node RewriteOneToOneNavigationProperty(
      RelProperty relProperty,
      List<RelationshipSet> relationshipSets,
      System.Data.Entity.Core.Query.InternalTrees.Node sourceRefNode)
    {
      Var outputVar;
      System.Data.Entity.Core.Query.InternalTrees.Node subquery = this.VisitNode(this.RewriteFromOneNavigationProperty(relProperty, relationshipSets, sourceRefNode, out outputVar));
      return this.AddSubqueryToParentRelOp(outputVar, subquery);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node RewriteFromOneNavigationProperty(
      RelProperty relProperty,
      List<RelationshipSet> relationshipSets,
      System.Data.Entity.Core.Query.InternalTrees.Node sourceRefNode,
      out Var outputVar)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(relationshipSets.Count > 0, "expected at least one relationship set here");
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(relProperty.FromEnd.RelationshipMultiplicity != RelationshipMultiplicity.Many, "Expected source end multiplicity to be one. Found 'Many' instead " + relProperty?.ToString());
      TypeUsage elementTypeUsage = TypeHelpers.GetElementTypeUsage(relProperty.ToEnd.TypeUsage);
      List<System.Data.Entity.Core.Query.InternalTrees.Node> nodeList = new List<System.Data.Entity.Core.Query.InternalTrees.Node>(relationshipSets.Count);
      List<Var> varList = new List<Var>(relationshipSets.Count);
      foreach (RelationshipSet relationshipSet in relationshipSets)
      {
        Var resultVar;
        System.Data.Entity.Core.Query.InternalTrees.Node node = this.BuildOfTypeTable(PreProcessor.FindTargetEntitySet(relationshipSet, relProperty.ToEnd), elementTypeUsage, out resultVar);
        nodeList.Add(node);
        varList.Add(resultVar);
      }
      System.Data.Entity.Core.Query.InternalTrees.Node resultNode;
      this.m_command.BuildUnionAllLadder((IList<System.Data.Entity.Core.Query.InternalTrees.Node>) nodeList, (IList<Var>) varList, out resultNode, out outputVar);
      RelProperty relProperty1 = new RelProperty(relProperty.Relationship, relProperty.ToEnd, relProperty.FromEnd);
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(this.m_command.IsRelPropertyReferenced(relProperty1), "Unreferenced rel property? " + relProperty1?.ToString());
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.m_command.CreateNode((Op) this.m_command.CreateRelPropertyOp(relProperty1), this.m_command.CreateNode((Op) this.m_command.CreateVarRefOp(outputVar)));
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.m_command.BuildComparison(OpType.EQ, sourceRefNode, node1, true);
      return this.m_command.CreateNode((Op) this.m_command.CreateFilterOp(), resultNode, node2);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node RewriteManyToManyNavigationProperty(
      RelProperty relProperty,
      List<RelationshipSet> relationshipSets,
      System.Data.Entity.Core.Query.InternalTrees.Node sourceRefNode)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(relationshipSets.Count > 0, "expected at least one relationship set here");
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(relProperty.ToEnd.RelationshipMultiplicity == RelationshipMultiplicity.Many && relProperty.FromEnd.RelationshipMultiplicity == RelationshipMultiplicity.Many, "Expected target end multiplicity to be 'many'. Found " + relProperty?.ToString() + "; multiplicity = " + relProperty.ToEnd.RelationshipMultiplicity.ToString());
      List<System.Data.Entity.Core.Query.InternalTrees.Node> nodeList = new List<System.Data.Entity.Core.Query.InternalTrees.Node>(relationshipSets.Count);
      List<Var> varList = new List<Var>(relationshipSets.Count * 2);
      foreach (RelationshipSet relationshipSet in relationshipSets)
      {
        Var rsVar;
        Var esVar;
        System.Data.Entity.Core.Query.InternalTrees.Node node = this.BuildJoinForNavProperty(relationshipSet, relProperty.ToEnd, out rsVar, out esVar);
        nodeList.Add(node);
        varList.Add(rsVar);
        varList.Add(esVar);
      }
      System.Data.Entity.Core.Query.InternalTrees.Node resultNode;
      IList<Var> resultVars;
      this.m_command.BuildUnionAllLadder((IList<System.Data.Entity.Core.Query.InternalTrees.Node>) nodeList, (IList<Var>) varList, out resultNode, out resultVars);
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.m_command.CreateNode((Op) this.m_command.CreatePropertyOp((EdmMember) relProperty.FromEnd), this.m_command.CreateNode((Op) this.m_command.CreateVarRefOp(resultVars[0])));
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.m_command.BuildComparison(OpType.EQ, sourceRefNode, node1, true);
      return this.m_command.BuildCollect(this.m_command.BuildProject(this.m_command.CreateNode((Op) this.m_command.CreateFilterOp(), resultNode, node2), (IEnumerable<Var>) new Var[1]
      {
        resultVars[1]
      }, (IEnumerable<System.Data.Entity.Core.Query.InternalTrees.Node>) new System.Data.Entity.Core.Query.InternalTrees.Node[0]), resultVars[1]);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node RewriteNavigationProperty(
      NavigationProperty navProperty,
      System.Data.Entity.Core.Query.InternalTrees.Node sourceEntityNode,
      TypeUsage resultType)
    {
      RelProperty relProperty = new RelProperty(navProperty.RelationshipType, navProperty.FromEndMember, navProperty.ToEndMember);
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(this.m_command.IsRelPropertyReferenced(relProperty) || relProperty.ToEnd.RelationshipMultiplicity == RelationshipMultiplicity.Many, "Unreferenced rel property? " + relProperty?.ToString());
      if (relProperty.FromEnd.RelationshipMultiplicity == RelationshipMultiplicity.Many && relProperty.ToEnd.RelationshipMultiplicity != RelationshipMultiplicity.Many)
        return this.RewriteManyToOneNavigationProperty(relProperty, sourceEntityNode, resultType);
      List<RelationshipSet> relationshipSets = this.GetRelationshipSets(relProperty.Relationship);
      if (relationshipSets.Count == 0)
        return relProperty.ToEnd.RelationshipMultiplicity == RelationshipMultiplicity.Many ? this.m_command.CreateNode((Op) this.m_command.CreateNewMultisetOp(resultType)) : this.m_command.CreateNode((Op) this.m_command.CreateNullOp(resultType));
      System.Data.Entity.Core.Query.InternalTrees.Node node = this.m_command.CreateNode((Op) this.m_command.CreateGetEntityRefOp(relProperty.FromEnd.TypeUsage), sourceEntityNode);
      if (relProperty.ToEnd.RelationshipMultiplicity != RelationshipMultiplicity.Many)
        return this.RewriteOneToOneNavigationProperty(relProperty, relationshipSets, node);
      return relProperty.FromEnd.RelationshipMultiplicity == RelationshipMultiplicity.Many ? this.RewriteManyToManyNavigationProperty(relProperty, relationshipSets, node) : this.RewriteOneToManyNavigationProperty(relProperty, relationshipSets, node);
    }

    protected override System.Data.Entity.Core.Query.InternalTrees.Node VisitScalarOpDefault(
      ScalarOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitChildren(n);
      this.AddTypeReference(op.Type);
      return n;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(DerefOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitScalarOpDefault((ScalarOp) op, n);
      Var outputVar;
      System.Data.Entity.Core.Query.InternalTrees.Node subquery = this.VisitNode(this.RewriteDerefOp(n, op, out outputVar));
      if (outputVar != null)
        subquery = this.AddSubqueryToParentRelOp(outputVar, subquery);
      return subquery;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(ElementOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitScalarOpDefault((ScalarOp) op, n);
      System.Data.Entity.Core.Query.InternalTrees.Node child0 = n.Child0;
      ProjectOp op1 = (ProjectOp) child0.Op;
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(op1.Outputs.Count == 1, "input to ElementOp has more than one output var?");
      return this.AddSubqueryToParentRelOp(op1.Outputs.First, child0);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(ExistsOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.m_compilerState.MarkPhaseAsNeeded(PlanCompilerPhase.Normalization);
      return base.Visit(op, n);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      FunctionOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      if (op.Function.IsFunctionImport)
      {
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(op.Function.IsComposableAttribute, "Cannot process a non-composable function inside query tree composition.");
        FunctionImportMapping targetFunctionMapping = (FunctionImportMapping) null;
        if (!this.m_command.MetadataWorkspace.TryGetFunctionImportMapping(op.Function, out targetFunctionMapping))
          throw new MetadataException(Strings.EntityClient_UnmappedFunctionImport((object) op.Function.FullName));
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(targetFunctionMapping is FunctionImportMappingComposable, "Composable function import must have corresponding mapping.");
        FunctionImportMappingComposable mappingComposable = (FunctionImportMappingComposable) targetFunctionMapping;
        this.VisitChildren(n);
        System.Data.Entity.Core.Query.InternalTrees.Node internalTree = mappingComposable.GetInternalTree(this.m_command, (IList<System.Data.Entity.Core.Query.InternalTrees.Node>) n.Children);
        if (op.Function.EntitySet != null)
        {
          this.m_entityTypeScopes.Push(op.Function.EntitySet);
          this.AddEntitySetReference(op.Function.EntitySet);
          System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(mappingComposable.TvfKeys != null && (uint) mappingComposable.TvfKeys.Length > 0U, "Function imports returning entities must have inferred keys.");
          if (!this.m_tvfResultKeys.ContainsKey(mappingComposable.TargetFunction))
            this.m_tvfResultKeys.Add(mappingComposable.TargetFunction, mappingComposable.TvfKeys);
        }
        System.Data.Entity.Core.Query.InternalTrees.Node node = this.VisitNode(internalTree);
        if (op.Function.EntitySet != null)
          System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(this.m_entityTypeScopes.Pop() == op.Function.EntitySet, "m_entityTypeScopes stack is broken");
        return node;
      }
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(op.Function.EntitySet == null, "Entity type scope is not supported on functions that aren't mapped.");
      if (TypeSemantics.IsCollectionType(op.Type) || PlanCompilerUtil.IsCollectionAggregateFunction(op, n))
      {
        this.m_compilerState.MarkPhaseAsNeeded(PlanCompilerPhase.NestPullup);
        this.m_compilerState.MarkPhaseAsNeeded(PlanCompilerPhase.Normalization);
      }
      return base.Visit(op, n);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(CaseOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitScalarOpDefault((ScalarOp) op, n);
      if (PlanCompilerUtil.IsRowTypeCaseOpWithNullability(op, n, out bool _))
        this.m_typesNeedingNullSentinel.Add(op.Type.EdmType.Identity);
      return n;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      ConditionalOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitScalarOpDefault((ScalarOp) op, n);
      this.ProcessConditionalOp(op, n);
      return n;
    }

    private void ProcessConditionalOp(ConditionalOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      if ((op.OpType != OpType.IsNull || !TypeSemantics.IsRowType(n.Child0.Op.Type)) && !TypeSemantics.IsComplexType(n.Child0.Op.Type))
        return;
      StructuredTypeNullabilityAnalyzer.MarkAsNeedingNullSentinel(this.m_typesNeedingNullSentinel, n.Child0.Op.Type);
    }

    private static void ValidateNavPropertyOp(PropertyOp op)
    {
      NavigationProperty propertyInfo = (NavigationProperty) op.PropertyInfo;
      TypeUsage typeUsage = propertyInfo.ToEndMember.TypeUsage;
      if (TypeSemantics.IsReferenceType(typeUsage))
        typeUsage = TypeHelpers.GetElementTypeUsage(typeUsage);
      if (propertyInfo.ToEndMember.RelationshipMultiplicity == RelationshipMultiplicity.Many)
        typeUsage = TypeUsage.Create((EdmType) typeUsage.EdmType.GetCollectionType());
      if (!TypeSemantics.IsStructurallyEqualOrPromotableTo(typeUsage, op.Type))
        throw new MetadataException(Strings.EntityClient_IncompatibleNavigationPropertyResult((object) propertyInfo.DeclaringType.FullName, (object) propertyInfo.Name));
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node VisitNavPropertyOp(
      PropertyOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      PreProcessor.ValidateNavPropertyOp(op);
      if (!PreProcessor.IsNavigationPropertyOverVarRef(n.Child0))
        this.VisitScalarOpDefault((ScalarOp) op, n);
      PreProcessor.NavigationPropertyOpInfo key = new PreProcessor.NavigationPropertyOpInfo(n, this.FindRelOpAncestor(), this.m_command);
      System.Data.Entity.Core.Query.InternalTrees.Node n1;
      if (this._navigationPropertyOpRewrites.TryGetValue(key, out n1))
        return OpCopier.Copy(this.m_command, n1);
      key.Seal();
      n1 = this.RewriteNavigationProperty((NavigationProperty) op.PropertyInfo, n.Child0, op.Type);
      n1 = this.VisitNode(n1);
      this._navigationPropertyOpRewrites.Add(key, n1);
      return n1;
    }

    private static bool IsNavigationPropertyOverVarRef(System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      if (n.Op.OpType != OpType.Property || !Helper.IsNavigationProperty(((PropertyOp) n.Op).PropertyInfo))
        return false;
      System.Data.Entity.Core.Query.InternalTrees.Node child0 = n.Child0;
      if (child0.Op.OpType == OpType.SoftCast)
        child0 = child0.Child0;
      return child0.Op.OpType == OpType.VarRef;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      PropertyOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      return !Helper.IsNavigationProperty(op.PropertyInfo) ? this.VisitScalarOpDefault((ScalarOp) op, n) : this.VisitNavPropertyOp(op, n);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(RefOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitScalarOpDefault((ScalarOp) op, n);
      this.AddEntitySetReference(op.EntitySet);
      return n;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(TreatOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      n = base.Visit(op, n);
      return this.CanRewriteTypeTest(op.Type.EdmType, n.Child0.Op.Type.EdmType) ? n.Child0 : n;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(IsOfOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitScalarOpDefault((ScalarOp) op, n);
      this.AddTypeReference(op.IsOfType);
      if (this.CanRewriteTypeTest(op.IsOfType.EdmType, n.Child0.Op.Type.EdmType))
        n = this.RewriteIsOfAsIsNull(op, n);
      if (op.IsOfOnly && op.IsOfType.EdmType.Abstract)
        this.m_suppressDiscriminatorMaps = true;
      return n;
    }

    private bool CanRewriteTypeTest(EdmType testType, EdmType argumentType)
    {
      if (!testType.EdmEquals((MetadataItem) argumentType) || testType.BaseType != null)
        return false;
      int num = 0;
      foreach (EdmType edmType in MetadataHelper.GetTypeAndSubtypesOf(testType, this.m_command.MetadataWorkspace, true))
      {
        ++num;
        if (2 == num)
          break;
      }
      return 1 == num;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node RewriteIsOfAsIsNull(
      IsOfOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      ConditionalOp conditionalOp = this.m_command.CreateConditionalOp(OpType.IsNull);
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.m_command.CreateNode((Op) conditionalOp, n.Child0);
      this.ProcessConditionalOp(conditionalOp, node1);
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.m_command.CreateNode((Op) this.m_command.CreateConditionalOp(OpType.Not), node1);
      System.Data.Entity.Core.Query.InternalTrees.Node node3 = this.m_command.CreateNode((Op) this.m_command.CreateConstantOp(op.Type, (object) true));
      System.Data.Entity.Core.Query.InternalTrees.Node node4 = this.m_command.CreateNode((Op) this.m_command.CreateNullOp(op.Type));
      System.Data.Entity.Core.Query.InternalTrees.Node node5 = this.m_command.CreateNode((Op) this.m_command.CreateCaseOp(op.Type), node2, node3, node4);
      return this.m_command.CreateNode((Op) this.m_command.CreateComparisonOp(OpType.EQ), node5, node3);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      NavigateOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.VisitScalarOpDefault((ScalarOp) op, n);
      Var outputVar;
      System.Data.Entity.Core.Query.InternalTrees.Node subquery = this.VisitNode(this.RewriteNavigateOp(n, op, out outputVar));
      if (outputVar != null)
        subquery = this.AddSubqueryToParentRelOp(outputVar, subquery);
      return subquery;
    }

    private EntitySet GetCurrentEntityTypeScope() => this.m_entityTypeScopes.Count == 0 ? (EntitySet) null : this.m_entityTypeScopes.Peek();

    private static RelationshipSet FindRelationshipSet(
      EntitySetBase entitySet,
      RelProperty relProperty)
    {
      foreach (EntitySetBase baseEntitySet in entitySet.EntityContainer.BaseEntitySets)
      {
        if (baseEntitySet is AssociationSet associationSet1 && associationSet1.ElementType.EdmEquals((MetadataItem) relProperty.Relationship) && associationSet1.AssociationSetEnds[relProperty.FromEnd.Identity].EntitySet.EdmEquals((MetadataItem) entitySet))
          return (RelationshipSet) associationSet1;
      }
      return (RelationshipSet) null;
    }

    private static int FindPosition(EdmType type, EdmMember member)
    {
      int num = 0;
      foreach (MetadataItem structuralMember in (IEnumerable) TypeHelpers.GetAllStructuralMembers(type))
      {
        if (structuralMember.EdmEquals((MetadataItem) member))
          return num;
        ++num;
      }
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(false, "Could not find property " + member?.ToString() + " in type " + type.Name);
      return -1;
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node BuildKeyExpressionForNewEntityOp(
      Op op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(op.OpType == OpType.NewEntity || op.OpType == OpType.DiscriminatedNewEntity, "BuildKeyExpression: Unexpected OpType:" + op.OpType.ToString());
      int num = op.OpType == OpType.DiscriminatedNewEntity ? 1 : 0;
      EntityTypeBase edmType = (EntityTypeBase) op.Type.EdmType;
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      List<KeyValuePair<string, TypeUsage>> keyValuePairList = new List<KeyValuePair<string, TypeUsage>>();
      foreach (EdmMember keyMember in edmType.KeyMembers)
      {
        int index = PreProcessor.FindPosition((EdmType) edmType, keyMember) + num;
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(n.Children.Count > index, "invalid position " + index.ToString() + "; total count = " + n.Children.Count.ToString());
        args.Add(n.Children[index]);
        keyValuePairList.Add(new KeyValuePair<string, TypeUsage>(keyMember.Name, keyMember.TypeUsage));
      }
      return this.m_command.CreateNode((Op) this.m_command.CreateNewRecordOp(TypeHelpers.CreateRowTypeUsage((IEnumerable<KeyValuePair<string, TypeUsage>>) keyValuePairList)), args);
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node BuildRelPropertyExpression(
      EntitySetBase entitySet,
      RelProperty relProperty,
      System.Data.Entity.Core.Query.InternalTrees.Node keyExpr)
    {
      keyExpr = OpCopier.Copy(this.m_command, keyExpr);
      RelationshipSet relationshipSet = PreProcessor.FindRelationshipSet(entitySet, relProperty);
      if (relationshipSet == null)
        return this.m_command.CreateNode((Op) this.m_command.CreateNullOp(relProperty.ToEnd.TypeUsage));
      ScanTableOp scanTableOp = this.m_command.CreateScanTableOp(Command.CreateTableDefinition((EntitySetBase) relationshipSet));
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(scanTableOp.Table.Columns.Count == 1, "Unexpected column count for table:" + scanTableOp.Table.TableMetadata.Extent?.ToString() + "=" + scanTableOp.Table.Columns.Count.ToString());
      Var column = scanTableOp.Table.Columns[0];
      System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.m_command.CreateNode((Op) scanTableOp);
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.m_command.CreateNode((Op) this.m_command.CreatePropertyOp((EdmMember) relProperty.FromEnd), this.m_command.CreateNode((Op) this.m_command.CreateVarRefOp(column)));
      System.Data.Entity.Core.Query.InternalTrees.Node node3 = this.m_command.BuildComparison(OpType.EQ, keyExpr, this.m_command.CreateNode((Op) this.m_command.CreateGetRefKeyOp(keyExpr.Op.Type), node2), true);
      System.Data.Entity.Core.Query.InternalTrees.Node subquery = this.VisitNode(this.m_command.CreateNode((Op) this.m_command.CreateFilterOp(), node1, node3));
      System.Data.Entity.Core.Query.InternalTrees.Node parentRelOp = this.AddSubqueryToParentRelOp(column, subquery);
      return this.m_command.CreateNode((Op) this.m_command.CreatePropertyOp((EdmMember) relProperty.ToEnd), parentRelOp);
    }

    private IEnumerable<System.Data.Entity.Core.Query.InternalTrees.Node> BuildAllRelPropertyExpressions(
      EntitySetBase entitySet,
      List<RelProperty> relPropertyList,
      Dictionary<RelProperty, System.Data.Entity.Core.Query.InternalTrees.Node> prebuiltExpressions,
      System.Data.Entity.Core.Query.InternalTrees.Node keyExpr)
    {
      foreach (RelProperty relProperty in relPropertyList)
      {
        System.Data.Entity.Core.Query.InternalTrees.Node node;
        if (!prebuiltExpressions.TryGetValue(relProperty, out node))
          node = this.BuildRelPropertyExpression(entitySet, relProperty, keyExpr);
        yield return node;
      }
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      NewEntityOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      if (op.Scoped || op.Type.EdmType.BuiltInTypeKind != BuiltInTypeKind.EntityType)
        return base.Visit(op, n);
      EntityType edmType = (EntityType) op.Type.EdmType;
      EntitySet currentEntityTypeScope = this.GetCurrentEntityTypeScope();
      List<RelProperty> relPropertyList1;
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args;
      if (currentEntityTypeScope == null)
      {
        this.m_freeFloatingEntityConstructorTypes.Add(edmType);
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(op.RelationshipProperties == null || op.RelationshipProperties.Count == 0, "Related Entities cannot be specified for Entity constructors that are not part of the Query Mapping View for an Entity Set.");
        this.VisitScalarOpDefault((ScalarOp) op, n);
        relPropertyList1 = op.RelationshipProperties;
        args = n.Children;
      }
      else
      {
        relPropertyList1 = new List<RelProperty>(this.m_relPropertyHelper.GetRelProperties((EntityTypeBase) edmType));
        int index1 = op.RelationshipProperties.Count - 1;
        List<RelProperty> relPropertyList2 = new List<RelProperty>((IEnumerable<RelProperty>) op.RelationshipProperties);
        int index2 = n.Children.Count - 1;
        while (index2 >= edmType.Properties.Count)
        {
          if (!relPropertyList1.Contains(op.RelationshipProperties[index1]))
          {
            n.Children.RemoveAt(index2);
            relPropertyList2.RemoveAt(index1);
          }
          --index2;
          --index1;
        }
        this.VisitScalarOpDefault((ScalarOp) op, n);
        System.Data.Entity.Core.Query.InternalTrees.Node keyExpr = this.BuildKeyExpressionForNewEntityOp((Op) op, n);
        Dictionary<RelProperty, System.Data.Entity.Core.Query.InternalTrees.Node> prebuiltExpressions = new Dictionary<RelProperty, System.Data.Entity.Core.Query.InternalTrees.Node>();
        int index3 = 0;
        int count = edmType.Properties.Count;
        while (count < n.Children.Count)
        {
          prebuiltExpressions[relPropertyList2[index3]] = n.Children[count];
          ++count;
          ++index3;
        }
        args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
        for (int index4 = 0; index4 < edmType.Properties.Count; ++index4)
          args.Add(n.Children[index4]);
        foreach (System.Data.Entity.Core.Query.InternalTrees.Node propertyExpression in this.BuildAllRelPropertyExpressions((EntitySetBase) currentEntityTypeScope, relPropertyList1, prebuiltExpressions, keyExpr))
          args.Add(propertyExpression);
      }
      return this.m_command.CreateNode((Op) this.m_command.CreateScopedNewEntityOp(op.Type, relPropertyList1, currentEntityTypeScope), args);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      DiscriminatedNewEntityOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      HashSet<RelProperty> relPropertySet = new HashSet<RelProperty>();
      List<RelProperty> relPropertyList1 = new List<RelProperty>();
      foreach (KeyValuePair<object, EntityType> type in op.DiscriminatorMap.TypeMap)
      {
        EntityTypeBase entityType = (EntityTypeBase) type.Value;
        this.AddTypeReference(TypeUsage.Create((EdmType) entityType));
        foreach (RelProperty relProperty in this.m_relPropertyHelper.GetRelProperties(entityType))
          relPropertySet.Add(relProperty);
      }
      List<RelProperty> relPropertyList2 = new List<RelProperty>((IEnumerable<RelProperty>) relPropertySet);
      this.VisitScalarOpDefault((ScalarOp) op, n);
      System.Data.Entity.Core.Query.InternalTrees.Node keyExpr = this.BuildKeyExpressionForNewEntityOp((Op) op, n);
      List<System.Data.Entity.Core.Query.InternalTrees.Node> args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
      int num = n.Children.Count - op.RelationshipProperties.Count;
      for (int index = 0; index < num; ++index)
        args.Add(n.Children[index]);
      Dictionary<RelProperty, System.Data.Entity.Core.Query.InternalTrees.Node> prebuiltExpressions = new Dictionary<RelProperty, System.Data.Entity.Core.Query.InternalTrees.Node>();
      int index1 = num;
      int index2 = 0;
      while (index1 < n.Children.Count)
      {
        prebuiltExpressions[op.RelationshipProperties[index2]] = n.Children[index1];
        ++index1;
        ++index2;
      }
      foreach (System.Data.Entity.Core.Query.InternalTrees.Node propertyExpression in this.BuildAllRelPropertyExpressions((EntitySetBase) op.EntitySet, relPropertyList2, prebuiltExpressions, keyExpr))
        args.Add(propertyExpression);
      return this.m_command.CreateNode((Op) this.m_command.CreateDiscriminatedNewEntityOp(op.Type, op.DiscriminatorMap, op.EntitySet, relPropertyList2), args);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      NewMultisetOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      System.Data.Entity.Core.Query.InternalTrees.Node resultNode = (System.Data.Entity.Core.Query.InternalTrees.Node) null;
      Var var = (Var) null;
      CollectionType edmType = TypeHelpers.GetEdmType<CollectionType>(op.Type);
      if (!n.HasChild0)
      {
        System.Data.Entity.Core.Query.InternalTrees.Node node = this.m_command.CreateNode((Op) this.m_command.CreateSingleRowTableOp());
        Var projectVar;
        resultNode = this.m_command.BuildProject(this.m_command.CreateNode((Op) this.m_command.CreateFilterOp(), node, this.m_command.CreateNode((Op) this.m_command.CreateFalseOp())), this.m_command.CreateNode((Op) this.m_command.CreateNullOp(edmType.TypeUsage)), out projectVar);
        var = projectVar;
      }
      else if (n.Children.Count == 1 || PreProcessor.AreAllConstantsOrNulls(n.Children))
      {
        List<System.Data.Entity.Core.Query.InternalTrees.Node> nodeList = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
        List<Var> varList = new List<Var>();
        foreach (System.Data.Entity.Core.Query.InternalTrees.Node child in n.Children)
        {
          Var projectVar;
          System.Data.Entity.Core.Query.InternalTrees.Node node = this.m_command.BuildProject(this.m_command.CreateNode((Op) this.m_command.CreateSingleRowTableOp()), child, out projectVar);
          nodeList.Add(node);
          varList.Add(projectVar);
        }
        this.m_command.BuildUnionAllLadder((IList<System.Data.Entity.Core.Query.InternalTrees.Node>) nodeList, (IList<Var>) varList, out resultNode, out var);
      }
      else
      {
        List<System.Data.Entity.Core.Query.InternalTrees.Node> nodeList = new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
        List<Var> varList = new List<Var>();
        for (int index = 0; index < n.Children.Count; ++index)
        {
          Var projectVar;
          System.Data.Entity.Core.Query.InternalTrees.Node node = this.m_command.BuildProject(this.m_command.CreateNode((Op) this.m_command.CreateSingleRowTableOp()), this.m_command.CreateNode((Op) this.m_command.CreateInternalConstantOp(this.m_command.IntegerType, (object) index)), out projectVar);
          nodeList.Add(node);
          varList.Add(projectVar);
        }
        this.m_command.BuildUnionAllLadder((IList<System.Data.Entity.Core.Query.InternalTrees.Node>) nodeList, (IList<Var>) varList, out resultNode, out var);
        List<System.Data.Entity.Core.Query.InternalTrees.Node> args = new List<System.Data.Entity.Core.Query.InternalTrees.Node>(n.Children.Count * 2 + 1);
        for (int index = 0; index < n.Children.Count; ++index)
        {
          if (index != n.Children.Count - 1)
          {
            System.Data.Entity.Core.Query.InternalTrees.Node node = this.m_command.CreateNode((Op) this.m_command.CreateComparisonOp(OpType.EQ), this.m_command.CreateNode((Op) this.m_command.CreateVarRefOp(var)), this.m_command.CreateNode((Op) this.m_command.CreateConstantOp(this.m_command.IntegerType, (object) index)));
            args.Add(node);
          }
          args.Add(n.Children[index]);
        }
        System.Data.Entity.Core.Query.InternalTrees.Node node1 = this.m_command.CreateNode((Op) this.m_command.CreateCaseOp(edmType.TypeUsage), args);
        resultNode = this.m_command.BuildProject(resultNode, node1, out var);
      }
      System.Data.Entity.Core.Query.InternalTrees.Node node2 = this.m_command.CreateNode((Op) this.m_command.CreatePhysicalProjectOp(var), resultNode);
      return this.VisitNode(this.m_command.CreateNode((Op) this.m_command.CreateCollectOp(op.Type), node2));
    }

    private static bool AreAllConstantsOrNulls(List<System.Data.Entity.Core.Query.InternalTrees.Node> nodes)
    {
      foreach (System.Data.Entity.Core.Query.InternalTrees.Node node in nodes)
      {
        if (node.Op.OpType != OpType.Constant && node.Op.OpType != OpType.Null)
          return false;
      }
      return true;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(CollectOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.m_compilerState.MarkPhaseAsNeeded(PlanCompilerPhase.NestPullup);
      return this.VisitScalarOpDefault((ScalarOp) op, n);
    }

    private void HandleTableOpMetadata(ScanTableBaseOp op)
    {
      if (op.Table.TableMetadata.Extent is EntitySet extent)
        this.AddEntitySetReference(extent);
      this.AddTypeReference(TypeUsage.Create((EdmType) op.Table.TableMetadata.Extent.ElementType));
    }

    private System.Data.Entity.Core.Query.InternalTrees.Node ProcessScanTable(
      System.Data.Entity.Core.Query.InternalTrees.Node scanTableNode,
      ScanTableOp scanTableOp,
      ref IsOfOp typeFilter)
    {
      this.HandleTableOpMetadata((ScanTableBaseOp) scanTableOp);
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(scanTableOp.Table.TableMetadata.Extent != null, "ScanTableOp must reference a table with an extent");
      return scanTableOp.Table.TableMetadata.Extent.EntityContainer.DataSpace == DataSpace.SSpace ? scanTableNode : this.VisitNode(this.ExpandView(scanTableOp, ref typeFilter));
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      ScanTableOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      IsOfOp typeFilter = (IsOfOp) null;
      return this.ProcessScanTable(n, op, ref typeFilter);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      ScanViewOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      bool flag = false;
      if (op.Table.TableMetadata.Extent.BuiltInTypeKind == BuiltInTypeKind.EntitySet)
      {
        this.m_entityTypeScopes.Push((EntitySet) op.Table.TableMetadata.Extent);
        flag = true;
      }
      this.HandleTableOpMetadata((ScanTableBaseOp) op);
      this.VisitRelOpDefault((RelOp) op, n);
      if (flag)
        System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(this.m_entityTypeScopes.Pop() == op.Table.TableMetadata.Extent, "m_entityTypeScopes stack is broken");
      return n;
    }

    protected override System.Data.Entity.Core.Query.InternalTrees.Node VisitJoinOp(
      JoinBaseOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      if (op.OpType == OpType.InnerJoin || op.OpType == OpType.LeftOuterJoin)
        this.m_compilerState.MarkPhaseAsNeeded(PlanCompilerPhase.JoinElimination);
      if (this.ProcessJoinOp(n))
        this.m_compilerState.MarkPhaseAsNeeded(PlanCompilerPhase.Normalization);
      return n;
    }

    protected override System.Data.Entity.Core.Query.InternalTrees.Node VisitApplyOp(
      ApplyBaseOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.m_compilerState.MarkPhaseAsNeeded(PlanCompilerPhase.JoinElimination);
      return this.VisitRelOpDefault((RelOp) op, n);
    }

    private bool IsSortUnnecessary()
    {
      System.Data.Entity.Core.Query.InternalTrees.Node node = this.m_ancestors.Peek();
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(node != null, "unexpected SortOp as root node?");
      return node.Op.OpType != OpType.PhysicalProject;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(SortOp op, System.Data.Entity.Core.Query.InternalTrees.Node n) => this.IsSortUnnecessary() ? this.VisitNode(n.Child0) : this.VisitRelOpDefault((RelOp) op, n);

    private static bool IsOfTypeOverScanTable(System.Data.Entity.Core.Query.InternalTrees.Node n, out IsOfOp typeFilter)
    {
      typeFilter = (IsOfOp) null;
      if (!(n.Child1.Op is IsOfOp op1) || !(n.Child0.Op is ScanTableOp op2) || (op2.Table.Columns.Count != 1 || !(n.Child1.Child0.Op is VarRefOp op3)) || op3.Var != op2.Table.Columns[0])
        return false;
      typeFilter = op1;
      return true;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(FilterOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      IsOfOp typeFilter;
      if (!PreProcessor.IsOfTypeOverScanTable(n, out typeFilter))
        return this.VisitRelOpDefault((RelOp) op, n);
      System.Data.Entity.Core.Query.InternalTrees.Node node = this.ProcessScanTable(n.Child0, (ScanTableOp) n.Child0.Op, ref typeFilter);
      if (typeFilter != null)
      {
        n.Child1 = this.VisitNode(n.Child1);
        n.Child0 = node;
        node = n;
      }
      return node;
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(ProjectOp op, System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      System.Data.Entity.Core.Query.PlanCompiler.PlanCompiler.Assert(n.HasChild0, "projectOp without input?");
      if (OpType.Sort == n.Child0.Op.OpType || OpType.ConstrainedSort == n.Child0.Op.OpType)
      {
        SortBaseOp op1 = (SortBaseOp) n.Child0.Op;
        if (op1.Keys.Count > 0)
        {
          IList<System.Data.Entity.Core.Query.InternalTrees.Node> args = (IList<System.Data.Entity.Core.Query.InternalTrees.Node>) new List<System.Data.Entity.Core.Query.InternalTrees.Node>();
          args.Add(n);
          for (int index = 1; index < n.Child0.Children.Count; ++index)
            args.Add(n.Child0.Children[index]);
          n.Child0 = n.Child0.Child0;
          foreach (SortKey key in op1.Keys)
            op.Outputs.Set(key.Var);
          return this.VisitNode(this.m_command.CreateNode((Op) op1, args));
        }
      }
      return this.VisitRelOpDefault((RelOp) op, n);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      GroupByIntoOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      this.m_compilerState.MarkPhaseAsNeeded(PlanCompilerPhase.AggregatePushdown);
      return base.Visit(op, n);
    }

    public override System.Data.Entity.Core.Query.InternalTrees.Node Visit(
      ComparisonOp op,
      System.Data.Entity.Core.Query.InternalTrees.Node n)
    {
      if (op.OpType == OpType.EQ || op.OpType == OpType.NE)
        this.m_compilerState.MarkPhaseAsNeeded(PlanCompilerPhase.NullSemantics);
      return base.Visit(op, n);
    }

    private class NavigationPropertyOpInfo
    {
      private System.Data.Entity.Core.Query.InternalTrees.Node _node;
      private readonly System.Data.Entity.Core.Query.InternalTrees.Node _root;
      private readonly Command _command;
      private readonly int _hashCode;

      public NavigationPropertyOpInfo(System.Data.Entity.Core.Query.InternalTrees.Node node, System.Data.Entity.Core.Query.InternalTrees.Node root, Command command)
      {
        this._node = node;
        this._root = root;
        this._command = command;
        this._hashCode = ((this._root != null ? RuntimeHelpers.GetHashCode((object) this._root) : 0) * 397 ^ RuntimeHelpers.GetHashCode((object) PreProcessor.NavigationPropertyOpInfo.GetProperty(this._node))) * 397 ^ this._node.GetNodeInfo(this._command).HashValue;
      }

      public override int GetHashCode() => this._hashCode;

      public override bool Equals(object obj) => obj is PreProcessor.NavigationPropertyOpInfo navigationPropertyOpInfo && this._root != null && (this._root == navigationPropertyOpInfo._root && PreProcessor.NavigationPropertyOpInfo.GetProperty(this._node) == PreProcessor.NavigationPropertyOpInfo.GetProperty(navigationPropertyOpInfo._node)) && this._node.IsEquivalent(navigationPropertyOpInfo._node);

      public void Seal() => this._node = OpCopier.Copy(this._command, this._node);

      private static EdmMember GetProperty(System.Data.Entity.Core.Query.InternalTrees.Node node) => ((PropertyOp) node.Op).PropertyInfo;
    }
  }
}

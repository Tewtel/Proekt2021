﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.Command
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Query.PlanCompiler;
using System.Linq;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class Command
  {
    private readonly Dictionary<string, ParameterVar> m_parameterMap;
    private readonly List<Var> m_vars;
    private readonly List<Table> m_tables;
    private readonly MetadataWorkspace m_metadataWorkspace;
    private readonly TypeUsage m_boolType;
    private readonly TypeUsage m_intType;
    private readonly TypeUsage m_stringType;
    private readonly ConstantPredicateOp m_trueOp;
    private readonly ConstantPredicateOp m_falseOp;
    private readonly NodeInfoVisitor m_nodeInfoVisitor;
    private readonly KeyPullup m_keyPullupVisitor;
    private int m_nextNodeId;
    private int m_nextBranchDiscriminatorValue = 1000;
    private bool m_disableVarVecEnumCaching;
    private readonly Stack<VarVec.VarVecEnumerator> m_freeVarVecEnumerators;
    private readonly Stack<VarVec> m_freeVarVecs;
    private readonly HashSet<RelProperty> m_referencedRelProperties;

    internal Command(MetadataWorkspace metadataWorkspace)
    {
      this.m_parameterMap = new Dictionary<string, ParameterVar>();
      this.m_vars = new List<Var>();
      this.m_tables = new List<Table>();
      this.m_metadataWorkspace = metadataWorkspace;
      if (!Command.TryGetPrimitiveType(PrimitiveTypeKind.Boolean, out this.m_boolType))
        throw new ProviderIncompatibleException(System.Data.Entity.Resources.Strings.Cqt_General_NoProviderBooleanType);
      if (!Command.TryGetPrimitiveType(PrimitiveTypeKind.Int32, out this.m_intType))
        throw new ProviderIncompatibleException(System.Data.Entity.Resources.Strings.Cqt_General_NoProviderIntegerType);
      if (!Command.TryGetPrimitiveType(PrimitiveTypeKind.String, out this.m_stringType))
        throw new ProviderIncompatibleException(System.Data.Entity.Resources.Strings.Cqt_General_NoProviderStringType);
      this.m_trueOp = new ConstantPredicateOp(this.m_boolType, true);
      this.m_falseOp = new ConstantPredicateOp(this.m_boolType, false);
      this.m_nodeInfoVisitor = new NodeInfoVisitor(this);
      this.m_keyPullupVisitor = new KeyPullup(this);
      this.m_freeVarVecEnumerators = new Stack<VarVec.VarVecEnumerator>();
      this.m_freeVarVecs = new Stack<VarVec>();
      this.m_referencedRelProperties = new HashSet<RelProperty>();
    }

    internal Command()
    {
    }

    internal virtual MetadataWorkspace MetadataWorkspace => this.m_metadataWorkspace;

    internal virtual Node Root { get; set; }

    internal virtual void DisableVarVecEnumCaching() => this.m_disableVarVecEnumCaching = true;

    internal virtual int NextBranchDiscriminatorValue => this.m_nextBranchDiscriminatorValue++;

    internal virtual int NextNodeId => this.m_nextNodeId;

    internal virtual TypeUsage BooleanType => this.m_boolType;

    internal virtual TypeUsage IntegerType => this.m_intType;

    internal virtual TypeUsage StringType => this.m_stringType;

    private static bool TryGetPrimitiveType(PrimitiveTypeKind modelType, out TypeUsage type)
    {
      type = (TypeUsage) null;
      type = modelType != PrimitiveTypeKind.String ? MetadataWorkspace.GetCanonicalModelTypeUsage(modelType) : TypeUsage.CreateStringTypeUsage(MetadataWorkspace.GetModelPrimitiveType(modelType), false, false);
      return type != null;
    }

    internal virtual VarVec CreateVarVec()
    {
      VarVec varVec;
      if (this.m_freeVarVecs.Count == 0)
      {
        varVec = new VarVec(this);
      }
      else
      {
        varVec = this.m_freeVarVecs.Pop();
        varVec.Clear();
      }
      return varVec;
    }

    internal virtual VarVec CreateVarVec(Var v)
    {
      VarVec varVec = this.CreateVarVec();
      varVec.Set(v);
      return varVec;
    }

    internal virtual VarVec CreateVarVec(IEnumerable<Var> v)
    {
      VarVec varVec = this.CreateVarVec();
      varVec.InitFrom(v);
      return varVec;
    }

    internal virtual VarVec CreateVarVec(VarVec v)
    {
      VarVec varVec = this.CreateVarVec();
      varVec.InitFrom(v);
      return varVec;
    }

    internal virtual void ReleaseVarVec(VarVec vec) => this.m_freeVarVecs.Push(vec);

    internal virtual VarVec.VarVecEnumerator GetVarVecEnumerator(VarVec vec)
    {
      VarVec.VarVecEnumerator varVecEnumerator;
      if (this.m_disableVarVecEnumCaching || this.m_freeVarVecEnumerators.Count == 0)
      {
        varVecEnumerator = new VarVec.VarVecEnumerator(vec);
      }
      else
      {
        varVecEnumerator = this.m_freeVarVecEnumerators.Pop();
        varVecEnumerator.Init(vec);
      }
      return varVecEnumerator;
    }

    internal virtual void ReleaseVarVecEnumerator(VarVec.VarVecEnumerator enumerator)
    {
      if (this.m_disableVarVecEnumCaching)
        return;
      this.m_freeVarVecEnumerators.Push(enumerator);
    }

    internal static VarList CreateVarList() => new VarList();

    internal static VarList CreateVarList(IEnumerable<Var> vars) => new VarList(vars);

    private int NewTableId() => this.m_tables.Count;

    internal static TableMD CreateTableDefinition(TypeUsage elementType) => new TableMD(elementType, (EntitySetBase) null);

    internal static TableMD CreateTableDefinition(EntitySetBase extent) => new TableMD(TypeUsage.Create((EdmType) extent.ElementType), extent);

    internal virtual TableMD CreateFlatTableDefinition(RowType type) => this.CreateFlatTableDefinition((IEnumerable<EdmProperty>) type.Properties, (IEnumerable<EdmMember>) new List<EdmMember>(), (EntitySetBase) null);

    internal virtual TableMD CreateFlatTableDefinition(
      IEnumerable<EdmProperty> properties,
      IEnumerable<EdmMember> keyMembers,
      EntitySetBase entitySet)
    {
      return new TableMD(properties, keyMembers, entitySet);
    }

    internal virtual Table CreateTableInstance(TableMD tableMetadata)
    {
      Table table = new Table(this, tableMetadata, this.NewTableId());
      this.m_tables.Add(table);
      return table;
    }

    internal virtual IEnumerable<Var> Vars => this.m_vars.Where<Var>((Func<Var, bool>) (v => v.VarType != VarType.NotValid));

    internal virtual Var GetVar(int id) => this.m_vars[id];

    internal virtual ParameterVar GetParameter(string paramName) => this.m_parameterMap[paramName];

    private int NewVarId() => this.m_vars.Count;

    internal virtual ParameterVar CreateParameterVar(
      string parameterName,
      TypeUsage parameterType)
    {
      ParameterVar parameterVar = !this.m_parameterMap.ContainsKey(parameterName) ? new ParameterVar(this.NewVarId(), parameterType, parameterName) : throw new ArgumentException(System.Data.Entity.Resources.Strings.DuplicateParameterName((object) parameterName));
      this.m_vars.Add((Var) parameterVar);
      this.m_parameterMap[parameterName] = parameterVar;
      return parameterVar;
    }

    private ParameterVar ReplaceParameterVar(
      ParameterVar oldVar,
      Func<TypeUsage, TypeUsage> generateReplacementType)
    {
      ParameterVar parameterVar = new ParameterVar(this.NewVarId(), generateReplacementType(oldVar.Type), oldVar.ParameterName);
      this.m_parameterMap[oldVar.ParameterName] = parameterVar;
      this.m_vars.Add((Var) parameterVar);
      return parameterVar;
    }

    internal virtual ParameterVar ReplaceEnumParameterVar(ParameterVar oldVar) => this.ReplaceParameterVar(oldVar, (Func<TypeUsage, TypeUsage>) (t => TypeHelpers.CreateEnumUnderlyingTypeUsage(t)));

    internal virtual ParameterVar ReplaceStrongSpatialParameterVar(ParameterVar oldVar) => this.ReplaceParameterVar(oldVar, (Func<TypeUsage, TypeUsage>) (t => TypeHelpers.CreateSpatialUnionTypeUsage(t)));

    internal virtual ColumnVar CreateColumnVar(Table table, ColumnMD columnMD)
    {
      ColumnVar columnVar = new ColumnVar(this.NewVarId(), table, columnMD);
      table.Columns.Add((Var) columnVar);
      this.m_vars.Add((Var) columnVar);
      return columnVar;
    }

    internal virtual ComputedVar CreateComputedVar(TypeUsage type)
    {
      ComputedVar computedVar = new ComputedVar(this.NewVarId(), type);
      this.m_vars.Add((Var) computedVar);
      return computedVar;
    }

    internal virtual SetOpVar CreateSetOpVar(TypeUsage type)
    {
      SetOpVar setOpVar = new SetOpVar(this.NewVarId(), type);
      this.m_vars.Add((Var) setOpVar);
      return setOpVar;
    }

    internal virtual Node CreateNode(Op op) => this.CreateNode(op, new List<Node>());

    internal virtual Node CreateNode(Op op, Node arg1) => this.CreateNode(op, new List<Node>()
    {
      arg1
    });

    internal virtual Node CreateNode(Op op, Node arg1, Node arg2) => this.CreateNode(op, new List<Node>()
    {
      arg1,
      arg2
    });

    internal virtual Node CreateNode(Op op, Node arg1, Node arg2, Node arg3) => this.CreateNode(op, new List<Node>()
    {
      arg1,
      arg2,
      arg3
    });

    internal virtual Node CreateNode(Op op, IList<Node> args) => new Node(this.m_nextNodeId++, op, new List<Node>((IEnumerable<Node>) args));

    internal virtual Node CreateNode(Op op, List<Node> args) => new Node(this.m_nextNodeId++, op, args);

    internal virtual ConstantBaseOp CreateConstantOp(TypeUsage type, object value)
    {
      if (value == null)
        return (ConstantBaseOp) new NullOp(type);
      return TypeSemantics.IsBooleanType(type) ? (ConstantBaseOp) new InternalConstantOp(type, value) : (ConstantBaseOp) new ConstantOp(type, value);
    }

    internal virtual InternalConstantOp CreateInternalConstantOp(
      TypeUsage type,
      object value)
    {
      return new InternalConstantOp(type, value);
    }

    internal virtual NullSentinelOp CreateNullSentinelOp() => new NullSentinelOp(this.IntegerType, (object) 1);

    internal virtual NullOp CreateNullOp(TypeUsage type) => new NullOp(type);

    internal virtual ConstantPredicateOp CreateConstantPredicateOp(bool value) => !value ? this.m_falseOp : this.m_trueOp;

    internal virtual ConstantPredicateOp CreateTrueOp() => this.m_trueOp;

    internal virtual ConstantPredicateOp CreateFalseOp() => this.m_falseOp;

    internal virtual FunctionOp CreateFunctionOp(EdmFunction function) => new FunctionOp(function);

    internal virtual TreatOp CreateTreatOp(TypeUsage type) => new TreatOp(type, false);

    internal virtual TreatOp CreateFakeTreatOp(TypeUsage type) => new TreatOp(type, true);

    internal virtual IsOfOp CreateIsOfOp(TypeUsage isOfType) => new IsOfOp(isOfType, false, this.m_boolType);

    internal virtual IsOfOp CreateIsOfOnlyOp(TypeUsage isOfType) => new IsOfOp(isOfType, true, this.m_boolType);

    internal virtual CastOp CreateCastOp(TypeUsage type) => new CastOp(type);

    internal virtual SoftCastOp CreateSoftCastOp(TypeUsage type) => new SoftCastOp(type);

    internal virtual ComparisonOp CreateComparisonOp(
      OpType opType,
      bool useDatabaseNullSemantics = false)
    {
      return new ComparisonOp(opType, this.BooleanType)
      {
        UseDatabaseNullSemantics = useDatabaseNullSemantics
      };
    }

    internal virtual LikeOp CreateLikeOp() => new LikeOp(this.BooleanType);

    internal virtual ConditionalOp CreateConditionalOp(OpType opType) => new ConditionalOp(opType, this.BooleanType);

    internal virtual CaseOp CreateCaseOp(TypeUsage type) => new CaseOp(type);

    internal virtual AggregateOp CreateAggregateOp(EdmFunction aggFunc, bool distinctAgg) => new AggregateOp(aggFunc, distinctAgg);

    internal virtual NewInstanceOp CreateNewInstanceOp(TypeUsage type) => new NewInstanceOp(type);

    internal virtual NewEntityOp CreateScopedNewEntityOp(
      TypeUsage type,
      List<RelProperty> relProperties,
      EntitySet entitySet)
    {
      return new NewEntityOp(type, relProperties, true, entitySet);
    }

    internal virtual NewEntityOp CreateNewEntityOp(
      TypeUsage type,
      List<RelProperty> relProperties)
    {
      return new NewEntityOp(type, relProperties, false, (EntitySet) null);
    }

    internal virtual DiscriminatedNewEntityOp CreateDiscriminatedNewEntityOp(
      TypeUsage type,
      ExplicitDiscriminatorMap discriminatorMap,
      EntitySet entitySet,
      List<RelProperty> relProperties)
    {
      return new DiscriminatedNewEntityOp(type, discriminatorMap, entitySet, relProperties);
    }

    internal virtual NewMultisetOp CreateNewMultisetOp(TypeUsage type) => new NewMultisetOp(type);

    internal virtual NewRecordOp CreateNewRecordOp(TypeUsage type) => new NewRecordOp(type);

    internal virtual NewRecordOp CreateNewRecordOp(RowType type) => new NewRecordOp(TypeUsage.Create((EdmType) type));

    internal virtual NewRecordOp CreateNewRecordOp(
      TypeUsage type,
      List<EdmProperty> fields)
    {
      return new NewRecordOp(type, fields);
    }

    internal virtual VarRefOp CreateVarRefOp(Var v) => new VarRefOp(v);

    internal virtual ArithmeticOp CreateArithmeticOp(OpType opType, TypeUsage type) => new ArithmeticOp(opType, type);

    internal PropertyOp CreatePropertyOp(EdmMember prop)
    {
      if (prop is NavigationProperty navigationProperty)
      {
        this.AddRelPropertyReference(new RelProperty(navigationProperty.RelationshipType, navigationProperty.FromEndMember, navigationProperty.ToEndMember));
        this.AddRelPropertyReference(new RelProperty(navigationProperty.RelationshipType, navigationProperty.ToEndMember, navigationProperty.FromEndMember));
      }
      return new PropertyOp(Helper.GetModelTypeUsage(prop), prop);
    }

    internal RelPropertyOp CreateRelPropertyOp(RelProperty prop)
    {
      this.AddRelPropertyReference(prop);
      return new RelPropertyOp(prop.ToEnd.TypeUsage, prop);
    }

    internal virtual RefOp CreateRefOp(EntitySet entitySet, TypeUsage type) => new RefOp(entitySet, type);

    internal ExistsOp CreateExistsOp() => new ExistsOp(this.BooleanType);

    internal virtual ElementOp CreateElementOp(TypeUsage type) => new ElementOp(type);

    internal virtual GetEntityRefOp CreateGetEntityRefOp(TypeUsage type) => new GetEntityRefOp(type);

    internal virtual GetRefKeyOp CreateGetRefKeyOp(TypeUsage type) => new GetRefKeyOp(type);

    internal virtual CollectOp CreateCollectOp(TypeUsage type) => new CollectOp(type);

    internal virtual DerefOp CreateDerefOp(TypeUsage type) => new DerefOp(type);

    internal NavigateOp CreateNavigateOp(TypeUsage type, RelProperty relProperty)
    {
      this.AddRelPropertyReference(relProperty);
      return new NavigateOp(type, relProperty);
    }

    internal virtual VarDefListOp CreateVarDefListOp() => VarDefListOp.Instance;

    internal virtual VarDefOp CreateVarDefOp(Var v) => new VarDefOp(v);

    internal Node CreateVarDefNode(Node definingExpr, out Var computedVar)
    {
      ScalarOp op = definingExpr.Op as ScalarOp;
      computedVar = (Var) this.CreateComputedVar(op.Type);
      return this.CreateNode((Op) this.CreateVarDefOp(computedVar), definingExpr);
    }

    internal Node CreateVarDefListNode(Node definingExpr, out Var computedVar)
    {
      Node varDefNode = this.CreateVarDefNode(definingExpr, out computedVar);
      return this.CreateNode((Op) this.CreateVarDefListOp(), varDefNode);
    }

    internal ScanTableOp CreateScanTableOp(TableMD tableMetadata) => this.CreateScanTableOp(this.CreateTableInstance(tableMetadata));

    internal virtual ScanTableOp CreateScanTableOp(Table table) => new ScanTableOp(table);

    internal virtual ScanViewOp CreateScanViewOp(Table table) => new ScanViewOp(table);

    internal virtual ScanViewOp CreateScanViewOp(TableMD tableMetadata) => this.CreateScanViewOp(this.CreateTableInstance(tableMetadata));

    internal virtual UnnestOp CreateUnnestOp(Var v)
    {
      Table tableInstance = this.CreateTableInstance(Command.CreateTableDefinition(TypeHelpers.GetEdmType<CollectionType>(v.Type).TypeUsage));
      return this.CreateUnnestOp(v, tableInstance);
    }

    internal virtual UnnestOp CreateUnnestOp(Var v, Table t) => new UnnestOp(v, t);

    internal virtual FilterOp CreateFilterOp() => FilterOp.Instance;

    internal virtual ProjectOp CreateProjectOp(VarVec vars) => new ProjectOp(vars);

    internal virtual ProjectOp CreateProjectOp(Var v)
    {
      VarVec varVec = this.CreateVarVec();
      varVec.Set(v);
      return new ProjectOp(varVec);
    }

    internal virtual InnerJoinOp CreateInnerJoinOp() => InnerJoinOp.Instance;

    internal virtual LeftOuterJoinOp CreateLeftOuterJoinOp() => LeftOuterJoinOp.Instance;

    internal virtual FullOuterJoinOp CreateFullOuterJoinOp() => FullOuterJoinOp.Instance;

    internal virtual CrossJoinOp CreateCrossJoinOp() => CrossJoinOp.Instance;

    internal virtual CrossApplyOp CreateCrossApplyOp() => CrossApplyOp.Instance;

    internal virtual OuterApplyOp CreateOuterApplyOp() => OuterApplyOp.Instance;

    internal static SortKey CreateSortKey(Var v, bool asc, string collation) => new SortKey(v, asc, collation);

    internal static SortKey CreateSortKey(Var v, bool asc) => new SortKey(v, asc, "");

    internal static SortKey CreateSortKey(Var v) => new SortKey(v, true, "");

    internal virtual SortOp CreateSortOp(List<SortKey> sortKeys) => new SortOp(sortKeys);

    internal virtual ConstrainedSortOp CreateConstrainedSortOp(
      List<SortKey> sortKeys)
    {
      return new ConstrainedSortOp(sortKeys, false);
    }

    internal virtual ConstrainedSortOp CreateConstrainedSortOp(
      List<SortKey> sortKeys,
      bool withTies)
    {
      return new ConstrainedSortOp(sortKeys, withTies);
    }

    internal virtual GroupByOp CreateGroupByOp(VarVec gbyKeys, VarVec outputs) => new GroupByOp(gbyKeys, outputs);

    internal virtual GroupByIntoOp CreateGroupByIntoOp(
      VarVec gbyKeys,
      VarVec inputs,
      VarVec outputs)
    {
      return new GroupByIntoOp(gbyKeys, inputs, outputs);
    }

    internal virtual DistinctOp CreateDistinctOp(VarVec keyVars) => new DistinctOp(keyVars);

    internal virtual DistinctOp CreateDistinctOp(Var keyVar) => new DistinctOp(this.CreateVarVec(keyVar));

    internal virtual UnionAllOp CreateUnionAllOp(VarMap leftMap, VarMap rightMap) => this.CreateUnionAllOp(leftMap, rightMap, (Var) null);

    internal virtual UnionAllOp CreateUnionAllOp(
      VarMap leftMap,
      VarMap rightMap,
      Var branchDiscriminator)
    {
      VarVec varVec = this.CreateVarVec();
      foreach (Var key in (IEnumerable<Var>) leftMap.Keys)
        varVec.Set(key);
      return new UnionAllOp(varVec, leftMap, rightMap, branchDiscriminator);
    }

    internal virtual IntersectOp CreateIntersectOp(VarMap leftMap, VarMap rightMap)
    {
      VarVec varVec = this.CreateVarVec();
      foreach (Var key in (IEnumerable<Var>) leftMap.Keys)
        varVec.Set(key);
      return new IntersectOp(varVec, leftMap, rightMap);
    }

    internal virtual ExceptOp CreateExceptOp(VarMap leftMap, VarMap rightMap)
    {
      VarVec varVec = this.CreateVarVec();
      foreach (Var key in (IEnumerable<Var>) leftMap.Keys)
        varVec.Set(key);
      return new ExceptOp(varVec, leftMap, rightMap);
    }

    internal virtual SingleRowOp CreateSingleRowOp() => SingleRowOp.Instance;

    internal virtual SingleRowTableOp CreateSingleRowTableOp() => SingleRowTableOp.Instance;

    internal virtual PhysicalProjectOp CreatePhysicalProjectOp(
      VarList outputVars,
      SimpleCollectionColumnMap columnMap)
    {
      return new PhysicalProjectOp(outputVars, columnMap);
    }

    internal virtual PhysicalProjectOp CreatePhysicalProjectOp(Var outputVar)
    {
      VarList varList = Command.CreateVarList();
      varList.Add(outputVar);
      VarRefColumnMap varRefColumnMap = new VarRefColumnMap(outputVar);
      SimpleCollectionColumnMap columnMap = new SimpleCollectionColumnMap(TypeUtils.CreateCollectionType(varRefColumnMap.Type), (string) null, (ColumnMap) varRefColumnMap, new SimpleColumnMap[0], new SimpleColumnMap[0]);
      return this.CreatePhysicalProjectOp(varList, columnMap);
    }

    internal static CollectionInfo CreateCollectionInfo(
      Var collectionVar,
      ColumnMap columnMap,
      VarList flattenedElementVars,
      VarVec keys,
      List<SortKey> sortKeys,
      object discriminatorValue)
    {
      return new CollectionInfo(collectionVar, columnMap, flattenedElementVars, keys, sortKeys, discriminatorValue);
    }

    internal virtual SingleStreamNestOp CreateSingleStreamNestOp(
      VarVec keys,
      List<SortKey> prefixSortKeys,
      List<SortKey> postfixSortKeys,
      VarVec outputVars,
      List<CollectionInfo> collectionInfoList,
      Var discriminatorVar)
    {
      return new SingleStreamNestOp(keys, prefixSortKeys, postfixSortKeys, outputVars, collectionInfoList, discriminatorVar);
    }

    internal virtual MultiStreamNestOp CreateMultiStreamNestOp(
      List<SortKey> prefixSortKeys,
      VarVec outputVars,
      List<CollectionInfo> collectionInfoList)
    {
      return new MultiStreamNestOp(prefixSortKeys, outputVars, collectionInfoList);
    }

    internal virtual NodeInfo GetNodeInfo(Node n) => n.GetNodeInfo(this);

    internal virtual ExtendedNodeInfo GetExtendedNodeInfo(Node n) => n.GetExtendedNodeInfo(this);

    internal virtual void RecomputeNodeInfo(Node n) => this.m_nodeInfoVisitor.RecomputeNodeInfo(n);

    internal virtual KeyVec PullupKeys(Node n) => this.m_keyPullupVisitor.GetKeys(n);

    internal static bool EqualTypes(TypeUsage x, TypeUsage y) => TypeUsageEqualityComparer.Instance.Equals(x, y);

    internal static bool EqualTypes(EdmType x, EdmType y) => TypeUsageEqualityComparer.Equals(x, y);

    internal virtual void BuildUnionAllLadder(
      IList<Node> inputNodes,
      IList<Var> inputVars,
      out Node resultNode,
      out IList<Var> resultVars)
    {
      if (inputNodes.Count == 0)
      {
        resultNode = (Node) null;
        resultVars = (IList<Var>) null;
      }
      else
      {
        int num = inputVars.Count / inputNodes.Count;
        if (inputNodes.Count == 1)
        {
          resultNode = inputNodes[0];
          resultVars = inputVars;
        }
        else
        {
          List<Var> varList1 = new List<Var>();
          Node node = inputNodes[0];
          for (int index = 0; index < num; ++index)
            varList1.Add(inputVars[index]);
          for (int index1 = 1; index1 < inputNodes.Count; ++index1)
          {
            VarMap leftMap = new VarMap();
            VarMap rightMap = new VarMap();
            List<Var> varList2 = new List<Var>();
            for (int index2 = 0; index2 < num; ++index2)
            {
              SetOpVar setOpVar = this.CreateSetOpVar(varList1[index2].Type);
              varList2.Add((Var) setOpVar);
              leftMap.Add((Var) setOpVar, varList1[index2]);
              rightMap.Add((Var) setOpVar, inputVars[index1 * num + index2]);
            }
            node = this.CreateNode((Op) this.CreateUnionAllOp(leftMap, rightMap), node, inputNodes[index1]);
            varList1 = varList2;
          }
          resultNode = node;
          resultVars = (IList<Var>) varList1;
        }
      }
    }

    internal virtual void BuildUnionAllLadder(
      IList<Node> inputNodes,
      IList<Var> inputVars,
      out Node resultNode,
      out Var resultVar)
    {
      IList<Var> resultVars;
      this.BuildUnionAllLadder(inputNodes, inputVars, out resultNode, out resultVars);
      if (resultVars != null && resultVars.Count > 0)
        resultVar = resultVars[0];
      else
        resultVar = (Var) null;
    }

    internal virtual Node BuildProject(
      Node inputNode,
      IEnumerable<Var> inputVars,
      IEnumerable<Node> computedExpressions)
    {
      Node node1 = this.CreateNode((Op) this.CreateVarDefListOp());
      VarVec varVec = this.CreateVarVec(inputVars);
      foreach (Node computedExpression in computedExpressions)
      {
        Var computedVar = (Var) this.CreateComputedVar(computedExpression.Op.Type);
        varVec.Set(computedVar);
        Node node2 = this.CreateNode((Op) this.CreateVarDefOp(computedVar), computedExpression);
        node1.Children.Add(node2);
      }
      return this.CreateNode((Op) this.CreateProjectOp(varVec), inputNode, node1);
    }

    internal virtual Node BuildProject(Node input, Node computedExpression, out Var projectVar)
    {
      Node node = this.BuildProject(input, (IEnumerable<Var>) new Var[0], (IEnumerable<Node>) new Node[1]
      {
        computedExpression
      });
      projectVar = ((ProjectOp) node.Op).Outputs.First;
      return node;
    }

    internal virtual void BuildOfTypeTree(
      Node inputNode,
      Var inputVar,
      TypeUsage desiredType,
      bool includeSubtypes,
      out Node resultNode,
      out Var resultVar)
    {
      Node node1 = this.CreateNode(includeSubtypes ? (Op) this.CreateIsOfOp(desiredType) : (Op) this.CreateIsOfOnlyOp(desiredType), this.CreateNode((Op) this.CreateVarRefOp(inputVar)));
      Node node2 = this.CreateNode((Op) this.CreateFilterOp(), inputNode, node1);
      resultNode = this.BuildFakeTreatProject(node2, inputVar, desiredType, out resultVar);
    }

    internal virtual Node BuildFakeTreatProject(
      Node inputNode,
      Var inputVar,
      TypeUsage desiredType,
      out Var resultVar)
    {
      Node node = this.CreateNode((Op) this.CreateFakeTreatOp(desiredType), this.CreateNode((Op) this.CreateVarRefOp(inputVar)));
      return this.BuildProject(inputNode, node, out resultVar);
    }

    internal Node BuildComparison(
      OpType opType,
      Node arg0,
      Node arg1,
      bool useDatabaseNullSemantics = false)
    {
      if (!Command.EqualTypes(arg0.Op.Type, arg1.Op.Type))
      {
        TypeUsage commonTypeUsage = TypeHelpers.GetCommonTypeUsage(arg0.Op.Type, arg1.Op.Type);
        if (!Command.EqualTypes(commonTypeUsage, arg0.Op.Type))
          arg0 = this.CreateNode((Op) this.CreateSoftCastOp(commonTypeUsage), arg0);
        if (!Command.EqualTypes(commonTypeUsage, arg1.Op.Type))
          arg1 = this.CreateNode((Op) this.CreateSoftCastOp(commonTypeUsage), arg1);
      }
      return this.CreateNode((Op) this.CreateComparisonOp(opType, useDatabaseNullSemantics), arg0, arg1);
    }

    internal virtual Node BuildCollect(Node relOpNode, Var relOpVar)
    {
      Node node = this.CreateNode((Op) this.CreatePhysicalProjectOp(relOpVar), relOpNode);
      return this.CreateNode((Op) this.CreateCollectOp(TypeHelpers.CreateCollectionTypeUsage(relOpVar.Type)), node);
    }

    private void AddRelPropertyReference(RelProperty relProperty)
    {
      if (relProperty.ToEnd.RelationshipMultiplicity == RelationshipMultiplicity.Many || this.m_referencedRelProperties.Contains(relProperty))
        return;
      this.m_referencedRelProperties.Add(relProperty);
    }

    internal virtual HashSet<RelProperty> ReferencedRelProperties => this.m_referencedRelProperties;

    internal virtual bool IsRelPropertyReferenced(RelProperty relProperty) => this.m_referencedRelProperties.Contains(relProperty);
  }
}

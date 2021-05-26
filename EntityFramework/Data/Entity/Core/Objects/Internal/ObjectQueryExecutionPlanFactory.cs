// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.ObjectQueryExecutionPlanFactory
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.Internal.Materialization;
using System.Data.Entity.Core.Common.Utils;
using System.Data.Entity.Core.EntityClient.Internal;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.ELinq;
using System.Data.Entity.Resources;
using System.Data.Entity.Utilities;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal class ObjectQueryExecutionPlanFactory
  {
    private readonly System.Data.Entity.Core.Common.Internal.Materialization.Translator _translator;

    public ObjectQueryExecutionPlanFactory(System.Data.Entity.Core.Common.Internal.Materialization.Translator translator = null) => this._translator = translator ?? new System.Data.Entity.Core.Common.Internal.Materialization.Translator();

    public virtual ObjectQueryExecutionPlan Prepare(
      ObjectContext context,
      DbQueryCommandTree tree,
      Type elementType,
      MergeOption mergeOption,
      bool streaming,
      Span span,
      IEnumerable<Tuple<ObjectParameter, QueryParameterExpression>> compiledQueryParameters,
      AliasGenerator aliasGenerator)
    {
      TypeUsage resultType = tree.Query.ResultType;
      DbExpression newQuery;
      SpanIndex spanInfo;
      if (ObjectSpanRewriter.TryRewrite(tree, span, mergeOption, aliasGenerator, out newQuery, out spanInfo))
        tree = DbQueryCommandTree.FromValidExpression(tree.MetadataWorkspace, tree.DataSpace, newQuery, tree.UseDatabaseNullSemantics, tree.DisableFilterOverProjectionSimplificationForCustomFunctions);
      else
        spanInfo = (SpanIndex) null;
      EntityCommandDefinition commandDefinition = ObjectQueryExecutionPlanFactory.CreateCommandDefinition(context, tree);
      ShaperFactory resultShaperFactory = System.Data.Entity.Core.Common.Internal.Materialization.Translator.TranslateColumnMap(this._translator, elementType, commandDefinition.CreateColumnMap((DbDataReader) null), context.MetadataWorkspace, spanInfo, mergeOption, streaming, false);
      EntitySet singleEntitySet = (EntitySet) null;
      if (resultType.EdmType.BuiltInTypeKind == BuiltInTypeKind.CollectionType && commandDefinition.EntitySets != null)
      {
        foreach (EntitySet entitySet in commandDefinition.EntitySets)
        {
          if (entitySet != null && entitySet.ElementType.IsAssignableFrom(((CollectionType) resultType.EdmType).TypeUsage.EdmType))
          {
            if (singleEntitySet == null)
            {
              singleEntitySet = entitySet;
            }
            else
            {
              singleEntitySet = (EntitySet) null;
              break;
            }
          }
        }
      }
      return new ObjectQueryExecutionPlan((DbCommandDefinition) commandDefinition, resultShaperFactory, resultType, mergeOption, streaming, singleEntitySet, compiledQueryParameters);
    }

    private static EntityCommandDefinition CreateCommandDefinition(
      ObjectContext context,
      DbQueryCommandTree tree)
    {
      DbProviderServices providerServices = DbProviderServices.GetProviderServices(context.Connection ?? throw new InvalidOperationException(Strings.ObjectQuery_InvalidConnection));
      DbCommandDefinition commandDefinition;
      try
      {
        commandDefinition = providerServices.CreateCommandDefinition((DbCommandTree) tree, context.InterceptionContext);
      }
      catch (EntityCommandCompilationException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        if (ex.IsCatchableExceptionType())
          throw new EntityCommandCompilationException(Strings.EntityClient_CommandDefinitionPreparationFailed, ex);
        throw;
      }
      return commandDefinition != null ? (EntityCommandDefinition) commandDefinition : throw new NotSupportedException(Strings.ADP_ProviderDoesNotSupportCommandTrees);
    }
  }
}

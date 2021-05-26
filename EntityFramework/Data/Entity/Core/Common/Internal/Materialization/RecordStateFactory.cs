// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Internal.Materialization.RecordStateFactory
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq.Expressions;

namespace System.Data.Entity.Core.Common.Internal.Materialization
{
  internal class RecordStateFactory
  {
    internal readonly int StateSlotNumber;
    internal readonly int ColumnCount;
    internal readonly DataRecordInfo DataRecordInfo;
    internal readonly Func<Shaper, bool> GatherData;
    internal readonly ReadOnlyCollection<RecordStateFactory> NestedRecordStateFactories;
    internal readonly ReadOnlyCollection<string> ColumnNames;
    internal readonly ReadOnlyCollection<TypeUsage> TypeUsages;
    internal readonly ReadOnlyCollection<bool> IsColumnNested;
    internal readonly bool HasNestedColumns;
    internal readonly FieldNameLookup FieldNameLookup;
    private readonly string Description;

    public RecordStateFactory(
      int stateSlotNumber,
      int columnCount,
      RecordStateFactory[] nestedRecordStateFactories,
      DataRecordInfo dataRecordInfo,
      Expression<Func<Shaper, bool>> gatherData,
      string[] propertyNames,
      TypeUsage[] typeUsages,
      bool[] isColumnNested)
    {
      this.StateSlotNumber = stateSlotNumber;
      this.ColumnCount = columnCount;
      this.NestedRecordStateFactories = new ReadOnlyCollection<RecordStateFactory>((IList<RecordStateFactory>) nestedRecordStateFactories);
      this.DataRecordInfo = dataRecordInfo;
      this.GatherData = gatherData.Compile();
      this.Description = gatherData.ToString();
      this.ColumnNames = new ReadOnlyCollection<string>((IList<string>) propertyNames);
      this.TypeUsages = new ReadOnlyCollection<TypeUsage>((IList<TypeUsage>) typeUsages);
      this.FieldNameLookup = new FieldNameLookup(this.ColumnNames);
      if (isColumnNested == null)
      {
        isColumnNested = new bool[columnCount];
        for (int index = 0; index < columnCount; ++index)
        {
          switch (typeUsages[index].EdmType.BuiltInTypeKind)
          {
            case BuiltInTypeKind.CollectionType:
            case BuiltInTypeKind.ComplexType:
            case BuiltInTypeKind.EntityType:
            case BuiltInTypeKind.RowType:
              isColumnNested[index] = true;
              this.HasNestedColumns = true;
              break;
            default:
              isColumnNested[index] = false;
              break;
          }
        }
      }
      this.IsColumnNested = new ReadOnlyCollection<bool>((IList<bool>) isColumnNested);
    }

    public RecordStateFactory(
      int stateSlotNumber,
      int columnCount,
      RecordStateFactory[] nestedRecordStateFactories,
      DataRecordInfo dataRecordInfo,
      Expression gatherData,
      string[] propertyNames,
      TypeUsage[] typeUsages)
      : this(stateSlotNumber, columnCount, nestedRecordStateFactories, dataRecordInfo, CodeGenEmitter.BuildShaperLambda<bool>(gatherData), propertyNames, typeUsages, (bool[]) null)
    {
    }

    internal RecordState Create(CoordinatorFactory coordinatorFactory) => new RecordState(this, coordinatorFactory);
  }
}

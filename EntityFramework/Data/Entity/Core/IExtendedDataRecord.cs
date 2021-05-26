// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.IExtendedDataRecord
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Common;
using System.Data.Entity.Core.Common;

namespace System.Data.Entity.Core
{
  /// <summary>
  /// DataRecord interface supporting structured types and rich metadata information.
  /// </summary>
  public interface IExtendedDataRecord : IDataRecord
  {
    /// <summary>
    /// Gets <see cref="T:System.Data.Entity.Core.Common.DataRecordInfo" /> for this
    /// <see cref="T:System.Data.Entity.Core.IExtendedDataRecord" />
    /// .
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Data.Entity.Core.Common.DataRecordInfo" /> object.
    /// </returns>
    DataRecordInfo DataRecordInfo { get; }

    /// <summary>
    /// Gets a <see cref="T:System.Data.Common.DbDataRecord" /> object with the specified index.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.Data.Common.DbDataRecord" /> object.
    /// </returns>
    /// <param name="i">The index of the row.</param>
    DbDataRecord GetDataRecord(int i);

    /// <summary>
    /// Returns nested readers as <see cref="T:System.Data.Common.DbDataReader" /> objects.
    /// </summary>
    /// <returns>
    /// Nested readers as <see cref="T:System.Data.Common.DbDataReader" /> objects.
    /// </returns>
    /// <param name="i">The ordinal of the column.</param>
    DbDataReader GetDataReader(int i);
  }
}

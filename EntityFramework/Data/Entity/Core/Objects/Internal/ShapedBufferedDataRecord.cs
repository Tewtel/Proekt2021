// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.ShapedBufferedDataRecord
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal class ShapedBufferedDataRecord : BufferedDataRecord
  {
    private int _rowCapacity = 1;
    private BitArray _bools;
    private bool[] _tempBools;
    private int _boolCount;
    private byte[] _bytes;
    private int _byteCount;
    private char[] _chars;
    private int _charCount;
    private DateTime[] _dateTimes;
    private int _dateTimeCount;
    private Decimal[] _decimals;
    private int _decimalCount;
    private double[] _doubles;
    private int _doubleCount;
    private float[] _floats;
    private int _floatCount;
    private Guid[] _guids;
    private int _guidCount;
    private short[] _shorts;
    private int _shortCount;
    private int[] _ints;
    private int _intCount;
    private long[] _longs;
    private int _longCount;
    private object[] _objects;
    private int _objectCount;
    private int[] _ordinalToIndexMap;
    private BitArray _nulls;
    private bool[] _tempNulls;
    private int _nullCount;
    private int[] _nullOrdinalToIndexMap;
    private ShapedBufferedDataRecord.TypeCase[] _columnTypeCases;

    protected ShapedBufferedDataRecord()
    {
    }

    internal static BufferedDataRecord Initialize(
      string providerManifestToken,
      DbProviderServices providerServices,
      DbDataReader reader,
      Type[] columnTypes,
      bool[] nullableColumns)
    {
      ShapedBufferedDataRecord bufferedDataRecord = new ShapedBufferedDataRecord();
      bufferedDataRecord.ReadMetadata(providerManifestToken, providerServices, reader);
      DbSpatialDataReader spatialDataReader = (DbSpatialDataReader) null;
      if (((IEnumerable<Type>) columnTypes).Any<Type>((Func<Type, bool>) (t => t == typeof (DbGeography) || t == typeof (DbGeometry))))
        spatialDataReader = providerServices.GetSpatialDataReader(reader, providerManifestToken);
      return bufferedDataRecord.Initialize(reader, spatialDataReader, columnTypes, nullableColumns);
    }

    internal static Task<BufferedDataRecord> InitializeAsync(
      string providerManifestToken,
      DbProviderServices providerServices,
      DbDataReader reader,
      Type[] columnTypes,
      bool[] nullableColumns,
      CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();
      ShapedBufferedDataRecord bufferedDataRecord = new ShapedBufferedDataRecord();
      bufferedDataRecord.ReadMetadata(providerManifestToken, providerServices, reader);
      DbSpatialDataReader spatialDataReader = (DbSpatialDataReader) null;
      if (((IEnumerable<Type>) columnTypes).Any<Type>((Func<Type, bool>) (t => t == typeof (DbGeography) || t == typeof (DbGeometry))))
        spatialDataReader = providerServices.GetSpatialDataReader(reader, providerManifestToken);
      return bufferedDataRecord.InitializeAsync(reader, spatialDataReader, columnTypes, nullableColumns, cancellationToken);
    }

    private BufferedDataRecord Initialize(
      DbDataReader reader,
      DbSpatialDataReader spatialDataReader,
      Type[] columnTypes,
      bool[] nullableColumns)
    {
      this.InitializeFields(columnTypes, nullableColumns);
      while (reader.Read())
      {
        ++this._currentRowNumber;
        if (this._rowCapacity == this._currentRowNumber)
          this.DoubleBufferCapacity();
        int num = Math.Max(columnTypes.Length, nullableColumns.Length);
        for (int ordinal = 0; ordinal < num; ++ordinal)
        {
          if (ordinal < this._columnTypeCases.Length)
          {
            switch (this._columnTypeCases[ordinal])
            {
              case ShapedBufferedDataRecord.TypeCase.Empty:
                if (nullableColumns[ordinal])
                {
                  this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal);
                  continue;
                }
                continue;
              case ShapedBufferedDataRecord.TypeCase.Bool:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadBool(reader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadBool(reader, ordinal);
                continue;
              case ShapedBufferedDataRecord.TypeCase.Byte:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadByte(reader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadByte(reader, ordinal);
                continue;
              case ShapedBufferedDataRecord.TypeCase.Char:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadChar(reader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadChar(reader, ordinal);
                continue;
              case ShapedBufferedDataRecord.TypeCase.DateTime:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadDateTime(reader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadDateTime(reader, ordinal);
                continue;
              case ShapedBufferedDataRecord.TypeCase.Decimal:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadDecimal(reader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadDecimal(reader, ordinal);
                continue;
              case ShapedBufferedDataRecord.TypeCase.Double:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadDouble(reader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadDouble(reader, ordinal);
                continue;
              case ShapedBufferedDataRecord.TypeCase.Float:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadFloat(reader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadFloat(reader, ordinal);
                continue;
              case ShapedBufferedDataRecord.TypeCase.Guid:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadGuid(reader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadGuid(reader, ordinal);
                continue;
              case ShapedBufferedDataRecord.TypeCase.Short:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadShort(reader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadShort(reader, ordinal);
                continue;
              case ShapedBufferedDataRecord.TypeCase.Int:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadInt(reader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadInt(reader, ordinal);
                continue;
              case ShapedBufferedDataRecord.TypeCase.Long:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadLong(reader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadLong(reader, ordinal);
                continue;
              case ShapedBufferedDataRecord.TypeCase.DbGeography:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadGeography(spatialDataReader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadGeography(spatialDataReader, ordinal);
                continue;
              case ShapedBufferedDataRecord.TypeCase.DbGeometry:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadGeometry(spatialDataReader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadGeometry(spatialDataReader, ordinal);
                continue;
              default:
                if (nullableColumns[ordinal])
                {
                  if (!(this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal)))
                  {
                    this.ReadObject(reader, ordinal);
                    continue;
                  }
                  continue;
                }
                this.ReadObject(reader, ordinal);
                continue;
            }
          }
          else if (nullableColumns[ordinal])
            this._tempNulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]] = reader.IsDBNull(ordinal);
        }
      }
      this._bools = new BitArray(this._tempBools);
      this._tempBools = (bool[]) null;
      this._nulls = new BitArray(this._tempNulls);
      this._tempNulls = (bool[]) null;
      this._rowCount = this._currentRowNumber + 1;
      this._currentRowNumber = -1;
      return (BufferedDataRecord) this;
    }

    private async Task<BufferedDataRecord> InitializeAsync(
      DbDataReader reader,
      DbSpatialDataReader spatialDataReader,
      Type[] columnTypes,
      bool[] nullableColumns,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      bufferedDataRecord.InitializeFields(columnTypes, nullableColumns);
label_84:
      System.Data.Entity.Utilities.TaskExtensions.CultureAwaiter<bool> cultureAwaiter = reader.ReadAsync(cancellationToken).WithCurrentCulture<bool>();
      if (await cultureAwaiter)
      {
        cancellationToken.ThrowIfCancellationRequested();
        ++bufferedDataRecord._currentRowNumber;
        if (bufferedDataRecord._rowCapacity == bufferedDataRecord._currentRowNumber)
          bufferedDataRecord.DoubleBufferCapacity();
        int columnCount = columnTypes.Length > nullableColumns.Length ? columnTypes.Length : nullableColumns.Length;
        for (int i = 0; i < columnCount; ++i)
        {
          if (i < bufferedDataRecord._columnTypeCases.Length)
          {
            switch (bufferedDataRecord._columnTypeCases[i])
            {
              case ShapedBufferedDataRecord.TypeCase.Empty:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag;
                  continue;
                }
                continue;
              case ShapedBufferedDataRecord.TypeCase.Bool:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadBoolAsync(reader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadBoolAsync(reader, i, cancellationToken).WithCurrentCulture();
                continue;
              case ShapedBufferedDataRecord.TypeCase.Byte:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadByteAsync(reader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadByteAsync(reader, i, cancellationToken).WithCurrentCulture();
                continue;
              case ShapedBufferedDataRecord.TypeCase.Char:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadCharAsync(reader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadCharAsync(reader, i, cancellationToken).WithCurrentCulture();
                continue;
              case ShapedBufferedDataRecord.TypeCase.DateTime:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadDateTimeAsync(reader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadDateTimeAsync(reader, i, cancellationToken).WithCurrentCulture();
                continue;
              case ShapedBufferedDataRecord.TypeCase.Decimal:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadDecimalAsync(reader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadDecimalAsync(reader, i, cancellationToken).WithCurrentCulture();
                continue;
              case ShapedBufferedDataRecord.TypeCase.Double:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadDoubleAsync(reader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadDoubleAsync(reader, i, cancellationToken).WithCurrentCulture();
                continue;
              case ShapedBufferedDataRecord.TypeCase.Float:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadFloatAsync(reader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadFloatAsync(reader, i, cancellationToken).WithCurrentCulture();
                continue;
              case ShapedBufferedDataRecord.TypeCase.Guid:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadGuidAsync(reader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadGuidAsync(reader, i, cancellationToken).WithCurrentCulture();
                continue;
              case ShapedBufferedDataRecord.TypeCase.Short:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadShortAsync(reader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadShortAsync(reader, i, cancellationToken).WithCurrentCulture();
                continue;
              case ShapedBufferedDataRecord.TypeCase.Int:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadIntAsync(reader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadIntAsync(reader, i, cancellationToken).WithCurrentCulture();
                continue;
              case ShapedBufferedDataRecord.TypeCase.Long:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadLongAsync(reader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadLongAsync(reader, i, cancellationToken).WithCurrentCulture();
                continue;
              case ShapedBufferedDataRecord.TypeCase.DbGeography:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadGeographyAsync(spatialDataReader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadGeographyAsync(spatialDataReader, i, cancellationToken).WithCurrentCulture();
                continue;
              case ShapedBufferedDataRecord.TypeCase.DbGeometry:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadGeometryAsync(spatialDataReader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadGeometryAsync(spatialDataReader, i, cancellationToken).WithCurrentCulture();
                continue;
              default:
                if (nullableColumns[i])
                {
                  cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
                  bool flag = await cultureAwaiter;
                  if (!(bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag))
                  {
                    await bufferedDataRecord.ReadObjectAsync(reader, i, cancellationToken).WithCurrentCulture();
                    continue;
                  }
                  continue;
                }
                await bufferedDataRecord.ReadObjectAsync(reader, i, cancellationToken).WithCurrentCulture();
                continue;
            }
          }
          else if (nullableColumns[i])
          {
            cultureAwaiter = reader.IsDBNullAsync(i, cancellationToken).WithCurrentCulture<bool>();
            bool flag = await cultureAwaiter;
            bufferedDataRecord._tempNulls[bufferedDataRecord._currentRowNumber * bufferedDataRecord._nullCount + bufferedDataRecord._nullOrdinalToIndexMap[i]] = flag;
          }
        }
        goto label_84;
      }
      else
      {
        bufferedDataRecord._bools = new BitArray(bufferedDataRecord._tempBools);
        bufferedDataRecord._tempBools = (bool[]) null;
        bufferedDataRecord._nulls = new BitArray(bufferedDataRecord._tempNulls);
        bufferedDataRecord._tempNulls = (bool[]) null;
        bufferedDataRecord._rowCount = bufferedDataRecord._currentRowNumber + 1;
        bufferedDataRecord._currentRowNumber = -1;
        return (BufferedDataRecord) bufferedDataRecord;
      }
    }

    private void InitializeFields(Type[] columnTypes, bool[] nullableColumns)
    {
      this._columnTypeCases = Enumerable.Repeat<ShapedBufferedDataRecord.TypeCase>(ShapedBufferedDataRecord.TypeCase.Empty, columnTypes.Length).ToArray<ShapedBufferedDataRecord.TypeCase>();
      int count = Math.Max(this.FieldCount, Math.Max(columnTypes.Length, nullableColumns.Length));
      this._ordinalToIndexMap = Enumerable.Repeat<int>(-1, count).ToArray<int>();
      for (int index = 0; index < columnTypes.Length; ++index)
      {
        Type columnType = columnTypes[index];
        if (!(columnType == (Type) null))
        {
          if (columnType == typeof (bool))
          {
            this._columnTypeCases[index] = ShapedBufferedDataRecord.TypeCase.Bool;
            this._ordinalToIndexMap[index] = this._boolCount;
            ++this._boolCount;
          }
          else if (columnType == typeof (byte))
          {
            this._columnTypeCases[index] = ShapedBufferedDataRecord.TypeCase.Byte;
            this._ordinalToIndexMap[index] = this._byteCount;
            ++this._byteCount;
          }
          else if (columnType == typeof (char))
          {
            this._columnTypeCases[index] = ShapedBufferedDataRecord.TypeCase.Char;
            this._ordinalToIndexMap[index] = this._charCount;
            ++this._charCount;
          }
          else if (columnType == typeof (DateTime))
          {
            this._columnTypeCases[index] = ShapedBufferedDataRecord.TypeCase.DateTime;
            this._ordinalToIndexMap[index] = this._dateTimeCount;
            ++this._dateTimeCount;
          }
          else if (columnType == typeof (Decimal))
          {
            this._columnTypeCases[index] = ShapedBufferedDataRecord.TypeCase.Decimal;
            this._ordinalToIndexMap[index] = this._decimalCount;
            ++this._decimalCount;
          }
          else if (columnType == typeof (double))
          {
            this._columnTypeCases[index] = ShapedBufferedDataRecord.TypeCase.Double;
            this._ordinalToIndexMap[index] = this._doubleCount;
            ++this._doubleCount;
          }
          else if (columnType == typeof (float))
          {
            this._columnTypeCases[index] = ShapedBufferedDataRecord.TypeCase.Float;
            this._ordinalToIndexMap[index] = this._floatCount;
            ++this._floatCount;
          }
          else if (columnType == typeof (Guid))
          {
            this._columnTypeCases[index] = ShapedBufferedDataRecord.TypeCase.Guid;
            this._ordinalToIndexMap[index] = this._guidCount;
            ++this._guidCount;
          }
          else if (columnType == typeof (short))
          {
            this._columnTypeCases[index] = ShapedBufferedDataRecord.TypeCase.Short;
            this._ordinalToIndexMap[index] = this._shortCount;
            ++this._shortCount;
          }
          else if (columnType == typeof (int))
          {
            this._columnTypeCases[index] = ShapedBufferedDataRecord.TypeCase.Int;
            this._ordinalToIndexMap[index] = this._intCount;
            ++this._intCount;
          }
          else if (columnType == typeof (long))
          {
            this._columnTypeCases[index] = ShapedBufferedDataRecord.TypeCase.Long;
            this._ordinalToIndexMap[index] = this._longCount;
            ++this._longCount;
          }
          else
          {
            this._columnTypeCases[index] = !(columnType == typeof (DbGeography)) ? (!(columnType == typeof (DbGeometry)) ? ShapedBufferedDataRecord.TypeCase.Object : ShapedBufferedDataRecord.TypeCase.DbGeometry) : ShapedBufferedDataRecord.TypeCase.DbGeography;
            this._ordinalToIndexMap[index] = this._objectCount;
            ++this._objectCount;
          }
        }
      }
      this._tempBools = new bool[this._rowCapacity * this._boolCount];
      this._bytes = new byte[this._rowCapacity * this._byteCount];
      this._chars = new char[this._rowCapacity * this._charCount];
      this._dateTimes = new DateTime[this._rowCapacity * this._dateTimeCount];
      this._decimals = new Decimal[this._rowCapacity * this._decimalCount];
      this._doubles = new double[this._rowCapacity * this._doubleCount];
      this._floats = new float[this._rowCapacity * this._floatCount];
      this._guids = new Guid[this._rowCapacity * this._guidCount];
      this._shorts = new short[this._rowCapacity * this._shortCount];
      this._ints = new int[this._rowCapacity * this._intCount];
      this._longs = new long[this._rowCapacity * this._longCount];
      this._objects = new object[this._rowCapacity * this._objectCount];
      this._nullOrdinalToIndexMap = Enumerable.Repeat<int>(-1, count).ToArray<int>();
      for (int index = 0; index < nullableColumns.Length; ++index)
      {
        if (nullableColumns[index])
        {
          this._nullOrdinalToIndexMap[index] = this._nullCount;
          ++this._nullCount;
        }
      }
      this._tempNulls = new bool[this._rowCapacity * this._nullCount];
    }

    private void DoubleBufferCapacity()
    {
      this._rowCapacity <<= 1;
      bool[] flagArray1 = new bool[this._tempBools.Length << 1];
      Array.Copy((Array) this._tempBools, (Array) flagArray1, this._tempBools.Length);
      this._tempBools = flagArray1;
      byte[] numArray1 = new byte[this._bytes.Length << 1];
      Array.Copy((Array) this._bytes, (Array) numArray1, this._bytes.Length);
      this._bytes = numArray1;
      char[] chArray = new char[this._chars.Length << 1];
      Array.Copy((Array) this._chars, (Array) chArray, this._chars.Length);
      this._chars = chArray;
      DateTime[] dateTimeArray = new DateTime[this._dateTimes.Length << 1];
      Array.Copy((Array) this._dateTimes, (Array) dateTimeArray, this._dateTimes.Length);
      this._dateTimes = dateTimeArray;
      Decimal[] numArray2 = new Decimal[this._decimals.Length << 1];
      Array.Copy((Array) this._decimals, (Array) numArray2, this._decimals.Length);
      this._decimals = numArray2;
      double[] numArray3 = new double[this._doubles.Length << 1];
      Array.Copy((Array) this._doubles, (Array) numArray3, this._doubles.Length);
      this._doubles = numArray3;
      float[] numArray4 = new float[this._floats.Length << 1];
      Array.Copy((Array) this._floats, (Array) numArray4, this._floats.Length);
      this._floats = numArray4;
      Guid[] guidArray = new Guid[this._guids.Length << 1];
      Array.Copy((Array) this._guids, (Array) guidArray, this._guids.Length);
      this._guids = guidArray;
      short[] numArray5 = new short[this._shorts.Length << 1];
      Array.Copy((Array) this._shorts, (Array) numArray5, this._shorts.Length);
      this._shorts = numArray5;
      int[] numArray6 = new int[this._ints.Length << 1];
      Array.Copy((Array) this._ints, (Array) numArray6, this._ints.Length);
      this._ints = numArray6;
      long[] numArray7 = new long[this._longs.Length << 1];
      Array.Copy((Array) this._longs, (Array) numArray7, this._longs.Length);
      this._longs = numArray7;
      object[] objArray = new object[this._objects.Length << 1];
      Array.Copy((Array) this._objects, (Array) objArray, this._objects.Length);
      this._objects = objArray;
      bool[] flagArray2 = new bool[this._tempNulls.Length << 1];
      Array.Copy((Array) this._tempNulls, (Array) flagArray2, this._tempNulls.Length);
      this._tempNulls = flagArray2;
    }

    private void ReadBool(DbDataReader reader, int ordinal) => this._tempBools[this._currentRowNumber * this._boolCount + this._ordinalToIndexMap[ordinal]] = reader.GetBoolean(ordinal);

    private async Task ReadBoolAsync(
      DbDataReader reader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      bool flag = await reader.GetFieldValueAsync<bool>(ordinal, cancellationToken).WithCurrentCulture<bool>();
      bufferedDataRecord._tempBools[bufferedDataRecord._currentRowNumber * bufferedDataRecord._boolCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = flag;
    }

    private void ReadByte(DbDataReader reader, int ordinal) => this._bytes[this._currentRowNumber * this._byteCount + this._ordinalToIndexMap[ordinal]] = reader.GetByte(ordinal);

    private async Task ReadByteAsync(
      DbDataReader reader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      byte num = await reader.GetFieldValueAsync<byte>(ordinal, cancellationToken).WithCurrentCulture<byte>();
      bufferedDataRecord._bytes[bufferedDataRecord._currentRowNumber * bufferedDataRecord._byteCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = num;
    }

    private void ReadChar(DbDataReader reader, int ordinal) => this._chars[this._currentRowNumber * this._charCount + this._ordinalToIndexMap[ordinal]] = reader.GetChar(ordinal);

    private async Task ReadCharAsync(
      DbDataReader reader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      char ch = await reader.GetFieldValueAsync<char>(ordinal, cancellationToken).WithCurrentCulture<char>();
      bufferedDataRecord._chars[bufferedDataRecord._currentRowNumber * bufferedDataRecord._charCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = ch;
    }

    private void ReadDateTime(DbDataReader reader, int ordinal) => this._dateTimes[this._currentRowNumber * this._dateTimeCount + this._ordinalToIndexMap[ordinal]] = reader.GetDateTime(ordinal);

    private async Task ReadDateTimeAsync(
      DbDataReader reader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      DateTime dateTime = await reader.GetFieldValueAsync<DateTime>(ordinal, cancellationToken).WithCurrentCulture<DateTime>();
      bufferedDataRecord._dateTimes[bufferedDataRecord._currentRowNumber * bufferedDataRecord._dateTimeCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = dateTime;
    }

    private void ReadDecimal(DbDataReader reader, int ordinal) => this._decimals[this._currentRowNumber * this._decimalCount + this._ordinalToIndexMap[ordinal]] = reader.GetDecimal(ordinal);

    private async Task ReadDecimalAsync(
      DbDataReader reader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      Decimal num = await reader.GetFieldValueAsync<Decimal>(ordinal, cancellationToken).WithCurrentCulture<Decimal>();
      bufferedDataRecord._decimals[bufferedDataRecord._currentRowNumber * bufferedDataRecord._decimalCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = num;
    }

    private void ReadDouble(DbDataReader reader, int ordinal) => this._doubles[this._currentRowNumber * this._doubleCount + this._ordinalToIndexMap[ordinal]] = reader.GetDouble(ordinal);

    private async Task ReadDoubleAsync(
      DbDataReader reader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      double num = await reader.GetFieldValueAsync<double>(ordinal, cancellationToken).WithCurrentCulture<double>();
      bufferedDataRecord._doubles[bufferedDataRecord._currentRowNumber * bufferedDataRecord._doubleCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = num;
    }

    private void ReadFloat(DbDataReader reader, int ordinal) => this._floats[this._currentRowNumber * this._floatCount + this._ordinalToIndexMap[ordinal]] = reader.GetFloat(ordinal);

    private async Task ReadFloatAsync(
      DbDataReader reader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      float num = await reader.GetFieldValueAsync<float>(ordinal, cancellationToken).WithCurrentCulture<float>();
      bufferedDataRecord._floats[bufferedDataRecord._currentRowNumber * bufferedDataRecord._floatCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = num;
    }

    private void ReadGuid(DbDataReader reader, int ordinal) => this._guids[this._currentRowNumber * this._guidCount + this._ordinalToIndexMap[ordinal]] = reader.GetGuid(ordinal);

    private async Task ReadGuidAsync(
      DbDataReader reader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      Guid guid = await reader.GetFieldValueAsync<Guid>(ordinal, cancellationToken).WithCurrentCulture<Guid>();
      bufferedDataRecord._guids[bufferedDataRecord._currentRowNumber * bufferedDataRecord._guidCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = guid;
    }

    private void ReadShort(DbDataReader reader, int ordinal) => this._shorts[this._currentRowNumber * this._shortCount + this._ordinalToIndexMap[ordinal]] = reader.GetInt16(ordinal);

    private async Task ReadShortAsync(
      DbDataReader reader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      short num = await reader.GetFieldValueAsync<short>(ordinal, cancellationToken).WithCurrentCulture<short>();
      bufferedDataRecord._shorts[bufferedDataRecord._currentRowNumber * bufferedDataRecord._shortCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = num;
    }

    private void ReadInt(DbDataReader reader, int ordinal) => this._ints[this._currentRowNumber * this._intCount + this._ordinalToIndexMap[ordinal]] = reader.GetInt32(ordinal);

    private async Task ReadIntAsync(
      DbDataReader reader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      int num = await reader.GetFieldValueAsync<int>(ordinal, cancellationToken).WithCurrentCulture<int>();
      bufferedDataRecord._ints[bufferedDataRecord._currentRowNumber * bufferedDataRecord._intCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = num;
    }

    private void ReadLong(DbDataReader reader, int ordinal) => this._longs[this._currentRowNumber * this._longCount + this._ordinalToIndexMap[ordinal]] = reader.GetInt64(ordinal);

    private async Task ReadLongAsync(
      DbDataReader reader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      long num = await reader.GetFieldValueAsync<long>(ordinal, cancellationToken).WithCurrentCulture<long>();
      bufferedDataRecord._longs[bufferedDataRecord._currentRowNumber * bufferedDataRecord._longCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = num;
    }

    private void ReadObject(DbDataReader reader, int ordinal) => this._objects[this._currentRowNumber * this._objectCount + this._ordinalToIndexMap[ordinal]] = reader.GetValue(ordinal);

    private async Task ReadObjectAsync(
      DbDataReader reader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      object obj = await reader.GetFieldValueAsync<object>(ordinal, cancellationToken).WithCurrentCulture<object>();
      bufferedDataRecord._objects[bufferedDataRecord._currentRowNumber * bufferedDataRecord._objectCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = obj;
    }

    private void ReadGeography(DbSpatialDataReader spatialReader, int ordinal) => this._objects[this._currentRowNumber * this._objectCount + this._ordinalToIndexMap[ordinal]] = (object) spatialReader.GetGeography(ordinal);

    private async Task ReadGeographyAsync(
      DbSpatialDataReader spatialReader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      DbGeography dbGeography = await spatialReader.GetGeographyAsync(ordinal, cancellationToken).WithCurrentCulture<DbGeography>();
      bufferedDataRecord._objects[bufferedDataRecord._currentRowNumber * bufferedDataRecord._objectCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = (object) dbGeography;
    }

    private void ReadGeometry(DbSpatialDataReader spatialReader, int ordinal) => this._objects[this._currentRowNumber * this._objectCount + this._ordinalToIndexMap[ordinal]] = (object) spatialReader.GetGeometry(ordinal);

    private async Task ReadGeometryAsync(
      DbSpatialDataReader spatialReader,
      int ordinal,
      CancellationToken cancellationToken)
    {
      ShapedBufferedDataRecord bufferedDataRecord = this;
      DbGeometry dbGeometry = await spatialReader.GetGeometryAsync(ordinal, cancellationToken).WithCurrentCulture<DbGeometry>();
      bufferedDataRecord._objects[bufferedDataRecord._currentRowNumber * bufferedDataRecord._objectCount + bufferedDataRecord._ordinalToIndexMap[ordinal]] = (object) dbGeometry;
    }

    public override bool GetBoolean(int ordinal) => this._columnTypeCases[ordinal] == ShapedBufferedDataRecord.TypeCase.Bool ? this._bools[this._currentRowNumber * this._boolCount + this._ordinalToIndexMap[ordinal]] : this.GetFieldValue<bool>(ordinal);

    public override byte GetByte(int ordinal) => this._columnTypeCases[ordinal] == ShapedBufferedDataRecord.TypeCase.Byte ? this._bytes[this._currentRowNumber * this._byteCount + this._ordinalToIndexMap[ordinal]] : this.GetFieldValue<byte>(ordinal);

    public override char GetChar(int ordinal) => this._columnTypeCases[ordinal] == ShapedBufferedDataRecord.TypeCase.Char ? this._chars[this._currentRowNumber * this._charCount + this._ordinalToIndexMap[ordinal]] : this.GetFieldValue<char>(ordinal);

    public override DateTime GetDateTime(int ordinal) => this._columnTypeCases[ordinal] == ShapedBufferedDataRecord.TypeCase.DateTime ? this._dateTimes[this._currentRowNumber * this._dateTimeCount + this._ordinalToIndexMap[ordinal]] : this.GetFieldValue<DateTime>(ordinal);

    public override Decimal GetDecimal(int ordinal) => this._columnTypeCases[ordinal] == ShapedBufferedDataRecord.TypeCase.Decimal ? this._decimals[this._currentRowNumber * this._decimalCount + this._ordinalToIndexMap[ordinal]] : this.GetFieldValue<Decimal>(ordinal);

    public override double GetDouble(int ordinal) => this._columnTypeCases[ordinal] == ShapedBufferedDataRecord.TypeCase.Double ? this._doubles[this._currentRowNumber * this._doubleCount + this._ordinalToIndexMap[ordinal]] : this.GetFieldValue<double>(ordinal);

    public override float GetFloat(int ordinal) => this._columnTypeCases[ordinal] == ShapedBufferedDataRecord.TypeCase.Float ? this._floats[this._currentRowNumber * this._floatCount + this._ordinalToIndexMap[ordinal]] : this.GetFieldValue<float>(ordinal);

    public override Guid GetGuid(int ordinal) => this._columnTypeCases[ordinal] == ShapedBufferedDataRecord.TypeCase.Guid ? this._guids[this._currentRowNumber * this._guidCount + this._ordinalToIndexMap[ordinal]] : this.GetFieldValue<Guid>(ordinal);

    public override short GetInt16(int ordinal) => this._columnTypeCases[ordinal] == ShapedBufferedDataRecord.TypeCase.Short ? this._shorts[this._currentRowNumber * this._shortCount + this._ordinalToIndexMap[ordinal]] : this.GetFieldValue<short>(ordinal);

    public override int GetInt32(int ordinal) => this._columnTypeCases[ordinal] == ShapedBufferedDataRecord.TypeCase.Int ? this._ints[this._currentRowNumber * this._intCount + this._ordinalToIndexMap[ordinal]] : this.GetFieldValue<int>(ordinal);

    public override long GetInt64(int ordinal) => this._columnTypeCases[ordinal] == ShapedBufferedDataRecord.TypeCase.Long ? this._longs[this._currentRowNumber * this._longCount + this._ordinalToIndexMap[ordinal]] : this.GetFieldValue<long>(ordinal);

    public override string GetString(int ordinal) => this._columnTypeCases[ordinal] == ShapedBufferedDataRecord.TypeCase.Object ? (string) this._objects[this._currentRowNumber * this._objectCount + this._ordinalToIndexMap[ordinal]] : this.GetFieldValue<string>(ordinal);

    public override object GetValue(int ordinal) => this.GetFieldValue<object>(ordinal);

    public override int GetValues(object[] values) => throw new NotSupportedException();

    public override T GetFieldValue<T>(int ordinal)
    {
      switch (this._columnTypeCases[ordinal])
      {
        case ShapedBufferedDataRecord.TypeCase.Empty:
          return default (T);
        case ShapedBufferedDataRecord.TypeCase.Bool:
          return (T) (System.ValueType) this.GetBoolean(ordinal);
        case ShapedBufferedDataRecord.TypeCase.Byte:
          return (T) (System.ValueType) this.GetByte(ordinal);
        case ShapedBufferedDataRecord.TypeCase.Char:
          return (T) (System.ValueType) this.GetChar(ordinal);
        case ShapedBufferedDataRecord.TypeCase.DateTime:
          return (T) (System.ValueType) this.GetDateTime(ordinal);
        case ShapedBufferedDataRecord.TypeCase.Decimal:
          return (T) (System.ValueType) this.GetDecimal(ordinal);
        case ShapedBufferedDataRecord.TypeCase.Double:
          return (T) (System.ValueType) this.GetDouble(ordinal);
        case ShapedBufferedDataRecord.TypeCase.Float:
          return (T) (System.ValueType) this.GetFloat(ordinal);
        case ShapedBufferedDataRecord.TypeCase.Guid:
          return (T) (System.ValueType) this.GetGuid(ordinal);
        case ShapedBufferedDataRecord.TypeCase.Short:
          return (T) (System.ValueType) this.GetInt16(ordinal);
        case ShapedBufferedDataRecord.TypeCase.Int:
          return (T) (System.ValueType) this.GetInt32(ordinal);
        case ShapedBufferedDataRecord.TypeCase.Long:
          return (T) (System.ValueType) this.GetInt64(ordinal);
        default:
          return (T) this._objects[this._currentRowNumber * this._objectCount + this._ordinalToIndexMap[ordinal]];
      }
    }

    public override Task<T> GetFieldValueAsync<T>(
      int ordinal,
      CancellationToken cancellationToken)
    {
      return Task.FromResult<T>(this.GetFieldValue<T>(ordinal));
    }

    public override bool IsDBNull(int ordinal) => this._nulls[this._currentRowNumber * this._nullCount + this._nullOrdinalToIndexMap[ordinal]];

    public override Task<bool> IsDBNullAsync(int ordinal, CancellationToken cancellationToken) => Task.FromResult<bool>(this.IsDBNull(ordinal));

    public override bool Read() => this.IsDataReady = ++this._currentRowNumber < this._rowCount;

    public override Task<bool> ReadAsync(CancellationToken cancellationToken) => Task.FromResult<bool>(this.Read());

    private enum TypeCase
    {
      Empty,
      Object,
      Bool,
      Byte,
      Char,
      DateTime,
      Decimal,
      Double,
      Float,
      Guid,
      Short,
      Int,
      Long,
      DbGeography,
      DbGeometry,
    }
  }
}

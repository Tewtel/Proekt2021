// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.BitVec
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Concurrent;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class BitVec
  {
    private const int BitsPerInt32 = 32;
    private const int BytesPerInt32 = 4;
    private const int BitsPerByte = 8;
    public int[] m_array;
    private int m_length;
    private int _version;
    private const int _ShrinkThreshold = 1024;

    private BitVec()
    {
    }

    public BitVec(int length)
      : this(length, false)
    {
    }

    public BitVec(int length, bool defaultValue)
    {
      this.m_array = length >= 0 ? BitVec.ArrayPool.Instance.GetArray(BitVec.GetArrayLength(length, 32)) : throw new ArgumentOutOfRangeException(nameof (length), "ArgumentOutOfRange_NeedNonNegNum");
      this.m_length = length;
      int num = defaultValue ? -1 : 0;
      for (int index = 0; index < this.m_array.Length; ++index)
        this.m_array[index] = num;
      this._version = 0;
    }

    public BitVec(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      if (bytes.Length > 268435455)
        throw new ArgumentException("Argument_ArrayTooLarge", nameof (bytes));
      this.m_array = BitVec.ArrayPool.Instance.GetArray(BitVec.GetArrayLength(bytes.Length, 4));
      this.m_length = bytes.Length * 8;
      int index1 = 0;
      int index2;
      for (index2 = 0; bytes.Length - index2 >= 4; index2 += 4)
        this.m_array[index1++] = (int) bytes[index2] & (int) byte.MaxValue | ((int) bytes[index2 + 1] & (int) byte.MaxValue) << 8 | ((int) bytes[index2 + 2] & (int) byte.MaxValue) << 16 | ((int) bytes[index2 + 3] & (int) byte.MaxValue) << 24;
      switch (bytes.Length - index2)
      {
        case 1:
          this.m_array[index1] |= (int) bytes[index2] & (int) byte.MaxValue;
          break;
        case 2:
          this.m_array[index1] |= ((int) bytes[index2 + 1] & (int) byte.MaxValue) << 8;
          goto case 1;
        case 3:
          this.m_array[index1] = ((int) bytes[index2 + 2] & (int) byte.MaxValue) << 16;
          goto case 2;
      }
      this._version = 0;
    }

    public BitVec(bool[] values)
    {
      if (values == null)
        throw new ArgumentNullException(nameof (values));
      this.m_array = BitVec.ArrayPool.Instance.GetArray(BitVec.GetArrayLength(values.Length, 32));
      this.m_length = values.Length;
      for (int index = 0; index < values.Length; ++index)
      {
        if (values[index])
          this.m_array[index / 32] |= 1 << index % 32;
      }
      this._version = 0;
    }

    public BitVec(int[] values)
    {
      int num = values != null ? values.Length : throw new ArgumentNullException(nameof (values));
      this.m_array = BitVec.ArrayPool.Instance.GetArray(values.Length);
      this.m_length = values.Length * 32;
      Array.Copy((Array) values, (Array) this.m_array, values.Length);
      this._version = 0;
    }

    public BitVec(BitVec bits)
    {
      int length = bits != null ? BitVec.GetArrayLength(bits.m_length, 32) : throw new ArgumentNullException(nameof (bits));
      this.m_array = BitVec.ArrayPool.Instance.GetArray(length);
      this.m_length = bits.m_length;
      Array.Copy((Array) bits.m_array, (Array) this.m_array, length);
      this._version = bits._version;
    }

    public bool this[int index]
    {
      get => this.Get(index);
      set => this.Set(index, value);
    }

    public bool Get(int index)
    {
      if (index < 0 || index >= this.Length)
        throw new ArgumentOutOfRangeException(nameof (index), "ArgumentOutOfRange_Index");
      return (uint) (this.m_array[index / 32] & 1 << index % 32) > 0U;
    }

    public void Set(int index, bool value)
    {
      if (index < 0 || index >= this.Length)
        throw new ArgumentOutOfRangeException(nameof (index), "ArgumentOutOfRange_Index");
      if (value)
        this.m_array[index / 32] |= 1 << index % 32;
      else
        this.m_array[index / 32] &= ~(1 << index % 32);
      ++this._version;
    }

    public void SetAll(bool value)
    {
      int num = value ? -1 : 0;
      int arrayLength = BitVec.GetArrayLength(this.m_length, 32);
      for (int index = 0; index < arrayLength; ++index)
        this.m_array[index] = num;
      ++this._version;
    }

    public BitVec And(BitVec value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (this.Length != value.Length)
        throw new ArgumentException("Arg_ArrayLengthsDiffer");
      int arrayLength = BitVec.GetArrayLength(this.m_length, 32);
      for (int index = 0; index < arrayLength; ++index)
        this.m_array[index] &= value.m_array[index];
      ++this._version;
      return this;
    }

    public BitVec Or(BitVec value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (this.Length != value.Length)
        throw new ArgumentException("Arg_ArrayLengthsDiffer");
      int arrayLength = BitVec.GetArrayLength(this.m_length, 32);
      for (int index = 0; index < arrayLength; ++index)
        this.m_array[index] |= value.m_array[index];
      ++this._version;
      return this;
    }

    public BitVec Xor(BitVec value)
    {
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      if (this.Length != value.Length)
        throw new ArgumentException("Arg_ArrayLengthsDiffer");
      int arrayLength = BitVec.GetArrayLength(this.m_length, 32);
      for (int index = 0; index < arrayLength; ++index)
        this.m_array[index] ^= value.m_array[index];
      ++this._version;
      return this;
    }

    public BitVec Not()
    {
      int arrayLength = BitVec.GetArrayLength(this.m_length, 32);
      for (int index = 0; index < arrayLength; ++index)
        this.m_array[index] = ~this.m_array[index];
      ++this._version;
      return this;
    }

    public int Length
    {
      get => this.m_length;
      set
      {
        int length = value >= 0 ? BitVec.GetArraySize(value, 32) : throw new ArgumentOutOfRangeException(nameof (value), "ArgumentOutOfRange_NeedNonNegNum");
        if (length > this.m_array.Length || length + 1024 < this.m_array.Length)
        {
          int[] array = BitVec.ArrayPool.Instance.GetArray(length);
          Array.Copy((Array) this.m_array, (Array) array, length > this.m_array.Length ? this.m_array.Length : length);
          BitVec.ArrayPool.Instance.PutArray(this.m_array);
          this.m_array = array;
        }
        if (value > this.m_length)
        {
          int index = BitVec.GetArrayLength(this.m_length, 32) - 1;
          int num = this.m_length % 32;
          if (num > 0)
            this.m_array[index] &= (1 << num) - 1;
          Array.Clear((Array) this.m_array, index + 1, length - index - 1);
        }
        this.m_length = value;
        ++this._version;
      }
    }

    /// <summary>
    /// Used for conversion between different representations of bit array.
    /// Returns (n+(div-1))/div, rearranged to avoid arithmetic overflow.
    /// For example, in the bit to int case, the straightforward calc would
    /// be (n+31)/32, but that would cause overflow. So instead it's
    /// rearranged to ((n-1)/32) + 1, with special casing for 0.
    /// 
    /// Usage:
    /// GetArrayLength(77, BitsPerInt32): returns how many ints must be
    /// allocated to store 77 bits.
    /// </summary>
    /// <param name="n">length of array</param>
    /// <param name="div">use a conversion constant, e.g. BytesPerInt32 to get
    /// how many ints are required to store n bytes</param>
    /// <returns>length of the array</returns>
    public static int GetArrayLength(int n, int div) => n <= 0 ? 0 : (n - 1) / div + 1;

    private static int GetArraySize(int n, int div)
    {
      int num1 = (int) Convert.ToUInt32(BitVec.GetArrayLength(n, div)) - 1;
      int num2 = num1 | (int) ((uint) num1 >> 1);
      int num3 = num2 | (int) ((uint) num2 >> 2);
      int num4 = num3 | (int) ((uint) num3 >> 4);
      int num5 = num4 | (int) ((uint) num4 >> 8);
      return Convert.ToInt32((uint) ((num5 | (int) ((uint) num5 >> 16)) + 1));
    }

    private class ArrayPool
    {
      private ConcurrentDictionary<int, ConcurrentBag<int[]>> dictionary;
      private static readonly BitVec.ArrayPool instance = new BitVec.ArrayPool();

      private ArrayPool() => this.dictionary = new ConcurrentDictionary<int, ConcurrentBag<int[]>>();

      public static BitVec.ArrayPool Instance => BitVec.ArrayPool.instance;

      public int[] GetArray(int length)
      {
        int[] result;
        return this.GetBag(length).TryTake(out result) ? result : new int[length];
      }

      private ConcurrentBag<int[]> GetBag(int length) => this.dictionary.GetOrAdd(length, (Func<int, ConcurrentBag<int[]>>) (l => new ConcurrentBag<int[]>()));

      public void PutArray(int[] arr)
      {
        ConcurrentBag<int[]> bag = this.GetBag(arr.Length);
        Array.Clear((Array) arr, 0, arr.Length);
        int[] numArray = arr;
        bag.Add(numArray);
      }
    }
  }
}

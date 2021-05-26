// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.VarVec
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class VarVec : IEnumerable<Var>, IEnumerable
  {
    private readonly BitVec m_bitVector;
    private readonly Command m_command;

    internal void Clear() => this.m_bitVector.Length = 0;

    internal void And(VarVec other)
    {
      this.Align(other);
      this.m_bitVector.And(other.m_bitVector);
    }

    internal void Or(VarVec other)
    {
      this.Align(other);
      this.m_bitVector.Or(other.m_bitVector);
    }

    internal void Minus(VarVec other)
    {
      VarVec varVec = this.m_command.CreateVarVec(other);
      varVec.m_bitVector.Length = this.m_bitVector.Length;
      varVec.m_bitVector.Not();
      this.And(varVec);
      this.m_command.ReleaseVarVec(varVec);
    }

    internal bool Overlaps(VarVec other)
    {
      VarVec varVec = this.m_command.CreateVarVec(other);
      varVec.And(this);
      int num = !varVec.IsEmpty ? 1 : 0;
      this.m_command.ReleaseVarVec(varVec);
      return num != 0;
    }

    internal bool Subsumes(VarVec other)
    {
      int[] array1 = this.m_bitVector.m_array;
      int[] array2 = other.m_bitVector.m_array;
      if (array2.Length > array1.Length)
      {
        for (int length = array1.Length; length < array2.Length; ++length)
        {
          if (array2[length] != 0)
            return false;
        }
      }
      int num = Math.Min(array2.Length, array1.Length);
      for (int index = 0; index < num; ++index)
      {
        if ((array1[index] & array2[index]) != array2[index])
          return false;
      }
      return true;
    }

    internal void InitFrom(VarVec other)
    {
      this.Clear();
      this.m_bitVector.Length = other.m_bitVector.Length;
      this.m_bitVector.Or(other.m_bitVector);
    }

    internal void InitFrom(IEnumerable<Var> other) => this.InitFrom(other, false);

    internal void InitFrom(IEnumerable<Var> other, bool ignoreParameters)
    {
      this.Clear();
      foreach (Var v in other)
      {
        if (!ignoreParameters || v.VarType != VarType.Parameter)
          this.Set(v);
      }
    }

    public IEnumerator<Var> GetEnumerator() => (IEnumerator<Var>) this.m_command.GetVarVecEnumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    internal int Count
    {
      get
      {
        int num = 0;
        foreach (Var var in this)
          ++num;
        return num;
      }
    }

    internal bool IsSet(Var v)
    {
      this.Align(v.Id);
      return this.m_bitVector.Get(v.Id);
    }

    internal void Set(Var v)
    {
      this.Align(v.Id);
      this.m_bitVector.Set(v.Id, true);
    }

    internal void Clear(Var v)
    {
      this.Align(v.Id);
      this.m_bitVector.Set(v.Id, false);
    }

    internal bool IsEmpty => this.First == null;

    internal Var First
    {
      get
      {
        using (IEnumerator<Var> enumerator = this.GetEnumerator())
        {
          if (enumerator.MoveNext())
            return enumerator.Current;
        }
        return (Var) null;
      }
    }

    internal VarVec Remap(IDictionary<Var, Var> varMap)
    {
      VarVec varVec = this.m_command.CreateVarVec();
      foreach (Var key in this)
      {
        Var v;
        if (!varMap.TryGetValue(key, out v))
          v = key;
        varVec.Set(v);
      }
      return varVec;
    }

    internal VarVec(Command command)
    {
      this.m_bitVector = new BitVec(64);
      this.m_command = command;
    }

    private void Align(VarVec other)
    {
      if (other.m_bitVector.Length == this.m_bitVector.Length)
        return;
      if (other.m_bitVector.Length > this.m_bitVector.Length)
        this.m_bitVector.Length = other.m_bitVector.Length;
      else
        other.m_bitVector.Length = this.m_bitVector.Length;
    }

    private void Align(int idx)
    {
      if (idx < this.m_bitVector.Length)
        return;
      this.m_bitVector.Length = idx + 1;
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      string str = string.Empty;
      foreach (Var var in this)
      {
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0}{1}", (object) str, (object) var.Id);
        str = ",";
      }
      return stringBuilder.ToString();
    }

    public VarVec Clone()
    {
      VarVec varVec = this.m_command.CreateVarVec();
      varVec.InitFrom(this);
      return varVec;
    }

    internal class VarVecEnumerator : IEnumerator<Var>, IDisposable, IEnumerator
    {
      private int m_position;
      private Command m_command;
      private BitVec m_bitArray;
      private static readonly int[] MultiplyDeBruijnBitPosition = new int[32]
      {
        0,
        1,
        28,
        2,
        29,
        14,
        24,
        3,
        30,
        22,
        20,
        15,
        25,
        17,
        4,
        8,
        31,
        27,
        13,
        23,
        21,
        19,
        16,
        7,
        26,
        12,
        18,
        6,
        11,
        5,
        10,
        9
      };

      internal VarVecEnumerator(VarVec vec) => this.Init(vec);

      internal void Init(VarVec vec)
      {
        this.m_position = -1;
        this.m_command = vec.m_command;
        this.m_bitArray = vec.m_bitVector;
      }

      public Var Current => this.m_position < 0 || this.m_position >= this.m_bitArray.Length ? (Var) null : this.m_command.GetVar(this.m_position);

      object IEnumerator.Current => (object) this.Current;

      public bool MoveNext()
      {
        int[] array = this.m_bitArray.m_array;
        ++this.m_position;
        int length = this.m_bitArray.Length;
        int arrayLength = BitVec.GetArrayLength(length, 32);
        int index1 = this.m_position / 32;
        if (index1 < arrayLength)
        {
          int num1 = array[index1] & -1 << this.m_position % 32;
          if (num1 != 0)
          {
            this.m_position = index1 * 32 + VarVec.VarVecEnumerator.MultiplyDeBruijnBitPosition[(int) ((uint) ((ulong) (num1 & -num1) * 125613361UL) >> 27)];
            return true;
          }
          for (int index2 = index1 + 1; index2 < arrayLength; ++index2)
          {
            int num2 = array[index2];
            if (num2 != 0)
            {
              this.m_position = index2 * 32 + VarVec.VarVecEnumerator.MultiplyDeBruijnBitPosition[(int) ((uint) ((ulong) (num2 & -num2) * 125613361UL) >> 27)];
              return true;
            }
          }
        }
        this.m_position = length;
        return false;
      }

      public void Reset() => this.m_position = -1;

      public void Dispose()
      {
        GC.SuppressFinalize((object) this);
        this.m_bitArray = (BitVec) null;
        this.m_command.ReleaseVarVecEnumerator(this);
      }
    }
  }
}

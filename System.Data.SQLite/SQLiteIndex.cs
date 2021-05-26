// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteIndex
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Runtime.InteropServices;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents the various inputs and outputs used with the
  /// <see cref="M:System.Data.SQLite.ISQLiteManagedModule.BestIndex(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteIndex)" /> method.
  /// </summary>
  public sealed class SQLiteIndex
  {
    private SQLiteIndexInputs inputs;
    private SQLiteIndexOutputs outputs;

    /// <summary>Constructs an instance of this class.</summary>
    /// <param name="nConstraint">
    /// The number of <see cref="T:System.Data.SQLite.SQLiteIndexConstraint" /> (and
    /// <see cref="T:System.Data.SQLite.SQLiteIndexConstraintUsage" />) instances to
    /// pre-allocate space for.
    /// </param>
    /// <param name="nOrderBy">
    /// The number of <see cref="T:System.Data.SQLite.SQLiteIndexOrderBy" /> instances to
    /// pre-allocate space for.
    /// </param>
    internal SQLiteIndex(int nConstraint, int nOrderBy)
    {
      this.inputs = new SQLiteIndexInputs(nConstraint, nOrderBy);
      this.outputs = new SQLiteIndexOutputs(nConstraint);
    }

    /// <summary>
    /// Attempts to determine the structure sizes needed to create and
    /// populate a native
    /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.sqlite3_index_info" />
    /// structure.
    /// </summary>
    /// <param name="sizeOfInfoType">
    /// The size of the native
    /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.sqlite3_index_info" />
    /// structure is stored here.
    /// </param>
    /// <param name="sizeOfConstraintType">
    /// The size of the native
    /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.sqlite3_index_constraint" />
    /// structure is stored here.
    /// </param>
    /// <param name="sizeOfOrderByType">
    /// The size of the native
    /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.sqlite3_index_orderby" />
    /// structure is stored here.
    /// </param>
    /// <param name="sizeOfConstraintUsageType">
    /// The size of the native
    /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.sqlite3_index_constraint_usage" />
    /// structure is stored here.
    /// </param>
    private static void SizeOfNative(
      out int sizeOfInfoType,
      out int sizeOfConstraintType,
      out int sizeOfOrderByType,
      out int sizeOfConstraintUsageType)
    {
      sizeOfInfoType = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_info));
      sizeOfConstraintType = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_constraint));
      sizeOfOrderByType = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_orderby));
      sizeOfConstraintUsageType = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_constraint_usage));
    }

    /// <summary>
    /// Attempts to allocate and initialize a native
    /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.sqlite3_index_info" />
    /// structure.
    /// </summary>
    /// <param name="nConstraint">
    /// The number of <see cref="T:System.Data.SQLite.SQLiteIndexConstraint" /> instances to
    /// pre-allocate space for.
    /// </param>
    /// <param name="nOrderBy">
    /// The number of <see cref="T:System.Data.SQLite.SQLiteIndexOrderBy" /> instances to
    /// pre-allocate space for.
    /// </param>
    /// <returns>
    /// The newly allocated native
    /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.sqlite3_index_info" /> structure
    /// -OR- <see cref="F:System.IntPtr.Zero" /> if it could not be fully allocated.
    /// </returns>
    private static IntPtr AllocateAndInitializeNative(int nConstraint, int nOrderBy)
    {
      IntPtr num1 = IntPtr.Zero;
      IntPtr num2 = IntPtr.Zero;
      IntPtr pMemory1 = IntPtr.Zero;
      IntPtr pMemory2 = IntPtr.Zero;
      IntPtr pMemory3 = IntPtr.Zero;
      try
      {
        int sizeOfInfoType;
        int sizeOfConstraintType;
        int sizeOfOrderByType;
        int sizeOfConstraintUsageType;
        SQLiteIndex.SizeOfNative(out sizeOfInfoType, out sizeOfConstraintType, out sizeOfOrderByType, out sizeOfConstraintUsageType);
        if (sizeOfInfoType > 0)
        {
          if (sizeOfConstraintType > 0)
          {
            if (sizeOfOrderByType > 0)
            {
              if (sizeOfConstraintUsageType > 0)
              {
                num2 = SQLiteMemory.Allocate(sizeOfInfoType);
                pMemory1 = SQLiteMemory.Allocate(sizeOfConstraintType * nConstraint);
                pMemory2 = SQLiteMemory.Allocate(sizeOfOrderByType * nOrderBy);
                pMemory3 = SQLiteMemory.Allocate(sizeOfConstraintUsageType * nConstraint);
                if (num2 != IntPtr.Zero)
                {
                  if (pMemory1 != IntPtr.Zero)
                  {
                    if (pMemory2 != IntPtr.Zero)
                    {
                      if (pMemory3 != IntPtr.Zero)
                      {
                        int offset1 = 0;
                        SQLiteMarshal.WriteInt32(num2, offset1, nConstraint);
                        int offset2 = SQLiteMarshal.NextOffsetOf(offset1, 4, IntPtr.Size);
                        SQLiteMarshal.WriteIntPtr(num2, offset2, pMemory1);
                        int offset3 = SQLiteMarshal.NextOffsetOf(offset2, IntPtr.Size, 4);
                        SQLiteMarshal.WriteInt32(num2, offset3, nOrderBy);
                        int offset4 = SQLiteMarshal.NextOffsetOf(offset3, 4, IntPtr.Size);
                        SQLiteMarshal.WriteIntPtr(num2, offset4, pMemory2);
                        int offset5 = SQLiteMarshal.NextOffsetOf(offset4, IntPtr.Size, IntPtr.Size);
                        SQLiteMarshal.WriteIntPtr(num2, offset5, pMemory3);
                        num1 = num2;
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      finally
      {
        if (num1 == IntPtr.Zero)
        {
          if (pMemory3 != IntPtr.Zero)
          {
            SQLiteMemory.Free(pMemory3);
            IntPtr zero = IntPtr.Zero;
          }
          if (pMemory2 != IntPtr.Zero)
          {
            SQLiteMemory.Free(pMemory2);
            IntPtr zero = IntPtr.Zero;
          }
          if (pMemory1 != IntPtr.Zero)
          {
            SQLiteMemory.Free(pMemory1);
            IntPtr zero = IntPtr.Zero;
          }
          if (num2 != IntPtr.Zero)
          {
            SQLiteMemory.Free(num2);
            IntPtr zero = IntPtr.Zero;
          }
        }
      }
      return num1;
    }

    /// <summary>
    /// Frees all the memory associated with a native
    /// <see cref="T:System.Data.SQLite.UnsafeNativeMethods.sqlite3_index_info" />
    /// structure.
    /// </summary>
    /// <param name="pIndex">
    /// The native pointer to the native sqlite3_index_info structure to
    /// free.
    /// </param>
    private static void FreeNative(IntPtr pIndex)
    {
      if (pIndex == IntPtr.Zero)
        return;
      int offset1 = SQLiteMarshal.NextOffsetOf(0, 4, IntPtr.Size);
      IntPtr pMemory1 = SQLiteMarshal.ReadIntPtr(pIndex, offset1);
      int offset2 = offset1;
      int offset3 = SQLiteMarshal.NextOffsetOf(SQLiteMarshal.NextOffsetOf(offset1, IntPtr.Size, 4), 4, IntPtr.Size);
      IntPtr pMemory2 = SQLiteMarshal.ReadIntPtr(pIndex, offset3);
      int offset4 = offset3;
      int offset5 = SQLiteMarshal.NextOffsetOf(offset3, IntPtr.Size, IntPtr.Size);
      IntPtr pMemory3 = SQLiteMarshal.ReadIntPtr(pIndex, offset5);
      int offset6 = offset5;
      if (pMemory3 != IntPtr.Zero)
      {
        SQLiteMemory.Free(pMemory3);
        IntPtr zero = IntPtr.Zero;
        SQLiteMarshal.WriteIntPtr(pIndex, offset6, zero);
      }
      if (pMemory2 != IntPtr.Zero)
      {
        SQLiteMemory.Free(pMemory2);
        IntPtr zero = IntPtr.Zero;
        SQLiteMarshal.WriteIntPtr(pIndex, offset4, zero);
      }
      if (pMemory1 != IntPtr.Zero)
      {
        SQLiteMemory.Free(pMemory1);
        IntPtr zero = IntPtr.Zero;
        SQLiteMarshal.WriteIntPtr(pIndex, offset2, zero);
      }
      if (!(pIndex != IntPtr.Zero))
        return;
      SQLiteMemory.Free(pIndex);
      pIndex = IntPtr.Zero;
    }

    /// <summary>
    /// Converts a native pointer to a native sqlite3_index_info structure
    /// into a new <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance.
    /// </summary>
    /// <param name="pIndex">
    /// The native pointer to the native sqlite3_index_info structure to
    /// convert.
    /// </param>
    /// <param name="includeOutput">
    /// Non-zero to include fields from the outputs portion of the native
    /// structure; otherwise, the "output" fields will not be read.
    /// </param>
    /// <param name="index">
    /// Upon success, this parameter will be modified to contain the newly
    /// created <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance.
    /// </param>
    internal static void FromIntPtr(IntPtr pIndex, bool includeOutput, ref SQLiteIndex index)
    {
      if (pIndex == IntPtr.Zero)
        return;
      int offset1 = 0;
      int nConstraint = SQLiteMarshal.ReadInt32(pIndex, offset1);
      int offset2 = SQLiteMarshal.NextOffsetOf(offset1, 4, IntPtr.Size);
      IntPtr pointer1 = SQLiteMarshal.ReadIntPtr(pIndex, offset2);
      int offset3 = SQLiteMarshal.NextOffsetOf(offset2, IntPtr.Size, 4);
      int nOrderBy = SQLiteMarshal.ReadInt32(pIndex, offset3);
      int offset4 = SQLiteMarshal.NextOffsetOf(offset3, 4, IntPtr.Size);
      IntPtr pointer2 = SQLiteMarshal.ReadIntPtr(pIndex, offset4);
      IntPtr pointer3 = IntPtr.Zero;
      if (includeOutput)
      {
        offset4 = SQLiteMarshal.NextOffsetOf(offset4, IntPtr.Size, IntPtr.Size);
        pointer3 = SQLiteMarshal.ReadIntPtr(pIndex, offset4);
      }
      index = new SQLiteIndex(nConstraint, nOrderBy);
      SQLiteIndexInputs inputs = index.Inputs;
      if (inputs == null)
        return;
      SQLiteIndexConstraint[] constraints = inputs.Constraints;
      if (constraints == null)
        return;
      SQLiteIndexOrderBy[] orderBys = inputs.OrderBys;
      if (orderBys == null)
        return;
      Type type1 = typeof (UnsafeNativeMethods.sqlite3_index_constraint);
      int num1 = Marshal.SizeOf(type1);
      for (int index1 = 0; index1 < nConstraint; ++index1)
      {
        UnsafeNativeMethods.sqlite3_index_constraint structure = (UnsafeNativeMethods.sqlite3_index_constraint) Marshal.PtrToStructure(SQLiteMarshal.IntPtrForOffset(pointer1, index1 * num1), type1);
        constraints[index1] = new SQLiteIndexConstraint(structure);
      }
      Type type2 = typeof (UnsafeNativeMethods.sqlite3_index_orderby);
      int num2 = Marshal.SizeOf(type2);
      for (int index1 = 0; index1 < nOrderBy; ++index1)
      {
        UnsafeNativeMethods.sqlite3_index_orderby structure = (UnsafeNativeMethods.sqlite3_index_orderby) Marshal.PtrToStructure(SQLiteMarshal.IntPtrForOffset(pointer2, index1 * num2), type2);
        orderBys[index1] = new SQLiteIndexOrderBy(structure);
      }
      if (!includeOutput)
        return;
      SQLiteIndexOutputs outputs = index.Outputs;
      if (outputs == null)
        return;
      SQLiteIndexConstraintUsage[] constraintUsages = outputs.ConstraintUsages;
      if (constraintUsages == null)
        return;
      Type type3 = typeof (UnsafeNativeMethods.sqlite3_index_constraint_usage);
      int num3 = Marshal.SizeOf(type3);
      for (int index1 = 0; index1 < nConstraint; ++index1)
      {
        UnsafeNativeMethods.sqlite3_index_constraint_usage structure = (UnsafeNativeMethods.sqlite3_index_constraint_usage) Marshal.PtrToStructure(SQLiteMarshal.IntPtrForOffset(pointer3, index1 * num3), type3);
        constraintUsages[index1] = new SQLiteIndexConstraintUsage(structure);
      }
      int offset5 = SQLiteMarshal.NextOffsetOf(offset4, IntPtr.Size, 4);
      outputs.IndexNumber = SQLiteMarshal.ReadInt32(pIndex, offset5);
      int offset6 = SQLiteMarshal.NextOffsetOf(offset5, 4, IntPtr.Size);
      outputs.IndexString = SQLiteString.StringFromUtf8IntPtr(SQLiteMarshal.ReadIntPtr(pIndex, offset6));
      int offset7 = SQLiteMarshal.NextOffsetOf(offset6, IntPtr.Size, 4);
      outputs.NeedToFreeIndexString = SQLiteMarshal.ReadInt32(pIndex, offset7);
      int offset8 = SQLiteMarshal.NextOffsetOf(offset7, 4, 4);
      outputs.OrderByConsumed = SQLiteMarshal.ReadInt32(pIndex, offset8);
      int offset9 = SQLiteMarshal.NextOffsetOf(offset8, 4, 8);
      outputs.EstimatedCost = new double?(SQLiteMarshal.ReadDouble(pIndex, offset9));
      int offset10 = SQLiteMarshal.NextOffsetOf(offset9, 8, 8);
      if (outputs.CanUseEstimatedRows())
        outputs.EstimatedRows = new long?(SQLiteMarshal.ReadInt64(pIndex, offset10));
      int offset11 = SQLiteMarshal.NextOffsetOf(offset10, 8, 4);
      if (outputs.CanUseIndexFlags())
        outputs.IndexFlags = new SQLiteIndexFlags?((SQLiteIndexFlags) SQLiteMarshal.ReadInt32(pIndex, offset11));
      int offset12 = SQLiteMarshal.NextOffsetOf(offset11, 4, 8);
      if (!outputs.CanUseColumnsUsed())
        return;
      outputs.ColumnsUsed = new long?(SQLiteMarshal.ReadInt64(pIndex, offset12));
    }

    /// <summary>
    /// Populates the outputs of a pre-allocated native sqlite3_index_info
    /// structure using an existing <see cref="T:System.Data.SQLite.SQLiteIndex" /> object
    /// instance.
    /// </summary>
    /// <param name="index">
    /// The existing <see cref="T:System.Data.SQLite.SQLiteIndex" /> object instance containing
    /// the output data to use.
    /// </param>
    /// <param name="pIndex">
    /// The native pointer to the pre-allocated native sqlite3_index_info
    /// structure.
    /// </param>
    /// <param name="includeInput">
    /// Non-zero to include fields from the inputs portion of the native
    /// structure; otherwise, the "input" fields will not be written.
    /// </param>
    internal static void ToIntPtr(SQLiteIndex index, IntPtr pIndex, bool includeInput)
    {
      if (index == null)
        return;
      SQLiteIndexOutputs outputs = index.Outputs;
      if (outputs == null)
        return;
      SQLiteIndexConstraintUsage[] constraintUsages = outputs.ConstraintUsages;
      if (constraintUsages == null)
        return;
      SQLiteIndexConstraint[] liteIndexConstraintArray = (SQLiteIndexConstraint[]) null;
      SQLiteIndexOrderBy[] liteIndexOrderByArray = (SQLiteIndexOrderBy[]) null;
      if (includeInput)
      {
        SQLiteIndexInputs inputs = index.Inputs;
        if (inputs == null)
          return;
        liteIndexConstraintArray = inputs.Constraints;
        if (liteIndexConstraintArray == null)
          return;
        liteIndexOrderByArray = inputs.OrderBys;
        if (liteIndexOrderByArray == null)
          return;
      }
      if (pIndex == IntPtr.Zero)
        return;
      int offset1 = 0;
      int num1 = SQLiteMarshal.ReadInt32(pIndex, offset1);
      if (includeInput && num1 != liteIndexConstraintArray.Length || num1 != constraintUsages.Length)
        return;
      int offset2 = SQLiteMarshal.NextOffsetOf(offset1, 4, IntPtr.Size);
      if (includeInput)
      {
        IntPtr pointer = SQLiteMarshal.ReadIntPtr(pIndex, offset2);
        int num2 = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_constraint));
        for (int index1 = 0; index1 < num1; ++index1)
          Marshal.StructureToPtr((object) new UnsafeNativeMethods.sqlite3_index_constraint(liteIndexConstraintArray[index1]), SQLiteMarshal.IntPtrForOffset(pointer, index1 * num2), false);
      }
      int offset3 = SQLiteMarshal.NextOffsetOf(offset2, IntPtr.Size, 4);
      int num3 = includeInput ? SQLiteMarshal.ReadInt32(pIndex, offset3) : 0;
      if (includeInput && num3 != liteIndexOrderByArray.Length)
        return;
      int offset4 = SQLiteMarshal.NextOffsetOf(offset3, 4, IntPtr.Size);
      if (includeInput)
      {
        IntPtr pointer = SQLiteMarshal.ReadIntPtr(pIndex, offset4);
        int num2 = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_orderby));
        for (int index1 = 0; index1 < num3; ++index1)
          Marshal.StructureToPtr((object) new UnsafeNativeMethods.sqlite3_index_orderby(liteIndexOrderByArray[index1]), SQLiteMarshal.IntPtrForOffset(pointer, index1 * num2), false);
      }
      int offset5 = SQLiteMarshal.NextOffsetOf(offset4, IntPtr.Size, IntPtr.Size);
      IntPtr pointer1 = SQLiteMarshal.ReadIntPtr(pIndex, offset5);
      int num4 = Marshal.SizeOf(typeof (UnsafeNativeMethods.sqlite3_index_constraint_usage));
      for (int index1 = 0; index1 < num1; ++index1)
        Marshal.StructureToPtr((object) new UnsafeNativeMethods.sqlite3_index_constraint_usage(constraintUsages[index1]), SQLiteMarshal.IntPtrForOffset(pointer1, index1 * num4), false);
      int offset6 = SQLiteMarshal.NextOffsetOf(offset5, IntPtr.Size, 4);
      SQLiteMarshal.WriteInt32(pIndex, offset6, outputs.IndexNumber);
      int offset7 = SQLiteMarshal.NextOffsetOf(offset6, 4, IntPtr.Size);
      SQLiteMarshal.WriteIntPtr(pIndex, offset7, SQLiteString.Utf8IntPtrFromString(outputs.IndexString, false));
      int offset8 = SQLiteMarshal.NextOffsetOf(offset7, IntPtr.Size, 4);
      int num5 = outputs.NeedToFreeIndexString != 0 ? outputs.NeedToFreeIndexString : 1;
      SQLiteMarshal.WriteInt32(pIndex, offset8, num5);
      int offset9 = SQLiteMarshal.NextOffsetOf(offset8, 4, 4);
      SQLiteMarshal.WriteInt32(pIndex, offset9, outputs.OrderByConsumed);
      int offset10 = SQLiteMarshal.NextOffsetOf(offset9, 4, 8);
      if (outputs.EstimatedCost.HasValue)
        SQLiteMarshal.WriteDouble(pIndex, offset10, outputs.EstimatedCost.GetValueOrDefault());
      int offset11 = SQLiteMarshal.NextOffsetOf(offset10, 8, 8);
      if (outputs.CanUseEstimatedRows() && outputs.EstimatedRows.HasValue)
        SQLiteMarshal.WriteInt64(pIndex, offset11, outputs.EstimatedRows.GetValueOrDefault());
      int offset12 = SQLiteMarshal.NextOffsetOf(offset11, 8, 4);
      if (outputs.CanUseIndexFlags() && outputs.IndexFlags.HasValue)
        SQLiteMarshal.WriteInt32(pIndex, offset12, (int) outputs.IndexFlags.GetValueOrDefault());
      int offset13 = SQLiteMarshal.NextOffsetOf(offset12, 4, 8);
      if (!outputs.CanUseColumnsUsed() || !outputs.ColumnsUsed.HasValue)
        return;
      SQLiteMarshal.WriteInt64(pIndex, offset13, outputs.ColumnsUsed.GetValueOrDefault());
    }

    /// <summary>
    /// The <see cref="T:System.Data.SQLite.SQLiteIndexInputs" /> object instance containing
    /// the inputs to the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.BestIndex(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteIndex)" />
    /// method.
    /// </summary>
    public SQLiteIndexInputs Inputs => this.inputs;

    /// <summary>
    /// The <see cref="T:System.Data.SQLite.SQLiteIndexOutputs" /> object instance containing
    /// the outputs from the <see cref="M:System.Data.SQLite.ISQLiteManagedModule.BestIndex(System.Data.SQLite.SQLiteVirtualTable,System.Data.SQLite.SQLiteIndex)" />
    /// method.
    /// </summary>
    public SQLiteIndexOutputs Outputs => this.outputs;
  }
}

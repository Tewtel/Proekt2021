// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteReadArrayEventArgs
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// This class represents the parameters that are provided
  /// to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" /> and
  /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" /> methods, with
  /// the exception of the column index (provided separately).
  /// </summary>
  public class SQLiteReadArrayEventArgs : SQLiteReadEventArgs
  {
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteReadArrayEventArgs.DataOffset" /> property.
    /// </summary>
    private long dataOffset;
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteReadArrayEventArgs.ByteBuffer" /> property.
    /// </summary>
    private byte[] byteBuffer;
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteReadArrayEventArgs.CharBuffer" /> property.
    /// </summary>
    private char[] charBuffer;
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteReadArrayEventArgs.BufferOffset" /> property.
    /// </summary>
    private int bufferOffset;
    /// <summary>
    /// Provides the underlying storage for the
    /// <see cref="P:System.Data.SQLite.SQLiteReadArrayEventArgs.Length" /> property.
    /// </summary>
    private int length;

    /// <summary>
    /// Constructs an instance of this class to pass into a user-defined
    /// callback associated with the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" />
    /// method.
    /// </summary>
    /// <param name="dataOffset">
    /// The value that was originally specified for the "dataOffset"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" /> methods.
    /// </param>
    /// <param name="byteBuffer">
    /// The value that was originally specified for the "buffer"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" />
    /// method.
    /// </param>
    /// <param name="bufferOffset">
    /// The value that was originally specified for the "bufferOffset"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" /> methods.
    /// </param>
    /// <param name="length">
    /// The value that was originally specified for the "length"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" /> methods.
    /// </param>
    internal SQLiteReadArrayEventArgs(
      long dataOffset,
      byte[] byteBuffer,
      int bufferOffset,
      int length)
    {
      this.dataOffset = dataOffset;
      this.byteBuffer = byteBuffer;
      this.bufferOffset = bufferOffset;
      this.length = length;
    }

    /// <summary>
    /// Constructs an instance of this class to pass into a user-defined
    /// callback associated with the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" />
    /// method.
    /// </summary>
    /// <param name="dataOffset">
    /// The value that was originally specified for the "dataOffset"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" /> methods.
    /// </param>
    /// <param name="charBuffer">
    /// The value that was originally specified for the "buffer"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" />
    /// method.
    /// </param>
    /// <param name="bufferOffset">
    /// The value that was originally specified for the "bufferOffset"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" /> methods.
    /// </param>
    /// <param name="length">
    /// The value that was originally specified for the "length"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" /> methods.
    /// </param>
    internal SQLiteReadArrayEventArgs(
      long dataOffset,
      char[] charBuffer,
      int bufferOffset,
      int length)
    {
      this.dataOffset = dataOffset;
      this.charBuffer = charBuffer;
      this.bufferOffset = bufferOffset;
      this.length = length;
    }

    /// <summary>
    /// The value that was originally specified for the "dataOffset"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" /> methods.
    /// </summary>
    public long DataOffset
    {
      get => this.dataOffset;
      set => this.dataOffset = value;
    }

    /// <summary>
    /// The value that was originally specified for the "buffer"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" />
    /// method.
    /// </summary>
    public byte[] ByteBuffer => this.byteBuffer;

    /// <summary>
    /// The value that was originally specified for the "buffer"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" />
    /// method.
    /// </summary>
    public char[] CharBuffer => this.charBuffer;

    /// <summary>
    /// The value that was originally specified for the "bufferOffset"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" /> methods.
    /// </summary>
    public int BufferOffset
    {
      get => this.bufferOffset;
      set => this.bufferOffset = value;
    }

    /// <summary>
    /// The value that was originally specified for the "length"
    /// parameter to the <see cref="M:System.Data.SQLite.SQLiteDataReader.GetBytes(System.Int32,System.Int64,System.Byte[],System.Int32,System.Int32)" /> or
    /// <see cref="M:System.Data.SQLite.SQLiteDataReader.GetChars(System.Int32,System.Int64,System.Char[],System.Int32,System.Int32)" /> methods.
    /// </summary>
    public int Length
    {
      get => this.length;
      set => this.length = value;
    }
  }
}

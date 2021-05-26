// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonNameTable
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll


#nullable enable
namespace Newtonsoft.Json
{
  /// <summary>Base class for a table of atomized string objects.</summary>
  public abstract class JsonNameTable
  {
    /// <summary>
    /// Gets a string containing the same characters as the specified range of characters in the given array.
    /// </summary>
    /// <param name="key">The character array containing the name to find.</param>
    /// <param name="start">The zero-based index into the array specifying the first character of the name.</param>
    /// <param name="length">The number of characters in the name.</param>
    /// <returns>A string containing the same characters as the specified range of characters in the given array.</returns>
    public abstract string? Get(char[] key, int start, int length);
  }
}

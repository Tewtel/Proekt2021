// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.IDbAsyncEnumerable`1
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Infrastructure
{
  /// <summary>
  /// Asynchronous version of the <see cref="T:System.Collections.Generic.IEnumerable`1" /> interface that allows elements of the enumerable sequence to be retrieved asynchronously.
  /// This interface is used to interact with Entity Framework queries and shouldn't be implemented by custom classes.
  /// </summary>
  /// <typeparam name="T"> The type of objects to enumerate. </typeparam>
  public interface IDbAsyncEnumerable<out T> : IDbAsyncEnumerable
  {
    /// <summary>
    /// Gets an enumerator that can be used to asynchronously enumerate the sequence.
    /// </summary>
    /// <returns> Enumerator for asynchronous enumeration over the sequence. </returns>
    IDbAsyncEnumerator<T> GetAsyncEnumerator();
  }
}

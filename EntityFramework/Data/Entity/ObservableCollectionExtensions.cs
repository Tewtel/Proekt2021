// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.ObservableCollectionExtensions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Internal;

namespace System.Data.Entity
{
  /// <summary>
  /// Extension methods for <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" />.
  /// </summary>
  public static class ObservableCollectionExtensions
  {
    /// <summary>
    /// Returns an <see cref="T:System.ComponentModel.BindingList`1" /> implementation that stays in sync with the given
    /// <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" />.
    /// </summary>
    /// <typeparam name="T"> The element type. </typeparam>
    /// <param name="source"> The collection that the binding list will stay in sync with. </param>
    /// <returns> The binding list. </returns>
    public static BindingList<T> ToBindingList<T>(this ObservableCollection<T> source) where T : class
    {
      System.Data.Entity.Utilities.Check.NotNull<ObservableCollection<T>>(source, nameof (source));
      return !(source is DbLocalView<T> dbLocalView) ? (BindingList<T>) new ObservableBackedBindingList<T>(source) : (BindingList<T>) dbLocalView.BindingList;
    }
  }
}

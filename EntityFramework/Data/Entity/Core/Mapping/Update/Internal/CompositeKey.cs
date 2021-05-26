// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.Update.Internal.CompositeKey
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Common.Utils;

namespace System.Data.Entity.Core.Mapping.Update.Internal
{
  internal class CompositeKey
  {
    internal readonly PropagatorResult[] KeyComponents;

    internal CompositeKey(PropagatorResult[] constants) => this.KeyComponents = constants;

    internal static IEqualityComparer<CompositeKey> CreateComparer(
      KeyManager keyManager)
    {
      return (IEqualityComparer<CompositeKey>) new CompositeKey.CompositeKeyComparer(keyManager);
    }

    internal CompositeKey Merge(KeyManager keyManager, CompositeKey other)
    {
      PropagatorResult[] constants = new PropagatorResult[this.KeyComponents.Length];
      for (int index = 0; index < this.KeyComponents.Length; ++index)
        constants[index] = this.KeyComponents[index].Merge(keyManager, other.KeyComponents[index]);
      return new CompositeKey(constants);
    }

    private class CompositeKeyComparer : IEqualityComparer<CompositeKey>
    {
      private readonly KeyManager _manager;

      internal CompositeKeyComparer(KeyManager manager) => this._manager = manager;

      public bool Equals(CompositeKey left, CompositeKey right)
      {
        if (left == right)
          return true;
        if (left == null || right == null || left.KeyComponents.Length != right.KeyComponents.Length)
          return false;
        for (int index = 0; index < left.KeyComponents.Length; ++index)
        {
          PropagatorResult keyComponent1 = left.KeyComponents[index];
          PropagatorResult keyComponent2 = right.KeyComponents[index];
          if (keyComponent1.Identifier != -1)
          {
            if (keyComponent2.Identifier == -1 || this._manager.GetCliqueIdentifier(keyComponent1.Identifier) != this._manager.GetCliqueIdentifier(keyComponent2.Identifier))
              return false;
          }
          else if (keyComponent2.Identifier != -1 || !ByValueEqualityComparer.Default.Equals(keyComponent1.GetSimpleValue(), keyComponent2.GetSimpleValue()))
            return false;
        }
        return true;
      }

      public int GetHashCode(CompositeKey key)
      {
        int num = 0;
        foreach (PropagatorResult keyComponent in key.KeyComponents)
          num = num << 5 ^ this.GetComponentHashCode(keyComponent);
        return num;
      }

      private int GetComponentHashCode(PropagatorResult keyComponent) => keyComponent.Identifier == -1 ? ByValueEqualityComparer.Default.GetHashCode(keyComponent.GetSimpleValue()) : this._manager.GetCliqueIdentifier(keyComponent.Identifier).GetHashCode();
    }
  }
}

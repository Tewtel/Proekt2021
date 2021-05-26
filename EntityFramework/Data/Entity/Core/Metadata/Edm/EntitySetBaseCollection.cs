// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.EntitySetBaseCollection
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Resources;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal sealed class EntitySetBaseCollection : MetadataCollection<EntitySetBase>
  {
    private readonly EntityContainer _entityContainer;

    internal EntitySetBaseCollection(EntityContainer entityContainer)
      : this(entityContainer, (IEnumerable<EntitySetBase>) null)
    {
    }

    internal EntitySetBaseCollection(
      EntityContainer entityContainer,
      IEnumerable<EntitySetBase> items)
      : base(items)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntityContainer>(entityContainer, nameof (entityContainer));
      this._entityContainer = entityContainer;
    }

    public override EntitySetBase this[int index]
    {
      get => base[index];
      set => throw new InvalidOperationException(Strings.OperationOnReadOnlyCollection);
    }

    public override EntitySetBase this[string identity]
    {
      get => base[identity];
      set => throw new InvalidOperationException(Strings.OperationOnReadOnlyCollection);
    }

    public override void Add(EntitySetBase item)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntitySetBase>(item, nameof (item));
      EntitySetBaseCollection.ThrowIfItHasEntityContainer(item, nameof (item));
      base.Add(item);
      item.ChangeEntityContainerWithoutCollectionFixup(this._entityContainer);
    }

    private static void ThrowIfItHasEntityContainer(EntitySetBase entitySet, string argumentName)
    {
      System.Data.Entity.Utilities.Check.NotNull<EntitySetBase>(entitySet, argumentName);
      if (entitySet.EntityContainer != null)
        throw new ArgumentException(Strings.EntitySetInAnotherContainer, argumentName);
    }
  }
}

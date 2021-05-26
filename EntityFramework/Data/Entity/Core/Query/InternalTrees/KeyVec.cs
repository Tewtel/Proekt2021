﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Query.InternalTrees.KeyVec
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;

namespace System.Data.Entity.Core.Query.InternalTrees
{
  internal class KeyVec
  {
    private readonly VarVec m_keys;
    private bool m_noKeys;

    internal KeyVec(Command itree)
    {
      this.m_keys = itree.CreateVarVec();
      this.m_noKeys = true;
    }

    internal void InitFrom(KeyVec keyset)
    {
      this.m_keys.InitFrom(keyset.m_keys);
      this.m_noKeys = keyset.m_noKeys;
    }

    internal void InitFrom(IEnumerable<Var> varSet) => this.InitFrom(varSet, false);

    internal void InitFrom(IEnumerable<Var> varSet, bool ignoreParameters)
    {
      this.m_keys.InitFrom(varSet, ignoreParameters);
      this.m_noKeys = false;
    }

    internal void InitFrom(KeyVec left, KeyVec right)
    {
      if (left.m_noKeys || right.m_noKeys)
      {
        this.m_noKeys = true;
      }
      else
      {
        this.m_noKeys = false;
        this.m_keys.InitFrom(left.m_keys);
        this.m_keys.Or(right.m_keys);
      }
    }

    internal void InitFrom(List<KeyVec> keyVecList)
    {
      this.m_noKeys = false;
      this.m_keys.Clear();
      foreach (KeyVec keyVec in keyVecList)
      {
        if (keyVec.m_noKeys)
        {
          this.m_noKeys = true;
          break;
        }
        this.m_keys.Or(keyVec.m_keys);
      }
    }

    internal void Clear()
    {
      this.m_noKeys = true;
      this.m_keys.Clear();
    }

    internal VarVec KeyVars => this.m_keys;

    internal bool NoKeys
    {
      get => this.m_noKeys;
      set => this.m_noKeys = value;
    }
  }
}

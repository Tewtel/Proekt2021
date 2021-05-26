﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver
// Assembly: Newtonsoft.Json, Version=12.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 2676A2DA-6EDC-420E-890E-D28AA4572EE5
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;


#nullable enable
namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Resolves member mappings for a type, camel casing property names.
  /// </summary>
  public class CamelCasePropertyNamesContractResolver : DefaultContractResolver
  {
    private static readonly object TypeContractCacheLock = new object();
    private static readonly DefaultJsonNameTable NameTable = new DefaultJsonNameTable();
    private static Dictionary<StructMultiKey<Type, Type>, JsonContract>? _contractCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver" /> class.
    /// </summary>
    public CamelCasePropertyNamesContractResolver()
    {
      CamelCaseNamingStrategy caseNamingStrategy = new CamelCaseNamingStrategy();
      caseNamingStrategy.ProcessDictionaryKeys = true;
      caseNamingStrategy.OverrideSpecifiedNames = true;
      this.NamingStrategy = (NamingStrategy) caseNamingStrategy;
    }

    /// <summary>Resolves the contract for a given type.</summary>
    /// <param name="type">The type to resolve a contract for.</param>
    /// <returns>The contract for a given type.</returns>
    public override JsonContract ResolveContract(Type type)
    {
      StructMultiKey<Type, Type> key = !(type == (Type) null) ? new StructMultiKey<Type, Type>(this.GetType(), type) : throw new ArgumentNullException(nameof (type));
      Dictionary<StructMultiKey<Type, Type>, JsonContract> contractCache1 = CamelCasePropertyNamesContractResolver._contractCache;
      JsonContract contract;
      if (contractCache1 == null || !contractCache1.TryGetValue(key, out contract))
      {
        contract = this.CreateContract(type);
        lock (CamelCasePropertyNamesContractResolver.TypeContractCacheLock)
        {
          Dictionary<StructMultiKey<Type, Type>, JsonContract> contractCache2 = CamelCasePropertyNamesContractResolver._contractCache;
          Dictionary<StructMultiKey<Type, Type>, JsonContract> dictionary = contractCache2 != null ? new Dictionary<StructMultiKey<Type, Type>, JsonContract>((IDictionary<StructMultiKey<Type, Type>, JsonContract>) contractCache2) : new Dictionary<StructMultiKey<Type, Type>, JsonContract>();
          dictionary[key] = contract;
          CamelCasePropertyNamesContractResolver._contractCache = dictionary;
        }
      }
      return contract;
    }

    internal override DefaultJsonNameTable GetNameTable() => CamelCasePropertyNamesContractResolver.NameTable;
  }
}

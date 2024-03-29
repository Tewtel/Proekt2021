﻿// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.Internal.SerializableImplementor
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Utilities;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Security;

namespace System.Data.Entity.Core.Objects.Internal
{
  internal sealed class SerializableImplementor
  {
    private readonly Type _baseClrType;
    private readonly bool _baseImplementsISerializable;
    private readonly bool _canOverride;
    private readonly MethodInfo _getObjectDataMethod;
    private readonly ConstructorInfo _serializationConstructor;
    internal static readonly MethodInfo GetTypeFromHandleMethod = typeof (Type).GetDeclaredMethod("GetTypeFromHandle", typeof (RuntimeTypeHandle));
    internal static readonly MethodInfo AddValueMethod = typeof (SerializationInfo).GetDeclaredMethod("AddValue", typeof (string), typeof (object), typeof (Type));
    internal static readonly MethodInfo GetValueMethod = typeof (SerializationInfo).GetDeclaredMethod("GetValue", typeof (string), typeof (Type));

    internal SerializableImplementor(EntityType ospaceEntityType)
    {
      this._baseClrType = ospaceEntityType.ClrType;
      this._baseImplementsISerializable = this._baseClrType.IsSerializable() && typeof (ISerializable).IsAssignableFrom(this._baseClrType);
      if (!this._baseImplementsISerializable)
        return;
      this._getObjectDataMethod = this._baseClrType.GetInterfaceMap(typeof (ISerializable)).TargetMethods[0];
      if ((!this._getObjectDataMethod.IsVirtual || this._getObjectDataMethod.IsFinal ? 0 : (this._getObjectDataMethod.IsPublic ? 1 : 0)) == 0)
        return;
      this._serializationConstructor = this._baseClrType.GetDeclaredConstructor((Func<ConstructorInfo, bool>) (c => c.IsPublic || c.IsFamily || c.IsFamilyOrAssembly), new Type[2]
      {
        typeof (SerializationInfo),
        typeof (StreamingContext)
      }, new Type[2]
      {
        typeof (SerializationInfo),
        typeof (object)
      }, new Type[2]
      {
        typeof (object),
        typeof (StreamingContext)
      }, new Type[2]{ typeof (object), typeof (object) });
      this._canOverride = this._serializationConstructor != (ConstructorInfo) null;
    }

    internal bool TypeIsSuitable => !this._baseImplementsISerializable || this._canOverride;

    internal bool TypeImplementsISerializable => this._baseImplementsISerializable;

    internal void Implement(TypeBuilder typeBuilder, IEnumerable<FieldBuilder> serializedFields)
    {
      if (!this._baseImplementsISerializable || !this._canOverride)
        return;
      Type[] parameterTypes = new Type[2]
      {
        typeof (SerializationInfo),
        typeof (StreamingContext)
      };
      MethodBuilder methodBuilder = typeBuilder.DefineMethod(this._getObjectDataMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, (Type) null, parameterTypes);
      methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof (SecurityCriticalAttribute).GetDeclaredConstructor(), new object[0]));
      ILGenerator ilGenerator1 = methodBuilder.GetILGenerator();
      foreach (FieldBuilder serializedField in serializedFields)
      {
        ilGenerator1.Emit(OpCodes.Ldarg_1);
        ilGenerator1.Emit(OpCodes.Ldstr, serializedField.Name);
        ilGenerator1.Emit(OpCodes.Ldarg_0);
        ilGenerator1.Emit(OpCodes.Ldfld, (FieldInfo) serializedField);
        ilGenerator1.Emit(OpCodes.Ldtoken, serializedField.FieldType);
        ilGenerator1.Emit(OpCodes.Call, SerializableImplementor.GetTypeFromHandleMethod);
        ilGenerator1.Emit(OpCodes.Callvirt, SerializableImplementor.AddValueMethod);
      }
      ilGenerator1.Emit(OpCodes.Ldarg_0);
      ilGenerator1.Emit(OpCodes.Ldarg_1);
      ilGenerator1.Emit(OpCodes.Ldarg_2);
      ilGenerator1.Emit(OpCodes.Call, this._getObjectDataMethod);
      ilGenerator1.Emit(OpCodes.Ret);
      MethodAttributes attributes = (MethodAttributes) (6272 | (this._serializationConstructor.IsPublic ? 6 : 1));
      ILGenerator ilGenerator2 = typeBuilder.DefineConstructor(attributes, CallingConventions.Standard | CallingConventions.HasThis, parameterTypes).GetILGenerator();
      ilGenerator2.Emit(OpCodes.Ldarg_0);
      ilGenerator2.Emit(OpCodes.Ldarg_1);
      ilGenerator2.Emit(OpCodes.Ldarg_2);
      ilGenerator2.Emit(OpCodes.Call, this._serializationConstructor);
      foreach (FieldBuilder serializedField in serializedFields)
      {
        ilGenerator2.Emit(OpCodes.Ldarg_0);
        ilGenerator2.Emit(OpCodes.Ldarg_1);
        ilGenerator2.Emit(OpCodes.Ldstr, serializedField.Name);
        ilGenerator2.Emit(OpCodes.Ldtoken, serializedField.FieldType);
        ilGenerator2.Emit(OpCodes.Call, SerializableImplementor.GetTypeFromHandleMethod);
        ilGenerator2.Emit(OpCodes.Callvirt, SerializableImplementor.GetValueMethod);
        ilGenerator2.Emit(OpCodes.Castclass, serializedField.FieldType);
        ilGenerator2.Emit(OpCodes.Stfld, (FieldInfo) serializedField);
      }
      ilGenerator2.Emit(OpCodes.Ret);
    }
  }
}

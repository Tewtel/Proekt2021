// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.DependencyResolution.ClrTypeAnnotationSerializer
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.IO;
using System.Reflection;

namespace System.Data.Entity.Infrastructure.DependencyResolution
{
  internal class ClrTypeAnnotationSerializer : IMetadataAnnotationSerializer
  {
    public string Serialize(string name, object value) => ((Type) value).AssemblyQualifiedName;

    public object Deserialize(string name, string value)
    {
      try
      {
        return (object) Type.GetType(value, false);
      }
      catch (FileLoadException ex)
      {
      }
      catch (TargetInvocationException ex)
      {
      }
      catch (BadImageFormatException ex)
      {
      }
      return (object) null;
    }
  }
}

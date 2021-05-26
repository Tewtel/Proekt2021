// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Metadata.Edm.LoadMessageLogger
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Resources;
using System.Text;

namespace System.Data.Entity.Core.Metadata.Edm
{
  internal class LoadMessageLogger
  {
    private readonly Action<string> _logLoadMessage;
    private readonly Dictionary<EdmType, StringBuilder> _messages = new Dictionary<EdmType, StringBuilder>();

    internal LoadMessageLogger(Action<string> logLoadMessage) => this._logLoadMessage = logLoadMessage;

    internal virtual void LogLoadMessage(string message, EdmType relatedType)
    {
      if (this._logLoadMessage != null)
        this._logLoadMessage(message);
      this.LogMessagesWithTypeInfo(message, relatedType);
    }

    internal virtual string CreateErrorMessageWithTypeSpecificLoadLogs(
      string errorMessage,
      EdmType relatedType)
    {
      return new StringBuilder(errorMessage).AppendLine(this.GetTypeRelatedLogMessage(relatedType)).ToString();
    }

    private string GetTypeRelatedLogMessage(EdmType relatedType) => this._messages.ContainsKey(relatedType) ? new StringBuilder().AppendLine().AppendLine(Strings.ExtraInfo).AppendLine(this._messages[relatedType].ToString()).ToString() : string.Empty;

    private void LogMessagesWithTypeInfo(string message, EdmType relatedType)
    {
      if (this._messages.ContainsKey(relatedType))
        this._messages[relatedType].AppendLine(message);
      else
        this._messages.Add(relatedType, new StringBuilder(message));
    }
  }
}

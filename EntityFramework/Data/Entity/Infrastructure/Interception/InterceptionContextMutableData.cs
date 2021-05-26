// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Infrastructure.Interception.InterceptionContextMutableData
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Data.Entity.Resources;
using System.Threading.Tasks;

namespace System.Data.Entity.Infrastructure.Interception
{
  internal class InterceptionContextMutableData
  {
    private const string LegacyUserState = "__LegacyUserState__";
    private Exception _exception;
    private bool _isSuppressed;
    private IDictionary<string, object> _userStateMap;

    public bool HasExecuted { get; set; }

    public Exception OriginalException { get; set; }

    public TaskStatus TaskStatus { get; set; }

    private IDictionary<string, object> UserStateMap
    {
      get
      {
        if (this._userStateMap == null)
          this._userStateMap = (IDictionary<string, object>) new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.Ordinal);
        return this._userStateMap;
      }
    }

    [Obsolete("Not safe when multiple interceptors are in use. Use SetUserState and FindUserState instead.")]
    public object UserState
    {
      get => this.FindUserState("__LegacyUserState__");
      set => this.SetUserState("__LegacyUserState__", value);
    }

    public object FindUserState(string key)
    {
      object obj;
      return this._userStateMap == null || !this.UserStateMap.TryGetValue(key, out obj) ? (object) null : obj;
    }

    public void SetUserState(string key, object value) => this.UserStateMap[key] = value;

    public bool IsExecutionSuppressed => this._isSuppressed;

    public void SuppressExecution() => this._isSuppressed = this._isSuppressed || !this.HasExecuted ? true : throw new InvalidOperationException(Strings.SuppressionAfterExecution);

    public Exception Exception
    {
      get => this._exception;
      set
      {
        if (!this.HasExecuted)
          this.SuppressExecution();
        this._exception = value;
      }
    }

    public void SetExceptionThrown(Exception exception)
    {
      this.HasExecuted = true;
      this.OriginalException = exception;
      this.Exception = exception;
    }
  }
}

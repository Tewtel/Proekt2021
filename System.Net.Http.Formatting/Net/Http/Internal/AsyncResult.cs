// Decompiled with JetBrains decompiler
// Type: System.Net.Http.Internal.AsyncResult
// Assembly: System.Net.Http.Formatting, Version=5.2.7.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: BFDADC3B-0C0A-4D10-B981-98C04ECCD25D
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Net.Http.Formatting.dll

using System.Net.Http.Properties;
using System.Threading;
using System.Web.Http;

namespace System.Net.Http.Internal
{
  internal abstract class AsyncResult : IAsyncResult
  {
    private AsyncCallback _callback;
    private object _state;
    private bool _isCompleted;
    private bool _completedSynchronously;
    private bool _endCalled;
    private Exception _exception;

    protected AsyncResult(AsyncCallback callback, object state)
    {
      this._callback = callback;
      this._state = state;
    }

    public object AsyncState => this._state;

    public WaitHandle AsyncWaitHandle => (WaitHandle) null;

    public bool CompletedSynchronously => this._completedSynchronously;

    public bool HasCallback => this._callback != null;

    public bool IsCompleted => this._isCompleted;

    protected void Complete(bool completedSynchronously)
    {
      if (this._isCompleted)
        throw Error.InvalidOperation(Resources.AsyncResult_MultipleCompletes, (object) this.GetType().Name);
      this._completedSynchronously = completedSynchronously;
      this._isCompleted = true;
      if (this._callback == null)
        return;
      try
      {
        this._callback((IAsyncResult) this);
      }
      catch (Exception ex)
      {
        string callbackThrewException = Resources.AsyncResult_CallbackThrewException;
        object[] objArray = new object[0];
        throw Error.InvalidOperation(ex, callbackThrewException, objArray);
      }
    }

    protected void Complete(bool completedSynchronously, Exception exception)
    {
      this._exception = exception;
      this.Complete(completedSynchronously);
    }

    protected static TAsyncResult End<TAsyncResult>(IAsyncResult result) where TAsyncResult : AsyncResult
    {
      if (result == null)
        throw Error.ArgumentNull(nameof (result));
      if (!(result is TAsyncResult asyncResult))
        throw Error.Argument(nameof (result), Resources.AsyncResult_ResultMismatch);
      if (!asyncResult._isCompleted)
        asyncResult.AsyncWaitHandle.WaitOne();
      asyncResult._endCalled = !asyncResult._endCalled ? true : throw Error.InvalidOperation(Resources.AsyncResult_MultipleEnds);
      return asyncResult._exception == null ? asyncResult : throw asyncResult._exception;
    }
  }
}

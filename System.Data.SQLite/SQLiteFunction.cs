// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteFunction
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Data.SQLite
{
  /// <summary>
  /// This abstract class is designed to handle user-defined functions easily.  An instance of the derived class is made for each
  /// connection to the database.
  /// </summary>
  /// <remarks>
  /// Although there is one instance of a class derived from SQLiteFunction per database connection, the derived class has no access
  /// to the underlying connection.  This is necessary to deter implementers from thinking it would be a good idea to make database
  /// calls during processing.
  /// 
  /// It is important to distinguish between a per-connection instance, and a per-SQL statement context.  One instance of this class
  /// services all SQL statements being stepped through on that connection, and there can be many.  One should never store per-statement
  /// information in member variables of user-defined function classes.
  /// 
  /// For aggregate functions, always create and store your per-statement data in the contextData object on the 1st step.  This data will
  /// be automatically freed for you (and Dispose() called if the item supports IDisposable) when the statement completes.
  /// </remarks>
  public abstract class SQLiteFunction : IDisposable
  {
    /// <summary>The base connection this function is attached to</summary>
    internal SQLiteBase _base;
    /// <summary>
    /// Internal array used to keep track of aggregate function context data
    /// </summary>
    private Dictionary<IntPtr, SQLiteFunction.AggregateData> _contextDataList;
    /// <summary>
    /// The connection flags associated with this object (this should be the
    /// same value as the flags associated with the parent connection object).
    /// </summary>
    private SQLiteConnectionFlags _flags;
    /// <summary>
    /// Holds a reference to the callback function for user functions
    /// </summary>
    private SQLiteCallback _InvokeFunc;
    /// <summary>
    /// Holds a reference to the callbakc function for stepping in an aggregate function
    /// </summary>
    private SQLiteCallback _StepFunc;
    /// <summary>
    /// Holds a reference to the callback function for finalizing an aggregate function
    /// </summary>
    private SQLiteFinalCallback _FinalFunc;
    /// <summary>
    /// Holds a reference to the callback function for collating sequences
    /// </summary>
    private SQLiteCollation _CompareFunc;
    private SQLiteCollation _CompareFunc16;
    /// <summary>
    /// Current context of the current callback.  Only valid during a callback
    /// </summary>
    internal IntPtr _context;
    /// <summary>
    /// This static dictionary contains all the registered (known) user-defined
    /// functions declared using the proper attributes.  The contained dictionary
    /// values are always null and are not currently used.
    /// </summary>
    private static IDictionary<SQLiteFunctionAttribute, object> _registeredFunctions = (IDictionary<SQLiteFunctionAttribute, object>) new Dictionary<SQLiteFunctionAttribute, object>();
    private bool disposed;

    /// <summary>
    /// Internal constructor, initializes the function's internal variables.
    /// </summary>
    protected SQLiteFunction() => this._contextDataList = new Dictionary<IntPtr, SQLiteFunction.AggregateData>();

    /// <summary>
    /// Constructs an instance of this class using the specified data-type
    /// conversion parameters.
    /// </summary>
    /// <param name="format">
    /// The DateTime format to be used when converting string values to a
    /// DateTime and binding DateTime parameters.
    /// </param>
    /// <param name="kind">
    /// The <see cref="T:System.DateTimeKind" /> to be used when creating DateTime
    /// values.
    /// </param>
    /// <param name="formatString">
    /// The format string to be used when parsing and formatting DateTime
    /// values.
    /// </param>
    /// <param name="utf16">
    /// Non-zero to create a UTF-16 data-type conversion context; otherwise,
    /// a UTF-8 data-type conversion context will be created.
    /// </param>
    protected SQLiteFunction(
      SQLiteDateFormats format,
      DateTimeKind kind,
      string formatString,
      bool utf16)
      : this()
    {
      if (utf16)
        this._base = (SQLiteBase) new SQLite3_UTF16(format, kind, formatString, IntPtr.Zero, (string) null, false);
      else
        this._base = (SQLiteBase) new SQLite3(format, kind, formatString, IntPtr.Zero, (string) null, false);
    }

    /// <summary>
    /// Disposes of any active contextData variables that were not automatically cleaned up.  Sometimes this can happen if
    /// someone closes the connection while a DataReader is open.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void CheckDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(typeof (SQLiteFunction).Name);
    }

    /// <summary>Placeholder for a user-defined disposal routine</summary>
    /// <param name="disposing">True if the object is being disposed explicitly</param>
    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (disposing)
      {
        foreach (KeyValuePair<IntPtr, SQLiteFunction.AggregateData> contextData in this._contextDataList)
        {
          if (contextData.Value._data is IDisposable data3)
            data3.Dispose();
        }
        this._contextDataList.Clear();
        this._contextDataList = (Dictionary<IntPtr, SQLiteFunction.AggregateData>) null;
        this._flags = SQLiteConnectionFlags.None;
        this._InvokeFunc = (SQLiteCallback) null;
        this._StepFunc = (SQLiteCallback) null;
        this._FinalFunc = (SQLiteFinalCallback) null;
        this._CompareFunc = (SQLiteCollation) null;
        this._base = (SQLiteBase) null;
      }
      this.disposed = true;
    }

    /// <summary>
    /// Cleans up resources associated with the current instance.
    /// </summary>
    ~SQLiteFunction() => this.Dispose(false);

    /// <summary>
    /// Returns a reference to the underlying connection's SQLiteConvert class, which can be used to convert
    /// strings and DateTime's into the current connection's encoding schema.
    /// </summary>
    public SQLiteConvert SQLiteConvert
    {
      get
      {
        this.CheckDisposed();
        return (SQLiteConvert) this._base;
      }
    }

    /// <summary>
    /// Scalar functions override this method to do their magic.
    /// </summary>
    /// <remarks>
    /// Parameters passed to functions have only an affinity for a certain data type, there is no underlying schema available
    /// to force them into a certain type.  Therefore the only types you will ever see as parameters are
    /// DBNull.Value, Int64, Double, String or byte[] array.
    /// </remarks>
    /// <param name="args">The arguments for the command to process</param>
    /// <returns>You may return most simple types as a return value, null or DBNull.Value to return null, DateTime, or
    /// you may return an Exception-derived class if you wish to return an error to SQLite.  Do not actually throw the error,
    /// just return it!</returns>
    public virtual object Invoke(object[] args)
    {
      this.CheckDisposed();
      return (object) null;
    }

    /// <summary>
    /// Aggregate functions override this method to do their magic.
    /// </summary>
    /// <remarks>
    /// Typically you'll be updating whatever you've placed in the contextData field and returning as quickly as possible.
    /// </remarks>
    /// <param name="args">The arguments for the command to process</param>
    /// <param name="stepNumber">The 1-based step number.  This is incrememted each time the step method is called.</param>
    /// <param name="contextData">A placeholder for implementers to store contextual data pertaining to the current context.</param>
    public virtual void Step(object[] args, int stepNumber, ref object contextData) => this.CheckDisposed();

    /// <summary>
    /// Aggregate functions override this method to finish their aggregate processing.
    /// </summary>
    /// <remarks>
    /// If you implemented your aggregate function properly,
    /// you've been recording and keeping track of your data in the contextData object provided, and now at this stage you should have
    /// all the information you need in there to figure out what to return.
    /// NOTE:  It is possible to arrive here without receiving a previous call to Step(), in which case the contextData will
    /// be null.  This can happen when no rows were returned.  You can either return null, or 0 or some other custom return value
    /// if that is the case.
    /// </remarks>
    /// <param name="contextData">Your own assigned contextData, provided for you so you can return your final results.</param>
    /// <returns>You may return most simple types as a return value, null or DBNull.Value to return null, DateTime, or
    /// you may return an Exception-derived class if you wish to return an error to SQLite.  Do not actually throw the error,
    /// just return it!
    /// </returns>
    public virtual object Final(object contextData)
    {
      this.CheckDisposed();
      return (object) null;
    }

    /// <summary>
    /// User-defined collating sequences override this method to provide a custom string sorting algorithm.
    /// </summary>
    /// <param name="param1">The first string to compare.</param>
    /// <param name="param2">The second strnig to compare.</param>
    /// <returns>1 if param1 is greater than param2, 0 if they are equal, or -1 if param1 is less than param2.</returns>
    public virtual int Compare(string param1, string param2)
    {
      this.CheckDisposed();
      return 0;
    }

    /// <summary>
    /// Converts an IntPtr array of context arguments to an object array containing the resolved parameters the pointers point to.
    /// </summary>
    /// <remarks>
    /// Parameters passed to functions have only an affinity for a certain data type, there is no underlying schema available
    /// to force them into a certain type.  Therefore the only types you will ever see as parameters are
    /// DBNull.Value, Int64, Double, String or byte[] array.
    /// </remarks>
    /// <param name="nArgs">The number of arguments</param>
    /// <param name="argsptr">A pointer to the array of arguments</param>
    /// <returns>An object array of the arguments once they've been converted to .NET values</returns>
    internal object[] ConvertParams(int nArgs, IntPtr argsptr)
    {
      object[] objArray = new object[nArgs];
      IntPtr[] destination = new IntPtr[nArgs];
      Marshal.Copy(argsptr, destination, 0, nArgs);
      for (int index = 0; index < nArgs; ++index)
      {
        switch (this._base.GetParamValueType(destination[index]))
        {
          case TypeAffinity.Int64:
            objArray[index] = (object) this._base.GetParamValueInt64(destination[index]);
            break;
          case TypeAffinity.Double:
            objArray[index] = (object) this._base.GetParamValueDouble(destination[index]);
            break;
          case TypeAffinity.Text:
            objArray[index] = (object) this._base.GetParamValueText(destination[index]);
            break;
          case TypeAffinity.Blob:
            int paramValueBytes = (int) this._base.GetParamValueBytes(destination[index], 0, (byte[]) null, 0, 0);
            byte[] bDest = new byte[paramValueBytes];
            this._base.GetParamValueBytes(destination[index], 0, bDest, 0, paramValueBytes);
            objArray[index] = (object) bDest;
            break;
          case TypeAffinity.Null:
            objArray[index] = (object) DBNull.Value;
            break;
          case TypeAffinity.DateTime:
            objArray[index] = (object) this._base.ToDateTime(this._base.GetParamValueText(destination[index]));
            break;
        }
      }
      return objArray;
    }

    /// <summary>
    /// Takes the return value from Invoke() and Final() and figures out how to return it to SQLite's context.
    /// </summary>
    /// <param name="context">The context the return value applies to</param>
    /// <param name="returnValue">The parameter to return to SQLite</param>
    private void SetReturnValue(IntPtr context, object returnValue)
    {
      if (returnValue == null || returnValue == DBNull.Value)
      {
        this._base.ReturnNull(context);
      }
      else
      {
        Type type = returnValue.GetType();
        if (type == typeof (DateTime))
          this._base.ReturnText(context, this._base.ToString((DateTime) returnValue));
        else if (returnValue is Exception exception4)
        {
          this._base.ReturnError(context, exception4.Message);
        }
        else
        {
          switch (SQLiteConvert.TypeToAffinity(type, this._flags))
          {
            case TypeAffinity.Int64:
              this._base.ReturnInt64(context, Convert.ToInt64(returnValue, (IFormatProvider) CultureInfo.CurrentCulture));
              break;
            case TypeAffinity.Double:
              this._base.ReturnDouble(context, Convert.ToDouble(returnValue, (IFormatProvider) CultureInfo.CurrentCulture));
              break;
            case TypeAffinity.Text:
              this._base.ReturnText(context, returnValue.ToString());
              break;
            case TypeAffinity.Blob:
              this._base.ReturnBlob(context, (byte[]) returnValue);
              break;
            case TypeAffinity.Null:
              this._base.ReturnNull(context);
              break;
          }
        }
      }
    }

    /// <summary>
    /// Internal scalar callback function, which wraps the raw context pointer and calls the virtual Invoke() method.
    /// WARNING: Must not throw exceptions.
    /// </summary>
    /// <param name="context">A raw context pointer</param>
    /// <param name="nArgs">Number of arguments passed in</param>
    /// <param name="argsptr">A pointer to the array of arguments</param>
    internal void ScalarCallback(IntPtr context, int nArgs, IntPtr argsptr)
    {
      try
      {
        this._context = context;
        this.SetReturnValue(context, this.Invoke(this.ConvertParams(nArgs, argsptr)));
      }
      catch (Exception ex)
      {
        try
        {
          if (!HelperMethods.LogCallbackExceptions(this._flags))
            return;
          SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "Invoke", (object) ex));
        }
        catch
        {
        }
      }
    }

    /// <summary>
    /// Internal collating sequence function, which wraps up the raw string pointers and executes the Compare() virtual function.
    /// WARNING: Must not throw exceptions.
    /// </summary>
    /// <param name="ptr">Not used</param>
    /// <param name="len1">Length of the string pv1</param>
    /// <param name="ptr1">Pointer to the first string to compare</param>
    /// <param name="len2">Length of the string pv2</param>
    /// <param name="ptr2">Pointer to the second string to compare</param>
    /// <returns>Returns -1 if the first string is less than the second.  0 if they are equal, or 1 if the first string is greater
    /// than the second.  Returns 0 if an exception is caught.</returns>
    internal int CompareCallback(IntPtr ptr, int len1, IntPtr ptr1, int len2, IntPtr ptr2)
    {
      try
      {
        return this.Compare(SQLiteConvert.UTF8ToString(ptr1, len1), SQLiteConvert.UTF8ToString(ptr2, len2));
      }
      catch (Exception ex)
      {
        try
        {
          if (HelperMethods.LogCallbackExceptions(this._flags))
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "Compare", (object) ex));
        }
        catch
        {
        }
      }
      if (this._base != null && this._base.IsOpen())
        this._base.Cancel();
      return 0;
    }

    /// <summary>
    /// Internal collating sequence function, which wraps up the raw string pointers and executes the Compare() virtual function.
    /// WARNING: Must not throw exceptions.
    /// </summary>
    /// <param name="ptr">Not used</param>
    /// <param name="len1">Length of the string pv1</param>
    /// <param name="ptr1">Pointer to the first string to compare</param>
    /// <param name="len2">Length of the string pv2</param>
    /// <param name="ptr2">Pointer to the second string to compare</param>
    /// <returns>Returns -1 if the first string is less than the second.  0 if they are equal, or 1 if the first string is greater
    /// than the second.  Returns 0 if an exception is caught.</returns>
    internal int CompareCallback16(IntPtr ptr, int len1, IntPtr ptr1, int len2, IntPtr ptr2)
    {
      try
      {
        return this.Compare(SQLite3_UTF16.UTF16ToString(ptr1, len1), SQLite3_UTF16.UTF16ToString(ptr2, len2));
      }
      catch (Exception ex)
      {
        try
        {
          if (HelperMethods.LogCallbackExceptions(this._flags))
            SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "Compare (UTF16)", (object) ex));
        }
        catch
        {
        }
      }
      if (this._base != null && this._base.IsOpen())
        this._base.Cancel();
      return 0;
    }

    /// <summary>
    /// The internal aggregate Step function callback, which wraps the raw context pointer and calls the virtual Step() method.
    /// WARNING: Must not throw exceptions.
    /// </summary>
    /// <remarks>
    /// This function takes care of doing the lookups and getting the important information put together to call the Step() function.
    /// That includes pulling out the user's contextData and updating it after the call is made.  We use a sorted list for this so
    /// binary searches can be done to find the data.
    /// </remarks>
    /// <param name="context">A raw context pointer</param>
    /// <param name="nArgs">Number of arguments passed in</param>
    /// <param name="argsptr">A pointer to the array of arguments</param>
    internal void StepCallback(IntPtr context, int nArgs, IntPtr argsptr)
    {
      try
      {
        SQLiteFunction.AggregateData aggregateData = (SQLiteFunction.AggregateData) null;
        if (this._base != null)
        {
          IntPtr key = this._base.AggregateContext(context);
          if (this._contextDataList != null && !this._contextDataList.TryGetValue(key, out aggregateData))
          {
            aggregateData = new SQLiteFunction.AggregateData();
            this._contextDataList[key] = aggregateData;
          }
        }
        if (aggregateData == null)
          aggregateData = new SQLiteFunction.AggregateData();
        try
        {
          this._context = context;
          this.Step(this.ConvertParams(nArgs, argsptr), aggregateData._count, ref aggregateData._data);
        }
        finally
        {
          ++aggregateData._count;
        }
      }
      catch (Exception ex)
      {
        try
        {
          if (!HelperMethods.LogCallbackExceptions(this._flags))
            return;
          SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "Step", (object) ex));
        }
        catch
        {
        }
      }
    }

    /// <summary>
    /// An internal aggregate Final function callback, which wraps the context pointer and calls the virtual Final() method.
    /// WARNING: Must not throw exceptions.
    /// </summary>
    /// <param name="context">A raw context pointer</param>
    internal void FinalCallback(IntPtr context)
    {
      try
      {
        object contextData = (object) null;
        if (this._base != null)
        {
          IntPtr key = this._base.AggregateContext(context);
          if (this._contextDataList != null)
          {
            SQLiteFunction.AggregateData aggregateData;
            if (this._contextDataList.TryGetValue(key, out aggregateData))
            {
              contextData = aggregateData._data;
              this._contextDataList.Remove(key);
            }
          }
        }
        try
        {
          this._context = context;
          this.SetReturnValue(context, this.Final(contextData));
        }
        finally
        {
          if (contextData is IDisposable disposable4)
            disposable4.Dispose();
        }
      }
      catch (Exception ex)
      {
        try
        {
          if (!HelperMethods.LogCallbackExceptions(this._flags))
            return;
          SQLiteLog.LogMessage(-2146233088, HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "Caught exception in \"{0}\" method: {1}", (object) "Final", (object) ex));
        }
        catch
        {
        }
      }
    }

    /// <summary>
    /// Using reflection, enumerate all assemblies in the current appdomain looking for classes that
    /// have a SQLiteFunctionAttribute attribute, and registering them accordingly.
    /// </summary>
    [FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
    static SQLiteFunction()
    {
      try
      {
        if (UnsafeNativeMethods.GetSettingValue("No_SQLiteFunctions", (string) null) != null)
          return;
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        int length1 = assemblies.Length;
        AssemblyName name = Assembly.GetExecutingAssembly().GetName();
        for (int index1 = 0; index1 < length1; ++index1)
        {
          bool flag = false;
          Type[] types;
          try
          {
            AssemblyName[] referencedAssemblies = assemblies[index1].GetReferencedAssemblies();
            int length2 = referencedAssemblies.Length;
            for (int index2 = 0; index2 < length2; ++index2)
            {
              if (referencedAssemblies[index2].Name == name.Name)
              {
                flag = true;
                break;
              }
            }
            if (flag)
              types = assemblies[index1].GetTypes();
            else
              continue;
          }
          catch (ReflectionTypeLoadException ex)
          {
            types = ex.Types;
          }
          int length3 = types.Length;
          for (int index2 = 0; index2 < length3; ++index2)
          {
            if (!(types[index2] == (Type) null))
            {
              object[] customAttributes = types[index2].GetCustomAttributes(typeof (SQLiteFunctionAttribute), false);
              int length2 = customAttributes.Length;
              for (int index3 = 0; index3 < length2; ++index3)
              {
                if (customAttributes[index3] is SQLiteFunctionAttribute at8)
                {
                  at8.InstanceType = types[index2];
                  SQLiteFunction.ReplaceFunction(at8, (object) null);
                }
              }
            }
          }
        }
      }
      catch
      {
      }
    }

    /// <summary>
    /// Manual method of registering a function.  The type must still have the SQLiteFunctionAttributes in order to work
    /// properly, but this is a workaround for the Compact Framework where enumerating assemblies is not currently supported.
    /// </summary>
    /// <param name="typ">The type of the function to register</param>
    public static void RegisterFunction(Type typ)
    {
      foreach (object customAttribute in typ.GetCustomAttributes(typeof (SQLiteFunctionAttribute), false))
      {
        if (customAttribute is SQLiteFunctionAttribute functionAttribute1)
          SQLiteFunction.RegisterFunction(functionAttribute1.Name, functionAttribute1.Arguments, functionAttribute1.FuncType, typ, functionAttribute1.Callback1, functionAttribute1.Callback2);
      }
    }

    /// <summary>
    /// Alternative method of registering a function.  This method
    /// does not require the specified type to be annotated with
    /// <see cref="T:System.Data.SQLite.SQLiteFunctionAttribute" />.
    /// </summary>
    /// <param name="name">The name of the function to register.</param>
    /// <param name="argumentCount">
    /// The number of arguments accepted by the function.
    /// </param>
    /// <param name="functionType">
    /// The type of SQLite function being resitered (e.g. scalar,
    /// aggregate, or collating sequence).
    /// </param>
    /// <param name="instanceType">
    /// The <see cref="T:System.Type" /> that actually implements the function.
    /// This will only be used if the <paramref name="callback1" />
    /// and <paramref name="callback2" /> parameters are null.
    /// </param>
    /// <param name="callback1">
    /// The <see cref="T:System.Delegate" /> to be used for all calls into the
    /// <see cref="M:System.Data.SQLite.SQLiteFunction.Invoke(System.Object[])" />,
    /// <see cref="M:System.Data.SQLite.SQLiteFunction.Step(System.Object[],System.Int32,System.Object@)" />,
    /// and <see cref="M:System.Data.SQLite.SQLiteFunction.Compare(System.String,System.String)" /> virtual methods.
    /// </param>
    /// <param name="callback2">
    /// The <see cref="T:System.Delegate" /> to be used for all calls into the
    /// <see cref="M:System.Data.SQLite.SQLiteFunction.Final(System.Object)" /> virtual method.  This
    /// parameter is only necessary for aggregate functions.
    /// </param>
    public static void RegisterFunction(
      string name,
      int argumentCount,
      FunctionType functionType,
      Type instanceType,
      Delegate callback1,
      Delegate callback2)
    {
      SQLiteFunction.ReplaceFunction(new SQLiteFunctionAttribute(name, argumentCount, functionType)
      {
        InstanceType = instanceType,
        Callback1 = callback1,
        Callback2 = callback2
      }, (object) null);
    }

    /// <summary>
    /// Replaces a registered function, disposing of the associated (old)
    /// value if necessary.
    /// </summary>
    /// <param name="at">
    /// The attribute that describes the function to replace.
    /// </param>
    /// <param name="newValue">The new value to use.</param>
    /// <returns>
    /// Non-zero if an existing registered function was replaced; otherwise,
    /// zero.
    /// </returns>
    private static bool ReplaceFunction(SQLiteFunctionAttribute at, object newValue)
    {
      object obj;
      if (SQLiteFunction._registeredFunctions.TryGetValue(at, out obj))
      {
        if (obj is IDisposable disposable2)
          disposable2.Dispose();
        SQLiteFunction._registeredFunctions[at] = newValue;
        return true;
      }
      SQLiteFunction._registeredFunctions.Add(at, newValue);
      return false;
    }

    /// <summary>
    /// Creates a <see cref="T:System.Data.SQLite.SQLiteFunction" /> instance based on the specified
    /// <see cref="T:System.Data.SQLite.SQLiteFunctionAttribute" />.
    /// </summary>
    /// <param name="functionAttribute">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunctionAttribute" /> containing the metadata about
    /// the function to create.
    /// </param>
    /// <param name="function">
    /// The created function -OR- null if the function could not be created.
    /// </param>
    /// <returns>
    /// Non-zero if the function was created; otherwise, zero.
    /// </returns>
    private static bool CreateFunction(
      SQLiteFunctionAttribute functionAttribute,
      out SQLiteFunction function)
    {
      if (functionAttribute == null)
      {
        function = (SQLiteFunction) null;
        return false;
      }
      if ((object) functionAttribute.Callback1 != null || (object) functionAttribute.Callback2 != null)
      {
        function = (SQLiteFunction) new SQLiteDelegateFunction(functionAttribute.Callback1, functionAttribute.Callback2);
        return true;
      }
      if (functionAttribute.InstanceType != (Type) null)
      {
        function = (SQLiteFunction) Activator.CreateInstance(functionAttribute.InstanceType);
        return true;
      }
      function = (SQLiteFunction) null;
      return false;
    }

    /// <summary>
    /// Called by the SQLiteBase derived classes, this method binds all registered (known) user-defined functions to a connection.
    /// It is done this way so that all user-defined functions will access the database using the same encoding scheme
    /// as the connection (UTF-8 or UTF-16).
    /// </summary>
    /// <remarks>
    /// The wrapper functions that interop with SQLite will create a unique cookie value, which internally is a pointer to
    /// all the wrapped callback functions.  The interop function uses it to map CDecl callbacks to StdCall callbacks.
    /// </remarks>
    /// <param name="sqlbase">The base object on which the functions are to bind.</param>
    /// <param name="flags">The flags associated with the parent connection object.</param>
    /// <returns>Returns a logical list of functions which the connection should retain until it is closed.</returns>
    internal static IDictionary<SQLiteFunctionAttribute, SQLiteFunction> BindFunctions(
      SQLiteBase sqlbase,
      SQLiteConnectionFlags flags)
    {
      IDictionary<SQLiteFunctionAttribute, SQLiteFunction> dictionary = (IDictionary<SQLiteFunctionAttribute, SQLiteFunction>) new Dictionary<SQLiteFunctionAttribute, SQLiteFunction>();
      foreach (KeyValuePair<SQLiteFunctionAttribute, object> registeredFunction in (IEnumerable<KeyValuePair<SQLiteFunctionAttribute, object>>) SQLiteFunction._registeredFunctions)
      {
        SQLiteFunctionAttribute key = registeredFunction.Key;
        if (key != null)
        {
          SQLiteFunction function;
          if (SQLiteFunction.CreateFunction(key, out function))
          {
            SQLiteFunction.BindFunction(sqlbase, key, function, flags);
            dictionary[key] = function;
          }
          else
            dictionary[key] = (SQLiteFunction) null;
        }
      }
      return dictionary;
    }

    /// <summary>
    /// Called by the SQLiteBase derived classes, this method unbinds all registered (known)
    /// functions -OR- all previously bound user-defined functions from a connection.
    /// </summary>
    /// <param name="sqlbase">The base object from which the functions are to be unbound.</param>
    /// <param name="flags">The flags associated with the parent connection object.</param>
    /// <param name="registered">
    /// Non-zero to unbind all registered (known) functions -OR- zero to unbind all functions
    /// currently bound to the connection.
    /// </param>
    /// <returns>Non-zero if all the specified user-defined functions were unbound.</returns>
    internal static bool UnbindAllFunctions(
      SQLiteBase sqlbase,
      SQLiteConnectionFlags flags,
      bool registered)
    {
      if (sqlbase == null)
        return false;
      IDictionary<SQLiteFunctionAttribute, SQLiteFunction> functions = sqlbase.Functions;
      if (functions == null)
        return false;
      bool flag = true;
      if (registered)
      {
        foreach (KeyValuePair<SQLiteFunctionAttribute, object> registeredFunction in (IEnumerable<KeyValuePair<SQLiteFunctionAttribute, object>>) SQLiteFunction._registeredFunctions)
        {
          SQLiteFunctionAttribute key = registeredFunction.Key;
          SQLiteFunction function;
          if (key != null && (!functions.TryGetValue(key, out function) || function == null || !SQLiteFunction.UnbindFunction(sqlbase, key, function, flags)))
            flag = false;
        }
      }
      else
      {
        foreach (KeyValuePair<SQLiteFunctionAttribute, SQLiteFunction> keyValuePair in (IEnumerable<KeyValuePair<SQLiteFunctionAttribute, SQLiteFunction>>) new Dictionary<SQLiteFunctionAttribute, SQLiteFunction>(functions))
        {
          SQLiteFunctionAttribute key = keyValuePair.Key;
          if (key != null)
          {
            SQLiteFunction function = keyValuePair.Value;
            if (function != null && SQLiteFunction.UnbindFunction(sqlbase, key, function, flags))
              sqlbase.Functions.Remove(key);
            else
              flag = false;
          }
        }
      }
      return flag;
    }

    /// <summary>
    /// This function binds a user-defined function to a connection.
    /// </summary>
    /// <param name="sqliteBase">
    /// The <see cref="T:System.Data.SQLite.SQLiteBase" /> object instance associated with the
    /// <see cref="T:System.Data.SQLite.SQLiteConnection" /> that the function should be bound to.
    /// </param>
    /// <param name="functionAttribute">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunctionAttribute" /> object instance containing
    /// the metadata for the function to be bound.
    /// </param>
    /// <param name="function">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunction" /> object instance that implements the
    /// function to be bound.
    /// </param>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    internal static void BindFunction(
      SQLiteBase sqliteBase,
      SQLiteFunctionAttribute functionAttribute,
      SQLiteFunction function,
      SQLiteConnectionFlags flags)
    {
      if (sqliteBase == null)
        throw new ArgumentNullException(nameof (sqliteBase));
      if (functionAttribute == null)
        throw new ArgumentNullException(nameof (functionAttribute));
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      FunctionType funcType = functionAttribute.FuncType;
      function._base = sqliteBase;
      function._flags = flags;
      function._InvokeFunc = funcType == FunctionType.Scalar ? new SQLiteCallback(function.ScalarCallback) : (SQLiteCallback) null;
      function._StepFunc = funcType == FunctionType.Aggregate ? new SQLiteCallback(function.StepCallback) : (SQLiteCallback) null;
      function._FinalFunc = funcType == FunctionType.Aggregate ? new SQLiteFinalCallback(function.FinalCallback) : (SQLiteFinalCallback) null;
      function._CompareFunc = funcType == FunctionType.Collation ? new SQLiteCollation(function.CompareCallback) : (SQLiteCollation) null;
      function._CompareFunc16 = funcType == FunctionType.Collation ? new SQLiteCollation(function.CompareCallback16) : (SQLiteCollation) null;
      string name = functionAttribute.Name;
      if (funcType != FunctionType.Collation)
      {
        bool needCollSeq = function is SQLiteFunctionEx;
        int function1 = (int) sqliteBase.CreateFunction(name, functionAttribute.Arguments, needCollSeq, function._InvokeFunc, function._StepFunc, function._FinalFunc, true);
      }
      else
      {
        int collation = (int) sqliteBase.CreateCollation(name, function._CompareFunc, function._CompareFunc16, true);
      }
    }

    /// <summary>
    /// This function unbinds a user-defined functions from a connection.
    /// </summary>
    /// <param name="sqliteBase">
    /// The <see cref="T:System.Data.SQLite.SQLiteBase" /> object instance associated with the
    /// <see cref="T:System.Data.SQLite.SQLiteConnection" /> that the function should be bound to.
    /// </param>
    /// <param name="functionAttribute">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunctionAttribute" /> object instance containing
    /// the metadata for the function to be bound.
    /// </param>
    /// <param name="function">
    /// The <see cref="T:System.Data.SQLite.SQLiteFunction" /> object instance that implements the
    /// function to be bound.
    /// </param>
    /// <param name="flags">
    /// The flags associated with the parent connection object.
    /// </param>
    /// <returns>Non-zero if the function was unbound.</returns>
    internal static bool UnbindFunction(
      SQLiteBase sqliteBase,
      SQLiteFunctionAttribute functionAttribute,
      SQLiteFunction function,
      SQLiteConnectionFlags flags)
    {
      if (sqliteBase == null)
        throw new ArgumentNullException(nameof (sqliteBase));
      if (functionAttribute == null)
        throw new ArgumentNullException(nameof (functionAttribute));
      if (function == null)
        throw new ArgumentNullException(nameof (function));
      FunctionType funcType = functionAttribute.FuncType;
      string name = functionAttribute.Name;
      if (funcType == FunctionType.Collation)
        return sqliteBase.CreateCollation(name, (SQLiteCollation) null, (SQLiteCollation) null, false) == SQLiteErrorCode.Ok;
      bool needCollSeq = function is SQLiteFunctionEx;
      return sqliteBase.CreateFunction(name, functionAttribute.Arguments, needCollSeq, (SQLiteCallback) null, (SQLiteCallback) null, (SQLiteFinalCallback) null, false) == SQLiteErrorCode.Ok;
    }

    private class AggregateData
    {
      internal int _count = 1;
      internal object _data;
    }
  }
}

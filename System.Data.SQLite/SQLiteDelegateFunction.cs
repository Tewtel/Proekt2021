// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteDelegateFunction
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Globalization;

namespace System.Data.SQLite
{
  /// <summary>
  /// This class implements a SQLite function using a <see cref="T:System.Delegate" />.
  /// All the virtual methods of the <see cref="T:System.Data.SQLite.SQLiteFunction" /> class are
  /// implemented using calls to the <see cref="T:System.Data.SQLite.SQLiteInvokeDelegate" />,
  /// <see cref="T:System.Data.SQLite.SQLiteStepDelegate" />, <see cref="T:System.Data.SQLite.SQLiteFinalDelegate" />,
  /// and <see cref="T:System.Data.SQLite.SQLiteCompareDelegate" /> strongly typed delegate types
  /// or via the <see cref="M:System.Delegate.DynamicInvoke(System.Object[])" /> method.
  /// The arguments are presented in the same order they appear in
  /// the associated <see cref="T:System.Data.SQLite.SQLiteFunction" /> methods with one exception:
  /// the first argument is the name of the virtual method being implemented.
  /// </summary>
  public class SQLiteDelegateFunction : SQLiteFunction
  {
    /// <summary>
    /// This error message is used by the overridden virtual methods when
    /// a required <see cref="T:System.Delegate" /> property (e.g.
    /// <see cref="P:System.Data.SQLite.SQLiteDelegateFunction.Callback1" /> or <see cref="P:System.Data.SQLite.SQLiteDelegateFunction.Callback2" />) has not been
    /// set.
    /// </summary>
    private const string NoCallbackError = "No \"{0}\" callback is set.";
    /// <summary>
    /// This error message is used by the overridden <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Compare(System.String,System.String)" />
    /// method when the result does not have a type of <see cref="T:System.Int32" />.
    /// </summary>
    private const string ResultInt32Error = "\"{0}\" result must be Int32.";
    private Delegate callback1;
    private Delegate callback2;

    /// <summary>Constructs an empty instance of this class.</summary>
    public SQLiteDelegateFunction()
      : this((Delegate) null, (Delegate) null)
    {
    }

    /// <summary>
    /// Constructs an instance of this class using the specified
    /// <see cref="T:System.Delegate" /> as the <see cref="T:System.Data.SQLite.SQLiteFunction" />
    /// implementation.
    /// </summary>
    /// <param name="callback1">
    /// The <see cref="T:System.Delegate" /> to be used for all calls into the
    /// <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Invoke(System.Object[])" />, <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Step(System.Object[],System.Int32,System.Object@)" />, and
    /// <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Compare(System.String,System.String)" /> virtual methods needed by the
    /// <see cref="T:System.Data.SQLite.SQLiteFunction" /> base class.
    /// </param>
    /// <param name="callback2">
    /// The <see cref="T:System.Delegate" /> to be used for all calls into the
    /// <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Final(System.Object)" /> virtual methods needed by the
    /// <see cref="T:System.Data.SQLite.SQLiteFunction" /> base class.
    /// </param>
    public SQLiteDelegateFunction(Delegate callback1, Delegate callback2)
    {
      this.callback1 = callback1;
      this.callback2 = callback2;
    }

    /// <summary>
    /// Returns the list of arguments for the <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Invoke(System.Object[])" /> method,
    /// as an <see cref="T:System.Array" /> of <see cref="T:System.Object" />.  The first
    /// argument is always the literal string "Invoke".
    /// </summary>
    /// <param name="args">
    /// The original arguments received by the <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Invoke(System.Object[])" /> method.
    /// </param>
    /// <param name="earlyBound">
    /// Non-zero if the returned arguments are going to be used with the
    /// <see cref="T:System.Data.SQLite.SQLiteInvokeDelegate" /> type; otherwise, zero.
    /// </param>
    /// <returns>
    /// The arguments to pass to the configured <see cref="T:System.Delegate" />.
    /// </returns>
    protected virtual object[] GetInvokeArgs(object[] args, bool earlyBound)
    {
      object[] objArray = new object[2]
      {
        (object) "Invoke",
        (object) args
      };
      if (!earlyBound)
        objArray = new object[1]{ (object) objArray };
      return objArray;
    }

    /// <summary>
    /// Returns the list of arguments for the <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Step(System.Object[],System.Int32,System.Object@)" /> method,
    /// as an <see cref="T:System.Array" /> of <see cref="T:System.Object" />.  The first
    /// argument is always the literal string "Step".
    /// </summary>
    /// <param name="args">
    /// The original arguments received by the <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Step(System.Object[],System.Int32,System.Object@)" /> method.
    /// </param>
    /// <param name="stepNumber">
    /// The step number (one based).  This is incrememted each time the
    /// <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Step(System.Object[],System.Int32,System.Object@)" /> method is called.
    /// </param>
    /// <param name="contextData">
    /// A placeholder for implementers to store contextual data pertaining
    /// to the current context.
    /// </param>
    /// <param name="earlyBound">
    /// Non-zero if the returned arguments are going to be used with the
    /// <see cref="T:System.Data.SQLite.SQLiteStepDelegate" /> type; otherwise, zero.
    /// </param>
    /// <returns>
    /// The arguments to pass to the configured <see cref="T:System.Delegate" />.
    /// </returns>
    protected virtual object[] GetStepArgs(
      object[] args,
      int stepNumber,
      object contextData,
      bool earlyBound)
    {
      object[] objArray = new object[4]
      {
        (object) "Step",
        (object) args,
        (object) stepNumber,
        contextData
      };
      if (!earlyBound)
        objArray = new object[1]{ (object) objArray };
      return objArray;
    }

    /// <summary>
    /// Updates the output arguments for the <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Step(System.Object[],System.Int32,System.Object@)" /> method,
    /// using an <see cref="T:System.Array" /> of <see cref="T:System.Object" />.  The first
    /// argument is always the literal string "Step".  Currently, only the
    /// <paramref name="contextData" /> parameter is updated.
    /// </summary>
    /// <param name="args">
    /// The original arguments received by the <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Step(System.Object[],System.Int32,System.Object@)" /> method.
    /// </param>
    /// <param name="contextData">
    /// A placeholder for implementers to store contextual data pertaining
    /// to the current context.
    /// </param>
    /// <param name="earlyBound">
    /// Non-zero if the returned arguments are going to be used with the
    /// <see cref="T:System.Data.SQLite.SQLiteStepDelegate" /> type; otherwise, zero.
    /// </param>
    /// <returns>
    /// The arguments to pass to the configured <see cref="T:System.Delegate" />.
    /// </returns>
    protected virtual void UpdateStepArgs(object[] args, ref object contextData, bool earlyBound)
    {
      object[] objArray = !earlyBound ? args[0] as object[] : args;
      if (objArray == null)
        return;
      contextData = objArray[objArray.Length - 1];
    }

    /// <summary>
    /// Returns the list of arguments for the <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Final(System.Object)" /> method,
    /// as an <see cref="T:System.Array" /> of <see cref="T:System.Object" />.  The first
    /// argument is always the literal string "Final".
    /// </summary>
    /// <param name="contextData">
    /// A placeholder for implementers to store contextual data pertaining
    /// to the current context.
    /// </param>
    /// <param name="earlyBound">
    /// Non-zero if the returned arguments are going to be used with the
    /// <see cref="T:System.Data.SQLite.SQLiteFinalDelegate" /> type; otherwise, zero.
    /// </param>
    /// <returns>
    /// The arguments to pass to the configured <see cref="T:System.Delegate" />.
    /// </returns>
    protected virtual object[] GetFinalArgs(object contextData, bool earlyBound)
    {
      object[] objArray = new object[2]
      {
        (object) "Final",
        contextData
      };
      if (!earlyBound)
        objArray = new object[1]{ (object) objArray };
      return objArray;
    }

    /// <summary>
    /// Returns the list of arguments for the <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Compare(System.String,System.String)" /> method,
    /// as an <see cref="T:System.Array" /> of <see cref="T:System.Object" />.  The first
    /// argument is always the literal string "Compare".
    /// </summary>
    /// <param name="param1">The first string to compare.</param>
    /// <param name="param2">The second strnig to compare.</param>
    /// <param name="earlyBound">
    /// Non-zero if the returned arguments are going to be used with the
    /// <see cref="T:System.Data.SQLite.SQLiteCompareDelegate" /> type; otherwise, zero.
    /// </param>
    /// <returns>
    /// The arguments to pass to the configured <see cref="T:System.Delegate" />.
    /// </returns>
    protected virtual object[] GetCompareArgs(string param1, string param2, bool earlyBound)
    {
      object[] objArray = new object[3]
      {
        (object) "Compare",
        (object) param1,
        (object) param2
      };
      if (!earlyBound)
        objArray = new object[1]{ (object) objArray };
      return objArray;
    }

    /// <summary>
    /// The <see cref="T:System.Delegate" /> to be used for all calls into the
    /// <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Invoke(System.Object[])" />, <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Step(System.Object[],System.Int32,System.Object@)" />, and
    /// <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Compare(System.String,System.String)" /> virtual methods needed by the
    /// <see cref="T:System.Data.SQLite.SQLiteFunction" /> base class.
    /// </summary>
    public virtual Delegate Callback1
    {
      get => this.callback1;
      set => this.callback1 = value;
    }

    /// <summary>
    /// The <see cref="T:System.Delegate" /> to be used for all calls into the
    /// <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Final(System.Object)" /> virtual methods needed by the
    /// <see cref="T:System.Data.SQLite.SQLiteFunction" /> base class.
    /// </summary>
    public virtual Delegate Callback2
    {
      get => this.callback2;
      set => this.callback2 = value;
    }

    /// <summary>
    /// This virtual method is the implementation for scalar functions.
    /// See the <see cref="M:System.Data.SQLite.SQLiteFunction.Invoke(System.Object[])" /> method for more
    /// details.
    /// </summary>
    /// <param name="args">The arguments for the scalar function.</param>
    /// <returns>The result of the scalar function.</returns>
    public override object Invoke(object[] args)
    {
      if ((object) this.callback1 == null)
        throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "No \"{0}\" callback is set.", (object) nameof (Invoke)));
      return this.callback1 is SQLiteInvokeDelegate callback1 ? callback1(nameof (Invoke), args) : this.callback1.DynamicInvoke(this.GetInvokeArgs(args, false));
    }

    /// <summary>
    /// This virtual method is part of the implementation for aggregate
    /// functions.  See the <see cref="M:System.Data.SQLite.SQLiteFunction.Step(System.Object[],System.Int32,System.Object@)" /> method
    /// for more details.
    /// </summary>
    /// <param name="args">The arguments for the aggregate function.</param>
    /// <param name="stepNumber">
    /// The step number (one based).  This is incrememted each time the
    /// <see cref="M:System.Data.SQLite.SQLiteDelegateFunction.Step(System.Object[],System.Int32,System.Object@)" /> method is called.
    /// </param>
    /// <param name="contextData">
    /// A placeholder for implementers to store contextual data pertaining
    /// to the current context.
    /// </param>
    public override void Step(object[] args, int stepNumber, ref object contextData)
    {
      if ((object) this.callback1 == null)
        throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "No \"{0}\" callback is set.", (object) nameof (Step)));
      if (this.callback1 is SQLiteStepDelegate callback1)
      {
        callback1(nameof (Step), args, stepNumber, ref contextData);
      }
      else
      {
        object[] stepArgs = this.GetStepArgs(args, stepNumber, contextData, false);
        this.callback1.DynamicInvoke(stepArgs);
        this.UpdateStepArgs(stepArgs, ref contextData, false);
      }
    }

    /// <summary>
    /// This virtual method is part of the implementation for aggregate
    /// functions.  See the <see cref="M:System.Data.SQLite.SQLiteFunction.Final(System.Object)" /> method
    /// for more details.
    /// </summary>
    /// <param name="contextData">
    /// A placeholder for implementers to store contextual data pertaining
    /// to the current context.
    /// </param>
    /// <returns>The result of the aggregate function.</returns>
    public override object Final(object contextData)
    {
      if ((object) this.callback2 == null)
        throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "No \"{0}\" callback is set.", (object) nameof (Final)));
      return this.callback2 is SQLiteFinalDelegate callback2 ? callback2(nameof (Final), contextData) : this.callback1.DynamicInvoke(this.GetFinalArgs(contextData, false));
    }

    /// <summary>
    /// This virtual method is part of the implementation for collating
    /// sequences.  See the <see cref="M:System.Data.SQLite.SQLiteFunction.Compare(System.String,System.String)" /> method
    /// for more details.
    /// </summary>
    /// <param name="param1">The first string to compare.</param>
    /// <param name="param2">The second strnig to compare.</param>
    /// <returns>
    /// A positive integer if the <paramref name="param1" /> parameter is
    /// greater than the <paramref name="param2" /> parameter, a negative
    /// integer if the <paramref name="param1" /> parameter is less than
    /// the <paramref name="param2" /> parameter, or zero if they are
    /// equal.
    /// </returns>
    public override int Compare(string param1, string param2)
    {
      if ((object) this.callback1 == null)
        throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "No \"{0}\" callback is set.", (object) nameof (Compare)));
      if (this.callback1 is SQLiteCompareDelegate callback1)
        return callback1(nameof (Compare), param1, param2);
      if (this.callback1.DynamicInvoke(this.GetCompareArgs(param1, param2, false)) is int num)
        return num;
      throw new InvalidOperationException(HelperMethods.StringFormat((IFormatProvider) CultureInfo.CurrentCulture, "\"{0}\" result must be Int32.", (object) nameof (Compare)));
    }
  }
}

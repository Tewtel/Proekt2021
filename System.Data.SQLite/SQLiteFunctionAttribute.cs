// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteFunctionAttribute
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

namespace System.Data.SQLite
{
  /// <summary>
  /// A simple custom attribute to enable us to easily find user-defined functions in
  /// the loaded assemblies and initialize them in SQLite as connections are made.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
  public sealed class SQLiteFunctionAttribute : Attribute
  {
    private string _name;
    private int _argumentCount;
    private FunctionType _functionType;
    private Type _instanceType;
    private Delegate _callback1;
    private Delegate _callback2;

    /// <summary>
    /// Default constructor, initializes the internal variables for the function.
    /// </summary>
    public SQLiteFunctionAttribute()
      : this((string) null, -1, FunctionType.Scalar)
    {
    }

    /// <summary>
    /// Constructs an instance of this class.  This sets the initial
    /// <see cref="P:System.Data.SQLite.SQLiteFunctionAttribute.InstanceType" />, <see cref="P:System.Data.SQLite.SQLiteFunctionAttribute.Callback1" />, and
    /// <see cref="P:System.Data.SQLite.SQLiteFunctionAttribute.Callback2" /> properties to null.
    /// </summary>
    /// <param name="name">
    /// The name of the function, as seen by the SQLite core library.
    /// </param>
    /// <param name="argumentCount">
    /// The number of arguments that the function will accept.
    /// </param>
    /// <param name="functionType">
    /// The type of function being declared.  This will either be Scalar,
    /// Aggregate, or Collation.
    /// </param>
    public SQLiteFunctionAttribute(string name, int argumentCount, FunctionType functionType)
    {
      this._name = name;
      this._argumentCount = argumentCount;
      this._functionType = functionType;
      this._instanceType = (Type) null;
      this._callback1 = (Delegate) null;
      this._callback2 = (Delegate) null;
    }

    /// <summary>
    /// The function's name as it will be used in SQLite command text.
    /// </summary>
    public string Name
    {
      get => this._name;
      set => this._name = value;
    }

    /// <summary>
    /// The number of arguments this function expects.  -1 if the number of arguments is variable.
    /// </summary>
    public int Arguments
    {
      get => this._argumentCount;
      set => this._argumentCount = value;
    }

    /// <summary>The type of function this implementation will be.</summary>
    public FunctionType FuncType
    {
      get => this._functionType;
      set => this._functionType = value;
    }

    /// <summary>
    /// The <see cref="T:System.Type" /> object instance that describes the class
    /// containing the implementation for the associated function.  The value of
    /// this property will not be used if either the <see cref="P:System.Data.SQLite.SQLiteFunctionAttribute.Callback1" /> or
    /// <see cref="P:System.Data.SQLite.SQLiteFunctionAttribute.Callback2" /> property values are set to non-null.
    /// </summary>
    internal Type InstanceType
    {
      get => this._instanceType;
      set => this._instanceType = value;
    }

    /// <summary>
    /// The <see cref="T:System.Delegate" /> that refers to the implementation for the
    /// associated function.  If this property value is set to non-null, it will
    /// be used instead of the <see cref="P:System.Data.SQLite.SQLiteFunctionAttribute.InstanceType" /> property value.
    /// </summary>
    internal Delegate Callback1
    {
      get => this._callback1;
      set => this._callback1 = value;
    }

    /// <summary>
    /// The <see cref="T:System.Delegate" /> that refers to the implementation for the
    /// associated function.  If this property value is set to non-null, it will
    /// be used instead of the <see cref="P:System.Data.SQLite.SQLiteFunctionAttribute.InstanceType" /> property value.
    /// </summary>
    internal Delegate Callback2
    {
      get => this._callback2;
      set => this._callback2 = value;
    }
  }
}

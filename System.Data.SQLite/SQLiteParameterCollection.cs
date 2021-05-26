// Decompiled with JetBrains decompiler
// Type: System.Data.SQLite.SQLiteParameterCollection
// Assembly: System.Data.SQLite, Version=1.0.112.0, Culture=neutral, PublicKeyToken=db937bc2d44ff139
// MVID: 5EE7A633-7B43-42FB-884D-DDCB2E803B48
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\System.Data.SQLite.dll

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data.SQLite
{
  /// <summary>SQLite implementation of DbParameterCollection.</summary>
  [ListBindable(false)]
  [Editor("Microsoft.VSDesigner.Data.Design.DBParametersEditor, Microsoft.VSDesigner, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
  public sealed class SQLiteParameterCollection : DbParameterCollection
  {
    /// <summary>
    /// The underlying command to which this collection belongs
    /// </summary>
    private SQLiteCommand _command;
    /// <summary>The internal array of parameters in this collection</summary>
    private List<SQLiteParameter> _parameterList;
    /// <summary>
    /// Determines whether or not all parameters have been bound to their statement(s)
    /// </summary>
    private bool _unboundFlag;

    /// <summary>Initializes the collection</summary>
    /// <param name="cmd">The command to which the collection belongs</param>
    internal SQLiteParameterCollection(SQLiteCommand cmd)
    {
      this._command = cmd;
      this._parameterList = new List<SQLiteParameter>();
      this._unboundFlag = true;
    }

    /// <summary>Returns false</summary>
    public override bool IsSynchronized => false;

    /// <summary>Returns false</summary>
    public override bool IsFixedSize => false;

    /// <summary>Returns false</summary>
    public override bool IsReadOnly => false;

    /// <summary>Returns null</summary>
    public override object SyncRoot => (object) null;

    /// <summary>Retrieves an enumerator for the collection</summary>
    /// <returns>An enumerator for the underlying array</returns>
    public override IEnumerator GetEnumerator() => (IEnumerator) this._parameterList.GetEnumerator();

    /// <summary>Adds a parameter to the collection</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="parameterType">The data type</param>
    /// <param name="parameterSize">The size of the value</param>
    /// <param name="sourceColumn">The source column</param>
    /// <returns>A SQLiteParameter object</returns>
    public SQLiteParameter Add(
      string parameterName,
      DbType parameterType,
      int parameterSize,
      string sourceColumn)
    {
      SQLiteParameter parameter = new SQLiteParameter(parameterName, parameterType, parameterSize, sourceColumn);
      this.Add(parameter);
      return parameter;
    }

    /// <summary>Adds a parameter to the collection</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="parameterType">The data type</param>
    /// <param name="parameterSize">The size of the value</param>
    /// <returns>A SQLiteParameter object</returns>
    public SQLiteParameter Add(
      string parameterName,
      DbType parameterType,
      int parameterSize)
    {
      SQLiteParameter parameter = new SQLiteParameter(parameterName, parameterType, parameterSize);
      this.Add(parameter);
      return parameter;
    }

    /// <summary>Adds a parameter to the collection</summary>
    /// <param name="parameterName">The parameter name</param>
    /// <param name="parameterType">The data type</param>
    /// <returns>A SQLiteParameter object</returns>
    public SQLiteParameter Add(string parameterName, DbType parameterType)
    {
      SQLiteParameter parameter = new SQLiteParameter(parameterName, parameterType);
      this.Add(parameter);
      return parameter;
    }

    /// <summary>Adds a parameter to the collection</summary>
    /// <param name="parameter">The parameter to add</param>
    /// <returns>A zero-based index of where the parameter is located in the array</returns>
    public int Add(SQLiteParameter parameter)
    {
      int index = -1;
      if (!string.IsNullOrEmpty(parameter.ParameterName))
        index = this.IndexOf(parameter.ParameterName);
      if (index == -1)
      {
        index = this._parameterList.Count;
        this._parameterList.Add(parameter);
      }
      this.SetParameter(index, (DbParameter) parameter);
      return index;
    }

    /// <summary>Adds a parameter to the collection</summary>
    /// <param name="value">The parameter to add</param>
    /// <returns>A zero-based index of where the parameter is located in the array</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int Add(object value) => this.Add((SQLiteParameter) value);

    /// <summary>
    /// Adds a named/unnamed parameter and its value to the parameter collection.
    /// </summary>
    /// <param name="parameterName">Name of the parameter, or null to indicate an unnamed parameter</param>
    /// <param name="value">The initial value of the parameter</param>
    /// <returns>Returns the SQLiteParameter object created during the call.</returns>
    public SQLiteParameter AddWithValue(string parameterName, object value)
    {
      SQLiteParameter parameter = new SQLiteParameter(parameterName, value);
      this.Add(parameter);
      return parameter;
    }

    /// <summary>Adds an array of parameters to the collection</summary>
    /// <param name="values">The array of parameters to add</param>
    public void AddRange(SQLiteParameter[] values)
    {
      int length = values.Length;
      for (int index = 0; index < length; ++index)
        this.Add(values[index]);
    }

    /// <summary>Adds an array of parameters to the collection</summary>
    /// <param name="values">The array of parameters to add</param>
    public override void AddRange(Array values)
    {
      int length = values.Length;
      for (int index = 0; index < length; ++index)
        this.Add((SQLiteParameter) values.GetValue(index));
    }

    /// <summary>Clears the array and resets the collection</summary>
    public override void Clear()
    {
      this._unboundFlag = true;
      this._parameterList.Clear();
    }

    /// <summary>
    /// Determines if the named parameter exists in the collection
    /// </summary>
    /// <param name="parameterName">The name of the parameter to check</param>
    /// <returns>True if the parameter is in the collection</returns>
    public override bool Contains(string parameterName) => this.IndexOf(parameterName) != -1;

    /// <summary>Determines if the parameter exists in the collection</summary>
    /// <param name="value">The SQLiteParameter to check</param>
    /// <returns>True if the parameter is in the collection</returns>
    public override bool Contains(object value) => this._parameterList.Contains((SQLiteParameter) value);

    /// <summary>Not implemented</summary>
    /// <param name="array"></param>
    /// <param name="index"></param>
    public override void CopyTo(Array array, int index) => throw new NotImplementedException();

    /// <summary>Returns a count of parameters in the collection</summary>
    public override int Count => this._parameterList.Count;

    /// <summary>
    /// Overloaded to specialize the return value of the default indexer
    /// </summary>
    /// <param name="parameterName">Name of the parameter to get/set</param>
    /// <returns>The specified named SQLite parameter</returns>
    public SQLiteParameter this[string parameterName]
    {
      get => (SQLiteParameter) this.GetParameter(parameterName);
      set => this.SetParameter(parameterName, (DbParameter) value);
    }

    /// <summary>
    /// Overloaded to specialize the return value of the default indexer
    /// </summary>
    /// <param name="index">The index of the parameter to get/set</param>
    /// <returns>The specified SQLite parameter</returns>
    public SQLiteParameter this[int index]
    {
      get => (SQLiteParameter) this.GetParameter(index);
      set => this.SetParameter(index, (DbParameter) value);
    }

    /// <summary>Retrieve a parameter by name from the collection</summary>
    /// <param name="parameterName">The name of the parameter to fetch</param>
    /// <returns>A DbParameter object</returns>
    protected override DbParameter GetParameter(string parameterName) => this.GetParameter(this.IndexOf(parameterName));

    /// <summary>Retrieves a parameter by its index in the collection</summary>
    /// <param name="index">The index of the parameter to retrieve</param>
    /// <returns>A DbParameter object</returns>
    protected override DbParameter GetParameter(int index) => (DbParameter) this._parameterList[index];

    /// <summary>Returns the index of a parameter given its name</summary>
    /// <param name="parameterName">The name of the parameter to find</param>
    /// <returns>-1 if not found, otherwise a zero-based index of the parameter</returns>
    public override int IndexOf(string parameterName)
    {
      int count = this._parameterList.Count;
      for (int index = 0; index < count; ++index)
      {
        if (string.Compare(parameterName, this._parameterList[index].ParameterName, StringComparison.OrdinalIgnoreCase) == 0)
          return index;
      }
      return -1;
    }

    /// <summary>Returns the index of a parameter</summary>
    /// <param name="value">The parameter to find</param>
    /// <returns>-1 if not found, otherwise a zero-based index of the parameter</returns>
    public override int IndexOf(object value) => this._parameterList.IndexOf((SQLiteParameter) value);

    /// <summary>
    /// Inserts a parameter into the array at the specified location
    /// </summary>
    /// <param name="index">The zero-based index to insert the parameter at</param>
    /// <param name="value">The parameter to insert</param>
    public override void Insert(int index, object value)
    {
      this._unboundFlag = true;
      this._parameterList.Insert(index, (SQLiteParameter) value);
    }

    /// <summary>Removes a parameter from the collection</summary>
    /// <param name="value">The parameter to remove</param>
    public override void Remove(object value)
    {
      this._unboundFlag = true;
      this._parameterList.Remove((SQLiteParameter) value);
    }

    /// <summary>
    /// Removes a parameter from the collection given its name
    /// </summary>
    /// <param name="parameterName">The name of the parameter to remove</param>
    public override void RemoveAt(string parameterName) => this.RemoveAt(this.IndexOf(parameterName));

    /// <summary>
    /// Removes a parameter from the collection given its index
    /// </summary>
    /// <param name="index">The zero-based parameter index to remove</param>
    public override void RemoveAt(int index)
    {
      this._unboundFlag = true;
      this._parameterList.RemoveAt(index);
    }

    /// <summary>
    /// Re-assign the named parameter to a new parameter object
    /// </summary>
    /// <param name="parameterName">The name of the parameter to replace</param>
    /// <param name="value">The new parameter</param>
    protected override void SetParameter(string parameterName, DbParameter value) => this.SetParameter(this.IndexOf(parameterName), value);

    /// <summary>Re-assign a parameter at the specified index</summary>
    /// <param name="index">The zero-based index of the parameter to replace</param>
    /// <param name="value">The new parameter</param>
    protected override void SetParameter(int index, DbParameter value)
    {
      this._unboundFlag = true;
      this._parameterList[index] = (SQLiteParameter) value;
    }

    /// <summary>Un-binds all parameters from their statements</summary>
    internal void Unbind() => this._unboundFlag = true;

    /// <summary>
    /// This function attempts to map all parameters in the collection to all statements in a Command.
    /// Since named parameters may span multiple statements, this function makes sure all statements are bound
    /// to the same named parameter.  Unnamed parameters are bound in sequence.
    /// </summary>
    internal void MapParameters(SQLiteStatement activeStatement)
    {
      if (!this._unboundFlag || this._parameterList.Count == 0 || this._command._statementList == null)
        return;
      int num1 = 0;
      int num2 = -1;
      foreach (SQLiteParameter parameter in this._parameterList)
      {
        ++num2;
        string s1 = parameter.ParameterName;
        if (s1 == null)
        {
          s1 = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, ";{0}", (object) num1);
          ++num1;
        }
        bool flag = false;
        int num3 = activeStatement != null ? 1 : this._command._statementList.Count;
        SQLiteStatement sqLiteStatement1 = activeStatement;
        for (int index = 0; index < num3; ++index)
        {
          flag = false;
          if (sqLiteStatement1 == null)
            sqLiteStatement1 = this._command._statementList[index];
          if (sqLiteStatement1._paramNames != null && sqLiteStatement1.MapParameter(s1, parameter))
            flag = true;
          sqLiteStatement1 = (SQLiteStatement) null;
        }
        if (!flag)
        {
          string s2 = HelperMethods.StringFormat((IFormatProvider) CultureInfo.InvariantCulture, ";{0}", (object) num2);
          SQLiteStatement sqLiteStatement2 = activeStatement;
          for (int index = 0; index < num3; ++index)
          {
            if (sqLiteStatement2 == null)
              sqLiteStatement2 = this._command._statementList[index];
            if (sqLiteStatement2._paramNames != null && sqLiteStatement2.MapParameter(s2, parameter))
              ;
            sqLiteStatement2 = (SQLiteStatement) null;
          }
        }
      }
      if (activeStatement != null)
        return;
      this._unboundFlag = false;
    }
  }
}

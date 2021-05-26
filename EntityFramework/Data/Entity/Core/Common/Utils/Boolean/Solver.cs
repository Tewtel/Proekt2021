// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Common.Utils.Boolean.Solver
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace System.Data.Entity.Core.Common.Utils.Boolean
{
  internal sealed class Solver
  {
    private readonly Dictionary<Triple<Vertex, Vertex, Vertex>, Vertex> _computedIfThenElseValues = new Dictionary<Triple<Vertex, Vertex, Vertex>, Vertex>();
    private readonly Dictionary<Vertex, Vertex> _knownVertices = new Dictionary<Vertex, Vertex>((IEqualityComparer<Vertex>) Solver.VertexValueComparer.Instance);
    private int _variableCount;
    internal static readonly Vertex[] BooleanVariableChildren = new Vertex[2]
    {
      Vertex.One,
      Vertex.Zero
    };

    internal int CreateVariable() => ++this._variableCount;

    internal Vertex Not(Vertex vertex) => this.IfThenElse(vertex, Vertex.Zero, Vertex.One);

    internal Vertex And(IEnumerable<Vertex> children) => children.OrderByDescending<Vertex, int>((Func<Vertex, int>) (child => child.Variable)).Aggregate<Vertex, Vertex>(Vertex.One, (Func<Vertex, Vertex, Vertex>) ((left, right) => this.IfThenElse(left, right, Vertex.Zero)));

    internal Vertex And(Vertex left, Vertex right) => this.IfThenElse(left, right, Vertex.Zero);

    internal Vertex Or(IEnumerable<Vertex> children) => children.OrderByDescending<Vertex, int>((Func<Vertex, int>) (child => child.Variable)).Aggregate<Vertex, Vertex>(Vertex.Zero, (Func<Vertex, Vertex, Vertex>) ((left, right) => this.IfThenElse(left, Vertex.One, right)));

    internal Vertex CreateLeafVertex(int variable, Vertex[] children) => this.GetUniqueVertex(variable, children);

    private Vertex GetUniqueVertex(int variable, Vertex[] children)
    {
      Vertex key = new Vertex(variable, children);
      Vertex vertex;
      if (this._knownVertices.TryGetValue(key, out vertex))
        return vertex;
      this._knownVertices.Add(key, key);
      return key;
    }

    private Vertex IfThenElse(Vertex condition, Vertex then, Vertex @else)
    {
      if (condition.IsOne())
        return then;
      if (condition.IsZero())
        return @else;
      if (then.IsOne() && @else.IsZero())
        return condition;
      if (then.Equals(@else))
        return then;
      Triple<Vertex, Vertex, Vertex> key = new Triple<Vertex, Vertex, Vertex>(condition, then, @else);
      Vertex vertex;
      if (this._computedIfThenElseValues.TryGetValue(key, out vertex))
        return vertex;
      int topVariableDomainCount;
      int topVariable = Solver.DetermineTopVariable(condition, then, @else, out topVariableDomainCount);
      Vertex[] children = new Vertex[topVariableDomainCount];
      bool flag = true;
      for (int variableAssignment = 0; variableAssignment < topVariableDomainCount; ++variableAssignment)
      {
        children[variableAssignment] = this.IfThenElse(Solver.EvaluateFor(condition, topVariable, variableAssignment), Solver.EvaluateFor(then, topVariable, variableAssignment), Solver.EvaluateFor(@else, topVariable, variableAssignment));
        if (variableAssignment > 0 & flag && !children[variableAssignment].Equals(children[0]))
          flag = false;
      }
      if (flag)
        return children[0];
      Vertex uniqueVertex = this.GetUniqueVertex(topVariable, children);
      this._computedIfThenElseValues.Add(key, uniqueVertex);
      return uniqueVertex;
    }

    private static int DetermineTopVariable(
      Vertex condition,
      Vertex then,
      Vertex @else,
      out int topVariableDomainCount)
    {
      int variable;
      if (condition.Variable < then.Variable)
      {
        variable = condition.Variable;
        topVariableDomainCount = condition.Children.Length;
      }
      else
      {
        variable = then.Variable;
        topVariableDomainCount = then.Children.Length;
      }
      if (@else.Variable < variable)
      {
        variable = @else.Variable;
        topVariableDomainCount = @else.Children.Length;
      }
      return variable;
    }

    private static Vertex EvaluateFor(Vertex vertex, int variable, int variableAssignment) => variable < vertex.Variable ? vertex : vertex.Children[variableAssignment];

    [Conditional("DEBUG")]
    private void AssertVerticesValid(IEnumerable<Vertex> vertices)
    {
      foreach (Vertex vertex in vertices)
        ;
    }

    [Conditional("DEBUG")]
    private void AssertVertexValid(Vertex vertex) => vertex.IsSink();

    private class VertexValueComparer : IEqualityComparer<Vertex>
    {
      internal static readonly Solver.VertexValueComparer Instance = new Solver.VertexValueComparer();

      private VertexValueComparer()
      {
      }

      public bool Equals(Vertex x, Vertex y)
      {
        if (x.IsSink())
          return x.Equals(y);
        if (x.Variable != y.Variable || x.Children.Length != y.Children.Length)
          return false;
        for (int index = 0; index < x.Children.Length; ++index)
        {
          if (!x.Children[index].Equals(y.Children[index]))
            return false;
        }
        return true;
      }

      public int GetHashCode(Vertex vertex) => vertex.IsSink() ? vertex.GetHashCode() : (vertex.Children[0].GetHashCode() << 5) + 1 + vertex.Children[1].GetHashCode();
    }
  }
}

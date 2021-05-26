// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Objects.ExecutionOptions
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

namespace System.Data.Entity.Core.Objects
{
  /// <summary>Options for query execution.</summary>
  public class ExecutionOptions
  {
    internal static readonly ExecutionOptions Default = new ExecutionOptions(MergeOption.AppendOnly);

    /// <summary>
    /// Creates a new instance of <see cref="T:System.Data.Entity.Core.Objects.ExecutionOptions" />.
    /// </summary>
    /// <param name="mergeOption"> Merge option to use for entity results. </param>
    public ExecutionOptions(MergeOption mergeOption) => this.MergeOption = mergeOption;

    /// <summary>
    /// Creates a new instance of <see cref="T:System.Data.Entity.Core.Objects.ExecutionOptions" />.
    /// </summary>
    /// <param name="mergeOption"> Merge option to use for entity results. </param>
    /// <param name="streaming"> Whether the query is streaming or buffering. </param>
    public ExecutionOptions(MergeOption mergeOption, bool streaming)
    {
      this.MergeOption = mergeOption;
      this.UserSpecifiedStreaming = new bool?(streaming);
    }

    internal ExecutionOptions(MergeOption mergeOption, bool? streaming)
    {
      this.MergeOption = mergeOption;
      this.UserSpecifiedStreaming = streaming;
    }

    /// <summary>Merge option to use for entity results.</summary>
    public MergeOption MergeOption { get; private set; }

    /// <summary>Whether the query is streaming or buffering.</summary>
    [Obsolete("Queries are now streaming by default unless a retrying ExecutionStrategy is used. This property no longer returns an accurate value.")]
    public bool Streaming => this.UserSpecifiedStreaming ?? true;

    internal bool? UserSpecifiedStreaming { get; private set; }

    /// <summary>Determines whether the specified objects are equal.</summary>
    /// <returns>true if the two objects are equal; otherwise, false.</returns>
    /// <param name="left">The left object to compare.</param>
    /// <param name="right">The right object to compare.</param>
    public static bool operator ==(ExecutionOptions left, ExecutionOptions right)
    {
      if ((object) left == (object) right)
        return true;
      return (object) left != null && left.Equals((object) right);
    }

    /// <summary>
    /// Determines whether the specified objects are not equal.
    /// </summary>
    /// <param name="left">The left object to compare.</param>
    /// <param name="right">The right object to compare.</param>
    /// <returns>true if the two objects are not equal; otherwise, false.</returns>
    public static bool operator !=(ExecutionOptions left, ExecutionOptions right) => !(left == right);

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      ExecutionOptions executionOptions = obj as ExecutionOptions;
      if ((object) executionOptions == null || this.MergeOption != executionOptions.MergeOption)
        return false;
      bool? specifiedStreaming1 = this.UserSpecifiedStreaming;
      bool? specifiedStreaming2 = executionOptions.UserSpecifiedStreaming;
      return specifiedStreaming1.GetValueOrDefault() == specifiedStreaming2.GetValueOrDefault() & specifiedStreaming1.HasValue == specifiedStreaming2.HasValue;
    }

    /// <inheritdoc />
    public override int GetHashCode() => this.MergeOption.GetHashCode() ^ this.UserSpecifiedStreaming.GetHashCode();
  }
}

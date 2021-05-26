// Decompiled with JetBrains decompiler
// Type: System.Data.Entity.Core.Mapping.ViewGeneration.ConfigViewGenerator
// Assembly: EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// MVID: 13DB822D-E6C4-49C2-A425-BD1B471FFCED
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\EntityFramework.dll

using System.Data.Entity.Core.Common.Utils;
using System.Diagnostics;
using System.Text;

namespace System.Data.Entity.Core.Mapping.ViewGeneration
{
  internal sealed class ConfigViewGenerator : InternalBase
  {
    private ViewGenTraceLevel m_traceLevel;
    private readonly TimeSpan[] m_breakdownTimes;
    private readonly Stopwatch m_watch;
    private readonly Stopwatch m_singleWatch;
    private PerfType m_singlePerfOp;
    private bool m_enableValidation = true;
    private bool m_generateUpdateViews = true;

    internal ConfigViewGenerator()
    {
      this.m_watch = new Stopwatch();
      this.m_singleWatch = new Stopwatch();
      this.m_breakdownTimes = new TimeSpan[Enum.GetNames(typeof (PerfType)).Length];
      this.m_traceLevel = ViewGenTraceLevel.None;
      this.m_generateUpdateViews = false;
      this.StartWatch();
    }

    internal bool GenerateEsql { get; set; }

    internal TimeSpan[] BreakdownTimes => this.m_breakdownTimes;

    internal ViewGenTraceLevel TraceLevel
    {
      get => this.m_traceLevel;
      set => this.m_traceLevel = value;
    }

    internal bool IsValidationEnabled
    {
      get => this.m_enableValidation;
      set => this.m_enableValidation = value;
    }

    internal bool GenerateUpdateViews
    {
      get => this.m_generateUpdateViews;
      set => this.m_generateUpdateViews = value;
    }

    internal bool GenerateViewsForEachType { get; set; }

    internal bool IsViewTracing => this.IsTraceAllowed(ViewGenTraceLevel.ViewsOnly);

    internal bool IsNormalTracing => this.IsTraceAllowed(ViewGenTraceLevel.Normal);

    internal bool IsVerboseTracing => this.IsTraceAllowed(ViewGenTraceLevel.Verbose);

    private void StartWatch() => this.m_watch.Start();

    internal void StartSingleWatch(PerfType perfType)
    {
      this.m_singleWatch.Start();
      this.m_singlePerfOp = perfType;
    }

    internal void StopSingleWatch(PerfType perfType)
    {
      TimeSpan elapsed = this.m_singleWatch.Elapsed;
      int index = (int) perfType;
      this.m_singleWatch.Stop();
      this.m_singleWatch.Reset();
      this.BreakdownTimes[index] = this.BreakdownTimes[index].Add(elapsed);
    }

    internal void SetTimeForFinishedActivity(PerfType perfType)
    {
      TimeSpan elapsed = this.m_watch.Elapsed;
      int index = (int) perfType;
      this.BreakdownTimes[index] = this.BreakdownTimes[index].Add(elapsed);
      this.m_watch.Reset();
      this.m_watch.Start();
    }

    internal bool IsTraceAllowed(ViewGenTraceLevel traceLevel) => this.TraceLevel >= traceLevel;

    internal override void ToCompactString(StringBuilder builder) => StringUtil.FormatStringBuilder(builder, "Trace Switch: {0}", (object) this.m_traceLevel);
  }
}

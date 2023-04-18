namespace DotTja.Types;

using System.Collections.Immutable;
using Commands;
using Enums;

public sealed partial record CourseVariant
{
    internal sealed class Builder
    {
        public ImmutableList<int>.Builder? Balloon { get; set; }
        public ImmutableList<int>.Builder? BalloonNor { get; set; }
        public ImmutableList<int>.Builder? BalloonExp { get; set; }
        public ImmutableList<int>.Builder? BalloonMas { get; set; }
        public int? ScoreInit { get; set; }
        public int? ScoreDiff { get; set; }
        public Style? Style { get; set; }
        public DojoGauge? DojoGauge1 { get; set; }
        public DojoGauge? DojoGauge2 { get; set; }
        public DojoGauge? DojoGauge3 { get; set; }
        public GaugeIncrementMethod? GaugeIncr { get; set; }
        public int? Total { get; set; } // maybe int?
        public bool? HiddenBranch { get; set; }
        public ImmutableList<Command>.Builder? Player1Commands { get; set; }
        public ImmutableList<Command>.Builder? Player2Commands { get; set; }

        public CourseVariant ToCourseVariant() => new()
        {
            Balloon = this.Balloon?.ToImmutable(),
            BalloonNor = this.BalloonNor?.ToImmutable(),
            BalloonExp = this.BalloonExp?.ToImmutable(),
            BalloonMas = this.BalloonMas?.ToImmutable(),
            ScoreInit = this.ScoreInit,
            ScoreDiff = this.ScoreDiff,
            Style = this.Style,
            DojoGauge1 = this.DojoGauge1,
            DojoGauge2 = this.DojoGauge2,
            DojoGauge3 = this.DojoGauge3,
            GaugeIncr = this.GaugeIncr,
            Total = this.Total,
            HiddenBranch = this.HiddenBranch,
            Player1Commands = this.Player1Commands?.ToImmutable(),
            Player2Commands = this.Player2Commands?.ToImmutable()
        };
    }
};

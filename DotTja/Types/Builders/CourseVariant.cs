namespace DotTja.Types;

using System.Collections.Immutable;
using System.Reflection;
using Commands;
using Enums;

public sealed partial record CourseVariant
{
    internal sealed class Builder
    {
        public ImmutableList<int>? Balloon { get; set; }
        public ImmutableList<int>? BalloonNor { get; set; }
        public ImmutableList<int>? BalloonExp { get; set; }
        public ImmutableList<int>? BalloonMas { get; set; }
        /// <summary>
        /// todo: explain this
        /// </summary>
        public (int normal, int? shinUchi)? ScoreInit { get; set; }
        public int? ScoreDiff { get; set; }
        public Style? Style { get; set; }
        public DojoGauge? DojoGauge1 { get; set; }
        public DojoGauge? DojoGauge2 { get; set; }
        public DojoGauge? DojoGauge3 { get; set; }
        public GaugeIncrementMethod? GaugeIncr { get; set; }
        public int? Total { get; set; } // maybe int?
        public bool? HiddenBranch { get; set; }
        public ImmutableList<Command>? Player1Commands { get; set; }
        public ImmutableList<Command>? Player2Commands { get; set; }

        public CourseVariant ToCourseVariant() => new()
        {
            Balloon = this.Balloon,
            BalloonNor = this.BalloonNor,
            BalloonExp = this.BalloonExp,
            BalloonMas = this.BalloonMas,
            ScoreInit = this.ScoreInit,
            ScoreDiff = this.ScoreDiff,
            Style = this.Style,
            DojoGauge1 = this.DojoGauge1,
            DojoGauge2 = this.DojoGauge2,
            DojoGauge3 = this.DojoGauge3,
            GaugeIncr = this.GaugeIncr,
            Total = this.Total,
            HiddenBranch = this.HiddenBranch,
            Player1Commands = this.Player1Commands,
            Player2Commands = this.Player2Commands
        };

        public static readonly ImmutableDictionary<string, PropertyInfo> Properties =
            typeof(Builder)
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .ToImmutableDictionary(p => p.Name.ToUpperInvariant(), p => p)
                .MoveKey("DOJOGAUGE1", "EXAM1")
                .MoveKey("DOJOGAUGE2", "EXAM2")
                .MoveKey("DOJOGAUGE3", "EXAM3");
    }
};

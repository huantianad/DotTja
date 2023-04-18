namespace DotTja.Types;

using System.Collections.Immutable;
using Commands;
using Enums;

public sealed partial record CourseVariant
{
    public ImmutableList<int>? Balloon { get; init; }
    public ImmutableList<int>? BalloonNor { get; init; }
    public ImmutableList<int>? BalloonExp { get; init; }
    public ImmutableList<int>? BalloonMas { get; init; }
    public (int normal, int? shinUchi)? ScoreInit { get; init; }
    public int? ScoreDiff { get; init; }
    public Style? Style { get; init; }
    public DojoGauge? DojoGauge1 { get; init; }
    public DojoGauge? DojoGauge2 { get; init; }
    public DojoGauge? DojoGauge3 { get; init; }
    public GaugeIncrementMethod? GaugeIncr { get; init; }
    public int? Total { get; init; } // maybe double?
    public bool? HiddenBranch { get; init; }
    public ImmutableList<Command>? Player1Commands { get; init; } // maybe this shouldn't be nullable
    public ImmutableList<Command>? Player2Commands { get; init; }
}

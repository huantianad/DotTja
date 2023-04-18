namespace DotTja.Types;

using System.Collections.Immutable;
using Commands;
using Enums;

public sealed record CourseVariant
{
    public ImmutableList<int>? Balloon { get; init; }
    public ImmutableList<int>? BalloonNor { get; init; }
    public ImmutableList<int>? BalloonExp { get; init; }
    public ImmutableList<int>? BalloonMas { get; init; }
    public int? ScoreInit { get; init; }
    public int? ScoreDiff { get; init; }
    public Style? Style { get; init; }
    public DojoGauge? DojoGauge1 { get; init; }
    public DojoGauge? DojoGauge2 { get; init; }
    public DojoGauge? DojoGauge3 { get; init; }
    public GaugeIncrementMethod? GaugeIncr { get; init; }
    public int? Total { get; init; } // maybe int?
    public bool? HiddenBranch { get; init; }
    public ImmutableList<Command> Player1Commands { get; init; } = ImmutableList.Create<Command>();
    public ImmutableList<Command>? Player2Commands { get; init; }
}

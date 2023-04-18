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
    /// <summary>
    /// TODO: add main docs later
    /// This isn't mentioned in documentation, but each course can actually override the
    /// main <see cref="Metadata.ScoreMode"/> set in <see cref="Metadata"/>.
    /// </summary>
    public ScoreMode? ScoreMode { get; set; }
    /// <summary>
    /// TODO: add main docs later
    /// Unlike how it's shown in the docs, this can have a second optional value,
    /// that determines the ScoreInit value used in shin uchi mode.
    /// There is no corresponding value for <see cref="ScoreDiff"/>, as that value is
    /// completely ignored in shin uchi mode.
    /// </summary>
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

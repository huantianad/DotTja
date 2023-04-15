namespace DotTja.Types;

using System.Collections.Immutable;
using Enums;

public record Course(
    Difficulty Difficulty,
    int Level,
    ImmutableList<int> Balloon,
    int ScoreInit,
    int ScoreDiff

);

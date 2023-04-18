namespace DotTja.Types;

using Enums;

public sealed record DojoGauge(
    DojoGaugeCondition Condition,
    int RedClearRequirement,
    int GoldClearRequirement,
    DojoGaugeScope Scope
);

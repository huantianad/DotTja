namespace DotTja.Types;

using Enums;

public record DojoGauge(
    DojoGaugeCondition Condition,
    int RedClearRequirement,
    int GoldClearRequirement,
    DojoGaugeScope Scope
);

namespace DotTja.Types.Enums;

using EnumConverter;

public enum GaugeIncrementMethod
{
    [EnumAlias("Normal")]
    Normal,
    [EnumAlias("Floor")]
    Floor,
    [EnumAlias("Round")]
    Round,
    [EnumAlias("NotFix")]
    NotFix,
    [EnumAlias("Ceiling")]
    Ceiling
}

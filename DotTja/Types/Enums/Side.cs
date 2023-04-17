namespace DotTja.Types.Enums;

using EnumConverter;

public enum Side
{
    [EnumAlias("Normal", "1")]
    Normal,
#pragma warning disable CA1711
    [EnumAlias("Ex", "2")]
    Ex,
#pragma warning restore CA1711
    [EnumAlias("Both", "3")]
    Both
}

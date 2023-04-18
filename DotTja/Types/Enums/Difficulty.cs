namespace DotTja.Types.Enums;

using EnumConverter;

public enum Difficulty
{
    [EnumAlias("Easy", "0")]
    Easy,
    [EnumAlias("Normal", "1")]
    Normal,
    [EnumAlias("Hard", "2")]
    Hard,
    [EnumAlias("Oni", "3")]
    Oni,
    [EnumAlias("Ura", "Edit", "4")]
    Ura,
    [EnumAlias("Tower", "5")]
    Tower,
    [EnumAlias("Dan", "6")]
    Dan
}

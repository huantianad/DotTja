namespace DotTja.EnumConverter;

public sealed class MissingEnumAliasException : Exception
{
    public MissingEnumAliasException(Type enumType, object enumMember) : base(
        $"Attempted to use {nameof(EnumConverter)} on '{enumType}', " +
        $"but member '{enumMember}' is missing a {nameof(EnumAliasAttribute)}."
    )
    {
    }
}

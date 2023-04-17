namespace DotTja.EnumConverter;

using System.Collections.Immutable;

[AttributeUsage(AttributeTargets.Field)]
public class EnumAliasAttribute : Attribute
{
    public string SerializedName { get; }
    public ImmutableList<string> Aliases { get; }

    public EnumAliasAttribute(string serializedName, params string[] aliases)
    {
        this.SerializedName = serializedName;
        this.Aliases = aliases.ToImmutableList();
    }
}

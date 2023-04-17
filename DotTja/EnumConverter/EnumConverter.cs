namespace DotTja.EnumConverter;

using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;
using Exceptions;

public static class EnumConverter
{
    /// <summary>
    /// A mapping of Enum type -> (Alias name -> Enum value).
    /// Used so we only have to do reflection calls on each enum once.
    /// </summary>
    private static readonly Dictionary<Type, Dictionary<string, object>> DeserializeCache = new();

    /// <summary>
    /// A mapping of Enum type -> (Enum value -> Serialized string).
    /// Used so we only have to do reflection calls on each enum once.
    /// </summary>
    private static readonly Dictionary<Type, Dictionary<object, string>> SerializeCache = new();

    private static void UpdateCache(Type enumType)
    {
        if (DeserializeCache.ContainsKey(enumType))
        {
            return;
        }

        Debug.Assert(enumType.IsEnum, $"enumType={enumType}, enumType.IsEnum");

        SerializeCache[enumType] = new Dictionary<object, string>();
        DeserializeCache[enumType] = new Dictionary<string, object>();

        foreach (var member in enumType.GetFields())
        {
            var attribute = member.GetCustomAttribute<EnumAliasAttribute>(false);
            if (attribute == null)
            {
                continue;
            }

            var enumValue = member.GetValue(null);
            Debug.Assert(enumValue != null, nameof(enumValue) + " != null");

            SerializeCache[enumType][enumValue] = attribute.SerializedName;
            DeserializeCache[enumType][attribute.SerializedName] = enumValue;

            foreach (var alias in attribute.Aliases)
            {
                DeserializeCache[enumType][alias] = enumValue;
            }
        }
    }

    [Pure]
    public static object Parse(Type enumType, string value)
    {
        UpdateCache(enumType);

        if (!DeserializeCache[enumType].ContainsKey(value))
        {
            throw new ParsingException(
                $"Attempted to convert string '{value}' to enum '{enumType.Name}', " +
                "but enum does not have member with that alias."
            );
        }

        return DeserializeCache[enumType][value];
    }

    [Pure]
    public static object Serialize(Enum enumValue)
    {
        var enumType = enumValue.GetType();

        if (!Enum.IsDefined(enumType, enumValue))
        {
            throw new ArgumentOutOfRangeException(
                nameof(enumValue), enumValue, $"Value out of range for enum '{enumType.Name}'.'"
            );
        }

        UpdateCache(enumType);

        return SerializeCache[enumType][enumValue];
    }
}
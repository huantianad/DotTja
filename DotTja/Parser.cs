namespace DotTja;

using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;
using Exceptions;
using Types;
using Types.Builders;

internal static class Parser
{
    public static TjaFile Deserialize(ParserReader reader)
    {
        var result = new TjaFileBuilder();

        while (true)
        {
            var (key, value) = reader.ReadKeyValuePair()
                ?? throw new ParsingException("Encountered end of stream when parsing metadata.");

            if (key == "COURSE")
            {
                break;
            }

            ParseLineOfMainMetadata(result.Metadata, key, value);
        }

        return result.ToTjaFile();
    }

    private static void ParseLineOfMainMetadata(MetadataBuilder metadata, string key, string value)
    {
        if (key.StartsWith("TITLE", StringComparison.InvariantCulture))
        {
            var languageKey = key == "TITLE"
                ? "DEFAULT"
                : key["TITLE".Length..];
            SetValue(languageKey, value, metadata.Title, LocalizedStringBuilder.Properties);
        }
        else if (key.StartsWith("SUBTITLE", StringComparison.InvariantCulture))
        {
            var languageKey = key == "SUBTITLE"
                ? "DEFAULT"
                : key["SUBTITLE".Length..];
            SetValue(languageKey, value, metadata.Subtitle, LocalizedStringBuilder.Properties);
        }
        else
        {
            SetValue(key, value, metadata, MetadataBuilder.Properties);
        }
    }

    private static object StringToValue(string rawValue, Type targetType)
    {
        if (targetType == typeof(string))
        {
            return rawValue;
        }
        if (targetType == typeof(int))
        {
            return int.Parse(rawValue, CultureInfo.InvariantCulture);
        }
        if (targetType == typeof(double))
        {
            return double.Parse(rawValue, CultureInfo.InvariantCulture);
        }
        if (targetType == typeof(FileInfo))
        {
            return new FileInfo(rawValue);
        }
        if (targetType.IsEnum)
        {
            return EnumConverter.EnumConverter.Parse(targetType, rawValue);
        }
        if (targetType == typeof(TaikoWebSkin))
        {
            var pairs = rawValue
                .Split(",")
                .Select(
                    rawPair =>
                    {
                        var split = rawPair.Split(' ', 2);
                        if (split.Length != 2)
                        {
                            throw new ParsingException($"TaikoWebSkin missing space separator: {rawPair}");
                        }

                        return split;
                    }
                )
                .ToImmutableDictionary(p => p[0], p => p[1]);

            return new TaikoWebSkin(
                new DirectoryInfo(pairs["dir"]),
                pairs["name"],
                pairs.GetValueOrDefault("song"),
                pairs.GetValueOrDefault("stage"),
                pairs.GetValueOrDefault("don")
            );
        }

        throw new ParsingException($"Internal error: no implementation to convert value to '{targetType}'");
    }

    private static void SetValue(
        string key,
        string rawValue,
        object owner,
        IReadOnlyDictionary<string, PropertyInfo> propertiesCache
    )
    {
        var propertyInfo = propertiesCache.GetValueOrDefault(key) ?? throw new ArgumentException(
            $"Can't find property for key '{key}' in type '{owner.GetType()}'.", nameof(key)
        );

        var existingValue = propertyInfo.GetValue(owner);
        if (existingValue != null)
        {
            throw new DuplicateKeyException(key, existingValue, rawValue);
        }

        // if (rawValue == "")
        // {
        //     throw new ParsingException("beans");
        // }

        var type = propertyInfo.PropertyType;
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            type = type.GetGenericArguments()[0];
        }

        object converted;
        try
        {
            converted = StringToValue(rawValue, type);
        }
        catch (Exception e)
        {
            throw new ParsingException($"Unable to parse value '{rawValue}' for key '{key}' as type '{type.Name}'.", e);
        }
        propertyInfo.SetValue(owner, converted);
    }
}

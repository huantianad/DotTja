namespace DotTja;

using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;
using EnumConverter;
using Exceptions;
using Types;
using Types.Builders;

internal static class Parser
{
    private static string ReadLineOrThrow(this TextReader reader) =>
        reader.ReadLine() ?? throw new ParsingException("Encountered end of stream early!");

    public static TjaFile Deserialize(TextReader reader)
    {
        var result = new TjaFileBuilder();

        var doneParsingMainMetadata = false;

        while (true)
        {
            var line = reader.ReadLineOrThrow().Trim();

            if (line == "" || line.StartsWith("//", StringComparison.InvariantCulture))
            {
                continue;
            }

            var split = line.Split(':', 2);
            if (split.Length != 2)
            {
                throw new ParsingException($"Expected key-value pair, but line is missing colon separator: {line}");
            }

            var (key, value) = (split[0].Trim(), split[1].Trim());

            if (key == "COURSE")
            {
                doneParsingMainMetadata = true;
            }

            if (!doneParsingMainMetadata)
            {
                ParseLineOfMainMetadata(result.Metadata, key, value);
            }
            else
            {
                break;
            }

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
            SetValue(metadata.Title, languageKey, value, LocalizedStringBuilder.Properties);
        }
        else if (key.StartsWith("SUBTITLE", StringComparison.InvariantCulture))
        {
            var languageKey = key == "SUBTITLE"
                ? "DEFAULT"
                : key["SUBTITLE".Length..];
            SetValue(metadata.Subtitle, languageKey, value, LocalizedStringBuilder.Properties);
        }
        else
        {
            SetValue(metadata, key, value, MetadataBuilder.Properties);
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
        object owner,
        string key,
        string rawValue,
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

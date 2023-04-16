namespace DotTja;

using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
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

    public static void SetValue(
        object owner,
        string key,
        string rawValue,
        IReadOnlyDictionary<string, PropertyInfo> propertiesCache,
        Func<string, object>? customParser = null
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

        object parsedValue;

        var propertyType = propertyInfo.PropertyType;
        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            propertyType = propertyType.GetGenericArguments()[0];
        }

        if (customParser != null)
        {
            parsedValue = customParser(rawValue);
        }
        else if (propertyType == typeof(string))
        {
            parsedValue = rawValue;
        }
        else if (propertyType == typeof(int))
        {
            if (int.TryParse(rawValue, out var result))
            {
                parsedValue = result;
            }
            else
            {
                throw new ParsingException($"Unable to parse value '{rawValue}' for key '{key}' as int.");
            }
        }
        else if (propertyType == typeof(double))
        {
            if (double.TryParse(rawValue, out var result))
            {
                parsedValue = result;
            }
            else
            {
                throw new ParsingException($"Unable to parse value '{rawValue}' for key '{key}' as double.");
            }
        }
        else if (propertyType == typeof(FileInfo))
        {
            parsedValue = new FileInfo(rawValue);
        }
        else if (propertyType == typeof(TaikoWebSkin))
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

            parsedValue = new TaikoWebSkin(
                new DirectoryInfo(pairs["dir"]),
                pairs["name"],
                pairs.GetValueOrDefault("song"),
                pairs.GetValueOrDefault("stage"),
                pairs.GetValueOrDefault("don")
            );
        }
        else if (propertyType.IsEnum)
        {
            if (Enum.TryParse(propertyType, rawValue, out var result))
            {
                Debug.Assert(result != null, nameof(result) + " != null");
                parsedValue = result;
            }
            else
            {
                throw new ParsingException($"Unable to parse value '{rawValue}' for key '{key}' as Enum of type '{propertyType.Name}'.");
            }
        }
        else
        {
            throw new ParsingException(
                $"Internal error: attempted to set value of property with type '{propertyType} "
                + "but no code was implemented for this type."
            );
        }

        propertyInfo.SetValue(owner, parsedValue);
    }
}

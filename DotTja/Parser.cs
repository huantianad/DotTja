namespace DotTja;

using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Exceptions;
using Types;
using Types.Commands;
using Types.Enums;

internal static class Parser
{
    public static TjaFile Deserialize(ParserReader reader)
    {
        var result = new TjaFile.Builder();

        while (true)
        {
            var (key, value) = reader.ReadKeyValuePair(
                "Encountered end of stream when parsing metadata."
            );

            if (key == "COURSE")
            {
                reader.ReuseLastLine();
                break;
            }

            ParseLineOfMainMetadata(result.Metadata, key, value);
        }

        while (reader.PeekContentLine() != null)
        {
            var course = ParseCourse(reader);
            result.Courses.Add(course);
        }

        return result.ToTjaFile();
    }

    private static void ParseLineOfMainMetadata(Metadata.Builder metadata, string key, string value)
    {
        if (key.StartsWith("TITLE", StringComparison.InvariantCulture))
        {
            var languageKey = key == "TITLE"
                ? "DEFAULT"
                : key["TITLE".Length..];
            SetValue(languageKey, value, metadata.Title, LocalizedString.Builder.Properties);
        }
        else if (key.StartsWith("SUBTITLE", StringComparison.InvariantCulture))
        {
            var languageKey = key == "SUBTITLE"
                ? "DEFAULT"
                : key["SUBTITLE".Length..];
            SetValue(languageKey, value, metadata.Subtitle, LocalizedString.Builder.Properties);
        }
        else
        {
            SetValue(key, value, metadata, Metadata.Builder.Properties);
        }
    }

    private static Course ParseCourse(ParserReader reader)
    {
        var result = new Course.Builder();

        (string, string) ReadKeyValuePair() =>
            reader.ReadKeyValuePair("Encountered end of stream when parsing course metadata.");

        // The first line of each Course should be COURSE:Difficulty
        // Assume this is true and parse it separately
        // This is especially important since in the next while loop, we use a COURSE key as a
        // sign that the current Course section is ending and the next one is starting.
        {
            var (key, value) = ReadKeyValuePair();
            if (key != "COURSE")
            {
                throw new ParsingException(
                    $"Expected first key of course metadata to be COURSE, but it was '{key}:{value}' instead."
                );
            }

            result.Difficulty = EnumConverter.EnumConverter.Parse<Difficulty>(value);
        }

        var activeVariant = result.SingleCourse;

        while (true)
        {
            if (reader.PeekContentLine()?.StartsWith('#') ?? false)
            {
                activeVariant.Player1Commands = ParseSongSection(reader);

                if (reader.PeekContentLine() == null)
                {
                    break;
                }

                // Don't go straight back to parsing metadata below, as there could be another
                // start block (p2 start) immediately after a start (p1 start)
                continue;
            }

            var (key, value) = ReadKeyValuePair();

            if (key == "COURSE")
            {
                reader.ReuseLastLine();
                break;
            }
            else if (key == "LEVEL")
            {
                result.Stars = int.Parse(value, CultureInfo.InvariantCulture);
            }
            else if (key == "STYLE")
            {
                var style = EnumConverter.EnumConverter.Parse<Style>(value);
                activeVariant = style == Style.Single ? result.SingleCourse : result.DoubleCourse;
            }
            else if (key is "NOTESDESIGNER3" or "NOTESDESIGNER2" or "NOTESDESIGNRE3")
            {
                // TODO: Figure out what to do with this key that's taikocatscaffe only
                continue;
            }
            else
            {
                SetValue(key, value, activeVariant, CourseVariant.Builder.Properties);
            }
        }

        return result.ToCourse();
    }

    private static ImmutableList<Command> ParseSongSection(ParserReader reader)
    {
        Debug.Assert(reader.ReadContentLine().StartsWith("#START", StringComparison.InvariantCulture));
        while (reader.ReadContentLine() != "#END")
        {

        }

        return ImmutableList<Command>.Empty;
    }

    private static object StringToValue(string rawValue, Type targetType)
    {
        if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            targetType = targetType.GetGenericArguments()[0];
        }

        if (targetType == typeof(string))
        {
            return rawValue;
        }
        if (targetType == typeof(bool))
        {
            return rawValue switch
            {
                "0" => false,
                "1" => true,
                _ => throw new ParsingException($"Expected value '{rawValue}' to either be '0' or '1'.")
            };
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
        if (targetType == typeof(ImmutableList<int>))
        {
            return rawValue
                .TrimEnd(',') // Remove possible trailing commas TODO: should this be strict
                .Split(',')
                .Select(StringToValue<int>)
                .ToImmutableList();
        }
        if (targetType == typeof((int, int?)))
        {
            var split = rawValue.Split(',').Select(StringToValue<int>).ToImmutableList();
            return split.Count switch
            {
                1 => new ValueTuple<int, int?>(split[0], null),
                2 => new ValueTuple<int, int?>(split[0], split[1]),
                _ => throw new ParsingException($"Expected value '{rawValue}' to be of form 'int' or 'int,int'.")
            };
        }

        throw new ParsingException($"Internal error: no implementation to convert value to '{targetType}'");
    }

    private static T StringToValue<T>(string rawValue) => (T) StringToValue(rawValue, typeof(T));

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
            if (existingValue.GetType().IsGenericType &&
                existingValue.GetType().GetGenericTypeDefinition() == typeof(ImmutableList<>) &&
                existingValue.GetType().GetGenericArguments()[0] == typeof(int))
            {
                existingValue = string.Join(",", (ImmutableList<int>) existingValue);
            }
            throw new DuplicateKeyException(key, existingValue, rawValue);
        }

        // TODO: Add strict mode for blank fields
        if (rawValue == "")
        {
            return;
        }

        var type = propertyInfo.PropertyType;
        object converted;
        try
        {
            converted = StringToValue(rawValue, type);
        }
        catch (Exception e)
        {
            throw new ParsingException(
                $"Unable to parse value '{rawValue}' for key '{key}' as type '{type.Name}'.", e
            );
        }
        propertyInfo.SetValue(owner, converted);
    }
}

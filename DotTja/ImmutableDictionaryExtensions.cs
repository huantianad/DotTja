namespace DotTja;

using System.Collections.Immutable;

public static class ImmutableDictionaryExtensions
{
    public static ImmutableDictionary<TKey, TValue> MoveKey<TKey, TValue>(
        this ImmutableDictionary<TKey, TValue> dict, TKey originalKey, TKey newKey
    ) where TKey : notnull
    {
        var value = dict[originalKey];
        return dict.Remove(originalKey).Add(newKey, value);
    }
}

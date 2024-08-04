using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System;

public static class StringExtensions
{
    public static string FixedWidth(this string source, int width)
    {
        return source?.PadRight(width) ?? new string(' ', width);
    }

    public static string GetInitials(this string source)
    {
        var parts = source.Split(" ")
            .Select(part =>
            {
                if (string.IsNullOrEmpty(part))
                    return string.Empty;
                if (part.Length == 1)
                    return part.ToUpper();

                return part[..1].ToUpper();
            });

        return string.Join("", parts);
    }

    public static string? RemoveDiacritics(this string source)
    {
        if (string.IsNullOrEmpty(source))
            return source;

        var normalisedCharaters = source
            .Normalize(NormalizationForm.FormD)
            .Where(x => CharUnicodeInfo.GetUnicodeCategory(x) != UnicodeCategory.NonSpacingMark)
            .ToArray();

        var result = Regex.Replace(new string(normalisedCharaters),
            @"[^\u0000-\u007F]+", string.Empty);

        return result;
    }

    public static string RemoveInstancesOfString(this string source, IEnumerable<string> values)
    {
        if (string.IsNullOrEmpty(source))
            return source;

        return values.Aggregate(source, (x, value) => x.Replace(value, ""));
    }

    public static string RemoveSpaces(this string source)
    {
        if (string.IsNullOrEmpty(source))
            return source;

        return source.Replace(" ", "");
    }

    public static string RemoveTrailingInstanceOfString(this string source, string toRemove)
    {
        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(toRemove))
            return source;

        var instanceOffset = source.TrimEnd().EndsWith(toRemove) ? source.LastIndexOf(toRemove) : -1;
        return (instanceOffset == -1) ? source : source[..instanceOffset];
    }

    public static string RemoveTrailingInstancesOfString(this string source, IEnumerable<string> values)
    {
        if (string.IsNullOrEmpty(source))
            return source;

        var instanceOffset = values.Min(v => source.EndsWith(v) ? source.LastIndexOf(v) : int.MaxValue);
        return (instanceOffset == int.MaxValue) ? source : source[..instanceOffset];
    }

    public static string SubstringFromLastIndexOf(this string source, char value)
    {
        if (string.IsNullOrEmpty(source))
            return source;

        var index = source.LastIndexOf(value);
        if (index < 0)
            return string.Empty;
        return source[(index + 1)..];
    }

    public static string SubstringToLastIndexOf(this string source, char value)
    {
        if (string.IsNullOrEmpty(source))
            return source;

        var index = source.LastIndexOf(value);
        if (index < 0)
            return source;
        return source[..index];
    }

    public static string TakeFirstCharacters(this string source, int count, char padding = ' ')
    {
        var paddingString = new string(padding, count);

        if (string.IsNullOrEmpty(source))
            return paddingString;

        if (source.Length < count)
            return (source + paddingString)[..count];

        return source[..count];
    }

    public static string ToTitleCase(this string source)
    {
        if (string.IsNullOrEmpty(source))
            return source;

        var parts = source.Split(" ")
            .Select(part =>
            {
                if (string.IsNullOrEmpty(part))
                    return part;
                if (part.Length == 1)
                    return part.ToUpper();

                return part[..1].ToUpper() + part[1..].ToLower();
            });

        return string.Join(" ", parts);
    }

    public static string TrimWhitespaceAndPunctuation(this string source)
    {
        if (string.IsNullOrWhiteSpace(source))
            return string.Empty;

        var index = source.Length - 1;
        while (char.IsWhiteSpace(source, index) || char.IsPunctuation(source, index))
            index--;

        return source[..(index + 1)];
    }

    public static string TrimWhitespaceAndPunctuation(this string source, char[] allowedPunctuation)
    {
        if (string.IsNullOrWhiteSpace(source))
            return string.Empty;

        var index = source.Length - 1;
        while (char.IsWhiteSpace(source, index) || (char.IsPunctuation(source, index) && !allowedPunctuation.Contains(source[index])))
            index--;

        return source[..(index + 1)];
    }

    public static string TruncateMultipleOccurancesOfChar(this string source, char c)
    {
        if (string.IsNullOrEmpty(source))
            return source;

        var s = new string(c, 1);

        return source
            .Replace(s, $"{s}\a")
            .Replace($"\a{s}", "")
            .Replace("\a", "");
    }

    public static string TruncateMultipleSpaces(this string source)
    {
        return TruncateMultipleOccurancesOfChar(source, ' ');
    }
}

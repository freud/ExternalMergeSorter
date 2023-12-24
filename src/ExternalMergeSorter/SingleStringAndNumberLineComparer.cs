namespace Sorter;

internal sealed class StringAndNumberLinesComparer : IComparer<string>
{
    private const string Separator = ". ";

    public int Compare(string? left, string? right)
    {
        if (left == null) return -1;
        if (right == null) return 1;

        int leftSeparatorIdx = left.IndexOf(Separator, 1, StringComparison.Ordinal),
            rightSeparatorIdx = right.IndexOf(Separator, 1, StringComparison.Ordinal);

        ReadOnlySpan<char> leftString = left.AsSpan(leftSeparatorIdx + Separator.Length);
        ReadOnlySpan<char> rightString = right.AsSpan(rightSeparatorIdx + Separator.Length);

        var result = leftString.CompareTo(rightString, StringComparison.Ordinal);

        if (result != 0)
        {
            return result;
        }

        var leftNumber = int.Parse(left.AsSpan(0, leftSeparatorIdx));
        var rightNumber = int.Parse(right.AsSpan(0, rightSeparatorIdx));
        return leftNumber.CompareTo(rightNumber);
    }
}
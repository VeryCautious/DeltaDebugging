using FluentAssertions;

namespace InputSimplification;

public class DdMinTests
{

    [Fact]
    public void SlidesExample_DdMin_11()
    {
        var data = new[] { 1, 3, 5, 3, 9, 17, 44, 3, 6, 1, 1, 0, 44, 1, 44, 0 };
        var inpSimplification = new InputSimplification<int>(NoConsecutiveNumbersValidator);

        var reduced = inpSimplification.DdMin(data);

        reduced.Should().BeEquivalentTo(new[] { 1, 1 });
    }

    [Fact]
    public void Task1_DdMin_34()
    {
        var data = new[] { 9, 8, 7, 4, 3, 7, 4, 5, 1, 5, 2, 3, 3, 3, 4, 6 };
        var inpSimplification = new InputSimplification<int>(No3FollowedBy4NumbersValidator);

        var reduced = inpSimplification.DdMin(data);

        reduced.Should().BeEquivalentTo(new[] { 3, 4 });
    }

    [Fact]
    public void Task2_DdMin_777()
    {
        var data = new[] { 7, 8, 7, 1, 8, 1, 8, 7, 7, 7, 1, 7, 7, 1, 8, 1 };
        var inpSimplification = new InputSimplification<int>(NoTripleConsecutiveNumbersValidator);

        var reduced = inpSimplification.DdMin(data);

        reduced.Should().BeEquivalentTo(new[] { 7, 7, 7 });
    }

    [Fact]
    public void Task3_DdMin_33_33()
    {
        var data = new[] { 7, -66, 23, 121, 33, 33, 23, 5, -121, 30 };
        var inpSimplification = new InputSimplification<int>(NoConsecutivePalindromeValidator);

        var reduced = inpSimplification.DdMin(data);

        reduced.Should().BeEquivalentTo(new[] { 33, 33 });
    }

    private static bool NoConsecutiveNumbersValidator(int[] data) => 
        data.
            Zip(data.Skip(1), Tuple.Create).
            Any(tuple => tuple.Item1 == tuple.Item2);

    private static bool No3FollowedBy4NumbersValidator(int[] data) => 
        data.
            Zip(data.Skip(1), Tuple.Create).
            Any(tuple => tuple is { Item1: 3, Item2: 4 });

    private static bool NoTripleConsecutiveNumbersValidator(int[] data) =>
        data.
            Zip(data.Skip(1), Tuple.Create).
            Zip(data.Skip(2), (t, v) => Tuple.Create(t.Item1, t.Item2, v)).
            Any(tuple => tuple.Item1 == tuple.Item2 && tuple.Item1 == tuple.Item3);

    private static bool NoConsecutivePalindromeValidator(int[] data) =>
        data.
            Zip(data.Skip(1), Tuple.Create).
            Any(tuple => IsPalindrome(tuple.Item1.ToString(), tuple.Item2.ToString()));

    private static bool IsPalindrome(string s1, string s2) => 
        new string(s1.Reverse().ToArray()) == s2;
}
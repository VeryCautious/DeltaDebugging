namespace InputSimplification;

public class InputSimplification<T>
{
    private readonly Func<T[], bool> _validator;

    public InputSimplification(Func<T[], bool> validator)
    {
        _validator = validator;
    }

    /// <summary>
    /// Delta Debugging - Input Simplification
    /// based on https://www.cs.columbia.edu/~junfeng/08fa-e6998/sched/readings/delta-debug-input.pdf
    /// </summary>
    /// <param name="data">The input to be simplified</param>
    /// <returns>a minimum failing input</returns>
    public T[] DdMin(T[] data) => DdMin(data, 2);

    private T[] DdMin(T[] data, int n)
    {
        if (HasFailingSubSet(data, n, out var failingSubSet))
        {
            return DdMin(failingSubSet!, 2); //Reduce to subset
        }

        if (IsFailingWithoutSubSet(data, n, out var complement))
        {
            return DdMin(complement!, Math.Max(n-1, 2)); //Reduce to complement
        }

        if (n < data.Length)
        {
            return DdMin(data, 2*n); //increase granularity
        }

        return data;
    }

    private bool IsFailingWithoutSubSet(IEnumerable<T> data, int n, out T[]? complement)
    {
        var chunks = Split(data, n).ToArray();

        complement = Enumerable.Range(0, chunks.Length).
            Select(i => chunks.Take(i).Concat(chunks.Skip(i + 1))).
            Select(complement => complement.Aggregate(Enumerable.Empty<T>(),Enumerable.Concat)).
            Select(enumerable => enumerable.ToArray()).
            FirstOrDefault(_validator);

        return complement != null;
    }

    private bool HasFailingSubSet(IEnumerable<T> data, int n, out T[]? failingSubSet)
    {
        failingSubSet = Split(data, n).FirstOrDefault(_validator);

        return failingSubSet != null;
    }

    private static IEnumerable<T[]> Split(IEnumerable<T> data, int amountOfChunks)
    {
        var dataArray = data as T[] ?? data.ToArray();
        var chunkSize = (int)Math.Ceiling((double)dataArray.Length / amountOfChunks);
        return dataArray.Chunk(chunkSize);
    }
}
using System.Threading;

public static class IdGenerator
{
    private static int _nextId = 0;

    public static int GenerateId()
    {
        return Interlocked.Increment(ref _nextId);
    }
}

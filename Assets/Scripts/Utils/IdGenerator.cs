using System;


public static class IdGenerator
{
    private static readonly Random _random = new Random();
        
    public static int GenerateId()
    {
        return _random.Next();
    }
}

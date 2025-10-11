using System;



public static class UUIDGenerator
{
    public static string GenerateUUID()
    {
        return Guid.NewGuid().ToString();
    }
}

using System;
namespace LibHammer.Util;

public class StdLogger : ILogger
{
    public void Write(string what)
    {
        Console.WriteLine(what);
    }
}
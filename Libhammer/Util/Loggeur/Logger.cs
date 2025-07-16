namespace LibHammer.Util;

static class Logger
{
    public static ILogger? logger = new StdLogger();

    public static void Write(string what)
    {
        if (logger != null) logger.Write(what);
    }
}
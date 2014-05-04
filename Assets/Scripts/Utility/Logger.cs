using UnityEngine;

/// <summary>
/// Release版本不输出log
/// </summary>
public class Logger
{
    public static void Log(object message)
    {
#if !RELEASE_VERSION
        Debug.Log(message);
#endif
    }

    public static void Log(object message, Object context)
    {
#if !RELEASE_VERSION
        Debug.Log(message, context);
#endif
    }

    public static void LogError(object message)
    {
#if !RELEASE_VERSION
        Debug.LogError(message);
#endif
    }

    public static void LogError(object message, Object context)
    {
#if !RELEASE_VERSION
        Debug.LogError(message, context);
#endif
    }

    public static void LogWarning(object message)
    {
#if !RELEASE_VERSION
        Debug.LogWarning(message);
#endif
    }

    public static void LogWarning(object message, Object context)
    {
#if !RELEASE_VERSION
        Debug.LogWarning(message, context);
#endif
    }

    public static void LogException(System.Exception exception)
    {
#if !RELEASE_VERSION
        Debug.LogException(exception);
#endif
    }

    public static void LogException(System.Exception exception, Object context)
    {
#if !RELEASE_VERSION
        Debug.LogException(exception, context);
#endif
    }
}

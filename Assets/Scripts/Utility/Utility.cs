using UnityEngine;
using System.Collections;
using System;

public class Utility
{
    /// <summary>
    /// 解析枚举，可能抛异常
    /// </summary>
    public static T EnumParse<T>(string str)
    {
        return (T)Enum.Parse(typeof(T), str);
    }

    /// <summary>
    /// 解析枚举，不抛异常
    /// </summary>
    public static bool EnumTryParse<T>(string str, out T ret)
    {
        try
        {
            ret = (T)Enum.Parse(typeof(T), str);
        }
        catch (Exception)
        {
            ret = default(T);
            return false;
        }
        return true;
    }

}

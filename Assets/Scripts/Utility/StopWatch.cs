using System;
using System.Collections.Generic;

/// <summary>
/// ��ʱ���
/// </summary>
public class StopWatch
{
    static List<DateTime> dateTimeList = new List<DateTime>();

    static StopWatch()
    {
        //dateTimeList = new List<DateTime>();
        Reset();
    }

    static public void Reset()
    {
        dateTimeList.Clear();
        dateTimeList.Add(DateTime.Now);
        Logger.Log("[StopWatch] -- Reseted");
    }

    static public DateTime GetStartTime()
    {
        return dateTimeList[0];
    }

    static public DateTime GetLastTime()
    {
        return dateTimeList[dateTimeList.Count - 1];
    }

    static public TimeSpan GetLastLap()
    {
        DateTime currentTime = DateTime.Now;
        DateTime lastTime = dateTimeList[dateTimeList.Count - 1];
        TimeSpan tsLap = currentTime - lastTime;
        return tsLap;
    }

    /// <summary>
    /// ��ʱ
    /// </summary>
    /// <param name="message">��ʾ��log��Ϣ</param>
    static public void Split(string message)
    {
        DateTime currentTime = DateTime.Now;
        DateTime lastTime = dateTimeList[dateTimeList.Count - 1];
        dateTimeList.Add(currentTime);
        TimeSpan tsLap = currentTime - lastTime;
        TimeSpan tsTotal = currentTime - GetStartTime();
        Logger.Log(string.Format("{0} || [StopWatch] --  Total: {1:F}s,   Lap: {2:F}s", message, tsTotal.TotalSeconds, tsLap.TotalSeconds));
    }
}

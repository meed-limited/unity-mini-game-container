using System;
using UnityEngine;

public static class ExtensionDateTime
{
    public static DateTime FromTimeStamp(this DateTime data, double timeStamp)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        DateTime newDate  =  dateTime.AddMilliseconds(timeStamp);
        return newDate;
    }
}
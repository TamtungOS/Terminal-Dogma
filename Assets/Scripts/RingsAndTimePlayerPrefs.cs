using UnityEngine;

public static class RingsAndTimePlayerPrefs
{
    const string ringsKey = "Rings";
    const string timeKey = "Time";

    public static void SetRings(int value)
    {
        PlayerPrefs.SetInt(ringsKey, value);
    }

    public static int GetRings()
    {
        return PlayerPrefs.GetInt(ringsKey);
    }

    public static void SetTime(float value)
    {
        PlayerPrefs.SetFloat(timeKey, value);
    }

    public static float GetTime()
    {
        return PlayerPrefs.GetFloat(timeKey);
    }

    public static void SavePlayerPrefs()
    {
        PlayerPrefs.Save();
    }
}

using UnityEngine;

public static class RingsAndTimePlayerPrefs
{
    const string RingsKey = "Rings";
    const string TimeKey = "Time";

    public static void SetRings(int Value)
    {
        PlayerPrefs.SetInt(RingsKey, Value);
    }

    public static int GetRings()
    {
       return PlayerPrefs.GetInt(RingsKey);
    }

    public static void SetTime(float Value)
    {
        PlayerPrefs.SetFloat(TimeKey, Value);
    }

    public static float GetTime()
    {
       return PlayerPrefs.GetFloat(TimeKey);
    }

    public static void SavePlayerPrefs()
    {
        PlayerPrefs.Save();
    }
}

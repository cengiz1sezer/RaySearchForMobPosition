using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    #region LEVEL

    const string KEY_LEVEL = "levels";

    public static void IncreaseLevel() => PlayerPrefs.SetInt(KEY_LEVEL, GetLevel() + 1);
    public static int GetLevel() => PlayerPrefs.GetInt(KEY_LEVEL, 0);

    #endregion

    #region COIN

    const string KEY_COIN = "coins";

    public static void AddCoin(int add) => PlayerPrefs.SetInt(KEY_COIN, GetCoin() + add);
    public static int GetCoin() => PlayerPrefs.GetInt(KEY_COIN, 0);

    #endregion

    #region COIN

    const string KEY_VIBRATION = "vibrator";

    public static bool HasVibration() => PlayerPrefs.GetInt(KEY_VIBRATION, 1) == 1;

    public static void ChangeVibrationStatus() { if (HasVibration()) SetVibrationStatus(false); else SetVibrationStatus(true); }

    public static void SetVibrationStatus(bool isEnabled) { PlayerPrefs.SetInt(KEY_VIBRATION, isEnabled ? 1 : 0); /*UIManager.I.UpdateHapticStatus()*/; }

    #endregion

    #region PRIZES

    const string KEY_PRIZES = "priozes_";

    public static bool HasPrizeTaken(int id) => PlayerPrefs.GetInt(KEY_PRIZES + id, 0) == 1;

    public static void SetPrizeTaken(int id) => PlayerPrefs.SetInt(KEY_PRIZES + id, 1);

    #endregion

    #region SAVE-LOAD BALLS
    const string KEY_BALLS = "balsxzs";
    const string KEY_ACTIVE_BALL = "Activision";
    public static void SaveBalls(string s)
    {
        PlayerPrefs.SetString(KEY_BALLS, s);
    }

    public static List<int> GetBalls()
    {
        List<int> ls = new List<int>();

        string s = PlayerPrefs.GetString(KEY_BALLS, "0,1,2");

        string[] all = s.Split(',');

        foreach (string _s in all)
        {
            if (!string.IsNullOrEmpty(_s.Trim()))
            {
                if (int.TryParse(_s.Trim(), out int o))
                {
                    ls.Add(o);
                }
            }

        }

        return ls;
    }

    public static void SaveActiveBall(int id)
    {
        PlayerPrefs.SetInt(KEY_ACTIVE_BALL, id);
    }

    public static int GetActiveBall()
    {
        return PlayerPrefs.GetInt(KEY_ACTIVE_BALL, 1);
    }
    #endregion

    #region SOUND
    const string KEY_SOUND = "Sounds";
    public static void ChangeSoundState()
    {
        int crntState = PlayerPrefs.GetInt(KEY_SOUND, 1);
        PlayerPrefs.SetInt(KEY_SOUND, (crntState + 1) % 2);
    }

    public static bool HasSound()
    {
        return PlayerPrefs.GetInt(KEY_SOUND, 1) == 1;
    }

    #endregion
}

using UnityEngine;

namespace Assets.Scripts
{
    /**
     * A wrapper class around PlayerPrefs to suit the specific needs of this game. 
     * Provides abstraction for keys and default values.
     */
    static class Prefs
    {
        // keys
        private static string KEY_MOUSE_SENSITIVITY = "mouse_sensitivity";
        private static string KEY_CASH = "cash";
        private static string KEY_DINER_NAME = "diner_name";

        // defaults
        private static float DEFAULT_MOUSE_SENSITIVITY = 0.5f; // float 0..1
        private static float DEFAULT_CASH = 0.0f;
        private static string DEFAULT_DINER_NAME = "Code Corner";

        /**
         * Get the int value for a key or return its default value if it does not exist
         */
        static private int GetIntOrDefault(string key, int def)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) : def;
        }

        /**
         * Get the float value for a key or return its default value if it does not exist
         */
        static private float GetFloatOrDefault(string key, float def)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : def;
        }

        /**
         * Get the string value for a key or return its default value if it does not exist
         */
        static private string GetStringOrDefault(string key, string def)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : def;
        }

        static public void ClearSavedLevelData()
        {
            SetCash(DEFAULT_CASH);
        }

        static public float GetMouseSensitivity()
        {
            return GetFloatOrDefault(KEY_MOUSE_SENSITIVITY, DEFAULT_MOUSE_SENSITIVITY);
        }

        static public void SetMouseSensitivity(float value)
        {
            PlayerPrefs.SetFloat(KEY_MOUSE_SENSITIVITY, value);
        }

        static public float GetCash()
        {
            return GetFloatOrDefault(KEY_CASH, DEFAULT_CASH);
        }

        static public void SetCash(float value)
        {
            PlayerPrefs.SetFloat(KEY_CASH, value);
        }

        static public string GetDinerName()
        {
            return GetStringOrDefault(KEY_DINER_NAME, DEFAULT_DINER_NAME);
        }

        static public void SetDinerName(string value)
        {
            PlayerPrefs.SetString(KEY_DINER_NAME, value);
        }
    }
}

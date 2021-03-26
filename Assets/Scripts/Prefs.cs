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
        private static string KEY_LEVEL_PROFIT = "level_profit";
        private static string KEY_DINER_NAME = "diner_name";
        private static string KEY_DAY_NUMBER = "day_number";
        private static string KEY_INVENTORY_BURGER = "burger_count";

        // defaults
        private static float DEFAULT_MOUSE_SENSITIVITY = 0.5f; // float 0..1
        private static float DEFAULT_CASH = 0.0f;
        private static string DEFAULT_DINER_NAME = "Code Corner";
        private static int DEFAULT_DAY_NUMBER = 1;
        private static int DEFAULT_INVENTORY_BURGER = 5;

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
            SetDinerName(DEFAULT_DINER_NAME);
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

        static public int GetDayNumber()
        {
            return GetIntOrDefault(KEY_DAY_NUMBER, DEFAULT_DAY_NUMBER);
        }

        static public void SetDayNumber(int value)
        {
            PlayerPrefs.SetInt(KEY_DAY_NUMBER, value);
        }

        static public float GetLevelProfit()
        {
            return GetFloatOrDefault(KEY_LEVEL_PROFIT, DEFAULT_CASH);
        }

        static public void SetLevelProfit(float value)
        {
            PlayerPrefs.SetFloat(KEY_LEVEL_PROFIT, value);
        }

        static public int GetInventoryBurger()
        {
            return GetIntOrDefault(KEY_INVENTORY_BURGER, DEFAULT_INVENTORY_BURGER);
        }

        static public void SetInventoryBurger(int value)
        {
            PlayerPrefs.SetInt(KEY_INVENTORY_BURGER, value);
        }

        static public void SetAllToDefault()
        {
            SetCash(DEFAULT_CASH);
            SetDayNumber(DEFAULT_DAY_NUMBER);
            SetDinerName(DEFAULT_DINER_NAME);
            SetLevelProfit(DEFAULT_CASH);
            SetMouseSensitivity(DEFAULT_MOUSE_SENSITIVITY);
            SetInventoryBurger(DEFAULT_INVENTORY_BURGER);
        }
    }
}

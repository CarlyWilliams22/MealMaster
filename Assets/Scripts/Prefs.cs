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
        private static string KEY_LEVEL_TIPS = "level_tips";
        private static string KEY_LEVEL_CUSTOMERS_SERVED = "customers_served";
        private static string KEY_DINER_NAME = "diner_name";
        private static string KEY_DAY_NUMBER = "day_number";
        private static string KEY_INVENTORY_BURGER = "burger_count";
        private static string KEY_INVENTORY_BUN = "bun_count";
        private static string KEY_INVENTORY_CUP = "cup_count";
        private static string KEY_GAME_IN_PROGRESS = "game_in_progress"; //game is not lost
        private static string KEY_EMPLOYEE_HIRED = "hired_employee";
        private static string KEY_CURRENT_SCENE = "current_scene";
        private static string KEY_POPULARITY_SCORE = "popularity_score";

        // defaults
        private static float DEFAULT_MOUSE_SENSITIVITY = 0.5f; // float 0..1
        private static float DEFAULT_CASH = 0.0f;
        private static int DEFAULT_LEVEL_CUSTOMERS_SERVED = 0;
        private static string DEFAULT_DINER_NAME = "The Prancing Pony";
        private static int DEFAULT_DAY_NUMBER = 0;
        private static int DEFAULT_INVENTORY_BURGER = 5;
        private static int DEFAULT_INVENTORY_BUN = 10;
        private static int DEFAULT_INVENTORY_CUP = 5;
        private static bool DEFAULT_GAME_IN_PROGRESS = true;
        private static bool DEFAULT_EMPLOYEE_HIRED = false;
        private static string DEFAULT_SCENE = "GameScene";
        private static float DEFAULT_POPULARITY_SCORE = 0;

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

        static private bool GetBoolOrDefault(string key, bool def)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) == 1 : def;

        }

        static private void SetBool(string key, bool value)
        {
            if (value)
            {
                PlayerPrefs.SetInt(key, 1);
            }
            else
            {
                PlayerPrefs.SetInt(key, 0);
            }
        }

        static public void ClearSavedLevelData()
        {
            SetAllToDefault();
        }

        static public bool GetIsGameInProgress()
        {
            return GetBoolOrDefault(KEY_GAME_IN_PROGRESS, DEFAULT_GAME_IN_PROGRESS);
        }

        static public void SetGameInProgress(bool value)
        {
            SetBool(KEY_GAME_IN_PROGRESS, value);
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

        static public int GetInventoryBun()
        {
            return GetIntOrDefault(KEY_INVENTORY_BUN, DEFAULT_INVENTORY_BUN);
        }

        static public void SetInventoryBun(int value)
        {
            PlayerPrefs.SetInt(KEY_INVENTORY_BUN, value);
        }

        static public int GetInventoryCup()
        {
            return GetIntOrDefault(KEY_INVENTORY_CUP, DEFAULT_INVENTORY_CUP);
        }

        static public void SetInventoryCup(int value)
        {
            PlayerPrefs.SetInt(KEY_INVENTORY_CUP, value);
        }

        static public bool GetEmployeeHired()
        {
            return GetBoolOrDefault(KEY_EMPLOYEE_HIRED, DEFAULT_EMPLOYEE_HIRED);
        }

        static public void SetEmployeeHired(bool value)
        {
            SetBool(KEY_EMPLOYEE_HIRED, value);
        }

        static public int GetLevelCustomersServed()
        {
            return GetIntOrDefault(KEY_LEVEL_CUSTOMERS_SERVED, DEFAULT_LEVEL_CUSTOMERS_SERVED);
        }

        static public void SetLevelCustomerServed(int value)
        {
            PlayerPrefs.SetInt(KEY_LEVEL_CUSTOMERS_SERVED, value);
        }

        static public float GetLevelTips()
        {
            return GetFloatOrDefault(KEY_LEVEL_TIPS, DEFAULT_CASH);
        }

        static public void SetLevelTips(float value)
        {
            PlayerPrefs.SetFloat(KEY_LEVEL_TIPS, value);
        }

        static public string GetCurrentScene()
        {
            return GetStringOrDefault(KEY_CURRENT_SCENE, DEFAULT_SCENE);
        }

        static public void SetCurrentScene(string value)
        {
            PlayerPrefs.SetString(KEY_CURRENT_SCENE, value);
        }

        static public float GetPopularityScore()
        {
            return GetFloatOrDefault(KEY_POPULARITY_SCORE, DEFAULT_POPULARITY_SCORE);
        }

        static public void SetPopularityScore(float value)
        {
            PlayerPrefs.SetFloat(KEY_POPULARITY_SCORE, value);
        }

        static public void SetAllToDefault()
        {
            SetCash(DEFAULT_CASH);
            SetDayNumber(DEFAULT_DAY_NUMBER);
            SetDinerName(DEFAULT_DINER_NAME);
            SetLevelProfit(DEFAULT_CASH);
            SetLevelCustomerServed(DEFAULT_LEVEL_CUSTOMERS_SERVED);
            SetMouseSensitivity(DEFAULT_MOUSE_SENSITIVITY);
            SetInventoryBurger(DEFAULT_INVENTORY_BURGER);
            SetInventoryBun(DEFAULT_INVENTORY_BUN);
            SetInventoryCup(DEFAULT_INVENTORY_CUP);
            SetGameInProgress(DEFAULT_GAME_IN_PROGRESS);
            SetEmployeeHired(DEFAULT_EMPLOYEE_HIRED);
            SetCurrentScene(DEFAULT_SCENE);
            SetPopularityScore(DEFAULT_POPULARITY_SCORE);
        }
    }
}

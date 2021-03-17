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

        // defaults
        private static float DEFAULT_MOUSE_SENSITIVITY = 0.5f; // float 0..1

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

        static public float GetMouseSensitivity()
        {
            return GetFloatOrDefault(KEY_MOUSE_SENSITIVITY, DEFAULT_MOUSE_SENSITIVITY);
        }

        static public void SetMouseSensitivity(float value)
        {
            PlayerPrefs.SetFloat(KEY_MOUSE_SENSITIVITY, value);
        }
    }
}

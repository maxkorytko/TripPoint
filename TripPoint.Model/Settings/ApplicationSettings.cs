using System;
using System.Collections.Generic;

namespace TripPoint.Model.Settings
{
    public sealed class ApplicationSettings
    {
        private static ApplicationSettings _instance;

        private static IDictionary<string, Object> _settingsStore;

        private ApplicationSettings() { }

        public static void Initialize(IDictionary<string, Object> settingsStore)
        {
            if (settingsStore == null)
                throw new ArgumentException("settingsStore");

            _settingsStore = settingsStore;
            _instance = new ApplicationSettings();
        }

        public static ApplicationSettings Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("instance is not initialized");

                return _instance;
            }
        }

        public void SaveSetting(string key, Object value)
        {
            if (key == null) return;

            // do not save null values unless it has been saved before
            if (value == null && !_settingsStore.ContainsKey(key)) return;

            _settingsStore[key] = value;
        }

        public T GetSetting<T>(string key, T defaultValue)
        {
            T result = defaultValue;

            if (key != null && _settingsStore.ContainsKey(key))
            {
                var value = _settingsStore[key];

                result = (value == null) ? default(T) : (T)value;
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;

namespace TripPoint.WindowsPhone.State
{
    public class StateManager
    {
        private static StateManager _instance;
        private static IDictionary<string, Object> _state;

        public static void Initialize(IDictionary<string, Object> stateStore)
        {
            if (stateStore == null)
                throw new ArgumentException("stateStore");

            _state = stateStore;
            _instance = new StateManager();
        }

        private StateManager() { }

        public static StateManager Instance
        {
            get
            {
                if (_instance == null)
                    throw new InvalidOperationException("Instance is not initialized");

                return _instance;
            }
        }

        public void Set<T>(string key, T value)
        {
            _state[key] = value;
        }

        public T Get<T>(string key, T defaultValue = default(T))
        {
            object value;
            
            if (!_state.TryGetValue(key, out value))
            {
                value = defaultValue;
            }

            return (T)value;
        }

        public void Remove(string key)
        {
            _state.Remove(key);
        }
    }
}

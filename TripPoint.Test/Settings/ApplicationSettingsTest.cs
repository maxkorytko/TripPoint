using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TripPoint.Model.Settings;

namespace TripPoint.Test.Settings
{
    [TestClass]
    public class ApplicationSettingsTest
    {
        private IDictionary<string, Object> _settingsStore = new Dictionary<string, Object>();

        [ClassInitialize]
        public void InitializeClass()
        {
            ApplicationSettings.Initialize(_settingsStore);
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _settingsStore.Clear();
        }

        [TestMethod]
        public void TestSaveSetting()
        {
            var key = "key";
            var value = 21;

            ApplicationSettings.Instance.SaveSetting(key, value);

            var setting = (int)_settingsStore[key];

            Assert.AreEqual<int>(value, setting, "The setting was not saved in settings store");
        }

        [TestMethod]
        public void TestSaveSettingWithNull()
        {
            var key = "key";

            ApplicationSettings.Instance.SaveSetting(key, null);

            Assert.IsFalse(_settingsStore.ContainsKey(key), "Null setting must not be saved in settings");
        }

        [TestMethod]
        public void TestSaveSettingSetExistingSettingToNull()
        {
            var key = "key";

            ApplicationSettings.Instance.SaveSetting(key, DateTime.Now);
            ApplicationSettings.Instance.SaveSetting(key, null);

            Assert.IsTrue(_settingsStore.ContainsKey(key), "The settings store must contain the setting");

            var setting = _settingsStore[key];

            Assert.IsNull(setting, "Setting value must be null");
        }

        [TestMethod]
        public void TestGetSetting()
        {
            var key = "key";
            var value = "value";

            _settingsStore[key] = value;

            var setting = ApplicationSettings.Instance.GetSetting<string>(key, string.Empty);

            Assert.AreEqual(value, setting, "The saved setting must be equal to the retrieved one");
        }

        [TestMethod]
        public void TestGetSettingKeyDoesNotExist()
        {
            var setting = ApplicationSettings.Instance.GetSetting<int>("key", 25);

            Assert.AreEqual<int>(25, setting, "Default value must be returned if setting is not in the settings store");
        }

        [TestMethod]
        public void TestGetSettingNullKey()
        {
            var setting = ApplicationSettings.Instance.GetSetting<string>(null, "default");

            Assert.AreEqual<string>("default", setting, "Default value must be returned if key is null");
        }

        [TestMethod]
        public void TestGetSettingNullValue()
        {
            var key  = "key";
            var key2 = "key2";

            _settingsStore[key]  = null;
            _settingsStore[key2] = null;

            var setting  = ApplicationSettings.Instance.GetSetting<int>(key, 14092012);
            var setting2 = ApplicationSettings.Instance.GetSetting<string>(key, string.Empty);

            Assert.AreEqual<int>(0, setting, "Setting must be equal to the default value for the type, e.g. 0 for int, false for bool");
            Assert.IsNull(setting2);
        }

        [TestMethod]
        public void TestSaveAndGetSettings()
        {
            var key = "key";
            var value = "testing";

            ApplicationSettings.Instance.SaveSetting(key, value);

            var setting = ApplicationSettings.Instance.GetSetting<string>(key, string.Empty);

            Assert.AreEqual<string>(value, setting, "Saved and retrieved setting value must be the same");
        }
    }
}

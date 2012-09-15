using System;
using System.Windows.Input;

using TripPoint.Model.Settings;
using TripPoint.WindowsPhone.Services;
using TripPoint.WindowsPhone.I18N;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class ApplicationSettingsViewModel : TripPointViewModelBase
    {
        private Nullable<bool> _locationServiceToggleIsChecked;
        private string _locationServiceSettingDescription;

        public ApplicationSettingsViewModel()
        {
            LocationServiceToggleIsChecked = ApplicationSettings.Instance.GetSetting<bool>(LocationService.ENABLED_SETTING_KEY,
                LocationService.ENABLED_SETTING_DEFAULT_VALUE);

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            LocationServiceToggleChangedCommand = new RelayCommand(LocationServiceToggleChangedAction);
        }

        public Nullable<bool> LocationServiceToggleIsChecked
        {
            get { return _locationServiceToggleIsChecked; }
            set
            {
                if (_locationServiceToggleIsChecked == value) return;

                _locationServiceToggleIsChecked = value;
                RaisePropertyChanged("LocationServiceToggleIsChecked");

                UpdateLocationServiceSettingDescription();
            }
        }

        private void UpdateLocationServiceSettingDescription()
        {
            if (LocationServiceToggleIsChecked.Value)
                LocationServiceSettingDescription = Resources.LocationServiceOnSettingDescription;
            else
                LocationServiceSettingDescription = Resources.LocationServiceOffSettingDescription;
        }

        public string LocationServiceSettingDescription
        {
            get { return _locationServiceSettingDescription; }
            set
            {
                if (value != null && value.Equals(_locationServiceSettingDescription)) return;

                _locationServiceSettingDescription = value;
                RaisePropertyChanged("LocationServiceSettingDescription");
            }
        }

        public ICommand LocationServiceToggleChangedCommand { get; private set; }

        private void LocationServiceToggleChangedAction()
        {
            if (!LocationServiceToggleIsChecked.HasValue) return;

            UpdateLocationServiceSettingDescription();
            SaveSetting(LocationService.ENABLED_SETTING_KEY, LocationServiceToggleIsChecked.Value);
        }

        private static void SaveSetting(string key, Object value)
        {
            ApplicationSettings.Instance.SaveSetting(key, value);
        }
    }
}

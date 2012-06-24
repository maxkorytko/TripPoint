#region SDK Usings

using System;
using System.Windows;
using System.Windows.Input;

#endregion

using GalaSoft.MvvmLight.Command;
using TripPoint.Model.Domain;
using TripPoint.Model.Utils;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CreateCheckpointViewModel : TripPointViewModelBase
    {
        public CreateCheckpointViewModel()
        {
            Checkpoint = new Checkpoint
            {
                Timestamp = DateTime.Now
            };

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            CreateCheckpointCommand = new RelayCommand(CreateCheckpointAction);
            CancelCreateCheckpointCommand = new RelayCommand(CancelCreateCheckpointAction);
            AddPicturesCommand = new RelayCommand(AddPicturesAction);
        }

        public Checkpoint Checkpoint { get; private set; }

        public ICommand CreateCheckpointCommand { get; private set; }

        public ICommand CancelCreateCheckpointCommand { get; private set; }

        public ICommand AddPicturesCommand { get; private set; }

        private void CreateCheckpointAction()
        {
            Logger.Log(this, "{0}", Checkpoint);
        }

        private void CancelCreateCheckpointAction()
        {
            if ((Application.Current as App).RootFrame.CanGoBack)
                (Application.Current as App).RootFrame.GoBack();
        }

        private void AddPicturesAction()
        {
            Logger.Log(this, "Add pictures");
        }
    }
}

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TripPoint.Model.Domain;
using TripPoint.Model.Data.Repository.Factory;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.ViewModel;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointDetailsViewModel : TripPointViewModelBase
    {
        public CheckpointDetailsViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            Checkpoint = new Checkpoint();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            EditCheckpointCommand = new RelayCommand(EditCheckpointAction);
            DeleteCheckpointCommand = new RelayCommand(DeleteCheckpointAction);
        }

        public Checkpoint Checkpoint { get; private set; }

        public ICommand EditCheckpointCommand { get; private set; }

        public ICommand DeleteCheckpointCommand { get; private set; }

        private void EditCheckpointAction()
        {
            Logger.Log(this, "Edit checkpoint");
        }

        private void DeleteCheckpointAction()
        {
            Logger.Log(this, "Delete checkpoint");
        }

        public override void OnNavigatedTo(Navigation.TripPointNavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Logger.Log(this, "checkpoint ID: {0}", e.View.TryGetQueryStringParameter("checkpointID"));
        }
    }
}

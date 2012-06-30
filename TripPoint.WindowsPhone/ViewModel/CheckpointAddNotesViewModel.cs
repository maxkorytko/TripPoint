#region SDK Usings
using System;
using System.Windows;
using System.Windows.Input;
using System.Linq;
#endregion

using GalaSoft.MvvmLight.Command;
using TripPoint.Model.Utils;
using TripPoint.WindowsPhone.Navigation;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class CheckpointAddNotesViewModel : TripPointViewModelBase
    {
        public CheckpointAddNotesViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddNotesCommand = new RelayCommand(AddNotesAction);
            CancelAddNotesCommand = new RelayCommand(CancelAddNotesAction);
        }

        public string Notes { get; set; }

        public ICommand AddNotesCommand { get; private set; }

        public ICommand CancelAddNotesCommand { get; private set; }

        private void AddNotesAction()
        {
            var checkpointIndex = GetCheckpointIndex();

            if (checkpointIndex != -1)
            {
                AddNotesToCheckpoint(checkpointIndex, Notes);
            }

            TripPointNavigation.GoBack();
        }

        private static int GetCheckpointIndex()
        {
            var index = -1;

            var page = TripPointNavigation.CurrentPage;

            if (page != null)
            {   
                var checkpointIndexParameter = page.TryGetQueryStringParameter("checkpointIndex");
                if (!Int32.TryParse(checkpointIndexParameter, out index))
                    index = -1;
            }

            return index;
        }

        private void AddNotesToCheckpoint(int checkpointIndex, string notes)
        {
            Logger.Log(this, string.Format("checkpoint {0}, notes '{1}'", checkpointIndex, notes));
        }

        private void CancelAddNotesAction()
        {
            TripPointNavigation.GoBack();
        }
    }
}

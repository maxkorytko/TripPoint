﻿using System.Windows.Input;

using TripPoint.Model.Data.Repository.Factory;
using GalaSoft.MvvmLight.Command;

namespace TripPoint.WindowsPhone.ViewModel
{
    public class TripEditViewModel : Base.TripViewModelBase
    {
        public TripEditViewModel(IRepositoryFactory repositoryFactory)
            : base(repositoryFactory)
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SaveTripCommand = new RelayCommand(SaveTripAction);
            CancelEditTripCommand = new RelayCommand(CancelEditTripAction);
        }

        public ICommand SaveTripCommand { get; private set; }

        public ICommand CancelEditTripCommand { get; private set; }

        private void SaveTripAction()
        {
            TripRepository.UpdateTrip(Trip);

            Navigator.GoBack();
        }

        private void CancelEditTripAction()
        {
            Navigator.GoBack();
        }
    }
}

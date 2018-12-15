using System;
using System.ComponentModel;
using JetBrains.Annotations;
using Model.DTO;
using ViewModels.Utils;

namespace ViewModels.VM
{
    public class StaffVM: StaffModel, INotifyPropertyChanged
    {
        private int _voting;
        private bool _isNotVoted = true;

        public static StaffVM GetVMFromDTO([NotNull] StaffModel model)
        {
            return ViewModelConverter.CastToView(model);
        }

        public int Voting
        {
            get { return _voting; }
            set
            {
                _voting = value;
                OnPropertyChanged(nameof(Voting));
            }
        }

        public int CurrentEventId { get; set; }

        public bool IsNotVoted
        {
            get { return _isNotVoted; }
            set
            {
                _isNotVoted = value;
                OnPropertyChanged(nameof(IsNotVoted));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raised when a property has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that was changed.</param>
        protected void OnPropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void GetInfoFromDataModel(StaffModel staffModel)
        {
            ViewModelConverter.CastToView(this, staffModel);
            OnPropertyChanged(nameof(Id));
        }
    }
}

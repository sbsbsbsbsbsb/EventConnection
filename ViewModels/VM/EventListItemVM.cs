using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using Model.DTO;
using Model.DTO.Enum;
using Tools.Extentions;
using ViewModels.Utils;

namespace ViewModels.VM
{
    public class EventListItemVM : EventModel
    {
        private int _voting;
        private bool _isNotVoted = true;

        public static EventListItemVM GetVMFromDTO([NotNull] EventModel model)
        {
            return ViewModelConverter.CastToView(model);
        }

        private DateTime DateStartObject => DateTimeExtentions.GetDateFromTimeSpan(DateStart);
        private DateTime DateEndObject => DateTimeExtentions.GetDateFromTimeSpan(DateEnd);

        public string Title => Name;
        public string DateDisplay => DateStartObject.ToString("dd.MM.yy");
        public string TimePeriod =>  $"{DateStartObject.ToString("hh:mm")} - {DateEndObject.ToString("hh:mm")}";

        public List<StaffModel> Speakers => Staffs.Where(x => x.Type == StaffType.SPEAKER).ToList();
        public List<StaffModel> Moderators => Staffs.Where(x => x.Type == StaffType.MODERATOR).ToList();

        public int Voting
        {
            get { return _voting; }
            set
            {
                _voting = value;
                OnPropertyChanged(nameof(Voting));
            }
        }

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

        public void GetInfoFromDataModel(EventModel eventModel)
        {
            ViewModelConverter.CastToView(this, eventModel);
            OnPropertyChanged(nameof(Id));
        }
    }
}

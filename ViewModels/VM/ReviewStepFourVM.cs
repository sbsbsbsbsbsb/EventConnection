using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using JetBrains.Annotations;
using Microsoft.Practices.Unity;
using Model.DataModel;
using Model.DTO;
using Model.Runtime.Interfaces;
using Tools.ValidationEngine.Models;
using Tools.ValidationEngine.Models.ValidationRules;
using ViewModels.Utils;

namespace ViewModels.VM
{
    public sealed class ReviewStepFourVM : BaseVM, IViewModel<ReviewStepFour>
    {
        private int _selectedSectionId;
        private int _selectedSpeakerId;
        private List<EventModel> _sections = new List<EventModel>();
        private List<StaffModel> _speakers = new List<StaffModel>();
        
        [InjectionConstructor]
        public ReviewStepFourVM(IBusinessDataProvider service)
        {
            _service = service;
            Initialization = InitializeAsync();

            PropertyChanged += OnEventSelected;
        }

        public ReviewStepFourVM()
        {
        }

        public Task Initialization { get; protected set; }

        public Visibility LoadingVisible => Initialization.Status != TaskStatus.Running ? Visibility.Collapsed : Visibility.Visible;

        [ValidateObjectHasValue(LocalizationKey = "FieldRequired",
            ValidationMessageType = typeof(ValidationErrorMessage), FailureMessage = "")]
        public int SelectedSectionId
        {
            get { return _selectedSectionId; }
            set
            {
                _selectedSectionId = value;
                OnPropertyChanged(nameof(SelectedSectionId));
            }
        }

        [ValidateObjectHasValue(LocalizationKey = "FieldRequired",
            ValidationMessageType = typeof(ValidationErrorMessage), FailureMessage = "")]
        public int SelectedSpeakerId
        {
            get { return _selectedSpeakerId; }
            set
            {
                _selectedSpeakerId = value;
                OnPropertyChanged(nameof(SelectedSpeakerId));
            }
        }

        public List<EventModel> Sections
        {
            get { return _sections; }
            set
            {
                _sections = value;
                OnPropertyChanged(nameof(Sections));
            }
        }

        public List<StaffModel> Speakers
        {
            get { return _speakers; }
            set
            {
                _speakers = value;
                OnPropertyChanged(nameof(Speakers));
            }
        }

        public StaffModel SelectedSpeaker
        {
            get { return Speakers.FirstOrDefault(x => x.Id == SelectedSpeakerId); }
        }

        public EventModel SelectedSection
        {
            get { return Sections.FirstOrDefault(x => x.Id == SelectedSectionId); }
        }

        private readonly IBusinessDataProvider _service;
        

        private async void OnEventSelected(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SelectedSectionId)) return;
            SelectedSpeakerId = 0;
            if (SelectedSectionId < 0)
                Speakers.Clear();

            if(SelectedSectionId == 0) return;

            var @event = await _service.GetEvent(SelectedSectionId);
            if(@event != null)
                Speakers = @event.Staffs;
        }


        /// <summary>
        /// Init Sections
        /// </summary>
        /// <returns></returns>
        private async Task InitializeAsync()
        {
            Sections = await _service.GetEvents();
        }

        public ReviewStepFour GetDataModel()
        {
            return ViewModelConverter.CastToModel(this);
        }

        public void GetDataFromModel([NotNull] ReviewStepFour model)
        {
            ViewModelConverter.CastToView(this, model);
        }
    }
}

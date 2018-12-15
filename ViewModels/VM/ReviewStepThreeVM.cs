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
    public sealed class ReviewStepThreeVM : BaseVM, IViewModel<ReviewStepThree>
    {
        private int _selectedSectionId;
        private int _selectedModeratorId;
        private List<EventModel> _sections = new List<EventModel>();
        private List<StaffModel> _moderators = new List<StaffModel>();

        private readonly IBusinessDataProvider _serviceProvider;

        [InjectionConstructor]
        public ReviewStepThreeVM(IBusinessDataProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Initialization = InitializeAsync();

            PropertyChanged += OnEventSelected;
        }

        public ReviewStepThreeVM()
        {
        }

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
        public int SelectedModeratorId
        {
            get { return _selectedModeratorId; }
            set
            {
                _selectedModeratorId = value;
                OnPropertyChanged(nameof(SelectedModeratorId));
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

        public List<StaffModel> Moderators
        {
            get { return _moderators; }
            set
            {
                _moderators = value;
                OnPropertyChanged(nameof(Moderators));
            }
        }

        public StaffModel SelectedModerator
        {
            get { return Moderators.FirstOrDefault(x => x.Id == SelectedModeratorId); }
        }

        public EventModel SelectedSection
        {
            get { return Sections.FirstOrDefault(x => x.Id == SelectedSectionId); }
        }

        public Visibility LoadingVisible => Initialization.Status != TaskStatus.Running ? Visibility.Collapsed : Visibility.Visible;

        public Task Initialization { get; protected set; }

        private async void OnEventSelected(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SelectedSectionId)) return;
            SelectedModeratorId = 0;
            if (SelectedSectionId < 0)
                Moderators.Clear();

            if(SelectedSectionId == 0) return;

            var @event = await _serviceProvider.GetEvent(SelectedSectionId);
            Moderators = @event.Staffs;
        }


        public ReviewStepThree GetDataModel()
        {
            return ViewModelConverter.CastToModel(this);
        }

        public void GetDataFromModel([NotNull] ReviewStepThree model)
        {
            ViewModelConverter.CastToView(this, model);
        }


        /// <summary>
        /// Init Sections
        /// </summary>
        /// <returns></returns>
        private async Task InitializeAsync()
        {
            Sections = await _serviceProvider.GetEvents();
        }
    }
}

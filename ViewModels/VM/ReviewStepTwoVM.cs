using System.Collections.Generic;
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
    public sealed class ReviewStepTwoVM : BaseVM, IViewModel<ReviewStepTwo>
    {
        [InjectionConstructor]
        public ReviewStepTwoVM(IBusinessDataProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Initialization = InitializeAsync();
        }

        public ReviewStepTwoVM()
        {
        }

        public Task Initialization { get; protected set; }

        public Visibility LoadingVisible => Initialization.Status != TaskStatus.Running ? Visibility.Collapsed : Visibility.Visible;

        private int _selectedSectionId;
        private List<EventModel> _sections = new List<EventModel>();

        private readonly IBusinessDataProvider _serviceProvider;

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

        public List<EventModel> Sections
        {
            get { return _sections; }
            set
            {
                _sections = value;
                OnPropertyChanged(nameof(Sections));
            }
        }

        public EventModel SelectedSection
        {
            get { return Sections.FirstOrDefault(x => x.Id == SelectedSectionId); }
        }

        public ReviewStepTwo GetDataModel()
        {
            return ViewModelConverter.CastToModel(this);
        }

        public void GetDataFromModel([NotNull] ReviewStepTwo model)
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

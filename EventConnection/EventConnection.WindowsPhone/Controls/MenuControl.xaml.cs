using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.Mvvm;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace EventConnection.Controls
{
    public sealed partial class MenuControl : UserControl, IView
    {
        private readonly ViewModels.Context.MenuControl _vm;

        public MenuControl()
        {
            InitializeComponent();

            _vm = (ViewModels.Context.MenuControl) DataContext;
        }

        private void ToList_OnClick(object sender, RoutedEventArgs e)
        {
            _vm.GoToList();
        }

        private void ToChat_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: implement
        }

        private void ToReview_OnClick(object sender, RoutedEventArgs e)
        {
            _vm.GoToReview();
        }
        
        public bool ExpliciteEventsLock
        {
            get
            {
                var val = (bool) GetValue(ExpliciteEventsLockProperty);
                return val;
            }
            set
            {
                SetValue(ExpliciteEventsLockProperty, value);
                _vm.IsListEnabled = !value;
            }
        }
        
        public static readonly DependencyProperty ExpliciteEventsLockProperty =
            DependencyProperty.Register(nameof(ExpliciteEventsLock), typeof(object),
              typeof(MenuControl), new PropertyMetadata(null));
        
        public bool ExpliciteChatLock
        {
            get
            {
                var val = (bool)GetValue(ExpliciteChatLockProperty);
                //return val;

                return false; //todo: correct
            }
            set
            {
                SetValue(ExpliciteChatLockProperty, value);
                _vm.IsChatEnabled = !value;
            }
        }
        
        public static readonly DependencyProperty ExpliciteChatLockProperty =
            DependencyProperty.Register(nameof(ExpliciteChatLock), typeof(object),
              typeof(MenuControl), new PropertyMetadata(null));
        
        public bool ExpliciteReviewLock
        {
            get
            {
                var val = (bool)GetValue(ExpliciteReviewLockProperty);
                return val;
            }
            set
            {
                SetValue(ExpliciteReviewLockProperty, value);
                _vm.IsReviewEnabled = !value;
            }
        }
        
        public static readonly DependencyProperty ExpliciteReviewLockProperty =
            DependencyProperty.Register(nameof(ExpliciteReviewLock), typeof(object),
              typeof(MenuControl), new PropertyMetadata(null));
    }
}

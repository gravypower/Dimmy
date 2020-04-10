using System.Windows;
using System.Windows.Input;
using DIMS.Ui.UserControls;

namespace DIMS.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ICommand AddNewSitecoreEnvironment;

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}

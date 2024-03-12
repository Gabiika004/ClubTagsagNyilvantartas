using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MemberDataService _dataService;

        public MainWindow()
        {
            InitializeComponent();
            _dataService = new MemberDataService();
        }

        private async void LoadMembers_Click(object sender, RoutedEventArgs e)
        {
            var members = await _dataService.GetAllMembersAsync();
            MembersListView.ItemsSource = members;
        }

        private async void AddMember_Click(object sender, RoutedEventArgs e)
        {
            var detailsWindow = new MemberDetailsWindow();
            if (detailsWindow.ShowDialog() == true)
            {
                await _dataService.AddMemberAsync(detailsWindow.Member);
                RefreshMembers();
            }
        }

        private async void UpdateMember_Click(object sender, RoutedEventArgs e)
        {
            var selectedMember = MembersListView.SelectedItem as Member;
            if (selectedMember == null)
            {
                MessageBox.Show("Please select a member to update.", "Selection Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var detailsWindow = new MemberDetailsWindow(selectedMember);
            if (detailsWindow.ShowDialog() == true)
            {
                await _dataService.UpdateMemberAsync(selectedMember.Id, detailsWindow.Member);
                RefreshMembers();
            }
        }

        private async void DeleteMember_Click(object sender, RoutedEventArgs e)
        {
            var selectedMember = MembersListView.SelectedItem as Member;
            if (selectedMember == null)
            {
                MessageBox.Show("Please select a member to delete.", "Selection Missing", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this member?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _dataService.DeleteMemberAsync(selectedMember.Id);
                RefreshMembers();
            }
        }

        private void RefreshMembers_Click(object sender, RoutedEventArgs e)
        {
            RefreshMembers();
        }

        private async void RefreshMembers()
        {
            var members = await _dataService.GetAllMembersAsync();
            MembersListView.ItemsSource = members;
        }

    }

}
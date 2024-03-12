using ClubTagsagGUI.ViewModels;
using ClubTagsagGUI.Views;
using ClubTagsagNyilvantartas;
using System.Diagnostics;
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

namespace ClubTagsagGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MemberViewModel _memberViewModel;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MemberViewModel();
            _memberViewModel = new MemberViewModel();
        }

        private async void UpdateButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (DataContext is MemberViewModel viewModel)
            {
                try
                {
                    await viewModel.RefreshData();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Hiba történt az adatok frissítése közben: {ex.Message}");
                    MessageBox.Show("Hiba történt az adatok frissítése közben. Kérjük, próbálja meg később.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            // Feltételezve, hogy a DataGrid neve: MemberDataGrid
            if (MemberDataGrid.SelectedItem is Member selectedMember)
            {
                EditMember editMemberWindow = new EditMember(selectedMember);
                editMemberWindow.ShowDialog(); // Modal ablak megnyitása, ami vár a bezárásra
            }
            else
            {
                MessageBox.Show("Kérjük, válasszon ki egy tagot a módosításhoz.", "Nincs kiválasztva tag", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void NewButton_Click(object sender, RoutedEventArgs e)
        {
            // Létrehozzuk és megjelenítjük az új tag felvételéhez szükséges ablakot
            var newMemberWindow = new ClubTagsagGUI.Views.NewMember();
            var dialogResult = newMemberWindow.ShowDialog();

            // Itt kezelhetjük az új tag ablak lezárása utáni logikát, pl. adatok frissítése
            if (dialogResult == true)
            {
                await _memberViewModel.RefreshMembersAndFiltersAsync();
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMembers = MemberDataGrid.SelectedItems.Cast<Member>().ToList();
            if (selectedMembers.Any())
            {
                var deleteViewModel = new DeleteMemberViewModel
                {
                    SelectedMembers = selectedMembers
                };

                var success = await deleteViewModel.ConfirmAndDeleteAsync();
                if (success)
                {
                   await _memberViewModel.RefreshMembersAndFiltersAsync();
                }
            }
        }


    }
}
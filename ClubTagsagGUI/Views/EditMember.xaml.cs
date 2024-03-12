using ClubTagsagGUI.Services;
using ClubTagsagGUI.ViewModels;
using ClubTagsagNyilvantartas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClubTagsagGUI.Views
{
    /// <summary>
    /// Interaction logic for EditMember.xaml
    /// </summary>
    public partial class EditMember : Window
    {
        private EditMemberViewModel _viewModel;
        private MemberViewModel _memberViewModel;
        public Member CurrentMember { get; set; }
        public EditMember(Member member)
        {
            InitializeComponent();
            _viewModel = new EditMemberViewModel(member);
            _memberViewModel = new MemberViewModel();
            this.DataContext = _viewModel;
            CurrentMember = member;
        }


        private async void ChangeButton_Click(object sender, EventArgs e)
        {
            await _viewModel.UpdateMemberAsync();
            await _memberViewModel.RefreshMembersAndFiltersAsync();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var confirmDelete = MessageBox.Show($"Biztosan törölni szeretnéd a következő tagot: {CurrentMember.Fullname}?", "Megerősítés szükséges", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmDelete == MessageBoxResult.Yes)
            {
                var apiService = new ApiService();
                var success = await apiService.DeleteMemberAsync(CurrentMember.Id);

                if (success)
                {
                    MessageBox.Show("A tag sikeresen törölve lett.", "Sikeres Törlés", MessageBoxButton.OK, MessageBoxImage.Information);

                    await _memberViewModel.RefreshMembersAndFiltersAsync();

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nem sikerült törölni a tagot.", "Törlési hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}

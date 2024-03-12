using ClubTagsagGUI.ViewModels;
using System.Windows;

namespace ClubTagsagGUI.Views
{
    public partial class NewMember : Window
    {
        private NewMemberViewModel _viewModel;
        private MemberViewModel _memberViewModel;

        public NewMember()
        {
            InitializeComponent();
            _viewModel = new NewMemberViewModel();
            _memberViewModel = new MemberViewModel();
            this.DataContext = _viewModel;
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.FullName = FullNameBox.Text;
            _viewModel.Interest = InterestBox.Text;
            _viewModel.Rating = RatingBox.Text;

            var success = await _viewModel.AddMemberAsync();
            if (success)
            {
                this.DialogResult = true;
                await _memberViewModel.RefreshMembersAndFiltersAsync();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

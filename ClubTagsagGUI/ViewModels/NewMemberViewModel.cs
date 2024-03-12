using ClubTagsagNyilvantartas;
using ClubTagsagGUI.Services;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;

namespace ClubTagsagGUI.ViewModels
{
    public class NewMemberViewModel
    {
        public string FullName { get; set; }
        public string Interest { get; set; }
        public string Rating { get; set; }

        private readonly ApiService _apiService;
        private MemberViewModel _memberViewModel;

        public NewMemberViewModel()
        {
            _apiService = new ApiService();
            _memberViewModel = new MemberViewModel();
        }

        public async Task<bool> AddMemberAsync()
        {
            if (string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Interest) ||
                !long.TryParse(Rating, out long rating))
            {
                MessageBox.Show("Kérjük, töltse ki az összes mezőt helyesen!", "Validálási Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var newMember = new Member
            {
                Fullname = FullName,
                Interest = Interest,
                Rating = Convert.ToInt32(Rating),
                Entry = DateTime.UtcNow.ToString("MMM dd, yyyy h:mm tt", CultureInfo.InvariantCulture) // Például "Jun 22, 2023 10:49 PM"
            };

            try
            {
                var createdMember = await _apiService.CreateMemberAsync(newMember);
                if (createdMember != null)
                {
                    MessageBox.Show("A tag sikeresen hozzáadva.", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    await _memberViewModel.RefreshMembersAndFiltersAsync();

                    return true;
                }
                else
                {
                    MessageBox.Show("A tag hozzáadása nem sikerült.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a tag hozzáadásakor: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}

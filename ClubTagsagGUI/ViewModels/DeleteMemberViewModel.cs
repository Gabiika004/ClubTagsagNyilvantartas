using ClubTagsagNyilvantartas;
using ClubTagsagGUI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ClubTagsagGUI.ViewModels
{
    public class DeleteMemberViewModel
    {
        public IList<Member> SelectedMembers { get; set; }
        private readonly ApiService _apiService;

        public DeleteMemberViewModel()
        {
            _apiService = new ApiService();
        }

        public async Task<bool> ConfirmAndDeleteAsync()
        {
            if (SelectedMembers == null || !SelectedMembers.Any())
            {
                MessageBox.Show("Nincs kiválasztva tag a törléshez.", "Figyelmeztetés", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var confirmDelete = MessageBox.Show($"Biztosan törölni szeretnéd a kiválasztott {SelectedMembers.Count} tag(oka)t?", "Megerősítés szükséges", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmDelete == MessageBoxResult.Yes)
            {
                foreach (var member in SelectedMembers.ToList()) 
                {
                    var success = await _apiService.DeleteMemberAsync(member.Id);
                    if (!success)
                    {
                        MessageBox.Show($"Nem sikerült törölni a következő tagot: {member.Fullname}", "Törlési Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }

                MessageBox.Show("A kiválasztott tagok sikeresen törölve lettek.", "Sikeres Törlés", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }

            return false;
        }
    }
}

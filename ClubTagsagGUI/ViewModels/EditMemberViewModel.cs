using ClubTagsagGUI.Services;
using ClubTagsagNyilvantartas;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace ClubTagsagGUI.ViewModels
{
    public class EditMemberViewModel: INotifyPropertyChanged, IDataErrorInfo
    {
        private readonly ApiService _apiService = new ApiService();
        private MemberViewModel _memberViewModel;
        private Member _currentMember;
        public Member CurrentMember
        {
            get => _currentMember;
            set
            {
                _currentMember = value;
                OnPropertyChanged();
            }
        }

        public string Fullname
        {
            get => CurrentMember.Fullname;
            set
            {
                if (CurrentMember.Fullname != value)
                {
                    CurrentMember.Fullname = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Interest
        {
            get => CurrentMember.Interest;
            set
            {
                if (CurrentMember.Interest != value)
                {
                    CurrentMember.Interest = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Rating
        {
            get => CurrentMember.Rating.ToString();
            set
            {
                if (int.TryParse(value, out int newRating) && CurrentMember.Rating != newRating)
                {
                    CurrentMember.Rating = newRating;
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }

        public string Entry
        {
            get => CurrentMember.Entry;
            set
            {
                if (CurrentMember.Entry != value)
                {
                    CurrentMember.Entry = value;
                    OnPropertyChanged();
                }
            }
        }


        public EditMemberViewModel(Member member)
        {
            CurrentMember = member;
            _memberViewModel = new MemberViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public string Error => null;

        public string this[string propertyName]
        {
            get
            {
                string error = string.Empty;
                switch (propertyName)
                {
                    case nameof(Fullname):
                        if (string.IsNullOrWhiteSpace(Fullname))
                        {
                            error = "A név nem lehet üres.";
                        }
                        break;
                    case nameof(Interest):
                        if (string.IsNullOrWhiteSpace(Interest))
                        {
                            error = "Az érdeklődési kör nem lehet üres.";
                        }
                        break;
                    case nameof(Rating):
                        if (!int.TryParse(Rating, out int rating) || rating <= 0)
                        {
                            error = "Az értékelésnek nagyobbnak kell lennie, mint 0.";
                        }
                        break;
                }
                return error;
            }
        }

        public async Task UpdateMemberAsync()
        {
            if (string.IsNullOrWhiteSpace(CurrentMember.Fullname) ||
                string.IsNullOrWhiteSpace(CurrentMember.Interest) ||
                CurrentMember.Rating <= 0)
            {
                MessageBox.Show("Kérjük, töltse ki az összes mezőt helyesen!", "Validálási Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var updatedMember = await _apiService.UpdateMemberAsync(CurrentMember.Id, CurrentMember);
                if (updatedMember != null)
                {
                    MessageBox.Show("A tag sikeresen frissítve.", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    await _memberViewModel.RefreshMembersAndFiltersAsync();

                }
                else
                {
                    MessageBox.Show("Nem sikerült frissíteni a tag adatait.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a frissítés során: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

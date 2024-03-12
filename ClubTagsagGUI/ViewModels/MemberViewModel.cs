using ClubTagsagGUI.Services;
using ClubTagsagNyilvantartas;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ClubTagsagGUI.ViewModels
{
    public class MemberViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService = new ApiService();
        private ObservableCollection<Member> _members = new ObservableCollection<Member>();
        private List<Member> originalMembers = new List<Member>(); // Az eredeti tagok listája

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Member> Members
        {
            get => _members;
            set
            {
                _members = value;
                OnPropertyChanged(nameof(Members));
            }
        }

        public ObservableCollection<InterestFilter> AvailableInterests { get; } = new ObservableCollection<InterestFilter>();

        public MemberViewModel()
        {
            LoadMembersAsync().ContinueWith(task => InitializeFilters(), TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadMembersAsync()
        {
            var members = await _apiService.GetAllMembersAsync();
            if (members != null)
            {
                originalMembers = members.ToList(); // Az eredeti lista feltöltése
                Members.Clear();
                foreach (var member in members)
                {
                    Members.Add(member);
                }
            }
        }

        private void InitializeFilters()
        {
            AvailableInterests.Clear();
            var interests = originalMembers.Select(m => m.Interest).Distinct();
            foreach (var interest in interests)
            {
                var filter = new InterestFilter(interest);
                filter.PropertyChanged += Filter_PropertyChanged;
                AvailableInterests.Add(filter);
            }
        }

        private void Filter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(InterestFilter.IsSelected))
            {
                UpdateFilteredMembers();
            }
        }

        private void UpdateFilteredMembers()
        {
            var selectedInterests = AvailableInterests.Where(f => f.IsSelected).Select(f => f.InterestName).ToList();
            var filteredMembers = originalMembers.Where(m => selectedInterests.Contains(m.Interest)).ToList();

            Members.Clear();
            foreach (var member in filteredMembers)
            {
                Members.Add(member);
            }
        }

        public async Task RefreshData()
        {
            var members = await _apiService.GetAllMembersAsync();
            if (members != null)
            {
                Members.Clear();
                foreach (var member in members)
                {
                    Members.Add(member);
                }
 
                InitializeFilters();
            }
        }

        public async Task RefreshMembersAndFiltersAsync()
        {
            await LoadMembersAsync();
            InitializeFilters();
            OnPropertyChanged(nameof(AvailableInterests));
        }
    }

}

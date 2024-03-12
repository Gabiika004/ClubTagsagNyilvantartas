using System.ComponentModel;

namespace ClubTagsagGUI.ViewModels
{
    public class InterestFilter : INotifyPropertyChanged
    {
        private string _interest;
        private bool _isSelected;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string InterestName
        {
            get { return _interest; }
            set
            {
                if (_interest != value)
                {
                    _interest = value;
                    OnPropertyChanged(nameof(InterestName));
                }
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        // Konstruktor
        public InterestFilter(string interest)
        {
            _interest = interest;
            _isSelected = true;
        }
    }
}

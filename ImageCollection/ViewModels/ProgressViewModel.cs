using ImageCollection.Interfaces;
using Prism.Mvvm;

namespace ImageCollection.ViewModels
{
    public class ProgressViewModel : BindableBase, IWindowTitle, IWorkProgress
    {
        #region Fields
        private string _state;
        public double _value;
        public double _maximum;
        public double _minimum;
        public bool _isIndeterminate;
        #endregion

        #region Propirties
        public string Title => App.Name;

        public string State
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged();
            }
        }

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }

        public double Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                RaisePropertyChanged();
            }
        }

        public double Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;
                RaisePropertyChanged();
            }
        }

        public bool IsIndeterminate
        {
            get { return _isIndeterminate; }
            set
            {
                _isIndeterminate = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Events
        public delegate void DoWorkEventHandler(IWorkProgress progress);
        public event DoWorkEventHandler DoWork;
        #endregion

        public void Run() => DoWork?.Invoke(this);

        public ProgressViewModel() { _isIndeterminate = true; }

        public ProgressViewModel(double maximum, double minimum)
        {
            _maximum = maximum;
            _minimum = minimum;
            _value = 0;
            _isIndeterminate = false;
        }
    }
}
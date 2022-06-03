using ImageCollection.Interfaces;

namespace ImageCollection.ViewModels
{
    public class ProgressViewModel : IWindowTitle
    {
        #region Propirties
        public string Title { get { return App.Name; } }
        public string State { get; set; }
        public double Value { get; set; }
        public double Maximum { get; set; }
        public double Minimum { get; set; }
        public bool IsIndeterminate { get; set; }
        #endregion

        #region Events
        public delegate void DoWorkEventHandler(ProgressViewModel progress);
        public event DoWorkEventHandler DoWork;
        #endregion

        public void Run() => DoWork?.Invoke(this);
    }
}

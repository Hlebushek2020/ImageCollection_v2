namespace ImageCollection.Interfaces
{
    public interface IWorkProgress
    {
        string State { get; set; }
        double Value { get; set; }
        double Maximum { get; set; }
        double Minimum { get; set; }
        bool IsIndeterminate { get; set; }
    }
}

using CourseProject.GridGenerator.Area.Core;

namespace CourseProject.GridGenerator.Area.Splitting;

public interface IntervalSplitter
{
    public IEnumerable<double> EnumerateValues(Interval interval);
    public int Steps { get; }
}
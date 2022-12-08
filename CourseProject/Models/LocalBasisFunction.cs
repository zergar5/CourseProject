namespace CourseProject.Models;

public class LocalBasisFunction
{
    private readonly Func<double, double> _xFunction;
    private readonly Func<double, double> _yFunction;

    public LocalBasisFunction(Func<double, double> xFunction, Func<double, double> yFunction)
    {
        _xFunction=xFunction;
        _yFunction=yFunction;
    }

    public double CalcFunction(double x, double y)
    {
        return _xFunction(x) * _yFunction(y);
    }
}
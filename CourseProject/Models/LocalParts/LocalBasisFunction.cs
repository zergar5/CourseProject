namespace CourseProject.Models.LocalParts;

public class LocalBasisFunction
{
    private readonly Func<double, double> _rFunction;
    private readonly Func<double, double> _zFunction;

    public LocalBasisFunction(Func<double, double> rFunction, Func<double, double> zFunction)
    {
        _rFunction = rFunction;
        _zFunction = zFunction;
    }

    public double CalcFunction(double r, double z)
    {
        return _rFunction(r) * _zFunction(z);
    }
}
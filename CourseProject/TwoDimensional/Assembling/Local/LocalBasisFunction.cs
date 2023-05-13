using CourseProject.Core.GridComponents;

namespace CourseProject.TwoDimensional.Assembling.Local;

public class LocalBasisFunction
{
    private readonly Func<double, double> _xFunction;
    private readonly Func<double, double> _yFunction;

    public LocalBasisFunction(Func<double, double> xFunction, Func<double, double> yFunction)
    {
        _xFunction = xFunction;
        _yFunction = yFunction;
    }

    public double Calculate(Node2D node)
    {
        return _xFunction(node.X) * _yFunction(node.Y);
    }

    public double Calculate(double r, double z)
    {
        return _xFunction(r) * _yFunction(z);
    }
}
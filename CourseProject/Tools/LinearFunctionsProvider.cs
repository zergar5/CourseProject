using System.Runtime.CompilerServices;
using CourseProject.Models;

namespace CourseProject.Tools;

public class LinearFunctionsProvider
{
    public Func<double, double> CreateFirstFunction(double rightCoordinate, double h)
    {
        return coordinate => (rightCoordinate - coordinate) / h;
    }

    public Func<double, double> CreateSecondFunction(double leftCoordinate, double h)
    {
        return coordinate => (coordinate - leftCoordinate) / h;
    }
}
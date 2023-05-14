using CourseProject.Core.GridComponents;

namespace CourseProject.FEM.Parameters;

public interface IFunctionalParameter
{
    public double Calculate(int nodeIndex, double time);

    public double Calculate(Node2D node, double time);
}
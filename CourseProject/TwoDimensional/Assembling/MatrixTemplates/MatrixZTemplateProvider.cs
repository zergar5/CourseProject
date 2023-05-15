using CourseProject.Core.Base;
using CourseProject.FEM.Assembling;

namespace CourseProject.TwoDimensional.Assembling.MatrixTemplates;

public class MatrixZTemplateProvider : ITemplateMatrixProvider
{
    public BaseMatrix GetMatrix()
    {
        return new BaseMatrix(
            new [,]
            {
                { 1d, 1d },
                { 1d, 3d }
            }
        );
    }
}
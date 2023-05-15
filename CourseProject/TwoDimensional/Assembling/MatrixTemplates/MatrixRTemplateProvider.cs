using CourseProject.Core.Base;
using CourseProject.FEM.Assembling;

namespace CourseProject.TwoDimensional.Assembling.MatrixTemplates;

public class MatrixRTemplateProvider : ITemplateMatrixProvider
{
    public BaseMatrix GetMatrix()
    {
        return new BaseMatrix(
            new[,]
            {
                { 2d, 1d },
                { 1d, 2d }
            }
        );
    }
}
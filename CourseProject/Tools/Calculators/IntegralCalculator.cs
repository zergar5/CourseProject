using CourseProject.Models.LocalParts;

namespace CourseProject.Tools.Calculators;

public class IntegralCalculator
{
    private static readonly double[] InterpolationNodes = { -0.5773503, 0.5773503 };

    private static readonly double[] Weights = { 1.0, 1.0 };

    private const int GaussMethodNumber = 2;

    private const int NumberOfSegments = 512;

    public static double CalcDoubleIntegralForStiffnessMatrix(double rUpperLimit, double rDownLimit, double zUpperLimit, double zDownLimit, LocalBasisFunction PsiI, LocalBasisFunction PsiJ, double lambda)
    {
        var hr = (rUpperLimit - rDownLimit) / NumberOfSegments;
        var hz = (zUpperLimit - zDownLimit) / NumberOfSegments;

        var rGrid = new double[NumberOfSegments + 1];
        var zGrid = new double[NumberOfSegments + 1];

        for (var i = 0; i < NumberOfSegments + 1; i++)
        {
            rGrid[i] = rDownLimit + i * hr;
            zGrid[i] = zUpperLimit + i * hz;
        }

        var outerIntegralValue = 0.0;

        for (var i = 0; i < GaussMethodNumber; i++)
        {
            var sumOfOuterIntegral = 0.0;

            for (var r = 0; r < NumberOfSegments; r++)
            {
                //var rI = (rDownLimit + r * hr + rDownLimit + (r + 1) * hr) / 2.0 + InterpolationNodes[i] * hr / 2.0;
                var rI = (rGrid[r] + rGrid[r + 1]) / 2.0 + InterpolationNodes[i] * hr / 2.0;

                var innerIntegralValue = 0.0;

                for (var j = 0; j < GaussMethodNumber; j++)
                {
                    var sumOfInnerIntegral = 0.0;
                    for (var z = 0; z < NumberOfSegments; z++)
                    {
                        //var zJ = (zDownLimit + z * hz + zDownLimit + (z + 1) * hz) / 2.0 + InterpolationNodes[j] * hz / 2.0;
                        var zJ = (zGrid[z] + zGrid[z + 1]) / 2.0 + InterpolationNodes[j] * hz / 2.0;

                        sumOfInnerIntegral += hz * rI *
                                              (DerivativeCalculator.CalcDerivative(PsiI, rI, zJ, 'r') *
                                               DerivativeCalculator.CalcDerivative(PsiJ, rI, zJ, 'r') +
                                               DerivativeCalculator.CalcDerivative(PsiI, rI, zJ, 'z') *
                                               DerivativeCalculator.CalcDerivative(PsiJ, rI, zJ, 'z'));
                    }

                    innerIntegralValue += sumOfInnerIntegral * Weights[j] / 2.0;
                }

                sumOfOuterIntegral += hr * innerIntegralValue;
            }
            outerIntegralValue += Weights[i] / 2.0 * sumOfOuterIntegral;
        }

        return lambda * outerIntegralValue;
    }

    public static double CalcDoubleIntegralForMassMatrix(double rUpperLimit, double rDownLimit, double zUpperLimit, double zDownLimit, LocalBasisFunction PsiI, LocalBasisFunction PsiJ)
    {
        var hr = (rUpperLimit - rDownLimit) / NumberOfSegments;
        var hz = (zUpperLimit - zDownLimit) / NumberOfSegments;

        var rGrid = new double[NumberOfSegments + 1];
        var zGrid = new double[NumberOfSegments + 1];

        for (var i = 0; i < NumberOfSegments + 1; i++)
        {
            rGrid[i] = rDownLimit + i * hr;
            zGrid[i] = zUpperLimit + i * hz;
        }

        var outerIntegralValue = 0.0;

        for (var i = 0; i < GaussMethodNumber; i++)
        {
            var sumOfOuterIntegral = 0.0;

            for (var r = 0; r < NumberOfSegments; r++)
            {
                var rI = (rGrid[r] + rGrid[r + 1]) / 2.0 + InterpolationNodes[i] * hr / 2.0;

                var innerIntegralValue = 0.0;

                for (var j = 0; j < GaussMethodNumber; j++)
                {
                    var sumOfInnerIntegral = 0.0;
                    for (var z = 0; z < NumberOfSegments; z++)
                    {
                        var zJ = (zGrid[z] + zGrid[z + 1]) / 2.0 + InterpolationNodes[j] * hz / 2.0;
                        sumOfInnerIntegral += hz * rI * PsiI.CalcFunction(rI, zJ) * PsiJ.CalcFunction(rI, zJ);
                    }

                    innerIntegralValue += sumOfInnerIntegral * Weights[j] / 2.0;
                }

                sumOfOuterIntegral += hr * innerIntegralValue;
            }
            outerIntegralValue += Weights[i] / 2.0 * sumOfOuterIntegral;
        }

        return outerIntegralValue;
    }
}
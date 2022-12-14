﻿using CourseProject.Models.GlobalParts;

namespace CourseProject.Tools.Calculators;

public class ScalarProductCalculator
{
    public static double CalcScalarProduct(GlobalVector globalVector1, GlobalVector globalVector2)
    {
        var result = 0.0;
        for (var i = 0; i < globalVector1.Count; i++)
        {
            result += globalVector1[i] * globalVector2[i];
        }
        return result;
    }
}
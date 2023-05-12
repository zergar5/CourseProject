﻿namespace CourseProject.FEM.Assembling.Local;

public interface ILocalAssembler
{
    public LocalMatrix AssembleMatrix(Element element);
    public LocalVector AssembleRightSide(Element element);
}
﻿using CourseProject.Core;

namespace CourseProject.FEM.Assembling;

public interface IMatrixPortraitBuilder<TNode, out TMatrix>
{
    TMatrix Build(Grid<TNode> grid);
}
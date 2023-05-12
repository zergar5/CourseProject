namespace CourseProject.SLAE.Preconditions;

public interface IPreconditioner<out TResult>
{
    public TResult Decompose(SparseMatrix globalMatrix);
}
namespace CourseProject.Models.Grid;

public class Material
{
    public int Id { get; set; }
    public double[] Lambdas { get; set; }
    public double Gamma { get; set; }

    public Material(int id, double[] lambdas, double gamma)
    {
        Id = id;
        Lambdas = lambdas;
        Gamma = gamma;
    }
}
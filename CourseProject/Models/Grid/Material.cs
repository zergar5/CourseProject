namespace CourseProject.Models.Grid;

public class Material : IEquatable<Material>
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

    public bool Equals(Material? other)
    {
        return Id.Equals(other.Id) && Lambdas.SequenceEqual(other.Lambdas) && Gamma.Equals(other.Gamma);
    }
}
namespace CourseProject.Models.Grid;

public class Material
{
    public int Id { get; set; }
    public double[] Lamdas { get; set; }
    public double Gamma { get; set; }

    public Material(int id, double[] lamdas, double gamma)
    {
        Id = id;
        Lamdas = lamdas;
        Gamma = gamma;
    }
}
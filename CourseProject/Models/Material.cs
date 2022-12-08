namespace CourseProject.Models;

public class Material
{
    public int Id { get; set; }
    public double[] Lamdas { get; set; }
    public double Gamma { get; set; }

    public Material()
    {
        Id = 0;
    }
}
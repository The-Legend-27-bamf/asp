namespace Asp_projekt.Models;

public class HomeDashboardViewModel
{
    public Student? Student { get; set; }
    public Kolegij? Kolegij { get; set; }
    public Profesor? Profesor { get; set; }
    public Ocjena? Ocjena { get; set; }
    public Fakultet? Fakultet { get; set; }
    public int? FakultetId { get; set; }
    public Izvjestaj? Izvjestaj { get; set; }
}
namespace Asp_projekt.Models;

public class DateTimePickerModel
{
    public string InputId { get; set; } = string.Empty;
    public string InputName { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public DateTime? Value { get; set; }
    public string Placeholder { get; set; } = "Odaberi datum i vrijeme";
    public bool EnableTime { get; set; } = true;
}
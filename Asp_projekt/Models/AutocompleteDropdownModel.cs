namespace Asp_projekt.Models;

public class AutocompleteDropdownModel
{
    public string ComponentId { get; set; } = string.Empty;
    public string EndpointUrl { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Placeholder { get; set; } = "Pretrazi...";
    public int MinChars { get; set; } = 2;
    public string InputId { get; set; } = string.Empty;
    public string InitialText { get; set; } = string.Empty;
    public string HiddenInputId { get; set; } = string.Empty;
    public string HiddenInputName { get; set; } = string.Empty;
    public string HiddenInputValue { get; set; } = string.Empty;
}
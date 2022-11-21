namespace BlazorTestApp.Pages.HostManagement;

public class HostInput
{
    public bool HasAudio { get; set; } = true;
    public bool HasVideo { get; set; } = true;
    public string OutputMode { get; set; } = "composed";
}
namespace Traktor.Interfaces 
{
    public interface ITractorDiagnostics
    {
        Dictionary<string, string> GetDiagnosticInfo();
    }
}
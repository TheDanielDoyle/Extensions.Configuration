namespace Microsoft.Extensions.Configuration;

internal sealed record ConfigurationFiles
{
    public IEnumerable<string>? Json { get; set; }

    public bool HasJson()
    {
        return Json is not null;
    }
}

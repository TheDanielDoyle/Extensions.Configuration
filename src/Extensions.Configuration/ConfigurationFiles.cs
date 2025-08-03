namespace Microsoft.Extensions.Configuration;

public sealed record ConfigurationFiles
{
    public IEnumerable<string>? Json { get; set; }

    public bool HasJson()
    {
        return Json is not null;
    }
}

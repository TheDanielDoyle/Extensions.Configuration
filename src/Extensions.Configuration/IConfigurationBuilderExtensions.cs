namespace Microsoft.Extensions.Configuration;

public static class IConfigurationBuilderExtensions
{
    private const string _configurationFilesDefault = "ConfigurationFiles";

    /// <summary>
    /// <b>AddJsonfromFiles</b>
    /// <br/>A list of file names which can be read from your existing configuration in the form of:
    /// <code xml:space="preserve">
    /// {
    ///   "ConfigurationFiles": {
    ///     "Json": [ "file1.json", "file2.json", "file3.json" ]
    ///   }
    /// }
    /// </code>
    /// </summary>
    /// <param name="configurationBuilder">The IConfigurationBuilder to add to.</param>
    /// <param name="configurationSection">Override the default configuratio section (<b>ConfigurationFiles</b>)</param>
    /// <param name="optional">Whether the file is optional.</param>
    /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
    /// <returns>IConfigurationBuilder</returns>
    public static IConfigurationBuilder AddJsonFromFiles(
        this IConfigurationBuilder configurationBuilder, 
        string? configurationSection = default,
        bool optional = false,
        bool reloadOnChange = false)
    {
        ConfigurationFiles configurationFiles = BindConfigurationFiles(configurationBuilder, configurationSection);

        if (configurationFiles.HasJson())
        {
            foreach (string file in configurationFiles.Json!)
            {
                configurationBuilder.AddJsonFile(file, optional, reloadOnChange);
            }
        }
        return configurationBuilder;
    }

    private static ConfigurationFiles BindConfigurationFiles(IConfigurationBuilder configurationBuilder, string? configurationSection)
    {
        if (configurationBuilder is not IConfigurationRoot configurationRoot)
        {
            throw new InvalidOperationException("Unable to get IConfigurationRoot");
        }
        return Bind(configurationRoot, configurationSection);
    }

    private static ConfigurationFiles Bind(IConfigurationRoot configurationRoot, string? configurationSection)
    {
        ConfigurationFiles configurationFiles = new();
        configurationRoot.Bind(configurationSection ?? _configurationFilesDefault, configurationFiles);
        return configurationFiles;
    }
}
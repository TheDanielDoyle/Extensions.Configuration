using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.Configuration;

public static class IConfigurationBuilderExtensions
{
    private const string _configurationFilesDefault = "ConfigurationFiles";

    /// <summary>
    /// Adds one or more JSON configuration files to the configuration builder based on an existing configuration section.
    /// <br/>
    /// The method looks for a section (default: <c>ConfigurationFiles</c>) with the structure:
    /// <code>
    /// {
    ///   "ConfigurationFiles": {
    ///     "Json": [ "file1.json", "file2.json", "file3.json" ]
    ///   }
    /// }
    /// </code>
    /// If no files are specified and <paramref name="optional"/> is set to <c>false</c>, an <see cref="InvalidOperationException"/> is thrown.
    /// </summary>
    /// <param name="configurationBuilder">The configuration builder to modify.</param>
    /// <param name="configurationSection">An optional configuration section to override the default <c>ConfigurationFiles</c> section name.</param>
    /// <param name="optional">Specifies whether the configuration files are optional.</param>
    /// <param name="reloadOnChange">Whether to reload the configuration if the files change.</param>
    /// <returns>The modified <see cref="IConfigurationBuilder"/>.</returns>
    public static IConfigurationBuilder AddJsonFromFiles(
        this IConfigurationBuilder configurationBuilder, 
        string? configurationSection = null,
        bool optional = true,
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
        else if (!optional)
        {
            throw new InvalidOperationException("No JSON configuration files specified and 'optional' is false.");
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

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(ConfigurationFiles))]
    private static ConfigurationFiles Bind(IConfigurationRoot configurationRoot, string? configurationSection)
    {
        ConfigurationFiles configurationFiles = new();
        configurationRoot.Bind(configurationSection ?? _configurationFilesDefault, configurationFiles);
        return configurationFiles;
    }
}
using Vivacity.Server.Services;

namespace Vivacity.Server.Factories;

public class FileSystemFactory
{
    private readonly IServiceProvider _serviceProvider;
    
    public FileSystemFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public FileSystem CreateFileSystem(string name) => _serviceProvider.GetRequiredKeyedService<FileSystem>(name);
}
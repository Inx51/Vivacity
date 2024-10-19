namespace Vivacity.Server.Services;

public class FileSystem
{
    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }
}
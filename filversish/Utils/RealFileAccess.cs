namespace filversish.Utils;

public class RealFileAccess : IFileAccess
{
    public string ReadFile(string path)
    {
        return File.ReadAllText(path);
    }

    public void WriteFile(string path, string contents)
    {
        var file = new FileInfo(path);
        file.Directory?.Create();
        File.WriteAllText(file.FullName, contents);
    }

    public string[] GetFilesIn(string path)
    {
        return Directory.GetFiles(path);
    }

    public bool IsExist(string path)
    {
        return File.Exists(path) || Directory.Exists(path);
    }

    public void CopyDirectory(string fromPath, string toPath)
    {
        if (!Directory.Exists(toPath))
        {
            Directory.CreateDirectory(toPath);
        }

        var files = Directory.GetFiles(fromPath);
        foreach (var file in files)
        {
            var name = Path.GetFileName(file);
            var dest = Path.Combine(toPath, name);


            File.Copy(file, dest, true);
        }

        var folders = Directory.GetDirectories(fromPath);
        foreach (var folder in folders)
        {
            var name = Path.GetFileName(folder);
            var dest = Path.Combine(toPath, name);
            CopyDirectory(folder, dest);
        }
    }

    public void CopyFile(string fromPath, string toPath)
    {
        File.Copy(fromPath, toPath, true);
    }

    public void DeleteDirectory(string path)
    {
        if (Directory.Exists(path)) Directory.Delete(path, true);
    }

    public void DeleteFile(string path)
    {
        if (File.Exists(path)) File.Delete(path);
    }

    public DateTime GetLastModified(string path)
    {
        return File.GetLastWriteTime(path);
    }
}
namespace filversish.Utils;

public interface IFileAccess
{
    string ReadFile(string path);
    void WriteFile(string path, string contents);
    string[] GetFilesIn(string path);
    bool IsExist(string path);
    void CopyDirectory(string fromPath, string toPath);

    void CopyFile(string fromPath, string toPath);
}
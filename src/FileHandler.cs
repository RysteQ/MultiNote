using System.IO;

public class FileHandler
{
    public void saveFile(String path, String filename, String data)
    {
        StreamWriter streamWriter = new StreamWriter(path + filename);

        streamWriter.Write(data);
        streamWriter.Close();
    }

    public String? readFile(String path)
    {
        if (File.Exists(path))
        {
            StreamReader streamReader = new StreamReader(path);

            return streamReader.ReadToEnd();
        }

        return null;
    }
}
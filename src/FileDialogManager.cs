public class FileDialogManager
{
    public FileDialogManager(OpenFileDialog openFD, SaveFileDialog saveFD)
    {
        this.openFD = openFD;
        this.saveFD = saveFD;
    }
    
    public String? saveFileDialog(String[] fileTypeNames, String[] acceptableFileExtensions)
    {
        saveFD.Filter = convertFileExtensionsListToString(fileTypeNames, acceptableFileExtensions);
        saveFD.FileName = "";
    
        while (true)
        {
            if (saveFD.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(saveFD.FileName))
                    if (MessageBox.Show("This file already exists\nReplace it ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        File.Delete(saveFD.FileName);

                return saveFD.FileName;
            }
            else
            {
                if (MessageBox.Show("You haven't saved the file yet\n Are you sure you want to close this window ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    return null;
            }
        }
    }

    public String? openFileDialog(String[] fileTypeNames, String[] acceptableFileExtensions)
    {
        openFD.Filter = convertFileExtensionsListToString(fileTypeNames, acceptableFileExtensions);
        openFD.FileName = "";

        while (true)
        {
            if (openFD.ShowDialog() == DialogResult.OK)
            {
                return openFD.FileName;
            } else
            {
                if (MessageBox.Show("You haven't selected a file yet\nAre you sure you want to close this window ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    return null;
            }
        }
    }

    private String convertFileExtensionsListToString(String[] fileTypeNames, String[] acceptableFileExtensions)
    {
        String fileExtensions = "";

        for (int i = 0; i < acceptableFileExtensions.Length; i++)
        {
            fileExtensions += fileTypeNames[i] + "|" + acceptableFileExtensions[i] + "|";
        }

        return fileExtensions.TrimEnd('|');
    }

    OpenFileDialog openFD;
    SaveFileDialog saveFD;
}
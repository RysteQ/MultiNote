namespace MultiNote
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            initObjects();
        }

        private void initObjects()
        {
            FDManager = new FileDialogManager(openFD, saveFD);
            fileHandler = new FileHandler();
            highlighter = new Highlighter();
            config = new Config();
            stringManipulator = new StringManipulator();

            savedFile.Add(true);
            saveLocations.Add(null);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox textBox = (RichTextBox) textAreaTabControl.Controls[textAreaTabControl.SelectedIndex].Controls[0];
            String filePath = FDManager.openFileDialog(config.filenames, config.fileExtensions);

            if (filePath != null)
            {
                String fileData = fileHandler.readFile(filePath);

                textAreaTabControl.Controls[textAreaTabControl.SelectedIndex].Text = filePath.Split('\\').Last();
                extensionLabel.Text = "Extension: " + filePath.Split('\\').Last().Split('.').Last();
                textBox.Text = fileData;
            }
        }

        private void openNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(null, null);
            openToolStripMenuItem_Click(null, null);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = textAreaTabControl.SelectedIndex;

            if (savedFile[index] == false)
            {
                String locationToSaveAt = stringManipulator.getAllButLastWord(saveLocations[index], "\\");
                String filename = stringManipulator.getLastWord(saveLocations[index], "\\");
                String dataToSave = textAreaTabControl.Controls[index].Controls[0].Text;

                fileHandler.saveFile(locationToSaveAt, filename, dataToSave);

                textAreaTabControl.Controls[index].Text = filename;
                savedFile[index] = true;
            } else
            {
                saveAsToolStripMenuItem_Click(null, null);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            String fullSaveLocation = FDManager.saveFileDialog(config.filenames, config.fileExtensions);

            if (fullSaveLocation != null)
            {
                int index = textAreaTabControl.SelectedIndex;
                
                String dataToSave = textAreaTabControl.Controls[index].Controls[0].Text;
                String filename = fullSaveLocation.Split('\\').Last();

                textAreaTabControl.Controls[index].Text = filename;
                saveLocations[index] = fullSaveLocation;
                savedFile[index] = true;

                fileHandler.saveFile(fullSaveLocation.Replace(filename, ""), filename, dataToSave);
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        FileDialogManager FDManager;
        FileHandler fileHandler;
        Highlighter highlighter;
        Config config;
        StringManipulator stringManipulator;
        
        List<bool> savedFile = new List<bool>();
        List<String> saveLocations = new List<String>();

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TabPage tabPageToAdd = new TabPage("New File");
            RichTextBox richTextboxToAdd = new RichTextBox();

            richTextboxToAdd.Size = textArea.Size;
            richTextboxToAdd.Anchor = textArea.Anchor;
            richTextboxToAdd.Font = textArea.Font;
            richTextboxToAdd.TextChanged += new EventHandler(updateColumnLineLabel);

            tabPageToAdd.Controls.Add(richTextboxToAdd);
            textAreaTabControl.Controls.Add(tabPageToAdd);

            textAreaTabControl.SelectTab(textAreaTabControl.Controls.Count - 1);

            savedFile.Add(true);
        }

        private void increaseFontSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const int maximumFontSize = 96;

            for (int i = 0; i < textAreaTabControl.Controls.Count; i++)
            {
                RichTextBox textboxToUpdate = textAreaTabControl.Controls[i].Controls[0] as RichTextBox;
                
                if (textboxToUpdate.Font.Size != maximumFontSize)
                    textboxToUpdate.Font = new Font(textboxToUpdate.Font.Name, textboxToUpdate.Font.Size + 1);
            }

            updateColumnLineLabel(textAreaTabControl.SelectedTab.Controls[0], null);
        }

        private void decreaseFontSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const int minimumFontSize = 7;

            for (int i = 0; i < textAreaTabControl.Controls.Count; i++)
            {
                RichTextBox textboxToUpdate = textAreaTabControl.Controls[i].Controls[0] as RichTextBox;
                
                if (textboxToUpdate.Font.Size != minimumFontSize)
                    textboxToUpdate.Font = new Font(textboxToUpdate.Font.Name, textboxToUpdate.Font.Size - 1);
            }

            updateColumnLineLabel(textAreaTabControl.SelectedTab.Controls[0], null);
        }

        private void changeFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fontWindow = new FontDialog();
            
            if (fontWindow.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < textAreaTabControl.Controls.Count; i++)
                {
                    RichTextBox textboxToUpdate = textAreaTabControl.Controls[i].Controls[0] as RichTextBox;
                    textboxToUpdate.Font = fontWindow.Font;
                }
            }
        }

        private void updateColumnLineLabel(object sender, EventArgs e)
        {
            RichTextBox currentRichTextbox = sender as RichTextBox;
            String template = "Column: {0}{1}Line: {2}{3}{4}%{5}{6} Words";

            // We just get the line, column and font size percentage (10 being 100%)
            int index = currentRichTextbox.SelectionStart;
            int line = currentRichTextbox.GetLineFromCharIndex(index);
            int column = index - currentRichTextbox.GetFirstCharIndexFromLine(line);
            int fontSize = (int)(currentRichTextbox.Font.Size * 10);
            int words = stringManipulator.countWords(currentRichTextbox.Text);

            line++;
            column++;

            int[] padding =
            {
                config.maxPadding - column.ToString().Length,
                config.maxPadding - line.ToString().Length,
                config.maxPadding - fontSize.ToString().Length
            };

            String[] replaceWith =
            {
                column.ToString(),
                " ".PadRight(padding[0]),
                line.ToString(),
                " ".PadRight(padding[1]),
                fontSize.ToString(),
                " ".PadRight(padding[2]),
                words.ToString()
            };

            columnLineLabel.Text = stringManipulator.replaceStringPoints(template, replaceWith);
        }

        private void changeSavedFileStatus(object sender, KeyPressEventArgs e)
        {
            int index = textAreaTabControl.SelectedIndex;

            if (saveLocations[index] == null)
                return;

            if (savedFile[index])
            {
                textAreaTabControl.Controls[index].Text = "*" + textAreaTabControl.Controls[index].Text;
                savedFile[index] = false;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About_Class about = new About_Class();
            about.showInfo();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox currentRichTextBox = getCurrentRichTextBox(textAreaTabControl);
            currentRichTextBox.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RichTextBox currentRichTextBox = getCurrentRichTextBox(textAreaTabControl);
            currentRichTextBox.Redo();
        }

        // ------ MISC FUNCS

        private RichTextBox getCurrentRichTextBox(TabControl mainTextArea)
        {
            int index = textAreaTabControl.SelectedIndex;
            RichTextBox toReturn = textAreaTabControl.Controls[index].Controls[0] as RichTextBox;

            return toReturn;
        }
    }
}
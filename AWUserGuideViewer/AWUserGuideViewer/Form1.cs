using AWUserGuide.Common.Guides;
using AWUserGuideViewer.Guides;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AWUserGuideViewer
{
    public partial class Form1 : Form
    {
        DisplaySettings settings;
        public bool formInitialized = false;
        UserGuideToTreeViewAdapter _loadedGuide;

        public Form1()
        {
            InitializeComponent();

            settings = new DisplaySettings();
            ComboBox_NumberedLines.SelectedIndex = 0;
            formInitialized = true;
        }

        #region TreeView

        private void clearTreeView()
        {
            treeView1.Nodes.Clear();
        }

        private void buildTreeView()
        {
            clearTreeView();
            treeView1.ImageList = _loadedGuide.BuildImageList();
            treeView1.Nodes.AddRange(_loadedGuide.GuideNodes.ToArray());
        }

        #endregion

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            DialogResult result = fileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                backgroundWorker1.RunWorkerAsync(fileDialog.FileName);
            }
        }

        // Load the selected file.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string filePath = e.Argument as string;

            UserGuideToTreeViewAdapter currentGuide = new UserGuideToTreeViewAdapter(filePath, backgroundWorker1, settings);

            e.Result = currentGuide;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UserGuideToTreeViewAdapter guide = e.Result as UserGuideToTreeViewAdapter;
            _loadedGuide = guide;

            if (guide != null)
            {
                txtFile.Text = guide.FilePath;

                buildTreeView();
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar_Load.Value = e.ProgressPercentage;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
        }

        private void ComboBox_NumberedLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (formInitialized)
            {
                if (ComboBox_NumberedLines.SelectedIndex == 0)
                {
                    settings.LineNumbers = true;
                }
                else
                {
                    settings.LineNumbers = false;
                }

                if (txtFile.Text != null)
                {
                    DialogResult result = MessageBox.Show("You changed your settings.  Would you like to refresh the guide?",
                            "Important Query",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                    switch (result)
                    {
                        case DialogResult.Yes:
                            {
                                backgroundWorker1.RunWorkerAsync(txtFile.Text);
                                break;
                            }
                        case DialogResult.No:
                            {
                                break;
                            }
                    }
                }
            }
        }
    }
}
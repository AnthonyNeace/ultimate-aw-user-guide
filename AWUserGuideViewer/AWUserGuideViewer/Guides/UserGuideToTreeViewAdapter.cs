using AWUserGuideViewer.Common;
using AWUserGuideViewer.Common.Guides;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AWUserGuideViewer.Guides
{
    public class UserGuideToTreeViewAdapter
    {
        string _name;
        string _filePath;
        byte[] _contents;
        int _counter = 0;

        public string Name { get; set; }
        public string FilePath { get; set; }
        public string[] AllLines { get; set; }
        public List<TreeNode> GuideNodes { get; set; }
        public DisplaySettings Settings { get; set; }
        public BackgroundWorker Worker { get; set; }
        public UserGuide UserGuide { get; set; }

        public UserGuideToTreeViewAdapter(string filePath, BackgroundWorker backgroundWorker, DisplaySettings settings)
        {
            Name = Path.GetFileName(filePath);
            FilePath = filePath;
            GuideNodes = new List<TreeNode>();
            Settings = settings;
            Worker = backgroundWorker;

            AllLines = LoadFile(filePath);
            _counter = 0;

            UserGuide = new UserGuide(AllLines, settings);
            GuideNodes = BuildTree(UserGuide.Lines);
        }


        public string[] LoadFile(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        public List<TreeNode> BuildTree(List<UserGuideLines> guideLines)
        {
            TreeNode masterNode = new TreeNode();
            List<TreeNode> output = new List<TreeNode>();
            TreeNode parentNode = new TreeNode();
            TreeNode currentNode = new TreeNode();
            Stack<TreeNode> parentNodes = new Stack<TreeNode>();
            int progress = 0;

            // Depth processing - delimiters are "topic" and "endtopic".
            // If topic is processed, increment the depth on subsequent lines.
            // If endtopic is processes, decrement the depth on the current and subsequent lines.
            foreach (UserGuideLines line in guideLines)
            {
                currentNode = new TreeNode(line.AWString);

                currentNode.ImageIndex = 0;
                currentNode.SelectedImageIndex = 1;

                if (line.Link != null)
                {
                    currentNode.ImageIndex = 2;
                    currentNode.SelectedImageIndex = 3;
                }

                if (line.IsEnd.Value)
                {
                    parentNodes.Pop();
                }
                else
                {
                    if (parentNodes.Count > 0)
                    {
                        parentNode = parentNodes.Peek();
                        parentNode.Nodes.Add(currentNode);
                    }
                    else
                    {
                        masterNode.Nodes.Add(currentNode);
                        output.Add(currentNode);
                    }

                    if (line.IsBegin.Value)
                    {
                        parentNodes.Push(currentNode);
                    }
                }
            }
            return output;
        }

        public ImageList BuildImageList()
        {
            ImageList myImageList = new ImageList();

            myImageList.Images.Add(Properties.Resources.book);
            myImageList.Images.Add(Properties.Resources.book_open);
            myImageList.Images.Add(Properties.Resources.book_link);
            myImageList.Images.Add(Properties.Resources.link);
            myImageList.Images.Add(Properties.Resources.page_white_text);

            return myImageList;
        }

    }
}

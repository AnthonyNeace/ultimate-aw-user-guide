using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWUserGuideViewer.Guides
{
    public class UserGuideLines
    {
        string beginDelimiter = @"topic".ToLowerInvariant().Trim();
        string endDelimiter = @"endtopic".ToLowerInvariant().Trim();

        public string RawString { get; set; }
        public string FormattedString { get; set; }
        public string AWString { get; set; }
        public int LineNumber { get; set; }
        public bool? IsBegin { get; set; }
        public bool? IsEnd { get; set; }
        public List<UserGuideLines> Children { get; set; }
        public int Depth { get; set; }

        public UserGuideLines() {}

        public UserGuideLines(string line, int count)
        {
            RawString = line;
            FormattedString = line.Trim();
            AWString = FormattedString;
            LineNumber = count;

            if (FormattedString.ToLowerInvariant().StartsWith(beginDelimiter))
            {
                IsBegin = true;
                IsEnd = false;
            }
            else if (FormattedString.ToLowerInvariant().StartsWith(endDelimiter))
            {
                IsEnd = true;
                IsBegin = false;
            }
            else
            {
                IsBegin = false;
                IsEnd = false;
            } 
        }

        public UserGuideLines(string line, int count, DisplaySettings settings)
        {
            RawString = line;
            FormattedString = line.Trim();
            // Formatted String
            if (settings.LineNumbers)
            {
                AWString = String.Format(@"{0} - {1}", count.ToString("D4"), RawString.Trim());
            }
            else
            {
                AWString = FormattedString;
            }
            LineNumber = count;

            if (FormattedString.ToLowerInvariant().StartsWith(beginDelimiter))
            {
                IsBegin = true;
                IsEnd = false;
            }
            else if (FormattedString.ToLowerInvariant().StartsWith(endDelimiter))
            {
                IsEnd = true;
                IsBegin = false;
            }
            else
            {
                IsBegin = false;
                IsEnd = false;
            }
        }
    }
}

//            if (Settings.LineNumbers)
//            {
//                currentNode = new TreeNode(String.Format(@"{0} - {1}", _counter.ToString("D4"), line.RawString.Trim()));
//            }
//            else
//            {
//                currentNode = new TreeNode(line.RawString.Trim());
//            }
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public string Link { get; set; }

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
            string newString = parseRawString(RawString);
            if (settings.LineNumbers)
            {
                AWString = String.Format(@"{0} - {1}", count.ToString("D4"), newString);
            }
            else
            {
                AWString = newString;
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

        // Fetched from Stack Overflow @ http://stackoverflow.com/a/2201648/775544
        private string removeSubString(string sourceString, string removeString)
        {
            int index = sourceString.IndexOf(removeString);
            return (index < 0)
                ? sourceString
                : sourceString.Remove(index, removeString.Length);
        }

        private string parseRawString(string rawString)
        {
            string newString = removeSubString(rawString.Trim(), beginDelimiter);

            string regexQuotes = "\"[^\"]*\"\\s";

            Match finalString = Regex.Match(newString, regexQuotes);
            string[] keys = Regex.Split(newString, regexQuotes);

            if (keys.Length == 2)
            {
                //keys.GetValue(0);
                Link = keys[1];
                return finalString.ToString();
            }
            else
            {
                return newString;
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
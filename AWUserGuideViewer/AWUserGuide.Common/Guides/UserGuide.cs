using AWUserGuide.Common.Guides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWUserGuide.Common.Guides
{
    public class UserGuide
    {
        private List<UserGuideLines> _lines;
        private string _helpbase;
        private readonly string _helpDelimiter = @"help_base";

        #region public properties

        public List<UserGuideLines> Lines
        {
            get { return _lines; }
            protected set
            {
                _lines = value;
            }
        }

        public string HelpBase
        {
            get { return _helpbase; }
            protected set
            {
                _helpbase = value;
            }
        }

        #endregion

        #region constructors

        public UserGuide(string[] lines)
        {
            initialize(lines);
        }

        public UserGuide(string[] lines, DisplaySettings settings)
        {
            initialize(lines, settings);
        }

        #endregion

        private void initialize(string[] lines)
        {
            List<string> rawLines = lines.ToList<string>();
            HelpBase = SetHelpBase(rawLines.First());
            rawLines.RemoveAt(0);
            Lines = LoadLines(rawLines);
        }

        private void initialize(string[] lines, DisplaySettings settings)
        {
            List<string> rawLines = lines.ToList<string>();
            HelpBase = SetHelpBase(rawLines.First());
            rawLines.RemoveAt(0);
            Lines = LoadLines(rawLines, settings);
        }

        public string SetHelpBase(string line)
        {
            if (line.ToLowerInvariant().StartsWith(_helpDelimiter))
            {
                string[] tokens = line.Split(' ');
                return tokens[1];
            }
            else
            {
                return string.Empty;
            }
        }

        public List<UserGuideLines> LoadLines(List<string> lines)
        {
            List<UserGuideLines> guideLines = new List<UserGuideLines>();
            int counter = 0;
            int depth = 0;

            // Depth processing - delimiters are "topic" and "endtopic".
            // If topic is processed, increment the depth on subsequent lines.
            // If endtopic is processes, decrement the depth on the current and subsequent lines.
            foreach (string line in lines)
            {
                if (line.Length > 0)
                {
                    guideLines.Add(new UserGuideLines(line, counter));
                    if (guideLines[counter].IsEnd.Value)
                    {
                        depth--;
                    }

                    guideLines[counter].Depth = depth;
                    //Console.WriteLine("{0} - {1} - {2}",
                    //    guideLines[counter].LineNumber,
                    //    guideLines[counter].Depth,
                    //    guideLines[counter].FormattedString);

                    if (guideLines[counter].IsBegin.Value)
                    {
                        depth++;
                    }

                    counter++;
                }
            }

            return guideLines;
        }

        public List<UserGuideLines> LoadLines(List<string> lines, DisplaySettings settings)
        {
            List<UserGuideLines> guideLines = new List<UserGuideLines>();
            int counter = 0;
            int depth = 0;

            // Depth processing - delimiters are "topic" and "endtopic".
            // If topic is processed, increment the depth on subsequent lines.
            // If endtopic is processes, decrement the depth on the current and subsequent lines.
            foreach (string line in lines)
            {
                if (line.Length > 0)
                {
                    guideLines.Add(new UserGuideLines(line, counter, settings));
                    if (guideLines[counter].IsEnd.Value)
                    {
                        depth--;
                    }

                    guideLines[counter].Depth = depth;

                    if (guideLines[counter].IsBegin.Value)
                    {
                        depth++;
                    }

                    counter++;
                }
            }

            return guideLines;
        }
    }
}

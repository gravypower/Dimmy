using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SharpHostsFile
{
    internal class RegexHelper
    {
        public static string ReplaceNamedGroup(string input, string groupName, string replacement, Match match)
        {
            var capture = match.Value;
            capture = capture.Remove(match.Groups[groupName].Index - match.Index, match.Groups[groupName].Length);
            capture = capture.Insert(match.Groups[groupName].Index - match.Index, replacement);
            return capture;
        }

        public static string ReplaceNamedGroups(string input, Dictionary<string, string> groupReplacments, Regex pattern)
        {
            foreach (var replacement in groupReplacments)
            {
                var match = pattern.Match(input);
                input = ReplaceNamedGroup(input, replacement.Key, replacement.Value, match);
            }

            return input;
        }
    }
}
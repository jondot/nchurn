using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NChurn.Core.Support;

namespace NChurn.Core.Adapters.Svn
{
    internal class SvnAdapter : BaseAdapter
    {
        private readonly Regex _matcher = new Regex(@"\W*[A,M]\W+(\/.*)\b",RegexOptions.Compiled);
        public override IEnumerable<string> ChangedResources()
        {
            string text = CommandRunner.ExecuteAndGetOutput(@"svn log --verbose");
            return ParseLines(text);
        }

        public override IEnumerable<string> ChangedResources(DateTime backTo)
        {
            string text = CommandRunner.ExecuteAndGetOutput(string.Format(@"svn log {{{0}}}:{{{1}}} --verbose", backTo.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd")));
            return ParseLines(text);
        }

        private IEnumerable<string> ParseLines(string text)
        {
            string[] strings = text.SplitLines();
            return from s in strings
                   select _matcher.Match(s)
                       into match
                       where match.Success && match.Groups.Count == 2
                       select match.Groups[1].Value;
        }
    }
}
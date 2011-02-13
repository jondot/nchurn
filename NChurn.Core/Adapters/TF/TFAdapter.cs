using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NChurn.Core.Adapters.TF
{
    internal class TFAdapter : BaseAdapter
    {
        private readonly Regex _changedResourceMatcher = new Regex(@"\s+(\w+)\s\$(.*)\s*",RegexOptions.Compiled);

        public override IEnumerable<string> ChangedResources()
        {
            string text = CommandRunner.ExecuteAndGetOutput(@"tf history /format:detailed /noprompt /recursive .");

            return ParseLines(text);
        }

        public override IEnumerable<string> ChangedResources(DateTime backTo)
        {
            string text = CommandRunner.ExecuteAndGetOutput(string.Format(@"tf history /version:D""{0}""~D""{1}"" /format:detailed /noprompt /recursive .", backTo.ToString("dd-MM-yyyy"), DateTime.Now.ToString("dd-MM-yyyy")));
            return ParseLines(text);
        }

        private IEnumerable<string> ParseLines(string text)
        {
            if(string.IsNullOrEmpty(text))
            {
                return Enumerable.Empty<string>();
            }

            string[] strings = text.Split(new[] { Environment.NewLine },StringSplitOptions.RemoveEmptyEntries);
            
            return from s in strings select _changedResourceMatcher.Match(s) 
                   into match
                   where match.Success && match.Groups.Count == 3 
                   select match.Groups[2].Value;
        }

        
    }
}
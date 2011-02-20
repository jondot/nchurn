using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NChurn.Core.Adapters.TF
{
    public class TFAdapter : BaseAdapter
    {
        private readonly Regex _changedResourceMatcher = new Regex(@"\s+(\w+)\s\$(.*)\s*",RegexOptions.Compiled);

        public override IEnumerable<string> ChangedResources()
        {
            string text = DataSource.GetDataWithQuery(@"tf history /format:detailed /noprompt /recursive .");

            return Parse(text);
        }

        public override IEnumerable<string> ChangedResources(DateTime backTo)
        {
            string text = DataSource.GetDataWithQuery(string.Format(@"tf history /version:D""{0}""~D""{1}"" /format:detailed /noprompt /recursive .", backTo.ToString("dd-MM-yyyy"), DateTime.Now.ToString("dd-MM-yyyy")));
            return Parse(text);
        }

        public override IEnumerable<string> ParseImpl(string text)
        {
            string[] strings = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            return from s in strings
                   select _changedResourceMatcher.Match(s)
                       into match
                       where match.Success && match.Groups.Count == 3
                       select match.Groups[2].Value;
        }
    }
}
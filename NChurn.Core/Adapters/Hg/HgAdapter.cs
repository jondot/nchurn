using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NChurn.Core.Support;

namespace NChurn.Core.Adapters.Hg
{
    public class HgAdapter : BaseAdapter
    {
        public override IEnumerable<string> ChangedResources()
        {
            string text = DataSource.GetDataWithQuery(@"hg log -v");
            return Parse(text);
        }

        public override IEnumerable<string> ChangedResources(DateTime backTo)
        {
            string text = DataSource.GetDataWithQuery(string.Format(@"hg log -v -d ""> {0}""", backTo.ToString("yyyy-MM-dd")));
            return Parse(text);
        }

        public override IEnumerable<string> ParseImpl(string text)
        {
            string[] strings = text.SplitLines().Where(x => x.StartsWith("files:")).SelectMany(x => Regex.Split(x, @"\s+").Skip(1)).ToArray();

            return strings;
        }
    }
}
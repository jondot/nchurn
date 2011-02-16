using System;
using System.Collections.Generic;
using NChurn.Core.Support;

namespace NChurn.Core.Adapters.Git
{
    public class GitAdapter : BaseAdapter
    {
        public override IEnumerable<string> ChangedResources()
        {
            string text = CommandRunner.ExecuteAndGetOutput(@"git log  --name-only --pretty=format:");
            return Parse(text);
        }

        public override IEnumerable<string> ChangedResources(DateTime backTo)
        {
            string text = CommandRunner.ExecuteAndGetOutput(string.Format(@"git log  --after={0} --name-only --pretty=format:", backTo.ToString("yyyy-MM-dd")));
            return Parse(text);
        }

        public override IEnumerable<string> ParseImpl(string text)
        {
            string[] strings = text.SplitLines();

            return strings;
        }
    }
}
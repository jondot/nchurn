using System;
using System.Collections.Generic;
using System.Linq;
using NChurn.Core.Support;

namespace NChurn.Core.Adapters.Git
{
    internal class GitAdapter : BaseAdapter
    {
        public override IEnumerable<string> ChangedResources()
        {
            string text = CommandRunner.ExecuteAndGetOutput(@"git log  --name-only --pretty=format:");
            return ParseLines(text);
        }

        public override IEnumerable<string> ChangedResources(DateTime backTo)
        {
            string text = CommandRunner.ExecuteAndGetOutput(string.Format(@"git log  --after={0} --name-only --pretty=format:", backTo.ToString("yyyy-MM-dd")));
            return ParseLines(text);
        }

        private static IEnumerable<string> ParseLines(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Enumerable.Empty<string>();
            }

            string[] strings = text.SplitLines();

            return strings;
        }

    }
}
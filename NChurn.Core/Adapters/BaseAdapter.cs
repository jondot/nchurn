using System;
using System.Collections.Generic;
using System.Linq;
using NChurn.Core.Support;
using NChurn.Core.Support.Win32;

namespace NChurn.Core.Adapters
{
    internal abstract class BaseAdapter : IVersioningAdapter
    {
        protected BaseAdapter()
        {
            CommandRunner = new Win32CommandRunner(); //todo: decision point which runner to ins. based on platform.
        }

        public ICommandRunner CommandRunner { get; set; }
        public abstract IEnumerable<string> ChangedResources();
        public abstract IEnumerable<string> ChangedResources(DateTime backTo);
        public abstract IEnumerable<string> ParseImpl(string text);

        public IEnumerable<string> Parse(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Enumerable.Empty<string>();
            }
            return ParseImpl(text);
        }
        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using NChurn.Core.Support;
using NChurn.Core.Support.Win32;

namespace NChurn.Core.Adapters
{
    public abstract class BaseAdapter : IVersioningAdapter
    {
        protected BaseAdapter()
        {
            DataSource = new Win32CommandRunnerDataSource(); //todo: decision point which runner to ins. based on platform.
        }

        public IAdapterDataSource DataSource { get; set; }
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
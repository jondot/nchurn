using System;
using System.Collections.Generic;
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
    }
}
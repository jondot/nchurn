using System;
using System.Collections.Generic;
using NChurn.Core.Support;

namespace NChurn.Core.Adapters
{
    public interface IVersioningAdapter
    {
        IEnumerable<string> ChangedResources();
        IEnumerable<string> ChangedResources(DateTime backTo);
        ICommandRunner CommandRunner { get; set; }
    }
}
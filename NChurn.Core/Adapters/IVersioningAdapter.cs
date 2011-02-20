using System;
using System.Collections.Generic;
using NChurn.Core.Support;

namespace NChurn.Core.Adapters
{
    public interface IVersioningAdapter
    {
        IEnumerable<string> ChangedResources();
        IEnumerable<string> ChangedResources(DateTime backTo);
        IAdapterDataSource DataSource { get; set; }
        IEnumerable<string> Parse(string text);
    }
}
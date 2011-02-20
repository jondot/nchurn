using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NChurn.Core.Processors
{
    public interface IProcessor<T>
    {
        IEnumerable<T> Apply(IEnumerable<T> input);
    }
}

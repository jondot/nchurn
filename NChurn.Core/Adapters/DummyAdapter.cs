using System;
using System.Collections.Generic;
using System.Linq;
using NChurn.Core.Support;

namespace NChurn.Core.Adapters
{
    internal class DummyAdapter : IVersioningAdapter
    {
        private readonly string _text;
        public DummyAdapter(string text)
        {
            _text = text;
        }
        public IEnumerable<string> ChangedResources()
        {
            if(_text ==null)
                return Enumerable.Repeat("this is a string", 10000);
            return Parse(_text);
        }

        public IEnumerable<string> ChangedResources(DateTime backTo)
        {
            return ChangedResources();
        }

        public ICommandRunner CommandRunner
        {
            get { return null; }
            set {  }
        }

        public IEnumerable<string> Parse(string text)
        {
            return _text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using NChurn.Core.Adapters.Git;
using NChurn.Core.Adapters.Hg;
using NChurn.Core.Adapters.Svn;
using NChurn.Core.Adapters.TF;

namespace NChurn.Core.Adapters
{
    public class AutoDiscoveryAdapter : BaseAdapter
    {
        private readonly IVersioningAdapter _adapter;

        public AutoDiscoveryAdapter()
        {
            if(Directory.Exists(".git"))
            {
                _adapter = new GitAdapter();
            }
            else if (Directory.Exists(".hg"))
            {
                _adapter = new HgAdapter();
            }
            else if(Directory.Exists(".svn"))
            {
                _adapter = new SvnAdapter();
            }
            else
            {
                _adapter = new TFAdapter();
            }
        }

        public override IEnumerable<string> ChangedResources()
        {
            return _adapter.ChangedResources();
        }

        public override IEnumerable<string> ChangedResources(DateTime backTo)
        {
            return _adapter.ChangedResources(backTo);
        }

        public override IEnumerable<string> ParseImpl(string text)
        {
            return _adapter.Parse(text);
        }
    }
}
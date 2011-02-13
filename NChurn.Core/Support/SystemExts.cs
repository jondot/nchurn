using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NChurn.Core.Support
{
    static class SystemExts
    {
        public static string [] SplitLines(this string text)
        {
            return text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}

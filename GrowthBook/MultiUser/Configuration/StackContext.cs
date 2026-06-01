using System.Collections.Generic;

namespace GrowthBook.MultiUser
{
    internal sealed class StackContext
    {
        public HashSet<string> EvaluatedFeatures { get; set; } = new HashSet<string>();
    }
}

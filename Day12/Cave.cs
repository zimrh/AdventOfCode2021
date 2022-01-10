using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    internal class Cave
    {
        public const string StartCaveId = "start";
        public const string EndCaveId = "end";

        public string CaveId { get; set; }
        public bool IsBigCave => Equals(CaveId, CaveId.ToUpperInvariant());
        public List<string> ConnectedCaves = new List<string>();
    }
}

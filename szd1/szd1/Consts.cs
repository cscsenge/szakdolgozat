using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szd1 {
	public class Consts {
		public const int STICKY_CANVAS_WIDTH = 800;
		public const int FILL_GCOUNT = 500;
		public const int FILL_PCOUNT = 1000;
		public const int STICKY_RULEBTABLE_SIZE = 4;
		public const string BACKTRACK = "Backtrack";
		public const string GENETIC = "Genetic";
		public const string NEURAL = "Neural";
		public static readonly string[] STICKY_ALGORITHMS = { BACKTRACK, NEURAL };
		public static readonly string[] FILLOMINO_ALGORITHMS = { BACKTRACK, GENETIC };
	}
}

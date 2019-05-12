using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szd1 {
	public class Consts {
		public const int STICKY_CANVAS_WIDTH = 800;
		public const int FILL_GCOUNT = 3000;
		public const int FILL_PCOUNT = 1000;
		public const int STICKY_RULEBTABLE_SIZE = 4;
		public const string BACKTRACK = "Backtrack";
		public const string GENETIC = "Genetic";
		public const string NEURAL = "Neural";
		public const string QLEARNING = "Q-Learning";
		public static readonly string[] STICKY_ALGORITHMS = { BACKTRACK, NEURAL };
		public static readonly string[] FILLOMINO_ALGORITHMS = { BACKTRACK, GENETIC };
		public static readonly string[] SOKOBAN_ALGORITHMS = { QLEARNING };
		public const char SOKOBAN_PLAYER_GOAL = '+';
		public const char SOKOBAN_WALL = '#';
		public const char SOKOBAN_EMPTY = ' ';
		public const char SOKOBAN_EMPTY_GOAL = '.';
		public const char SOKOBAN_PLAYER_FLOOR = '@';
		public const char SOKOBAN_BOX_FLOOR = '$';
		public const char SOKOBAN_BOX_GOAL = '*';
        public const double RATE = 0.3;
        public const double GAMMA = 0.9;
        public const double REWARD = 1000;
        public const double DECAY = 0.999; // decay for randomization
        public const int K = 100; // parameter for epsilon greedy
        public const double PENALTY = 0;
        public const double PUSH_REWARD = 0;// encourage the player to push box
        public const double TIME_PENALTY = 0;// living penalty for training in order to optimize move steps
        public const int TRAINING_EPOCH = 10000;// num of epochs to be trained
        public const double TRIAL_TIMES_PARAM = 0; // parameter for exploration function
        public const int NUM_STEPS = 100; // num of steps to move;
    }
}

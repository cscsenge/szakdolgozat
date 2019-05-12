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

		//	# (hash) Wall
		//	_ (space) empty space
		//	. (period) Empty goal
		//	@ (at) Player on floor
		//	+ (plus) Player on goal 
		//  $ (dollar) Box on floor
		//	* (asterisk) Box on goal

		public const char SOKOBAN_PLAYER_GOAL = '+';
		public const char SOKOBAN_WALL = '#';
		public const char SOKOBAN_EMPTY = ' ';
		public const char SOKOBAN_EMPTY_GOAL = '.';
		public const char SOKOBAN_PLAYER_FLOOR = '@';
		public const char SOKOBAN_BOX_FLOOR = '$';
		public const char SOKOBAN_BOX_GOAL = '*';
        /**
		* This is the learning rate for Q learning
		*/
        public const double RATE = 0.3; // Learning rate

        /**
		 * This is the reward decay for Q learning
		 */
        public const double GAMMA = 0.9; // Eagerness - 0 looks in the near future, 1 looks in the distant future

        /**
		 * This is the reward when one of boxes reaches a goal
		 */
        public const double REWARD = 1000; // one of boxes reaches a goal

        /**
		 * This is the decay parameter for randomization
		 */
        public const double DECAY = 0.999; // decay for randomization

        /**
		 * This is the parameter for epsilon greedy
		 */
        public const int K = 100; // parameter for epsilon greedy

        /**
		 * This is the penalty when one of boxes reaches a deadlock
		 */
        public const double PENALTY = 0; // one of boxes reaches a deadlock

        /**
		 * This is the reward when the player pushes a box
		 */
        public const double PUSH_REWARD = 0;// encourage the player to push box

        /**
		 * This is the living penalty
		 */
        public const double TIME_PENALTY = 0;// living penalty for training in order to optimize move steps

        /**
		 * This is the number of training epochs
		 */
        public const int TRAINING_EPOCH = 10000;// num of epochs to be trained

        /**
		 * This is the parameter for exploration function
		 */
        public const double TRIAL_TIMES_PARAM = 0; // parameter for exploration function

        /**
		 * This is the number of steps to move for print strategy
		 */
        public const int NUM_STEPS = 100; // num of steps to move;
    }
}

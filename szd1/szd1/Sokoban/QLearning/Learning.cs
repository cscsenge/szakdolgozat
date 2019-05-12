using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szd1.Sokoban.QLearning {
	class Learning {

		/**
		 * This is the Q table
		 */
		private Dictionary<Node, Double> table; // Q table

		/**
		 * This is the variable to record number of success
		 */
		private int numOfSuccess = 0;

		/**
		 * This is the initial state for Q learning
		 */
		private State initState;

		/**
		 * This is the current state for Q learning
		 */
		private State state;

		/**
		 * This method is used to execute Q learning
		 * @param  filePath the file to be loaded
		 * @return strategy for Q learning
		 */
		public static string Execute(SokobanBusinessLogic bl) {
			Learning ql = new Learning();
			ql.Init(bl);
			ql.Learn();
			return ql.GetStrategyFromTable();
		}

		/**
		 * This method is to do initialization for Q learning
		 * @param  filePath the file to be loaded
		 */
		public void Init(SokobanBusinessLogic bl) {
			initState = bl.Init();
			state = bl.Init();

			// initialize Q table
			table = new Dictionary<Node, Double>();
		}

		/**
		 * This method is the main part of learning process
		 */
		private void Learn() {
            Debug.WriteLine("Start: " + DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture));
            Random rand = new Random();
            int count = 0;
			for (int i = 0; i < Consts.TRAINING_EPOCH; i++) { // Train cycles
								  // Select random initial state
				int counter = 0;
				while (!state.IsGoal() && counter <= 10000) {
                    
					////Console.WriteLine(counter + " steps of move");
					counter++;
					int[] actionsFromCurrentState = state.PossibleActionsFromState();

					// Pick a random action from the ones possible
					int index = rand.Next(actionsFromCurrentState.Length);
					int randomAction = actionsFromCurrentState[index];
					int suboptimalAction = GetPolicyFromState(state);
					int action = 0;
					if (EpsilonGreedy(i, Consts.DECAY, Consts.K)) {
						action = randomAction;
					} else {
						action = suboptimalAction;
					}
					if (!table.ContainsKey(new Node(state, action))) {
						table.Add(new Node(state, action), 0.0);
					}
					double q = table[new Node(state, action)];
					State nextState = state.ComputeState(state, action);
					nextState.UpdateExplore();// add exploration num - visit count n, at least one to avoid numeric problem
											   // double maxQ = maxQ(nextState);
					double maxQ = ModifiedMaxQ(nextState);

					if (nextState.CheckDeadlock()) {
						double r = Consts.PENALTY;
						double value = q + Consts.RATE * (r + Consts.GAMMA * maxQ - q);
						table.Remove(new Node(state, action));
						table.Add(new Node(state, action), value);

						state = new State(initState);
						count = 1;
						break;
					} else {
						double r = (nextState.NumOfGoals() - state.NumOfGoals()) * Consts.REWARD + Consts.TIME_PENALTY;
						if (nextState.StickToBox() && state.StickToBox()) {
							r += Consts.PUSH_REWARD;
						}
						double value = q + Consts.RATE * (r + Consts.GAMMA * maxQ - q);// (1-alpha)*Q(s,a) + alpha[sample]
						table.Remove(new Node(state, action));
						table.Add(new Node(state, action), value);
						if (nextState.IsGoal()) {
							numOfSuccess++;
							state = new State(initState);
							count = 1;
							break;
						}
						state = nextState;
					}
                    //string s = GetStrategyFromTable();
                    if (count == 1) Debug.WriteLine($"{count}:{action}");
					count++;
				}
			}
            Debug.WriteLine($"{count}");
            Debug.WriteLine("End: " + DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture));
        }

		/**
		 * This method is used to do calculate the maximum Q value got the given state
		 * @param  nextState the input state
		 * @return the Q value of the given state
		 */
		private double MaxQ(State nextState) {
			int[] actionsFromState = nextState.PossibleActionsFromState();
			double maxValue = -Double.MaxValue;
			foreach (var nextAction in actionsFromState) {
				if (!table.ContainsKey(new Node(nextState, nextAction))) {
					table.Add(new Node(nextState, nextAction), 0.0);
				}
				double value = table[new Node(nextState, nextAction)];
				if (value > maxValue)
					maxValue = value;
			}
			return maxValue;
		}

		/**
		 * This method is used to do calculate the maximum Q with exploration function implemented
		 * @param  nextState the input state
		 * @return the Q value of the given state
		 */
		private double ModifiedMaxQ(State nextState) {
			int[] actionsFromState = nextState.PossibleActionsFromState();
			double maxValue = -Double.MaxValue;
			int i = 0;
			foreach (var nextAction in actionsFromState) {
				if (!table.ContainsKey(new Node(nextState, nextAction))) {
					table.Add(new Node(nextState, nextAction), 0.0);
				}
				double value = table[new Node(nextState, nextAction)];
				if (value + Consts.TRIAL_TIMES_PARAM / nextState.NumExplore() > maxValue)
					maxValue = value + Consts.TRIAL_TIMES_PARAM / nextState.NumExplore();
			}
			return maxValue;
		}

		/**
		 * This method is used to extract policy from the given state
		 * @param  state the input state
		 * @return the policy of the given state
		 */
		private int GetPolicyFromState(State state) {
			int[] actionsFromState = state.PossibleActionsFromState();
			double maxValue = -Double.MaxValue;
			int action = 0;
			foreach (var nextAction in actionsFromState) {
				if (!table.ContainsKey(new Node(state, nextAction))) {
					table.Add(new Node(state, nextAction), 0.0);
				}
				double value = table[new Node(state, nextAction)];
				if (value > maxValue) {
					maxValue = value;
					action = nextAction;
				}
			}
			return action;
		}

		/**
		 * This method is used to extract policy from the Q table
		 * @return the policy extracted from Q table
		 */
		private string GetStrategyFromTable() {
			string result = "";
			State test = new State(initState);
			for (int i = 0; i < Consts.NUM_STEPS; i++) {
				int action = GetPolicyFromState(test);
				switch (action) {
					case 1:
						result += "u";
						break;
					case 2:
						result += "d";
						break;
					case 3:
						result += "l";
						break;
					case 4:
						result += "r";
						break;
					default:
						break;
				}
				test = test.ComputeState(test, action);
				if (test.IsGoal()) {
					break;
				}
			}
			return result;
		}

		/**
		 * This method is used to decide whether to adopt a random action or based on policy
		 * @param numOfEpoches the number of current epochs
		 * @param decay the decay parameter for randomization
		 * @param k the parameter for epsilon greedy
		 * @return the result is true if it decides to move randomly and the result is false if it decides to move based on policy
		 */
		private bool EpsilonGreedy(int numOfEpoches, double decay, int k) {
			Random rand = new Random();
			int n = rand.Next(100) + 1;
			if (n <= k * Math.Pow((double)decay, (double)numOfEpoches)) {
				return true; // randomize
			} else {
				return false; // policy
			}
		}
	}
}

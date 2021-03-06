﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szd1.Sokoban.QLearning {
	public class Node {
		/**
 * This is the state
 */
		private State state;

		/**
		 * This is the action
		 * 1 stands for up
		 * 2 stands for down
		 * 3 stands for left
		 * 4 stands for right
		 */
		private int action;


		/**
		 * This method is the constructor of a QNode object
		 * @param  state the State object to be set
		 * @param  action the action to be set
		 */
		public Node(State state, int action) {
			this.state = state;
			this.action = action;
		}

		/**
		 * This method is the overriding function of equals for QNode class
		 * @param o the object to be compared with
		 * @return the result is true if o is equal to this QNode object, otherwise the result if false
		 */
		public override bool Equals(object obj) {
			if (this == obj) {
				return true;
			}
			if ((obj.GetType() != typeof(Node))) {
				return false;
			}
			Node qnode = (Node)obj;
			if (state.Equals(qnode.state) && action == qnode.action) {
				return true;
			} else {
				return false;
			}
		}

		/**
		 * This method is the overriding function of hashCode for QNode class
		 * @return the result is hash code for this QNode object
		 */
		public override int GetHashCode() {
			string result = "";
			for (int i = 0; i < state.GetState().Length; i++) {
				for (int j = 0; j < state.GetState()[i].Length; j++) {
					result += state.GetState()[i][j];
				}
			}
			int hash = 1;
			hash = hash * 17 + action;
			hash = hash * 31 + result.GetHashCode();
			return hash;
		}
	}
}

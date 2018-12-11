using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szd1.StickyBlocks.Classes;
using Windows.Foundation;

namespace szd1.StickyBlocks.Algorithms.RuleBased {
	class RuleBased {

		private Unit[,] table;
		private static int[,] ruleTable;

		public RuleBased(Unit[,] table) {
			this.table = table;
			ruleTable = new int[Consts.STICKY_RULEBTABLE_SIZE, Consts.STICKY_RULEBTABLE_SIZE];
			Start();
		}

		public void Start() {
			for (int i = 0; i < ruleTable.GetLength(0); i++) {
				for (int j = 0; j < ruleTable.GetLength(1); j++) {
					ruleTable[i, j] = 0;
				}
			}
			Point startPos = GetStartPos();
			Point finishPos = GetFinishPos();
			Point currentPos = startPos;
			while (currentPos != finishPos) {

				Point newPos = currentPos;
				double directionIndex = -1;
				double dx = finishPos.X - newPos.X;
				double dy = finishPos.Y - newPos.Y;

				if (dx > dy && dx > 0) {
					directionIndex = 0; //right
				} else if (dx > dy && dx < 0) {
					directionIndex = 1; //left
				} else if (dx < dy && dx < 0) {
					directionIndex = 2; //up
				} else {
					directionIndex = 3; //down
				}

				double originalDistance = (Math.Pow(newPos.X - finishPos.X, 2) + Math.Pow(newPos.Y - finishPos.Y, 2));

				double movementIndex = GetMovementByDirection(directionIndex);
				if (movementIndex == 0) {
					newPos.X++;
				} else if (movementIndex == 1) {
					newPos.X--;
				} else if (movementIndex == 2) {
					newPos.Y--;
				} else {
					newPos.Y++;
				}

				double newDistance = (Math.Pow(newPos.X - finishPos.X, 2) + Math.Pow(newPos.Y - finishPos.Y, 2));

				if (CanMove(newPos)) {
					if (newDistance > originalDistance) {
						ruleTable[(int)directionIndex, (int)movementIndex]--;
					} else {
						ruleTable[(int)directionIndex, (int)movementIndex]++;
					}
					currentPos = newPos;
				} else {
					ruleTable[(int)directionIndex, (int)movementIndex]--; //maybe put it into an array?
				}				
			}
		}

		public Point GetStartPos() {
			foreach (var item in table) {
				if (item.Type == UnitType.Player) {
					return item.Point;
				}
			}
			return new Point();
		}

		public Point GetFinishPos() {
			foreach (var item in table) {
				if (item.Type == UnitType.Filled) {
					return item.Point;
				}
			}
			return new Point();
		}

		public double GetMovementByDirection(double directionIndex) {
			int[] subTable = new int[Consts.STICKY_RULEBTABLE_SIZE];
			for (int i = 0; i < ruleTable.GetLength(0); i++) {
				for (int j = 0; j < ruleTable.GetLength(1); j++) {
					if (i == directionIndex) {
						subTable[j] = ruleTable[i, j];
					}
				}
			}
			int maxIndex = 0;
			for (int i = 0; i < subTable.Length; i++) {
				if (subTable[i] > subTable[maxIndex]) {
					maxIndex = i;
				}
			}
			return maxIndex;
		}

		public bool CanMove(Point newPos) {
			if (table[(int)newPos.X, (int)newPos.Y].Type == UnitType.Empty) {
				return true;
			}
			return false;
		}
	}
}

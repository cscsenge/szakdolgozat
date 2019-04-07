using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szd1.Sokoban.QLearning;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace szd1.Sokoban {
	class SokobanBusinessLogic {

		private Point sokobanGamer;

		private ViewModel VM;

		//	# (hash) Wall
		//	_ (space) empty space
		//	. (period) Empty goal
		//	@ (at) Player on floor
		//	+ (plus) Player on goal 
		//  $ (dollar) Box on floor
		//	* (asterisk) Box on goal

		/**
		* a 2D array to store the levelmap
		*/
		private char[][] levelmap;

		/**
		 * The constructor of the levelloader
		 * @param levelsource The input file
		 */
		public SokobanBusinessLogic(ViewModel VM) {
			this.VM = VM;
		}

		public SokobanBusinessLogic() {

		}

		public void LoadMap(string filePath) {
			parseRowList(loadRowList(filePath));
			VM.SokobanArray = levelmap;
		}

		/**
		 * construct a new State depend on the level map
		 * @return State a new State
		 */
		public State init() {
			return new State(levelmap, getX(), getY());
		}

		/**
		 * load the txt file by row
		 * @param levelsource the source .txt file
		 * @return rowlist an List of input levelmap
		 */
		private List<string> loadRowList(string filePath) {
			List<string> rowlist = new List<string>();
			string[] s = File.ReadAllLines(filePath);
			int height = int.Parse(s[0]);
			for (int i = 1; i < height + 1; i++) {
				rowlist.Add(s[i]);
			}
			return rowlist;
		}

		/**
		 * change the signal of some specific char
		 * @param rowlist The List after loading
		 */
		private void parseRowList(List<string> rowlist) {
			int height = rowlist.Count;
			levelmap = new char[height][];
			for (int i = 0; i < height; i++) {
				levelmap[i] = rowlist[i].ToCharArray();
			}
		}

		/**
		 * get the player's location
		 * @return result an int array store the location of player
		 */
		private Point getPlayerLocation() {
			for (int i = 0; i < levelmap.Length; i++) {
				for (int j = 0; j < levelmap[i].Length; j++) {
					if (levelmap[i][j] == '@' || levelmap[i][j] == '+')
						return new Point(i, j);
				}
			}
			//never reached, ignoring playerless game entry for now.
			//Console.WriteLine("something went wrong...");
			return new Point(0, 0);
		}

		/**
		 * get the x-coordinate of player and print out
		 * @return getPlayerLocation the x-coordinate of the player
		 */
		private int getX() {
			//		Console.WriteLine("getting x: " + getPlayerLocation()[0]);
			return (int)getPlayerLocation().X;
		}

		/**
		 * get the y-coordinate of player and print out
		 * @return getPlayerLocation the y-coordinate of the player
		 */
		private int getY() {
			//		Console.WriteLine("getting y: " + getPlayerLocation()[1]);
			return (int)getPlayerLocation().Y;
		}

		public void PlayerMove(Windows.UI.Core.KeyEventArgs e) {
			boxes.Clear();
			switch (e.VirtualKey) {
				case Windows.System.VirtualKey.Right:
					if (IsUnitCanMove(getX(), getY(), 0, 1)) {
						PlayerMoveOne(0, 1);
					}
					break;
				case Windows.System.VirtualKey.Left:
					if (IsUnitCanMove(getX(), getY(), 0, -1)) {
						PlayerMoveOne(0, -1);
					}
					break;
				case Windows.System.VirtualKey.Up:
					if (IsUnitCanMove(getX(), getY(), -1, 0)) {
						PlayerMoveOne(-1, 0);
					}
					break;
				case Windows.System.VirtualKey.Down:
					if (IsUnitCanMove(getX(), getY(), 1, 0)) {
						PlayerMoveOne(1, 0);
					}
					break;
			}
		}

		List<Point> boxes = new List<Point>();

		public bool IsUnitCanMove(int x, int y, int dx, int dy) {
			if (levelmap[x + dx][y + dy] == Consts.SOKOBAN_EMPTY) {
				return true;
			} else if (levelmap[x + dx][y + dy] == Consts.SOKOBAN_BOX_FLOOR) {
				if (IsUnitCanMove(x + dx, y + dy, dx, dy)) {
					boxes.Add(new Point(x + dx, y + dy));
					return true;
				}
			} else if (levelmap[x + dx][y + dy] == Consts.SOKOBAN_EMPTY_GOAL) {
				if (levelmap[x][y] == Consts.SOKOBAN_BOX_FLOOR || levelmap[x][y] == Consts.SOKOBAN_PLAYER_FLOOR) {
					return true;
				}
			}
			return false;
		}

		public void PlayerMoveOne(int dx, int dy) {
			sokobanGamer = getPlayerLocation();
			foreach (var box in boxes) {
				levelmap[(int)box.X][(int)box.Y] = Consts.SOKOBAN_EMPTY;
				if (levelmap[(int)box.X + dx][(int)box.Y + dy] == Consts.SOKOBAN_EMPTY_GOAL) {
					levelmap[(int)box.X + dx][(int)box.Y + dy] = Consts.SOKOBAN_BOX_GOAL;
				} else {
					levelmap[(int)box.X + dx][(int)box.Y + dy] = Consts.SOKOBAN_BOX_FLOOR;
				}
			}
			if (levelmap[(int)sokobanGamer.X][(int)sokobanGamer.Y] == Consts.SOKOBAN_PLAYER_GOAL) {
				levelmap[(int)sokobanGamer.X][(int)sokobanGamer.Y] = Consts.SOKOBAN_EMPTY_GOAL;
			} else {
				levelmap[(int)sokobanGamer.X][(int)sokobanGamer.Y] = Consts.SOKOBAN_EMPTY;
			}
			if (levelmap[(int)sokobanGamer.X + dx][(int)sokobanGamer.Y + dy] == Consts.SOKOBAN_EMPTY_GOAL) {
				levelmap[(int)sokobanGamer.X + dx][(int)sokobanGamer.Y + dy] = Consts.SOKOBAN_PLAYER_GOAL;
			} else {
				levelmap[(int)sokobanGamer.X + dx][(int)sokobanGamer.Y + dy] = Consts.SOKOBAN_PLAYER_FLOOR;
			}
			VM.SokobanArray = levelmap;
		}

		public void SetComboBoxes(ComboBox levelChooser, ComboBox algorithmChooser) {
			levelChooser.Items.Clear();
			algorithmChooser.Items.Clear();
			DirectoryInfo dir = new DirectoryInfo(@"Levels\Sokoban\");
			foreach (FileInfo file in dir.GetFiles()) {
				if (file.Extension.Contains("txt")) {
					levelChooser.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
				}
			}
			foreach (string algorithm in Consts.SOKOBAN_ALGORITHMS) {
				algorithmChooser.Items.Add(algorithm);
			}
		}
	}
}

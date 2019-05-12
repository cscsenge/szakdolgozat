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
        private List<Point> boxes;
		private ViewModel VM;
		//	# (hash) Wall
		//	_ (space) empty space
		//	. (period) Empty goal
		//	@ (at) Player on floor
		//	+ (plus) Player on goal 
		//  $ (dollar) Box on floor
		//	* (asterisk) Box on goal
		private char[][] levelmap;
		private char[][] originalLevelmap;

		public SokobanBusinessLogic(ViewModel VM) {
			this.VM = VM;
            boxes = new List<Point>();
        }

		public SokobanBusinessLogic() {
            boxes = new List<Point>();
        }

		public void LoadMap(string filePath) {
			ParseRowList(LoadRowList(filePath));
			VM.SokobanArray = levelmap;
		}

		public void PlayerMove(Windows.UI.Core.KeyEventArgs e) {
			switch (e.VirtualKey) {
				case Windows.System.VirtualKey.Right:
					if (IsUnitCanMove(GetPlayerX(), GetPlayerY(), 0, 1)) {
						PlayerMoveOne(0, 1);
					}
					break;
				case Windows.System.VirtualKey.Left:
					if (IsUnitCanMove(GetPlayerX(), GetPlayerY(), 0, -1)) {
						PlayerMoveOne(0, -1);
					}
					break;
				case Windows.System.VirtualKey.Up:
					if (IsUnitCanMove(GetPlayerX(), GetPlayerY(), -1, 0)) {
						PlayerMoveOne(-1, 0);
					}
					break;
				case Windows.System.VirtualKey.Down:
					if (IsUnitCanMove(GetPlayerX(), GetPlayerY(), 1, 0)) {
						PlayerMoveOne(1, 0);
					}
					break;
			}
		}

		public bool IsUnitCanMove(int x, int y, int dx, int dy) {
			if (levelmap[x + dx][y + dy] == Consts.SOKOBAN_EMPTY) {
				return true;
			} else if (levelmap[x + dx][y + dy] == Consts.SOKOBAN_BOX_FLOOR || levelmap[x + dx][y + dy] == Consts.SOKOBAN_BOX_GOAL) {
				if (IsUnitCanMove(x + dx, y + dy, dx, dy)) {
					boxes.Add(new Point(x + dx, y + dy));
					return true;
				}
			} else if (levelmap[x + dx][y + dy] == Consts.SOKOBAN_EMPTY_GOAL) {
				if (levelmap[x][y] == Consts.SOKOBAN_BOX_FLOOR || levelmap[x][y] == Consts.SOKOBAN_PLAYER_FLOOR || levelmap[x][y] == Consts.SOKOBAN_BOX_GOAL) {
					return true;
				}
			}
			return false;
		}

		public void PlayerMoveOne(int dx, int dy) {
			sokobanGamer = GetPlayerLocation();
			foreach (var box in boxes) {
				if (originalLevelmap[(int)box.X][(int)box.Y] == Consts.SOKOBAN_BOX_GOAL) levelmap[(int)box.X][(int)box.Y] = Consts.SOKOBAN_EMPTY;
				if (levelmap[(int)box.X + dx][(int)box.Y + dy] == Consts.SOKOBAN_EMPTY_GOAL) {
					levelmap[(int)box.X + dx][(int)box.Y + dy] = Consts.SOKOBAN_BOX_GOAL;
				} else {
					levelmap[(int)box.X + dx][(int)box.Y + dy] = Consts.SOKOBAN_BOX_FLOOR;
				}
			}
			if (levelmap[(int)sokobanGamer.X][(int)sokobanGamer.Y] == Consts.SOKOBAN_PLAYER_GOAL || levelmap[(int)sokobanGamer.X][(int)sokobanGamer.Y] == Consts.SOKOBAN_BOX_GOAL || originalLevelmap[(int)sokobanGamer.X][(int)sokobanGamer.Y] == Consts.SOKOBAN_EMPTY_GOAL) {
				levelmap[(int)sokobanGamer.X][(int)sokobanGamer.Y] = Consts.SOKOBAN_EMPTY_GOAL;
			} else {
				levelmap[(int)sokobanGamer.X][(int)sokobanGamer.Y] = Consts.SOKOBAN_EMPTY;
			}
			if (levelmap[(int)sokobanGamer.X + dx][(int)sokobanGamer.Y + dy] == Consts.SOKOBAN_EMPTY_GOAL) {
				levelmap[(int)sokobanGamer.X + dx][(int)sokobanGamer.Y + dy] = Consts.SOKOBAN_PLAYER_GOAL;
			} else {
				levelmap[(int)sokobanGamer.X + dx][(int)sokobanGamer.Y + dy] = Consts.SOKOBAN_PLAYER_FLOOR;
			}
			boxes.Clear();
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

		public async void ExecuteQLearning() {
			string result = Learning.Execute(this);
			char[] steps = result.ToCharArray();
			foreach (char step in steps) {
				switch (step) {
					case 'u':
						if (IsUnitCanMove(GetPlayerX(), GetPlayerY(), -1, 0)) PlayerMoveOne(-1, 0);
						break;
					case 'd':
						if (IsUnitCanMove(GetPlayerX(), GetPlayerY(), 1, 0)) PlayerMoveOne(1, 0);
						break;
					case 'l':
						if (IsUnitCanMove(GetPlayerX(), GetPlayerY(), 0, -1)) PlayerMoveOne(0, -1);
						break;
					case 'r':
						if (IsUnitCanMove(GetPlayerX(), GetPlayerY(), 0, 1)) PlayerMoveOne(0, 1);
						break;
				}
				await Task.Delay(TimeSpan.FromSeconds(1));
			}
		}

        public State Init()
        {
            return new State(levelmap, GetPlayerX(), GetPlayerY());
        }

        private List<string> LoadRowList(string filePath)
        {
            List<string> rowlist = new List<string>();
            string[] s = File.ReadAllLines(filePath);
            int height = int.Parse(s[0]);
            for (int i = 1; i < height + 1; i++)
            {
                rowlist.Add(s[i]);
            }
            return rowlist;
        }

        private void ParseRowList(List<string> rowlist)
        {
            int height = rowlist.Count;
            levelmap = new char[height][];
			originalLevelmap = new char[height][];
			for (int i = 0; i < height; i++)
            {
                levelmap[i] = rowlist[i].ToCharArray();
				originalLevelmap[i] = rowlist[i].ToCharArray();
			}
        }

        private Point GetPlayerLocation()
        {
            for (int i = 0; i < levelmap.Length; i++)
            {
                for (int j = 0; j < levelmap[i].Length; j++)
                {
                    if (levelmap[i][j] == '@' || levelmap[i][j] == '+')
                        return new Point(i, j);
                }
            }
            return new Point(0, 0);
        }

        private int GetPlayerX()
        {
            return (int)GetPlayerLocation().X;
        }

        private int GetPlayerY()
        {
            return (int)GetPlayerLocation().Y;
        }
    }
}

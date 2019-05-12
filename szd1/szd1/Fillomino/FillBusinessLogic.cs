using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szd1.Fillomino.Classes;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.Foundation;
using szd1.Fillomino.Algorithms.Backtrack;
using szd1.Fillomino.Algorithms.Genetic;

namespace szd1.Fillomino {
	class FillBusinessLogic {

		public static Unit[,] FillArray;
		public static Unit[,] tempFillArray;
		public ViewModel VM;

		private List<Point> emptyFields;
		private bool alreadyChecked;
		private int fillSize;
		private Grid gameGrid;

		private static Backtrack fillBacktrack;

		public FillBusinessLogic(ViewModel VM, Grid gameGrid) {
			this.VM = VM;
			this.gameGrid = gameGrid;
			fillBacktrack = new Backtrack();
		}

		public void LoadFillomino(string fileName) {
			string[] rows = File.ReadAllLines(fileName);
			fillSize = int.Parse(rows[0]);
			FillArray = new Unit[fillSize, fillSize];
			tempFillArray = new Unit[fillSize, fillSize];
			for (int i = 0; i < fillSize; i++) {
				for (int j = 0; j < fillSize; j++) {
					int number = (int)Char.GetNumericValue(rows[1 + i][j]);
					FillArray[i, j] = new Unit(new Point(i, j), i * fillSize + j, number, number > 0 ? true: false);
					tempFillArray[i, j] = new Unit(new Point(i, j), i * fillSize + j, number, number > 0 ? true : false);
				}
			}
			alreadyChecked = false;
		}

		public void SetFillominoGrid(string fileName = null) {
			if (fileName != null) {
				 LoadFillomino(fileName);
			}
			gameGrid.RowDefinitions.Clear();
			gameGrid.ColumnDefinitions.Clear();
			gameGrid.Children.Clear();
			for (int i = 0; i < fillSize; i++) {
				ColumnDefinition cd = new ColumnDefinition();
				RowDefinition rd = new RowDefinition();
				gameGrid.ColumnDefinitions.Add(cd);
				gameGrid.RowDefinitions.Add(rd);
			}
			for (int i = 0; i < fillSize; i++) {
				for (int j = 0; j < fillSize; j++) {
					Border border = new Border() {
						BorderThickness = new Thickness(1, 1, 1, 1),
						BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0))
					};
					Button button = new Button() {
						BorderThickness = new Thickness(0, 0, 0, 0),
						Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
						Width = gameGrid.Width / fillSize,
						HorizontalAlignment = HorizontalAlignment.Stretch,
						VerticalAlignment = VerticalAlignment.Stretch,
						Name = $"{i}|{j}"
					};
					button.FontSize = button.Width / 2;
					if (FillArray[i, j].HasValue) {
						button.Content = FillArray[i, j].Number.ToString();
						button.IsEnabled = false;
						button.Background = new SolidColorBrush(Color.FromArgb(255, 192, 192, 192));
					}
					button.KeyDown += FillominoKeyDown;
					border.Child = button;
					gameGrid.Children.Add(border);
					Grid.SetColumn(border, j);
					Grid.SetRow(border, i);
				}
			}
		}

		public void SetComboBoxes(ComboBox levelChooser, ComboBox algorithmChooser) {
			levelChooser.Items.Clear();
			algorithmChooser.Items.Clear();
			DirectoryInfo dir = new DirectoryInfo(@"Levels\Fillomino\");
			foreach (FileInfo file in dir.GetFiles()) {
				if (file.Extension.Contains("txt")) {
					levelChooser.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
				}
			}
			foreach (string algorithm in Consts.FILLOMINO_ALGORITHMS) {
				algorithmChooser.Items.Add(algorithm);
			}
		}

		private void FillominoKeyDown(object sender, KeyRoutedEventArgs e) {
			if ((sender as Button).FocusState == FocusState.Pointer) {
				string keyString = e.Key.ToString();
				if (keyString.Contains("Number")) {
					double number = char.GetNumericValue(keyString.Where(x => Char.IsNumber(x)).First());
					(sender as Button).Content = number;
					string i = (sender as Button).Name.Split('|')[0];
					string j = (sender as Button).Name.Split('|')[1];
					tempFillArray[int.Parse(i), int.Parse(j)].Number = (int)number;
				}
			}
			alreadyChecked = false;
		}

		public void BacktrackAndFill(Grid gameGrid) {
			Backtrack(true);
			FillEmptyFields();
			SetFillominoGrid();
		}

		public void BacktrackAndHelp(Grid gameGrid) {
			if (!alreadyChecked) {
				Backtrack();
			}
			bool end = false;
			alreadyChecked = true;
			for (int i = 0; i < fillBacktrack.FinalArrays.Count; i++) {
				bool ok = false;
				for (int j = 0; j < fillBacktrack.FinalArrays[i].GetLength(0); j++) {
					for (int z = 0; z < fillBacktrack.FinalArrays[i].GetLength(1); z++) {
						if (tempFillArray[j, z].HasValue) {
							if (fillBacktrack.FinalArrays[i][j, z].Number == tempFillArray[j, z].Number) {
								ok = true;
							} else {
								break;
							}
						}
					}
					if (!ok) break;
				}
				if (ok) {
					for (int j = 0; j < tempFillArray.GetLength(0); j++) {
						for (int z = 0; z < tempFillArray.GetLength(0); z++) {
							if (!tempFillArray[j, z].HasValue && fillBacktrack.FinalArrays[i][j, z].HasValue) {
								FillArray[j, z].Number = fillBacktrack.FinalArrays[i][j, z].Number;
								tempFillArray[j, z].Number = fillBacktrack.FinalArrays[i][j, z].Number;
								end = true;
								break;
							}
						}
						if (end) break;
					}
				}
				if (end) break;
			}
			if (end) SetFillominoGrid(null);
		}

		public bool BacktrackAndCheck() {
			if (!alreadyChecked) {
				Backtrack();
			}
			alreadyChecked = true;
			for (int i = 0; i < fillBacktrack.FinalArrays.Count; i++) {
				bool ok = false;
				for (int j = 0; j < fillBacktrack.FinalArrays[i].GetLength(0); j++) {
					for (int z = 0; z < fillBacktrack.FinalArrays[i].GetLength(1); z++) {
						if (tempFillArray[j, z].HasValue) {
							if (fillBacktrack.FinalArrays[i][j, z].Number == tempFillArray[j, z].Number) {
								ok = true;
							} else {
								break;
							}
						}
					}
					if (!ok) break;
				}
				if (ok) {
					return true;
				}
			}
			return false;
		}

		private void Backtrack(bool fillArray = false) {
			Unit[,] endArray = fillBacktrack.Execute();
			if (fillArray) {
				FillArray = endArray;
			}
		}

		private void FillEmptyFields() {
			Point p;
			while (fillBacktrack.HasEmptyFields(FillArray).X != -1) {
				emptyFields = new List<Point>();
				p = fillBacktrack.HasEmptyFields(FillArray);
				emptyFields.Add(p);
				GetEmptyFields((int)p.X, (int)p.Y);
				int number = emptyFields.Count;
				foreach (var field in emptyFields) {
					FillArray[(int)field.X, (int)field.Y].Number = number;
				}
			}
		}

		private void GetEmptyFields(int x, int y) {
			if (x + 1 < FillArray.GetLength(0)) {
				if (!FillArray[x + 1, y].HasValue) {
					emptyFields.Add(FillArray[x + 1, y].Point);
				}
			}
			if (y + 1 < FillArray.GetLength(1)) {
				if (!FillArray[x, y + 1].HasValue) {
					emptyFields.Add(FillArray[x, y + 1].Point);
				}
			}
			if (x - 1 >= 0) {
				if (!FillArray[x - 1, y].HasValue) {
					emptyFields.Add(FillArray[x - 1, y].Point);
				}
			}
			if (y - 1 >= 0) {
				if (!FillArray[x, y - 1].HasValue) {
					emptyFields.Add(FillArray[x, y - 1].Point);
				}
			}
		}

		public void StartGenetic(Grid gameGrid) {
			List<Unit> emptyPlaces = GetEmptyPlaces();
			Genetic gen = new Genetic(units: emptyPlaces, size: fillSize);
            SetFillominoGrid();
		}

		public List<Unit> GetEmptyPlaces() {
			List<Unit> emptyPlaces = new List<Unit>();
			foreach (var item in FillArray) {
				if (!item.HasValue) {
					emptyPlaces.Add(item);
				}
			}
			return emptyPlaces;
		}
	}
}

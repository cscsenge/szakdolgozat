using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace szd1.Fillomino {
	class FillBusinessLogic {

		private int fillSize;
		private int[,] fillArray;
		public ViewModel VM;

		public FillBusinessLogic(ViewModel VM) {
			this.VM = VM;
		}

		public void LoadFillomino(string fileName) {
			string[] rows = File.ReadAllLines(fileName);
			fillSize = int.Parse(rows[0]);
			fillArray = new int[fillSize, fillSize];
			for (int i = 0; i < fillSize; i++) {
				for (int j = 0; j < fillSize; j++) {
					if (rows[1 + i][j] != 'x') {
						fillArray[i, j] = (int)Char.GetNumericValue(rows[1 + i][j]);
					}
				}
			}
		}

		public void SetFillominoGrid(Grid gameGrid, string fileName) {
			VM.FillBL.LoadFillomino(fileName);
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
						FontSize = 50
					};
					if (fillArray[i, j] > 0) {
						button.Content = fillArray[i, j].ToString();
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

		private void FillominoKeyDown(object sender, KeyRoutedEventArgs e) {
			if ((sender as Button).FocusState == FocusState.Pointer) {
				string keyString = e.Key.ToString();
				if (keyString.Contains("Number")) {
					(sender as Button).Content = keyString.Where(x => Char.IsNumber(x)).First();
				}
			}
		}
	}
}

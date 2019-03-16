﻿using System;
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

		private int fillSize;
		public static Unit[,] FillArray;
		public ViewModel VM;

		public FillBusinessLogic(ViewModel VM) {
			this.VM = VM;
		}

		public void LoadFillomino(string fileName) {
			string[] rows = File.ReadAllLines(fileName);
			fillSize = int.Parse(rows[0]);
			FillArray = new Unit[fillSize, fillSize];
			for (int i = 0; i < fillSize; i++) {
				for (int j = 0; j < fillSize; j++) {
					int number = (int)Char.GetNumericValue(rows[1 + i][j]);
					FillArray[i, j] = new Unit(new Point(i, j), i * fillSize + j ,number, number > 0 ? true: false);
				}
			}
		}

		public void SetFillominoGrid(Grid gameGrid, string fileName = null) {
			if (fileName != null) {
				VM.FillBL.LoadFillomino(fileName);
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
						VerticalAlignment = VerticalAlignment.Stretch
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
					levelChooser.Items.Add(file.Name);
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
					(sender as Button).Content = keyString.Where(x => Char.IsNumber(x)).First();
				}
			}
		}

		public void StartBacktrack(Grid gameGrid) {
			FillBacktrack fbt = new FillBacktrack();
			FillArray = fbt.ExecuteBacktrack(FillArray);
			FillEmptyFields();
			SetFillominoGrid(gameGrid);
		}

		private void FillEmptyFields() {
			foreach (var unit in FillArray) {
				if (!unit.HasValue) {
					unit.Number = 1; //TODO FindEmptyFields
				}
			}
		}

		public void StartGenetic(Grid gameGrid) {
			List<Unit> emptyPlaces = GetEmptyPlaces();
			FillGenetic gen = new FillGenetic(emptyPlaces, 9); //todo get max value of array
			//gen.Find(gameGrid);
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

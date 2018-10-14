using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace szd1
{
    public sealed partial class MainPage : Page
    {
		private ViewModel viewModel;
        private int fillSize;
		private int[,] fillArray;

        public MainPage()
        {
            this.InitializeComponent();
			viewModel = new ViewModel();
			this.DataContext = viewModel;
			Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
			gameGrid.Visibility = Visibility.Collapsed;
        }

		private void SetFillominoGrid()
        {
			string fileName = @"Levels\Fillomino\fillomino.txt"; //TODO
			LoadFillomino(fileName);
            for (int i = 0; i < fillSize; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                RowDefinition rd = new RowDefinition();
                gameGrid.ColumnDefinitions.Add(cd);
                gameGrid.RowDefinitions.Add(rd);
            }
            for (int i = 0; i < fillSize; i++)
            {
                for (int j = 0; j < fillSize; j++)
                {
                    Border border = new Border()
                    {
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
				string s = Windows.System.VirtualKey.Number1.ToString();
				switch (e.Key) {
					case Windows.System.VirtualKey.Number1: (sender as Button).Content = "1"; break;
					case Windows.System.VirtualKey.Number2: (sender as Button).Content = "2"; break;
					case Windows.System.VirtualKey.Number3: (sender as Button).Content = "3"; break;
					case Windows.System.VirtualKey.Number4: (sender as Button).Content = "4"; break;
					case Windows.System.VirtualKey.Number5: (sender as Button).Content = "5"; break;
					case Windows.System.VirtualKey.Number6: (sender as Button).Content = "6"; break;
					case Windows.System.VirtualKey.Number7: (sender as Button).Content = "7"; break;
					case Windows.System.VirtualKey.Number8: (sender as Button).Content = "8"; break;
					case Windows.System.VirtualKey.Number9: (sender as Button).Content = "9"; break;
					case Windows.System.VirtualKey.NumberPad1: (sender as Button).Content = "1"; break;
					case Windows.System.VirtualKey.NumberPad2: (sender as Button).Content = "2"; break;
					case Windows.System.VirtualKey.NumberPad3: (sender as Button).Content = "3"; break;
					case Windows.System.VirtualKey.NumberPad4: (sender as Button).Content = "4"; break;
					case Windows.System.VirtualKey.NumberPad5: (sender as Button).Content = "5"; break;
					case Windows.System.VirtualKey.NumberPad6: (sender as Button).Content = "6"; break;
					case Windows.System.VirtualKey.NumberPad7: (sender as Button).Content = "7"; break;
					case Windows.System.VirtualKey.NumberPad8: (sender as Button).Content = "8"; break;
					case Windows.System.VirtualKey.NumberPad9: (sender as Button).Content = "9"; break;
				}
			}
		}

		private void LoadFillomino(string fileName) {
			string[] rows = File.ReadAllLines(fileName);
			fillSize= int.Parse(rows[0]);
			fillArray = new int[fillSize, fillSize];
			for (int i = 0; i < fillSize; i++) {
				for (int j = 0; j < fillSize; j++) {
					if (rows[1 + i][j] != 'x') {
						fillArray[i, j] = (int)Char.GetNumericValue(rows[1 + i][j]);
					}
				}
			}
		}

		private void FillominoClick(object sender, RoutedEventArgs e) {
			viewModel.IsInFillomino = true;
			viewModel.IsInMenu = false;
			SetFillominoGrid();
		}

		private void StickyBlocksClick(object sender, RoutedEventArgs e) {
			viewModel.IsInMenu = false;
			viewModel.IsInSticky = true;
			string fileName = @"Levels\StickyBlocks\stickyblocks.txt"; //TODO
			viewModel.LoadStickyBlocks(fileName);
		}

		private void FillominoBackButtonClick(object sender, RoutedEventArgs e) {
			viewModel.IsInMenu = true;
			viewModel.IsInFillomino = false;
			gameGrid = new Grid(); //todo: its an ugly hack -> need to load everything first, only the visibilities change
		}

		private void StickyBackButtonClick(object sender, RoutedEventArgs e) {
			viewModel.IsInMenu = true;
			viewModel.IsInSticky = false;
		}

		private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args) {
			if (viewModel.IsInSticky) {
				viewModel.StickyUnitMove(args);
			}
		}

		private void StickyStartButtonClick(object sender, RoutedEventArgs e) {
			viewModel.BL.Start();
		}
	}
}

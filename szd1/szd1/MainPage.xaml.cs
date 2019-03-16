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
		private ViewModel VM;

        public MainPage()
        {
            this.InitializeComponent();
			VM = new ViewModel();
			this.DataContext = VM;
			Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
			gameGrid.Visibility = Visibility.Collapsed;
        }

		private void FillominoClick(object sender, RoutedEventArgs e) {
			VM.IsInFillomino = true;
			VM.IsInMenu = false;
			VM.FillBL.SetComboBoxes(levelChooser, algorithmChooser);
		}

		private void StickyBlocksClick(object sender, RoutedEventArgs e) {
			VM.IsInMenu = false;
			VM.IsInSticky = true;
			VM.StickyBL.SetComboBoxes(levelChooser, algorithmChooser);
		}

		private void BackButtonClick(object sender, RoutedEventArgs e) {
			VM.IsInMenu = true;
			VM.IsInFillomino = false;
			VM.IsInSticky = false;
			VM.IsItTheEnd = false;
			gameGrid = new Grid(); //todo: its an ugly hack -> need to load everything first, only the visibilities change
		}

		private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args) {
			if (VM.IsInSticky) {
				VM.StickyBL.PlayerMove(args);
			}
		}

		private void StartButtonClick(object sender, RoutedEventArgs e) {
			string value = algorithmChooser.SelectedValue.ToString();
			if (VM.IsInFillomino) {
				if (value == Consts.BACKTRACK) {
					VM.FillBL.StartBacktrack(gameGrid);
				} else if (value == Consts.GENETIC) {
					VM.FillBL.StartGenetic(gameGrid);
				}
			} else {
				if (value == Consts.NEURAL) {
					//todo
				}
			}
			
		}

		private void LevelChooserSelectedChanged(object sender, SelectionChangedEventArgs e) {
			if ((sender as ComboBox).SelectedValue != null) {
				if (VM.IsInFillomino) {
					string fileName = Path.Combine(@"Levels\Fillomino\", (string)(sender as ComboBox).SelectedValue); //TODO
					VM.FillBL.SetFillominoGrid(gameGrid, fileName);
				} else if (VM.IsInSticky) {
					string fileName = Path.Combine(@"Levels\StickyBlocks\", (string)(sender as ComboBox).SelectedValue); //TODO
					VM.StickyBL.LoadStickyBlocks(fileName);
				}
			}
		}

		private void LevelChooserDropDownClosed(object sender, object e) {
			startButton.Focus(FocusState.Pointer);
		}

		private void AlgorithmChooserSelectedChanged(object sender, SelectionChangedEventArgs e) {
		}

		private void ReloadButtonClick(object sender, RoutedEventArgs e) {
			if (VM.IsInFillomino) {
				VM.FillBL.SetFillominoGrid(gameGrid, Path.Combine(@"Levels\Fillomino\", levelChooser.SelectedValue.ToString()));
			} else {
				VM.StickyBL.LoadStickyBlocks(Path.Combine(@"Levels\StickyBlocks\", levelChooser.SelectedValue.ToString()));
			}
		}
	}
}

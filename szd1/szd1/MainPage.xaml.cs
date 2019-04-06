using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
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
		private SolidColorBrush defaultButtonColor;
		private ViewModel VM;

        public MainPage()
        {
            this.InitializeComponent();
			VM = new ViewModel(gameGrid);
			this.DataContext = VM;
			Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
			gameGrid.Visibility = Visibility.Collapsed;
			defaultButtonColor = (SolidColorBrush)checkButton.Background;
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

		private void SolveButtonClick(object sender, RoutedEventArgs e) {
			string value = algorithmChooser.SelectedValue.ToString();
			if (VM.IsInFillomino) {
				if (value == Consts.BACKTRACK) {
					VM.FillBL.BacktrackAndFill(gameGrid);
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
					string fileName = Path.Combine(@"Levels\Fillomino\", (string)(sender as ComboBox).SelectedValue + ".txt"); //TODO
					VM.FillBL.SetFillominoGrid(fileName);
				} else if (VM.IsInSticky) {
					string fileName = Path.Combine(@"Levels\StickyBlocks\", (string)(sender as ComboBox).SelectedValue + ".txt"); //TODO
					VM.StickyBL.LoadStickyBlocks(fileName);
				}
			}
		}

		private void LevelChooserDropDownClosed(object sender, object e) {
			solveButton.Focus(FocusState.Pointer);
		}

		private void AlgorithmChooserSelectedChanged(object sender, SelectionChangedEventArgs e) {
		}

		private void ReloadButtonClick(object sender, RoutedEventArgs e) {
			if (levelChooser.SelectedItem != null) {
				if (VM.IsInFillomino) {
					VM.FillBL.SetFillominoGrid(Path.Combine(@"Levels\Fillomino\", levelChooser.SelectedValue.ToString() + ".txt"));
				} else {
					VM.StickyBL.LoadStickyBlocks(Path.Combine(@"Levels\StickyBlocks\", levelChooser.SelectedValue.ToString() + ".txt"));
				}
			}
			checkButton.Background = defaultButtonColor;
		}

		private void HelpButtonClick(object sender, RoutedEventArgs e) {
			if (levelChooser.SelectedItem != null) {
				if (VM.IsInFillomino) {
					VM.FillBL.BacktrackAndHelp(gameGrid);
				} else {
					//todo
				}
			}
		}

		private void CheckButtonClick(object sender, RoutedEventArgs e) {
			if (levelChooser.SelectedItem != null) {
				if (VM.IsInFillomino) {
					bool ok = VM.FillBL.BacktrackAndCheck();
					if (ok) {
						(sender as Button).Background = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
					} else {
						(sender as Button).Background = defaultButtonColor;
					}
				} else {
					//todo
				}
			}
		}
	}
}

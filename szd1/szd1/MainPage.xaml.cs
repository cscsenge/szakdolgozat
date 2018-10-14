﻿using System;
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
			gameGrid = new Grid(); //todo: its an ugly hack -> need to load everything first, only the visibilities change
		}

		private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args) {
			if (VM.IsInSticky) {
				VM.StickyBL.PlayerMove(args);
			}
		}

		private void StickyStartButtonClick(object sender, RoutedEventArgs e) {
			//VM.StickyBL.Start();
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
			//todo
		}
	}
}

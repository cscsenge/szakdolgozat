using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using szd1.Fillomino;
using szd1.Fillomino.Classes;
using szd1.Sokoban;
using Windows.UI.Xaml.Controls;

namespace szd1 {
	class ViewModel: Bindable {
		private bool isInMenu = true;
		public bool IsInMenu { get { return isInMenu; } set { isInMenu = value; OPC(); } }
        private bool isInFillomino = false;
		public bool IsInFillomino { get { return isInFillomino; } set { isInFillomino = value; OPC(); } }
        private bool isInSokoban = false;
		public bool IsInSokoban { get { return isInSokoban; } set { isInSokoban = value; OPC(); } }
		private char[][] sokobanArray;
		public char[][] SokobanArray { get { return sokobanArray; } set { sokobanArray = value; OPC(); } }
		public FillBusinessLogic FillBL;
		public SokobanBusinessLogic SokobanBL;
        private bool isItTheEnd = false;
		public bool IsItTheEnd { get { return isItTheEnd; } set { isItTheEnd = value; OPC(); } }

		public ViewModel(Grid gameGrid) {
			FillBL = new FillBusinessLogic(this, gameGrid);
			SokobanBL = new SokobanBusinessLogic(this);
		}
	}

	abstract class Bindable : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;
		public void OPC([CallerMemberName] string n = "") {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
		}
	}
}

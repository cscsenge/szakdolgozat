using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szd1.Fillomino.Classes;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace szd1.Fillomino.Algorithms.Genetic {
	public class FillGenetic {

		Random r;
		Unit[,] fillArray;
		List<FillPopulation> generations;
		FillChromosome parent1;
		FillChromosome parent2;
		FillChromosome kid;

		public FillGenetic(Unit[,] fillArray) {
			this.fillArray = fillArray;
			r = new Random();
			generations = new List<FillPopulation>();
			FillPopulation population = new FillPopulation(Consts.FILL_PCOUNT, fillArray);
			for (int i = 0; i < Consts.FILL_GCOUNT; i++) {
				generations.Add(null);
			}
			generations[0] = population;
		}

		public void Find(Grid gameGrid) {
			for (int i = 0; i < Consts.FILL_GCOUNT - 1; i++) {
				generations[i + 1] = new FillPopulation();
				for (int j = 0; j < Consts.FILL_PCOUNT; j++) {

					parent1 = generations[i].fillArrayPopulation[r.Next(generations[i].fillArrayPopulation.Count)];
					parent2 = generations[i].fillArrayPopulation[r.Next(generations[i].fillArrayPopulation.Count)];

					kid = Crossover(parent1, parent2);
					if (kid.Goodness > parent1.Goodness && kid.Goodness > parent1.Goodness) {
						generations[i + 1].Add(kid);
						j++;
						fillArray = generations[i + 1].Fittest.FillArray;
						SetGrid(gameGrid);
					}
				}
			}
		}

		public void SetGrid(Grid gameGrid) {
			gameGrid.RowDefinitions.Clear();
			gameGrid.ColumnDefinitions.Clear();
			gameGrid.Children.Clear();
			for (int i = 0; i < fillArray.GetLength(1); i++) {
				ColumnDefinition cd = new ColumnDefinition();
				RowDefinition rd = new RowDefinition();
				gameGrid.ColumnDefinitions.Add(cd);
				gameGrid.RowDefinitions.Add(rd);
			}
			for (int i = 0; i < fillArray.GetLength(1); i++) {
				for (int j = 0; j < fillArray.GetLength(0); j++) {
					Border border = new Border() {
						BorderThickness = new Thickness(1, 1, 1, 1),
						BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0))
					};
					Button button = new Button() {
						BorderThickness = new Thickness(0, 0, 0, 0),
						Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
						Width = gameGrid.Width / fillArray.GetLength(1),
						HorizontalAlignment = HorizontalAlignment.Stretch,
						VerticalAlignment = VerticalAlignment.Stretch
					};
					button.FontSize = button.Width / 2;
					if (fillArray[i, j].DoesHaveNumber) {
						button.Content = fillArray[i, j].Number.ToString();
						button.IsEnabled = false;
						button.Background = new SolidColorBrush(Color.FromArgb(255, 192, 192, 192));
					}
					//button.KeyDown += FillominoKeyDown;
					border.Child = button;
					gameGrid.Children.Add(border);
					Grid.SetColumn(border, j);
					Grid.SetRow(border, i);
				}
			}
		}

		public FillChromosome Crossover(FillChromosome parent1, FillChromosome parent2) {
			Unit[,] newArray = fillArray;
			for (int i = 0; i < newArray.GetLength(1); i++) {
				for (int j = 0; j < newArray.GetLength(0); j++) {
					if (!newArray[i, j].DefaultNumber) {
						int parent = r.Next(0, 2);
						if (parent == 0) {
							newArray[i, j] = parent1.FillArray[i, j];
						} else {
							newArray[i, j] = parent2.FillArray[i, j];
						}
					}
				}
			}
			Mutation(newArray);
			return Mutation(newArray);
		}

		public FillChromosome Mutation(Unit[,] newArray) {
			for (int i = 0; i < newArray.GetLength(1); i++) {
				for (int j = 0; j < newArray.GetLength(0); j++) {
					if (!newArray[i, j].DefaultNumber) {
						int number = r.Next(0, 4);
						if (number == 0) {
							newArray[i, j].Number = r.Next(2, 10);
						}
					}
				}
			}
			FillChromosome child = new FillChromosome(newArray);
			return child;
		}

		public void SetFittest() {

		}
	}
}

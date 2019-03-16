using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szd1.Fillomino.Classes;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace szd1.Fillomino.Algorithms.Genetic {
	public class FillGenetic {

		//random stuff
		private static readonly Random random = new Random();
		private static readonly object syncLock = new object();
		public static int RandomNumber(int min, int max) {
			lock (syncLock) { // synchronize
				return random.Next(min, max);
			}
		}

		//fields
		private FillPopulation population;
		private FillPopulation halfPopulation;
		private List<Unit> units;
		private int maxValue;

		//methods
		public FillGenetic(List<Unit> units, int maxValue) {
			bool tesztike = false;
			this.units = new List<Unit>(units);
			this.maxValue = maxValue;
			SetPopulation();
			population.OrderPopulationByFitness();
			while (!population.Chromosomes.Exists(x => x.Fitness == 0)) {
				if (population.Chromosomes.First().Fitness < 10) {
					tesztike = true;
				}
				Crossover();
				foreach (var item in population.Chromosomes) {
					item.SetFitness();
				}
				population.OrderPopulationByFitness();
				if (!population.Chromosomes.Any(o => o != population.Chromosomes[0])) {
					tesztike = true;
				}
			}
		}

		private void SetPopulation() {
			population = new FillPopulation();
			for (int i = 0; i < Consts.FILL_PCOUNT; i++) {
				List<FillGene> genes = new List<FillGene>();
				foreach (var item in units) {
					genes.Add(new FillGene(item.GetX(), item.GetY(), maxValue));
				}
				population.Chromosomes.Add(new FillChromosome(genes));
			}
		}

		private void Crossover() {
			halfPopulation = new FillPopulation() {
				Chromosomes = new List<FillChromosome>()
			};
			for (int i = 0; i < Consts.FILL_PCOUNT / 2; i++) {
				halfPopulation.Chromosomes.Add(population.Chromosomes[i]);
			}
			population.Chromosomes.Clear();
			for (int i = 0; i < Consts.FILL_PCOUNT / 2; i++) {
				int randomValue = RandomNumber(1, (Consts.FILL_PCOUNT / 2) - 1);
				FillChromosome chromosome1 = halfPopulation.Chromosomes.First();
				int randomValue2 = RandomNumber(1, (Consts.FILL_PCOUNT / 2) - 1);
				FillChromosome chromosome2 = halfPopulation.Chromosomes[randomValue2];
				FillChromosome newChromosome1 = new FillChromosome(chromosome1.Genes.ToList());
				FillChromosome newChromosome2 = new FillChromosome(chromosome2.Genes.ToList());
				for (int j = 0; j < chromosome1.Genes.Count; j++) {
					if (chromosome2.Genes[j].Number != chromosome1.Genes[j].Number) {
						int randomValue3 = RandomNumber(0, 10);
						if (randomValue3 == 1) {
							newChromosome1.Genes[j].SetNumber(chromosome2.Genes[j].Number);
						}
					}
				}
				for (int j = 0; j < chromosome2.Genes.Count; j++) {
					if (chromosome2.Genes[j].Number != chromosome1.Genes[j].Number) {
						int randomValue3 = RandomNumber(0, 10);
						if (randomValue3 == 1) {
							newChromosome2.Genes[j].SetNumber(chromosome1.Genes[j].Number);
						}
					}
				}
				Mutation(newChromosome1);
				Mutation(newChromosome2);
			}
		}

		private void Mutation(FillChromosome chromosome) {
			for (int i = 0; i < 3; i++) {
				int newNumber;
				int geneIndex = RandomNumber(0, units.Count);
				do {
					newNumber = RandomNumber(1, maxValue);
				} while (newNumber == chromosome.Genes[geneIndex].Number);
				chromosome.Genes[geneIndex].SetNumber(newNumber);
			}
			population.Chromosomes.Add(chromosome);
		}

		//population class
		public class FillPopulation {

			public List<FillChromosome> Chromosomes { get; set; }

			public FillPopulation() {
				Chromosomes = new List<FillChromosome>();
			}

			public void OrderPopulationByFitness() {
				Chromosomes = Chromosomes.OrderBy(x => x.Fitness).ToList();
			}
		}

		//chromosome class
		public class FillChromosome {

			public List<FillGene> Genes { get; set; }
			public int Fitness { get; private set; }

			private int nearbyValue;
			private List<Point> nearbyPoints;

			public FillChromosome(List<FillGene> genes) {
				Genes = genes;
				SetFitness();
			}

			public void SetFitness() {
				int value = 0;
				foreach (var item in FillBusinessLogic.FillArray) {
					if (item.HasValue && item.Number > 1) {
						nearbyPoints = new List<Point>() {
							new Point(item.Point.X, item.Point.Y)
						};
						nearbyValue = item.Number - 1;
						GetNearbyNumbers(item.GetX(), item.GetY(), item.Number);
						value += nearbyValue;
					}
				}
				Fitness = value;
			}

			public void GetNearbyNumbers(int x, int y, int number) {
				if (x + 1 < FillBusinessLogic.FillArray.GetLength(0)) {
					if ((Genes.Exists(z => z.X == x + 1 && z.Y == y && z.Number == number) || FillBusinessLogic.FillArray[x + 1, y].Number == number) && nearbyPoints.Count < number - 1 && !nearbyPoints.Exists(z => z.X == x + 1 && z.Y == y)) {
						nearbyValue--;
						nearbyPoints.Add(new Point(x + 1, y));
						GetNearbyNumbers(x + 1, y, number);
					}
				}
				if (y + 1 < FillBusinessLogic.FillArray.GetLength(1)) {
					if (Genes.Exists(z => z.X == x && z.Y == y + 1 && z.Number == number || FillBusinessLogic.FillArray[x, y + 1].Number == number) && nearbyPoints.Count < number - 1 && !nearbyPoints.Exists(z => z.X == x && z.Y == y + 1)) {
						nearbyValue--;
						nearbyPoints.Add(new Point(x, y + 1));
						GetNearbyNumbers(x, y + 1, number);
					}
				}
				if (x - 1 >= 0) {
					if (Genes.Exists(z => z.X == x - 1 && z.Y == y && z.Number == number || FillBusinessLogic.FillArray[x - 1, y].Number == number) && nearbyPoints.Count < number - 1 && !nearbyPoints.Exists(z => z.X == x - 1 && z.Y == y)) {
						nearbyValue--;
						nearbyPoints.Add(new Point(x - 1, y));
						GetNearbyNumbers(x - 1, y, number);
					}
				}
				if (y - 1 >= 0) {
					if (Genes.Exists(z => z.X == x && z.Y == y - 1 && z.Number == number || FillBusinessLogic.FillArray[x, y - 1].Number == number) && nearbyPoints.Count < number - 1 && !nearbyPoints.Exists(z => z.X == x && z.Y == y - 1)) {
						nearbyValue--;
						nearbyPoints.Add(new Point(x, y - 1));
						GetNearbyNumbers(x, y - 1, number);
					}
				}
			}
		}

		//gene class
		public class FillGene {

			public int X { get; private set; }
			public int Y { get; private set; }
			public int Number { get; private set; }

			public FillGene(int x, int y, int maxValue) {
				X = x;
				Y = y;
				Number = RandomNumber(1, maxValue + 1);
			}

			public void SetNumber(int number) {
				Number = number;
			}
		}
	}
}

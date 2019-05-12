using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
	public class Genetic {

		//random stuff
		private static readonly Random random = new Random();
		private static readonly object syncLock = new object();
		public static int RandomNumber(int min, int max) {
			lock (syncLock) { // synchronize
				return random.Next(min, max);
			}
		}

        //fields
        private int maxNumber;
		private Population population;
		private Population halfPopulation;
		private List<Unit> units;
        
		//methods
		public Genetic(List<Unit> units, int size) {
            this.units = units;
            SetMaxNumber();
			SetPopulation();
			population.OrderPopulationByFitness();
			Unit[,] us = population.Chromosomes.First().GetArray(size);
			int count = 0;
			while (!population.Chromosomes.Exists(x => x.Fitness == 0) && count < Consts.FILL_GCOUNT) {
				Crossover();
				population.OrderPopulationByFitness();
				us = population.Chromosomes.First().GetArray(size);
				for (int i = 0; i < us.GetLength(0); i++) {
					string s = "";
					for (int j = 0; j < us.GetLength(1); j++) {
						s += us[i, j].Number;
					}
					Debug.WriteLine(s);
				}
				Debug.WriteLine("");
				count++;
			}
            FillBusinessLogic.FillArray = us;
		}

        private void SetMaxNumber() {
            maxNumber = 0;
            foreach (var unit in FillBusinessLogic.FillArray) {
                if (unit.Number > maxNumber) maxNumber = unit.Number;
            }
        }

		private void SetPopulation() {
			population = new Population();
			for (int i = 0; i < Consts.FILL_PCOUNT; i++) {
				List<Gene> genes = new List<Gene>();
				foreach (var item in units) {
					genes.Add(new Gene(item.GetX(), item.GetY(), maxNumber));
				}
				population.Chromosomes.Add(new Chromosome(genes));
			}
		}

		private void Crossover() {
			halfPopulation = new Population() {
				Chromosomes = new List<Chromosome>()
			};
			for (int i = 0; i < Consts.FILL_PCOUNT / 2; i++) {
				halfPopulation.Chromosomes.Add(population.Chromosomes[i]);
			}
			population.Chromosomes.Clear();
			for (int i = 0; i < Consts.FILL_PCOUNT / 2; i++) {
				Chromosome chromosome1 = halfPopulation.Chromosomes.First();
				int randomValue = RandomNumber(1, (Consts.FILL_PCOUNT / 2) - 1);
				Chromosome chromosome2 = halfPopulation.Chromosomes[randomValue];
				Chromosome newChromosome1 = new Chromosome(chromosome1.Genes.ToList());
				Chromosome newChromosome2 = new Chromosome(chromosome2.Genes.ToList());
				for (int j = 0; j < chromosome1.Genes.Count; j++) {
					if (chromosome2.Genes[j].Number != chromosome1.Genes[j].Number) {
						int randomValue3 = RandomNumber(0, maxNumber);
						if (randomValue3 == 1) {
							newChromosome1.Genes[j].SetNumber(chromosome2.Genes[j].Number);
						}
					}
				}
				for (int j = 0; j < chromosome2.Genes.Count; j++) {
					if (chromosome2.Genes[j].Number != chromosome1.Genes[j].Number) {
						int randomValue3 = RandomNumber(0, maxNumber);
						if (randomValue3 == 1) {
							newChromosome2.Genes[j].SetNumber(chromosome1.Genes[j].Number);
						}
					}
				}
				Mutation(newChromosome1);
				Mutation(newChromosome2);
			}
		}

        private void Mutation(Chromosome chromosome) {
            int newNumber;
            int geneIndex = RandomNumber(0, units.Count);
            do {
                newNumber = RandomNumber(1, maxNumber);
            } while (newNumber == chromosome.Genes[geneIndex].Number);
            chromosome.Genes[geneIndex].SetNumber(newNumber);
            population.Chromosomes.Add(chromosome);
        }

		//population class
		public class Population {

			public List<Chromosome> Chromosomes { get; set; }

			public Population() {
				Chromosomes = new List<Chromosome>();
			}

			public void OrderPopulationByFitness() {
				Chromosomes = Chromosomes.OrderBy(x => x.Fitness).ToList();
			}
		}

		//chromosome class
		public class Chromosome {

			public List<Gene> Genes { get; set; }
			public int Fitness { get; set; }

			private int nearbyValue;
			private List<Point> nearbyPoints;

			public Chromosome(List<Gene> genes) {
				Genes = genes;
				SetFitness();
			}

			public Chromosome() { }

			public void SetFitness() {
				Fitness = 0;
				int value = 0;
				foreach (var item in FillBusinessLogic.tempFillArray) {
					if (item.DefaultNumber && item.Number > 1) {
						nearbyPoints = new List<Point>() {
							new Point(item.Point.X, item.Point.Y)
						};
						nearbyValue = (item.Number - 1) * 2;
						GetNearbyNumbersByEnvironment(item.GetX(), item.GetY(), item.Number);
						GetNearbyNumbersByConnected(item.GetX(), item.GetY(), item.Number);
						
						//todo az adott környezetben mennyi olyan szám van
						value += nearbyValue;
					}
				}
				Fitness = value;
			}

			private void GetNearbyNumbersByEnvironment(int x, int y, int number) {
				int nvalue = 0;
				foreach (var item in Genes) {
					if (Math.Abs(x - item.X) + Math.Abs(y - item.Y) < number && item.Number == number) {
						nvalue++;
						if (nvalue == number - 1) break;
					}
				}
				nearbyValue -= nvalue;
			}

			private void GetNearbyNumbersByConnected(int x, int y, int number) {
				if (x + 1 < FillBusinessLogic.tempFillArray.GetLength(0)) {
					if ((Genes.Exists(z => z.X == x + 1 && z.Y == y && z.Number == number) || FillBusinessLogic.tempFillArray[x + 1, y].Number == number) && nearbyPoints.Count < number - 1 && !nearbyPoints.Exists(z => z.X == x + 1 && z.Y == y)) {
						nearbyValue--;
						nearbyPoints.Add(new Point(x + 1, y));
						GetNearbyNumbersByConnected(x + 1, y, number);
					}
				}
				if (y + 1 < FillBusinessLogic.tempFillArray.GetLength(1)) {
					if (Genes.Exists(z => z.X == x && z.Y == y + 1 && z.Number == number || FillBusinessLogic.tempFillArray[x, y + 1].Number == number) && nearbyPoints.Count < number - 1 && !nearbyPoints.Exists(z => z.X == x && z.Y == y + 1)) {
						nearbyValue--;
						nearbyPoints.Add(new Point(x, y + 1));
						GetNearbyNumbersByConnected(x, y + 1, number);
					}
				}
				if (x - 1 >= 0) {
					if (Genes.Exists(z => z.X == x - 1 && z.Y == y && z.Number == number || FillBusinessLogic.tempFillArray[x - 1, y].Number == number) && nearbyPoints.Count < number - 1 && !nearbyPoints.Exists(z => z.X == x - 1 && z.Y == y)) {
						nearbyValue--;
						nearbyPoints.Add(new Point(x - 1, y));
						GetNearbyNumbersByConnected(x - 1, y, number);
					}
				}
				if (y - 1 >= 0) {
					if (Genes.Exists(z => z.X == x && z.Y == y - 1 && z.Number == number || FillBusinessLogic.tempFillArray[x, y - 1].Number == number) && nearbyPoints.Count < number - 1 && !nearbyPoints.Exists(z => z.X == x && z.Y == y - 1)) {
						nearbyValue--;
						nearbyPoints.Add(new Point(x, y - 1));
						GetNearbyNumbersByConnected(x, y - 1, number);
					}
				}
			}

			public Unit[,] GetArray(int size) {
				Unit[,] fArray = FillBusinessLogic.tempFillArray;
				int key = 0;
				foreach (var gene in Genes) {
					fArray[gene.X, gene.Y] = new Unit(new Point(gene.X, gene.Y), key, gene.Number);
					key++;
				}
				return fArray;
			}
		}

		//gene class
		public class Gene {

			public int X { get; private set; }
			public int Y { get; private set; }
			public int Number { get; private set; }

			public Gene(int x, int y, int maxValue) {
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

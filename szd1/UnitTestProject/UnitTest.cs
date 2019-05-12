
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using szd1;
using Windows.Foundation;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
		private List<Point> points = new List<Point>() {
				new Point(0, 0),
				new Point(0, 1),
				new Point(0, 2)
			};

		private List<Point> testPoints = new List<Point>() {
				new Point(0, 1),
				new Point(1, 1),
				new Point(1, 2),
				new Point(1, 3)
			};

		private List<Point> wrongTestPoints = new List<Point>() {
				new Point(2, 3),
				new Point(1, 1),
				new Point(1, 2),
				new Point(1, 3)
			};

		private szd1.Fillomino.Classes.Unit[,] fArray;

		[TestMethod]
        public void HaveCommonPoint()
        {
			szd1.Fillomino.Classes.Variation testVariation = new szd1.Fillomino.Classes.Variation(testPoints, 4);
			szd1.Fillomino.Classes.Variation variation = new szd1.Fillomino.Classes.Variation(points, 3);
			Assert.IsTrue(variation.HaveCommonPointWithVariation(testVariation));
        }

		[TestMethod]
		public void NoCommonPoint() {
			szd1.Fillomino.Classes.Variation testVariation = new szd1.Fillomino.Classes.Variation(wrongTestPoints, 4);
			szd1.Fillomino.Classes.Variation variation = new szd1.Fillomino.Classes.Variation(points, 3);
			Assert.IsFalse(variation.HaveCommonPointWithVariation(testVariation));
		}

		[TestMethod]
		public void CheckFitness() {
			szd1.Fillomino.Algorithms.Genetic.Genetic.Population pop = new szd1.Fillomino.Algorithms.Genetic.Genetic.Population();
			pop.Chromosomes = new List<szd1.Fillomino.Algorithms.Genetic.Genetic.Chromosome>() {
				new szd1.Fillomino.Algorithms.Genetic.Genetic.Chromosome(){
					Genes = new List<szd1.Fillomino.Algorithms.Genetic.Genetic.Gene>{
					new szd1.Fillomino.Algorithms.Genetic.Genetic.Gene(0, 0, 5),
					new szd1.Fillomino.Algorithms.Genetic.Genetic.Gene(0, 1, 5),
					new szd1.Fillomino.Algorithms.Genetic.Genetic.Gene(0, 2, 5),
					}
				},
				new szd1.Fillomino.Algorithms.Genetic.Genetic.Chromosome(){
					Genes = new List<szd1.Fillomino.Algorithms.Genetic.Genetic.Gene>{
					new szd1.Fillomino.Algorithms.Genetic.Genetic.Gene(0, 0, 5),
					new szd1.Fillomino.Algorithms.Genetic.Genetic.Gene(0, 1, 5),
					new szd1.Fillomino.Algorithms.Genetic.Genetic.Gene(0, 2, 5),
					}
				},
			};
			pop.Chromosomes[0].Fitness = 1;
			pop.Chromosomes[1].Fitness = 2;
			pop.OrderPopulationByFitness();
			Assert.IsTrue(pop.Chromosomes[0].Fitness < pop.Chromosomes[1].Fitness);
		}

		[TestMethod]
		public void HasEmptyPlaces() {
			szd1.Fillomino.Algorithms.Backtrack.Backtrack bt = new szd1.Fillomino.Algorithms.Backtrack.Backtrack();
			fArray = new szd1.Fillomino.Classes.Unit[1, 2];
			fArray[0, 0] = new szd1.Fillomino.Classes.Unit(new Point(0, 0), 1);
			fArray[0, 1] = new szd1.Fillomino.Classes.Unit(new Point(0, 1), 1);
			Assert.IsTrue(bt.HasEmptyFields(fArray).X != -1);
		}
	}
}

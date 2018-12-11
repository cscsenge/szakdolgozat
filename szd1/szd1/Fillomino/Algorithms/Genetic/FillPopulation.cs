using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szd1.Fillomino.Classes;

namespace szd1.Fillomino.Algorithms.Genetic {
	public class FillPopulation {

		public List<FillChromosome> fillArrayPopulation { get; set; }
		public FillChromosome Fittest { get { return GetFittest(); } }

		public FillPopulation(int count, Unit[,] fillArray) {
			fillArrayPopulation = new List<FillChromosome>();
			for (int i = 0; i < count; i++) {
				fillArrayPopulation.Add(new FillChromosome(fillArray));
			}
		}

		public FillPopulation() {
			fillArrayPopulation = new List<FillChromosome>();
		}

		public void Add(FillChromosome chromosome) {
			fillArrayPopulation.Add(chromosome);
		}

		public FillChromosome GetFittest() {
			return fillArrayPopulation.OrderByDescending(xi => xi.Goodness).First();
		}
	}
}

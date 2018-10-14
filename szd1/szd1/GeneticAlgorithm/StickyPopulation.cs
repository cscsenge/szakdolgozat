using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szd1.Classes;

namespace szd1.GeneticAlgorithm {
	class StickyPopulation {
		public StickyChromosome fittest = new StickyChromosome();
		public List<StickyChromosome> chromosomes;

		public StickyPopulation(int count, Unit[,] table) {
			chromosomes = new List<StickyChromosome>();
			for (int i = 0; i < count; i++) {
				chromosomes.Add(new StickyChromosome(table));
			}
			fittest = GetFittest(0);
		}

		public StickyPopulation() {
			chromosomes = new List<StickyChromosome>();
		}

		public StickyChromosome GetFittest(int index) {
			return chromosomes.OrderBy(x => x.GetDistance()).ToArray()[index];
		}

		internal void Add(StickyChromosome chromosome) {
			chromosomes.Add(chromosome);
		}
	}
}

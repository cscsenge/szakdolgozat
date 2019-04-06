using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szd1.Fillomino.Classes;
using Windows.Foundation;

namespace szd1.Fillomino.Algorithms.Backtrack {
	public class FillBacktrack {

		public Dictionary<int, List<Variation>> FinalEndings { get; private set; }
		public Unit[,] FinalArray { get; private set; }
		public List<Unit[,]> FinalArrays { get; private set; }

		private List<Point> subVariations;
		private Dictionary<int, List<Variation>> allVariationsByUnits = new Dictionary<int, List<Variation>>();
		private List<Variation> variations = new List<Variation>();
		private List<List<Variation>> finalVariations = new List<List<Variation>>();
		private List<Unit[,]> finalArrays = new List<Unit[,]>();
		private List<Variation> backtrackVariations = new List<Variation>();
		private int count = -1;
		private static Random random = new Random();

		public Unit[,] ExecuteBacktrack() {
			allVariationsByUnits.Clear();
			variations.Clear();
			finalVariations.Clear();
			backtrackVariations.Clear();
			finalArrays.Clear();
			FinalArray = new Unit[FillBusinessLogic.FillArray.GetLength(0), FillBusinessLogic.FillArray.GetLength(1)];
			for (int i = 0; i < FillBusinessLogic.FillArray.GetLength(0); i++) {
				for (int j = 0; j < FillBusinessLogic.FillArray.GetLength(1); j++) {
					FinalArray[i, j] = FillBusinessLogic.FillArray[i, j];
				}
			}
			foreach (Unit fillUnit in FinalArray) {
				if (fillUnit.DefaultNumber && fillUnit.Number > 1) {
					allVariationsByUnits.Add(fillUnit.Key, new List<Variation>());
					subVariations = new List<Point>();
					subVariations.Add(fillUnit.Point);
					FindVariations(fillUnit.Number, FinalArray, fillUnit.Point);
				}
			}
			MergeVariations();
			return FinalArray;
		}

		private void MergeVariations() {
			FinalEndings = new Dictionary<int, List<Variation>>();
			Backtrack(0);
			//TODO no elements error
			FinalArrays = new List<Unit[,]>();
			DictionaryToArrayList();
			int key = random.Next(0, FinalEndings.Count - 1);
			List<Variation> final = FinalEndings.First().Value;// FinalEndings[key];
			ListToArray(final);
		}

		private void ListToArray(List<Variation> list) {
			foreach (var variation in list) {
				foreach (var variationPoint in variation.VariationPoints) {
					int key = (int)variationPoint.X * FinalArray.GetLength(0) + (int)variationPoint.Y;
					FinalArray[(int)variationPoint.X, (int)variationPoint.Y] = new Unit(variationPoint, key, variation.Number);
				}
				FinalArrays.Add(FinalArray);
			}
		}

		private void DictionaryToArrayList() {
			foreach (var item in FinalEndings) {
				ListToArray(item.Value);
			}
		}

		private void Backtrack(int key) {
			if (allVariationsByUnits.Count > 0) {
				if (key > allVariationsByUnits.Keys.Max()) {
					count++;
					List<Variation> variations = new List<Variation>();
					foreach (var item in backtrackVariations) {
						variations.Add(item);
					}
					
					FinalEndings.Add(count, variations);
					return;
				}
				if (!allVariationsByUnits.ContainsKey(key)) {
					Backtrack(key + 1);
					return;
				}
				foreach (var variation in allVariationsByUnits[key]) {
					if (!variation.HaveCommonPointWithVariationList(backtrackVariations)) {
						backtrackVariations.Add(variation);
						Backtrack(key + 1);
						backtrackVariations.Remove(variation);
					}
				}
				key--;
			}
		}

		private void AddUnitToVariations(int key, int number, Point unit, Unit[,] fillArray) {
			Point previous = subVariations.Last();
			int previousKey = fillArray[(int)previous.X, (int)previous.Y].Key;
			subVariations.Add(unit);
			if (subVariations.Count == number) {
				if (!DoesAllVariationsByUnitsContainVariation(key, subVariations)) allVariationsByUnits[key].Add(new Variation(subVariations, number));
			}
		}

		private bool DoesAllVariationsByUnitsContainVariation(int key, List<Point> points) {
			foreach (Variation variation in allVariationsByUnits[key]) {
				int j = 0;
				foreach (Point point in points) {
					if (variation.VariationPoints.Any(x => x.X == point.X && x.Y == point.Y)) {
						j++;
					}
				}
				if (j == points.Count) {
					return true;
				}
			}
			return false;
		}

		private void FindVariations(int number, Unit[,] fillArray, Point originalPosition) {
			int originalKey = fillArray[(int)originalPosition.X, (int)originalPosition.Y].Key;
			List<Point> okPlaces = GetOKPlaces(number, fillArray, originalPosition);
			foreach (Point kPoint in okPlaces) {
				if (!subVariations.Contains(kPoint) && subVariations.Count < number && subVariations.Count > 0) {
					AddUnitToVariations(originalKey, number, kPoint, fillArray);
					FindVariations(number, fillArray, originalPosition);
					if (subVariations.Count > 1) subVariations.Remove(subVariations.Last());
				}
			}
		}

		private List<Point> GetOKPlaces(int number, Unit[,] fillArray, Point originalPosition) {
			List<Point> okPlaces = new List<Point>();
			int originalKey = fillArray[(int)originalPosition.X, (int)originalPosition.Y].Key;
			foreach (Point subVariation in subVariations) {
				int x = (int)subVariation.X;
				int y = (int)subVariation.Y;
				if (y - 1 >= 0) {
					Unit tempUnit = fillArray[x, y - 1];
					if ((!tempUnit.HasValue || tempUnit.Number == number) && !(x == originalPosition.X && y - 1 == originalPosition.Y)) {
						if (!subVariations.Any(xi => xi.X == x && xi.Y == y - 1)) {
							if (!okPlaces.Contains(tempUnit.Point)) okPlaces.Add(tempUnit.Point);
						}
					}
				}
				if (x - 1 >= 0) {
					Unit tempUnit = fillArray[x - 1, y];
					if ((!tempUnit.HasValue || tempUnit.Number == number) && !(x - 1 == originalPosition.X && y == originalPosition.Y)) {
						if (!subVariations.Any(xi => xi.X == x - 1 && xi.Y == y)) {
							if (!okPlaces.Contains(tempUnit.Point)) okPlaces.Add(tempUnit.Point);
						}
					}
				}
				if (x + 1 < fillArray.GetLength(1)) {
					Unit tempUnit = fillArray[x + 1, y];
					if ((!tempUnit.HasValue || tempUnit.Number == number) && !(x + 1 == originalPosition.X && y == originalPosition.Y)) {
						if (!subVariations.Any(xi => xi.X == x + 1 && xi.Y == y)) {
							if (!okPlaces.Contains(tempUnit.Point)) okPlaces.Add(tempUnit.Point);
						}
					}
				}
				if (y + 1 < fillArray.GetLength(0)) {
					Unit tempUnit = fillArray[x, y + 1];
					if ((!tempUnit.HasValue || tempUnit.Number == number) && !(x == originalPosition.X && y + 1 == originalPosition.Y)) {
						if (!subVariations.Any(xi => xi.X == x && xi.Y == y + 1)) {
							if (!okPlaces.Contains(tempUnit.Point)) okPlaces.Add(tempUnit.Point);
						}
					}
				}
			}
			return okPlaces;
		}

		private void MergeVariations(Variation fillVariation, Unit[,] fillArray) {
			//get new fillArray
			Unit[,] newFillArray = GetNewFillArray(fillVariation, fillArray);
			//find an empty place to fill
			Unit emptyUnit = FindUnitToFill(newFillArray);
			//get variations that fills
			List<Variation> variationList = FindVariationsToFillUnit(emptyUnit);
			if (emptyUnit != null && variationList.Count > 0) {
				foreach (var item in variationList) {
					if (IsVariableFills(item, newFillArray)) { //if it fills
						ChangeInVariation(item, newFillArray, true);
						finalArrays.Add(newFillArray);
						MergeVariations(item, newFillArray);
						newFillArray = RemoveVariationFromArray(item, newFillArray);
						ChangeInVariation(item, newFillArray, false);
					}
				}
			}
		}

		private void ChangeInVariation(Variation variation, Unit[,] newFillArray, bool state) {
			foreach (var item in variation.VariationPoints) {
				newFillArray[(int)item.X, (int)item.Y].IsInVariation = state;
			}
		}

		private bool IsVariableFills(Variation variation, Unit[,] newFillArray) {
			foreach (var item in variation.VariationPoints) {
				if (newFillArray[(int)item.X, (int)item.Y].HasValue && (!newFillArray[(int)item.X, (int)item.Y].DefaultNumber || newFillArray[(int)item.X, (int)item.Y].IsInVariation)) {
					return false;
				}
			}
			return true;
		}

		private Unit[,] RemoveVariationFromArray(Variation variation, Unit[,] newFillArray) {
			foreach (var variationPoint in variation.VariationPoints) {
				if (!newFillArray[(int)variationPoint.X, (int)variationPoint.Y].DefaultNumber) {
					newFillArray[(int)variationPoint.X, (int)variationPoint.Y].Number = 0;
				}
			}
			return newFillArray;
		}

		private Unit[,] GetNewFillArray(Variation variation, Unit[,] fillArray) {
			if (variation != null) {
				Unit[,] newFillArray = fillArray;
				foreach (var variationPoint in variation.VariationPoints) {
					newFillArray[(int)variationPoint.X, (int)variationPoint.Y].Number = variation.Number;
				}
				return newFillArray;
			} else {
				return fillArray;
			}
		}

		private Unit FindUnitToFill(Unit[,] newFillArray) {
			for (int i = 0; i < newFillArray.GetLength(1); i++) {
				for (int j = 0; j < newFillArray.GetLength(0); j++) {
					if (!newFillArray[i, j].HasValue && IsThereAVariationToFillUnit(newFillArray[i, j])) {
						return newFillArray[i, j];
					}
				}
			}
			return null;
		}

		private List<Variation> FindVariationsToFillUnit(Unit unit) {
			List<Variation> variationList = new List<Variation>();
			double x = unit.Point.X;
			double y = unit.Point.Y;
			foreach (var item in allVariationsByUnits.Values) {
				foreach (var variation in item) {
					if (variation.VariationPoints.Any(xi => xi.X == unit.Point.X && xi.Y == unit.Point.Y)) {
						variationList.Add(variation);
					}
				}
			}
			return variationList;
		}

		private bool IsThereAVariationToFillUnit(Unit unit) {
			foreach (var item in allVariationsByUnits.Values) {
				foreach (var variation in item) {
					if (!variations.Contains(variation) && variation.VariationPoints.Any(xi => xi.X == unit.Point.X && xi.Y == unit.Point.Y)) {
						return true;
					}
				}
			}
			return false;
		}
	}
}

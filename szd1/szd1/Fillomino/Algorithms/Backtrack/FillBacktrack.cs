using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szd1.Fillomino.Classes;
using Windows.Foundation;

namespace szd1.Fillomino.Algorithms.Backtrack {
	public class FillBacktrack {
		List<Point> subVariations;
		Dictionary<int, List<Variation>> allVariationsByUnits = new Dictionary<int, List<Variation>>();
		private Unit[,] fillArray;
		List<Variation> variations = new List<Variation>();
		List<List<Variation>> finalVariations = new List<List<Variation>>();
		List<Unit[,]> finalArrays = new List<Unit[,]>();

		public void ExecuteBacktrack(Unit[,] fArray) {
			fillArray = fArray;
			foreach (Unit fillUnit in fillArray) {
				if (fillUnit.DefaultNumber && fillUnit.Number > 1) {
					allVariationsByUnits.Add(fillUnit.Key, new List<Variation>());
					subVariations = new List<Point>();
					subVariations.Add(fillUnit.Point);
					FindVariations(fillUnit.Number, fillArray, fillUnit.Point);
				}
			}
			MergeVariations(null, fillArray);
			List<int> arrayCounts = new List<int>();
			foreach (Unit[,] finalArray in finalArrays) {
				int count = 0;
				for (int i = 0; i < finalArray.GetLength(1); i++) {
					for (int j = 0; j < finalArray.GetLength(0); j++) {
						if (finalArray[i, j].IsInVariation) {
							count++;
						}
					}
				}
				arrayCounts.Add(count);
			}
			List<Unit[,]> finalFinal = new List<Unit[,]>();
			int max = arrayCounts.Max();
			for (int i = 0; i < arrayCounts.Count; i++) {
				if (arrayCounts[i] == max) {
					finalFinal.Add(finalArrays[i]);
				}
			}
		}

		public void AddUnitToVariations(int key, int number, Point unit, Unit[,] fillArray) {
			Point previous = subVariations.Last();
			int previousKey = fillArray[(int)previous.X, (int)previous.Y].Key;
			subVariations.Add(unit);
			if (subVariations.Count == number) {
				if (!DoesAllVariationsByUnitsContainVariation(key, subVariations)) allVariationsByUnits[key].Add(new Variation(subVariations, number));
			}
		}

		public bool DoesAllVariationsByUnitsContainVariation(int key, List<Point> points) {
			foreach (Variation variation in allVariationsByUnits[key]) {
				int j = 0;
				foreach (Point point in points) {
					if (variation.VariationPoints.Any(x => x.X == point.X && x.Y == point.Y)){
						j++;
					}
				}
				if (j == points.Count) {
					return true;
				}
			}
			return false;
		}

		public void FindVariations(int number, Unit[,] fillArray, Point originalPosition) {
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

		public List<Point> GetOKPlaces(int number, Unit[,] fillArray, Point originalPosition) {
			List<Point> okPlaces = new List<Point>();
			int originalKey = fillArray[(int)originalPosition.X, (int)originalPosition.Y].Key;
			foreach (Point subVariation in subVariations) {
				int x = (int)subVariation.X;
				int y = (int)subVariation.Y;
				if (y - 1 >= 0) {
					Unit tempUnit = fillArray[x, y - 1];
					if ((!tempUnit.DoesHaveNumber || tempUnit.Number == number) && !(x == originalPosition.X && y - 1 == originalPosition.Y)) {
						if (!subVariations.Any(xi => xi.X == x && xi.Y == y - 1)) {
							if (!okPlaces.Contains(tempUnit.Point)) okPlaces.Add(tempUnit.Point);
						}
					}
				}
				if (x - 1 >= 0) {
					Unit tempUnit = fillArray[x - 1, y];
					if ((!tempUnit.DoesHaveNumber || tempUnit.Number == number) && !(x - 1 == originalPosition.X && y == originalPosition.Y)) {
						if (!subVariations.Any(xi => xi.X == x - 1 && xi.Y == y)) {
							if (!okPlaces.Contains(tempUnit.Point)) okPlaces.Add(tempUnit.Point);
						}
					}
				}
				if (x + 1 < fillArray.GetLength(1)) {
					Unit tempUnit = fillArray[x + 1, y];
					if ((!tempUnit.DoesHaveNumber || tempUnit.Number == number) && !(x + 1 == originalPosition.X && y == originalPosition.Y)) {
						if (!subVariations.Any(xi => xi.X == x + 1 && xi.Y == y)) {
							if (!okPlaces.Contains(tempUnit.Point)) okPlaces.Add(tempUnit.Point);
						}
					}
				}
				if (y + 1 < fillArray.GetLength(0)) {
					Unit tempUnit = fillArray[x, y + 1];
					if ((!tempUnit.DoesHaveNumber || tempUnit.Number == number) && !(x == originalPosition.X && y + 1 == originalPosition.Y)) {
						if (!subVariations.Any(xi => xi.X == x && xi.Y == y + 1)) {
							if (!okPlaces.Contains(tempUnit.Point)) okPlaces.Add(tempUnit.Point);
						}
					}
				}
			}
			return okPlaces;
		}

		public void MergeVariations(Variation fillVariation, Unit[,] fillArray) {
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

		public void ChangeInVariation(Variation variation, Unit[,] newFillArray, bool state) {
			foreach (var item in variation.VariationPoints) {
				newFillArray[(int)item.X, (int)item.Y].IsInVariation = state;
			}
		}

		public bool IsVariableFills(Variation variation, Unit[,] newFillArray) {
			foreach (var item in variation.VariationPoints) {
				if (newFillArray[(int)item.X, (int)item.Y].DoesHaveNumber && (!newFillArray[(int)item.X, (int)item.Y].DefaultNumber || newFillArray[(int)item.X, (int)item.Y].IsInVariation)) {
					return false;
				}
			}
			return true;
		}

		public Unit[,] RemoveVariationFromArray(Variation variation, Unit[,] newFillArray) {
			foreach (var variationPoint in variation.VariationPoints) {
				if (!newFillArray[(int)variationPoint.X, (int)variationPoint.Y].DefaultNumber) {
					newFillArray[(int)variationPoint.X, (int)variationPoint.Y].Number = 0;
				}
			}
			return newFillArray;
		}

		public Unit[,] GetNewFillArray(Variation variation, Unit[,] fillArray) {
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

		public Unit FindUnitToFill(Unit[,] newFillArray) {
			for (int i = 0; i < newFillArray.GetLength(1); i++) {
				for (int j = 0; j < newFillArray.GetLength(0); j++) {
					if (!newFillArray[i, j].DoesHaveNumber && IsThereAVariationToFillUnit(newFillArray[i, j])) {
						return newFillArray[i, j];
					}
				}
			}
			return null;
		}

		public List<Variation> FindVariationsToFillUnit(Unit unit) {
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

		public bool IsThereAVariationToFillUnit(Unit unit) {
			foreach (var item in allVariationsByUnits.Values) {
				foreach (var variation in item) {
					if (!variations.Contains(variation) && variation.VariationPoints.Any(xi => xi.X == unit.Point.X && xi.Y == unit.Point.Y)) {
						return true;
					}
				}
			}
			return false;
		}

		//public void MergeVariations() {
		//	int firstKey = allVariationsByUnits.First().Key;
		//	List<Variation> firstVariations = allVariationsByUnits[firstKey];
		//	allVariationsByUnits.Remove(firstKey);
		//	finalVariations = new List<List<Variation>>();
		//	foreach (Variation firstVariation in firstVariations) {
		//		variations = new List<Variation>();
		//		variations.Add(firstVariation);
		//		MergeBacktrack(firstKey, firstVariation);
		//	}
		//}

		//public void MergeBacktrack(int previousKey, Variation previousVariation) {
		//	int currentKey = GetKeyByPreviousKey(previousKey);
		//	if (currentKey > -1) {
		//		List<Variation> variationList = allVariationsByUnits[currentKey];
		//		foreach (Variation variation in variationList) {
		//			if (DoesVariationsFit(variation) && !variations.Contains(variation)) {
		//				MergeAdding(variation);
		//				MergeBacktrack(currentKey, variation);
		//				if (variations.Count > 1) variations.Remove(variations.Last());
		//			}
		//		}
		//	}
		//}

		//public void MergeAdding(Variation variation) {
		//	variations.Add(variation);
		//	if (variations.Count == allVariationsByUnits.Count) {
		//		finalVariations.Add(variations);
		//	}
		//}

		//public bool DoesVariationsFit(Variation variation1) {
		//	List<Point> allVariationPoints = new List<Point>();
		//	foreach (var variation in variations) {
		//		foreach (var point in variation.VariationPoints) {
		//			allVariationPoints.Add(point);
		//		}
		//	}
		//	foreach (Point variationPoint in variation1.VariationPoints) {
		//		if (allVariationPoints.Any(xi => xi.X == variationPoint.X && xi.Y == variationPoint.Y)) {
		//			return false;
		//		}
		//	}
		//	return true;
		//}

		//public int GetKeyByPreviousKey(int previousKey) {
		//	if (keys.Last() != previousKey) {
		//		int index = keys.IndexOf(previousKey);
		//		index = keys[index + 1];
		//		return index;
		//	}
		//	return -1;
		//}
	}
}

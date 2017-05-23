using System;
using System.Collections.Generic;

namespace Match3 {

    struct Cluster {
        public int column { get; private set; }
        public int row { get; private set; }
        public int length { get; private set; }
        public bool horizontal { get; private set; }

        public Cluster(int row, int column, int length, bool horizontal) {
            this.column = column;
            this.row = row;
            this.length = length;
            this.horizontal = horizontal;
        }
    }

    struct Move {
        public int col1 { get; private set; }
        public int row1 { get; private set; }
        public int col2 { get; private set; }
        public int row2 { get; private set; }

        public Move(int col1, int row1, int col2, int row2) {
            this.col1 = col1;
            this.row1 = row1;
            this.col2 = col2;
            this.row2 = row2;
        }
    }

    public class GameField {
        private const int MIN_CLUSTER_LEN = 3;

        public int fieldSize { get; private set; }
        public int numElements { get; private set; }
        public int[,] field { get; private set; }

        public uint score { get; private set; }
        private List<Move> avaibleMoves = new List<Move>();

        public GameField(int fieldSize, int numElements) {
            this.fieldSize = fieldSize;
            this.numElements = numElements;
            this.score = 0;

            this.field = new int[fieldSize, fieldSize];

            generateField();
        }

        public void swap(int x1, int y1, int x2, int y2) {
			int tmp = this.field[y1, x1];
			this.field[y1, x1] = this.field[y2, x2];
			this.field[y2, x2] = tmp;
		}

		public bool canSwap(int x1, int y1, int x2, int y2) {
			if ((x1 == x2 && y1 == y2 + 1) || (x1 == x2 && y1 == y2 - 1) ||
				(x1 == x2 + 1 && y1 == y2) || (x1 == x2 - 1 && y1 == y2)) {
				return true;
			}
			else {
				return false;
			}
		}

        private void generateField() {
            Random rand = new Random();

            for (int i = 0; i < this.fieldSize; i++) {
                for (int j = 0; j < this.fieldSize; j++) {
                    this.field[i, j] = rand.Next(this.numElements);
                }
            }

            resolveClusters();
        }

        public void resolveClusters() {
            List<Cluster> clusters = findClusters();

            while (clusters.Count > 0) {
                removeClusters(clusters);
                shiftTiles();

                clusters = findClusters();
            }
        }

        private List<Cluster> findClusters() {
            List<Cluster> clusters = new List<Cluster>();

            // horizontal
            for (int i = 0; i < fieldSize; i++) {
                int matchLen = 1;

                for (int j = 0; j < fieldSize; j++) {
                    bool check = false;

                    if (j == fieldSize - 1) {
                        check = true;
                    }
                    else {
                        if (this.field[i, j] == this.field[i, j + 1] && this.field[i, j] != -1) {
                            matchLen++;
                        }
                        else {
                            check = true;
                        }
                    }

                    if (check) {
                        if (matchLen >= MIN_CLUSTER_LEN) {
                            clusters.Add(new Cluster(i, j + 1 - matchLen, matchLen, true));
                        }

                        matchLen = 1;
                    }
                }
            }

			// vertical
			for (int j = 0; j < fieldSize; j++) {
				int matchLen = 1;

				for (int i = 0; i < fieldSize; i++) {
					bool check = false;

					if (i == fieldSize - 1) {
						check = true;
					}
					else {
						if (this.field[i, j] == this.field[i + 1, j] && this.field[i, j] != -1) {
							matchLen++;
						}
						else {
							check = true;
						}
					}

					if (check) {
						if (matchLen >= MIN_CLUSTER_LEN) {
							clusters.Add(new Cluster(i + 1 - matchLen, j, matchLen, false));
						}

						matchLen = 1;
					}
				}
			}

            return clusters;
        }

        private void removeClusters(List<Cluster> clusters) {
            loopClusters(clusters);
		}

        private void loopClusters(List<Cluster> clusters) {
            for (int i = 0; i < clusters.Count; i++) {
                Cluster cluster = clusters[i];
                int cOffset = 0;
                int rOffset = 0;

                for (int j = 0; j < cluster.length; j++) {
                    this.field[cluster.row + rOffset, cluster.column + cOffset] = -1;
                    this.score++;

                    if (cluster.horizontal) {
                        cOffset++;
                    }
                    else {
                        rOffset++;
                    }
                }
            }
        }

        private void shiftTiles() {
            
        }

        private void findMoves() {
            this.avaibleMoves.Clear();
            int[,] diff = { { 1, 0 }, { 0, 1 } };

            for (var d = 0; d < diff.Length; d++) {
                for (var i = 0; i < fieldSize; i++) {
                    for (var j = 0; j < fieldSize; j++) {
                        swap(i, j, i + diff[d, 0], j + diff[d, 1]);
                        List<Cluster> clusters = findClusters();
                        swap(i, j, i + diff[d, 0], j + diff[d, 1]);

                        if (clusters.Count > 0) {
                            this.avaibleMoves.Add(new Move(i, j, i + diff[d, 0], j + diff[d, 1]));
                        }
                    }
                }
            }
        }
    }
}

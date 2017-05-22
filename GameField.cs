using System;

namespace Match3
{
    public class GameField
    {
        public int fieldSize { get; private set; }
        public int numElements { get; private set; }
        public int[,] field { get; private set; }

        public GameField(int fieldSize, int numElements)
        {
            this.fieldSize = fieldSize;
            this.numElements = numElements;

            field = new int[fieldSize, fieldSize];
            generateField();
        }

        private void generateField() {
            Random rand = new Random();

            for (int i = 0; i < this.fieldSize; i++) {
                for (int j = 0; j < this.fieldSize; j++) {
                    this.field[i, j] = rand.Next(this.numElements);
                }
            }
        }

        private bool canSwap(int x1, int y1, int x2, int y2) {
            if ((x1 == x2 && y1 == y2 + 1) || (x1 == x2 && y1 == y2 - 1) ||
                (x1 == x2 + 1 && y1 == y2) || (x1 == x2 - 1 && y1 == y2)) {
                return true;
            }
            else {
                return false;
            }
        }

        public bool swap(int x1, int y1, int x2, int y2) {
            if (canSwap(x1, y1, x2, y2)) {
                int tmp = this.field[y1, x1];
                this.field[y1, x1] = this.field[y2, x2];
                this.field[y2, x2] = tmp;
                return true;
            }
            else {
                return false;
            }
        }
    }
}

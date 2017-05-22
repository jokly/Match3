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
    }
}

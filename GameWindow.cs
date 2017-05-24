using Gtk;
using Gdk;
using Cairo;
using System;
using System.Collections.Generic;
using Match3;

namespace Match3 {
    struct Offset {
        public int x, y;

        public Offset(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

    enum State {Game, Animation}

    public partial class GameWindow : Gtk.Window {
        GameField gameField;

        private const int FIELD_SIZE = 8;
        private const int NUM_ElEMENTS = 5;
        private const int CELL_SIZE = 70;
        private const int MARGIN = 2;
        private const int GAME_DURATION = 60 * 1000;
        private const int TIMER_FREQUENCY = 100;
		private double[,] colors = {
			{255, 255, 255}, // White
            {0, 204, 0}, // Green
            {255, 0, 0}, // Red
            {204, 0, 102}, // Purple
            {102, 204, 255}, // Orange
            {255, 255, 0} // Yellow
        };

        private Offset[,] offsets;
        private int choosenX = -1, choosenY = -1, tx, ty;
        private int timeLeft = GAME_DURATION;
        private bool isChoosen = false;
        private bool wasMove = false;
        private State state;

        public GameWindow() : base(Gtk.WindowType.Toplevel) {
            this.Build();

            this.state = Match3.State.Game;

            for (int i = 0; i < colors.Length / 3; i++) {
                for (int j = 0; j < 3; j++) {
                    colors[i, j] /= 255.0;
                }
            }

            offsets = new Offset[FIELD_SIZE, FIELD_SIZE];
            for (int i = 0; i < FIELD_SIZE; i++) {
                for (int j = 0; j< FIELD_SIZE; j++) {
                    offsets[i, j] = new Offset(0, 0);
                }
            }

            gameField = new GameField(FIELD_SIZE, NUM_ElEMENTS, updateOffsets);

            drawingarea.AddEvents((int)EventMask.ButtonPressMask);
            drawingarea.ButtonPressEvent += OnFieldClick;

            GLib.Timeout.Add(TIMER_FREQUENCY, new GLib.TimeoutHandler(OnTimer));
        }

        bool OnTimer() {
            drawingarea.QueueDraw();

            animation();
            updateTime();
            updateScore();

            if (checkEnd())
                return false;

            return true;
        }

        private void animation() {
			bool isAnim = false;
			for (int i = 0; i < FIELD_SIZE; i++) {
				for (int j = 0; j < FIELD_SIZE; j++) {
					if (offsets[i, j].x != 0) {
						offsets[i, j].x -= offsets[i, j].x / Math.Abs(offsets[i, j].x) * 24;
						isAnim = true;
					}
					if (offsets[i, j].y != 0) {
						offsets[i, j].y -= offsets[i, j].y / Math.Abs(offsets[i, j].y) * 24;
						isAnim = true;
					}
				}
			}

			if (!isAnim && this.state == Match3.State.Animation) {
				uint oldScore = gameField.score;
				this.gameField.resolveClusters();

				if (oldScore == gameField.score && this.wasMove) {
					gameField.swap(this.choosenX, this.choosenY, this.tx, this.ty);
					updateOffsets(this.choosenX, this.choosenY, this.tx, this.ty);
					this.wasMove = false;
				}

				this.state = Match3.State.Game;
			}
        }

        private void updateTime() {
			this.timeLeft -= TIMER_FREQUENCY;
			label_time.Text = "Time: " + this.timeLeft / 1000;    
        }

		private void updateScore() {
            label_score.Text = "Score: " + gameField.score;
		}

        private bool checkEnd() {
			if (this.timeLeft <= 0) {
				new GameOverWindow(gameField.score);
				this.Destroy();

				return true;
			}

            return false;
        }

        private void drawField(Cairo.Context cc) {
            cc.LineWidth = 1;

            int x = MARGIN, y = MARGIN;

            for (int i = 0; i < gameField.fieldSize; i++) {
                for (int j = 0; j < gameField.fieldSize; j++) {
                    cc.Rectangle(new PointD(x + offsets[i, j].x, y + offsets[i, j].y),
                                 CELL_SIZE, CELL_SIZE);
                    int id = gameField.field[i, j] + 1;
                    cc.SetSourceRGB(colors[id, 0], colors[id, 1], colors[id, 2]);
                    cc.Fill();
                    x += CELL_SIZE + MARGIN;
                }
                y += CELL_SIZE + MARGIN;
                x = MARGIN;
            }
        }

        void drawSelectionFrame(Cairo.Context cc) {
            if (this.choosenX == -1 || this.choosenY == -1 || !this.isChoosen)
                return;
            
			cc.LineWidth = 2;
            cc.Rectangle(new PointD(choosenX * CELL_SIZE + MARGIN * (choosenX + 1),
                                    choosenY * CELL_SIZE + MARGIN * (choosenY + 1)),
                         CELL_SIZE, CELL_SIZE);
            cc.SetSourceRGB(0, 0, 102);
            cc.StrokePreserve();
        }

        void drawAvaibleMoves(Cairo.Context cc) {
			if (checkbutton_showMoves.Active) {
				List<Move> avaibleMove = gameField.findMoves();

                cc.SetSourceRGB(0, 0, 102);
                cc.LineWidth = 2;
                int cellSize = CELL_SIZE + MARGIN;
                int offset = cellSize / 2;
				for (int i = 0; i < avaibleMove.Count; i++) {
                    int y1 = avaibleMove[i].col1 * cellSize + offset;
                    int x1 = avaibleMove[i].row1 * cellSize + offset;
                    int y2 = avaibleMove[i].col2 * cellSize + offset;
                    int x2 = avaibleMove[i].row2 * cellSize + offset;

                    cc.MoveTo(new PointD(x1, y1));
                    cc.LineTo(new PointD(x2, y2));
                    cc.Stroke();
				}
			}
        }

        void updateOffsets(int x1, int y1, int x2, int y2) {
            this.state = Match3.State.Animation;
            int offsetX = (CELL_SIZE + MARGIN) * (x2 - x1);
            int offsetY = (CELL_SIZE + MARGIN) * (y2 - y1);

            offsets[y1, x1].x = offsetX;
            offsets[y1, x1].y = offsetY;
            offsets[y2, x2].x = -offsetX;
            offsets[y2, x2].y = -offsetY;
        }

        protected void OnDeleteEvent(object o, Gtk.DeleteEventArgs args) {
            this.timeLeft = 0;
        }

        protected void OnDrawingareaExposeEvent(object o, Gtk.ExposeEventArgs args) {
            DrawingArea area = (DrawingArea)o;
            Cairo.Context cc = Gdk.CairoHelper.Create(area.GdkWindow);

            drawField(cc);
            drawSelectionFrame(cc);
            drawAvaibleMoves(cc);

			((IDisposable)cc.GetTarget()).Dispose();
			((IDisposable)cc).Dispose();
        }

        private void OnFieldClick(object o, ButtonPressEventArgs args) {
            if (!this.isChoosen) {
                this.choosenX = (int)(args.Event.X / (CELL_SIZE + MARGIN));
                this.choosenY = (int)(args.Event.Y / (CELL_SIZE + MARGIN));
                this.isChoosen = true;
            }
            else {
                this.tx = (int)(args.Event.X / (CELL_SIZE + MARGIN));
                this.ty = (int)(args.Event.Y / (CELL_SIZE + MARGIN));

                if (gameField.canSwap(this.choosenX, this.choosenY, this.tx, this.ty)) {
                    gameField.swap(this.choosenX, this.choosenY, this.tx, this.ty);
                    updateOffsets(this.choosenX, this.choosenY, this.tx, this.ty);
                }

                this.isChoosen = false;
                this.wasMove = true;
            }
        }
    }
}

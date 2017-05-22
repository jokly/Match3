﻿using Gtk;
using Gdk;
using Cairo;
using System;
using Match3;

namespace Match3 {
    public partial class GameWindow : Gtk.Window {
        GameField gameField;

        private const int CELL_SIZE = 70;
        private const int MARGIN = 2;
        private readonly int[,] COLORS = {
            {0, 204, 0},
            {255, 0, 0},
            {204, 0, 102},
            {102, 51, 0},
            {255, 255, 0}
        };
        private const int GAME_DURATION = 10 * 1000;
        private const int TIMER_FREQUENCY = 100;

        private int choosenX = -1, choosenY = -1;
        private int timeLeft = GAME_DURATION;
        private bool isChoosen = false;

        public GameWindow() : base(Gtk.WindowType.Toplevel) {
            this.Build();

            gameField = new GameField(8, 5);

            drawingarea.AddEvents((int)EventMask.ButtonPressMask);
            drawingarea.ButtonPressEvent += OnFieldClick;

            GLib.Timeout.Add(TIMER_FREQUENCY, new GLib.TimeoutHandler(OnTimer));
        }

        bool OnTimer() {
            drawingarea.QueueDraw();
            this.timeLeft -= TIMER_FREQUENCY;

            label_time.Text = "Time: " + this.timeLeft / 1000;

            if (this.timeLeft <= 0) {
                new GameOverWindow();
                this.Destroy();
                return false;
            }

            return true;
        }

        private void DrawField(Cairo.Context cc) {
            cc.LineWidth = 1;

            int x = MARGIN, y = MARGIN;

            for (int i = 0; i < gameField.fieldSize; i++) {
                for (int j = 0; j < gameField.fieldSize; j++) {
                    cc.Rectangle(new PointD(x, y), CELL_SIZE, CELL_SIZE);
                    int id = gameField.field[i, j];
                    cc.SetSourceRGB(COLORS[id, 0], COLORS[id, 1], COLORS[id, 2]);
                    cc.Fill();
                    x += CELL_SIZE + MARGIN;
                }
                y += CELL_SIZE + MARGIN;
                x = MARGIN;
            }
        }

        void DrawSelectionFrame(Cairo.Context cc) {
            if (choosenX == -1 || choosenY == -1)
                return;
            
			cc.LineWidth = 2;
            cc.Rectangle(new PointD(choosenX * CELL_SIZE + MARGIN * (choosenX + 1),
                                    choosenY * CELL_SIZE + MARGIN * (choosenY + 1)),
                         CELL_SIZE, CELL_SIZE);
            cc.SetSourceRGB(0, 0, 102);
            cc.StrokePreserve();
        }

        protected void OnDeleteEvent(object o, Gtk.DeleteEventArgs args) {
            this.timeLeft = 0;
        }

        protected void OnDrawingareaExposeEvent(object o, Gtk.ExposeEventArgs args) {
            DrawingArea area = (DrawingArea)o;
            Cairo.Context cc = Gdk.CairoHelper.Create(area.GdkWindow);

            DrawField(cc);
            DrawSelectionFrame(cc);

			((IDisposable)cc.GetTarget()).Dispose();
			((IDisposable)cc).Dispose();
        }

        private void OnFieldClick(object o, ButtonPressEventArgs args) {
            if (!isChoosen) {
                this.choosenX = (int)(args.Event.X / (CELL_SIZE + MARGIN));
                this.choosenY = (int)(args.Event.Y / (CELL_SIZE + MARGIN));
                isChoosen = true;
            }
            else {
                int tx = (int)(args.Event.X / (CELL_SIZE + MARGIN));
                int ty = (int)(args.Event.Y / (CELL_SIZE + MARGIN));

                gameField.swap(this.choosenX, this.choosenY, tx, ty);

                this.choosenX = -1;
                this.choosenY = -1;
                isChoosen = false;
            }
        }
    }
}

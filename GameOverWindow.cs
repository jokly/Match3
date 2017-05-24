using System;
namespace Match3
{
    public partial class GameOverWindow : Gtk.Window
    {
        public GameOverWindow(uint score) : base(Gtk.WindowType.Toplevel) {
            this.Build();

            label_score.Text = "Score: " + score;
        }

        protected void OnDeleteEvent(object o, Gtk.DeleteEventArgs args) {
            button_toMenu.Click();
        }

        protected void OnButtonToMenuClicked(object sender, EventArgs e) {
            new MainWindow();
            this.Destroy();
        }
    }
}

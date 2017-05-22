using System;
namespace Match3
{
    public partial class GameWindow : Gtk.Window
    {
        public GameWindow() :
                base(Gtk.WindowType.Toplevel)
        {
            this.Build();
        }

        protected void OnDeleteEvent(object o, Gtk.DeleteEventArgs args)
        {
            new MainWindow();
            this.Destroy();
        }
    }
}

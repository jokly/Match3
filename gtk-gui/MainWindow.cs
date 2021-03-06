
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.VBox vbox_mainMenu;

	private global::Gtk.Label label_main;

	private global::Gtk.Button button_play;

	protected virtual void Build()
	{
		global::Stetic.Gui.Initialize(this);
		// Widget MainWindow
		this.WidthRequest = 300;
		this.HeightRequest = 200;
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString("Match3");
		this.Icon = global::Gdk.Pixbuf.LoadFromResource("Match3.icon.png");
		this.WindowPosition = ((global::Gtk.WindowPosition)(1));
		this.Resizable = false;
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox_mainMenu = new global::Gtk.VBox();
		this.vbox_mainMenu.Name = "vbox_mainMenu";
		this.vbox_mainMenu.BorderWidth = ((uint)(50));
		// Container child vbox_mainMenu.Gtk.Box+BoxChild
		this.label_main = new global::Gtk.Label();
		this.label_main.Name = "label_main";
		this.label_main.LabelProp = global::Mono.Unix.Catalog.GetString("Main menu");
		this.label_main.UseMarkup = true;
		this.vbox_mainMenu.Add(this.label_main);
		global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox_mainMenu[this.label_main]));
		w1.Position = 0;
		w1.Expand = false;
		w1.Fill = false;
		// Container child vbox_mainMenu.Gtk.Box+BoxChild
		this.button_play = new global::Gtk.Button();
		this.button_play.CanFocus = true;
		this.button_play.Name = "button_play";
		this.button_play.UseUnderline = true;
		this.button_play.Label = global::Mono.Unix.Catalog.GetString("Play");
		this.vbox_mainMenu.Add(this.button_play);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox_mainMenu[this.button_play]));
		w2.Position = 1;
		w2.Fill = false;
		this.Add(this.vbox_mainMenu);
		if ((this.Child != null))
		{
			this.Child.ShowAll();
		}
		this.DefaultWidth = 800;
		this.DefaultHeight = 600;
		this.Show();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);
		this.button_play.Clicked += new global::System.EventHandler(this.OnButtonPlayClicked);
	}
}

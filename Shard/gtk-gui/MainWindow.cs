
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.Fixed fixed1;
	
	private global::Gtk.Label Label;
	
	private global::Gtk.Entry EnterCommand;
	
	private global::Gtk.Button SendCommand;
	
	private global::Gtk.Label HeartOutput;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.fixed1 = new global::Gtk.Fixed ();
		this.fixed1.Name = "fixed1";
		this.fixed1.HasWindow = false;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.Label = new global::Gtk.Label ();
		this.Label.Name = "Label";
		this.Label.LabelProp = global::Mono.Unix.Catalog.GetString ("Enter Command:");
		this.fixed1.Add (this.Label);
		global::Gtk.Fixed.FixedChild w1 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.Label]));
		w1.X = 139;
		w1.Y = 33;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.EnterCommand = new global::Gtk.Entry ();
		this.EnterCommand.CanFocus = true;
		this.EnterCommand.Name = "EnterCommand";
		this.EnterCommand.IsEditable = true;
		this.EnterCommand.InvisibleChar = '●';
		this.fixed1.Add (this.EnterCommand);
		global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.EnterCommand]));
		w2.X = 115;
		w2.Y = 57;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.SendCommand = new global::Gtk.Button ();
		this.SendCommand.CanFocus = true;
		this.SendCommand.Name = "SendCommand";
		this.SendCommand.UseUnderline = true;
		this.SendCommand.Label = global::Mono.Unix.Catalog.GetString ("Submit Command");
		this.fixed1.Add (this.SendCommand);
		global::Gtk.Fixed.FixedChild w3 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.SendCommand]));
		w3.X = 127;
		w3.Y = 85;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.HeartOutput = new global::Gtk.Label ();
		this.HeartOutput.Name = "HeartOutput";
		this.HeartOutput.LabelProp = global::Mono.Unix.Catalog.GetString ("Server Output: ");
		this.HeartOutput.Wrap = true;
		this.fixed1.Add (this.HeartOutput);
		global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.fixed1 [this.HeartOutput]));
		w4.X = 6;
		w4.Y = 168;
		this.Add (this.fixed1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 400;
		this.DefaultHeight = 300;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.SendCommand.Clicked += new global::System.EventHandler (this.Send_Clicked);
	}
}

// main.cs created with MonoDevelop
// User: shana at 05:51 20/12/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Drawing;
using System.Windows.Forms;
using Mono.WebBrowser;
using Mono.WebBrowser.DOM;
using System.ComponentModel;

namespace standalone
{
	public class MainForm : Form
	{
		public static void Main ()
		{
			Application.Run (new MainForm ());
		}
		
		public MainForm ()
		{		
			helper = new Page ();
			gui ();
			loadWebHost ();	
			address.Text = "file:///mono/test.html";
			helper.TextChanged += delegate (string text) {body.Text += text + "\r\n";};
			helper.RootNodeChanged += delegate () {
				DomInspector d = new DomInspector(helper, this);
				d.TopNode = helper.lastNodeFetched;
				d.Show ();
			};

			helper.ElementCollectionChanged += delegate () {
				DomInspector d = new DomInspector(helper, this);
				d.ElementCollection = helper.ElementCollection;
				d.Show ();
			};
		}
		
		protected override void OnClosed (EventArgs e)
		{
			base.OnClosed (e);
			webHost.Shutdown ();
		}
		
		private void navigate (object sender, EventArgs e)
		{
			helper.lastNodeFetched = null;
			webHost.Navigation.Go (address.Text);
		}

		private void forward (object sender, EventArgs e)
		{
			helper.lastNodeFetched = null;
			webHost.Navigation.Forward ();
		}

		private void back (object sender, EventArgs e)
		{
			helper.lastNodeFetched = null;
			webHost.Navigation.Back ();
		}

		private void updateInspector ()
		{
			if (helper.document != null)
				domInspector.TopNode = helper.document;
		}
		
		private void openInspector (object sender, EventArgs e)
		{
			updateInspector ();
			if (!domInspector.Visible)
				domInspector.Show ();
		}

		private void loadWebHost () 
		{
			webHost = Manager.GetNewInstance ();
			loaded = webHost.Load (control.Handle, control.Width, control.Height);
			if (!loaded) return;
			
			webHost.Completed += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine ("webHost: Completed");
				helper.document = webHost.Document;
				updateInspector ();
			};
			/*
			webHost.Focus += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine ("webHost: Focus");
			};
			
			webHost.CreateNewWindow += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine ("webHost: CreateNewWindow");
			};
			
			webHost.KeyDown += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine ("webHost: KeyDown");
			};
			webHost.KeyUp += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine ("webHost: KeyUp");
			};
			webHost.KeyPress += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine ("webHost: KeyPress");
			};

			webHost.MouseDown += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine ("webHost: MouseDown");
			};
			webHost.MouseUp += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine ("webHost: MouseUp");
			};
			*/
			webHost.MouseClick += delegate (object sender, Mono.WebBrowser.DOM.NodeEventArgs e) {
				Console.Error.WriteLine ("webHost: MouseClick " + e.Node.GetHashCode());
			};
			/*
			webHost.MouseDoubleClick += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine ("webHost: MouseDoubleClick");
			};
			webHost.MouseEnter += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine ("webHost: MouseEnter");
			};
			webHost.MouseLeave += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine ("webHost: MouseLeave");
			};
			*/
			webHost.Generic += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine ("webHost: Generic" + sender);
			};
		}


		private void gui () 
		{
			
			this.StartPosition = FormStartPosition.CenterScreen;
			this.Size = new Size (650, 650);

			menu = new MenuStrip ();

			menu.Items.Add ("Dom Inspector", null, new EventHandler (openInspector));
			
			ToolStripMenuItem menu1 = null;
			ToolStripMenuItem menu2 = null;
			ToolStripMenuItem menu3 = null;
			ToolStripTextBox menutxt = null;
			
			menu1 = new ToolStripMenuItem ("Document");
			menu.Items.Add (menu1);
			
			menu2 = new ToolStripMenuItem ("Title");	
			menu1.DropDownItems.Add (menu2);
			
			menutxt = new ToolStripTextBox ();
			menu2.DropDownItems.Add (menutxt);

			menu3 = new ToolStripMenuItem ("Get", null, delegate(object sender, EventArgs e) {
				helper.getTitle ();
			});
			menu2.DropDownItems.Add (menu3);


			menu3 = new ToolStripMenuItem ("Set", null, delegate(object sender, EventArgs e) {
				helper.setTitle (((ToolStripTextBox)((ToolStripMenuItem)((ToolStripMenuItem)sender).OwnerItem).DropDownItems[0]).Text);
			});			
			menu2.DropDownItems.Add (menu3);
			
			menu2 = new ToolStripMenuItem ("Get DocumentElement", null, delegate (object sender, EventArgs e) {
				helper.getDocumentElement ();
			});
			menu1.DropDownItems.Add (menu2);

			menu2 = new ToolStripMenuItem ("Get Body", null, delegate (object sender, EventArgs e) {
				helper.getBody ();
			});
			menu1.DropDownItems.Add (menu2);

			menu2 = new ToolStripMenuItem ("Get Active Element", null, delegate (object sender, EventArgs e) {
				helper.getActiveElement ();
			});
			menu1.DropDownItems.Add (menu2);

			menu2 = new ToolStripMenuItem ("Get Element");
			menu1.DropDownItems.Add (menu2);

			menutxt = new ToolStripTextBox ();
			menu2.DropDownItems.Add (menutxt);
			
			menu3 = new ToolStripMenuItem ("By ID", null, delegate (object sender, EventArgs e) {
				helper.getElementById (((ToolStripTextBox)((ToolStripMenuItem)((ToolStripMenuItem)sender).OwnerItem).DropDownItems[0]).Text);
			});
			menu2.DropDownItems.Add (menu3);

			menu3 = new ToolStripMenuItem ("By Location", null, delegate (object sender, EventArgs e) {
				string s = ((ToolStripTextBox)((ToolStripMenuItem)((ToolStripMenuItem)sender).OwnerItem).DropDownItems[0]).Text;
				string[] point = s.Split (',');
				if (point.Length != 2) return;
				int x, y;
				if (!(int.TryParse(point[0], out x))) return;
				if (!(int.TryParse(point[1], out y))) return;
				helper.getElement (x, y);
			});
			menu2.DropDownItems.Add (menu3);

			menu2 = new ToolStripMenuItem ("Encoding");	
			menu1.DropDownItems.Add (menu2);
			
			menutxt = new ToolStripTextBox ();
			menu2.DropDownItems.Add (menutxt);

			menu3 = new ToolStripMenuItem ("Get", null, delegate(object sender, EventArgs e) {
				helper.getCharset ();
			});
			menu2.DropDownItems.Add (menu3);


			menu3 = new ToolStripMenuItem ("Set", null, delegate(object sender, EventArgs e) {
				helper.setCharset (((ToolStripTextBox)((ToolStripMenuItem)((ToolStripMenuItem)sender).OwnerItem).DropDownItems[0]).Text);
			});			
			menu2.DropDownItems.Add (menu3);


			menu2 = new ToolStripMenuItem ("Cookie");	
			menu1.DropDownItems.Add (menu2);
			
			menutxt = new ToolStripTextBox ();
			menu2.DropDownItems.Add (menutxt);

			menu3 = new ToolStripMenuItem ("Get", null, delegate(object sender, EventArgs e) {
				helper.getCookie ();
			});
			menu2.DropDownItems.Add (menu3);


			menu3 = new ToolStripMenuItem ("Set", null, delegate(object sender, EventArgs e) {
				helper.setCookie (((ToolStripTextBox)((ToolStripMenuItem)((ToolStripMenuItem)sender).OwnerItem).DropDownItems[0]).Text);
			});			
			menu2.DropDownItems.Add (menu3);


			menu2 = new ToolStripMenuItem ("Anchors", null, delegate (object sender, EventArgs e) {
				helper.getAnchors ();
			});
			menu1.DropDownItems.Add (menu2);

			menu2 = new ToolStripMenuItem ("Applets", null, delegate (object sender, EventArgs e) {
				helper.getApplets ();
			});
			menu1.DropDownItems.Add (menu2);

			menu2 = new ToolStripMenuItem ("Forms", null, delegate (object sender, EventArgs e) {
				helper.getForms ();
			});
			menu1.DropDownItems.Add (menu2);

			menu2 = new ToolStripMenuItem ("Images", null, delegate (object sender, EventArgs e) {
				helper.getImages ();
			});
			menu1.DropDownItems.Add (menu2);

			menu2 = new ToolStripMenuItem ("Links", null, delegate (object sender, EventArgs e) {
				helper.getLinks ();
			});
			menu1.DropDownItems.Add (menu2);

			menu2 = new ToolStripMenuItem ("Url", null, delegate (object sender, EventArgs e) {
				helper.getUrl ();
			});
			menu1.DropDownItems.Add (menu2);


			menu1 = new ToolStripMenuItem ("Element");
			menu.Items.Add (menu1);
			
			menu2 = new ToolStripMenuItem ("Attribute");	
			menu1.DropDownItems.Add (menu2);
			
			menutxt = new ToolStripTextBox ();
			menu2.DropDownItems.Add (menutxt);

			menu3 = new ToolStripMenuItem ("Has", null, delegate(object sender, EventArgs e) {
				helper.hasAttribute (((ToolStripTextBox)((ToolStripMenuItem)((ToolStripMenuItem)sender).OwnerItem).DropDownItems[0]).Text);
			});
			menu2.DropDownItems.Add (menu3);

			menu3 = new ToolStripMenuItem ("Get", null, delegate(object sender, EventArgs e) {
				helper.getAttribute (((ToolStripTextBox)((ToolStripMenuItem)((ToolStripMenuItem)sender).OwnerItem).DropDownItems[0]).Text);
			});			
			menu2.DropDownItems.Add (menu3);

			menu2 = new ToolStripMenuItem ("Show Children", null, delegate (object sender, EventArgs e) {
				helper.getChildren ();
			});
			menu1.DropDownItems.Add (menu2);


			menu2 = new ToolStripMenuItem ("FirstChild", null, delegate (object sender, EventArgs e) {
				helper.getFirstChild ();
			});
			menu1.DropDownItems.Add (menu2);

			menu2 = new ToolStripMenuItem ("InnerHTML", null, delegate (object sender, EventArgs e) {
				helper.getInnerHTML ();
			});
			menu1.DropDownItems.Add (menu2);

			menu2 = new ToolStripMenuItem ("InnerText", null, delegate (object sender, EventArgs e) {
				helper.getInnerText ();
			});
			menu1.DropDownItems.Add (menu2);

			menu2 = new ToolStripMenuItem ("LocalName", null, delegate (object sender, EventArgs e) {
				helper.getLocalName ();
			});
			menu1.DropDownItems.Add (menu2);

			menu2 = new ToolStripMenuItem ("Value", null, delegate (object sender, EventArgs e) {
				helper.getValue ();
			});
			menu1.DropDownItems.Add (menu2);

			this.Controls.Add (menu);

			int top = menu.Height;

			// first line
			cmdBack = new Button ();
			cmdBack.Text = "<";			
			cmdBack.Size = new Size (30, 30);
			cmdBack.Click += new EventHandler (this.back);

			cmdForward = new Button ();
			cmdForward.Text = ">";			
			cmdForward.Size = new Size (30, 30);
			cmdForward.Click += new EventHandler (this.forward);


			// second line
			lblBody = new Label ();
			lblBody.Text = "body";
			lblBody.Width = 60;
			
			body = new TextBox ();
			body.Multiline = true;
			body.Width = this.Width - lblBody.Width;
			body.Height = body.Height * 2;
			body.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);


			// third line
			lblAddress = new Label ();
			lblAddress.Text = "address";
			lblAddress.Width = 60;

			cmdNavigate = new Button ();
			cmdNavigate.Text = "Navigate";			
			cmdNavigate.Size = new Size (100, 30);
			cmdNavigate.Click += new EventHandler (this.navigate);

			address = new TextBox ();
			address.Width = this.Width - lblAddress.Width - cmdNavigate.Width;
			address.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Right);

			// fourth line
			control = new Control ();
			control.Size = this.ClientSize;
			this.Size = new Size (this.Width, this.Height - 150);
			control.Anchor = (AnchorStyles) (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right| AnchorStyles.Bottom);
			control.GotFocus += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine (" Control: GotFocus");
				Console.Error.WriteLine (Environment.StackTrace);
				webHost.FocusIn (FocusOption.FocusFirstElement);
			};
			control.LostFocus += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine (" Control: LostFocus");
				webHost.FocusOut ();
			};
			control.VisibleChanged += delegate (object sender, EventArgs e) {
				Console.Error.WriteLine (" Control: VisibleChanged");
				if (webHost != null && control.Visible && !control.Disposing && !control.IsDisposed && loaded) {
					webHost.Activate ();
				} else if (webHost != null && loaded && !control.Visible) {
					webHost.Deactivate ();
				}
			};
			control.MouseClick += delegate (object sender, MouseEventArgs e) {
				Console.Error.WriteLine (" Control: MouseClick");
			};

			// positioning
			
			// first line
			cmdBack.Location = new Point (0, top);
			cmdForward.Location = new Point (cmdBack.Right, top);

			// second line
			top += 30;
			lblBody.Location = new Point (0, top);
			body.Location = new Point (lblBody.Right, top);
			
			// third line
			top += 60;
			lblAddress.Location = new Point (0, top);
			cmdNavigate.Location = new Point (lblAddress.Right, top);
			address.Location = new Point (cmdNavigate.Right, top);

			// fourth line
			top += 30;
			control.Location = new Point (0, top);

			// add
			this.Controls.Add (control);			
			this.Controls.Add (cmdNavigate);
			this.Controls.Add (address);
			this.Controls.Add (lblAddress);
			this.Controls.Add (cmdBack);
			this.Controls.Add (cmdForward);
			this.Controls.Add (body);
			this.Controls.Add (lblBody);		

			domInspector = new DomInspector (helper, this);
			domInspector.Closing += delegate (object sender, CancelEventArgs e) {
				e.Cancel = true;
				((Form)sender).Hide();
			};
		}

		private Label lblBody;
		private TextBox body;

		private Button cmdNavigate;
		private Button cmdForward;
		private Button cmdBack;
		
		private Label lblAddress;
		private TextBox address;

		private Control control;
		IWebBrowser webHost;
		
		const int BUTTON_WIDTH = 80;
		private Page helper;
		
		private MenuStrip menu;

		private DomInspector domInspector;
		private bool loaded;
	}
}

//// THIS FILE AUTOMATICALLY GENERATED BY xpidl2cs.pl
//// EDITING IS PROBABLY UNWISE
//// Permission is hereby granted, free of charge, to any person obtaining
//// a copy of this software and associated documentation files (the
//// "Software"), to deal in the Software without restriction, including
//// without limitation the rights to use, copy, modify, merge, publish,
//// distribute, sublicense, and/or sell copies of the Software, and to
//// permit persons to whom the Software is furnished to do so, subject to
//// the following conditions:
//// 
//// The above copyright notice and this permission notice shall be
//// included in all copies or substantial portions of the Software.
//// 
//// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
//// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
//// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////
//// Copyright (c) 2008 Novell, Inc.
////
//// Authors:
////	Andreia Gaita (avidigal@novell.com)
////
//

using System;
using System.ComponentModel;

using Mono.WebBrowser.DOM;

namespace standalone
{
	
	public class Page : Component
	{
		public delegate void TextChangedHandler (string text);	
		public delegate void RootNodeChangedHandler ();
		public delegate void ElementCollectionChangedHandler ();
		
		public INode lastNodeFetched;
		public IDocument document;
		
		public INode NewRootNode {
			get {return lastNodeFetched;}
			set {
				lastNodeFetched = value;
				OnRootNodeChanged ();
			}
		}

		public IElementCollection elementCollection;
		public IElementCollection ElementCollection {
			get {return elementCollection;}
			set {
				elementCollection = value;
				OnElementCollectionChanged ();
			}
		}
		
		public string retVal;
		
		static object TextChangedEvent = new object ();
		public event TextChangedHandler TextChanged {
			add {Events.AddHandler (TextChangedEvent, value);}
			remove {Events.RemoveHandler (TextChangedEvent, value);}
		}
		
		
		public void OnTextChanged(string text) {
			TextChangedHandler eh = (TextChangedHandler)(Events[TextChangedEvent]);
			if (eh != null)
				eh (text);
		}
		
		static object RootNodeChangedEvent = new object ();
		public event RootNodeChangedHandler RootNodeChanged {
			add {Events.AddHandler (RootNodeChangedEvent, value);}
			remove {Events.RemoveHandler (RootNodeChangedEvent, value);}
		}

		public void OnRootNodeChanged() {
			RootNodeChangedHandler eh = (RootNodeChangedHandler)(Events[RootNodeChangedEvent]);
			if (eh != null)
				eh ();
		}		


		static object ElementCollectionChangedEvent = new object ();
		public event ElementCollectionChangedHandler ElementCollectionChanged {
			add {Events.AddHandler (ElementCollectionChangedEvent, value);}
			remove {Events.RemoveHandler (ElementCollectionChangedEvent, value);}
		}

		public void OnElementCollectionChanged() {
			ElementCollectionChangedHandler eh = (ElementCollectionChangedHandler)(Events[ElementCollectionChangedEvent]);
			if (eh != null)
				eh ();
		}		
		// document
		
		public void getDocumentElement () {
			if (document == null) return;
			lastNodeFetched = document.DocumentElement;
		}

		public void getTitle () {
			if (document == null) return;
			retVal = document.Title;
			OnTextChanged (retVal);			
		}
		
		public void setTitle (string text) {
			if (document == null) return;
			document.Title = text;
		}

		public void getBody () {
			if (document == null) return;
			lastNodeFetched = document.Body;
		}
		
		public void getActiveElement () {
			if (document == null) return;
			lastNodeFetched = document.Active;			
		}
		
		public void getElementById (string id) {
			if (document == null) return;
			lastNodeFetched = document.GetElementById (id);			
		}

		public void getElement (int x, int y) {
			if (document == null) return;
			lastNodeFetched = document.GetElement (x, y);
		}

		public void getCookie () {
			if (document == null) return;
			retVal = document.Cookie;
			OnTextChanged (retVal);
		}

		public void setCookie (string cookie) {
			if (document == null) return;
			document.Cookie = cookie;
		}


		public void getApplets () {
			if (document == null) return;
			ElementCollection = document.Applets;
		}
		
		public void getAnchors () {
			if (document == null) return;
			ElementCollection = document.Anchors;
		}

		public void getForms () {
			if (document == null) return;
			ElementCollection = document.Forms;
		}

		public void getImages () {
			if (document == null) return;
			ElementCollection = document.Images;
		}

		public void getLinks () {
			if (document == null) return;
			ElementCollection = document.Links;
		}
		
		public void getUrl () {
			if (document == null) return;
			retVal = document.Url;
			OnTextChanged (retVal);
		}
		
		public void getCharset () {
			if (document == null) return;
			retVal = document.Charset;
			OnTextChanged (retVal);
		}
		
		public void setCharset (string charset) {
			if (document == null) return;
			document.Charset = charset;
		}
		
		
		// Element
		
		
		public void getInnerText () {
			if (!(lastNodeFetched is IElement)) return;
			retVal = ((IElement)lastNodeFetched).InnerText;
			OnTextChanged (retVal);
		}
		
		public void getInnerHTML () {
			if (!(lastNodeFetched is IElement)) return;
			retVal = ((IElement)lastNodeFetched).InnerHTML;
			OnTextChanged (retVal);
		}
		
		public void hasAttribute (string name) {
			if (!(lastNodeFetched is IElement)) return;
			if (name.Equals (String.Empty)) return;
			retVal = ((IElement)lastNodeFetched).HasAttribute (name).ToString();			
			OnTextChanged (retVal);
		}

		public void getAttribute (string name) {
			if (!(lastNodeFetched is IElement)) return;
			if (name.Equals (String.Empty)) return;
			retVal = ((IElement)lastNodeFetched).GetAttribute (name);
			OnTextChanged (retVal);
		}

		public void getChildren () {
			if (!(lastNodeFetched is IElement)) return;
			OnRootNodeChanged ();			
		}

		// node

		public void getFirstChild () {
			if (!(lastNodeFetched is INode)) return;
			lastNodeFetched = lastNodeFetched.FirstChild;
		}


		public void getLocalName () {
			if (!(lastNodeFetched is INode)) return;
			retVal = lastNodeFetched.LocalName;
			OnTextChanged (retVal);
		}

		public void getValue () {
			if (!(lastNodeFetched is INode)) return;
			retVal = lastNodeFetched.Value;
			OnTextChanged (retVal);			
		}

		public void getType () {
			if (!(lastNodeFetched is INode)) return;
			retVal = lastNodeFetched.Type.ToString();
			OnTextChanged (retVal);			
		}		
		
		public void Click (object sender, EventArgs e) 
		{
			INode node = (INode) sender;
			retVal = node.LocalName + " Click";
			OnTextChanged (retVal);
		}
		
		public void DoubleClick (object sender, EventArgs e) 
		{
			INode node = (INode) sender;
			retVal = node.LocalName + " DoubleClick";
			OnTextChanged (retVal);
		}
		public void KeyDown (object sender, EventArgs e) 
		{
			INode node = (INode) sender;
			retVal = node.LocalName + " KeyDown";
			OnTextChanged (retVal);
		}
		public void KeyPress (object sender, EventArgs e) 
		{
			INode node = (INode) sender;
			retVal = node.LocalName + " KeyPress";
			OnTextChanged (retVal);
		}
		public void KeyUp (object sender, EventArgs e) 
		{
			INode node = (INode) sender;
			retVal = node.LocalName + " KeyUp";
			OnTextChanged (retVal);
		}
		public void MouseDown (object sender, EventArgs e) 
		{
			INode node = (INode) sender;
			retVal = node.LocalName + " MouseDown";
			OnTextChanged (retVal);
		}
		public void MouseEnter (object sender, EventArgs e) 
		{
			INode node = (INode) sender;
			retVal = node.LocalName + " MouseEnter";
			OnTextChanged (retVal);
		}
		public void MouseLeave (object sender, EventArgs e) 
		{
			INode node = (INode) sender;
			retVal = node.LocalName + " MouseLeave";
			OnTextChanged (retVal);
		}
		public void MouseMove (object sender, EventArgs e) 
		{
			INode node = (INode) sender;
			retVal = node.LocalName + " MouseMove";
			OnTextChanged (retVal);
		}
		public void MouseOver (object sender, EventArgs e) 
		{
			INode node = (INode) sender;
			retVal = node.LocalName + " MouseOver";
			OnTextChanged (retVal);
		}
		public void MouseUp (object sender, EventArgs e) 
		{
			INode node = (INode) sender;
			retVal = node.LocalName + " MouseUp";
			OnTextChanged (retVal);
		}
	}
}

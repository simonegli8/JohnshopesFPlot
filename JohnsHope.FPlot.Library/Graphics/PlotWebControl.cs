using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Permissions;

namespace JohnsHope.FPlot.Library.Web {
	/// <summary>
	/// The EventArgs of the Zoomed event. Contains a bool field that indicates if the PlotImage was zoomed in or out.
	/// </summary>
	public class ZoomedEventArgs: EventArgs {
		/// <summary>
		/// true if the PlotImage was zoomed in, false if it was zoomed out.
		/// </summary>
		public bool zoomedIn;
		/// <summary>
		/// The rectangle of device coordinates that was selected for zooming on the client. If you have disabled ViewState and still 
		/// want to implement client zooming facilities you must implement the zooming manually. You can call
		/// ((Plot2D)Plot).Zoom(zoomedIn, zoomRect) in the Zoomed event handler.
		/// </summary>
		public Rectangle zoomRect;
		/// <summary>
		/// The contructor.
		/// </summary>
		/// <param name="zoomedIn">true if zoomed in, false if zoomed out.</param>
		/// <param name="zoomRect">The rectangle in device coordinates, that was selected for zooming.</param>
		public ZoomedEventArgs(bool zoomedIn, Rectangle zoomRect) { this.zoomedIn = zoomedIn; this.zoomRect = zoomRect; }
	}
	/// <summary>
	/// The Zoomed event delegate.
	/// </summary>
	public delegate void ZoomedEventHandler(object sender, ZoomedEventArgs args);

	/// <summary>
	/// A WebControl derived from the WebControl Image that shows an image of a Plot.
	/// Here is an Example of how to include a Plot - Image on a ASP.NET Page:
	/// <code>
	/// &lt;%@ Page Language="C#" %&gt;
	/// &lt;%@ Import Namespace="JohnsHope.FPlot.Library" %&gt;
	/// &lt;%@ Register TagPrefix="fplot" Namespace="JohnsHope.FPlot.Library.Web" Assembly="JohnsHope.FPlot.Library" %&gt;
	/// 
	/// &lt;script runat="server"&gt;
	///   protected void Page_Load(object sender, EventArgs e) {
	///     Function1DItem f = new Function1DItem("return sin(x);");
	///     plot.Model.Add(f);
	///     plot.Model.FixXtoY = false;
	///     plot.Model.SetRange(-10, 10, -1.2, 1.2);
	///   }
	/// &lt;/script&gt;
	/// 	
	/// &lt;html&gt;
	/// &lt;body&gt;
	///   &lt;form id="form1" runat="server"&gt;
	///     &lt;fplot:PlotImage ID="plot" runat="server" TempPath="~/temp" Width="300px" Height="200px" EnableZooming="true" /&gt;
	///   &lt;/form&gt;
	/// &lt;/body&gt;
	/// &lt;/html&gt;
	/// </code>
	/// 
	/// In order for this WebControl to work, The ASP.NET account (Either ASP.NET or NETWORK SERVICE depending on the
	/// version of IIS you are using) must have full permissions on the <see cref="PlotImage.TempPath">TempPath</see>.
	/// If you use big data in your plot, it is recommended to turn off ViewState in the WebControl, since otherwise all
	/// the data of the WebControls Plot is send forth and back between the client browser and the webserver.
	/// You do this by setting the EnableViewState property of the WebControl to false. You can store the PlotImages Plot in the Session
	/// object instead.
	/// You can enable zooming capabilities on the client side by setting EnableZooming to true.
	/// The zooming does not work and is disabled on the Konqueror Browser.
	/// </summary>
	[DefaultProperty("TempPath"), ParseChildren(false),
		AspNetHostingPermission(SecurityAction.Demand, Level=AspNetHostingPermissionLevel.Minimal),
		AspNetHostingPermission(SecurityAction.InheritanceDemand, Level=AspNetHostingPermissionLevel.Minimal),
		ToolboxData("<{0}:PlotImage runat=\"server\" TempPath=\"~/temp\" />"),
		ToolboxBitmap(typeof(resfinder), "JohnsHope.FPlot.Library.Resources.Mandelbrot.ico")]
	public class PlotImage: System.Web.UI.WebControls.Image, IPostBackDataHandler, IPostBackEventHandler {

		private class Footprint {
			public DateTime date = DateTime.Now;
			public int ticks = Environment.TickCount;

			public override string ToString() {
				return date.Year.ToString() + "." + date.Month.ToString() + "." + date.Day.ToString() + "." +
					date.Hour.ToString() + "." + date.Minute.ToString() + "." + date.Second.ToString() + "." + ticks.ToString();
			}
		}
 
		Plot plot = null;
		string tempPath;
		bool enableZooming;
		string imageFileUrl;
		int scrollX = -1;
		int scrollY = -1;

		/// <summary>
		/// An event that occurs when the PlotImage was zoomed in or out on the client.
		/// </summary>
		public event ZoomedEventHandler Zoomed;
		ZoomedEventArgs zoomArgs = null;
		/// <summary>
		/// The default constructor.
		/// </summary>
		public PlotImage(): base() {
			imageFileUrl = null;
			// Debug.Write("\n\n");
			// if (Plot == null) Plot = new Plot2D(new PlotModel());
		}

		// Cleaning up of old files

		bool IsOlderThanOneHour(DateTime time) {
			DateTime now = DateTime.Now;
			TimeSpan diff = new TimeSpan(now.Hour - time.Hour, now.Minute - time.Minute,
				now.Second - time.Second);

			return now.Date > time.Date || diff > new TimeSpan(1, 0, 0);
		}

		void CleanupTempPath() { // delete old files
			DateTime cleanup = new DateTime();
			bool cleanupIsNull;

			Page.Application.Lock();
			cleanupIsNull = Page.Application["JohnsHope.PlotImage.Cleanup"] == null;
			if (!cleanupIsNull) cleanup = (DateTime)Page.Application["JohnsHope.PlotImage.Cleanup"];
			Page.Application.UnLock();

			if (!DesignMode && !string.IsNullOrEmpty(TempPath) && Page != null && Page.Server != null &&
				(cleanupIsNull || IsOlderThanOneHour(cleanup))) {

				Page.Application.Lock();
				Page.Application["JohnsHope.PlotImage.Cleanup"] = DateTime.Now;
				Page.Application.UnLock();

				string path = Page.Server.MapPath(TempPath);
				DirectoryInfo dir = new DirectoryInfo(path);
				FileInfo[] files = dir.GetFiles("*.png", SearchOption.TopDirectoryOnly);
				foreach (FileInfo file in files) {
					if (IsOlderThanOneHour(file.CreationTime)) { // file is older than 1 hour
						try {
							File.Delete(file.FullName);
						} catch { }
					}
				}
			}
		}
		/// <summary>
		/// Removes old (older than 1 hour) files from the TempPath.
		/// </summary>
		public override void Dispose() {
			CleanupTempPath();
			base.Dispose();
		}

		/// <summary>
		/// If you use big data in your plot, it is recommended to turn off ViewState in the WebControl, since otherwise all
		/// the data of the WebControls Plot is send forth and back between the client browser and the webserver.
		/// You do this by setting the EnableViewState property to false.
		/// </summary>
		[Browsable(true), Category("Behavior"), Description("If set to false, the PlotImage uses no ViewState information.\n" +
			"Normally the whole Plot is stored in the ViewState, but if you use big data objects\n" + 
			"in your Plot you should turn off ViewState. If you turn off ViewState you must\n" + 
			"reinitialize the Plot on every Page request for example by storing the Plot in the Session object."),
			DefaultValue(true)]
		public override bool EnableViewState {
			get { return base.EnableViewState; }
			set { base.EnableViewState = value; }
		}
		/// <summary>
		/// By setting this property to true you can enable zooming capabilities on the client side. This only works for 2D Plots.
		/// The zooming does not work and is disabled on the Konqueror Browser. If you disable ViewState and still want to use client zooming
		/// you must implement zooming by hand. You can do this by calling ((Plot2D)Plot).Zoom(zoomedIn, zoomRect) in the Zoomed event handler.
		/// </summary>
		[Browsable(true), Category("Behavior"), Description("If set to true and the plot is a 2D plot, the control implements zooming facilities " +
			"using client side javascript."), DefaultValue(false)]
		public bool EnableZooming {
			get {
				if (EnableViewState) {
					object vs = ViewState["JohnsHope.PlotImage.EnableZooming"];
					enableZooming = vs != null ? (bool)vs : false;
				}
				if (!DesignMode && Page != null && Page.Request != null) {
					HttpBrowserCapabilities caps = Context.Request.Browser;
					enableZooming = enableZooming && caps.W3CDomVersion.Major >= 1 && caps.EcmaScriptVersion >= new Version(1, 2);
				}
				return enableZooming;
			}
			set {
				enableZooming = value;
				if (EnableViewState) ViewState["JohnsHope.PlotImage.EnableZooming"] = enableZooming;
			}
		}
		/// <summary>
		/// The path where temporary files are stored. The ASP.NET Application must have write access to this path.
		/// </summary>
		[Browsable(true), Category("Misc"),
			Description("Specifies a path where temporary files are stored.\n" +
				"The ASP.NET Application must have write access to this path."),
			DefaultValue("~/temp")]
		public string TempPath {
			get {
				if (EnableViewState) {
					string s = ViewState["JohnsHope.PlotImage.TempPath"] as string;
					tempPath = (s == null)? string.Empty : s;
				}
				if (string.IsNullOrEmpty(tempPath)) throw new ArgumentException("PlotImage.TempPath: You must specify " +
					"a valid path for temporary files.");
				return tempPath;
			}
			set {
				if (EnableViewState) ViewState["JohnsHope.PlotImage.TempPath"] = value;
				tempPath = value;
				if (!DesignMode && Page != null && Page.Server != null) Compiler.TempPath = Page.Server.MapPath(value);
				if (string.IsNullOrEmpty(value)) throw new ArgumentException("PlotImage.TempPath: You must specify " +
					"a valid path for temporary files.");
			}
		}
		/// <summary>
		/// The <see cref="Plot">Plot</see> object to view in the WebControl
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Plot Plot {
			get {
				if (EnableViewState) plot = ViewState["JohnsHope.PlotImage.Plot"] as Plot;
				if (plot == null) {
					Plot = new Plot2D(new PlotModel());
				}
				return plot;
			}
			set {
				if (EnableViewState) ViewState["JohnsHope.PlotImage.Plot"] = value;
				plot = value;
			}
		}
		/// <summary>
		/// Gets or sets the Model of the property Plot. Same as Plot.Model.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public PlotModel Model {
			get { return Plot != null ? Plot.Model : null;  }
			set {
				if (Plot != null) Plot.Model = value;
				else Plot = Plot.New(value);
			}
		}

		string ImageFileUrl() {
			if (!DesignMode && Page != null && imageFileUrl == null) {
				Page.Application.Lock();
				if (Page.Application["JohnsHope.PlotImage.Footprint"] == null) {
					Page.Application["JohnsHope.PlotImage.Footprint"] = new Footprint();
					Page.Application["JohnsHope.PlotImage.FileIndex"] = 0;
				}
				Footprint fp = (Footprint)Page.Application["JohnsHope.PlotImage.Footprint"];
				int index = (int)Page.Application["JohnsHope.PlotImage.FileIndex"];
				index++;
				Page.Application["JohnsHope.PlotImage.FileIndex"] = index;
				Page.Application.UnLock();
				imageFileUrl = TempPath + "/image" + fp.ToString() + "." + index.ToString() + ".png";
			} else if (imageFileUrl == null) {
				imageFileUrl = "";
			}
			return imageFileUrl;
		}

		string ImageFilePath() {
			if (Page != null && Page.Server != null) return Page.Server.MapPath(ImageFileUrl());
			else return "";
		}

		bool IsOldFile() {
			FileInfo file = new FileInfo(ImageFilePath());
			if (!file.Exists) return true;
			DateTime time = file.CreationTime;
			DateTime now = DateTime.Now;
			TimeSpan diff = new TimeSpan(now.Hour - time.Hour, now.Minute - time.Minute,
				now.Second - time.Second);

			return now.Date > time.Date || diff > new TimeSpan(0, 55, 0);
		}

		void SetImageUrl() {
			ImageUrl = ImageFileUrl();
		}

		int GetPixels(Unit size) {
			switch (size.Type) {
				case UnitType.Pixel: return (int)(size.Value + 0.5);
				case UnitType.Inch: return (int)(size.Value*96 + 0.5);
				case UnitType.Mm: return (int)(size.Value/25.4*96 + 0.5);
				case UnitType.Cm: return (int)(size.Value/2.54*96 + 0.5);
				case UnitType.Point: return (int)(size.Value/72.0*96 + 0.5);
				case UnitType.Ex:
				case UnitType.Em:
				case UnitType.Pica: return (int)(size.Value/72.0*12*96 + 0.5);
				case UnitType.Percentage: return 0;
				default: return 0;
			}
		}
		/// <summary>
		/// Sets or gets the Width of the WebControl. Percentage width's are not supported, use pixels instead.
		/// </summary>
		[Browsable(true), Category("Layout"), Description("Specifies the width of the Control. This value is always converted to pixels. " +
			"Percentage values are not supported.")]
		public override Unit Width {
			get { return base.Width; }
			set {
				if (value.Type == UnitType.Percentage) throw new NotSupportedException("PlotImage.Width: Percentage width not supported.");
				base.Width = new Unit(GetPixels(value));
			}
		}
		/// <summary>
		/// Sets or gets the Height of the WebControl. Percentage Height's are not supported, use pixels instead.
		/// </summary>
		[Browsable(true), Category("Layout"), Description("Specifies the height of the Control. This value is always converted to pixels. " +
			"Percentage values are not supported.")]
		public override Unit Height {
			get { return base.Height; }
			set {
				if (value.Type == UnitType.Percentage) throw new NotSupportedException("PlotImage.Height: Percentage height not supported.");
				base.Height = new Unit(GetPixels(value));
			}
		}

		/// <summary>
		/// Sets or gets the Width and Height of the image in pixels.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Rectangle Bounds {
			get { return new Rectangle(0, 0, GetPixels(Width), GetPixels(Height)); }
			set {
				base.Width = new Unit(value.Width);
				base.Height = new Unit(value.Height);
			}
		}

		bool Modified = true;

		void SaveToImage(Plot plot, string path, Rectangle bounds) {
			Rectangle bounds0 = new Rectangle(0, 0, bounds.Width, bounds.Height);
			Bitmap bmp = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format24bppRgb);
			plot.SynchDraw = true;
			Graphics g = Graphics.FromImage(bmp);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			plot.Draw(g, bounds0);
			try {
				bmp.Save(path, ImageFormat.Png);
			} catch (Exception) {
				throw new Exception("Invalid TempPath: " + TempPath);
			}
			bmp.Dispose();
			g.Dispose();
		}

		void SavePlot() {
			if (Plot != null && Plot.Model != null) {
				Modified = Plot.Model.Modified;
				if (Visible && (Modified || IsOldFile())) { // draw image
					Rectangle bounds = Bounds;
					if (Plot != null && Plot.Model != null) {
						if (bounds.Width <= 0 || bounds.Height <= 0) throw new ArgumentException("PlotImage: You must set a valid Image Width and Height");
						SaveToImage(Plot, ImageFilePath(), bounds);
						SetImageUrl();
					}
				}
				Plot.Model.Modified = false;
			}
		}

		/// <summary>
		/// Saves the PlotImages ViewState.
		/// </summary>
		protected override object SaveViewState() {
			SavePlot();
			return base.SaveViewState();
		}
		/// <summary>
		/// Loads PostData from a zooming operation
		/// </summary>
		/// <param name="key">The name of the first input field</param>
		/// <param name="form">The form values</param>
		public virtual bool LoadPostData(string key, NameValueCollection form) {
			string zoom = form[key];
			int x = -1, y = -1, w = -1, h = -1;
			scrollX = -1;
			scrollY = -1;
			if (zoom != "-1") {
				int.TryParse(form[key + "_x"], out x);
				int.TryParse(form[key + "_y"], out y);
				int.TryParse(form[key + "_w"], out w);
				int.TryParse(form[key + "_h"], out h);
				int.TryParse(form[key + "_scrollX"], out scrollX);
				int.TryParse(form[key + "_scrollY"], out scrollY);

				zoomArgs = new ZoomedEventArgs(zoom == "0", new Rectangle(x, y, w, h));
				if (EnableViewState && Plot != null && Plot is Plot2D) ((Plot2D)Plot).Zoom(zoomArgs.zoomedIn, zoomArgs.zoomRect);

				return true;
			}
			return false;
		}
		/// <summary>
		/// Is called when the PostBackEvent occurs.
		/// </summary>
		/// <param name="eventArgument">Is always String.Empty</param>
		public void RaisePostBackEvent(string eventArgument) { }

		/// <summary>
		/// Raises the PostDataChanged event.
		/// </summary>
		public virtual void RaisePostDataChangedEvent() {
			OnZoomed(zoomArgs);
			zoomArgs = null;
		}
		/// <summary>
		/// Raises the Zoomed event.
		/// </summary>
		public virtual void OnZoomed(ZoomedEventArgs zoomArgs) {
			if (Zoomed != null) Zoomed(this, zoomArgs);
		}

		private const string Script0 = 
@"
<script type='text/javascript' language='javascript'>
<!--
function JohnsHope_FPlot_GetNext(item, type) {
	while (item && item.nodeName != type) item = item.nextSibling;
	return item;
}

function JohnsHope_FPlot_Items_Class(img) {
	this.x = 0;
	this.y = 0;
	this.w = 0;
	this.h = 0;
	this.zx = 0;
	this.zy = 0;
	this.zw = 0;
	this.zh = 0;
	this.imgx = 0;
	this.imgy = 0;
	this.ScrollX = 0;
	this.ScrollY = 0;
	this.containerx = 0;
	this.containery = 0;
	this.dragging = false;
	this.onmouseup = null;
	this.min = 20;

	this.parent = img.parentNode;
	this.img = img;
	this.div0 = JohnsHope_FPlot_GetNext(this.parent.firstChild, 'SPAN');
	this.div1 = JohnsHope_FPlot_GetNext(this.div0.nextSibling, 'SPAN');
	this.izoom = JohnsHope_FPlot_GetNext(this.div1.nextSibling, 'INPUT');
	this.ix = JohnsHope_FPlot_GetNext(this.izoom.nextSibling, 'INPUT');
	this.iy = JohnsHope_FPlot_GetNext(this.ix.nextSibling, 'INPUT');
	this.iw = JohnsHope_FPlot_GetNext(this.iy.nextSibling, 'INPUT');
	this.ih = JohnsHope_FPlot_GetNext(this.iw.nextSibling, 'INPUT');
	this.ipx = JohnsHope_FPlot_GetNext(this.ih.nextSibling, 'INPUT');
	this.ipy = JohnsHope_FPlot_GetNext(this.ipx.nextSibling, 'INPUT');
	this.ipw = JohnsHope_FPlot_GetNext(this.ipy.nextSibling, 'INPUT');
	this.iph = JohnsHope_FPlot_GetNext(this.ipw.nextSibling, 'INPUT');
	this.ifix = JohnsHope_FPlot_GetNext(this.iph.nextSibling, 'INPUT');
	this.iscrollx = JohnsHope_FPlot_GetNext(this.ifix.nextSibling, 'INPUT');
	this.iscrolly = JohnsHope_FPlot_GetNext(this.iscrollx.nextSibling, 'INPUT');
	this.px = parseInt(this.ipx.value);
	this.py = parseInt(this.ipy.value);
	this.pw = parseInt(this.ipw.value);
	this.ph = parseInt(this.iph.value);
	this.fix = this.ifix.value;
	this.zoom = -1;
}

var JohnsHope_FPlot_Images = new Array();
var JohnsHope_FPlot_Items = new Array();
var JohnsHope_FPlot_LastItem = null;

function JohnsHope_FPlot_Sender(evt) {
	if (!evt) evt = event;
	var targ;
	if (evt) {
		if (evt.currentTarget) targ = evt.currentTarget;
		else if (evt.target) targ = evt.target;
		else if (evt.srcElement) targ = evt.srcElement;
		if (targ.nodeType == 3) // defeat Safari bug
			targ = targ.parentNode;
		return targ;
	} else return null;
}

function JohnsHope_FPlot_Register(evt) {
	if (!evt) evt = event;
	var img = JohnsHope_FPlot_Sender(evt);
	if (!img) {
		return JohnsHope_FPlot_LastItem;
	}
	if (img.nodeName != 'IMG') {
		img = img.firstChild;
		while (img && img.nodeName != 'IMG') img = img.nextSibling;
	}
	if (img && img.nodeName == 'IMG') {
		var item = new JohnsHope_FPlot_Items_Class(img);
		JohnsHope_FPlot_Items.push(item);
		JohnsHope_FPlot_Images.push(img);
		JohnsHope_FPlot_ScrollTo(item);
		return item;
	}
	return null;
}

function JohnsHope_FPlot_Item(evt) {
	if (!evt) evt = event;
	var img = JohnsHope_FPlot_Sender(evt);
	if (!img || img == document) {
		return JohnsHope_FPlot_LastItem;
	}
	if (img.nodeName != 'IMG') {
		img = img.firstChild;
		while (img && img.nodeName != 'IMG') img = img.nextSibling;
	}
	if (img) {
		var i = 0;
		while (i < JohnsHope_FPlot_Images.length && JohnsHope_FPlot_Images[i] != img) i++;
		if (i < JohnsHope_FPlot_Images.length) JohnsHope_FPlot_LastItem = JohnsHope_FPlot_Items[i];
	}
	return JohnsHope_FPlot_LastItem;
}

function JohnsHope_FPlot_Load(evt) {
	if (!evt) evt = event;
	var item = JohnsHope_FPlot_Register(evt);
	if (item) {
		item.img.onmousedown = JohnsHope_FPlot_DragStart;
		item.img.ondragstart = JohnsHope_FPlot_DragAndDropStop;
	}
}

function JohnsHope_FPlot_DragAndDropStop(evt) {
	if (!evt) evt = event;
	evt.returnValue = false;
}
	
function JohnsHope_FPlot_SetRect(item) {
	var x = item.x - item.imgx, y = item.y - item.imgy, w = item.w, h = item.h;
	var min = item.min;
	if (-min <= w && w <= min) {
		w = (w >= 0) ? min : -min;
	}
	if (-min <= h && h <= min) {
		h = (h >= 0) ? min : -min;
	}
	if (w < 0) { item.zx = x + w; item.zw = -w; }
	else { item.zx = x; item.zw = w; }
	if (h < 0) { item.zy = y + h; item.zh = -h; }
	else { item.zy = y; item.zh = h; }
	// clip to item.p
	if (item.zx < item.px) {
		item.zw -= item.px - item.zx;
		item.zx = item.px;
	}
	if (item.zy < item.py) {
		item.zh -= item.py - item.zy;
		item.zy = item.py;
	}
	if (item.zx + item.zw > item.px + item.pw) item.zw = item.px + item.pw - item.zx;
	if (item.zy + item.zh > item.py + item.ph) item.zh = item.py + item.ph - item.zy; 

	if (item.fix == 'x') {
		item.zx = item.px;
		item.zw = item.pw;
	} else if (item.fix == 'y') {
		item.zy = item.py;
		item.zh = item.ph;
	}

	x = item.zx + item.imgx - item.containerx;
	y = item.zy + item.imgy - item.containery;
	w = item.zw;
	h = item.zh;
	
	if (item.zw > 1 && item.zh > 1) {
		item.div0.style.visibility = 'visible';
		item.div0.style.top = y  + 'px';
		item.div0.style.left = x + 'px';
		item.div0.style.width = w + 'px';
		item.div0.style.height = h + 'px';
		item.div1.style.visibility = 'visible';
		item.div1.style.top = y + 'px';
		item.div1.style.left = x + 'px';
		item.div1.style.width = w + 'px';
		item.div1.style.height = h + 'px';	
	} else {
		item.div0.style.visibility = 'hidden';
		item.div1.style.visibility = 'hidden';
	}
}

function JohnsHope_FPlot_FindPos(obj) {
	var curleft = curtop = 0;
	if (obj.offsetParent) {
		curleft = obj.offsetLeft;
		curtop = obj.offsetTop;
		while (obj = obj.offsetParent) {
			curleft += obj.offsetLeft;
			curtop += obj.offsetTop;
		}
	} else {
		if (obj.x) curleft = obj.x; 
		if (obj.y) curtop = obj.y;
	}
	if (navigator.userAgent.indexOf('Mac') != -1 && typeof document.body.leftMargin != 'undefined') {
		curleft += document.body.leftMargin;
		curtop += document.body.topMargin;
	}
	return [curleft,curtop];
}


function JohnsHope_FPlot_FindRelPos(item) {
	var obj = item.parent;
	while (obj != null && (!obj.style || obj.style.position == null || obj.style.position == '' ||
		obj.style.position == 'static')) obj = obj.parentNode;
	if (obj != null) {
		var pos = JohnsHope_FPlot_FindPos(obj);
		item.containerx = pos[0]; item.containery = pos[1];
	}
}

function JohnsHope_FPlot_DragStart(evt) {
	if (!evt) evt = event;
	if (evt && evt.preventDefault) evt.preventDefault();
	var item = JohnsHope_FPlot_Item(evt);
	var pos = JohnsHope_FPlot_FindPos(item.img);
	JohnsHope_FPlot_FindRelPos(item);
	item.imgx = pos[0]; item.imgy = pos[1];
	if (evt.pageX) {
		item.x = evt.pageX;
		item.y = evt.pageY;
	} else if (evt.offsetX && evt.clientX) {
		item.ScrollX = item.imgx + evt.offsetX - evt.clientX + 2;
		item.ScrollY = item.imgy + evt.offsetY - evt.clientY + 2;
		item.x = evt.clientX + item.ScrollX;
		item.y = evt.clientY + item.ScrollY;
	} else return;
	item.w = 0;
	item.h = 0;
	item.parent.onmousemove = JohnsHope_FPlot_DragMove;
	item.img.onmousemove = null;
	item.onmouseup = document.onmouseup;
	document.onmouseup = JohnsHope_FPlot_DragStop;
	if (evt.shiftKey) item.zoom = 1;
	else item.zoom = 0;
	item.dragging = true;
	return true;
}

function JohnsHope_FPlot_DragMove(evt) {
	if (!evt) evt = event;
	var item = JohnsHope_FPlot_Item(evt);
	var s = JohnsHope_FPlot_Sender(evt);
	
	if (evt.pageX) {
		item.w = evt.pageX - item.x;
		item.h = evt.pageY - item.y;
	} else if (evt.clientX) {
		item.w = evt.clientX + item.ScrollX - item.x;
		item.h = evt.clientY + item.ScrollY - item.y;
	}

	JohnsHope_FPlot_SetRect(item);
	return false;
}

function JohnsHope_FPlot_DragStop(evt) {
	if (!evt) evt = event;
	var item = JohnsHope_FPlot_Item(evt);
	JohnsHope_FPlot_DragEnd(evt, item);
	if (evt.shiftKey) item.zoom = 1;
	if (item.zw >= item.min && item.zh >= item.min) {
		item.ix.value = item.zx;
		item.iy.value = item.zy;
		item.iw.value = item.zw;
		item.ih.value = item.zh;
		item.izoom.value = item.zoom;
		JohnsHope_FPlot_SaveScrollPos(item);
		//postback
";
		private const string Script1 = 
@"
	}
}

function JohnsHope_FPlot_DragEnd(evt, item) {
	if (!evt) evt = event;
	if (!item) item = JohnsHope_FPlot_Item(evt);
	item.parent.onmousemove = null;
	document.onmouseup = item.onmouseup;
	item.div0.style.visibility = 'hidden';
	item.div1.style.visibility = 'hidden';
	item.dragging = false;
}

function JohnsHope_FPlot_DragCancel(evt) {
	if (!evt) evt = event;
	var item = JohnsHope_FPlot_Item(evt);
	if (item) JohnsHope_FPlot_DragEnd(evt, item);
}

function JohnsHope_FPlot_SaveScrollPos(item) {
	var scrollX, scrollY;
  
	if (document.all) {
		if (!document.documentElement.scrollLeft) scrollX = document.body.scrollLeft;
		else scrollX = document.documentElement.scrollLeft;
               
		if (!document.documentElement.scrollTop) scrollY = document.body.scrollTop;
		else scrollY = document.documentElement.scrollTop;
  } else {
		scrollX = window.pageXOffset;
		scrollY = window.pageYOffset;
	}
  
	item.iscrollx.value = scrollX;
	item.iscrolly.value = scrollY;
}
   
function JohnsHope_FPlot_ScrollTo(item) {
	if (item.iscrollx.value != -1 && item.iscrolly.value != -1) {
    window.scrollTo(item.iscrollx.value, item.iscrolly.value);
  }
}

//-->
</script>
";
		/// <summary>
		/// Registers the client script if client side zooming is turned on.
		/// </summary>
		protected override void OnPreRender(EventArgs e) {
			if (!EnableViewState) SavePlot();
			if (!DesignMode && Visible && EnableZooming && Plot != null && Plot is Plot2D) {
				Page.ClientScript.RegisterClientScriptBlock(GetType(), "script 0", Script0, false);
				Page.ClientScript.RegisterClientScriptBlock(GetType(), "postback", Page.ClientScript.GetPostBackEventReference(this, ""), false);
				Page.ClientScript.RegisterClientScriptBlock(GetType(), "script 1", Script1, false);
				Page.RegisterRequiresPostBack(this);
			}
			base.OnPreRender(e);
		}
		/// <summary>
		/// Renders the WebControl.
		/// </summary>
		protected override void Render(HtmlTextWriter writer) {
			if (!DesignMode && Visible) {

				if (EnableZooming && Plot != null && Plot is Plot2D && !(Model.x.fix && Model.y.fix)) {
					writer.Write(@"<span>
						<span style='visibility:hidden; position:absolute; cursor:crosshair; font-size:0pt; background-color:transparent; border: solid 1px white;'></span>
						<span style='visibility:hidden; position:absolute; cursor:crosshair; font-size:0pt; background-color:transparent; border: dashed 1px black;'></span>");

					writer.Write("<input name=\""); // zoom input field
					writer.Write(this.UniqueID);
					writer.WriteLine("\" type=\"hidden\" value=\"-1\"/>");

					writer.Write("<input name=\""); // x input field
					writer.Write(this.UniqueID);
					writer.WriteLine("_x\" type=\"hidden\" value=\"-1\"/>");

					writer.Write("<input name=\""); // y input field
					writer.Write(this.UniqueID);
					writer.WriteLine("_y\" type=\"hidden\" value=\"-1\"/>");

					writer.Write("<input name=\""); // w input field
					writer.Write(this.UniqueID);
					writer.WriteLine("_w\" type=\"hidden\" value=\"0\"/>");

					writer.Write("<input name=\""); // h input field
					writer.Write(this.UniqueID);
					writer.WriteLine("_h\" type=\"hidden\" value=\"0\"/>");

					Rectangle bounds = Plot.Bounds;

					writer.Write("<input name=\""); // px input field
					writer.Write(this.UniqueID);
					writer.Write("_px\" type=\"hidden\" value=\"");
					writer.Write(bounds.X);
					writer.WriteLine("\"/>");

					writer.Write("<input name=\""); // py input field
					writer.Write(this.UniqueID);
					writer.Write("_py\" type=\"hidden\" value=\"");
					writer.Write(bounds.Y);
					writer.WriteLine("\"/>");

					writer.Write("<input name=\""); // pw input field
					writer.Write(this.UniqueID);
					writer.Write("_pw\" type=\"hidden\" value=\"");
					writer.Write(bounds.Width);
					writer.WriteLine("\"/>");

					writer.Write("<input name=\""); // ph input field
					writer.Write(this.UniqueID);
					writer.Write("_ph\" type=\"hidden\" value=\"");
					writer.Write(bounds.Height);
					writer.WriteLine("\"/>");

					string fix = "";
					if (Model.x.fix) fix = "x";
					if (Model.y.fix) fix = "y";
					writer.Write("<input name=\""); // fix input field
					writer.Write(this.UniqueID);
					writer.Write("_fix\" type=\"hidden\" value=\"");
					writer.Write(fix);
					writer.WriteLine("\"/>");

					writer.Write("<input name=\""); // scrollX input field
					writer.Write(this.UniqueID);
					writer.Write("_scrollX\" type=\"hidden\" value=\"");
					writer.Write(scrollX);
					writer.WriteLine("\"/>");

					writer.Write("<input name=\""); // scrollY input field
					writer.Write(this.UniqueID);
					writer.Write("_scrollY\" type=\"hidden\" value=\"");
					writer.Write(scrollY);
					writer.WriteLine("\"/>");

					this.Style.Add(HtmlTextWriterStyle.ZIndex, "0");
					this.Style.Add(HtmlTextWriterStyle.Cursor, "crosshair");
					this.Attributes.Add("onload", "JohnsHope_FPlot_Load(event)");
					base.Render(writer);
					writer.WriteLine("</span>");
				} else base.Render(writer);
			} else base.Render(writer);
		}

	}
}

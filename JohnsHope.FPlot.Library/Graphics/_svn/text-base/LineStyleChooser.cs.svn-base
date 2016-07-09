using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace JohnsHope.FPlot.Library {
	//combo box show too many lines IF designer has added new items to Items list

	/// <summary>
	/// A ComboBox used to select <see cref="DashStyle"/>s.
	/// </summary>
	[ToolboxBitmap(typeof(resfinder), "JohnsHope.FPlot.Library.Resources.LineStyle.ico")]
	public class LineStyleChooser: ComboBox {

		private Pen pen = new Pen(Color.Black, 3);

		const int N = 5; // number of styles

		private DashStyle Style(int index) {
			switch (index) {
			case 0: return DashStyle.Solid;
			case 1: return DashStyle.Dash;
			case 2: return DashStyle.Dot;
			case 3: return DashStyle.DashDot;
			case 4: return DashStyle.DashDotDot;
			default: Debug.Fail("wrong dash style index in LineStyleChooser.Style");
				return DashStyle.Solid;
			}
		}

		/// <summary>
		/// The linewidth of the lines in the ComboBox.
		/// </summary>
		public float LineWidth = 3;

		/// <summary>
		/// The selected <see cref="DashStyle"/>.
		/// </summary>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DashStyle SelectedStyle {
			get {	return Style(SelectedIndex); }
			set {	SelectedIndex = (int)value - (int)DashStyle.Solid; }
		}

		/// <summary>
		/// The default constructor.
		/// </summary>
		public LineStyleChooser() {
			Reset();
			this.ParentChanged += HandleParentChanged;
		}

		private void HandleParentChanged(object sender, EventArgs e) {
			Reset();
		}

		/// <summary>
		/// Resets the LineStyleChooser
		/// </summary>
		public void Reset() {
			DropDownStyle = ComboBoxStyle.DropDownList;
			DrawMode = DrawMode.OwnerDrawFixed;
			DrawItem -= DrawLineItem;
			DrawItem += DrawLineItem;
			base.Items.Clear();
			for (int i = 0; i < N; i++) {
				base.Items.Add(Style(i).ToString());
			}
		}
		
		private void DrawLineItem(object sender, DrawItemEventArgs e) {
			e.DrawBackground();
			pen.Color = ForeColor;
			pen.Width = LineWidth;
			Graphics g = e.Graphics;
			pen.DashStyle = Style(e.Index);

			g.DrawLine(pen, e.Bounds.X, e.Bounds.Y + e.Bounds.Height/2, e.Bounds.X + e.Bounds.Width,
				e.Bounds.Y + e.Bounds.Height/2);
			e.DrawFocusRectangle();
		}

	}
}

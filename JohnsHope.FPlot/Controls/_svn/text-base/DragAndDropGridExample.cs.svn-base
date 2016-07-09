/*
		private Rectangle dragBoxFromMouseDown;
		private int rowIndexFromMouseDown;
		private int rowIndexOfItemUnderMouseToDrop;

		private void HandleMouseMove(object sender, MouseEventArgs e) {
			if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
				// If the mouse moves outside the rectangle, start the drag.
				if (dragBoxFromMouseDown != Rectangle.Empty &&
        !dragBoxFromMouseDown.Contains(e.X, e.Y)) {
					// Proceed with the drag and drop, passing in the list item. 
					DragDropEffects dropEffect = DoDragDrop(Rows[rowIndexFromMouseDown], DragDropEffects.Move);
				}
			}
		}

		private void HandleMouseDown(object sender, MouseEventArgs e) {
			// Get the index of the item the mouse is below.
			rowIndexFromMouseDown = HitTest(e.X, e.Y).RowIndex;

			if (rowIndexFromMouseDown != -1) {
				// Remember the point where the mouse down occurred. 
				// The DragSize indicates the size that the mouse can move 
				// before a drag event should be started. 
				Size dragSize = SystemInformation.DragSize;

				// Create a rectangle using the DragSize, with the mouse position being
				// at the center of the rectangle.
				dragBoxFromMouseDown = new Rectangle(
					new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
			} else {
				// Reset the rectangle if the mouse is not over an item in the ListBox.
				dragBoxFromMouseDown = Rectangle.Empty;
			}
		}

		private void HandleDragDrop(object sender, DragEventArgs e) {
			// The mouse locations are relative to the screen, so they must be 
			// converted to client coordinates.
			Point clientPoint = PointToClient(new Point(e.X, e.Y));

			// Get the row index of the item the mouse is below. 
			rowIndexOfItemUnderMouseToDrop = HitTest(clientPoint.X, clientPoint.Y).RowIndex;

			// If the drag operation was a move then remove and insert the row.
			if (e.Effect == DragDropEffects.Move) {
				DataGridViewRow rowToMove = e.Data.GetData(
					typeof(DataGridViewRow)) as DataGridViewRow;
				Rows.RemoveAt(rowIndexFromMouseDown);
				Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);
			}
		}

		private void HandleDragOver(object sender, DragEventArgs e) {
			e.Effect = DragDropEffects.Move;
		}
*/
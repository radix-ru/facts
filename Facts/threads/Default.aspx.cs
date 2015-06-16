using System;
using System.Web.UI.HtmlControls;

namespace Facts {
	public partial class ThreadsPage: BasePage {
		private enum TableColumn: uint {
			Id,
			Title,
			MessageCount,
			Rating,
			LastActivity,
			MAX
		}

		// LONG_TODO: data binding?
		protected void ThreadsTable_Init(Object sender, EventArgs e) {
			foreach (var thread in Thread.threads) {
				if (thread == null) {
					continue;
				}

				var cells = new HtmlTableCell[(uint)TableColumn.MAX];
				for (uint i = 0; i < (uint)TableColumn.MAX; ++i) {
					cells[i] = new HtmlTableCell();
				}

				var anchor = new HtmlAnchor();
				anchor.HRef = "/threads/thread.aspx?id=" + thread.Id;
				anchor.InnerText = thread.Title;

				cells[(uint)TableColumn.Id].InnerText = thread.Id.ToString();
				cells[(uint)TableColumn.Title].Controls.Add(anchor);
				cells[(uint)TableColumn.MessageCount].InnerText = thread.MessageCount.ToString();
				cells[(uint)TableColumn.Rating].InnerText = thread.Root.Rating.ToString();
				cells[(uint)TableColumn.LastActivity].InnerText = thread.LastActivity.ToString();

				var row = new HtmlTableRow();
				for (uint i = 0; i < (uint)TableColumn.MAX; ++i) {
					row.Cells.Add(cells[i]);
				}
				ThreadsTable.Rows.Add(row);
			}
		}
	}
}
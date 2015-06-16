using System;
using System.Web.UI.HtmlControls;

namespace Facts {
	public partial class UsersPage: BasePage {
		private enum TableColumns: uint {
			Id,
			Created,
			Name,
			Reputation,
			MessageCount,
			MAX,
		}

		// LONG_TODO: data binding?
		protected void UsersTable_Init(Object sender, EventArgs e) {
			foreach (var user in Facts.User.users) {
				if (user == null) {
					continue;
				}

				var cells = new HtmlTableCell[(uint)TableColumns.MAX];
				for (uint i = 0; i < (uint)TableColumns.MAX; ++i) {
					cells[i] = new HtmlTableCell();
				}

				var nameAnchor = new HtmlAnchor();
				nameAnchor.HRef = "/users/user.aspx?id=" + user.Id;
				nameAnchor.InnerText = user.Name;

				cells[(uint)TableColumns.Id].InnerText = user.Id.ToString();
				cells[(uint)TableColumns.Name].Controls.Add(nameAnchor);
				cells[(uint)TableColumns.Created].InnerText = user.Created.ToString();
				cells[(uint)TableColumns.Reputation].InnerText = user.Reputation.ToString();
				cells[(uint)TableColumns.MessageCount].InnerText = user.MessageCount.ToString();

				var row = new HtmlTableRow();
				for (uint i = 0; i < (uint)TableColumns.MAX; ++i) {
					row.Cells.Add(cells[i]);
				}
				UsersTable.Rows.Add(row);
			}
		}
	}
}
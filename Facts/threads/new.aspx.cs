using System;
using System.Net;

namespace Facts {
	public partial class NewThreadPage: BasePage {
		protected void CreateThread_Click(object sender, EventArgs e) {
			User user = Global.User;
			if (user == null) {
				Transfer(HttpStatusCode.Unauthorized);
				return;
			}

			Thread thread = Thread.CreateThread(user, ThreadTitle.Text, Statement.Text);

			Response.Redirect("/threads/thread.aspx?id=" + thread.Id);
		}
	}
}
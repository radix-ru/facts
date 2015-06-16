using System;
using System.Web;

namespace Facts {
	// LONG_TODO: use event handlers to render the page
	public partial class SidePanel: System.Web.UI.UserControl {
		protected void Page_Load(object sender, EventArgs e) {
			GotoLogIn.HRef = "/login.aspx?url=" + Page.Request.RawUrl;
		}

		protected void WriteUserName() {
			User user = Global.User;
			if (user != null) {
				Page.Response.Write(user.Name);
			}
		}

		protected void GotoLogout_Click(object sender, EventArgs e) {
			HttpContext.Current.Session.Abandon();
			Page.Response.Redirect(Page.Request.RawUrl);
		}

		protected void WriteCurrentDateTime() {
			Page.Response.Write(DateTime.Now);
		}

		protected static bool IsLoggedIn() {
			return Global.User != null;
		}
	}
}
using System;

namespace Facts {
	public partial class LoginPage: BasePage {
		protected void LogInButton_Click(object sender, EventArgs e) {
			User user = Facts.User.LogIn(EmailField.Text, PasswordField.Text);
			if (user == null) {
				StatusLabel.Text = "Login invalid!";
				return;
			}

			Global.User = user;
			Response.Redirect(Request.QueryString["url"] ?? "/Default.aspx");
		}
	}
}	
using System;

namespace Facts {
	public partial class SignupPage: BasePage {
		protected void SignUp_Click(object sender, EventArgs e) {
			bool emailIsValid = SignUpChecker.CheckEmail(EmailField.Text);
			bool nameIsValid = SignUpChecker.CheckName(NameField.Text);
			bool passwordIsValid = SignUpChecker.CheckPassword(PasswordField.Text);
			bool passwordConfirmIsValid = PasswordConfirmField.Text.Equals(PasswordField.Text);

			// LONG_TODO: no post back for status labels?
			EmailStatusLabel.Text = !emailIsValid ? "Email is invalid" : "";
			NameStatusLabel.Text = !nameIsValid ? SignUpChecker.NameRequirement : "";
			PasswordStatusLabel.Text = !passwordIsValid ? SignUpChecker.PasswordRequirement : "";
			PasswordConfirmStatusLabel.Text = !passwordConfirmIsValid ? "Password mismatch" : "";

			if (!(emailIsValid && nameIsValid && passwordIsValid && passwordConfirmIsValid)) {
				return;
			}

			var user = Facts.User.Create(EmailField.Text, NameField.Text, PasswordField.Text);
			if (user == null) {
				EmailStatusLabel.Text = "This email is already used";
				return;
			}

			Global.User = user;
			Response.Redirect(Request.QueryString["url"] ?? "/users/user.aspx?id=" + user.Id);
		}
	}
}
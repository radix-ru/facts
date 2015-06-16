<%@ Page Inherits="Facts.SignupPage" PageSpecificTitle="Sign up" StyleSheets="MainArea-signup-page.css" %>

<asp:Content ContentPlaceHolderId="PageContent" runat="server">
	<form id="SignUpForm" runat="server">
		<table>
			<tr>
				<td>Email:</td>
				<td><asp:TextBox Id="EmailField" runat="server" /></td>
				<td><asp:Label Id="EmailStatusLabel" runat="server" /></td>
			</tr>
			<tr>
				<td>User name:</td>
				<td><asp:TextBox Id="NameField" runat="server" /></td>
				<td><asp:Label Id="NameStatusLabel" runat="server" /></td>
			</tr>
			<tr>
				<td>Password:</td>
				<td><asp:TextBox Id="PasswordField" TextMode="password" runat="server" /></td>
				<td><asp:Label Id="PasswordStatusLabel" runat="server" /></td>
			</tr>
			<tr>
				<td>Confirm password:</td>
				<td><asp:TextBox Id="PasswordConfirmField" TextMode="password" runat="server" /></td>
				<td><asp:Label Id="PasswordConfirmStatusLabel" runat="server" /></td>
			</tr>
			<tr>
				<td><asp:Button Id="SignUpButton" Text="Sign up" OnClick="SignUp_Click" runat="server" /></td>
			</tr>
		</table>
	</form>
</asp:Content>

<%@ Page Inherits="Facts.LoginPage" PageSpecificTitle="Log in" StyleSheets="MainArea-login-page.css" %>

<asp:Content ContentPlaceHolderId="PageContent" runat="server">
	<form id="LoginForm" runat="server">
		<table>
			<tr>
				<td>Email:</td>
				<td><asp:TextBox Id="EmailField" runat="server" /></td>
			</tr>
			<tr>
				<td>Password:</td>
				<td><asp:TextBox Id="PasswordField" TextMode="password" runat="server" /></td>
			</tr>
			<tr>
				<td><asp:Button Id="LogInButton" Text="Log in" OnClick="LogInButton_Click" runat="server" /></td>
				<td><asp:Label Id="StatusLabel" runat="server" /></td>
			</tr>
		</table>
	</form>
</asp:Content>

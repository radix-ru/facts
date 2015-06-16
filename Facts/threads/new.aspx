<%@ Page Inherits="Facts.NewThreadPage" PageSpecificTitle="New thread" %>

<asp:Content ContentPlaceHolderId="PageContent" runat="server">
	<form id="NewThreadForm" runat="server">
		<table>
			<tr>
				<td>Thread title:</td>
				<td><asp:TextBox Id="ThreadTitle" runat="server" /></td>
			</tr>
			<tr>
				<td>Statement:</td>
				<td><asp:TextBox Id="Statement" TextMode="multiline" runat="server" /></td>
			</tr>
			<tr>
				<td><asp:Button Id="CreateThread" Text="Create" OnClick="CreateThread_Click" runat="server" /></td>
			</tr>
		</table>
	</form>
</asp:Content>

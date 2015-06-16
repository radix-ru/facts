<%@ Page Inherits="Facts.UserPage" %>

<asp:Content ContentPlaceHolderId="PageContent" runat="server">
	<table>
		<tr>
			<td>Id</td>
			<td id="Id" runat="server"><%-- user id will be written here --%></td>
		</tr>
		<tr>
			<td>Created</td>
			<td id="Created" runat="server"><%-- user registration datetime will be written here --%></td>
		</tr>
		<tr>
			<td>Name</td>
			<td id="Name" runat="server"><%-- user name will be written here --%></td>
		</tr>
		<tr>
			<td>Messages</td>
			<td id="MessageCount" runat="server"><%-- user message count will be written here --%></td>
		</tr>
		<tr>
			<td>Reputation</td>
			<td id="Reputation" runat="server"><%-- user reputation will be written here --%></td>
		</tr>
		<tr>
			<td>Action points</td>
			<td id="ActionPoints" runat="server"><%-- user action points will be written here --%></td>
		</tr>
	</table>
</asp:Content>

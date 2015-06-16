<%@ Page Inherits="Facts.UsersPage" PageSpecificTitle="Users" %>

<asp:Content ContentPlaceHolderId="PageContent" runat="server">
	<table id="UsersTable" OnInit="UsersTable_Init" runat="server">
		<tr>
			<th>#</th>
			<th>Created</th>
			<th>Name</th>
			<th>Rep.</th>
			<th>Messages</th>
		</tr>
		<%-- user rows will be written here --%>
	</table>
</asp:Content>

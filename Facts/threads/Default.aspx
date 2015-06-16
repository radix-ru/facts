<%@ Page Inherits="Facts.ThreadsPage" PageSpecificTitle="Threads" %>

<asp:Content ContentPlaceHolderId="PageContent" runat="server">
	<table id="ThreadsTable" OnInit="ThreadsTable_Init" runat="server">
		<tr>
			<th>#</th>
			<th>Thread</th>
			<th>Messages</th>
			<th>Rating</th>
			<th>Last activity</th>
		</tr>
		<%-- thread rows will be written here --%>
	</table>
</asp:Content>

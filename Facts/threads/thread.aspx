<%@ Page Inherits="Facts.ThreadPage" StyleSheets="MainArea-thread.css" Scripts="reply.js" %>
<%@ Register Src="~/_ascx/ReplyForm.ascx" TagPrefix="uc" TagName="ReplyForm" %>

<asp:Content ContentPlaceHolderId="PageContent" Id="ThreadPageContent" runat="server">
	<form id="MessagesForm" ClientIDMode="Static" runat="server">
		<div id="dMessages" OnInit="Messages_Init" runat="server" />
		<uc:ReplyForm Id="ReplyForm" runat="server" />
	</form>
</asp:Content>

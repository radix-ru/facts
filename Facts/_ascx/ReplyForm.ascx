<%@ Control Inherits="Facts.ReplyForm" %>

<table id="ReplyForm">
	<asp:HiddenField Id="activeMessageId" ClientIDMode="Static" runat="server" />
	<tr>
		<td>
			<asp:RadioButtonList Id="rblStatementDirection" runat="server">
				<asp:ListItem Text="Confirming" Value="conf" Selected="true" />
				<asp:ListItem Text="Disproving" Value="disp" />
			</asp:RadioButtonList>
			<asp:Button Id="Reply" Text="Reply" OnClick="Reply_Click" runat="server" />
		</td>
		<td><asp:TextBox Id="Statement" TextMode="multiline" Rows="5" Columns="50" runat="server" /></td>
	</tr>
</table>

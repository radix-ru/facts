<%@ Control Inherits="Facts.MessageSubTree" %>

<div class="message_subtree">
	<div id="dSubroot" class="message" runat="server">
		<div class="voting_area">
			<asp:Button Id="bVoteUp" OnClick="bVote_Click" class="vote_up_button" runat="server" />
			<div id="dVotesScore" class="votes_score" runat="server"></div>
			<asp:Button id="bVoteDown" OnClick="bVote_Click" class="vote_down_button" runat="server" />
			<div id="dRating" class="rating" runat="server"></div>
		</div>
		<div class="text_area">
			<div class="message_header">
				<a id="aAuthor" class="author" runat="server"></a>
				<asp:Label Id="lCreated" class="post_datetime" runat="server" />
				<a id="aSelfLink" runat="server">#</a>
				<button type="button" id="bOpenReplyForm" onclick="open_reply_form(event);" class="reply_button" runat="server">+</button>
			</div>
			<div id="dText" class="message_text" runat="server"></div>
		</div>
	</div>
	<div id="dChildren" class="message_children" runat="server"></div>
</div>
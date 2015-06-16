using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Net;

namespace Facts {
	public partial class MessageSubTree: System.Web.UI.UserControl {
		public HtmlGenericControl dChildren;

		public void Set(Message message, User user) {
			dSubroot.Attributes["class"] += message.Type == Message.MessageType.Disproving ? " disproving" : " confirming";
			bVoteUp.Text = message.UpVoters.Count.ToString();
			bVoteUp.ID = "u" + message.Id;
			if (message.GetVoteOf(user) == Message.Vote.Up) { 
				bVoteUp.Attributes["class"] += " pressed";
			}
			if (Global.User == null) {
				bVoteUp.Attributes["disabled"] = "true";
			}
			dVotesScore.InnerText = Math.Abs(message.VotesScore).ToString();
			if (message.VotesScore != 0) {
				dVotesScore.Attributes["class"] += message.VotesScore < 0 ? " negative" : " positive";
			}
			bVoteDown.Text = message.DownVoters.Count.ToString();
			bVoteDown.ID = "d" + message.Id;
			if (message.GetVoteOf(user) == Message.Vote.Down) {
				bVoteDown.Attributes["class"] += " pressed";
			}
			if (Global.User == null) {
				bVoteDown.Attributes["disabled"] = "true";
			}
			dRating.InnerText = ((double)message.Rating/Message.VotesTreshold*100).ToString();
			aAuthor.HRef = "/users/user.aspx?id=" + message.Author.Id;
			aAuthor.InnerText = message.Author.Name;
			lCreated.Text = message.Created.ToString();
			aSelfLink.HRef = "/threads/thread.aspx?id=" + message.Thread.Id + "#" + message.Id;
			bOpenReplyForm.Attributes["data-msg-id"] = message.Id.ToString();
			dText.InnerText = message.Text;
		}

		// FIXME: voting doesn't work
		protected void bVote_Click(Object sender, EventArgs e) {
			User user = Global.User;
			if (user == null) {
				((BasePage)Page).Transfer(HttpStatusCode.Unauthorized);
				return;
			}

			uint messageId;
			try {
				messageId = uint.Parse(((Button)sender).ID.Substring(1));
			} catch (Exception exc) {
				if (!(exc is FormatException || exc is OverflowException)) {
					throw;
				}
				((BasePage)Page).Transfer(HttpStatusCode.InternalServerError);
				return;
			}

			Message message = Message.messages[(int)messageId];
			message.ToggleVote(user, sender == bVoteUp ? Message.Vote.Up : Message.Vote.Down);

			Response.Redirect(Request.RawUrl);
		}
	}
}
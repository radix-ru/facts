using System;
using System.Net;

namespace Facts {
	public partial class ReplyForm: System.Web.UI.UserControl {
		protected void Reply_Click(Object sender, EventArgs e) {
			User user = Global.User;
			if (user == null) {
				((BasePage)Page).Transfer(HttpStatusCode.Unauthorized);
				return;
			}

			String messageText = Statement.Text;
			String messageType = rblStatementDirection.SelectedValue;
			if (messageType == null || !(messageType.Equals("conf") || messageType.Equals("disp"))) {
				((BasePage)Page).Transfer(HttpStatusCode.BadRequest);
				return;
			}

			MyDebug.Assert(!activeMessageId.Value.Equals(""));

			uint id;
			try {
				id = uint.Parse(activeMessageId.Value);
			} catch (Exception exc) {
				if (!(exc is FormatException || exc is OverflowException)) {
					throw;
				}
				((BasePage)Page).Transfer(HttpStatusCode.BadRequest);
				return;
			}
			Message.messages[(int)id].Reply(
				user, 
				messageType.Equals("conf") ? Message.MessageType.Confirming : Message.MessageType.Disproving,
				messageText
			);

			Response.Redirect(Request.RawUrl);
		}
	}
}

using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web.UI.HtmlControls;

namespace Facts {
	public partial class ThreadPage: BasePage {
		private Thread _Thread = null;

		protected void Page_PreInit(object sender, EventArgs e) {
			try {
				uint threadId = uint.Parse(Request.QueryString["id"]);
				if (threadId < Thread.threads.Count) {
					_Thread = Thread.threads[(int)threadId];
				}
			} catch (Exception exc) {
				if (!(exc is ArgumentNullException || exc is FormatException || exc is OverflowException)) {
					throw;
				}
			}

			if (_Thread == null) {
				Transfer(HttpStatusCode.NotFound);
				return;
			}
			PageSpecificTitle = _Thread.Title;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MessageSubTree NewMessageSubtree() {
			// LoadControl(String) should be used as LoadControl(Type, Object[]) doesn't initialize children.
			return (MessageSubTree)LoadControl("~/_ascx/MessageSubTree.ascx");
		}

		private void PopulateMessages(HtmlGenericControl subtreeContainer, Message subroot) {
			var messageSubTree = NewMessageSubtree();
			messageSubTree.Set(subroot, Global.User);
			subtreeContainer.Controls.Add(messageSubTree);
			foreach (var child in subroot.Children) {
				PopulateMessages(messageSubTree.dChildren, child);
			}
		}

		protected void Messages_Init(object sender, EventArgs e) {
			PopulateMessages(dMessages, _Thread.Root);
		}
	}
}
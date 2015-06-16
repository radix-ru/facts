using System;
using System.Net;

namespace Facts {
	public partial class UserPage: BasePage {
		private User _User = null;

		// LONG_TODO: data binding?
		protected void Page_PreInit(Object sender, EventArgs e) {
			try {
				uint userId = uint.Parse(Request.QueryString["id"]);
				if (userId < Facts.User.users.Count) {
					_User = Facts.User.users[(int)userId];
				}
			} catch (Exception exc) {
				if (!(exc is ArgumentNullException || exc is FormatException || exc is OverflowException)) {
					throw;
				}
			}

			if (_User == null) {
				Transfer(HttpStatusCode.NotFound);
				return;
			}
			PageSpecificTitle = _User.Name;
		}

		protected new void Page_PreRender(object sender, EventArgs e) {
			MyDebug.Assert(_User != null);

			base.Page_PreRender(sender, e);
			Id.InnerText = _User.Id.ToString();
			Created.InnerText = _User.Created.ToString();
			Name.InnerText = _User.Name;
			MessageCount.InnerText = _User.MessageCount.ToString();
			Reputation.InnerText = _User.Reputation.ToString();
			if (_User == Global.User) {
				ActionPoints.InnerText = _User.ActionPoints.ToString();
			} else {
				ActionPoints.InnerText = "XXX";
				ActionPoints.Attributes["title"] = "You may see only your own action points.";
			}
		}
	}
}
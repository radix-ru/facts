using System;
using System.Web.UI.HtmlControls;

namespace Facts {
	// It would be more convinient and correct to derive from HtmlHead, but it's sealed.
	public partial class Head: System.Web.UI.UserControl {
		/* NOTE:
		 * Page full title = page specific title + site title.
		 * Page full title ~ Page.Title ~ Head.PageFullTitle ~ Content of <title>
		 * Page specific title ~ BasePage.PageSpecificTitle ~ Head.PageSpecificTitle
		 * Site title ~ _SiteTitle
		 */

		private const String _SiteTitle = "Facts";

		/// NOTE: This property sholud be set before render stage for Head.
		public String PageSpecificTitle { get; set; }

		/// NOTE: This property should be set before Head.PreRender event.
		public String StyleSheets { get; set; }

		/// NOTE: This property should be set before Head.PreRender event.
		public String Scripts { get; set; }

		protected String PageFullTitle {
			get {
				if (String.IsNullOrEmpty(PageSpecificTitle)) {
					return _SiteTitle;
				}
				return PageSpecificTitle + " - " + _SiteTitle;
			}
		}

		private static readonly char[] _Semicolon = new char[]{';'};

		protected void Head_OnPreRender(Object sender, EventArgs e) {
			if (StyleSheets != null) {
				foreach (String styleSheet in StyleSheets.Split(_Semicolon)) {
					var linkTag = new HtmlLink();
					linkTag.Href = "/_css/" + styleSheet;
					linkTag.Attributes["rel"] = "stylesheet";
					HeadElement.Controls.Add(linkTag);
				}
			}
			if (Scripts != null) {
				foreach (String script in Scripts.Split(_Semicolon)) {
					var scriptTag = new HtmlGenericControl();
					scriptTag.TagName = "script";
					scriptTag.Attributes["src"] = "/_js/" + script;
					HeadElement.Controls.Add(scriptTag);
				}
			}
		}
	}
}
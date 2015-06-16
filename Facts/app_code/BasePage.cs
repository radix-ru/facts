using System;
using System.Net;

namespace Facts {
	public class BasePage: System.Web.UI.Page {
		/// INFO: See Head class for title stuff.
		/// NOTE: This property should be set before BasePage.PreRender event.
		public String PageSpecificTitle { get; set; }

		/// ';'-separated list of style sheet filenames within _css directory.
		/// NOTE: This property should be set before BasePage.PreRender event.
		public String StyleSheets { get; set; }

		/// ';'-separated list of js-script filenames within _js directory.
		/// NOTE: This property should be set before BasePage.PreRender event.
		public String Scripts { get; set; }

		// ASSUME: Preceding properties are set before PreInit event.

		protected void Page_PreRender(Object sender, EventArgs e) {
			BasicPage master = (BasicPage)Master;
			master.Head.PageSpecificTitle = PageSpecificTitle;
			master.Head.StyleSheets = StyleSheets;
			master.Head.Scripts = Scripts;
		}

		public void Transfer(HttpStatusCode statusCode) {
			MyDebug.Assert(
				statusCode == HttpStatusCode.BadRequest ||
				statusCode == HttpStatusCode.Unauthorized ||
				statusCode == HttpStatusCode.Forbidden ||
				statusCode == HttpStatusCode.NotFound ||
				statusCode == HttpStatusCode.InternalServerError
			);
			Response.StatusCode = (int)statusCode;
			Server.Transfer("~/error_pages/" + (int)statusCode + ".aspx");
		}
	}
}
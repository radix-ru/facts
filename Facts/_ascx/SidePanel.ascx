<%@ Control Inherits="Facts.SidePanel" %>

<aside id="SidePanel" class="flexbox">
	<nav id="NavPane">
		<a id="GotoMainPage" href="/"><img id="Logo" src="/_img/logo.png" alt="Main page" /></a>
		<hr />
		<% if (IsLoggedIn()) { %>
		<div id="AccountDiv" class="flexbox">
			<form runat="server">
				<asp:LinkButton Id="GotoLogout" OnClick="GotoLogout_Click" runat="server">
					<img width=16 id="LogoutImg" src="/_img/logout.png" />
				</asp:LinkButton>
			</form>
			<div class="hor5spacer"></div>
			<div id="UserName"><% WriteUserName(); %></div>
		</div>
		<% } else { %>
		<div id="AccountDiv" class="flexbox">
			<a id="GotoSignUp" href="/signup.aspx">Sign up</a>
			<div class="hor5spacer"></div>
			<a id="GotoLogIn" runat="server">Log in</a>
		</div>
		<% } %>
		<hr />
		<% if (IsLoggedIn()) { %>
		<a id="GotoNewStatement" href="/threads/new.aspx">Create a thread</a>
		<hr />
		<% } %>
		<a id="GotoThreads" href="/threads/">Threads</a>
		<a id="GotoUsers" href="/users/">Users</a>
	</nav>
	<div id="SidePanelToggle" onclick="onShowHideClicked()">«</div>
	<footer id="Footer">
		<a id="GotoHelp" href="/help.html">Help</a>
		<div id="CurrentDateTime">Date/time on server: <% WriteCurrentDateTime(); %></div>
		<div id="Copyright">Copyright © Radix, 2015</div>
	</footer>
</aside>

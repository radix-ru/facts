Node.prototype.insertAfter = function(e, ref) {
	this.insertBefore(e, ref);
}

Element.prototype.appendElement = function(tagName) {
	var el = document.createElement(tagName);
	this.appendChild(el);
	return el;
}

Element.prototype.prependElement = function (tagName) {
    var el = document.createElement(tagName);
    this.insertBefore(el, this.firstChild);
    return el;
}

function onShowHideClicked() {
	var SidePanel = document.getElementById("SidePanel");
	SidePanel.classList.toggle("collapsed");
	var SidePanelToggle = document.getElementById("SidePanelToggle");
	if (SidePanelToggle.innerHTML == "«") {
		SidePanelToggle.innerHTML = "»";
	} else {
		SidePanelToggle.innerHTML = "«";
	}
}

function onSidePanelLoad() {
	var SidePanel = document.getElementById("SidePanel");
	var w = SidePanel.getBoundingClientRect().width;
	SidePanel.style.width = w;

	var style = document.createElement("style");
	style.type = "text/css";
	style.innerHTML = "#SidePanel.collapsed { left: -" + w + "px }";
	document.getElementsByTagName('head')[0].appendChild(style);
}

function repaint_lines() {
	var subStatements = document.querySelectorAll(".statement_tree > .statement_tree > .statement");
	for (var i = 0; i < subStatements.length; ++i) {
		var subStatement = subStatements[i];
		var parentStatement = subStatement.parentElement.parentElement.children[0];
		var y1 = parentStatement.getBoundingClientRect().bottom;
		var y2 = subStatement.getBoundingClientRect().top + subStatement.getBoundingClientRect().height / 2;
		var h = y2 - y1;
		var conf_line = subStatement.querySelector(".statement_line");
		conf_line.style.height = h;
	}
}

function on_button_toggle_click(e) {
	if (!e) {
		e = window.event;
	}
	var button = e.srcElement || e.target;
	var type_div = button.parentElement;
	var control_div = type_div.parentElement;
	var header = control_div.parentElement;
	var section = header.parentElement;
	var type = type_div.className.substring(0, type_div.className.length - "_control".length);
	var subsections = section.children;
	var f = 0;
	for (var i = 0; i < subsections.length; ++i) {
		var subsection = subsections[i];
		if (subsection.tagName.toLowerCase() != "section") {
			continue;
		}
		var subheader = subsection.querySelector("header");
		if (subheader.classList.contains(type)) {
			if (window.getComputedStyle(subsection).display == "block") {
				subsection.style.display = "none";
				f = -1;
			} else {
				subsection.style.display = "block";
				f = +1;
			}
		}
	}
	if (f) {
		if (f == +1) {
			button.innerHTML = "-";
		} else {
			button.innerHTML = "+";
		}
		repaint_lines();
	}
}

function add_buttons() {
	var MainArea = document.getElementById("RootTree");
	var headers = MainArea.getElementsByTagName("header");
	for (var i = 0; i < headers.length; ++i) {
	    var header = headers[i];
//	    alert(header.tagName + " " + header.className);
		var RespCtrl = header.prependElement("div");
		RespCtrl.className = "responses_control";
		var msg_types = ["comment", "disproving", "confirming"];
		for (var j = 0; j < 3; ++j) {
		    var div = RespCtrl.appendElement("div");
		    div.className = msg_types[j] + "_control";
		    var button_new = div.appendElement("button");
		    button_new.innerHTML = "N";
		    button_new.onclick = "on_button_new_click(event)";
		    var button_toggle = div.appendElement("button");
		    button_toggle.innerHTML = "-";
		    button_toggle.onclick = function (e) {
		    	on_button_toggle_click(e);
		    };
		}
	}
}

function onLoad() {
	add_buttons();

	var subStatements = document.querySelectorAll(".statement_tree > .statement_tree > .statement");
	for (var i = 0; i < subStatements.length; ++i) {
		var conf_line = subStatements[i].appendElement("div");
		conf_line.className = "statement_line";
	}

	repaint_lines();
	
/*	for (var i = 0; i < subStatements.length; ++i) {
		var subStatement = subStatements[i];
		var line = document.createElement("div");
		subStatement.appendChild(line);
		line.className = "statement_line";
//		line.style.height = subStatement.getBoundingClientRect().height / 2 + 5;
		line.style.top = "-5px";
		line.style.bottom = "50%";
//		subStatement.innerHTML += subStatement.getBoundingClientRect().height;

		var line2 = document.createElement("div");
		subStatement.appendChild(line2);
		line2.className = "statement_line2";
	}
*/}
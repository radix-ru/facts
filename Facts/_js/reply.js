function open_reply_form(event) {
	var bReply = event.target;
	var MessageHeader = bReply.parentNode;
	var TextArea = MessageHeader.parentNode;
	var Message = TextArea.parentNode;
	var MessageSubtree = Message.parentNode;
	var dMessageChildren = MessageSubtree.querySelector(".message_children");

	var ReplyForm = document.getElementById("ReplyForm");
	
	if (ReplyForm.parentNode == dMessageChildren) {
		ReplyForm.style.display = "none";
		MessagesForm.appendChild(ReplyForm);
		return;
	}
	
	dMessageChildren.insertBefore(ReplyForm, dMessageChildren.firstChild);
	ReplyForm.style.display = "block";
	
	var activeMessageIdField = document.getElementById("activeMessageId");
	activeMessageIdField.value = bReply.dataset.msgId;
}

var port = null;

function appendMessage(text) {
	console.log(text);
}

function sendNativeMessage(content = new Date().toISOString()) {
	if (port == null)
		connect();
	var message = {
		"content": content
	}
	port.postMessage(message);
	appendMessage("Sent message: " + JSON.stringify(message));
	return true;
}

function onNativeMessage(message) {
	appendMessage("Received message: " + JSON.stringify(message));
}

function onDisconnected() {
	appendMessage("Failed to connect: " + chrome.runtime.lastError.message);
	port = null;
}

function connect() {
	var hostName = "com.my_company.my_application";
	if (port != null) {
		appendMessage("Already connected to native messaging host " + hostName);
		return;
	}
	appendMessage("Connecting to native messaging host " + hostName);
	port = chrome.runtime.connectNative(hostName);
	port.onMessage.addListener(onNativeMessage);
	port.onDisconnect.addListener(onDisconnected);
}
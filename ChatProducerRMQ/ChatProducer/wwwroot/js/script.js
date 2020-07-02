const uriLogin = 'api/login';
const uriChats = 'api/client/find-chat/';
const uriSendMessage = 'api/message';
const uriMessagesChat = 'api/message/find-message/';
let chats = [];
let messagesChat = [];
let user = {};

function getChats() {
    fetch(uriChats + user.clientEmail, {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer  ' + user.clientToken
        }
    }).then(response => response.json())
        .then(data => _displayChats(data))
        .catch(error => console.error('Unable to get items.', error));
}


function login() {

    const clientemail = document.getElementById('clientemail');
    const clientpass = document.getElementById('clientpass');
    const bearer_token = document.getElementById('bearer_token');

    const client = {
        clientEmail: clientemail.value.trim(),
        clientPass: clientpass.value.trim()
    };

    fetch(uriLogin, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(client)
    })
        .then(response => response.json())
        .then(data => {
            user = data;
            bearer_token.innerHTML = data.clientToken;
            clientemail.value = '';
            clientpass.value = '';
            getChats();
        })
        .catch(error => console.error('Unable to add item.', error));
}
/*
function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = todos.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isComplete').checked = item.isComplete;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isComplete: document.getElementById('edit-isComplete').checked,
        name: document.getElementById('edit-name').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'to-do' : 'to-dos';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}
*/
function _displayChats(data) {
    const chat_content = document.getElementById('chats');
    chat_content.innerHTML = '';

    //_displayCount(data.length);

    //const button = document.createElement('button');

    data.forEach(client => {
        let clientname = document.createElement('h1');
        clientname.innerText =  client.clientName;

        let clientemail = document.createElement('p');
        clientemail.innerText = client.clientEmail;

        let openChatButton = document.createElement('button');
        openChatButton.innerText = 'Open';
        openChatButton.setAttribute('class', 'button');
        openChatButton.setAttribute('onclick', 'openChat()');

        chat_content.appendChild(clientname);
        chat_content.appendChild(clientemail);
        chat_content.appendChild(openChatButton);

        //let td3 = tr.insertCell(2);
        //td3.appendChild(editButton);

        //let td4 = tr.insertCell(3);
        //td4.appendChild(deleteButton);
    });

    chats = data;
}

function openChat() {
    fetch(uriMessagesChat + user.clientEmail, {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer  ' + user.clientToken
        }
    }).then(response => response.json())
        .then(data => _displaMessages(data))
        .catch(error => console.error('Unable to get items.', error));
}

function _displaMessages(data) {

    const messages = document.getElementById('messages');
    messages.innerHTML = '';

    //_displayCount(data.length);

    //const button = document.createElement('button');

    let divinputmessage = document.createElement('div');
    divinputmessage.setAttribute('id', 'input-message');

    let messagecontent = document.createElement('textarea');
    messagecontent.setAttribute('id', 'messagecontent');

    let openChatButton = document.createElement('button');
    openChatButton.innerText = 'Send';
    openChatButton.setAttribute('class', 'button');
    openChatButton.setAttribute('onclick', `sendMessage(${data[0].chat.chatId})`);

    divinputmessage.appendChild(messagecontent);
    divinputmessage.appendChild(openChatButton);

    let divmessageGlobal = document.createElement('div');

    data.forEach(message => {

        let divmessage = document.createElement('div');

        let messageContent = document.createElement('h1');
        messageContent.innerText = message.messageContent;

        let fromEmail = document.createElement('p');
        fromEmail.innerText = message.chat.from;

        if (message.chat.from == user.clientEmail) {
            divmessage.style.color = "#000"; divmessage.style.marginRight = "0px"; messageContent.style.color = "#000"; divmessage.style.backgroundColor = "#fff";
        } else {
            divmessage.style.color = "#fff"; divmessage.style.marginLeft = "0px"; divmessage.style.borderRadius = "0px 10px 10px 0px"; messageContent.style.color = "#fff"; divmessage.style.backgroundColor = "#000";
        }

        divmessage.appendChild(fromEmail);
        divmessage.appendChild(messageContent);

        divmessageGlobal.appendChild(divmessage);

        //let td3 = tr.insertCell(2);
        //td3.appendChild(editButton);

        //let td4 = tr.insertCell(3);
        //td4.appendChild(deleteButton);
    });

    divmessageGlobal.appendChild(divinputmessage);
    messages.appendChild(divmessageGlobal);
    messages.style.display = "block";

    messagesChat = data;
}

function sendMessage(id){

    const messagecontent = document.getElementById('messagecontent');

    const message = {
        messageContent: messagecontent.value,
        chatId: id
    };

    fetch(uriSendMessage, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer  ' + user.clientToken
        },
        body: JSON.stringify(message)
    })
        .then(response => response.json())
        .then(data => {
            messagecontent.value = '';
            openChat();
        })
        .catch(error => console.error('Unable to add item.', error));
}
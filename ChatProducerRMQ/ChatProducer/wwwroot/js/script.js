const uriLogin = 'api/login';
const uriChats = 'api/client/find-chat/';
const uriSendMessage = 'api/message';
const uriMessagesChat = 'api/message/find-message/';
let chats = [];
let messagesChat = [];
let user = {};
let chat_id = 0;

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
        openChatButton.setAttribute('onclick', `openChat(${client.chat.chatId})`);

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

function openChat(id) {

    setTimeout(function () {

    fetch(uriMessagesChat + user.clientEmail, {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer  ' + user.clientToken
        }
    }).then(response => response.json())
        .then(data => _displaMessages(data, id))
            .catch(error => console.error('Unable to get items.', error));

        openChat(id);
    }, 1000);
}

function _displaMessages(data, id) {

    const messages = document.getElementById('messages');
    const messages_finded = document.getElementById('messages_finded');
    messages_finded.innerHTML = '';
    const send_btn = document.getElementById('send');

    data.forEach(message => {

        let div = document.createElement('div');

        let h1 = document.createElement('h1');
        h1.innerText = message.messageContent;
        let p = document.createElement('p');
        p.innerText = message.chat.from;

        if (message.chat.from == user.clientEmail) {
            div.style.marginRight = "0px";
            div.style.backgroundColor = "#fff";
            div.style.borderRadius = "10px 0px 0px 10px";
        } else {
            div.style.marginLeft = "0px";
            div.style.backgroundColor = "#e0afaf";
            div.style.borderRadius = "0px 10px 10px 0px";
        }

        div.appendChild(h1);
        div.appendChild(p);
        messages_finded.appendChild(div);

    });

    send_btn.setAttribute('onclick', `sendMessage(${id})`);
    messages.style.display = "block";
    messages_finded.style.display = "block";

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
            openChat(id);
        })
        .catch(error => console.error('Unable to add item.', error));
}

function timeout() {
}
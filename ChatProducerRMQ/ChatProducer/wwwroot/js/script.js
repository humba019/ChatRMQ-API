const uriLogin = 'api/login';
const uriChats = 'api/client/find-chat/';
const uriChat = 'api/chat';
const uriClients = 'api/client/diff/';
const uriSendMessage = 'api/message';
const uriMessagesChat = 'api/message/find-message/';
const uriMessagesCount = 'api/message/count-message/';
let chats = [];
let clients = [];
let messagesChat = [];
let user = {};
let chat_id = 0;
let chat_messages_count = 0;

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
function getClients() {
    fetch(uriClients + user.clientEmail, {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer  ' + user.clientToken
        }
    }).then(response => response.json())
        .then(data => _displayClients(data))
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
function _displayClients(data) {
    const client_content = document.getElementById('clients');
    client_content.innerHTML = '';

    //_displayCount(data.length);

    //const button = document.createElement('button');

    data.forEach(client => {

        let clientdiv = document.createElement('div');

        let clientname = document.createElement('h1');
        clientname.innerText = client.clientName;

        let createChatButton = document.createElement('button');
        createChatButton.setAttribute('class', 'button');
        createChatButton.setAttribute('onclick', `createChat('${client.clientEmail}')`);

        let icon = document.createElement('i');
        icon.innerText = "mail";
        icon.setAttribute('class', 'large material-icons');

        createChatButton.appendChild(icon);
        createChatButton.style.marginRight = "10px";
        clientdiv.appendChild(createChatButton);
        clientdiv.appendChild(clientname);
        client_content.appendChild(clientdiv);

        //let td3 = tr.insertCell(2);
        //td3.appendChild(editButton);

        //let td4 = tr.insertCell(3);
        //td4.appendChild(deleteButton);
    });

    configBeforeOpenClient();

    clients = data;

}

function createChat(to) {

    const chat = {
        from: user.clientEmail,
        to: to
    };

    fetch(uriChat, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer  ' + user.clientToken
        },
        body: JSON.stringify(chat)
    })
        .then(response => response.json())
        .then(data => {
            getChats();
        })
        .catch(error => console.error('Unable to add item.', error));
}

function searchInLoopCount(id, email) {

    setTimeout(function () {
        fetch(uriMessagesCount + email, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer  ' + user.clientToken
            }
        }).then(response => response.json())
            .then(data => {
                if (data > 0) {
                    openChat(id, email, 'true');
                }
            })
            .catch(error => console.error('Unable to get items.', error));
        searchInLoopCount(id, email);
    }, 1000);
}

function _displayChats(data) {
    const chat_content = document.getElementById('chats');
    chat_content.innerHTML = '';

    const new_messages = document.getElementById('new_messages');

    //_displayCount(data.length);

    //const button = document.createElement('button');

    data.forEach(client => {

        let clientdiv = document.createElement('div');
        let h3count = document.createElement('h3');
        let spancount = document.createElement('span');
        spancount.setAttribute('class', 'count');
        spancount.setAttribute('id', 'count' + client.chat.chatId);

        fetch(uriMessagesCount + client.clientEmail, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer  ' + user.clientToken
            }
        }).then(response => response.json())
            .then(data => {
                spancount.innerHTML = data;
                new_messages.innerHTML = `${data} Messages`;
                new_messages.style.display = "block";
            })
            .catch(error => console.error('Unable to get items.', error));

        h3count.appendChild(spancount);
        let clientname = document.createElement('h1');
        clientname.innerText =  client.clientName;

        let clientemail = document.createElement('p');
        clientemail.innerText = client.clientEmail;

        let openChatButton = document.createElement('button');
        openChatButton.innerText = 'Chat now!';
        openChatButton.setAttribute('class', 'button');
        openChatButton.setAttribute('onclick', `openChat(${client.chat.chatId},'${client.clientEmail}', 'true')`);

        clientdiv.appendChild(h3count);
        clientdiv.appendChild(clientname);
        clientdiv.appendChild(clientemail);
        clientdiv.appendChild(openChatButton);
        chat_content.appendChild(clientdiv);

        //let td3 = tr.insertCell(2);
        //td3.appendChild(editButton);

        //let td4 = tr.insertCell(3);
        //td4.appendChild(deleteButton);
    });

    configBeforeOpenChat();

    chats = data;

}
function openChat(id, email, consume) {

    //setTimeout(function () {

        fetch(uriMessagesChat + user.clientEmail + "/" + consume, {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer  ' + user.clientToken
        }
    }).then(response => response.json())
        .then(data => _displaMessages(data, email, id))
        .catch(error => console.error('Unable to get items.', error));

    searchInLoopCount(id, email);
        //openChat(id, consume);
    //}, 1000);
}

function _displaMessages(data, email, id) {

    const messages = document.getElementById('messages');
    const count = document.getElementById('count' + id);
    const new_messages = document.getElementById('new_messages');
    const messages_finded = document.getElementById('messages_finded');
    messages_finded.innerHTML = '';
    const send_btn = document.getElementById('input_button');

    data.forEach(message => {

        let div = document.createElement('div');

        fetch(uriMessagesCount + message.chat.to, {
            method: 'GET',
            headers: {
                'Authorization': 'Bearer  ' + user.clientToken
            }
        }).then(response => response.json())
            .then(data => {
                count.innerHTML = data;
                new_messages.innerHTML = `${data} Messages`;
                new_messages.style.display = "block";
            })
            .catch(error => console.error('Unable to get items.', error));

        let h1 = document.createElement('h1');
        h1.innerText = message.messageContent;
        let p = document.createElement('p');
        p.innerText = message.chat.from;

        if (message.chat.from == user.clientEmail) {
            div.style.marginLeft = "50%";
            div.style.backgroundColor = "#fff";
            div.style.borderRadius = "10px 0px 0px 10px";
        } else {
            div.style.marginLeft = "0%";
            div.style.backgroundColor = "#000";
            div.style.borderRadius = "0px 10px 10px 0px";
        }

        div.appendChild(h1);
        div.appendChild(p);
        messages_finded.appendChild(div);

    });

    send_btn.setAttribute('onclick', `sendMessage(${id}, '${email}')`);

    configBeforeOpenMessage();

    messagesChat = data;
}

function sendMessage(id, email){

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
            openChat(id, email, "false");
        })
        .catch(error => console.error('Unable to add item.', error));
}

function configBeforeOpenMessage() {
    document.getElementById('bearer_token').style.display = "none";
    document.getElementById('form_login').style.display = "none";
    document.getElementById('clients').style.display = "none";
    document.getElementById('chats').style.display = "none";
    document.getElementById('messages').style.display = "block";
}

function configBeforeOpenChat() {
    document.getElementById('bearer_token').style.display = "none";
    document.getElementById('form_login').style.display = "none";
    document.getElementById('clients').style.display = "none";
    document.getElementById('chats').style.display = "block";
    document.getElementById('messages').style.display = "none";
}
function configBeforeOpenClient() {
    //document.getElementById('').style.display = "";
    document.getElementById('bearer_token').style.display = "none";
    document.getElementById('form_login').style.display = "none";
    document.getElementById('clients').style.display = "block";
    document.getElementById('chats').style.display = "none";
    document.getElementById('messages').style.display = "none";

}
function configBeforeOpenFormLogin() {
    document.getElementById('bearer_token').style.display = "none";
    document.getElementById('form_login').style.display = "block";
    document.getElementById('clients').style.display = "none";
    document.getElementById('chats').style.display = "none";
    document.getElementById('messages').style.display = "none";
}
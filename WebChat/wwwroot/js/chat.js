function AddMessage(userId, message, time) {
    var username;
    var li = document.createElement("div");
    if (userId == _userId) {
        username = "you";
        li.classList.add("own-message");
    }
    else {
        for (let item of chatInfo.Users) {
            if (userId == item.Id) {
                username = item.Name
                break
            }
        }
        li.classList.add("otheer-message");
    }
    messages.appendChild(li);
    var txt = document.createElement("span");
    txt.classList.add("message-content");
    txt.innerText = message;
    li.appendChild(txt);
    var ownr = document.createElement("span");
    ownr.classList.add("message-owner");
    ownr.innerText = username;
    li.appendChild(ownr);
    var tm = document.createElement("span");
    tm.classList.add("message-timestamp");
    tm.innerText = time;
    li.appendChild(tm);
}

"use strict";
const messages = document.getElementById("messageList");
var connection = new
    signalR.HubConnectionBuilder().withUrl("/chatHub").build();
//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;
$.ajax({
    type: "GET",
    url: '../ChatMessages',
    data: {id:chatInfo.ChatId},
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    beforeSend: function(){
        
    },
    success: function (response) {
        $.each(response, function (index, mes) {
            AddMessage(mes.userId, mes.text, mes.time);
        })
    },
    complete: function(){
    },
    error: function (jqXHR, textStatus, errorThrown) {
        alert("HTTP Status: " + jqXHR.status + "; Error Text: " + jqXHR.responseText); // Display error message
    }
});
connection.on("ReceiveMessage", function (chatId,userId, message,time) {
    if(chatId==chatInfo.ChatId)
    AddMessage(userId, message, time);
});
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});
document.getElementById("sendButton").addEventListener("click", function
    (event) {
    var chat = chatInfo.ChatId;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", chatInfo.ChatId, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

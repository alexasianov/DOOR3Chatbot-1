(function () {
    let chatWidget = document.createElement("div");
    chatWidget.innerHTML = `
        <style>
            #chat-container {
                position: fixed;
                bottom: 20px;
                right: 20px;
                width: 300px;
                background: white;
                border-radius: 10px;
                box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
                font-family: Arial, sans-serif;
                display: flex;
                flex-direction: column;
            }
            #chat-header {
                background: #0078D7;
                color: white;
                padding: 10px;
                font-size: 16px;
                text-align: center;
                border-top-left-radius: 10px;
                border-top-right-radius: 10px;
                cursor: pointer;
            }
            #chat-messages {
                height: 200px;
                overflow-y: auto;
                padding: 10px;
            }
            #chat-input {
                display: flex;
                padding: 10px;
                border-top: 1px solid #ddd;
            }
            #chat-input input {
                flex: 1;
                padding: 5px;
                border: 1px solid #ddd;
                border-radius: 5px;
            }
            #chat-input button {
                margin-left: 5px;
                padding: 5px 10px;
                background: #0078D7;
                color: white;
                border: none;
                border-radius: 5px;
                cursor: pointer;
            }
        </style>
        <div id="chat-container">
            <div id="chat-header">Chat with AI</div>
            <div id="chat-messages"></div>
            <div id="chat-input">
                <input type="text" id="user-input" placeholder="Type a message..." />
                <button onclick="sendMessage()">Send</button>
            </div>
        </div>
    `;

    document.body.appendChild(chatWidget);

    window.sendMessage = function () {
        let inputField = document.getElementById("user-input");
        let message = inputField.value;
        if (!message.trim()) return;

        let chatMessages = document.getElementById("chat-messages");
        chatMessages.innerHTML += `<div><strong>You:</strong> ${message}</div>`;

        inputField.value = "";

        fetch("http://localhost:5000/api/chatbot/ask", { // Change to your actual API URL
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ message: message })
        })
            .then(response => response.json())
            .then(data => {
                let reply = JSON.parse(data.choices[0].message.content);
                chatMessages.innerHTML += `<div><strong>AI:</strong> ${reply}</div>`;
            })
            .catch(error => console.error("Error:", error));
    };
})();

import React, { useState, useCallback } from "react";  
import { Toggle, Stack, PrimaryButton, IconButton } from "@fluentui/react";  
import ChatBot from "react-chatbotify";  
  
const MyChatBot = () => {  
  const [useTool, setUseTool] = useState(false);  
  const [chatKey, setChatKey] = useState(0); // For restarting the conversation  
  const [isMinimized, setIsMinimized] = useState(false); // For minimizing the chatbot  
  
  const fetchData = useCallback(  
    async (prompt) => {  
      try {  
        const queryParams = new URLSearchParams({ useTool: useTool ? "true" : "false" });  
        const response = await fetch(`/api/weatherforecastchat?${queryParams.toString()}`, {  
          method: "POST",  
          headers: {  
            "Content-Type": "application/json",  
          },  
          body: JSON.stringify({ message: prompt.userInput }),  
        });  
  
        if (!response.ok) {  
          throw new Error(`HTTP error! status: ${response.status}`);  
        }  
  
        const chatJson = await response.json();  
        return chatJson.message;  
      } catch (error) {  
        console.error("Could not fetch the chat response:", error);  
        return "Oh no I don't know what to say!";  
      }  
    },  
    [useTool]  
  );  
  
  const flow = {  
    start: {  
      message: "Hi! How can I help you?",  
      path: "loop",  
    },  
    loop: {  
      message: async (params) => {  
        const result = await fetchData(params);  
        return result;  
      },  
      path: "loop",  
    },  
  };  
  
  const customStyles = {  
    chatContainer: {  
      height: "400px",  
      width: "300px",  
    },  
  };  
  
  const restartConversation = () => {  
    setChatKey(prevKey => prevKey + 1); // Change the key to restart the chatbot  
  };  
  
  const handleToggleChange = (e, checked) => {  
    setUseTool(!!checked); // Update the useTool state  
    restartConversation(); // Restart the conversation  
  };  
  
  const toggleMinimize = () => {  
    setIsMinimized(!isMinimized); // Toggle the minimized state  
  };  
  
  return (  
    <div className="chatbot-container">  
      <IconButton  
        iconProps={{ iconName: isMinimized ? "ChevronUp" : "ChevronDown" }}  
        title={isMinimized ? "Expand" : "Minimize"}  
        onClick={toggleMinimize}  
      />  
      {!isMinimized && (  
        <Stack tokens={{ childrenGap: 10 }}>  
          <ChatBot  
            key={chatKey}  
            settings={{  
              general: {  
                embedded: true,  
              },  
              header: {  
                title: "Weather CoPilot",  
              },  
              notification: {  
                disabled: true,  
              },  
              footer: {  
                text: (  
                  <div  
                    style={{  
                      display: "flex",  
                      justifyContent: "space-between",  
                      alignItems: "center",  
                      width: "100%",  
                    }}  
                  >  
                    <div style={{ flex: 1, display: "flex", justifyContent: "flex-start" }}>  
                      <Toggle  
                        label="Use Tools"  
                        checked={useTool}  
                        onChange={handleToggleChange}  
                        inlineLabel  
                      />  
                    </div>  
                    <div style={{ flex: 1, display: "flex", justifyContent: "flex-end" }}>  
                      <PrimaryButton text="Restart Conversation" onClick={restartConversation} />  
                    </div>  
                  </div>  
                ),  
                buttons: [],  
              },  
            }}  
            flow={flow}  
            customStyles={customStyles}  
          />  
        </Stack>  
      )}  
    </div>  
  );  
};  
  
export default MyChatBot;  

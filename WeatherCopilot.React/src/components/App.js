import { useState } from "react";  
import "./App.css";  
  
function App() {  
  const [forecasts, setForecasts] = useState([]);  
  const [city, setCity] = useState('');  
  const [state, setState] = useState('');  
  const [chatInput, setChatInput] = useState('');  
  const [chatResponse, setChatResponse] = useState('');  
  const [loadingChat, setLoadingChat] = useState(false); // Loading state for chat  
  const [loadingWeather, setLoadingWeather] = useState(false); // Loading state for weather  
  const [useTool, setUseTool] = useState(false); // State for the checkbox  
  
  const requestWeather = async (city, state) => {  
    setLoadingWeather(true); // Set loading to true when weather request starts  
    try {  
      const queryParams = new URLSearchParams({ city, state });  
      const response = await fetch(`api/weatherforecast?${queryParams}`);  
      if (!response.ok) {  
        throw new Error(`HTTP error! status: ${response.status}`);  
      }  
      const weatherJson = await response.json();  
      setForecasts(weatherJson);  
    } catch (error) {  
      console.error("Could not fetch the weather data:", error);  
    } finally {  
      setLoadingWeather(false); // Set loading to false when weather request ends  
    }  
  };  
  
  const requestChatResponse = async (prompt) => {  
    setLoadingChat(true); // Set loading to true when chat request starts  
    try {  
      const queryParams = new URLSearchParams({ useTool });  
      const response = await fetch(`/api/weatherforecastchat?${queryParams}`, {  
        method: 'POST',  
        headers: {  
          'Content-Type': 'application/json',  
        },  
        body: JSON.stringify({ message: prompt }),  
      });  
      if (!response.ok) {  
        throw new Error(`HTTP error! status: ${response.status}`);  
      }  
      const chatJson = await response.json();  
      setChatResponse(chatJson.message); // Ensure this matches the API response format  
    } catch (error) {  
      console.error("Could not fetch the chat response:", error);  
    } finally {  
      setLoadingChat(false); // Set loading to false when chat request ends  
    }  
  };  
  
  const handleCityChange = (event) => {  
    setCity(event.target.value);  
  };  
  
  const handleStateChange = (event) => {  
    setState(event.target.value);  
  };  
  
  const handleSearch = () => {  
    requestWeather(city, state);  
  };  
  
  const handleChatInputChange = (event) => {  
    setChatInput(event.target.value);  
  };  
  
  const handleChatSubmit = () => {  
    requestChatResponse(chatInput);  
  };  
  
  const handleUseToolChange = (event) => {  
    setUseTool(event.target.checked);  
  };  
  
  return (  
    <div className="App">  
      <header className="App-header">  
        <h1>Weather</h1>  
        {/* Chat Section */}  
        <div className="section-container chat-section">  
          <h2>Chat</h2>  
          <textarea  
            rows="4"  
            placeholder="Type your prompt..."  
            value={chatInput}  
            onChange={handleChatInputChange}  
          />  
          <div>  
            <input  
              type="checkbox"  
              id="useTool"  
              checked={useTool}  
              onChange={handleUseToolChange}  
            />  
            <label htmlFor="useTool">Use Tools</label>  
          </div>  
          <button onClick={handleChatSubmit}>Send</button>  
          {loadingChat ? <div className="spinner"></div> : chatResponse && <p>{chatResponse}</p>}  
        </div>  
        {/* Divider */}  
        <hr className="divider" />  
        {/* Weather Section */}  
        <div className="section-container weather-section">  
          <h2>Weather</h2>  
          <div>  
            <input  
              type="text"  
              placeholder="City"  
              value={city}  
              onChange={handleCityChange}  
            />  
            <input  
              type="text"  
              placeholder="State"  
              value={state}  
              onChange={handleStateChange}  
            />  
            <button onClick={handleSearch}>Get Weather</button>  
          </div>  
          {loadingWeather ? <div className="spinner"></div> : (  
            <table>  
              <thead>  
                <tr>  
                  <th>Name</th>  
                  <th>Temp. (C)</th>  
                  <th>Temp. (F)</th>  
                  <th>Summary</th>  
                </tr>  
              </thead>  
              <tbody>  
                {forecasts.length === 0 ? (  
                  <tr>  
                    <td colSpan="5">No forecasts available</td>  
                  </tr>  
                ) : (  
                  forecasts.map((forecast) => (  
                    <tr key={forecast.order}>  
                      <td>{forecast.name}</td>  
                      <td>{forecast.temperatureC}</td>  
                      <td>{forecast.temperatureF}</td>  
                      <td>{forecast.summary}</td>  
                    </tr>  
                  ))  
                )}  
              </tbody>  
            </table>  
          )}  
        </div>  
      </header>  
    </div>  
  );  
}  
  
export default App;  

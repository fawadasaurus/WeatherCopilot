import { useState } from "react";
import "./App.css";

function App() {
  const [forecasts, setForecasts] = useState([]);
  const [city, setCity] = useState('');
  const [state, setState] = useState('');

  const requestWeather = async (city, state) => {
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

  return (
    <div className="App">
      <header className="App-header">
        <h1>React Weather</h1>
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
        <table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Temp. (C)</th>
              <th>Temp. (F)</th>
              <th>Summary</th>
              <th>Details</th>
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
                  <td>{forecast.details}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </header>
    </div>
  );
}

export default App;

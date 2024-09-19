import { useState } from "react";  
import {  
  TextField,  
  PrimaryButton,  
  Spinner,  
  SpinnerSize,  
  Stack,  
  Label,  
  DetailsList,  
  DetailsListLayoutMode,  
  mergeStyleSets,  
  initializeIcons,  
  FontSizes  
} from "@fluentui/react";  
import "./App.css";  
import MyChatBot from "./MyChatBot";  
  
initializeIcons();  
  
const classNames = mergeStyleSets({  
  fontsize: {  
    fontSize: FontSizes.size18  
  },  
  body: {  
    fontSize: FontSizes.size18  
  },  
  container: {  
    padding: "40px",  
    backgroundColor: "#f3f2f1",  
    minHeight: "100vh",  
    fontSize: FontSizes.size18,  
    position: "relative" // Ensure relative positioning for correct stacking context  
  },  
  header: {  
    backgroundColor: "#0078d4",  
    color: "white",  
    padding: "20px",  
    textAlign: "center",  
    borderRadius: "5px",  
    marginBottom: "20px",  
  },  
  section: {  
    margin: "20px 0",  
    backgroundColor: "white",  
    padding: "20px",  
    borderRadius: "5px",  
    boxShadow: "0 2px 4px rgba(0, 0, 0, 0.1)",  
    fontSize: FontSizes.size18  
  },  
  inputStack: {  
    maxWidth: "600px",  
    margin: "0 auto",  
    fontSize: FontSizes.size18  
  },  
  list: {  
    marginTop: "20px",  
    fontSize: FontSizes.size18  
  },  
  listCell: {  
    fontSize: FontSizes.size18,  
  },  
  botContainer: {  
    fontSize: FontSizes.size18  
  },  
  button: {  
    height: '32px', // Adjust the height to match your design  
    marginTop: '28px', // Adjust the margin to match your design  
    padding: '0 16px', // Adjust the padding for a balanced look  
    fontSize: FontSizes.size18, // Adjust the font size to match other text elements  
  },  
});  
  
const App = () => {  
  const [forecasts, setForecasts] = useState([]);  
  const [city, setCity] = useState("");  
  const [state, setState] = useState("");  
  const [loadingWeather, setLoadingWeather] = useState(false); // Loading state for weather  
  
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
  
  const handleCityChange = (event) => {  
    setCity(event.target.value);  
  };  
  
  const handleStateChange = (event) => {  
    setState(event.target.value);  
  };  
  
  const handleSearch = () => {  
    requestWeather(city, state);  
  };  
  
  const columns = [  
    { key: "column1", name: "Timeframe", fieldName: "name", minWidth: 100, isResizable: true, styles: { cell: { fontSize: '18px' } } },  
    { key: "column2", name: "Temp. (C)", fieldName: "temperatureC", minWidth: 70, isResizable: true, styles: { cell: { fontSize: '18px' } } },  
    { key: "column3", name: "Temp. (F)", fieldName: "temperatureF", minWidth: 70, isResizable: true, styles: { cell: { fontSize: '18px' } } },  
    { key: "column4", name: "Summary", fieldName: "summary", minWidth: 200, isResizable: true, styles: { cell: { fontSize: '18px' } } },  
  ];  
  
  return (  
    <div className={classNames.container}>  
      <header className={classNames.header}>  
        <h1>Weather Forecast</h1>  
      </header>  
      <div className={classNames.section}>  
        <Stack tokens={{ childrenGap: 20 }} className={classNames.inputStack}>  
          <Label>Check the weather for your city</Label>  
          <Stack horizontal tokens={{ childrenGap: 10 }}>  
            <TextField label="City" value={city} onChange={handleCityChange} />  
            <TextField label="State" value={state} onChange={handleStateChange} />  
            <PrimaryButton  
              text="Get Weather"  
              onClick={handleSearch}  
              styles={{ root: classNames.button }} // Apply custom styles  
            />  
          </Stack>  
          {loadingWeather ? (  
            <Spinner size={SpinnerSize.large} />  
          ) : (  
            <DetailsList  
              items={forecasts}  
              columns={columns}  
              setKey="set"  
              layoutMode={DetailsListLayoutMode.justified}  
              className={classNames.list}  
            />  
          )}  
        </Stack>  
      </div>  
      <MyChatBot />  
    </div>  
  );  
};  
  
export default App;  

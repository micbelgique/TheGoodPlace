import {useEffect,useState} from 'react'
import axios from 'axios';
import Tooltip from '@mui/material/Tooltip';
import LinearProgress from '@mui/material/LinearProgress';
import Box from '@mui/material/Box';

function GoodPlace() {
    const [isLoading, setLoading] = useState(false)
    const[data,setData] = useState([])

    useEffect(() => {
      axios.get('https://goodplacewebservice20220714145722.azurewebsites.net/api/Rooms/ranking')
      .then(res => {
        setData(res.data.theGoodPlace)
        setLoading(true)
      })
      .catch(err => console.log(err))
    },[],
    {
      interval: 10_000,
    })
    
    
   
    const name = data.name
    const tempetature = data.temperature
    const humidity = data.humidity
    const luminosity = data.luminosity
    const image = data.pictureUrl
    const lastSync = data.lastSync
    const datepreFormate = new Date(lastSync)
    const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric', second: 'numeric' };
    const dateFormate = datepreFormate.toLocaleDateString(undefined, options);

    const divStyle = {
      backgroundImage: "url('"+image+"')",
      backgroundSize: 'cover',
      backgroundRepeat: 'no-repeat',
      backgroundPosition: 'center',
  };
  return (    
      
<div>
    <div className='title'>
    <h1>The Good Place</h1>
    </div>

    <Box hidden={isLoading} 
         sx={{ width: '100%',
         marginBottom:'1rem'}} >
      <LinearProgress />
    </Box>

    <Tooltip title={dateFormate}>
      <div className="main-container">
        <div className="cards">
         <div className="card card-1" style={divStyle}> 
          <h1 className="card__title">{name}</h1>
          <div className="card__body">
          <center>
          <table className='tableGoodplace'>
              <thead>
              <th>Temperature (C°)</th>
              <th>Luminosité (Lux)</th>
              <th>Humidite ( % )</th>
              </thead>
              <tbody>
              <td>{tempetature}</td>  
              <td>{luminosity}</td> 
              <td>{humidity}</td>
              </tbody>
          </table>
          </center>
        </div>
      </div> 
      </div>
      </div>
    </Tooltip>

</div>
  );
}

export default GoodPlace;

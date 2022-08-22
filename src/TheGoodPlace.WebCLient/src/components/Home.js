import {useEffect,useState} from 'react'
import axios from 'axios';
import Tooltip from '@mui/material/Tooltip';

function Home() {

    const[data,setData] = useState([])
    
    useEffect(() => {
      axios.get('https://goodplacewebservice20220714145722.azurewebsites.net/api/Rooms/ranking')
      .then(res => {
        setData(res.data.rooms)
      })
      .catch(err => console.log(err))
    },[],
    {
      interval: 10_000,
    })
 
     const value = data.map((data,index) => {
     const datepreFormate = new Date(data.lastSync)
     const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric', second: 'numeric' };
     const dateFormate = datepreFormate.toLocaleDateString(undefined, options);

       return(
      <>
         <tbody >
            <Tooltip title={dateFormate}>
            <td>{data.name}</td>
            </Tooltip>
            <td>{data.temperature}</td>
            <td>{data.luminosity}</td>
            <td>{data.humidity}</td>
         </tbody>
        </>
       )
     })
  return (   

<center>
    <center>
    <table>
    <thead>
        <th>Room</th>
        <th>Temperature (C°)</th>
        <th>Luminosité (Lux)</th>
        <th>Humidite ( % )</th>
    </thead>
        {value} 
    </table>
    </center>
</center>
 
  );
}

export default Home;

import {useEffect,useState} from 'react'
import axios from 'axios';


  
function Home() {

    
    const[data,setData] = useState([])
    useEffect(() => {
      axios.get('https://goodplacewebservice20220714145722.azurewebsites.net/api/Rooms/ranking')
      .then(res => {
        setData(res.data.rooms)
      })
      .catch(err => console.log(err))
    },[])
 
     const value = data.map((data,index) => {
       return(
         <tbody>
         <td>{data.name}</td>
           {/* Temperature  */}
                 <td>{data.temperature}</td>
           {/* Luminosity */}
                 <td>{data.luminosity}</td>
           {/* Presence */}
                 {/* <td>In the Future</td> */}
           {/* Humidite */}
                 <td>{data.humidity}</td>
         </tbody>
       )
     })


    






  return (    
<center>
  

      {/* Device name list */}
    <h1></h1>
    <center>
    <table>
    <thead>
            <th>Room</th>
            <th>Temperature (C°)</th>
            <th>Luminosité (Lux)</th>
            {/* <th>Presence (sec)</th> */}
            <th>Humidite ( % )</th>
    </thead>
      {value} 
    </table>
    </center>
</center>
 
  );
}

export default Home;

import {useEffect,useState} from 'react'
import axios from 'axios';


  
function GoodPlace() {
 
    const[data,setData] = useState([])
    useEffect(() => {
      axios.get('https://goodplacewebservice20220714145722.azurewebsites.net/api/Rooms/ranking')
      .then(res => {
        setData(res.data.theGoodPlace)
      })
      .catch(err => console.log(err))
    },[])
 
    const name = data.name
    const tempetature = data.temperature
    const humidity = data.humidity
    const luminosity = data.luminosity

  return (    
<center>


    <div className='title'>
    <h1>The Good Place</h1>
    </div>
      <div class="main-container">
        <div class="cards">
         <div class="card card-1">
         <h1 class="card__title">{name}</h1>
         <div class="card__body">
         <center>
         <table className='tableGoodplace'>
            <thead>
            <th>Temperature (C°)</th>
            <th>Luminosité (Lux)</th>
            {/* <th>Presence (sec)</th> */}
            <th>Humidite ( % )</th>
            </thead>
            <tbody>
              {/* Temperature  */}
                    <td>{tempetature}</td>
              {/* Luminosity */}
                    <td>{luminosity}</td>
              {/* Presence */}
                    {/* <td>3</td> */}
              {/* Humidite */}
                    <td>{humidity}</td>
            </tbody>
        </table>
        </center>
        </div>
        </div>
      </div>
      </div>

</center>
 
  );
}

export default GoodPlace;

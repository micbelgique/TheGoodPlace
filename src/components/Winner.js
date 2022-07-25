import {useEffect,useState} from 'react'
import axios from 'axios';


  
function Winner() {


    const[data,setData] = useState([])
    useEffect(() => {
      axios.get('https://goodplacewebservice20220714145722.azurewebsites.net/api/Data/goodPlace')
      .then(res => {
        console.log("",res.data)
        setData(res.data)
      })
      .catch(err => console.log(err))
    },[])
 
    //Convert device name en list 
    var convertIntoArray = [];
    for (var i = 0; i < data.length; i++) {
       convertIntoArray.push(data[i].deviceName);   
    }
    //convert value en list
    var calcul = [];
    for (var j = 0; j < data.length; j++) {
        calcul.push(data[j].value);
        
    }
    //calcul perfect temperature
    for (var t = 0; t < data.length; t++) {
      calcul[t]=calcul[t]-22;
    }
    var temp = (Math.min(...calcul));
    var bestplace =  (calcul.indexOf(temp));
    const alpha = convertIntoArray[bestplace] 

  return (    

  <center>
    <h1>The Good Place</h1>
      <div class="main-container">
        <div class="cards">
         <div class="card card-1">
         <h1 class="card__title">{alpha}</h1>
        </div>
      </div>
      </div>
   </center>
 
  );
}

export default Winner;

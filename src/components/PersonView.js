import {useEffect,useState} from 'react'
import axios from 'axios';





  
function Coffee() {
  const[data,setData] = useState([])
  useEffect(() => {
    axios.get('https://goodplacewebservice20220714145722.azurewebsites.net/api/Data/lastRecords/70B3D547501000B5/')
    .then(res => {
      console.log("",res.data)
      setData(res.data)
    })
    .catch(err => console.log(err))
  },[])

const title = data.map((data,index) => {
  return(
          <tr>
            <th>{data.container}</th>
          </tr>
  )
})

const value = data.map((data,index) => {
    return(
            <tr>
              <th>{data.value}</th>
            </tr>
    )
  })

  return (
    <div className="Coffee">
        <h1>Person View</h1>
        <center>
        <table>
    <thead>
            <th colspan="2"></th>
    </thead>
    <tbody>
            <td>{title}</td>
            <td>{value}</td>
    </tbody>
</table>
</center>
    </div>
  );
}

export default Coffee;

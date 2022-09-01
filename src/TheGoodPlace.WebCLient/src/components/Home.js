import {useEffect,useState} from 'react'
import axios from 'axios';
import Tooltip from '@mui/material/Tooltip';
import Modal from '@mui/material/Modal';
import Button from '@mui/material/Button';
import CloseFullscreenIcon from '@mui/icons-material/CloseFullscreen';
import Box from '@mui/material/Box';
import MeetingRoomIcon from '@mui/icons-material/MeetingRoom';
function Home() {
    
    const [open, setOpen] =  useState();
    // const handleOpen = () => setOpen(true);
    const handleClose = () => setOpen(false);
    const[data,setData] = useState([]);
    
    useEffect(() => {
      axios.get('https://goodplacewebservice20220714145722.azurewebsites.net/api/Rooms/ranking')
      .then(res => {
        setData(res.data.rooms)
      })
      .catch(err => console.log(err))
    },[data])
 
     const value = data.map((data,index) => {
     const datepreFormate = new Date(data.lastSync)
     const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric', hour: 'numeric', minute: 'numeric', second: 'numeric' };
     const dateFormate = datepreFormate.toLocaleDateString(undefined, options);
    //  const image = data.pictureUrl
   


     const style = {
      // backgroundImage: "url('"+image+"')",
      width:"75%",
      height:"75%",
      margin:"auto",
      marginTop:"5%",
      backgroundSize: 'cover',
      backgroundRepeat: 'no-repeat',
      color:'white', 
    };

    const OpenModal = (data) => {
      const image=data.pictureUrl
      console.log(image)
      setOpen(true);
    }

       return(
      <>
        <Modal open={open}>
          <Box sx={style}>
            <Button variant="secondary" onClick={handleClose}>
              <CloseFullscreenIcon style={{ 'color': "white" }}/>
            </Button>
          {/* {image} */}
          </Box>
        </Modal>
         <tbody>
            <Tooltip title={dateFormate}>
            <td>{data.name}</td>
            </Tooltip>
            <td>{data.temperature}</td>
            <td>{data.luminosity}</td>
            <td>{data.humidity}</td>
            <td><Button onClick={()=>{OpenModal(data)}}><MeetingRoomIcon style={{ 'color': "black" }} ></MeetingRoomIcon></Button></td>
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
            <th>Vision</th>
        </thead>
            {value} 
        </table>
        </center>
    </center>
 
  );
}

export default Home;

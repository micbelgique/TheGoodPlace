import { useEffect, useState } from "react";
import axios from "axios";
import Tooltip from "@mui/material/Tooltip";
import Modal from "@mui/material/Modal";
import Button from "@mui/material/Button";
import CloseFullscreenIcon from "@mui/icons-material/CloseFullscreen";
import Box from "@mui/material/Box";

import RemoveRedEyeIcon from "@mui/icons-material/RemoveRedEye";
function Home() {
  const [open, setOpen] = useState();
  const handleClose = () => setOpen(false);
  const [data, setData] = useState([]);
  const [image, setImage] = useState();

  useEffect(() => {
    axios
      .get(
        "https://localhost:7258/api/Rooms/ranking"
      )
      .then((res) => {
        setData(res.data.rooms);
      })
      .catch((err) => console.log(err));
  }, []);

  const OpenModal = (data) => {
    setImage("https://www.mic-belgique.be/rooms/" + data.name);
    setOpen(true);
  };

  const value = data.map((data, index) => {
    // const datepreFormate = new Date(data.lastSync);
    // const options = {
    //   weekday: "long",
    //   year: "numeric",
    //   month: "long",
    //   day: "numeric",
    //   hour: "numeric",
    //   minute: "numeric",
    //   second: "numeric",
    // };
    // const dateFormate = datepreFormate.toLocaleDateString(undefined, options);
    // const image = "theGoodPlace" + data.name;

    const style = {
      backgroundImage: "url('" + image + "')",
      width: "75%",
      height: "75%",
      margin: "auto",
      marginTop: "5%",
      backgroundSize: "cover",
      backgroundRepeat: "no-repeat",
      color: "white",
    };

    return (
      <>
        <Modal open={open}>
          <Box sx={style}>
            <Button variant="secondary" onClick={handleClose}>
              <CloseFullscreenIcon style={{ color: "red" }} />
            </Button>
          </Box>
        </Modal>
        <tbody>
          <Tooltip title={data.justification}>
            <td>{data.name}</td>
          </Tooltip>
          <td>{data.temperature}</td>
          <td>{data.luminosity}</td>
          <td>{data.humidity}</td>

          <td>
            <Button
              onClick={() => {
                OpenModal(data);
                console.log(image);
              }}
            >
              <RemoveRedEyeIcon style={{ color: "black" }}></RemoveRedEyeIcon>
            </Button>
          </td>
        </tbody>
      </>
    );
  });
  return (
    <center>
      <center>
        <table>
          <thead>
            <th>Salle</th>
            <th>Temperature (C°)</th>
            <th>Luminosité (Lux)</th>
            <th>Humidite ( % )</th>
            <th>Aperçu</th>
          </thead>
          {value}
        </table>
      </center>
    </center>
  );
}

export default Home;

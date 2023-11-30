import * as React from "react";
import { useEffect, useState } from "react";
import axios from "axios";
import Tooltip from "@mui/material/Tooltip";
import LinearProgress from "@mui/material/LinearProgress";
import Box from "@mui/material/Box";
import Modal from "@mui/material/Modal";
import Button from "@mui/material/Button";
import CloseFullscreenIcon from "@mui/icons-material/CloseFullscreen";
import GoodPlaceAI from "./GoodPlaceAI";
import Prompt from "./Prompt";
function GoodPlace() {
  const [isLoading, setLoading] = useState(false);
  const [data, setData] = useState([]);

  useEffect(() => {
    axios
      .get(
        "https://localhost:7258/api/Rooms/ranking"
      )
      .then((res) => {
        setData(res.data.theGoodPlace);
        setLoading(true);
      })
      .catch((err) => console.log(err));
  }, [data]);

  const name = data.name;
  const tempetature = data.temperature;
  const humidity = data.humidity;
  const luminosity = data.luminosity;
  const image = "https://www.mic-belgique.be/rooms/" + data.name;

  const lastSync = data.lastSync;
  const datepreFormate = new Date(lastSync);
  const options = {
    weekday: "long",
    year: "numeric",
    month: "long",
    day: "numeric",
    hour: "numeric",
    minute: "numeric",
    second: "numeric",
  };
  const dateFormate = datepreFormate.toLocaleDateString(undefined, options);

  const divStyle = {
    backgroundImage: "url('" + image + "')",
    backgroundSize: "cover",
    backgroundRepeat: "no-repeat",
    backgroundPosition: "center",
  };

  const style = {
    backgroundImage: "url('" + image + "')",
    width: "75%",
    height: "75%",
    margin: "auto",
    marginTop: "5%",
    backgroundSize: "cover",
    backgroundRepeat: "no-repeat",
  };

  const [open, setOpen] = useState();
  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  return (
    <div>
      <div className="title">
        <h1>The Good Place</h1>
      </div>
      
      <Box hidden={isLoading} sx={{ width: "100%", marginBottom: "1rem" }}>
        <LinearProgress />
      </Box>
    
      {/* Loader */}
      <Modal open={open}>
        <Box sx={style}>
          <Button variant="secondary" onClick={handleClose}>
            <CloseFullscreenIcon style={{ color: "red" }} />
          </Button>
        </Box>
      </Modal>

      {/* <Prompt/> */}

      <Tooltip title="The GoodPlace" onClick={handleOpen}>
        <div className="main-container">
          <div className="cards">
            <div className="card card-1" style={divStyle}>
              <h1 className="card__title">
                {name}
                <GoodPlaceAI />
              </h1>

              <div className="card__body">
                <center>
                  <table className="tableGoodplace">
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

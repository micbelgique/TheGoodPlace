import * as React from "react";
import { useEffect, useState } from "react";
import axios from "axios";
import Tooltip from "@mui/material/Tooltip";

function GoodPlaceAI() {
  const [justifications, setJustifications] = useState([]);

  useEffect(() => {
    axios
      .get(
        "https://goodplacewebservice20220714145722.azurewebsites.net/api/Rooms/ranking"
      )
      .then((res) => {
        setJustifications(res.data.theGoodPlace.justification); 
      })
      .catch((err) => console.log(err));
      console.log(justifications);
  }, []);

  return (
    <div>
      <Tooltip>
        <div className="main-container">
          <div className="subinfo"> {justifications}</div>
        </div>
      </Tooltip>
    </div>
  );
}

export default GoodPlaceAI;

import Tooltip from "@mui/material/Tooltip";
import Modal from "@mui/material/Modal";
import Button from "@mui/material/Button";
import CloseFullscreenIcon from "@mui/icons-material/CloseFullscreen";
import Box from "@mui/material/Box";
import RemoveRedEyeIcon from "@mui/icons-material/RemoveRedEye";
import { useEffect, useState } from "react";

interface Room {
  name: string;
  temperature: number;
  humidity: number;
  capacity: number;
  justification: string;
  pictureUrl: string;
}

function TableRoom() {
  const [open, setOpen] = useState<boolean>(false);
  const [rooms, setRooms] = useState<Room[]>([]);
  const [selectedRoomImage, setSelectedRoomImage] = useState<string>("");

  const handleClose = () => setOpen(false);

  useEffect(() => {
    fetch("https://goodplacewebservice20220714145722.azurewebsites.net/api/Rooms/other-rooms")
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then((data: Room[]) => {
        setRooms(data);
      })
      .catch((error) => {
        console.error("There was a problem fetching the data:", error);
      });
  }, []);

  const openModal = (imageUrl: string) => {
    setSelectedRoomImage(imageUrl);
    setOpen(true);
  };

  return (
    <div>
      <center>
        <table>
          <thead>
            <tr>
              <th>Salle</th>
              <th>Temperature (C°)</th>
              <th>Humidité (%)</th>
              <th>Capacité (place)</th>
              <th>Aperçu</th>
            </tr>
          </thead>
          <tbody>
            {rooms.map((room: Room, index: number) => (
              <tr key={index}>
                <Tooltip title={room.justification} key={index}>
                  <td>{room.name}</td>
                </Tooltip>
                <td>{room.temperature}</td>
                <td>{room.humidity}</td>
                <td>{room.capacity}</td>
                <td>
                  <Button onClick={() => openModal(room.pictureUrl)}>
                    <RemoveRedEyeIcon style={{ color: "black" }} />
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </center>

      <Modal open={open} onClose={handleClose}>
        <Box
          sx={{
            width: "75%",
            height: "75%",
            margin: "auto",
            marginTop: "5%",
            backgroundSize: "cover",
            backgroundRepeat: "no-repeat",
            color: "white",
            backgroundImage: `url('${selectedRoomImage}')`,
          }}
        >
          <Button onClick={handleClose}>
            <CloseFullscreenIcon style={{ color: "black" }} />
          </Button>
        </Box>
      </Modal>
    </div>
  );
}

export default TableRoom;

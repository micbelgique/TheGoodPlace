import React, { useState, useEffect } from 'react';
import Tooltip from '@mui/material/Tooltip';
import LinearProgress from '@mui/material/LinearProgress';
import Box from '@mui/material/Box';
import Modal from '@mui/material/Modal';
import Button from '@mui/material/Button';
import CloseFullscreenIcon from '@mui/icons-material/CloseFullscreen';

interface RoomData {
  name: string;
  capacity: number;
  wellnessValue: number;
  temperature: number;
  humidity: number;
  justification: string;
  pictureUrl: string;
}

const HighestRoom: React.FC = () => {
  const [roomData, setRoomData] = useState<RoomData | null>(null);
  const [isLoading, setLoading] = useState<boolean>(false);

  useEffect(() => {
    const fetchHighestRoom = async () => {
      try {
        const response = await fetch('https://goodplacewebservice20220714145722.azurewebsites.net/api/Rooms/highest-room');
        if (!response.ok) {
          throw new Error('Erreur lors de la récupération des données');
        }
        const data: RoomData = await response.json();
        setRoomData(data);
        setLoading(true);
      } catch (error) {
        console.error('Erreur lors de la récupération des données :', error);
      }
    };

    fetchHighestRoom();
  }, []);

  const renderRoomInfo = () => {
    if (roomData) {
      const { name, capacity, temperature, humidity, justification, pictureUrl } = roomData;
      return (
        <div>
          <div className="title">
            <h1>The Good Place</h1>
          </div>
          
          <Box hidden={!isLoading} sx={{ width: '100%', marginBottom: '1rem' }}>
            <LinearProgress />
          </Box>
        
          {/* Loader */}
          <Modal open={isLoading}>
            <Box
              style={{
                backgroundImage: `url('${pictureUrl}')`,
                width: '75%',
                height: '75%',
                margin: 'auto',
                marginTop: '5%',
                backgroundSize: 'cover',
                backgroundRepeat: 'no-repeat',
              }}
            >
              <Button onClick={() => setLoading(false)}>
                <CloseFullscreenIcon style={{ color: 'black' }} />
              </Button>
            </Box>
          </Modal>

          <Tooltip title="The GoodPlace" onClick={() => setLoading(true)}>
            <div className="main-container">
              <div className="cards">
                <div
                  className="card card-1"
                  style={{
                    backgroundImage: `url('${pictureUrl}')`,
                    backgroundSize: 'cover',
                    backgroundRepeat: 'no-repeat',
                    backgroundPosition: 'center',
                  }}
                >
                  <h1 className="card__title">
                    {name}
                    <div className="subinfo">{justification}</div>
                  </h1>

                  <div className="card__body">
                    <center>
                      <table className="tableGoodplace">
                        <thead>
                          <tr>
                            <th>Température (C°)</th>
                            <th>Humidité (%)</th>
                            <th>Capacité</th>
                            {/* <th>Justification</th>                           */}
                          </tr>
                        </thead>
                        <tbody>
                          <tr>                 
                            <td>{temperature}</td>
                            <td>{humidity}</td>
                            <td>{capacity}</td>
                            {/* <td>{justification}</td> */}
                          </tr>
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
    } else {
      return <p>Chargement en cours...</p>;
    }
  };

  return <div>{renderRoomInfo()}</div>;
};

export default HighestRoom;

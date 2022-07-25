import React from 'react';
import './App.css';
// import Nav from './Nav';
import Coffee from './components/Coffee';
import Coworking from './components/Coworking';
import Loft from './components/Loft';
import Cockpit from './components/Cockpit'
import PersonView from './components/PersonView';
import Winner from './components/Winner';






function App() {
  return (

    <div className="App">
      {/* <Nav/> */}
      <Winner/>
      <Coffee/>
      <Coworking/>
      <Loft/>
      <Cockpit/>
      <PersonView/> 
    </div>
  
    
  );
}

export default App;

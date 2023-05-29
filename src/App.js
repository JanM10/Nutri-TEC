import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { LoginNutri } from "./pages/LoginNutri";
import { LoginCliente } from "./pages/LoginCliente";
import { RegisterNutri } from "./pages/RegistroNutri";
import RegistroMedidas from "./pages/registroMedidas";


function App() {

  return (
    <>
    <Router>
        <Routes>
          <Route path="/" element={<LoginNutri />} /> {/* LoginNutri */}
          <Route path="/LoginCliente" element={<LoginCliente />} />
          <Route path="/RegisterNutri" element={<RegisterNutri />} />
          <Route path="/Medidas" element={<RegistroMedidas />} />
        </Routes>
      </Router>
    </>
  );
}

export default App;

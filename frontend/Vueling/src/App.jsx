import { useState } from "react";
// import "./App.css";
import Table from "./components/Table";
import { Administrative } from "./Administrative.jsx";
import Wrapper from "./components/Wrapper";
import useFetch from "./hooks/useFetch";

const url = "hola";

function App() {
  const { data, isLoading = false } = useFetch(url);

  return (
    <div className="dashboard">
      <Wrapper>
        <Table data={data} isLoading={isLoading} />
        <Administrative />
      </Wrapper>
    </div>
  );
}

export default App;

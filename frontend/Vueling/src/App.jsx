import { useState } from "react";
// import "./App.css";
import Table from "./components/Table";
import { Administrative } from "./Administrative.jsx";
import Wrapper from "./components/Wrapper";
import useFetch from "./hooks/useFetch";
import { DataTableContext } from "./DataTableContext";
const url = "hola";

function App() {
  // const { data, isLoading = false } = useFetch(url);

  const dataTable = useState(null);

  return (
    <DataTableContext.Provider value={dataTable}>
      <div className="dashboard">
        <Wrapper>
          <Table />
          <Administrative />
        </Wrapper>
      </div>
    </DataTableContext.Provider>
  );
}

export default App;

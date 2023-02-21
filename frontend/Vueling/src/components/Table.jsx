import React, { useEffect, useState } from "react";
import "./table.css";
import datatable from "../../output.json";
import DownloadButton from "./DownloadButton";
// import XLSX from "xlsx"; // Import XLSX library
import FilterTable from "./FilterTable";

export default function Table() {
  const [data, setData] = useState(datatable);
  const [sortKey, setSortKey] = useState(null);
  const [sortOrder, setSortOrder] = useState(1);
  const [filterHeader, setFilterHeader] = useState("");
  const [filterText, setFilterText] = useState("");

  const headers = Object.keys(data);
  const rows = Object.keys(data.Day).map((key) => {
    const row = {};
    headers.forEach((header) => {
      row[header] = data[header][key];
    });
    return row;
  });

  const handleSort = (key) => {
    if (sortKey === key) {
      setSortOrder(-sortOrder);
    } else {
      setSortKey(key);
      setSortOrder(1);
    }
  };

  const handleFilterHeader = (event) => {
    setFilterHeader(event.target.value);
  };

  const handleFilterText = (event) => {
    setFilterText(event.target.value);
  };

  const filteredRows = filterHeader
    ? rows.filter((row) =>
        row[filterHeader]
          .toString()
          .toLowerCase()
          .includes(filterText.toLowerCase())
      )
    : rows;

  const sortedRows = sortKey
    ? filteredRows.sort(
        (a, b) =>
          sortOrder *
          (a[sortKey] > b[sortKey] ? 1 : a[sortKey] < b[sortKey] ? -1 : 0)
      )
    : filteredRows;
  useEffect(() => {
    setData(data);
  }, [data]);

  return (
    <>
      <div className="buttonFilterContainer">
        <div>
          <FilterTable
            filterHeader={filterHeader}
            filterText={filterText}
            handleFilterHeader={handleFilterHeader}
            headers={headers}
            handleFilterText={handleFilterText}
          />
        </div>
        <DownloadButton headers={headers} sortedRows={sortedRows} />
      </div>

      <table>
        <thead>
          <tr>
            {headers.map((header) => (
              <th key={header} onClick={() => handleSort(header)}>
                {header}
                {sortKey === header && sortOrder === 1 && " ▲"}
                {sortKey === header && sortOrder === -1 && " ▼"}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {sortedRows.map((row, index) => (
            <tr key={index}>
              {headers.map((header) => (
                <td key={header}>{row[header]}</td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </>
  );
}

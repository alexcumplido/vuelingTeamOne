import React, { useEffect, useState } from "react";
import "./table.css";
import datatable from "../../output.json";
import DownloadButton from "./DownloadButton";
// import XLSX from "xlsx"; // Import XLSX library

export default function Table() {
  const [data, setData] = useState(datatable);
  const [sortKey, setSortKey] = useState(null);
  const [sortOrder, setSortOrder] = useState(1);
  const [filterText, setFilterText] = useState("");

  const headers = Object.keys(data);
  const rows = Object.keys(data.Day).map((key) => {
    const row = {};
    headers.forEach((header) => {
      row[header] = data[header][key];
    });
    console.log("row", row);
    console.log("headers", headers);
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

  const sortedRows = sortKey
    ? rows.sort(
        (a, b) =>
          sortOrder *
          (a[sortKey] > b[sortKey] ? 1 : a[sortKey] < b[sortKey] ? -1 : 0)
      )
    : rows;
  // data viene por props. Cada vez que cambia data poner un useEffect que setea la nueva data a la nueva data
  const filteredRows = filterText
    ? sortedRows.filter((row) =>
        headers.some((header) =>
          String(row[header]).toLowerCase().includes(filterText.toLowerCase())
        )
      )
    : sortedRows;
  useEffect(() => {
    setData(data);
  }, [data]);

  return (
    <>
      <DownloadButton headers={headers} sortedRows={sortedRows} />
      <div>
        <label htmlFor="filter">Filter:</label>
        <input
          type="text"
          id="filter"
          value={filterText}
          onChange={(e) => setFilterText(e.target.value)}
        />
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

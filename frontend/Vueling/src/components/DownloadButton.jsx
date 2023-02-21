export default function DownloadButton({ headers, sortedRows }) {
  const downloadCsv = () => {
    const csvContent =
      "data:text/csv;charset=utf-8," +
      headers.join(",") +
      "\n" +
      sortedRows
        .map((row) => headers.map((header) => row[header]).join(","))
        .join("\n");

    const encodedUri = encodeURI(csvContent);
    const link = document.createElement("a");
    link.setAttribute("href", encodedUri);
    link.setAttribute("download", "table.csv");
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };
  return <button onClick={downloadCsv}>Download as CSV</button>;
}

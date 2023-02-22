export default function FilterTable({
  filterHeader,
  filterText,
  handleFilterHeader,
  headers,
  handleFilterText,
}) {
  return (
    <div className="filterContainer">
      <label>
        <select value={filterHeader} onChange={handleFilterHeader}>
          <option value="">Choose a header</option>
          {headers.map((header) => (
            <option key={header} value={header}>
              {header}
            </option>
          ))}
        </select>
        <input
          type="text"
          value={filterText}
          onChange={handleFilterText}
          placeholder="Filter text"
        />
      </label>
    </div>
  );
}

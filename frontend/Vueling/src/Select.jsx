
export function Select(props) {
  return (
    <div className="control-wrapper">
      <label className="control-label" htmlFor={`${props.text}`}>
        {`${props.text}`}
      </label>
      <select
        className="control-select"
        disabled={props.disabled}
        id={`${props.text}`}
        value={props.value}
        onChange={(event) => props.onChange(event.target.value)}
      >
        <option value="">Any</option>
        {props.options &&
          props.options.map((element) => (
            <option key={element} value={element}>
              {element}
            </option>
          ))}
      </select>
    </div>
  );
}
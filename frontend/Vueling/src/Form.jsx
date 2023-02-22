import { Select } from "./Select.jsx";
import { useContext, useState } from "react";
import { DataTableContext } from "./DataTableContext.js";

export function Form() {
  const [data, setData] = useContext(DataTableContext);

  const [fullJardinera, setFullJardinera] = useState("");
  const [partJardinera, setPartJardinera] = useState("");
  const [fullEquipaje, setFullEquipaje] = useState("");
  const [partEquipaje, setPartEquipaje] = useState("");
  const [fullCoordinacion, setFullCoordinacion] = useState("");
  const [partCoordinacion, setPartCoordinacion] = useState("");
  const [day, setDay] = useState("");

  function transformToJson(input) {
    const json = JSON.parse(input);
    const output = JSON.stringify(json).replace(/\\/g, "");
    return JSON.parse(output);
  }

  async function fetchData() {
    try {
      const response = await fetch("http://localhost:8000/getData", {
        method: "post",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          full_time_wage_jardinera: fullJardinera,
          part_time_wage_jardinera: partJardinera,
          full_time_wage_equipaje: fullEquipaje,
          part_time_wage_equipaje: partEquipaje,
          full_time_wage_coordinacion: fullCoordinacion,
          part_time_wage_coordinacion: partCoordinacion,
          day: day,
        }),
      });
      const constData = await response.json();
      setData(transformToJson(constData));
      //console.log("constData:::", constData);
    } catch (err) {
      console.log(err);
    }
  }

  function submit(event) {
    event.preventDefault();
    fetchData();
  }

  return (
    <section className="search container-standard">
      <form className="form">
        <label className={"control-label"} htmlFor={"full_time_wage_jardinera"}>
          {"Full time wage jardinera"}
          <input
            className={"control-select"}
            type={"number"}
            id={"full_time_wage_jardinera"}
            name={"full_time_wage_jardinera"}
            onChange={(event) => setFullJardinera(event.target.value)}
            value={fullJardinera}
            placeholder={"Full time wage jardinera"}
          />
        </label>
        <label className={"control-label"} htmlFor={"part_time_wage_jardinera"}>
          {"Part time wage jardinera"}
          <input
            className={"control-select"}
            type={"number"}
            id={"part_time_wage_jardinera"}
            name={"part_time_wage_jardinera"}
            onChange={(event) => setPartJardinera(event.target.value)}
            value={partJardinera}
            placeholder={"Part time wage jardinera"}
          />
        </label>

        <label className={"control-label"} htmlFor={"full_time_wage_equipaje"}>
          {"Full time wage equipaje"}
          <input
            className={"control-select"}
            type={"number"}
            id={"full_time_wage_equipaje"}
            name={"full_time_wage_equipaje"}
            onChange={(event) => setFullEquipaje(event.target.value)}
            value={fullEquipaje}
            placeholder={"Full time wage equipaje"}
          />
        </label>
        <label className={"control-label"} htmlFor={"part_time_wage_equipaje"}>
          {"Part time wage equipaje"}
          <input
            className={"control-select"}
            type={"number"}
            id={"part_time_wage_equipaje"}
            name={"part_time_wage_equipaje"}
            onChange={(event) => setPartEquipaje(event.target.value)}
            value={partEquipaje}
            placeholder={"Part time wage equipaje"}
          />
        </label>
        <label
          className={"control-label"}
          htmlFor={"full_time_wage_coordinacion"}
        >
          {"Full time wage coordinacion"}
          <input
            className={"control-select"}
            type={"number"}
            id={"full_time_wage_coordinacion"}
            name={"full_time_wage_coordinacion"}
            onChange={(event) => setFullCoordinacion(event.target.value)}
            value={fullCoordinacion}
            placeholder={"Full time wage coordinacion"}
          />
        </label>
        <label
          className={"control-label"}
          htmlFor={"part_time_wage_coordinacion"}
        >
          {"Part time wage coordinacion"}
          <input
            className={"control-select"}
            type={"number"}
            id={"part_time_wage_coordinacion"}
            name={"part_time_wage_coordinacion"}
            onChange={(event) => setPartCoordinacion(event.target.value)}
            value={partCoordinacion}
            placeholder={"Part time wage coordinacion"}
          />
        </label>
        <label className={"control-label"} htmlFor={"day"}>
          {"Day"}
          <input
            className={"control-select"}
            type={"date"}
            id={"day"}
            name={"day"}
            onChange={(event) => setDay(event.target.value)}
            value={day}
            placeholder={"Day"}
          />
        </label>
        <button className="control-select" disabled={false} onClick={submit}>
          Submit
        </button>
      </form>
    </section>
  );
}

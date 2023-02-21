import { Select } from "./Select.jsx";
import { useState} from "react";

export function Form (){
  const [fullJardinera, setFullJardinera] = useState("");
  const [partJardinera, setPartJardinera] = useState("");
  const [fullEquipaje, setFullEquipaje] = useState("");
  const [partEquipaje, setPartEquipaje] = useState("");
  const [fullCoordinacion, setFullCoordinacion] = useState("");
  const [partCoordinacion, setPartCoordinacion] = useState("");
  const [day, setDay] = useState("");
  
  async function fetchData(){
    try{
        const response = await fetch('fetchData', {
            method: 'put',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
              "full_time_wage_jardinera": fullJardinera,
              "part_time_wage_jardinera": partJardinera,
              "full_time_wage_equipaje": fullEquipaje,
              "part_time_wage_equipaje": partEquipaje,
              "full_time_wage_coordinacion": fullCoordinacion,
              "part_time_wage_coordinacion": partCoordinacion,
              "day": day,
          })
        })
    } catch(err){
        console.log(err)
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
          {"full_time_wage_jardinera"}
          <input
            className={"control-select"}
            type={"number"}
            id={"full_time_wage_jardinera"}
            name={"full_time_wage_jardinera"}
            onChange={(event) => setFullJardinera(event.target.value)}
            value={fullJardinera}
            placeholder={"full_time_wage_jardinera"}
          />
        </label>
        <label className={"control-label"} htmlFor={"part_time_wage_jardinera"}>
          {"part_time_wage_jardinera"}
          <input
            className={"control-select"}
            type={"number"}
            id={"part_time_wage_jardinera"}
            name={"part_time_wage_jardinera"}
            onChange={(event) => setPartJardinera(event.target.value)}
            value={partJardinera}
            placeholder={"part_time_wage_jardinera"}
          />
        </label>

        <label className={"control-label"} htmlFor={"full_time_wage_equipaje"}>
          {"full_time_wage_equipaje"}
          <input
            className={"control-select"}
            type={"number"}
            id={"full_time_wage_equipaje"}
            name={"full_time_wage_equipaje"}
            onChange={(event) => setFullEquipaje(event.target.value)}
            value={fullEquipaje}
            placeholder={"full_time_wage_equipaje"}
          />
        </label>
        <label className={"control-label"} htmlFor={"part_time_wage_equipaje"}>
          {"part_time_wage_equipaje"}
          <input
            className={"control-select"}
            type={"number"}
            id={"part_time_wage_equipaje"}
            name={"part_time_wage_equipaje"}
            onChange={(event) => setPartEquipaje(event.target.value)}
            value={partEquipaje}
            placeholder={"part_time_wage_equipaje"}
          />
        </label>
        <label className={"control-label"} htmlFor={"full_time_wage_coordinacion"}>
          {"full_time_wage_coordinacion"}
          <input
            className={"control-select"}
            type={"number"}
            id={"full_time_wage_coordinacion"}
            name={"full_time_wage_coordinacion"}
            onChange={(event) => setFullCoordinacion(event.target.value)}
            value={fullCoordinacion}
            placeholder={"full_time_wage_coordinacion"}
          />
        </label>
        <label className={"control-label"} htmlFor={"part_time_wage_coordinacion"}>
          {"part_time_wage_coordinacion"}
          <input
            className={"control-select"}
            type={"number"}
            id={"part_time_wage_coordinacion"}
            name={"part_time_wage_coordinacion"}
            onChange={(event) => setPartCoordinacion(event.target.value)}
            value={partCoordinacion}
            placeholder={"part_time_wage_coordinacion"}
          />
        </label>
        <label className={"control-label"} htmlFor={"day"}>
          {"day"}
          <input
            className={"control-select"}
            type={"date"}
            id={"day"}
            name={"day"}
            onChange={(event) => setDay(event.target.value)}
            value={day}
            placeholder={"day"}
          />
        </label>
        <button
            className="control-select"
            disabled={false}
            onClick={submit}
          >
        Submit
        </button>
      </form>
    </section>
  )
}


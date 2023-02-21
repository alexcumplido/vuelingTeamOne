import { Select } from "./Select.jsx";
import { useState} from "react";

export function Form (){
  const [PartTimeShiftDuration, setPartTimeShiftDuration] = useState("");
  const [jardineria, setJardineria] = useState("");
  const [equipaje, setEquipaje] = useState("");
  const [coordination, setCoordination] = useState("");
  const [jardineriaPartTime, setJardineriaPartTime] = useState("");
  const [equipajePartTime, setEquipajePartTime] = useState("");
  const [coordinationPartTime, setCoordinationPartTime] = useState("");
  const [scheludeOpen, setScheludeOpen] = useState("");
  const [scheludeClose, setScheludeClose] = useState("");
  
 
  async function fetchData(){
    try{
        const response = await fetch('fetchData', {
            method: 'put',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify({
              "PartTimeShiftDuration": PartTimeShiftDuration,
              "HandlingFunctionHourPrice": {
                  "FullTime": {
                      "Jardineria": jardineria,
                      "Equipaje": equipaje,
                      "Coordination": coordination
                  },
                  "PartTime": {
                      "Jardineria": jardineriaPartTime,
                      "Equipaje": equipajePartTime,
                      "Coordination": coordinationPartTime
                  }
              },
              "AirportSchedule": {
                  "Open": scheludeOpen,
                  "Close": scheludeClose
              }
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
        <label className={"control-label"} htmlFor={"PartTimeShiftDuration"}>
          {"Part TimeShift Duration"}
          <input
            className={"control-select"}
            type={"PartTimeShiftDuration"}
            id={"PartTimeShiftDuration"}
            name={"PartTimeShiftDuration"}
            onChange={(event) => setPartTimeShiftDuration(event.target.value)}
            value={PartTimeShiftDuration}
            placeholder={"Part Time Shift Duration Input"}
          />
        </label>
        <label className={"control-label"} tmlFor={"jardineria"}>
          {"Jardineria"}
          <input
            className={"control-select"}
            type={"number"}
            id={"jardineria"}
            name={"jardineria"}
            onChange={(event) => setJardineria(event.target.value)}
            value={jardineria}
            placeholder={"Jardineria Input"}
          />
        </label>

        <label className={"control-label"} htmlFor={"equipaje"}>
          {"equipaje"}
          <input
            className={"control-select"}
            type={"number"}
            id={"equipaje"}
            name={"equipaje"}
            onChange={(event) => setEquipaje(event.target.value)}
            value={equipaje}
            placeholder={"Equipaje Input"}
          />
        </label>
        <label className={"control-label"} htmlFor={"coordination"}>
          {"coordination"}
          <input
            className={"control-select"}
            type={"number"}
            id={"coordination"}
            name={"coordination"}
            onChange={(event) => setCoordination(event.target.value)}
            value={coordination}
            placeholder={"Coordination Input"}
          />
        </label>
        <label className={"control-label"} htmlFor={"jardineriaPartTime"}>
          {"Jardineria Part Time"}
          <input
            className={"control-select"}
            type={"number"}
            id={"jardineriaPartTime"}
            name={"jardineriaPartTime"}
            onChange={(event) => setJardineriaPartTime(event.target.value)}
            value={jardineriaPartTime}
            placeholder={"Jardineria Part Time Input"}
          />
        </label>
        <label className={"control-label"} htmlFor={"equipajePartTime"}>
          {"Equipaje Part Time"}
          <input
            className={"control-select"}
            type={"number"}
            id={"equipajePartTime"}
            name={"equipajePartTime"}
            onChange={(event) => setEquipajePartTime(event.target.value)}
            value={equipajePartTime}
            placeholder={"Equipaje Part Time input"}
          />
        </label>
        <label className={"control-label"} htmlFor={"coordinationPartTime"}>
          {"Coordination Part Time"}
          <input
            className={"control-select"}
            type={"number"}
            id={"coordinationPartTime"}
            name={"coordinationPartTime"}
            onChange={(event) => setCoordinationPartTime(event.target.value)}
            value={coordinationPartTime}
            placeholder={"Coordination Part Time Input"}
          />
        </label>

        <label className={"control-label"} htmlFor={"scheludeOpen"}>
          {"Schelude Open"}
          <input
            className={"control-select"}
            type='time' step="3600"
            id={"scheludeOpen"}
            name={"scheludeOpen"}
            onChange={(event) => setScheludeOpen(event.target.value)}
            value={scheludeOpen}
            placeholder={"schelude Open Input"}
          />
        </label>

        <label className={"control-label"} htmlFor={"scheludeClose"}>
          {"Schelude Close"}
          <input
            className={"control-select"}
            type='time' step="3600"
            id={"scheludeClose"}
            name={"scheludeClose"}
            onChange={(event) => setScheludeClose(event.target.value)}
            value={scheludeClose}
            placeholder={"Schelude Close Input"}
          />
        </label>
        <button
            disabled={false}
            onClick={submit}
          >
        Submit
        </button>
      </form>
    </section>
  )
}


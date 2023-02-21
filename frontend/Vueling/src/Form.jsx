import { Select } from "./Select.jsx";
import { useState, useEffect } from "react";



export function Form (){
    const [selectOne, setSelectOne] = useState("");
    const [selectTwo, setSelectTwo] = useState("");7
    const [selectThree, setSelectThree] = useState("");
    
    return <section className="search container-standard">
      <form className="form">
        <Select
          text={"selectOne"}
          disabled={false}
          value={selectOne}
          onChange={setSelectOne}
          options={["SelectOne1", "SelectOne2"]}
        />
        <Select
          text={"SelectTwo"}
          disabled={false}
          value={selectTwo}
          onChange={setSelectTwo}
          options={["SelectTwo1", "SelectTwo2"]}
        />
        <Select
          text={"SelectThree"}
          disabled={false}
          value={selectThree}
          onChange={setSelectThree}
          options={["SelectThree1", "SelectThree2"]}
        />
      </form>
    </section>
}
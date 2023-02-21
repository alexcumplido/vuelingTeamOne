from typing import Any

from fastapi import FastAPI, Request
from pydantic import BaseModel

app = FastAPI()


class Model(BaseModel):
    full_time_wage_jardinera : float
    part_time_wage_jardinera : float
    full_time_wage_equipaje : float
    part_time_wage_equipaje : float
    full_time_wage_coordinacion : float
    part_time_wage_coordinacion : float
    day : str


@app.post("/getData")
def read_data(model: Model):

    
    return {str(model)}
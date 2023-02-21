from typing import Any

from fastapi import FastAPI, Request
from pydantic import BaseModel

app = FastAPI()

class AirportSchedule(BaseModel):
    Open: str
    Close: str

class Time(BaseModel):
    Jardineria: int
    Equipaje: float
    Coordination: int

class HandlingFunctionHourPrice(BaseModel):
    FullTime: Time
    PartTime: Time

class Request(BaseModel):
    PartTimeShiftDuration: int
    HandlingFunctionHourPrice: HandlingFunctionHourPrice
    AirportSchedule: AirportSchedule


@app.post("/getData")
def read_data(request: Request):
    return request
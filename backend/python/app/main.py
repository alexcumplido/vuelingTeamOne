from typing import Union

from fastapi import FastAPI

app = FastAPI()

@app.get("/simple/{handling_area}/{date}")
def read_simple(handling_area: str, date: str):
    return {  }

@app.get("/allAreas/{date}")
def read_all_areas(date: str):
    return {}

@app.get("/week/{start_date}/{end_date}")
def read_all_areas_per_week(start_date: str, end_date: str):
    return {}
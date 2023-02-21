from typing import List

from fastapi import FastAPI

app = FastAPI()

@app.get("/getData/{parameters}")
def read_data(parameters: List[str]):
    return { 

     }
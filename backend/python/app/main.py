from typing import Any

from fastapi import FastAPI, Request
from pydantic import BaseModel

import pandas as pd
import numpy as np
import os
from pulp import *

COLUMN_NAMES = ['Day','Hour','Required employees']
COLUMN_NAMES_INPUT = ['Day','Hour','Handling function','Required employees']

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

    sdir = os.path.split(os.path.split(os.path.abspath(__file__))[0])[0]
    file_path_raw = os.path.join(sdir,'app', 'data', 'input-ground-handling-optimizer.csv')

    #clean data
    print('Cleaning data')
    df_raw = load_data_df(file_path_raw)
    df_input = process_input(df_raw)

    #input parameters
    full_time_wage_jardinera = model.full_time_wage_jardinera
    part_time_wage_jardinera = model.part_time_wage_jardinera
    full_time_wage_equipaje = model.full_time_wage_equipaje
    part_time_wage_equipaje = model.part_time_wage_equipaje
    full_time_wage_coordinacion = model.full_time_wage_coordinacion
    part_time_wage_coordinacion = model.part_time_wage_coordinacion
    day = model.day
    
    df_matrix_shifts = construct_matrix()
    df_input = df_input[df_input['Day'] == day].reset_index(drop=True)

    number_shifts = df_matrix_shifts.shape[1]
    number_windows = df_matrix_shifts.shape[0]
    total_number_windows = df_input.shape[0]

    #run model
    num_workers = optimization(df_input, day, full_time_wage_jardinera, part_time_wage_jardinera,
                               full_time_wage_equipaje, part_time_wage_equipaje, full_time_wage_coordinacion,
                               part_time_wage_coordinacion, df_matrix_shifts,
                               number_shifts, total_number_windows, number_windows)

    #construct final dataframe
    df_results = construct_results(df_input, num_workers[0], num_workers[1], num_workers[2], df_matrix_shifts, number_shifts, number_windows, total_number_windows,
                                   full_time_wage_jardinera, part_time_wage_jardinera, full_time_wage_equipaje, part_time_wage_equipaje, full_time_wage_coordinacion,
                                    part_time_wage_coordinacion)
    

    return df_results.to_json()

def load_data_df(file_path):
    df = pd.read_csv(file_path, sep=';')
    return df


def process_input(df_raw):

    df_input_jardinera = pd.DataFrame(columns=COLUMN_NAMES)
    df_input_equipaje = pd.DataFrame(columns=COLUMN_NAMES)
    df_input_coordinacion = pd.DataFrame(columns=COLUMN_NAMES)

    df_final_input = pd.DataFrame(columns=COLUMN_NAMES_INPUT)

    df_raw_jardinera = df_raw[df_raw['handling_function'] == 'JARDINERA'].reset_index(drop=True)
    df_raw_equipaje = df_raw[df_raw['handling_function'] == 'EQUIPAJES'].reset_index(drop=True)
    df_raw_coordinacion = df_raw[df_raw['handling_function'] == 'COORDINADOR'].reset_index(drop=True)

    labels = [f"{i}-{i+1}" for i in range(6,24)]

    hours_start_jardinera = df_raw_jardinera['ts_operation_start'].str.split(' ').str[1].str.split(':').str[0].astype(int)
    hours_end_jardinera = df_raw_jardinera['ts_operation_end'].str.split(' ').str[1].str.split(':').str[0].astype(int)

    hours_start_equipaje = df_raw_equipaje['ts_operation_start'].str.split(' ').str[1].str.split(':').str[0].astype(int)
    hours_end_equipaje = df_raw_equipaje['ts_operation_end'].str.split(' ').str[1].str.split(':').str[0].astype(int)

    hours_start_coordinacion = df_raw_coordinacion['ts_operation_start'].str.split(' ').str[1].str.split(':').str[0].astype(int)
    hours_end_coordinacion = df_raw_coordinacion['ts_operation_end'].str.split(' ').str[1].str.split(':').str[0].astype(int)

    df_raw_jardinera['ts_operation_start'] = pd.cut(hours_start_jardinera, range(6,25), labels=labels, right=False)
    df_raw_jardinera['ts_operation_end'] = pd.cut(hours_end_jardinera, range(6, 25), labels=labels, right=False)

    df_raw_equipaje['ts_operation_start'] = pd.cut(hours_start_equipaje, range(6,25), labels=labels, right=False)
    df_raw_equipaje['ts_operation_end'] = pd.cut(hours_end_equipaje, range(6, 25), labels=labels, right=False)

    df_raw_coordinacion['ts_operation_start'] = pd.cut(hours_start_coordinacion, range(6,25), labels=labels, right=False)
    df_raw_coordinacion['ts_operation_end'] = pd.cut(hours_end_coordinacion, range(6, 25), labels=labels, right=False)


    df_raw_jardinera = df_raw_jardinera.drop('cd_flight_number', axis=1)

    df_input_jardinera_startblock = df_raw_jardinera.groupby(['dt_flight', 'ts_operation_start'], as_index=False).sum()
    df_input_jardinera_endblock = df_raw_jardinera.groupby(['dt_flight', 'ts_operation_end'], as_index=False).sum()
    df_input_jardinera_startblock['required_employees'] = df_input_jardinera_startblock['required_employees'].replace(np.nan, 0)
    df_input_jardinera_endblock['required_employees'] = df_input_jardinera_endblock['required_employees'].replace(np.nan, 0)

    df_input_jardinera['Day'] = df_input_jardinera_startblock['dt_flight']
    df_input_jardinera['Hour'] = df_input_jardinera_startblock['ts_operation_start']
    df_input_jardinera['Required employees'] = df_input_jardinera_startblock['required_employees'] + df_input_jardinera_endblock['required_employees']
    df_input_jardinera['Required employees'] = df_input_jardinera['Required employees'].astype('int')
    df_input_jardinera['Handling function'] = 'JARDINERA'

    df_raw_equipaje = df_raw_equipaje.drop('cd_flight_number', axis=1)

    df_input_equipaje_startblock = df_raw_equipaje.groupby(['dt_flight', 'ts_operation_start'], as_index=False).sum()
    df_input_equipaje_endblock = df_raw_equipaje.groupby(['dt_flight', 'ts_operation_end'], as_index=False).sum()
    df_input_equipaje_startblock['required_employees'] = df_input_equipaje_startblock['required_employees'].replace(np.nan, 0)
    df_input_equipaje_endblock['required_employees'] = df_input_equipaje_endblock['required_employees'].replace(np.nan, 0)

    df_input_equipaje['Day'] = df_input_equipaje_startblock['dt_flight']
    df_input_equipaje['Hour'] = df_input_equipaje_startblock['ts_operation_start']
    df_input_equipaje['Required employees'] = df_input_equipaje_startblock['required_employees'] + df_input_equipaje_endblock['required_employees']
    df_input_equipaje['Required employees'] = df_input_equipaje['Required employees'].astype('int')
    df_input_equipaje['Handling function'] = 'EQUIPAJE'


    df_raw_coordinacion = df_raw_coordinacion.drop('cd_flight_number', axis=1)

    df_input_coordinacion_startblock = df_raw_coordinacion.groupby(['dt_flight', 'ts_operation_start'], as_index=False).sum()
    df_input_coordinacion_endblock = df_raw_coordinacion.groupby(['dt_flight', 'ts_operation_end'], as_index=False).sum()
    df_input_coordinacion_startblock['required_employees'] = df_input_coordinacion_startblock['required_employees'].replace(np.nan, 0)
    df_input_coordinacion_endblock['required_employees'] = df_input_coordinacion_endblock['required_employees'].replace(np.nan, 0)

    df_input_coordinacion['Day'] = df_input_coordinacion_startblock['dt_flight']
    df_input_coordinacion['Hour'] = df_input_coordinacion_startblock['ts_operation_start']
    df_input_coordinacion['Required employees'] = df_input_coordinacion_startblock['required_employees'] + df_input_coordinacion_endblock['required_employees']
    df_input_coordinacion['Required employees'] = df_input_coordinacion['Required employees'].astype('int')
    df_input_coordinacion['Handling function'] = 'COORDINACION'

    df_final_input = df_final_input.append(df_input_jardinera).append(df_input_equipaje).append(df_input_coordinacion)
    df_final_input.replace(to_replace=0, value=1, inplace=True)

    return df_final_input


def construct_matrix():
    zero_data = np.zeros(shape=(18, 17))
    df_matrix_shifts = pd.DataFrame(zero_data)
    df_matrix_shifts['Hour'] = [f"{i}-{i + 1}" for i in range(6, 24)]
    df_matrix_shifts = df_matrix_shifts.set_index('Hour')

    df_matrix_shifts[0][0] = 1
    df_matrix_shifts[0][1] = 1
    df_matrix_shifts[0][2] = 1
    df_matrix_shifts[0][3] = 1
    df_matrix_shifts[0][5] = 1
    df_matrix_shifts[0][6] = 1
    df_matrix_shifts[0][7] = 1
    df_matrix_shifts[0][8] = 1
    df_matrix_shifts[1][9] = 1
    df_matrix_shifts[1][10] = 1
    df_matrix_shifts[1][11] = 1
    df_matrix_shifts[1][12] = 1
    df_matrix_shifts[1][14] = 1
    df_matrix_shifts[1][15] = 1
    df_matrix_shifts[1][16] = 1
    df_matrix_shifts[1][17] = 1

    k = 0
    for i in range(2, 17):
        for j in range(0, 4):
            df_matrix_shifts[i][j + k] = 1
        k = k + 1

    return df_matrix_shifts


def optimization(df_input, day, full_time_wage_jardinera, part_time_wage_jardinera, full_time_wage_equipaje,
                 part_time_wage_equipaje, full_time_wage_coordinacion, part_time_wage_coordinacion, df_matrix_shifts,
                 number_shifts, total_number_windows, number_windows):

    df_input = df_input[df_input['Day'] == day].reset_index(drop=True)

    number_employees_required_per_time_window = df_input['Required employees'].values

    num_workers_jardinera = LpVariable.dicts("num_workers_jardinera", indices=list(range(number_shifts)), lowBound=0, cat="Integer")
    num_workers_equipaje = LpVariable.dicts("num_workers_equipaje", indices=list(range(number_shifts)), lowBound=0, cat="Integer")
    num_workers_coordinacion = LpVariable.dicts("num_workers_coordinacion", indices=list(range(number_shifts)), lowBound=0, cat="Integer")

    problem = LpProblem("schedule_employees", LpMinimize)

    problem += lpSum([((num_workers_jardinera[j] * full_time_wage_jardinera) + (num_workers_equipaje[j] * full_time_wage_equipaje) + (num_workers_coordinacion[j] * full_time_wage_coordinacion)) for j in range(0,1)] +
                     [((num_workers_jardinera[i] * part_time_wage_jardinera) + (num_workers_equipaje[i] * part_time_wage_equipaje) + (num_workers_coordinacion[i] * part_time_wage_coordinacion)) for i in range(2,17)])

    for t in range(total_number_windows):
        if t < number_windows:
            problem += lpSum([df_matrix_shifts[j][t] * num_workers_jardinera[j] for j in range(number_shifts)]) >= number_employees_required_per_time_window[t]

        elif (t >= number_windows) and (t < (2 * number_windows)):
            problem += lpSum([df_matrix_shifts[j][t-number_windows] * num_workers_coordinacion[j] for j in range(number_shifts)]) >= number_employees_required_per_time_window[t]

        else:
            problem += lpSum([df_matrix_shifts[j][t-(2*number_windows)] * num_workers_equipaje[j] for j in range(number_shifts)]) >= number_employees_required_per_time_window[t]

    problem.solve()
    print("Status:", LpStatus[problem.status])

    return [num_workers_jardinera, num_workers_equipaje, num_workers_coordinacion]


def construct_results(df_input, num_workers_jardinera, num_workers_equipaje, num_workers_coordinacion, df_matrix_shifts, number_shifts, number_windows, total_number_windows, full_time_wage_jardinera, part_time_wage_jardinera, full_time_wage_equipaje, part_time_wage_equipaje, full_time_wage_coordinacion,
                      part_time_wage_coordinacion):

    df_output = df_input
    df_output['Full-time Employees'] = 0
    df_output['Part-time Employees'] = 0
    df_output['Full-time Employees cost'] = 0
    df_output['Part-time Employees cost'] = 0

    for shift in range(number_shifts):
        for window in range(number_windows):
            if df_matrix_shifts[shift][window] == 1 and shift < 2:
                df_output['Full-time Employees'][window] += int(num_workers_jardinera[shift].value())
                df_output['Full-time Employees'][window+number_windows] += int(num_workers_equipaje[shift].value())
                df_output['Full-time Employees'][window+2*number_windows] += int(num_workers_coordinacion[shift].value())
            elif df_matrix_shifts[shift][window] == 1 and shift >= 2:
                df_output['Part-time Employees'][window] += int(num_workers_jardinera[shift].value())
                df_output['Part-time Employees'][window+number_windows] += int(num_workers_equipaje[shift].value())
                df_output['Part-time Employees'][window+2*number_windows] += int(num_workers_coordinacion[shift].value())

    df_output['Total Employees'] = df_output['Full-time Employees'] + df_output['Part-time Employees']

    for i in range (total_number_windows):
        if df_output['Handling function'][i] == 'JARDINERA':
            df_output['Full-time Employees cost'][i] = df_output['Full-time Employees'][i] * full_time_wage_jardinera
            df_output['Part-time Employees cost'][i] = df_output['Part-time Employees'][i] * part_time_wage_jardinera

        elif df_output['Handling function'][i] == 'EQUIPAJE':
            df_output['Full-time Employees cost'][i] = df_output['Full-time Employees'][i] * full_time_wage_equipaje
            df_output['Part-time Employees cost'][i] = df_output['Part-time Employees'][i] * part_time_wage_equipaje

        elif df_output['Handling function'][i] == 'COORDINACION':
            df_output['Full-time Employees cost'][i] = df_output['Full-time Employees'][i] * full_time_wage_coordinacion
            df_output['Part-time Employees cost'][i] = df_output['Part-time Employees'][i] * part_time_wage_coordinacion

    df_output['Total cost'] = df_output['Full-time Employees cost'] + df_output['Part-time Employees cost']

    return df_output

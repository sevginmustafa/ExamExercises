import * as api from './api.js';

export const login = api.login;
export const register = api.register;
export const logout = api.logout;

const endpoints = {
    allCars: '/data/cars?sortBy=_createdOn%20desc',
    car: '/data/cars/'
};

export async function getAllCars() {
    return api.get(endpoints.allCars);
}

export async function getCarById(id) {
    return api.get(endpoints.car + id);
}
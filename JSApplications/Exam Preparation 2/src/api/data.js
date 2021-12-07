import * as api from './api.js';

export const login = api.login;
export const register = api.register;
export const logout = api.logout;

const endpoints = {
    allCars: '/data/cars?sortBy=_createdOn%20desc',
    myCars: (userId) => `/data/cars?where=_ownerId%3D%22${userId}%22&sortBy=_createdOn%20desc`,
    search: (query) => `/data/cars?where=year%3D${query}`,
    car: '/data/cars/',
    create: '/data/cars',
    edit: '/data/cars/',
    delete: '/data/cars/',
};

export async function getAllCars() {
    return api.get(endpoints.allCars);
}

export async function getMyCars(userId) {
    return api.get(endpoints.myCars(userId));
}

export async function getCarById(id) {
    return api.get(endpoints.car + id);
}

export async function getSearchResults(query) {
    return api.get(endpoints.search(query));
}

export async function createCar(data) {
    return api.post(endpoints.create, data);
}

export async function editCar(id, data) {
    return api.put(endpoints.edit + id, data);
}

export async function deleteCar(id) {
    return api.del(endpoints.edit + id);
}
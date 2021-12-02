import * as api from './api.js';

export const login = api.login;
export const register = api.register;
export const logout = api.logout;

const endpoints = {
    catalogGames: '/data/games?sortBy=_createdOn%20desc',
    homeGames: '/data/games?sortBy=_createdOn%20desc&distinct=category',
    details: '/data/games/'
};

export async function getAllCatalogGames() {
    return api.get(endpoints.catalogGames);
}

export async function getAllHomeGames() {
    return api.get(endpoints.homeGames);
}

export async function getGameById(id) {
    return api.get(endpoints.details + id);
}
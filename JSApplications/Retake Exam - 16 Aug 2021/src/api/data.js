import * as api from './api.js';

export const login = api.login;
export const register = api.register;
export const logout = api.logout;

const endpoints = {
    catalogGames: '/data/games?sortBy=_createdOn%20desc',
    homeGames: '/data/games?sortBy=_createdOn%20desc&distinct=category',
    details: '/data/games/',
    create: '/data/games',
    edit: '/data/games/',
    delete: '/data/games/',
    gameComments: (gameId) => `/data/comments?where=gameId%3D%22${gameId}%22`,
    createComment: '/data/comments'
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

export async function createGame(data) {
    return api.post(endpoints.create, data);
}

export async function editGame(id, data) {
    return api.put(endpoints.edit + id, data);
}

export async function deleteGame(id) {
    return api.del(endpoints.delete + id);
}

export async function getGameComments(id) {
    return api.get(endpoints.gameComments(id));
}

export async function createGameComment(data) {
    return api.post(endpoints.createComment, data);
}
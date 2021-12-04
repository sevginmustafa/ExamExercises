import * as api from './api.js';

export const login = api.login;
export const register = api.register;
export const logout = api.logout;

const endpoints = {
    catalog: '/data/albums?sortBy=_createdOn%20desc&distinct=name',
    create: '/data/albums',
    edit: '/data/albums/',
    details: '/data/albums/',
    delete: '/data/albums/',
    search: (query) => `/data/albums?where=name%20LIKE%20%22${query}%22`
};

export async function getAllAlbums() {
    return api.get(endpoints.catalog);
}

export async function getAlbumById(id) {
    return api.get(endpoints.details + id);
}

export async function getSearchResults(query) {
    return api.get(endpoints.search(query));
}

export async function createAlbum(data) {
    return api.post(endpoints.create, data);
}

export async function editAlbum(id, data) {
    return api.put(endpoints.edit + id, data);
}

export async function deleteAlbum(id) {
    return api.del(endpoints.delete + id);
}
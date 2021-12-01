import * as api from './api.js';

export const login = api.login;
export const register = api.register;
export const logout = api.logout;

const endpoints = {
    create: '/data/books',
    all: '/data/books?sortBy=_createdOn%20desc',
    details: '/data/books/',
    edit: '/data/books/',
    delete: '/data/books/',
    myBooks: (userId) => `/data/books?where=_ownerId%3D%22${userId}%22&sortBy=_createdOn%20desc`,
    totalBookLikes: (bookId) => `/data/likes?where=bookId%3D%22${bookId}%22&distinct=_ownerId&count`,
    userLikes: (bookId, userId) => `/data/likes?where=bookId%3D%22${bookId}%22%20and%20_ownerId%3D%22${userId}%22&count`,
    likeBook: `/data/likes`
};

export async function getAllBooks() {
    return api.get(endpoints.all);
}

export async function getMyBooks(userId) {
    return api.get(endpoints.myBooks(userId));
}

export async function createBook(book) {
    return api.post(endpoints.create, book);
}

export async function editBook(bookId, book) {
    return api.put(endpoints.edit + bookId, book);
}

export async function deleteBook(bookId) {
    return api.del(endpoints.delete + bookId);
}

export async function getBookById(bookId) {
    return api.get(endpoints.details + bookId);
}

export async function likeBook(likeData) {
    return api.post(endpoints.likeBook, likeData);
}

export async function getBookLikes(bookId) {
    return api.get(endpoints.totalBookLikes(bookId));
}

export async function getUserLikes(bookId, userId) {
    return api.get(endpoints.userLikes(bookId, userId));
}
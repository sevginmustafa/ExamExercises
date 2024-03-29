export function getUserData() {
    return JSON.parse(sessionStorage.getItem('userData'));
}

export function setUserData(userData) {
    sessionStorage.setItem('userData', JSON.stringify(userData));
}

export function clearUserData() {
    sessionStorage.removeItem('userData');
}

export function updateUserNav() {
    if (getUserData()) {
        [...document.querySelectorAll('.user')].forEach(x => x.style.display = 'inline-block');
        [...document.querySelectorAll('.guest')].forEach(x => x.style.display = 'none');
    }
    else {
        [...document.querySelectorAll('.user')].forEach(x => x.style.display = 'none');
        [...document.querySelectorAll('.guest')].forEach(x => x.style.display = 'inline-block');
    }
}
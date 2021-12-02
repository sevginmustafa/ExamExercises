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
        document.getElementById('user').style.display = 'inline-block';
        document.getElementById('guest').style.display = 'none';
    }
    else {
        document.getElementById('user').style.display = 'none';
        document.getElementById('guest').style.display = 'inline-block';
    }
}
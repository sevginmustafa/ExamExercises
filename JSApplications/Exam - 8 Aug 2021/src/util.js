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
    const userData = getUserData()
    const userElement = document.getElementById('user');
    const guestElement = document.getElementById('guest');

    if (userData) {
        userElement.style.display = 'inline-block';
        guestElement.style.display = 'none';
        userElement.querySelector('span').textContent = `Welcome, ${userData.email}`;
    }
    else {
        userElement.style.display = 'none';
        guestElement.style.display = 'inline-block';
    }
}
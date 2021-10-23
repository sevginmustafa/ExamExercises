window.addEventListener('load', solve);

function solve() {
    const genre = document.getElementById('genre');
    const name = document.getElementById('name');
    const author = document.getElementById('author');
    const date = document.getElementById('date');
    const addButton = document.getElementById('add-btn');
    addButton.addEventListener('click', add);

    function add(ev) {
        ev.preventDefault();
        if (genre.value != '' && name.value != '' && author.value != '' && date.value != '') {
            const div = document.createElement('div');
            div.classList.add('hits-info');
            div.innerHTML = `<img src="./static/img/img.png">
                             <h2>Genre: ${genre.value}</h2>
                             <h2>Name: ${name.value}</h2>
                             <h2>Author: ${author.value}</h2>
                             <h3>Date: ${date.value}</h3>`;

            const saveButton = document.createElement('button');
            saveButton.classList.add('save-btn');
            saveButton.textContent = 'Save song';
            saveButton.addEventListener('click', saveSong);
            const likeButton = document.createElement('button');
            likeButton.classList.add('like-btn');
            likeButton.textContent = 'Like song';
            likeButton.addEventListener('click', likeSong);

            const deleteButton = document.createElement('button');
            deleteButton.classList.add('delete-btn');
            deleteButton.textContent = 'Delete';
            deleteButton.addEventListener('click', deleteSong);

            div.appendChild(saveButton);
            div.appendChild(likeButton);
            div.appendChild(deleteButton);

            document.getElementsByClassName('all-hits-container')[0].appendChild(div);

            genre.value = '';
            name.value = '';
            author.value = '';
            date.value = '';
        }
    }

    function saveSong(ev) {
        const divToMove = ev.target.parentNode;
        divToMove.getElementsByClassName('save-btn')[0].remove();
        divToMove.getElementsByClassName('like-btn')[0].remove();

        document.getElementsByClassName('saved-container')[0].appendChild(divToMove);
    }

    function likeSong(ev) {
        const likes = document.querySelector('#total-likes p');
        let currentLikes = Number(likes.textContent.replace('Total Likes: ', '')) + 1;
        likes.textContent = `Total Likes: ${currentLikes}`;

        ev.target.disabled = true;
    }

    function deleteSong(ev) {
        ev.target.parentNode.remove();
    }
}
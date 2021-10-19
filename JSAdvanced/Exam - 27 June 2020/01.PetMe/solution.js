function solve() {
    const inputs = document.getElementsByTagName('input');
    const name = inputs[0];
    const age = inputs[1];
    const kind = inputs[2];
    const currentOwner = inputs[3];

    const addButton = document.querySelector('#add button').addEventListener('click', add);

    function add(ev) {
        ev.preventDefault();
        if (name.value != '' &&
            age.value != '' &&
            kind.value != '' &&
            currentOwner.value != '' &&
            !isNaN(Number(age.value))) {

            const li = document.createElement('li');
            const p = document.createElement('p');
            p.innerHTML = `<strong>${name.value}</strong> is a <strong>${age.value}</strong> year old <strong>${kind.value}</strong>`;
            const span = document.createElement('span');
            span.textContent = `Owner: ${currentOwner.value}`;
            const button = document.createElement('button');
            button.textContent = 'Contact with owner';
            button.addEventListener('click', contactOwner);

            li.appendChild(p);
            li.appendChild(span);
            li.appendChild(button);

            document.querySelector('#adoption ul').appendChild(li);

            for (let input of inputs) {
                input.value = '';
            }
        }
    }

    function contactOwner(ev) {
        const currSection = ev.target.parentNode;
        ev.target.textContent = 'Yes! I take it!';
        ev.target.removeEventListener('click', contactOwner);
        ev.target.addEventListener('click', takePet);

        const div = document.createElement('div');
        const input = document.createElement('input');
        input.placeholder = 'Enter your names';

        div.appendChild(input);
        div.appendChild(ev.target);

        currSection.appendChild(div);
    }

    function takePet(ev) {
        const inputField = ev.target.parentNode.querySelector('input');

        if (inputField.value != '') {
            const liToMove = ev.target.parentNode.parentNode;

            const button = liToMove.querySelector('button');
            button.textContent = `Checked`;
            button.removeEventListener('click', takePet);
            button.addEventListener('click', check);

            liToMove.querySelector('span').textContent = `New Owner: ${inputField.value}`

            liToMove.appendChild(button);
            liToMove.querySelector('div').remove();

            document.querySelector('#adopted ul').appendChild(liToMove);
        }
    }

    function check(ev) {
        ev.target.parentNode.remove();
    }
}
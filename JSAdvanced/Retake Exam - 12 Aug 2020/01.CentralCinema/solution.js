function solve() {
    document.querySelector('#container button').addEventListener('click', onScreen);
    const inputs = document.getElementById('container').children;

    function onScreen(ev) {
        document.getElementById('archive').lastElementChild.addEventListener('click', clear);

        ev.preventDefault();
        let isFilled = true;

        for (let i = 0; i < inputs.length - 1; i++) {
            if (inputs[i].value == '') {
                isFilled = false;
            }
        }

        if (!isNaN(Number(inputs[2].value)) && isFilled) {
            let ul = document.querySelector('#movies ul');

            let li = document.createElement('li');

            let liSpan = document.createElement('span');
            liSpan.textContent = inputs[0].value;

            let liStrong = document.createElement('strong');
            liStrong.textContent = `Hall: ${inputs[1].value}`;

            let div = document.createElement('div');
            let divStrong = document.createElement('strong');
            divStrong.textContent = Number(inputs[2].value).toFixed(2);
            let divInput = document.createElement('input');
            divInput.placeholder = 'Tickets sold';
            let divButton = document.createElement('button');
            divButton.textContent = 'Archive';
            divButton.addEventListener('click', archive);
            div.appendChild(divStrong);
            div.appendChild(divInput);
            div.appendChild(divButton);

            li.appendChild(liSpan);
            li.appendChild(liStrong);
            li.appendChild(div);

            ul.appendChild(li);
        }

        for (let i = 0; i < inputs.length - 1; i++) {
            inputs[i].value = ''
        }
    }


    function archive(ev) {
        const input = ev.target.parentNode.querySelector('input[placeholder = "Tickets sold"]');

        if (!isNaN(Number(input.value)) && input.value != '') {
            let ul = document.querySelector('#archive ul');

            let li = document.createElement('li');

            let liSpan = document.createElement('span');
            liSpan.textContent = ev.target.parentNode.parentNode.querySelector('span').textContent;

            let liStrong = document.createElement('strong');
            liStrong.textContent = `Total amount: ${(Number(input.value) * Number(ev.target.parentNode.querySelector('strong').textContent)).toFixed(2)}`;

            let liButton = document.createElement('button');
            liButton.textContent = 'Delete';
            liButton.addEventListener('click', delElement);

            li.appendChild(liSpan);
            li.appendChild(liStrong);
            li.appendChild(liButton);

            ul.appendChild(li);

            input.value='';
        }
    }

    function delElement(ev) {
        ev.target.parentNode.remove();
    }

    function clear(ev) {
        const liElements = ev.target.parentNode.querySelectorAll('li');

        for (let liElement of liElements) {
            liElement.remove();
        }
    }
}
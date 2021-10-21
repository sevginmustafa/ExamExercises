window.addEventListener('load', solution);

function solution() {
  const fname = document.getElementById('fname');
  const email = document.getElementById('email');
  const phone = document.getElementById('phone');
  const address = document.getElementById('address');
  const code = document.getElementById('code');

  let data;

  const submitButton = document.getElementById('submitBTN');
  submitButton.addEventListener('click', submit);

  const editButton = document.getElementById('editBTN');
  editButton.addEventListener('click', edit);

  const continueButton = document.getElementById('continueBTN');
  continueButton.addEventListener('click', finishReservation);

  function submit() {
    if (fname.value != '' && email.value != '') {
      document.getElementById('infoPreview').innerHTML = `<li>Full Name: ${fname.value}</li>
                                                          <li>Email: ${email.value}</li>
                                                          <li>Phone Number: ${phone.value}</li>
                                                          <li>Address: ${address.value}</li>
                                                          <li>Postal Code: ${code.value}</li>`;

      submitButton.disabled = true;
      editButton.disabled = false;
      continueButton.disabled = false;

      data = {
        fname: fname.value,
        email: email.value,
        phone: phone.value,
        address: address.value,
        code: code.value
      };

      fname.value = '';
      email.value = '';
      phone.value = '';
      address.value = '';
      code.value = '';
    }
  }

  function edit() {
    const liElements = document.querySelectorAll('#infoPreview li');

    for (let liElement of liElements) {
      liElement.remove();
    }

    fname.value = data.fname;
    email.value = data.email;
    phone.value = data.phone;
    address.value = data.address;
    code.value = data.code;

    editButton.disabled = true;
    continueButton.disabled = true;
    submitButton.disabled = false;
  }

  function finishReservation() {
    document.getElementById('block').remove();

    const newDiv = document.createElement('div');
    newDiv.id = 'block';
    newDiv.innerHTML = '<h3>Thank you for your reservation!</h3>';

    document.body.appendChild(newDiv);
  }
}

import { getMyCars } from '../api/data.js';
import { html } from '../lib.js';
import { getUserData } from '../util.js';

const template = (cars) => html`
<section id="my-listings">
    <h1>My car listings</h1>
    <div class="listings">
        ${cars.length > 0 ? html`${cars.map(carCard)}` : html`<p class="no-cars"> You haven't listed any cars yet.</p>`}
    </div>
</section>`;

const carCard = (car) => html`
<div class="listing">
    <div class="preview">
        <img src="${car.imageUrl}">
    </div>
    <h2>${car.brand} ${car.model}</h2>
    <div class="info">
        <div class="data-info">
            <h3>Year: ${car.year}</h3>
            <h3>Price: ${car.price} $</h3>
        </div>
        <div class="data-buttons">
            <a href="/details/${car._id}" class="button-carDetails">Details</a>
        </div>
    </div>
</div>`;

export async function myCarsPage(ctx) {
    const userData = getUserData();
    const myCars = await getMyCars(userData.id);

    ctx.render(template(myCars));
}
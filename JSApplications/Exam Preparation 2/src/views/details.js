import { deleteCar, getCarById } from '../api/data.js';
import { html } from '../lib.js';
import { getUserData } from '../util.js';

const template = (car, isOwner, onDelete) => html`
<section id="listing-details">
    <h1>Details</h1>
    <div class="details-info">
        <img src="${car.imageUrl}">
        <hr>
        <ul class="listing-props">
            <li><span>Brand:</span>${car.brand}</li>
            <li><span>Model:</span>${car.model}</li>
            <li><span>Year:</span>${car.year}</li>
            <li><span>Price:</span>${car.price}$</li>
        </ul>

        <p class="description-para">${car.description}</p>

        ${isOwner 
            ? html`
                <div class="listings-buttons">
                    <a href="/edit/${car._id}" class="button-list">Edit</a>
                    <a @click=${onDelete} href="#" class="button-list">Delete</a>
                </div>` 
            : null}
    </div>
</section>`;

export async function detailsPage(ctx) {
    const userData = getUserData();
    const carId = ctx.params.id;
    const car = await getCarById(carId);
    const isOwner = userData && userData.id == car._ownerId;

    ctx.render(template(car, isOwner, onDelete));

    async function onDelete(event){
        event.preventDefault();

        const confirmed = confirm('Are you sure you want to delete this car?')

        if(confirmed){
            await deleteCar(carId);

            ctx.page.redirect('/catalog');
            ctx.updateUserNav();
        }
    }
}
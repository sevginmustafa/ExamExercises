import { getSearchResults } from '../api/data.js';
import { html } from '../lib.js';
import { getUserData } from '../util.js';

const template = (onSearch, albums) => html`
<section id="searchPage">
    <h1>Search by Name</h1>

    <div class="search">
        <input id="search-input" type="text" name="search" placeholder="Enter desired albums's name">
        <button @click=${onSearch} class="button-list">Search</button>
    </div>

    <h2>Results:</h2>
    ${albums ? html`
    <div class="search-result">
        ${albums.length > 0 ? html`${albums.map(albumcard)}` : html`<p class="no-result">No result.</p>`}
    </div>` : null}
</section>`;

const albumcard = (album) => html`
<div class="card-box">
    <img src="${album.imgUrl}">
    <div>
        <div class="text-center">
            <p class="name">Name: ${album.name}</p>
            <p class="artist">Artist: ${album.artist}</p>
            <p class="genre">Genre: ${album.genre}</p>
            <p class="price">Price: $${album.price}</p>
            <p class="date">Release Date: ${album.releaseDate}</p>
        </div>
        ${getUserData() ? html`
        <div class="btn-group">
            <a href="/details/${album._id}" id="details">Details</a>
        </div>` : null}
    </div>
</div>`;

export async function searchPage(ctx) {
    ctx.render(template(onSearch));

    async function onSearch() {
        const query = document.getElementById('search-input').value.trim();

        if (query == ``) {
            return alert('Search field is required!')
        }
        const albums = await getSearchResults(query);

        ctx.render(template(onSearch, albums));
    }
}
import { getGameById } from '../api/data.js';
import { html } from '../lib.js';
import { getUserData } from '../util.js';

const template = (game, isOwner) => html`
<section id="game-details">
    <h1>Game Details</h1>
    <div class="info-section">

        <div class="game-header">
            <img class="game-img" src="images/MineCraft.png" />
            <h1>${game.title}</h1>
            <span class="levels">MaxLevel: ${game.maxLevel}</span>
            <p class="type">${game.category}</p>
        </div>

        <p class="text">${game.summary}</p>

        <div class="details-comments">
            <h2>Comments:</h2>
            <ul>
                <li class="comment">
                    <p>Content: I rate this one quite highly.</p>
                </li>
                <li class="comment">
                    <p>Content: The best game.</p>
                </li>
            </ul>
            <p class="no-comment">No comments.</p>
        </div>

        ${isOwner 
            ? html`
                <div class="buttons">
                    <a href="#" class="button">Edit</a>
                    <a href="#" class="button">Delete</a>
                </div>` 
        : null}
    </div>

    <article class="create-comment">
        <label>Add new comment:</label>
        <form class="form">
            <textarea name="comment" placeholder="Comment......"></textarea>
            <input class="btn submit" type="submit" value="Add Comment">
        </form>
    </article>

</section>`;

export async function detailsPage(ctx) {
    const game = await getGameById(ctx.params.id);
    const userData = getUserData();
    const isOwner = userData && userData.id == game._ownerId

    ctx.render(template(game, isOwner));
}
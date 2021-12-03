import { createGameComment, deleteGame, getGameById, getGameComments } from '../api/data.js';
import { html } from '../lib.js';
import { getUserData } from '../util.js';

const template = (game, isOwner, onDelete, comments, onCreate) => html`
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
        ${comments.length > 0 
            ? html`
                <ul>
                    ${comments.map(commentCard)}
                </ul>` 
            : html`
                <p class="no-comment">No comments.</p>`
            }
        </div>

        ${isOwner 
            ? html`
                <div class="buttons">
                    <a href="/edit/${game._id}" class="button">Edit</a>
                    <a @click=${onDelete} href="" class="button">Delete</a>
                </div>` 
            : null}
    </div>
        ${!isOwner && getUserData() 
            ? html`
                <article class="create-comment">
                    <label>Add new comment:</label>
                    <form @submit=${onCreate} class="form">
                        <textarea name="comment" placeholder="Comment......"></textarea>
                        <input class="btn submit" type="submit" value="Add Comment">
                    </form>
                </article>` 
            : null}
</section>`;

const commentCard = (comment) => html`
<li class="comment">
    <p>Content: ${comment.comment}</p> 
</li>`;

export async function detailsPage(ctx) {
    const gameId = ctx.params.id;

    const game = await getGameById(gameId);
    const userData = getUserData();
    const isOwner = userData && userData.id == game._ownerId

    const comments = await getGameComments(gameId);

    ctx.render(template(game, isOwner, onDelete, comments, onCreate));

    async function onDelete(event){
        event.preventDefault();

        const confirmed = confirm('Are you sure you want to delete this game?');

        if(confirmed) {
            await deleteGame(gameId);
            ctx.page.redirect('/');
        }
    }
    
    async function onCreate(event){
        event.preventDefault();

        const comment = event.target.querySelector('textarea').value.trim();

        if(comment == '') {
            return alert('Comment field is required!')
        }

        try{
            await createGameComment({gameId, comment});
        }
        catch(error) {
            
        }

        event.target.reset();
        ctx.page.redirect(gameId);
    }
}
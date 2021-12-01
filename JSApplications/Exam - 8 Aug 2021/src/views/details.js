import { deleteBook, getAllBooks, getBookById, getBookLikes, likeBook } from '../api/data.js';
import { html } from '../lib.js';
import { getUserData } from '../util.js';

const template = (onDelete, book, isOwner, onLike) => html`
<section id="details-page" class="details">
    <div class="book-information">
        <h3>${book.title}</h3>
        <p class="type">Type: ${book.type}</p>
        <p class="img"><img src="${book.imageUrl}"></p>
        <div class="actions">
            ${isOwner 
                ? html`
                    <a class="button" href="/edit/${book._id}">Edit</a>
                    <a @click=${onDelete} class="button" href="#">Delete</a>`
                    : null
                }

            ${getUserData() && !isOwner ? html`<a @click=${onLike} class="button" href="#">Like</a>` : null}
                
            <div class="likes">
                <img class="hearts" src="/images/heart.png">
                <span id="total-likes">Likes: ${book.likes}</span>
            </div>
        </div>
    </div>
    <div class="book-description">
        <h3>Description:</h3>
        <p>${book.description}</p>
    </div>
</section>`;

export async function detailsPage(ctx) {
    const bookId = ctx.params.id;

    const book = await getBookById(bookId);
    const likes = await getBookLikes(bookId);
    book['likes'] = likes;

    const userData = getUserData();
    const isOwner = userData && userData.id == book._ownerId;

    ctx.render(template(onDelete, book, isOwner, onLike));

    async function onDelete(event){
        event.preventDefault();

        const confirmed = confirm('Are you sure you want to delete this book?');

        if (confirmed) {
            await deleteBook(bookId);

            ctx.updateUserNav();
            ctx.page.redirect('/');
        }
    }  
    
    async function onLike(event){
        event.preventDefault();

        await likeBook({bookId, ownerId: userData.id});

        ctx.page.redirect(bookId);
    }
}
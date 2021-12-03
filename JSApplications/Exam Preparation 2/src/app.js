import { render, page } from './lib.js';
import { updateUserNav } from './util.js';
import { logout } from './api/data.js';
import { createPage } from './views/create.js';
import { detailsPage } from './views/details.js';
import { editPage } from './views/edit.js';
import { homePage } from './views/home.js';
import { loginPage } from './views/login.js';
import { registerPage } from './views/register.js';
import { myCarsPage } from './views/myCars.js';
import { catalogPage } from './views/catalog.js';
import { searchPage } from './views/search.js';

const root = document.getElementById('site-content');
document.getElementById('logoutBtn').addEventListener('click', onLogout);

page(decorateContext);
page('/', homePage);
page('/login', loginPage);
page('/register', registerPage);
page('/details/:id', detailsPage);
page('/create', createPage);
page('/edit/:id', editPage);
page('/catalog', catalogPage);
page('/myCars', myCarsPage);
page('/search', searchPage);
page('/index.html', '/')

updateUserNav();
page.start();

function decorateContext(ctx, next) {
    ctx.render = (template) => render(template, root);
    ctx.updateUserNav = updateUserNav;
    next();
}

async function onLogout() {
    await logout();
    updateUserNav();
    page.redirect('/');
}

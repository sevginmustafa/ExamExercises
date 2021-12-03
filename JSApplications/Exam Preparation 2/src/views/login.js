import { login } from '../api/api.js';
import { html } from '../lib.js';

const template = (onLogin) => html`
<section id="login">
    <div class="container">
        <form @submit=${onLogin} id="login-form" action="#" method="post">
            <h1>Login</h1>
            <p>Please enter your credentials.</p>
            <hr>

            <p>Username</p>
            <input placeholder="Enter Username" name="username" type="text">

            <p>Password</p>
            <input type="password" placeholder="Enter Password" name="password">
            <input type="submit" class="registerbtn" value="Login">
        </form>
        <div class="signin">
            <p>Dont have an account?
                <a href="/register">Sign up</a>.
            </p>
        </div>
    </div>
</section>`;

export async function loginPage(ctx) {
    ctx.render(template(onLogin));

    async function onLogin(event) {
        event.preventDefault();

        const formData = new FormData(event.target);

        const username = formData.get('username').trim();
        const password = formData.get('password').trim();

        await login(username, password);

        ctx.page.redirect('/catalog');
        ctx.updateUserNav();
    }
}
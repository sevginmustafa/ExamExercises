class Restaurant {
    constructor(budgetMoney) {
        this.budgetMoney = budgetMoney;
        this.menu = {};
        this.stockProducts = {};
        this.history = [];
    }

    loadProducts(products) {
        for (const product of products) {
            let [productName, productQuantity, productTotalPrice] = product.split(' ');
            productQuantity = Number(productQuantity);
            productTotalPrice = Number(productTotalPrice);

            if (productTotalPrice <= this.budgetMoney) {
                if (this.stockProducts[productName] == undefined) {
                    this.stockProducts[productName] = productQuantity;
                }
                else {
                    this.stockProducts[productName]+= productQuantity;
                }

                this.budgetMoney -= productTotalPrice;
                this.history.push(`Successfully loaded ${productQuantity} ${productName}`);
            }
            else {
                this.history.push(`There was not enough money to load ${productQuantity} ${productName}`)
            }
        }

        return this.history.join('\n');
    }

    addToMenu(meal, neededProducts, price) {
        if (this.menu[meal] == undefined) {
            this.menu[meal] = {};
            this.menu[meal].price = Number(price);
            this.menu[meal].products = [];
            for (const product of neededProducts) {
                let [productName, productQuantity] = product.split(' ');
                productQuantity = Number(productQuantity);
                this.menu[meal].products.push({ productName, productQuantity });
            }

            if (Object.entries(this.menu).length == 1) {
                return `Great idea! Now with the ${meal} we have 1 meal in the menu, other ideas?`;
            }
            else {
                return `Great idea! Now with the ${meal} we have ${Object.entries(this.menu).length} meals in the menu, other ideas?`;
            }
        }

        return `The ${meal} is already in the our menu, try something different.`;
    }

    showTheMenu() {
        let result = '';
        if (Object.entries(this.menu).length > 0) {
            for (const meal in this.menu) {
                result += `${meal} - $ ${this.menu[meal].price}` + '\n';
            }
        }
        else {
            result = 'Our menu is not ready yet, please come later...';
        }

        return result.trim();
    }

    makeTheOrder(meal) {
        if (this.menu[meal] == undefined) {
            return `There is not ${meal} yet in our menu, do you want to order something else?`;
        }

        for (const product of this.menu[meal].products) {
            if (this.stockProducts[product.productName] == undefined ||
                this.stockProducts[product.productName] < product.productQuantity) {
                return `For the time being, we cannot complete your order (${meal}), we are very sorry...`;
            }
        }

        this.budgetMoney += this.menu[meal].price;
        for (const product of this.menu[meal].products) {
            this.stockProducts[product.productName] -= product.productQuantity;
        }

        return `Your order (${meal}) will be completed in the next 30 minutes and will cost you ${this.menu[meal].price}.`;
    }
}

let kitchen = new Restaurant(1000);
console.log(kitchen.loadProducts(['Banana 10 5', 'Banana 20 10', 'Strawberries 50 30', 'Yogurt 10 10', 'Yogurt 500 1500', 'Honey 5 50']));

console.log(kitchen.addToMenu('frozenYogurt', ['Yogurt 1', 'Honey 1', 'Banana 1', 'Strawberries 10'], 9.99));
console.log(kitchen.addToMenu('Pizza', ['Flour 0.5', 'Oil 0.2', 'Yeast 0.5', 'Salt 0.1', 'Sugar 0.1', 'Tomato sauce 0.5', 'Pepperoni 1', 'Cheese 1.5'], 15.55));

console.log(kitchen.showTheMenu());

kitchen.loadProducts(['Yogurt 30 3', 'Honey 50 4', 'Strawberries 20 10', 'Banana 5 1']);
kitchen.addToMenu('frozenYogurt', ['Yogurt 1', 'Honey 1', 'Banana 1', 'Strawberries 10'], 9.99);
console.log(kitchen.makeTheOrder('frozenYogurt'));

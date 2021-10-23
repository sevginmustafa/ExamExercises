const { assert, expect } = require('chai');
const { library } = require('./library');

describe('Library', () => {
    describe('calcPriceOfBook', () => {
        it('works properly only if nameOfBook is of type string and year an integer', () => {
            expect(library.calcPriceOfBook('Book', 1979)).to.equal('Price of Book is 10.00');
            expect(library.calcPriceOfBook('Book', 1980)).to.equal('Price of Book is 10.00');
            expect(library.calcPriceOfBook('Book', 1981)).to.equal('Price of Book is 20.00');
        })

        it('throws error if nameOfBook is not of type string or/and year is not an integer', () => {
            expect(() => library.calcPriceOfBook(1979, '1979')).to.throw(Error, 'Invalid input');
            expect(() => library.calcPriceOfBook('Book', '1980')).to.throw(Error, 'Invalid input');
            expect(() => library.calcPriceOfBook(1981, 1981)).to.throw(Error, 'Invalid input');
            expect(() => library.calcPriceOfBook('Book', 1981.50)).to.throw(Error, 'Invalid input');
        })
    })

    describe('findBook', () => {
        it('works properly only if bookArr is of type array and is not empty', () => {
            expect(library.findBook(['Book1', 'Book2'], 'Book2')).to.equal('We found the book you want.');
            expect(library.findBook(['Book1', 'Book2'], 'Book3')).to.equal('The book you are looking for is not here!');
        })

        it('throws error if bookArr is an empty array', () => {
            expect(() => library.findBook([], 'Book')).to.throw(Error, 'No books currently available');
        })
    })

    describe('arrangeTheBooks', () => {
        it('works properly only if the input is of type int', () => {
            expect(library.arrangeTheBooks(4)).to.equal('Great job, the books are arranged.');
            expect(library.arrangeTheBooks(40)).to.equal('Great job, the books are arranged.');
            expect(library.arrangeTheBooks(400)).to.equal('Insufficient space, more shelves need to be purchased.');
        })

        it('throws error if input is not of type int or is negative number', () => {
            expect(() => library.arrangeTheBooks('5')).to.throw(Error, 'Invalid input');
            expect(() => library.arrangeTheBooks(-5)).to.throw(Error, 'Invalid input');
        })
    })
})
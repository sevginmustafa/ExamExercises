const { assert, expect } = require('chai');
const { testNumbers } = require('./testNumbers');

describe('Test Numbers', () => {
    describe('Sum Numbers', () => {
        it('parameters should be of type number', () => {
            expect(testNumbers.sumNumbers(5, 5)).to.equal('10.00');
            expect(testNumbers.sumNumbers(-5, -5)).to.equal('-10.00');
            expect(testNumbers.sumNumbers(-5, 5)).to.equal('0.00');
            expect(testNumbers.sumNumbers(5.5, 5)).to.equal('10.50');
        });

        it('if parameters are not of type number result should be undefined', () => {
            expect(testNumbers.sumNumbers('5', 5)).to.equal(undefined);
            expect(testNumbers.sumNumbers('5', '5')).to.equal(undefined);
            expect(testNumbers.sumNumbers([], '5')).to.equal(undefined);
        });
    });

    describe('Number Checker', () => {
        it('parameter should be of type number or to be parsable to number', () => {
            expect(testNumbers.numberChecker(10)).to.equal('The number is even!');
            expect(testNumbers.numberChecker(11)).to.equal('The number is odd!');
            expect(testNumbers.numberChecker(-6)).to.equal('The number is even!');
            expect(testNumbers.numberChecker(-5)).to.equal('The number is odd!');
            expect(testNumbers.numberChecker(0)).to.equal('The number is even!');
            expect(testNumbers.numberChecker('5')).to.equal('The number is odd!');
        });

        it('if parameter is not of type number or can not be parsed to number error should be thrown', () => {
            expect(() => testNumbers.numberChecker([5, 5])).to.throw(Error, 'The input is not a number!');
            expect(() => testNumbers.numberChecker(['string'])).to.throw(Error, 'The input is not a number!');
        });
    });

    describe('Average Sum Array', () => {
        it('function calculates the average value of num elements in array', () => {
            expect(testNumbers.averageSumArray([10, 10, 10])).to.equal(10);
            expect(testNumbers.averageSumArray([11, 10])).to.equal(10.5);
            expect(testNumbers.averageSumArray([-6, -8, -7])).to.equal(-7);
        });
    });
});
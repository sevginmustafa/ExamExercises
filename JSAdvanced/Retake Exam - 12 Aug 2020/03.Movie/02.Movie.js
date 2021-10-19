class Movie {
    constructor(movieName, ticketPrice) {
        this.movieName = movieName;
        this.ticketPrice = Number(ticketPrice);
        this.screenings = [];
        this.soldTickets = 0;
    }

    newScreening(date, hall, description) {
        if (this.screenings.some(x => x.date == date && x.hall == hall)) {
            throw new Error(`Sorry, ${hall} hall is not available on ${date}`);
        }

        this.screenings.push({ date, hall, description });
        return `New screening of ${this.movieName} is added.`
    }

    endScreening(date, hall, soldTickets) {
        for (let i = 0; i < this.screenings.length; i++) {
            if (this.screenings[i].date == date && this.screenings[i].hall == hall) {
                this.soldTickets += soldTickets;
                this.screenings.splice(i, 1);
                return `${this.movieName} movie screening on ${date} in ${hall} hall has ended. Screening profit: ${soldTickets * this.ticketPrice}`;
            }
        }

        throw new Error(`Sorry, there is no such screening for ${this.movieName} movie.`);
    }

    toString() {
        let result = `${this.movieName} full information:` + '\n' +
            `Total profit: ${(this.soldTickets * this.ticketPrice).toFixed(0)}$` + '\n' +
            `Sold Tickets: ${this.soldTickets}`;

        if (this.screenings.length > 0) {
            result += '\n' + 'Remaining film screenings:';

            for (const screening of this.screenings.sort((a, b) => a.hall.localeCompare(b.hall))) {
                result += '\n' + `${screening.hall} - ${screening.date} - ${screening.description}`;
            }
        }
        else {
            result += '\n' + 'No more screenings!';
        }

        return result;
    }
}

let m = new Movie('Wonder Woman 1984', '10.00');
console.log(m.newScreening('October 2, 2020', 'IMAX 3D', `3D`));
console.log(m.newScreening('October 3, 2020', 'Main', `regular`));
console.log(m.newScreening('October 4, 2020', 'IMAX 3D', `3D`));
console.log(m.endScreening('October 2, 2020', 'IMAX 3D', 150));
console.log(m.endScreening('October 3, 2020', 'Main', 78));
console.log(m.toString());

m.newScreening('October 4, 2020', '235', `regular`);
m.newScreening('October 5, 2020', 'Main', `regular`);
m.newScreening('October 3, 2020', '235', `regular`);
m.newScreening('October 4, 2020', 'Main', `regular`);
console.log(m.toString());


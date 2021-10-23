class SummerCamp {
    constructor(organizer, location) {
        this.organizer = organizer;
        this.location = location;
        this.priceForTheCamp = { "child": 150, "student": 300, "collegian": 500 };
        this.listOfParticipants = [];
    }

    registerParticipant(name, condition, money) {
        if (this.priceForTheCamp[condition] == undefined) {
            throw new Error('Unsuccessful registration at the camp.');
        }

        if (this.listOfParticipants.some(x => x.name == name)) {
            return `The ${name} is already registered at the camp.`;
        }

        if (money < this.priceForTheCamp[condition]) {
            return `The money is not enough to pay the stay at the camp.`;
        }

        this.listOfParticipants.push({
            name,
            condition,
            power: 100,
            wins: 0
        });

        return `The ${name} was successfully registered.`;
    }

    unregisterParticipant(name) {
        let findParticipant = this.listOfParticipants.find(x => x.name == name);

        if (findParticipant == undefined) {
            throw new Error(`The ${name} is not registered in the camp.`);
        }

        let indexOfParticipant = this.listOfParticipants.indexOf(findParticipant)

        this.listOfParticipants.splice(indexOfParticipant, 1);
        return `The ${name} removed successfully.`;
    }

    timeToPlay(typeOfGame, ...participants) {
        for (const participant of participants) {
            if (!this.listOfParticipants.some(x => x.name == participant)) {
                throw new Error('Invalid entered name/s.');
            }
        }

        if (typeOfGame == 'WaterBalloonFights') {
            let participant1 = this.listOfParticipants.find(x => x.name == participants[0]);
            let participant2 = this.listOfParticipants.find(x => x.name == participants[1]);

            if (participant1.condition != participant2.condition) {
                throw new Error('Choose players with equal condition.');
            }

            if (participant1.power > participant2.power) {
                participant1.wins++;
                return `The ${participant1.name} is winner in the game ${typeOfGame}.`;
            }
            else if (participant2.power > participant1.power) {
                participant2.wins++;
                return `The ${participant2.name} is winner in the game ${typeOfGame}.`;
            }
            else {
                return 'There is no winner.';
            };
        }
        else {
            let participant = this.listOfParticipants.find(x => x.name == participants[0]);
            participant.power += 20;

            return `The ${participants[0]} successfully completed the game ${typeOfGame}.`;
        }
    }

    toString() {
        let result = `${this.organizer} will take ${this.listOfParticipants.length} participants on camping to ${this.location}`;

        for (const participant of this.listOfParticipants.sort((a, b) => b.wins - a.wins)) {
            result += '\n' + `${participant.name} - ${participant.condition} - ${participant.power} - ${participant.wins}`;
        }

        return result;
    }
}

const summerCamp = new SummerCamp("Jane Austen", "Pancharevo Sofia 1137, Bulgaria");
console.log(summerCamp.registerParticipant("Petar Petarson", "student", 300));
console.log(summerCamp.timeToPlay("Battleship", "Petar Petarson"));
console.log(summerCamp.registerParticipant("Sara Dickinson", "child", 200));
console.log(summerCamp.timeToPlay("WaterBalloonFights", "Petar Petarson", "Sara Dickinson"));
console.log(summerCamp.registerParticipant("Dimitur Kostov", "student", 300));
console.log(summerCamp.timeToPlay("WaterBalloonFights", "Petar Petarson", "Dimitur Kostov"));

console.log(summerCamp.toString());
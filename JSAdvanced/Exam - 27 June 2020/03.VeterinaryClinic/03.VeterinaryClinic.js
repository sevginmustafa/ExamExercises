class VeterinaryClinic {
    constructor(clinicName, capacity) {
        this.clinicName = clinicName;
        this.capacity = capacity;
        this.clients = [];
        this.totalProfit = 0;
        this.currentWorkload = 0;
    }

    newCustomer(ownerName, petName, kind, procedures) {
        if (this.currentWorkload < this.capacity) {
            let findClient = this.clients.find(x => x[ownerName] != undefined);

            if (findClient == undefined) {
                findClient = {};
                findClient[ownerName] = {};
                findClient[ownerName][petName] = { kind, procedures };
                this.clients.push(findClient);
            }
            else {
                if (findClient[ownerName][petName] != undefined && findClient[ownerName][petName].procedures.length > 0) {
                    throw new Error(`This pet is already registered under ${ownerName} name! ${petName} is on our lists, waiting for ${findClient[ownerName][petName].procedures.join(', ')}.`);
                }
                else {
                    findClient[ownerName][petName] = { kind, procedures };
                }
            }

            this.currentWorkload++;
            return `Welcome ${petName}!`;
        }

        throw new Error(`Sorry, we are not able to accept more patients!`);
    }

    onLeaving(ownerName, petName) {
        let findClient = this.clients.find(x => x[ownerName] != undefined);

        if (findClient == undefined) {
            throw new Error('Sorry, there is no such client!');
        }

        if (findClient[ownerName][petName] == undefined || findClient[ownerName][petName].procedures.length == 0) {
            throw new Error(`Sorry, there are no procedures for ${petName}!`);
        }

        this.totalProfit += 500 * findClient[ownerName][petName].procedures.length;
        this.currentWorkload--;
        findClient[ownerName][petName].procedures.length = 0;

        return `Goodbye ${petName}. Stay safe!`;
    }

    toString() {
        let result = `${this.clinicName} is ${Math.floor(this.currentWorkload / this.capacity * 100)}% busy today!` + '\n' +
            `Total profit: ${this.totalProfit.toFixed(2)}$`;

        for (const client of this.clients.sort((a, b) => Object.keys(a)[0].localeCompare(Object.keys(b)[0]))) {
            result += '\n' + `${Object.keys(client)} with:`;

            for (const pets of Object.values(client)) {

                for (const pet of Object.entries(pets).sort((a, b) => a[0].localeCompare(b[0]))) {
                    const temp = pet;
                    result += '\n' + `---${pet[0]} - a ${pet[1].kind.toLowerCase()} that needs: ${pet[1].procedures.join(', ')}`;
                }
            }
        }

        return result;
    }
}

let clinic = new VeterinaryClinic('SoftCare', 10);
console.log(clinic.newCustomer('Jim Jones', 'Tom', 'Cat', ['A154B', '2C32B', '12CDB']));
console.log(clinic.newCustomer('Anna Morgan', 'Max', 'Dog', ['SK456', 'DFG45', 'KS456']));
console.log(clinic.newCustomer('Jim Jones', 'Tiny', 'Cat', ['A154B']));
console.log(clinic.onLeaving('Jim Jones', 'Tiny'));
console.log(clinic.toString());
clinic.newCustomer('Jim Jones', 'Sara', 'Dog', ['A154B']);
console.log(clinic.toString());









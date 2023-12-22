export class Sensor {
    constructor(
        public id: number,
        public name: string,
        public valueTemp: number,
        public valueHumidity: number,
        public createdTemp: Date,
        public createdHumidity: Date) {
    }
}

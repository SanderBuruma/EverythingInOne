export class Microchip {
  /** safsdf */
  private _conversions: {to: number}[] = [];
  private _name = '';

  constructor(max: number, name: string, allowNonconversion = false) {

    // check parameters
    if (max < 2) { throw new Error('Microchip must have a max larger than 1'); }
    max = Math.floor(max);

    this._name = name;
    this.RandomizeConversions(max, allowNonconversion);
  }

  private RandomizeConversions(max: number, allowNonconversion: boolean) {
    const arr: number[] = [];
    for (let i = 0; i < max; i++) {
      arr.push(i);
    }
    for (let i = 0; i < max; i++) {
      let randomIndex = Math.floor(Math.random() * arr.length);
      while (!allowNonconversion && arr[randomIndex] === i) {
        randomIndex = Math.floor(Math.random() * arr.length);
      }

      this._conversions.push({to: arr[randomIndex]});
      arr.splice(randomIndex, 1);
    }
  }

  public ConvertSignal(signal: number) {
    return this._conversions[signal].to;
  }

  public get Name() {
    return this._name;
  }

}

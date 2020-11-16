export class Microchip {
  /** Should convert from index to value */
  private _conversions: number[] = [];
  private _name = '';
  private _conversionsRevealed = true;

  constructor(max: number, name: string, conversionsRevealed = true) {

    // check parameters
    if (max < 2) { throw new Error('Microchip must have a max larger than 1'); }
    max = Math.floor(max);

    this._name = name;
    this._conversionsRevealed = conversionsRevealed;
    this.RandomizeConversions(max);
  }

  private RandomizeConversions(max: number) {
    const arr: number[] = [];
    for (let i = 0; i < max; i++) {
      arr.push(i);
    }
    for (let i = 0; i < max; i++) {
      const randomIndex = Math.floor(Math.random() * arr.length);

      this._conversions.push(arr[randomIndex]);
      arr.splice(randomIndex, 1);
    }
  }


  public ConvertSignal(signal: number) {
    return this._conversions[signal];
  }


  public get ConversionsRevealed() {
    return this._conversionsRevealed;
  }

  public get Name() {
    return this._name;
  }

  public get Conversions() {
    return this._conversions;
  }

}

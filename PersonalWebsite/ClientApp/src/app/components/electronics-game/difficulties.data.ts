class Difficulty {

  /** Description of this difficulty setting */
  description: string;
  /** The maximum number of values that in and outputs can be. */
  max: number;
  /** The number of chips in any given row. */
  width: number;
  /** The number of chips in any given column. */
  height: number;
  /** Index of the chip which needs to be determined. */
  chipTBD: number;
  /** The indices of outputs chich shall not be shown. */
  unknownOutputs: number[];
  /** The indices of microchips of which conversion values are not revealed at the start. */
  unknownMicrochipConversions: number[];
  /** Which chip need to be copied from what. */
  chipCopies: ChipCopy[];

  constructor(_description: string,
    _max: number,
    _width: number,
    _height: number,
    _chipTBD: number,
    _unknownOutputs: number[],
    _unknownMicrochipConversions: number[],
    _chipCopies: ChipCopy[]
  ) {
    this.description = _description;
    this.max = _max;
    this.width = _width;
    this.height = _height;
    this.chipTBD = _chipTBD;
    this.unknownOutputs = _unknownOutputs;
    this.unknownMicrochipConversions = _unknownMicrochipConversions;
    this.chipCopies = _chipCopies;
  }
}

class ChipCopy {
  /** The index of the chip to be converted. */
  public index: number;
  /** The index of the chip to be copied. */
  public convertTo: number;
  constructor(_index: number, _convertTo: number) {
    this.index = _index;
    this.convertTo = _convertTo;
  }

}

export let difficulties: Difficulty[] = [
  new Difficulty(
    'easy-1',
    4,
    1,
    1,
    0,
    [],
    [],
    [],
  ),
  new Difficulty(
    'easy-2',
    4,
    1,
    2,
    1,
    [2],
    [],
    [],
  ),
  new Difficulty(
    'easy-3',
    4,
    1,
    3,
    2,
    [3],
    [],
    [],
  ),
  new Difficulty(
    'easy-4',
    4,
    1,
    3,
    0,
    [1, 2, 3],
    [],
    [],
  ),
  new Difficulty(
    'easy-5',
    4,
    2,
    2,
    3,
    [],
    [],
    [],
  ),

  new Difficulty(
    'easy-6',
    4,
    2,
    2,
    3,
    [],
    [1, 2],
    [
      new ChipCopy(
        3,
        2
      ),
      new ChipCopy(
        2,
        1
      ),
    ],
  ),

  new Difficulty(
    'medium-1',
    4,
    2,
    2,
    3,
    [1],
    [0, 2],
    [
      new ChipCopy(
        3,
        2
      ),
      new ChipCopy(
        2,
        0
      ),
    ],
  ),

  new Difficulty(
    'medium-2',
    4,
    3,
    3,
    4,
    [],
    [],
    [],
  ),

];

export let difficulties: {
  /** Description of this difficulty setting */
  description: string,
  /** The maximum number of values that in and outputs can be. */
  max: number,
  /** The number of chips in any given row or column. */
  width: number,
  /** Index of the chip which needs to be determined. */
  chipTBD: number,
  /** The indices of outputs chich shall not be shown. */
  unknownOutputs: number[],
  /** The indices of microchips of which conversion values are not revealed at the start. */
  unknownMicrochipConversions: number[],
  /** Which chip need to be copied from what. */
  chipCopies: {
    /** The index of the chip to be converted. */
    index: number,
    /** The index of the chip to be copied. */
    convertTo: number,
  }[],
}[] = [
  {
    description: 'easy-1',
    max: 4,
    width: 2,
    chipTBD: 3,
    unknownOutputs: [],
    unknownMicrochipConversions: [],
    chipCopies: [],
  },

  {
    description: 'easy-2',
    max: 4,
    width: 2,
    chipTBD: 3,
    unknownOutputs: [],
    unknownMicrochipConversions: [1, 2],
    chipCopies: [
      {
        index: 3,
        convertTo: 2
      },
      {
        index: 2,
        convertTo: 1
      },
    ],
  },

  {
    description: 'medium-1',
    max: 4,
    width: 2,
    chipTBD: 3,
    unknownOutputs: [3],
    unknownMicrochipConversions: [0, 1],
    chipCopies: [
      {
        index: 3,
        convertTo: 2
      },
      {
        index: 2,
        convertTo: 0
      },
    ],
  },

  {
    description: 'medium-2',
    max: 5,
    width: 3,
    chipTBD: 8,
    unknownOutputs: [],
    unknownMicrochipConversions: [],
    chipCopies: [],
  },

];

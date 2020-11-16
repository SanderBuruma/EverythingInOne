import { Component } from '@angular/core';
import { BaseComponent } from 'src/app/shared/base/base.component';
import { ActivatedRoute, Router } from '@angular/router';

import { HttpService } from 'src/app/shared/services/Http.service';
import { CookieService } from 'ngx-cookie-service';
import { ThemesService } from 'src/app/shared/services/Themes.service';
import { LocalizationService } from 'src/app/shared/services/Localization.service';

import { fadeInOut } from 'src/app/shared/animations/main.animations';
import { AnimationTimers } from 'src/app/shared/enums/animation-timers.enum';

import { Microchip } from './chip.model';
import { difficulties } from './difficulties.data';
import { CookieKeys } from 'src/app/shared/enums/cookie-keys.enum';

@Component({
  animations: [ fadeInOut ],
  templateUrl: './electronics-game.component.html',
  styleUrls: ['./electronics-game.component.scss']
})
export class ElectronicsGameComponent extends BaseComponent {

  //#region Fields
  public _microchips: Microchip[] = [];
  public _inputs: number[] = [];
  public _guesses: number[] = [];

  public _gameIsRunning = false;
  public _difficulties = difficulties;
  //#endregion

  //#region Constructor
  constructor(
    public _httpService: HttpService,
    _router: Router,
    _route: ActivatedRoute,
    _cookieService: CookieService,
    _localizationService: LocalizationService,
    _themesService: ThemesService
  ) {
    super(_router, _route, _cookieService, _themesService, _localizationService);
    this.SetNewField(this.Max, this.Width, this.Height);
  }
  //#endregion

  //#region Methods
  public SetNewField(max: number, width: number, height: number) {
    this._inputs = [];
    this._microchips = [];
    this._guesses = [];

    for (let i = 0; i < width * height; i++) {
      this._microchips.push(new Microchip(
        max,
        'abcdefghijklmnopqrstuvwxyz'[i],
        this.Difficulty.unknownMicrochipConversions.indexOf(i) === -1,
      ));
    }

    // make copies of certain chips
    for (const i of this.Difficulty.chipCopies) {
      this._microchips[i.index] = this._microchips[i.convertTo];
    }

    for (let i = 0; i < width + height; i++) {
      this._inputs.push(0);
    }


    for (let i = 0; i < max; i++) {
      this._guesses.push(-1);
    }
    this._gameIsRunning = true;
  }

  public IncrementInput(i) {
    let temp = this._inputs[i] + 1;

    temp %= this.Max;

    this._inputs[i] = temp;

  }

  /** Calculates an output field. i=0 is bottom left, the last i is the righthand bottom most */
  public GetOutPut(i: number) {

    // if output may not be shown
    if (this.Difficulty.unknownOutputs.indexOf(i) !== -1) { return '?'; }

    let outputToBe = this._inputs[i];
    if (i < this.Width) {
      // southern outputs
      for (let j = 0; j < this.Height; j++) {
        outputToBe = this._microchips[j * this.Width + i].ConvertSignal(outputToBe);
      }
    } else {
      // eastern outputs
      for (let j = 0; j < this.Width; j++) {
        outputToBe = this._microchips[(i - this.Width) * this.Width + j].ConvertSignal(outputToBe);
      }
    }
    return outputToBe;
  }

  /** Increments a guess */
  public IncrementGuess(i: number) {
    this._guesses[i] = (this._guesses[i] + 1) % this.Max;

    // counts the nr of correct guesses
    let correctGuesses = 0;
    for (let j = 0; j < this.Max; j++) {
      if (this._guesses[j] === this._microchips[this.Difficulty.chipTBD].Conversions[j]) {
        correctGuesses++;
      }
    }

    // end round if all guesses are correct
    if (correctGuesses === this.Max) {
      this._gameIsRunning = false;

      setTimeout(() => {
        this.RoundIndexIncrement();
        this.SetNewField(this.Max, this.Width, this.Height);
        this._gameIsRunning = true;
      }, AnimationTimers.Fade * 1.5);
    }
  }

  public RoundIndexIncrement() {
    this.RoundIndex++;
  }

  public OutputHidden(i: number) {
    return this.Difficulty.unknownOutputs.indexOf(i) !== -1;
  }
  //#endregion

  //#region Properties
  public get DifficultiesIndex() {
    return this.RoundIndex % difficulties.length;
  }

  /** The current difficulty setting */
  public get Difficulty() {
    return this._difficulties[this.RoundIndex];
  }

  /** The number of microchips in a row (and column). */
  public get Width() {
    return this.Difficulty.width;
  }

  /** The number of microchips in a row (and column). */
  public get Height() {
    return this.Difficulty.height;
  }

  /** The maximum value of inputs, outputs and microchip conversions. */
  public get Max() {
    return this.Difficulty.max + Math.floor(this.RoundIndex / difficulties.length);
  }

  /** The index of the current round */
  public get RoundIndex() {
    return super.GetCookievalueNum(CookieKeys.ElxRound);
  }
  /** The index of the current round */
  public set RoundIndex(value: number) {
    super.SetCookievalue(CookieKeys.ElxRound, value);
  }
  /** The current round */
  public get RoundIndexDisplay() {
    return super.GetCookievalueNum(CookieKeys.ElxRound) + 1;
  }
  //#endregion

}

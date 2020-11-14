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

@Component({
  animations: [ fadeInOut ],
  templateUrl: './electronics-game.component.html',
  styleUrls: ['./electronics-game.component.scss']
})
export class ElectronicsGameComponent extends BaseComponent {

  public _microchips: Microchip[] = [];
  public _inputs: number[] = [];
  public _guesses: number[] = [];

  public _gameIsRunning = false;
  public _difficultiesIndex = 0;
  public _difficulties = difficulties;

  constructor(
    public _httpService: HttpService,
    _router: Router,
    _route: ActivatedRoute,
    _cookieService: CookieService,
    _localizationService: LocalizationService,
    _themesService: ThemesService
  ) {
    super(_router, _route, _cookieService, _themesService, _localizationService);
    this.SetNewField(this.Max, this.Width);
  }

  public SetNewField(max: number, width: number) {
    this._inputs = [];
    this._microchips = [];
    this._guesses = [];

    for (let i = 0; i < width * width; i++) {
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

    for (let i = 0; i < width * width; i++) {
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

  /** Calculates output. i=0 is bottom left, the last i is the righthand bottom most */
  public GetOutPut(i: number) {
    // if output may not be shown
    if (this.Difficulty.unknownOutputs.indexOf(i) !== -1) { return '?'; }

    let input = this._inputs[i];
    if (i < this.Width) {
      // bottom outputs
      input = this._microchips[i].ConvertSignal(input);
      return this._microchips[i + 2].ConvertSignal(input);
    } else {
      // righthand outputs
      input = this._microchips[i * 2 - 4].ConvertSignal(input);
      return this._microchips[i * 2 - 3].ConvertSignal(input);
    }
  }

  public IncrementGuess(i: number) {
    this._guesses[i] = (this._guesses[i] + 1) % this.Max;
    let correctGuesses = 0;
    for (let j = 0; j < this.Max; j++) {
      if (this._guesses[j] === this._microchips[this.Width * this.Width - 1].Conversions[j]) {
        correctGuesses++;
      }
    }

    if (correctGuesses === this.Max) {
      this._gameIsRunning = false;
      console.log({msg: 'round complete'});
      setTimeout(() => {
        this.DifficultiesIndexIncrement();
        this.SetNewField(this.Max, this.Width);
        this._gameIsRunning = true;
      }, AnimationTimers.Fade * 2);
    }
  }

  public DifficultiesIndexIncrement() {
    this._difficultiesIndex++;
  }

  public get DifficultiesIndex() {
    return this._difficultiesIndex % difficulties.length;
  }

  /** The current difficulty setting */
  public get Difficulty() {
    return this._difficulties[this.DifficultiesIndex];
  }

  /** The number of microchips in a row (and column). */
  public get Width() {
    return difficulties[this.DifficultiesIndex].width;
  }

  /** The maximum value of inputs, outputs and microchip conversions. */
  public get Max() {
    return difficulties[this.DifficultiesIndex].max + Math.floor(this._difficultiesIndex / difficulties.length);
  }

}

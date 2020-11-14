import { Component, OnChanges, SimpleChanges } from '@angular/core';
import { HttpService } from 'src/app/shared/services/Http.service';
import { BaseComponent } from 'src/app/shared/base/base.component';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { ThemesService } from 'src/app/shared/services/Themes.service';
import { LocalizationService } from 'src/app/shared/services/Localization.service';
import { Microchip } from './chip.model';
import { fadeInOut } from 'src/app/shared/animations/main.animations';
import { AnimationTimers } from 'src/app/shared/enums/animation-timers.enum';

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

  public _width = 2;
  public _max = 4;

  constructor(
    public _httpService: HttpService,
    _router: Router,
    _route: ActivatedRoute,
    _cookieService: CookieService,
    _localizationService: LocalizationService,
    _themesService: ThemesService
  ) {
    super(_router, _route, _cookieService, _themesService, _localizationService);
    this.SetNewField(this._max, this._width);

  }

  public SetNewField(max: number, width: number) {
    this._inputs = [];
    this._microchips = [];
    this._guesses = [];

    for (let i = 0; i < width * width; i++) {
      this._microchips.push(new Microchip(max, 'abcdefghijklmnopqrstuvwxyz'[i]));
    }

    for (let i = 0; i < width * 2; i++) {
      this._inputs.push(0);
    }

    for (let i = 0; i < max * 2; i++) {
      this._guesses.push(0);
    }
    this._gameIsRunning = true;
  }

  public FieldsString(i: number) {
    let str = '';
    if (i === 0) {
      str += '.';
      for (let j = 0; j < this._width; j++) {
        str += this._inputs[j];
      }
      for (let j = 0; j < this._width; j++) {
        str += this._inputs[j + this._width];
      }
    } else {

    }

  }

  public IncrementInput(i) {
    let temp = this._inputs[i] + 1;

    temp %= this._max;

    this._inputs[i] = temp;

  }

  /** Calculates output. i=0 is bottom left, the last i is the righthand bottom most */
  public GetOutPut(i: number) {
    let input = this._inputs[i];
    if (i < this._width) {
      // bottom outputs
      input = this._microchips[i].ConvertSignal(input);
      return this._microchips[i + 2].ConvertSignal(input);
    } else {
      // righthand outputs
      input = this._microchips[i * 2 - 4].ConvertSignal(input);
      return this._microchips[i * 2 - 3].ConvertSignal(input);
    }
    return '.';
  }

  public IncrementGuess(i: number) {
    this._guesses[i] = (this._guesses[i] + 1) % this._max;
    let correctGuesses = 0;
    for (let j = 0; j < this._max; j++) {
      if (this._guesses[j] === this._microchips[this._width * this._width - 1].Conversions[j]) {
        correctGuesses++;
      }
    }

    if (correctGuesses === this._max) {
      this._gameIsRunning = false;
      console.log({msg: 'round complete'});
      setTimeout(() => {
        this._max++;
        this.SetNewField(this._max, this._width);
        this._gameIsRunning = true;
      }, AnimationTimers.Fade * 2);
    }
  }

}

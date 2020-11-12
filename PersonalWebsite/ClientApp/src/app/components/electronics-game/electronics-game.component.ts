import { Component, OnChanges, SimpleChanges } from '@angular/core';
import { HttpService } from 'src/app/shared/services/Http.service';
import { BaseComponent } from 'src/app/shared/base/base.component';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { ThemesService } from 'src/app/shared/services/Themes.service';
import { LocalizationService } from 'src/app/shared/services/Localization.service';
import { Microchip } from './chip.model';

@Component({
  templateUrl: './electronics-game.component.html',
  styleUrls: ['./electronics-game.component.scss']
})
export class ElectronicsGameComponent extends BaseComponent {

  public _microchips: Microchip[] = [];
  public _inputs: number[] = [];

  public _width = 2;
  public _max = 8;

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
    console.log({num: this._microchips.length});
  }

  public SetNewField(max: number, width: number) {
    this._inputs = [];
    this._microchips = [];

    for (let i = 0; i < width * width; i++) {
      this._microchips.push(new Microchip(max, 'abcdef'[i]));
    }

    for (let i = 0; i < width * 2; i++) {
      this._inputs.push(Math.floor(Math.random() * max));
    }
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
    console.log({i, temp});
    temp %= this._max;
    console.log({i, temp});
    this._inputs[i] = temp;
    console.log({i, inp: this._inputs[i]});
  }

  public GetOutPut(i: number) {

  }

}

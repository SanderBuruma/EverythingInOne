import { Component, OnChanges, SimpleChanges } from '@angular/core';
import { HttpService } from 'src/app/shared/services/Http.service';
import { BaseComponent } from 'src/app/shared/base/base.component';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { ThemesService } from 'src/app/shared/services/Themes.service';
import { LocalizationService } from 'src/app/shared/services/Localization.service';

@Component({
  selector: 'app-big-prime-component',
  templateUrl: './big-prime.component.html',
  styleUrls: ['./big-prime.component.scss']
})
export class BigPrimeComponent extends BaseComponent {
  public _digitsToGet = 50;
  public _prime = '131';

  constructor(
    public _httpService: HttpService,
    _router: Router,
    _route: ActivatedRoute,
    _cookieService: CookieService,
    _localizationService: LocalizationService,
    _themesService: ThemesService
  ) {
    super(_router, _route, _cookieService, _themesService, _localizationService);
    this.GetPrime(this._digitsToGet);
  }

  public async GetPrime(digits: number) {
    if (digits % 1 !== 0) { digits = 25; }
    this._httpService.Get<{prime: string}>('gimmick/bigPrime?digits=' + digits).then(response => {
      if (response.prime.length === 1) {
        this._prime = 'We know you want some awesome prime numbers, but we\'re sorry because there may not be more than 1000 or less than 6 digits.';
      } else {
        this._prime = response.prime.replace(/,/g, ' ');
      }

    });
  }

}

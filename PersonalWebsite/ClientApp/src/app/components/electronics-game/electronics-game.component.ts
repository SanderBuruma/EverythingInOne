import { Component, OnChanges, SimpleChanges } from '@angular/core';
import { HttpService } from 'src/app/shared/services/Http.service';
import { BaseComponent } from 'src/app/shared/base/base.component';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { ThemesService } from 'src/app/shared/services/Themes.service';
import { LocalizationService } from 'src/app/shared/services/Localization.service';

@Component({
  templateUrl: './electronics-game.component.html',
  styleUrls: ['./electronics-game.component.scss']
})
export class ElectronicsGameComponent extends BaseComponent {

  constructor(
    public _httpService: HttpService,
    _router: Router,
    _route: ActivatedRoute,
    _cookieService: CookieService,
    _localizationService: LocalizationService,
    _themesService: ThemesService
  ) {
    super(_router, _route, _cookieService, _themesService, _localizationService);
  }

}

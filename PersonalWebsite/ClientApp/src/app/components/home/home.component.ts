import { Component } from '@angular/core';
import { ThemesService } from 'src/app/shared/services/Themes.service';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent } from 'src/app/shared/base/base.component';
import { CookieService } from 'ngx-cookie-service';
import { CookieKeys } from 'src/app/shared/enums/cookie-keys.enum';
import { HttpService } from 'src/app/shared/services/Http.service';
import { LocalizationService } from 'src/app/shared/services/Localization.service';

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent extends BaseComponent {
  public _count = 0;

  constructor(
    _router: Router,
    _route: ActivatedRoute,
    _themesService: ThemesService,
    _cookieService: CookieService,
    _localizationService: LocalizationService,
    private _httpService: HttpService
  ) {
    super(_router, _route, _cookieService, _themesService, _localizationService);

    const cookie = super.GetCookievalue(CookieKeys.Count);

    let nr: number = parseInt(cookie, 10);
    if (!(nr >= 0)) {
      nr = 0;
    }
    this._count = nr;
  }

  public Increment() {
    this._count++;
    super.SetCookievalue(CookieKeys.Count, this._count);
  }

  public EmailTheDev() {
    this._httpService.Post('dev/', {Sender: 'sanderburuma+test@gmail.com', Subject: 'personal website test email', Body: 'This body is fresh and not yet room temperature...'});
  }
}

import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

import { ThemeIndices } from 'src/app/shared/enums/themes.enum';
import { BaseComponent } from '../../base-component/base.component';
import { HttpService } from '../../services/Http.service';
import { ThemesService } from '../../services/Themes.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent extends BaseComponent {
  public _displayMe = false;
  public _themeIndices = ThemeIndices;
  constructor(
    _router: Router,
    _route: ActivatedRoute,
    _cookieService: CookieService,
    _themesSerice: ThemesService,
    public _httpService: HttpService
  ) {
    super(_router, _route, _cookieService, _themesSerice);
  }

}

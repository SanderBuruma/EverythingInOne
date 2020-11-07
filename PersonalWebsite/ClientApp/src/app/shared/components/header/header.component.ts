import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

import { ThemeIndices } from 'src/app/shared/enums/themes.enum';
import { BaseComponent } from '../../base/base.component';
import { HttpService } from '../../services/Http.service';
import { LocalizationService } from '../../services/Localization.service';
import { ThemesService } from '../../services/Themes.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent extends BaseComponent {

  public _displayMe = false;
  public _themeIndices = ThemeIndices;
  public _showModal = false;

  constructor(
    _router: Router,
    _route: ActivatedRoute,
    _cookieService: CookieService,
    _themesService: ThemesService,
    _localizationService: LocalizationService,
    public _httpService: HttpService
  ) {
    super(_router, _route, _cookieService, _themesService, _localizationService);
  }

  public GoToActionForm() {
    this.NavigateTo('contact-me');
  }

}

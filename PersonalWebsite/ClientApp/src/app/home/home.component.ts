import { Component } from '@angular/core';
import { ThemesService } from '../shared/services/Themes.service';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent } from '../shared/base-component/base.component';
import { CookieService } from 'ngx-cookie-service';
import { CookieKeys } from '../shared/enums/cookie-keys.enum';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent extends BaseComponent {
  public _count = 0;

  constructor(
    _router: Router,
    _route: ActivatedRoute,
    _themesService: ThemesService,
    _cookieService: CookieService
  ) {
    super(_router, _route, _cookieService, _themesService);

    const cookie = super.GetCookievalue(CookieKeys.Count);

    let nr: number = parseInt(cookie, 10);
    if (!(nr >= 0)) {
      nr = 0;
    }
    this._count = nr;
  }

  public IncrementTheme() {
    this._themesSerice.IncrementTheme();
  }

  public GoToBigPrime() {
    this._router.navigateByUrl('big-prime');
  }

  public Increment() {
    this._count++;
    super.SetCookievalue(CookieKeys.Count, this._count);
  }
}

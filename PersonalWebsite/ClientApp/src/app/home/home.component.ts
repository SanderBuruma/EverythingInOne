import { Component } from '@angular/core';
import { ThemesService } from '../shared/services/Themes.service';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent } from '../shared/base-component/base.component';
import { CookieService } from 'ngx-cookie-service';
import { CookieValues } from '../shared/enums/cookie-values.enum';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent extends BaseComponent {
  public _count: number = 0;

  constructor(
    _router: Router,
    _route: ActivatedRoute,
    _themesService: ThemesService,
    _cookieService: CookieService
  ) {
    super(_router, _route, _cookieService, _themesService);

    let cookie = this._cookieService.get(CookieValues.Count);
    let nr: number;
    if (cookie != 'NaN') nr = parseInt(cookie);
    else nr = 0;
    this._count = nr;
  }
  public IncrementTheme(){
    this._themesSerice.IncrementTheme();
  }

  public GoToBigPrime(){
    this._router.navigateByUrl("big-prime")
  }

  public Increment(){
    this._count++;
    this._cookieService.set('count', this._count.toString(), 7);
    let newcookie = parseInt(this._cookieService.get("count"));
    console.log({cookie: newcookie})
  }
}

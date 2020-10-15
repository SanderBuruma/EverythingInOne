import { Component, OnInit } from '@angular/core';

import { Router, ActivatedRoute } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { CookieKeys } from '../enums/cookie-keys.enum';
import { ThemesService } from '../services/Themes.service';

// tslint:disable-next-line: max-line-length
/** The base comonent which contains all fields, properties and methods that all components should contain (except perhaps app.component.ts)*/
@Component({
  selector: 'app-base-component',
  template: '',
  styles: ['']
})
export class BaseComponent {
  //#region Constructor & ngOnInit
  constructor(
    public _router: Router,
    public _route: ActivatedRoute,
    public _cookieService: CookieService,
    public _themesSerice: ThemesService
  ) {}

  public async NavigateTo(url: string) {
    await this._router.navigateByUrl(url);
  }

  /**
   *
   * @param nr the number to be floored
   * @param decimals the number of decimals to remain
   */
  public FloorNumber(nr: number, decimals: number = 0) {
    return Math.floor(nr * 10 ** decimals) / 10 ** decimals;
  }

  public GetCookievalueNum(key: CookieKeys): number {
    const value = this.GetCookievalue(key);
    if (value) {
      return parseInt(value, 10);
    } else {
      return 0;
    }
  }
  public GetCookievalue(key: CookieKeys) {
    return this._cookieService.get(key);
  }

  public SetCookievalue(key: CookieKeys, value) {
    this._cookieService.set(key, value, 7);
  }
  //#endregion

}

import { Component, OnInit } from '@angular/core';

import { Router, ActivatedRoute } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { ThemesService } from '../services/Themes.service';

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

  public async NavigateTo(url: string){
    await this._router.navigateByUrl(url);
  }

  /**
   *
   * @param nr the number to be floored
   * @param decimals the number of decimals to remain
   */
  public FloorNumber(nr: number, decimals: number = 0){
    return Math.floor(nr*10**decimals)/10**decimals
  }
  //#endregion

}

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
  //#endregion

}

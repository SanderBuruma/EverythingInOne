import { Component }              from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpService }            from '../shared/services/Http.service';
import { BaseComponent }          from '../shared/base-component/base.component';
import { CookieService }          from 'ngx-cookie-service';
import { ThemesService }          from '../shared/services/Themes.service';
import { CookieValues }           from '../shared/enums/cookie-values.enum';
import { Board }                  from './classes';

@Component({
  selector: 'app-big-prime-component',
  templateUrl: './snake-game.component.html',
  styleUrls: ['./snake-game.component.scss']
})
export class SnakeGameComponent extends BaseComponent {
  constructor(
    _router: Router,
    _route: ActivatedRoute,
    _cookieService: CookieService,
    _themesSerice: ThemesService,
    public _httpService: HttpService
  ) {
    super(_router, _route, _cookieService, _themesSerice);

    this.GetBoard();
  }

  public GetBoard(){
    this._httpService.Get<Board>("snake/board?i=5&widthHeight=11").then(board=>console.log({board}))
  }
}

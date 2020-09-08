import { Component } from '@angular/core';
import { ThemesService } from '../shared/services/Themes.service';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent } from '../shared/base-component/base.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent extends BaseComponent {
  constructor(
    _router: Router,
    _route: ActivatedRoute,
    public _themesService: ThemesService
  ) {
    super(_router, _route);
  }

  public GoToBigPrime(){
    this._router.navigateByUrl("big-prime")
  }
}

import { Component }              from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ThemeIndices }           from 'src/app/enums/themes.enum';
import { HttpService } from '../../services/Http.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  public _displayMe: boolean = false;
  public _themeIndices = ThemeIndices;
  constructor(
    public _route: ActivatedRoute,
    public _router: Router,
    public _httpService: HttpService) {
  }

}

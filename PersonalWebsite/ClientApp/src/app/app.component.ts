import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { ThemesService } from './shared/services/Themes.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  _theme: Observable<string>;
  title = 'app';

  constructor(public _themesService: ThemesService)
  {
    this._theme = this._themesService.ThemeObservable;
  }
}

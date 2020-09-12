import { Component, HostListener } from '@angular/core';
import { Observable } from 'rxjs';
import { ThemesService } from './shared/services/Themes.service';
import { HttpService } from './shared/services/Http.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'Sander Buruma\'s laboratory';

  constructor(
    public _themesService: ThemesService,
    public _httpService: HttpService)
  {
  }

  //#region Listeners
  @HostListener('document:keyup', ['$event'])
  handleDeleteKeyboardEvent(event: KeyboardEvent) {

    if (event.key === 'Escape') {
      this._httpService.CancelRequests();
    }

  }
  //#endregion

}

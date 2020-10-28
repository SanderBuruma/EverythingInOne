import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { BehaviorSubject } from 'rxjs';
import { CookieKeys } from '../enums/cookie-keys.enum';
import { ThemeIndices } from '../enums/themes.enum';

@Injectable({
  providedIn: 'root', // ensures this service is used as a singleton
})

// tslint:disable-next-line: max-line-length
/**Should handle all theming everywhere in the application. Different clients should each be able to use different ones than another. This service is supposed to control that.*/
export class ThemesService {
  //#region Fields
  /**This is supposed to list all themes put into the system at /src/themes/themes.scss*/
  private _listOfThemes: string[];
  private _themeSubject: BehaviorSubject<string>;
  //#endregion

  //#region Construtor
  constructor(
    public _cookieService: CookieService
  ) {
    this._listOfThemes = [
      // default theme
      'cubit-theme',

      // custom themes
      'generic-theme',
      'fire-theme',
      'jungle-theme'
    ];

    this._themeSubject = new BehaviorSubject<string>(this._listOfThemes[this.ThemeIndex]);
  }
  //#endregion

  //#region Methods
  /**Predictably increments the theme index*/ // for dev-testing purposes
  public IncrementTheme() {
    this.ThemeIndex = this.ThemeIndex + 1;
    this.ThemeIndex = this.ThemeIndex % this._listOfThemes.length;
    this._themeSubject.next(this._listOfThemes[this.ThemeIndex]);
  }
  //#endregion

  //#region Properties
  public get ThemeObservable() {
    return this._themeSubject.asObservable();
  }


  public get ThemeIndex() {
    const cookie: any = parseInt(this._cookieService.get(CookieKeys.ThemeIndex), 10);
    if (cookie >= 0) {
      return cookie;
    } else {
      this.ThemeIndex = 0;
      return 0;
    }
  }

  public set ThemeIndex(value: number) {
    this._cookieService.set(CookieKeys.ThemeIndex, value.toString(), 7);
  }
  //#endregion

}

import { Injectable } from '@angular/core'
import { BehaviorSubject } from 'rxjs';
import { ThemeIndices } from '../../enums/themes.enum'

@Injectable({
  providedIn: 'root', // ensures this service is used as a singleton
})

/**Should handle all theming everywhere in the application. Different clients should each be able to use different ones than another. This service is supposed to control that.*/
export class ThemesService {
  //#region Fields
  private _currentThemeIndex: number;
  /**This is supposed to list all themes put into the system at /src/themes/themes.scss*/
  private _listOfThemes: string[];
  private _themeSubject: BehaviorSubject<string>;
  //#endregion

  //#region Construtor
  constructor() {
    this._currentThemeIndex = ThemeIndices.Generic;

    this._listOfThemes = [
      //default theme
      "generic-theme",

      //custom themes
      "fire-theme",
      "jungle-theme"
    ];

    this._themeSubject = new BehaviorSubject<string>(this._listOfThemes[this._currentThemeIndex]);
  }
  //#endregion

  //#region Methods
  /**Predictably increments the theme index*/ //for dev-testing purposes
  public IncrementTheme() {
    this._currentThemeIndex++;
    this._currentThemeIndex %= this._listOfThemes.length;
    this._themeSubject.next(this._listOfThemes[this._currentThemeIndex]);
    console.log({theme: this._themeSubject.value})
  }
  //#endregion

  //#region Properties
  public get ThemeObservable(){
    return this._themeSubject.asObservable();
  }

  public get ThemeIndex(){
    return this._currentThemeIndex;
  }
  //#endregion

}

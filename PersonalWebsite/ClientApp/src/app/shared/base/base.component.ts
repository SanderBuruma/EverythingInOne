import { Router, ActivatedRoute } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { CookieKeys } from '../enums/cookie-keys.enum';
import { LocalizationService } from '../services/Localization.service';
import { ThemesService } from '../services/Themes.service';

// tslint:disable-next-line: max-line-length
/** The base comonent which contains all fields, properties and methods that all components should contain (except perhaps app.component.ts)*/
export class BaseComponent {
  //#region Constructor & ngOnInit
  constructor(
    public _router: Router,
    public _route: ActivatedRoute,
    public _cookieService: CookieService,
    public _themesService: ThemesService,
    public _localizationService: LocalizationService
  ) {
    // acts as a sort of user id
    if (!this.GetCookievalue(CookieKeys.RngId, '')) { this.SetCookievalue(CookieKeys.RngId, this.GenerateGuid() ); }
  }

  public async NavigateTo(url: string) {
    await this._router.navigateByUrl(url);
  }

  /**
   * get a localized text
   * @param label the label of the text to get
   */
  public GetLocalization(label: string) {
    return this._localizationService.TextByLabel(label);
  }

  /**
   *
   * @param nr the number to be floored
   * @param decimals the number of decimals to remain
   */
  public FloorNumber(nr: number, decimals: number = 0) {
    return Math.floor(nr * 10 ** decimals) / 10 ** decimals;
  }

  /**
   *
   * @param key cookie key used to find the correct cookie
   * @param dflt default value (0 if unset)
   */
  public GetCookievalueNum(key: CookieKeys, dflt = 0): number {
    const value = this.GetCookievalue(key);
    if (value) {
      return parseInt(value, 10);
    } else {
      return dflt;
    }
  }

  /**
   *
   * @param key the cookie key of the cookie to get
   * @param dflt default value
   */
  public GetCookievalue(key: CookieKeys, dflt = '') {
    return this._cookieService.get(key) ?? dflt;
  }

  /**
   *
   * @param key the cookie key of the cookie to get
   * @param value the value to set the cookie to
   * @param expires cookie expiry timer (7 days if unset)
   */
  public SetCookievalue(key: CookieKeys, value, expires = 7) {
    this._cookieService.set(key, value, expires);
  }

  // https://stackoverflow.com/a/2117523/10055628
  /**
   * Generates a random Guid
   */
  public GenerateGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      // tslint:disable-next-line: no-bitwise
      const r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

  /** Gets an array with a range of filled in values */
  public GetArray(start: number = 0, incrementor: number = 1, elements: number = 50) {
    const arr: number[] = [];

    for (let i = 0; i < elements; i++) {
      arr.push(start + i * incrementor);
    }
    return arr;
  }
  //#endregion

}

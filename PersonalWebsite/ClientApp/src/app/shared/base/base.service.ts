import { CookieService } from 'ngx-cookie-service';
import { CookieKeys } from '../enums/cookie-keys.enum';

export class BaseService {

  constructor(public _cookieService: CookieService) {}

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

}

import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { CookieKeys } from '../enums/cookie-keys.enum';
import { BaseService } from '../base/base.service';
import { LocalizationIndices } from '../enums/localization-indices.enum';

import { EnglishLocalizations } from '../data/localizations/en.data';
import { DutchLocalizations } from '../data/localizations/nl.data';
import { LatinLocalizations } from '../data/localizations/lt.data';

@Injectable({
  providedIn: 'root', // ensures this service is used as a singleton
})

// tslint:disable-next-line: max-line-length
/**Handles all localization texts.*/
export class LocalizationService extends BaseService {
  //#region Fields

  //#endregion

  //#region Construtor
  constructor(
    _cookieService: CookieService
  ) {super(_cookieService); }
  //#endregion

  //#region Methods
  private GetLocTexts() {
    if (this.LocalizationIndex === LocalizationIndices.English) {
      return EnglishLocalizations;
    } else if (this.LocalizationIndex === LocalizationIndices.Dutch) {
      return DutchLocalizations;
    } else if (this.LocalizationIndex === LocalizationIndices.Latin) {
      return LatinLocalizations;
    }
  }

  /**
   * Get a specific text from the localization files.
   * @param label The label of the localization text to be had
   */
  public TextByLabel(label: string) {
    const locs = this.GetLocTexts();
    for (let i = 0; i < locs.length; i++) {
      if (locs[i].label === label) {
        return locs[i].value;
      }
    }
  }

  /**
   * Changes the language by incrementing the index
   */
  public Increment() {
    this.LocalizationIndex = (this.LocalizationIndex + 1) % 3; // LocIndices.length
  }
  //#endregion

  //#region Properties
  public get LocalizationIndex() {
    return super.GetCookievalueNum(CookieKeys.LocalizationIndex);
  }

  public set LocalizationIndex(value: number) {
    super.SetCookievalue(CookieKeys.LocalizationIndex, value.toString());
  }
  //#endregion

}

export class LocalizationUnit {
  public label: string;
  public value: string;
}

import { Component, OnInit } from '@angular/core';
import { HttpService } from 'src/app/shared/services/Http.service';
import { BaseComponent } from 'src/app/shared/base/base.component';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { ThemesService } from 'src/app/shared/services/Themes.service';
import { CookieKeys } from 'src/app/shared/enums/cookie-keys.enum';
import { fadeIn, upIn } from 'src/app/shared/animations/main.animations';
import { LocalizationService } from 'src/app/shared/services/Localization.service';

@Component({
  templateUrl: './scribo-alacrito.component.html',
  styleUrls: ['./scribo-alacrito.component.scss'],
  animations: [ fadeIn, upIn ]
})
export class ScriboAlacritoComponent extends BaseComponent implements OnInit {
  //#region Fields
  public _text            = '';
  public _nextText        = '';
  public _title           = '';

  public _getTextI        = 0;
  public _input           = '';
  public _inputPrevLength = 0;
  public _i               = 0;
  public _nrOfLines       = 0;

  public _textCorrect     = '';
  public _textFalse       = '';
  public _textRemaining   = '';

  /**the last time in unix that typing was started */
  public _lastTime        = Date.now();
  public _wpmList: { wpm: number, length: number }[] = [];

  public _correct         = true;
  //#endregion

  //#region Constructor & ngOnInit
  constructor(
    _router: Router,
    _route: ActivatedRoute,
    _cookieService: CookieService,
    _themesService: ThemesService,
    _localizationService: LocalizationService,
    public _httpService: HttpService
  ) {
    super(_router, _route, _cookieService, _themesService, _localizationService);

  }

  ngOnInit(): void {
    const initialNr = parseInt(this._route.snapshot.paramMap.get(CookieKeys.ScriboI), 10);
    console.log({temp: initialNr, pMap: this._route.snapshot.paramMap});

    if (initialNr) { this.I = initialNr; }

    console.log({I: this.I});

    // start everything
    if (!this.I) { this.I = 0; }

    this.GetText(this.I);

    // get number of lines
    this._httpService.Get<number>('scriboAlacrito/getLinesNr').then(nr => this._nrOfLines = nr);

  }
  //#endregion

  //#region Properties
  /** words Per Minute (chars per second multiplied by 12) to 2 decimals */
  public get Wpm() {
    return Math.round(this.SumChars / this.SumTime * 1200) / 100;
  }

  /** Sum of chars typed */
  public get I() {
    return super.GetCookievalueNum(CookieKeys.ScriboI);
  }
  /** Sum of chars typed */
  public set I(value: number) {
    super.SetCookievalue(CookieKeys.ScriboI, value);
  }

  /** Sum of chars typed */
  public get SumChars() {
    return super.GetCookievalueNum(CookieKeys.ScriboCharSum);
  }
  /** Sum of chars typed */
  public set SumChars(value: number) {
    super.SetCookievalue(CookieKeys.ScriboCharSum, value);
  }

  /** Sum of seconds used to type texts */
  public get SumTime() {
    return super.GetCookievalueNum(CookieKeys.ScriboTimeSum, 60);
  }
  /** Sum of seconds used to type texts */
  public set SumTime(value: number) {
    super.SetCookievalue(CookieKeys.ScriboTimeSum, value);
  }
  //#endregion

  //#region Methods
  public ChangeEvent(event: string) {

    console.log({len: this._input.length});
    // if the input length is or was 0 reset the timer
    if (this._input.length <= 2) {
      this._lastTime = Date.now();
    }

    // if input text length is suddenly a lot longer than the previous length there's been a copy paste
    if (this._input.length > this._inputPrevLength + 10) {
      this._input = '';
      this._correct = this.UpdateTextColors();
      alert('No copy pasting!');
      return;
    }

    this._correct = this.UpdateTextColors();

    // TODO if input is in equal length to text then finish
    if (this._input.length >= this._text.length && !this._textFalse) {
      this.GetText(this._i + 2);
    }

    this._inputPrevLength = this._text.length;
  }

  /**
   * Updates the coloring of the texts.
   * @returns true if correct input
   */
  public UpdateTextColors() {
    // find the length of the input congruent text (stored in i)
    let i = 0;
    for (; i < this._input.length; i++) {
      if (this._input.substring(0, this._input.length - i) === this._text.substring(0, this._input.length - i)) {
        break;
      }
    }

    // fill in the three spans
    this._textRemaining = this._text.substring(this._input.length);
    if (i === 0) {
      // Full match between input and current text.
      this._textCorrect = this._text.substring(0, this._input.length);
      this._textFalse = '';
      return true;
    } else if (i < this._input.length) {
      // Partial match
      this._textCorrect = this._text.substring(0, this._input.length - i);
      this._textFalse = this._text.substring(this._input.length - i, this._input.length);
    } else {
      // No match
      this._textCorrect = '';
      this._textFalse = this._text.substring(0, this._input.length);
    }
    return false;
  }

  /**
   * Gets the next text to use from the server side database. Also posts local wpm scores to the user.
   * @param i the i value to get from the db
   */
  public async GetText(i: number = -1, reset = false) {
    // don't fetch another while the first is being requested.
    if (this._httpService.RunningRequestsCount) { return; }
    if (reset) {
      this._text = '';
      this._input = '';
      this._nextText = '';
    }

    // if a text was just successfully typed
    const textLength = this._text.length;
    if (this._text) {
      // reset
      this._correct = true;
      this._inputPrevLength = 0;
      this._text =  this._nextText + ' ';
      this._nextText = '';
    }

    if (this._input.length > 10) {
      this._input = this._input.substring(textLength);
      // update wpm by averaging with previous scores and reducing the weight of the previous scores
      const timeUnit = (Date.now() - this._lastTime) / 1e3;
      this._lastTime = Date.now();
      const wpm = textLength / timeUnit * 12;

      // save to cookies
      if (wpm > 15) {
        this.SumTime  = (this.SumTime  * 24) / 25 + timeUnit;
        this.SumChars = (this.SumChars * 24) / 25 + textLength;

        // expand the local recordings of typing speed
        this._wpmList.unshift({ wpm, length: textLength });
        if (this._wpmList.length > 10) { this._wpmList.pop(); }
      }


      // move texts around and fetch a new one
      this._i++;
      super.SetCookievalue(CookieKeys.ScriboI, this._i.toString());
    }
    this._input = '';

    console.log({i});
    // here we are
    this._httpService.Get<{ str: string, i: number, title: string }>('scriboAlacrito/getText?i=' + i).then(backendReturnText => {
      this._title = backendReturnText.title;

      // if this is the first call to GetText() then fetch two texts
      if (!this._text) {
        this._text = backendReturnText.str + ' ';
        this._httpService.Get<{ str: string, i: number, title: string }>('scriboAlacrito/getText?i=' + (++i)).then(nextTextToBe => {
          this._title = nextTextToBe.title;
          this._nextText = nextTextToBe.str;
        });
      } else {
        this._nextText = backendReturnText.str;
      }

      this.UpdateTextColors();
    });
  }

  public GetElapsedTime() {
    const difference = Date.now() - this._lastTime;
    return Math.floor(difference / 1000);
  }
  //#endregion

}

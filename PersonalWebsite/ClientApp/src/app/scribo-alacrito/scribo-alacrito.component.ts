import { Component }              from '@angular/core';
import { HttpService }            from '../shared/services/Http.service';
import { BaseComponent }          from '../shared/base-component/base.component';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService }          from 'ngx-cookie-service';
import { ThemesService }          from '../shared/services/Themes.service';
import { CookieValues }           from '../shared/enums/cookie-values.enum';

@Component({
  selector: 'app-big-prime-component',
  templateUrl: './scribo-alacrito.component.html',
  styleUrls: ['./scribo-alacrito.component.scss']
})
export class ScriboAlacritoComponent extends BaseComponent {
  public _text            = "";
  public _nextText        = "";

  public _input           = "";
  public _inputPrevLength = 0;
  public _i               = 0;

  public _textCorrect     = "";
  public _textFalse       = "";
  public _textRemaining   = "";

  /**the last time in unix that typing was started */
  public _lastTime        = Date.now();
  public _wpm             = 0;
  public _wpmList: {wpm: number, length: number}[]= []

  public _correct         = true;

  constructor(
    _router: Router,
    _route: ActivatedRoute,
    _cookieService: CookieService,
    _themesSerice: ThemesService,
    public _httpService: HttpService
  ) {
    super(_router, _route, _cookieService, _themesSerice);

    //get the index of the text in the que
    let cookieVal = parseInt(this._cookieService.get(CookieValues.ScriboI));
    let wpmVal = parseInt(this._cookieService.get(CookieValues.ScriboWpm));

    //start everything
    this._i = cookieVal >= 0 ? cookieVal : 0;
    this._wpm = wpmVal >= 0 ? wpmVal : 0;
    this.GetText(this._i);
  }

  ngOnChanges(): void {
  }

  ChangeEvent(){
    //if input text length is suddenly a lot longer than the previous length there's been a copy paste
    if (this._inputPrevLength == 0 || this._input.length == 0){
      this._lastTime = Date.now();
    }
    if (this._input.length > this._inputPrevLength + 10){
      this._input = "";
      this._correct = this.UpdateTextColors()
      alert("No copy pasting!");
      return;
    }

    this._correct = this.UpdateTextColors()

    //TODO if input is in equal length to text then finish
    if (this._input.length >= this._text.length && this._correct){
      this.GetText(this._i + 2);
    }

    this._inputPrevLength = this._text.length;
  }

  /**
   * Updates the coloring of the texts.
   * @returns true if correct input
   */
  public UpdateTextColors(){
    // find the length of the input congruent text (stored in i)
    let i = 0;
    for (; i < this._input.length; i++){
      if (this._input.substring(0, this._input.length-i) == this._text.substring(0, this._input.length-i)){
        break;
      }
    }

    //fill in the three spans
    this._textRemaining = this._text.substring(this._input.length)
    if (i == 0){
      // Full match between input and current text.
      this._textCorrect = this._text.substring(0, this._input.length);
      this._textFalse = "";
      return true;
    } else if (i < this._input.length) {
      // Partial match
      this._textCorrect = this._text.substring(0, this._input.length-i);
      this._textFalse = this._text.substring(this._input.length - i, this._input.length);
    } else {
      // No match
      this._textCorrect = "";
      this._textFalse = this._text.substring(0, this._input.length);
    }
    return false;
  }

  /**
   * Gets the next text to use from the server side database. Also posts local wpm scores to the user.
   * @param i the i value to get from the db
   */
  public async GetText(i: number = -1){
    //don't fetch another while the first is being requested.
    if (this._httpService.RunningRequestsCount) return;

    //if a text was just successfully typed
    if (this._text)
    {
      // reset
      this._correct = true;
      this._inputPrevLength = 0;
      this._text =  this._nextText + " ";
    }

    if (this._input.length > 10) {
      this._input = "";
      // update wpm by averaging with previous scores and reducing the weight of the previous scores
      let timeUnit = (Date.now() - this._lastTime) / 12e3
      this._lastTime = Date.now();
      let wpm = this._text.length / timeUnit

      this._wpm = Math.floor(
        this._wpm*24+wpm
      ) / 25
      this._cookieService.set(CookieValues.ScriboWpm, this._wpm.toString(), 7);

      // expand the local recordings of typing speed
      this._wpmList.unshift({ wpm, length: this._text.length })
      if (this._wpmList.length > 10) this._wpmList.pop();

      // move texts around and fetch a new one
      this._i++;
      this._cookieService.set(CookieValues.ScriboI, this._i.toString(), 7);
    }
    this._input = "";


    // here we are
    this._httpService.Get<{ str: string, i: number }>("scriboAlacrito/getText?i=" + i).then(backendReturnText=>{

      // if this is the first call to GetText() then fetch two texts
      if (!this._text)
      {
        this._text = backendReturnText.str + " ";
        this._httpService.Get<{ str: string, i: number }>("scriboAlacrito/getText?i=" + (++i)).then(nextTextToBe=>{
          this._nextText = nextTextToBe.str;
        })
      }

      // if a text has just been typed
      else
      {
        this._nextText = backendReturnText.str;
      }

      this.UpdateTextColors();
    })
  }

}

import { Component, OnInit } from '@angular/core';
import { HttpService } from '../shared/services/Http.service';
import { BaseComponent } from '../shared/base-component/base.component';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { ThemesService } from '../shared/services/Themes.service';
import { CookieKeys } from '../shared/enums/cookie-keys.enum';
import { fade } from 'src/app/shared/animations/main.animations';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-codebreaker-component',
  templateUrl: './mastermind.component.html',
  styleUrls: ['./mastermind.component.scss'],
  animations: [ fade ]
})
export class MastermindComponent extends BaseComponent implements OnInit {
  //#region Fields
  public guessCode = '';
  public guessedCodes: string[] = [];
  public validCodeRgx = /^[1-8]{4}$/;
  public guessesPlusFeedback: string[] = [];
  public roundComplete = false;
  public focus = true;
  //#endregion

  //#region Constructor & ngOnInit
  constructor(
    _router: Router,
    _route: ActivatedRoute,
    _cookieService: CookieService,
    _themesSerice: ThemesService,
    public _httpService: HttpService
  ) {
    super(_router, _route, _cookieService, _themesSerice);
    _httpService.Get('codebreaker/newCode');
  }

  ngOnInit(): void {

  }
  //#endregion

  //#region Properties
  /** Locks a few UI elements if the round is complete or a request is in progress */
  public get LockUi(): boolean {
    return this.roundComplete || !!this._httpService.RunningRequestsCount;
  }
  //#endregion

  //#region Methods
  public IsGuessCodeValid(guess: string) {
    const check = this.validCodeRgx.test(guess);
    console.log({check, rgx: this.validCodeRgx, guess});
    return check;
  }

  public OnKeyDown(event: KeyboardEvent) {
    const cond1 = event.key === 'Enter',
    cond3 = !this._httpService.RunningRequestsCount;
    if (
      cond1 && cond3
    ) {

        console.log({msg: 'true', complete: this.roundComplete});
        if (!this.roundComplete) { this.MakeGuess(); } else {this.NewGame(); }

    } else if (event.key === 'Escape') {

      // reset guess
      this.guessCode = '';

    } else if (this.guessCode.length > 4) {

      // shorten too long code
      this.guessCode = this.guessCode.substr(0, 4);
      // reset code if invalid
      if (!this.validCodeRgx.test(this.guessCode)) { this.guessCode = ''; }

    }
  }

  public MakeGuess() {
    const cond1 = this.roundComplete, cond2 = !this.IsGuessCodeValid(this.guessCode);
    if (cond1 || cond2) {
      console.log({msg: 'not guessing', invalid: cond2, complete: cond1});
      return;
    }
    console.log({msg: 'next'});
    this._httpService.Get<GuessFeedback>('codebreaker/makeGuess?guess=' + this.guessCode)
    .then(feedback => {
      this.guessesPlusFeedback.push(this.FormatGuessPlusFeedback(this.guessCode, feedback.bulls, feedback.cows));
      if (feedback.bulls === 4) {
        this.roundComplete = true;
        console.log({complete: this.roundComplete});
      }
      this.guessCode = '';
    });
  }

  public GetPreviousFeedback() {
    this._httpService.Get<{guesses: string[], rScores: {Bulls: string, Cows: string}[]}>('codebreaker/getFeedback').then(fb => {
      for (let i = 0; i < fb.guesses.length; i++) {
        this.guessesPlusFeedback.push(this.FormatGuessPlusFeedback(fb[i].guessCode, fb[i].Bulls, fb[i].Cows));
      }
    });
  }

  NewGame() {
    this._httpService.Get('codebreaker/newCode').then(s => {
      this.roundComplete = !s;
      this.guessesPlusFeedback = [];
      this.roundComplete = false;
    } );
  }

  private FormatGuessPlusFeedback(guess: string, bulls: number, cows: number) {
    return `${guess} - ${bulls} ${cows}`;
  }
  //#endregion

}

class GuessFeedback {
  public bulls: number;
  public cows: number;
}

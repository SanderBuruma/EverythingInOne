import { Component, OnInit } from '@angular/core';
import { HttpService } from 'src/app/shared/services/Http.service';
import { BaseComponent } from 'src/app/shared/base/base.component';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { ThemesService } from 'src/app/shared/services/Themes.service';
import { fadeIn, fadeInOut, upInOut } from 'src/app/shared/animations/main.animations';
import { LocalizationService } from 'src/app/shared/services/Localization.service';

@Component({
  templateUrl: './mastermind.component.html',
  styleUrls: ['./mastermind.component.scss'],
  animations: [ fadeIn, fadeInOut, upInOut ]
})
export class MastermindComponent extends BaseComponent implements OnInit {
  //#region Fields
  public _guessCode = '';
  public _validCodeRgx = /^[1-8]{4}$/;
  public _guessesPlusFeedback: {guess: string, cows: number, bulls: number}[] = [];
  public _roundComplete = false;
  public _focus = true;
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
    _httpService.Get('codebreaker/newCode');
  }

  ngOnInit(): void {

  }
  //#endregion

  //#region Properties
  /** Locks a few UI elements if the round is complete or a request is in progress */
  public get LockUi(): boolean {
    return this._roundComplete || !!this._httpService.RunningRequestsCount;
  }
  //#endregion

  //#region Methods
  public IsGuessCodeValid(guess: string) {
    const check = this._validCodeRgx.test(guess);
    return check;
  }

  public OnKeyDown(event: KeyboardEvent) {
    const cond1 = event.key === 'Enter',
    cond2 = !this._httpService.RunningRequestsCount;
    if (
      cond1 && cond2
    ) {
        if (!this._roundComplete) { this.MakeGuess(); } else { this.NewGame(); }

    } else if (event.key === 'Escape') {

      // reset guess
      this._guessCode = '';

    } else if (this._guessCode.length > 4) {

      // shorten too long code
      this._guessCode = this._guessCode.substr(0, 4);
      // reset code if invalid
      if (!this._validCodeRgx.test(this._guessCode)) { this._guessCode = ''; }

    }
  }

  public MakeGuess() {
    const cond1 = this._roundComplete, cond2 = !this.IsGuessCodeValid(this._guessCode);
    if (cond1 || cond2) {
      return;
    }
    this._httpService.Get<GuessFeedback>('codebreaker/makeGuess?guess=' + this._guessCode)
    .then(feedback => {
      this._guessesPlusFeedback.push({guess: this._guessCode, bulls: feedback.bulls, cows: feedback.cows});
      if (feedback.bulls === 4) {
        this.CompleteRound();
      }
      this._guessCode = '';
    });
  }

  public CompleteRound() {
    this._roundComplete = true;

    setTimeout(() => {
      if (this.CompleteRound) {
        this.NewGame();
      }
    }, 2000);
  }

  public GetPreviousFeedback() {
    this._httpService.Get<{guesses: string[], rScores: {
      Guess: string,
      Bulls: number,
      Cows: number
    }[]}>('codebreaker/getFeedback').then(fb => {
      for (let i = 0; i < fb.guesses.length; i++) {
        this._guessesPlusFeedback.push({guess: fb.rScores[i].Guess, cows: fb.rScores[i].Bulls, bulls: fb.rScores[i].Bulls});
      }
    });
  }

  NewGame() {
    this._httpService.Get('codebreaker/newCode').then(s => {
      this._roundComplete = !s;
      this._guessesPlusFeedback = [];
      this._roundComplete = false;
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

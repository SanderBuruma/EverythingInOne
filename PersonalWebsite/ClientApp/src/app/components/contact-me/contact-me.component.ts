import { Component } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { CookieService } from 'ngx-cookie-service';
import { BaseComponent } from 'src/app/shared/base/base.component';

import { HttpService } from 'src/app/shared/services/Http.service';
import { ThemesService } from 'src/app/shared/services/Themes.service';
import { LocalizationService } from 'src/app/shared/services/Localization.service';

@Component({
  templateUrl: './contact-me.component.html',
  styleUrls: ['./contact-me.component.scss']
})
export class ContactMeComponent extends BaseComponent {

  public senderEmail = '';
  public message = '';

  public emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email
  ]);
  public messageFormControl = new FormControl('', [
    Validators.minLength(10)
  ]);

  constructor(
    _router: Router,
    _route: ActivatedRoute,
    _cookieService: CookieService,
    _themesService: ThemesService,
    _localizationService: LocalizationService,
    public _httpService: HttpService,
    public _snackbar: MatSnackBar
  ) {
    super(_router, _route, _cookieService, _themesService, _localizationService);
  }

  public EmailTheDev() {
    if (this.emailFormControl.hasError('email') || this.emailFormControl.hasError('required')) {
      return;
    }
    this._httpService.Post('dev/', {
      Sender: this.senderEmail,
      Subject: `sanderburuma.nl - ${this.senderEmail} - ${this.message.substr(0, 30)}`,
      Body: this.message
    }).then(() => {
      const config = new MatSnackBarConfig();
      config.duration = 5000;
      this._snackbar.open('Email sent!', 'ok', config);

      this.NavigateTo('/');
    }).catch(() => {
      const config = new MatSnackBarConfig();
      config.duration = 5000;
      this._snackbar.open('Error :(', 'dismiss', config);
    });
  }
}

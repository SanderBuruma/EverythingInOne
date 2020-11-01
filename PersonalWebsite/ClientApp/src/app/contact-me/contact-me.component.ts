import { Component } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { BaseComponent } from '../shared/base-component/base.component';

import { HttpService } from '../shared/services/Http.service';
import { ThemesService } from '../shared/services/Themes.service';

@Component({
  selector: 'app-contact-me',
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
    _themesSerice: ThemesService,
    public _httpService: HttpService
  ) {
    super(_router, _route, _cookieService, _themesSerice);
  }

  public EmailTheDev() {
    if (this.emailFormControl.hasError('email') || this.emailFormControl.hasError('required')) {
      return;
    }
    this._httpService.Post('dev/', {
      Sender: 'sanderburuma+test@gmail.com',
      Subject: 'sanderburuma.nl - ' + this.message.substr(0, 30),
      Body: this.message});
  }
}

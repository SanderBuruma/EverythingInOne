import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { BigPrimeComponent } from './components/big-prime-component/big-prime.component';
import { HeaderComponent } from './shared/components/header/header.component';
import { ScriboAlacritoComponent } from './components/scribo-alacrito/scribo-alacrito.component';
import { MastermindComponent } from './components/mastermind/mastermind.component';
import { PersonalCvComponent } from './components/personal-cv/personal-cv.component';
import { ContactMeComponent } from './components/contact-me/contact-me.component';
import { ElectronicsGameComponent } from './components/electronics-game/electronics-game.component';
import { BgheaderComponent } from './shared/components/svg/bgheader/bgheader.component';

import { FlexLayoutModule } from '@angular/flex-layout';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatButtonModule } from '@angular/material/button';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatInputModule } from '@angular/material/input';
import { OverlayModule } from '@angular/cdk/overlay';

import { HttpService } from './shared/services/Http.service';
import { ThemesService } from './shared/services/Themes.service';
import { CookieService } from 'ngx-cookie-service';

import { routes } from './app.routing';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    BigPrimeComponent,
    HeaderComponent,
    ScriboAlacritoComponent,
    MastermindComponent,
    PersonalCvComponent,
    BgheaderComponent,
    ContactMeComponent,
    ElectronicsGameComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(routes),
    OverlayModule,
    MatCheckboxModule,
    MatButtonModule,
    MatToolbarModule,
    MatCardModule,
    MatDividerModule,
    MatIconModule,
    MatProgressBarModule,
    MatInputModule,
    MatSnackBarModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    ReactiveFormsModule
  ],
  providers: [HttpService, ThemesService, CookieService],
  bootstrap: [AppComponent]
})
export class AppModule { }

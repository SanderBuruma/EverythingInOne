import { BrowserModule }           from '@angular/platform-browser';
import { NgModule }                from '@angular/core';
import { FormsModule }             from '@angular/forms';
import { HttpClientModule }        from '@angular/common/http';
import { RouterModule }            from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent }            from './app.component';
import { NavMenuComponent }        from './nav-menu/nav-menu.component';
import { HomeComponent }           from './home/home.component';
import { CounterComponent }        from './counter/counter.component';
import { FetchDataComponent }      from './fetch-data/fetch-data.component';

import { MatCheckboxModule }       from '@angular/material/checkbox';
import { MatButtonModule }         from '@angular/material/button';
import { MatToolbarModule }        from '@angular/material/toolbar';
import { MatCardModule }           from '@angular/material/card';
import { MatDividerModule }        from '@angular/material/divider';
import { MatIconModule }           from '@angular/material/icon';
import { MatProgressBarModule }    from '@angular/material/progress-bar';
import { MatSnackBarModule }       from '@angular/material/snack-bar';
import { OverlayModule }           from '@angular/cdk/overlay';

import { HttpService }             from './shared/services/Http.service';
import { ThemesService }           from './shared/services/Themes.service';

import { routes }                 from './app.routing'

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(routes),
    BrowserAnimationsModule,
    OverlayModule,
    MatCheckboxModule,
    MatButtonModule,
    MatToolbarModule,
    MatCardModule,
    MatDividerModule,
    MatIconModule,
    MatProgressBarModule,
    MatSnackBarModule,
    BrowserAnimationsModule
  ],
  providers: [HttpService, ThemesService],
  bootstrap: [AppComponent]
})
export class AppModule { }

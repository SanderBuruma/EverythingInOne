//#region Imports
//#region Core Angular
import { Routes } from '@angular/router';
//#endregion
//#region Components
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { BigPrimeComponent } from './components/big-prime-component/big-prime.component';
import { ScriboAlacritoComponent } from './components/scribo-alacrito/scribo-alacrito.component';
import { MastermindComponent } from './components/mastermind/mastermind.component';
import { ContactMeComponent } from './components/contact-me/contact-me.component';
import { PersonalCvComponent } from './components/personal-cv/personal-cv.component';
//#endregion
import { RouteParamKeys } from './shared/enums/RouteParamKeys.enum';
import { ElectronicsGameComponent } from './components/electronics-game/electronics-game.component';
//#endregion

const routes: Routes = [

  { path: '',                                           component: HomeComponent,              pathMatch: 'full' },

  { path: 'contact-me',                                 component: ContactMeComponent,         pathMatch: 'full' },

  { path: 'big-prime',                                  component: BigPrimeComponent,          pathMatch: 'full' },
  { path: `scribo-alacrito/:${RouteParamKeys.ScriboI}`, component: ScriboAlacritoComponent,    pathMatch: 'full' },
  { path: 'scribo-alacrito',                            component: ScriboAlacritoComponent,    pathMatch: 'full' },
  { path: 'codebreaker',                                component: MastermindComponent,        pathMatch: 'full' },
  { path: 'cv',                                         component: PersonalCvComponent,        pathMatch: 'full' },
  { path: 'electronics-game',                           component: ElectronicsGameComponent,   pathMatch: 'full' },

  { path: '**',                                         component: NotFoundComponent}

];

export { routes };

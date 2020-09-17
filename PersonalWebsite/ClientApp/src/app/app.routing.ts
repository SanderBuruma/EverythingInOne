//#region Imports
//#region Core Angular
import { Routes }                   from "@angular/router";
//#endregion
//#region Components
import { HomeComponent }            from "./home/home.component";
import { NotFoundComponent }        from "./shared/components/not-found/not-found.component";
import { BigPrimeComponent }        from "./big-prime-component/big-prime.component";
import { ScriboAlacritoComponent }  from "./scribo-alacrito/scribo-alacrito.component";
import { SnakeGameComponent }       from "./snake-game/snake-game.component";
//#endregion
//#endregion

let routes: Routes = [

  { path: '',                component: HomeComponent,            pathMatch: 'full' },

  { path: 'big-prime',       component: BigPrimeComponent,        pathMatch: 'full' },
  { path: 'scribo-alacrito', component: ScriboAlacritoComponent,  pathMatch: 'full' },
  { path: 'snake',           component: SnakeGameComponent,       pathMatch: 'full' },

  { path: '**',              component: NotFoundComponent}

]

export { routes }

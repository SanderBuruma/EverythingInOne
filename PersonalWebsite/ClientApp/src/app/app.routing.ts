//#region Imports
//#region Core Angular
import { Routes }            from "@angular/router";
//#endregion
//#region Components
import { HomeComponent }     from "./home/home.component";
import { NotFoundComponent } from "./shared/components/not-found.component";
import { BigPrimeComponent } from "./big-prime-component/big-prime.component";
//#endregion
//#endregion

let routes: Routes = [

  { path: '',        component: HomeComponent,      pathMatch: 'full' },

  { path: 'big-prime',        component: BigPrimeComponent,      pathMatch: 'full' },

  { path: '**',      component: NotFoundComponent}

]

export { routes }

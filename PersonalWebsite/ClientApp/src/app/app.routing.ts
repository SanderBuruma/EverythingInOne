//#region Imports
//#region Core Angular
import { Routes }            from "@angular/router";
//#endregion
//#region Components
import { HomeComponent }     from "./home/home.component";
import { NotFoundComponent } from "./shared/components/not-found.component";
import { FetchDataComponent } from "./fetch-data/fetch-data.component";
import { CounterComponent } from "./counter/counter.component";
//#endregion
//#endregion

let routes: Routes = [

  { path: '',        component: HomeComponent,      pathMatch: 'full' },
  { path: 'weather', component: FetchDataComponent, pathMatch: 'full' },
  { path: 'counter', component: CounterComponent,   pathMatch: 'full' },

  { path: '**',      component: NotFoundComponent}

]

export { routes }

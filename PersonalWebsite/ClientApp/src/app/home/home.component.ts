import { Component } from '@angular/core';
import { ThemesService } from '../shared/services/Themes.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  constructor (public _themesService: ThemesService){

  }
}

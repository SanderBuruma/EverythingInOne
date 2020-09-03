import { Component, OnChanges, SimpleChanges } from '@angular/core';
import { HttpService } from '../shared/services/Http.service';

@Component({
  selector: 'app-big-prime-component',
  templateUrl: './big-prime.component.html',
  styleUrls: ['./big-prime.component.css']
})
export class BigPrimeComponent implements OnChanges {
  public _digitsToGet: number = 0;
  public _prime: string = "131";

  constructor(public _httpService: HttpService) { }

  ngOnChanges(changes: SimpleChanges): void {
    console.log({changes})
  }

  public async GetPrime(digits: number){
    if (digits%1 != 0) digits = 25;
    this._httpService.Get<{prime: string}>("gimmick/bigPrime?digits=" + digits).then(response=>{
      this._prime = response.prime;
      console.log({prime: response})
    })
  }

}

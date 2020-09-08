import { Component, OnChanges, SimpleChanges } from '@angular/core';
import { HttpService } from '../shared/services/Http.service';
import { BaseComponent } from '../shared/base-component/base.component';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-big-prime-component',
  templateUrl: './big-prime.component.html',
  styleUrls: ['./big-prime.component.scss']
})
export class BigPrimeComponent extends BaseComponent {
  public _digitsToGet: number = 50;
  public _prime: string = "131";

  constructor(
    _router: Router,
    _route: ActivatedRoute,
    public _httpService: HttpService
  ) {
    super(_router, _route);
    this.GetPrime(this._digitsToGet);
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log({changes})
  }

  public async GetPrime(digits: number){
    if (digits%1 != 0) digits = 25;
    this._httpService.Get<{prime: string}>("gimmick/bigPrime?digits=" + digits).then(response=>{
      if (response.prime.length==1){
        this._prime = "We know you want some awesome prime numbers, but we're sorry because there may not be more than 1000 or less than 6 digits.";
      } else {
        this._prime = response.prime.replace(/,/g, " ");
      }

    })
  }

}

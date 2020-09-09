import { Component, SimpleChanges }  from '@angular/core';
import { HttpService }                          from '../shared/services/Http.service';
import { BaseComponent }                        from '../shared/base-component/base.component';
import { ActivatedRoute, Router }               from '@angular/router';

@Component({
  selector: 'app-big-prime-component',
  templateUrl: './scribo-alacrito.component.html',
  styleUrls: ['./scribo-alacrito.component.scss']
})
export class ScriboAlacritoComponent extends BaseComponent {
  public _text: string;

  constructor(
    _router: Router,
    _route: ActivatedRoute,
    public _httpService: HttpService
  ) {
    super(_router, _route);
    this.GetText();
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log({changes})
  }

  public async GetText(){
    this._httpService.Get<{ str: string[] }>("scriboAlacrito/getText").then(backendReturnText=>{
      this._text = backendReturnText.str.join("\n");
    })
  }

}

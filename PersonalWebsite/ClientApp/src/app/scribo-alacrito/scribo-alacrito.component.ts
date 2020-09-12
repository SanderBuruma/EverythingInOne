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
  public _input: string = "";
  public _inputPrevLength: number = 0;
  public _i = 0;
  public _textCorrect: string = "";
  public _textFalse: string = "";
  public _textRemaining: string = "";
  public _correct = true;

  constructor(
    _router: Router,
    _route: ActivatedRoute,
    public _httpService: HttpService
  ) {
    super(_router, _route);
    this.GetText();
  }

  ngOnChanges(changes: SimpleChanges): void {
  }

  ChangeEvent(){
    //if input text length is suddenly a lot longer than the previous length there's been a copy paste
    if (this._input.length > this._inputPrevLength + 2){
      this._input = "";
      this._correct = this.UpdateTextColors()
      alert("No copy pasting!");
      return;
    }

    this._correct = this.UpdateTextColors()

    //TODO if input is in equal length to text then finish
    if (this._input.length >= this._text.length && this._correct){
      this.GetText(this._i + 1);
    }

    this._inputPrevLength = this._text.length;
  }

  /**
   * Updates the coloring of the texts.
   * @returns true if correct input
   */
  public UpdateTextColors(){
    // find the length of the input congruent text (stored in i)
    let i = 0;
    for (; i < this._input.length; i++){
      if (this._input.substring(0, this._input.length-i) == this._text.substring(0, this._input.length-i)){
        break;
      }
    }

    //fill in the three spans
    this._textRemaining = this._text.substring(this._input.length)
    if (i == 0){
      // Full match between input and current text.
      this._textCorrect = this._text.substring(0, this._input.length);
      this._textFalse = "";
      return true;
    } else if (i < this._input.length) {
      // Partial match
      this._textCorrect = this._text.substring(0, this._input.length-i);
      this._textFalse = this._text.substring(this._input.length - i, this._input.length);
    } else {
      // No match
      this._textCorrect = "";
      this._textFalse = this._text.substring(0, this._input.length);
    }
    return false;
  }

  /**
   * Gets the next text to use from the server side database
   * @param i the i value to get from the db
   */
  public async GetText(i: number = -1){
    this._httpService.Get<{ str: string, i }>("scriboAlacrito/getText?i=" + i).then(backendReturnText=>{
      this._correct = true;
      this._input = "";
      this._inputPrevLength = 0;
      this._text = backendReturnText.str;
      this._i = backendReturnText.i;
      this.UpdateTextColors();
    })
  }

}

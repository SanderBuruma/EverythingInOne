import { Field, Direction } from "./enums";

export class Board{
  public fields: Field[]
  public direction: Direction
  public ticks: number
  public ticksLeft: number
  public turns: number
}

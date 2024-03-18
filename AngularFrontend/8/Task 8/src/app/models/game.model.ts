import { BaseModel } from './base.model';

export class Game extends BaseModel {
  key!: string;
  name!: string;
  description?: string;

  price?: number;
  unitInStock?: number;
  discontinued?: number;
}

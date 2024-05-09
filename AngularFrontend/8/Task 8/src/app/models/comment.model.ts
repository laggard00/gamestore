import { BaseModel } from './base.model';

export class Comment extends BaseModel {
  name!: string;
  body!: string;

  childComments?: Comment[];
}

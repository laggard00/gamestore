import { Component, OnInit, Input } from '@angular/core';
import { ListItem } from '../list-item-component/list-item';
import { BaseComponent } from '../base.component';
@Component({
  selector: 'app-game-list',
  templateUrl: './game-list.component.html',
  styleUrls: ['./game-list.component.scss']
})
export class GameListComponent extends BaseComponent {
  @Input()
  gameList: ListItem[] = [];

  @Input()
  addLink?: string;

  super() { }

  

}

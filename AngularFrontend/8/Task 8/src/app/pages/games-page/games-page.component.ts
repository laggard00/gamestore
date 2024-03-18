import { HttpParams } from '@angular/common/http';
import { Component } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { ListItem } from 'src/app/componetns/list-item-component/list-item';
import { GameService } from 'src/app/services/game.service';

@Component({
  selector: 'gamestore-games',
  templateUrl: './games-page.component.html',
  styleUrls: ['./games-page.component.scss'],
})
export class GamesPageComponent extends BaseComponent {
  gamesList: ListItem[] = [];
  pagesSubject = new BehaviorSubject<{ totalPages: number; page: number }>({
    totalPages: 1,
    page: 1,
  });

  gamesLoadSubject = new Subject<void>();

  constructor(private gameService: GameService) {
    super();
  }

  loadGames(filterParams: HttpParams): void {
    this.gameService
      .getGames(filterParams)
      .pipe(
        tap((x) => {
          this.pagesSubject.next({
            totalPages: !!x.totalPages ? x.totalPages : 1,
            page: !!x.currentPage ? x.currentPage : 1,
          });
        }),
        map((gamesInfo) =>
          gamesInfo.games.map((game) => {
            const gameItem: ListItem = {
              title: game.name,
              price : game.price,
              pageLink: `${this.links.get(this.pageRoutes.Game)}/${game.key}`,
              updateLink: `${this.links.get(this.pageRoutes.UpdateGame)}/${game.key
                }`,
              deleteLink: `${this.links.get(this.pageRoutes.DeleteGame)}/${game.key
                }`,
            };

            return gameItem;
          })
        )
      )
      .subscribe((x) => this.onGamesLoaded(x), () => this.onGamesLoaded([]));
  }

  private onGamesLoaded(games: ListItem[]): void {
    this.gamesList = games ?? [];
    this.gamesLoadSubject.next();
  }
}

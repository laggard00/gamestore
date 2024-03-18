import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { BaseComponent } from 'src/app/componetns/base.component';
import { InfoItem } from 'src/app/componetns/info-component/info-item';
import { Game } from 'src/app/models/game.model';
import { Genre } from 'src/app/models/genre.model';
import { Platform } from 'src/app/models/platform.model';
import { Publisher } from 'src/app/models/publisher.model';
import { GameService } from 'src/app/services/game.service';
import { GenreService } from 'src/app/services/genre.service';
import { OrderService } from 'src/app/services/order.service';
import { PlatformService } from 'src/app/services/platform.service';
import { PublisherService } from 'src/app/services/publisher.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'gamestore-game',
  templateUrl: './game-page.component.html',
  styleUrls: ['./game-page.component.scss'],
})
export class GamePageComponent
  extends BaseComponent
  implements OnInit, AfterViewInit
{
  private file?: Blob;

  @ViewChild('download')
  downloadLink!: ElementRef;

  gameValue?: Game;

  canSeeComments = true;
  canBuy = false;

  gameInfoList: InfoItem[] = [];

  constructor(
    private gameService: GameService,
    private genreService: GenreService,
    private platformService: PlatformService,
    private publisherService: PublisherService,
    private orderService: OrderService,
    private route: ActivatedRoute,
    private userService: UserService,
    private router: Router
  ) {
    super();
  }

  get deleteGameLink(): string {
    return `${this.links.get(this.pageRoutes.DeleteGame)}/${
      this.gameValue?.key
    }`;
  }

  get updateGameLink(): string {
    return `${this.links.get(this.pageRoutes.UpdateGame)}/${
      this.gameValue?.key
    }`;
  }

  get game(): Game | undefined {
    return this.gameValue;
  }

  set game(value: Game | undefined) {
    this.gameValue = value;
    this.gameInfoList = [];
    if (!value) {
      return;
    }

    this.gameInfoList.push(
      {
        name: this.labels.gameNameLabel,
        value: value.name,
      },
      {
        name: this.labels.gameKeyLabel,
        value: value.key,
      },
      {
        name: this.labels.gameDescriptionLabel,
        value: value.description ?? '',
      },
      {
        name: this.labels.gamePriceLabel,
        value: (value.price ?? 0).toString(),
      },
      {
        name: this.labels.gameDiscontinuedLabel,
        value: (value.discontinued ?? 0).toString(),
      },
      {
        name: this.labels.gameUnitInStockLabel,
        value: (value.unitInStock ?? 0).toString(),
      }
    );
  }

  ngOnInit(): void {
    this.getRouteParam(this.route, 'key')
      .pipe(
        switchMap((key) => this.gameService.getGame(key)),
        tap((x) => (this.game = x)),
        switchMap((x) =>
          forkJoin({
            genres: this.genreService.getGenresByGameKey(x.key),
            platforms: this.platformService.getPlatformsByGameKey(x.key),
            file: this.gameService.getGameFile(x.key),
            publisher: this.publisherService.getPublisherByGameKey(x.key),
            canSeeComments: this.userService.checkAccess('Comments', x.key),
            canBuy: this.userService.checkAccess('Buy', x.key),
          })
        )
      )
      .subscribe((x) => {
        this.addPatformsInfo(x.platforms);
        this.addGenresInfo(x.genres);
        this.file = x.file;
        this.addDownloadFile();
        this.addPublisherInfo(x.publisher);
        this.canSeeComments = x.canSeeComments;
        this.canBuy = x.canBuy;
      });
  }

  ngAfterViewInit(): void {
    this.addDownloadFile();
  }

  addDownloadFile(): void {
    if (!!this.file && !!this.downloadLink) {
      const downloadURL = window.URL.createObjectURL(this.file);
      (this.downloadLink as any)._elementRef.nativeElement.href = downloadURL;
    }
  }

  addPatformsInfo(platforms: Platform[]): void {
    if (!platforms?.length) {
      return;
    }

    const platformsInfo: InfoItem = {
      name: this.labels.platformsMenuItem,
      nestedValues: [],
    };
    platforms.forEach((x) =>
      platformsInfo.nestedValues!.push({
        title: x.type,
        pageLink: `${this.links.get(this.pageRoutes.Platform)}/${x.id}`,
      })
    );

    this.gameInfoList.push(platformsInfo);
  }

  addPublisherInfo(publisher: Publisher): void {
    if (!publisher) {
      return;
    }

    this.gameInfoList.push({
      name: this.labels.publisherLabel,
      value: publisher.companyName,
      pageLink: !publisher?.id?.length
        ? undefined
        : `${this.links.get(this.pageRoutes.Publisher)}/${
            publisher.companyName
          }`,
    });
  }

  addGenresInfo(genres: Genre[]): void {
    if (!genres?.length) {
      return;
    }

    const genresInfo: InfoItem = {
      name: this.labels.genresMenuItem,
      nestedValues: [],
    };
    genres.forEach((x) =>
      genresInfo.nestedValues!.push({
        title: x.name,
        pageLink: !x?.id?.length
          ? undefined
          : `${this.links.get(this.pageRoutes.Genre)}/${x.id}`,
      })
    );

    this.gameInfoList.push(genresInfo);
  }

  buy(): void {
    this.orderService
      .buyGame(this.game!.key)
      .subscribe((_) =>
        this.router.navigateByUrl(this.links.get(this.pageRoutes.Basket)!)
      );
  }
}

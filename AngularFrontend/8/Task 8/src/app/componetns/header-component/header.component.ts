import { Component, OnInit } from '@angular/core';
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { gameCountSubject } from 'src/app/configuration/shared-info';
import { UserService } from 'src/app/services/user.service';
import { BaseComponent } from '../base.component';

@UntilDestroy()
@Component({
  selector: 'gamestore-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent extends BaseComponent implements OnInit {
  gameCount: string | null = null;

  constructor(private userService: UserService) {
    super();
  }

  get isAuth(): boolean {
    return this.userService.isAuth();
  }

  ngOnInit(): void {
    gameCountSubject
      .pipe(untilDestroyed(this))
      .subscribe((x) => (this.gameCount = x));
  }
}

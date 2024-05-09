import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { forkJoin } from 'rxjs';
import { BaseComponent } from 'src/app/componetns/base.component';
import { Comment } from 'src/app/models/comment.model';
import { UserService } from 'src/app/services/user.service';
import { CommentAction } from './comment-action';

@Component({
  selector: 'gamestore-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss'],
})
export class CommentComponent extends BaseComponent implements OnInit {
  @Input()
  comment!: Comment;

  @Output()
  select = new EventEmitter<{ comment: Comment; action: CommentAction }>();

  @Output()
  deleteComment = new EventEmitter<Comment>();

  @Output()
  banComment = new EventEmitter<Comment>();

  action = CommentAction;

  canReply = false;
  canQuote = false;
  canDelete = false;
  canBan = false;

  constructor(private userService: UserService) {
    super();
  }

  ngOnInit(): void {
    forkJoin({
      canReply: this.userService.checkAccess('ReplyComment', this.comment.id),
      canQuote: this.userService.checkAccess('QuoteComment', this.comment.id),
      canDelete: this.userService.checkAccess('DeleteComment', this.comment.id),
      canBan: this.userService.checkAccess('BanComment', this.comment.id),
    }).subscribe((x) => {
      this.canReply = x.canReply;
      this.canQuote = x.canQuote;
      this.canDelete = x.canDelete;
      this.canBan = x.canBan;
    });
  }

  onAction(action: CommentAction): void {
    this.select.emit({ comment: this.comment, action });
  }
}

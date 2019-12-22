import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { UsersService } from '../../_services/users.service';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  users: User[];

  constructor(private userService: UsersService,
              private alertifyService: AlertifyService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    // this.loadUsers();
    this.activatedRoute.data.subscribe(data => {
      this.users = data.dbusers; // dbusers name must match with route.ts route resover
    });
  }

  loadUsers() {
    // this.userService.getUsers().subscribe((dbUsers: User[]) => {
    // this.users = dbUsers;
    // },
    // (err: any) => {
    //   this.alertifyService.error(err);
    // },
    // () => this.alertifyService.message('call complete')
    // );
  }
}

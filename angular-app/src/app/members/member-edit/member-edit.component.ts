import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UsersService } from 'src/app/_services/users.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm', {static: true}) memberEditForm: NgForm;
  user: User;

  // for warning accidently closing edit form without saving changes
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.memberEditForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute, private alertifyService: AlertifyService,
              private userService: UsersService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
    this.user = data.user;
     });
  }
  updateUser() {
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
      this.alertifyService.success('profile updated successfully');

      // this reset our form to its initial state, so that dirty class works on form.
      this.memberEditForm.reset(this.user);
    }, error => {
        this.alertifyService.error(error);
    });
  }

}
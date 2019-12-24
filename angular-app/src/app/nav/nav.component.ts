import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  photoUrl: string;
  constructor(public authService: AuthService,
              private alertifyService: AlertifyService, private route: Router) { }

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(a => this.photoUrl = a);
  }
  login() {
    this.authService.login(this.model).subscribe(next => {
        this.alertifyService.success('Logged in successfully');
        this.route.navigate(['/members']);
      },
      error => {
        this.alertifyService.error(error);
      });
  }
  loggedIn() {
     return this.authService.loggedIn();
  }
  logout() {
     localStorage.removeItem('token');
     localStorage.removeItem('user');
     this.authService.decodedToken = null;
     this.authService.currentUser = null;
     this.alertifyService.message('logged out');
     this.route.navigate(['/home']);
  }

}

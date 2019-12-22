import { Injectable } from '@angular/core';
import { User } from '../_models/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { UsersService } from '../_services/users.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MemberListResolver implements Resolve<User[]> {
  constructor(
    private userService: UsersService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
      // resolver automatically subscribe to method unlike we do in component.
    return this.userService.getUsers().pipe(
      catchError(error => {
        this.alertify.error('Problem retrieving data' + error);
        this.router.navigate(['/home']);
        return of(null); // rxjs 6
      })
    );
  }
}

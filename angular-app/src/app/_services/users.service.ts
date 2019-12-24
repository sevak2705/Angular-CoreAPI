import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { User } from '../_models/user';

// const httpOptions = {
//   headers: new HttpHeaders({
//     Authorization: 'Bearer ' + localStorage.getItem('item')
//   })
// };
@Injectable({
  providedIn: 'root'
})
export class UsersService {

constructor(private http: HttpClient) { }

baseurl = environment.apiUrl;

// as this methods are authorized in api we need to pass token for authentication
getUsers(): Observable<User[]> {
  console.log(`${this.baseurl}users`);
  return this.http.get<User[]>(`${this.baseurl}users`);
 // return this.http.get<User[]>(`${this.baseurl}users`, httpOptions);
}
getUser(id: any): Observable<User> {
  return this.http.get<User>(`${this.baseurl}users/${id}`);
 // return this.http.get<User>(`${this.baseurl}users/${id}`, httpOptions);
 }
updateUser(id: number, user: User) {
  return this.http.put(this.baseurl + 'users/' + id, user);

}
setMainPhoto(userId: number, id: number) {
return this.http.post(this.baseurl + 'users/' + userId + '/photos/' + id + '/setMain', {});
}
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { throwError, BehaviorSubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;
  photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  currentPhotoUrl = this.photoUrl.asObservable();

  constructor(private httpClient: HttpClient) {}

  changeMemberPhoto(photoUrl: string) {
    this.photoUrl.next(photoUrl);
  }

  login(user: User) {
    return this.httpClient.post(this.baseUrl + 'login', user).pipe(
      map((response: any) => {
        const usr = response;
        if (usr) {
          localStorage.setItem('token', usr.token);
          localStorage.setItem('user', JSON.stringify(usr.user));
          this.decodedToken = this.jwtHelper.decodeToken(usr.token);
          this.currentUser = usr.user;
          this.changeMemberPhoto(this.currentUser.photoUrl);
        }
      }),
      catchError((error) => throwError(error))
    );
  }

  register(user: User) {
    return this.httpClient.post(this.baseUrl + 'register', user).pipe(
      map(response => response),
      catchError(error => throwError(error))
    );
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}

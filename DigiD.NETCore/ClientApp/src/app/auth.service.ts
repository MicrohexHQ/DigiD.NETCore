import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { tap, delay, catchError } from 'rxjs/operators';
import { DigidUser } from './digidUser';

@Injectable()
export class AuthService {
  isLoggedIn = false;
  lastLoginFailed = false;
  user: DigidUser;

  readonly keyRedirectUrl = 'AuthService.redirectUrl';
  readonly userDetailsUrl = '/api/auth/me';

  constructor(private router: Router, private http: HttpClient) {
    this.initLoginState();
  }

  initiateLogin(currentUrl: string): void {
    console.log('AuthService: initiateLogin', currentUrl);
    this.resetState();

    // Store currentUrl for later redirection
    if (currentUrl) {
      sessionStorage.setItem(this.keyRedirectUrl, currentUrl);
    }

    window.location.replace('/auth/login?returnUrl=/auth-callback/login_success'); // Evt navigate ipv replace
  }

  loggedIn(): void {
    console.log('AuthService: loggedIn');

    this.getUserDetails()
      .subscribe(user => {
        this.user = user;
        console.log(`AuthService: loggedIn userdetails: GebruikerID=${this.user.gebruikerID}`);
        this.isLoggedIn = true;
        this.lastLoginFailed = false;
        this.redirectIfRequired();
      }, err => {
        console.error('AuthService: loggedIn userdetails: error during retrieval', err);
        this.failedLogIn();
      });
  }

  failedLogIn(): void {
    console.log('AuthService: failedLogIn');
    this.isLoggedIn = false;
    this.lastLoginFailed = true;

    this.redirectIfRequired();
  }

  loggedOut(): void {
    console.log('AuthService: loggedOut');
    this.isLoggedIn = false;
    this.lastLoginFailed = false;

    // Bij uitlog altijd naar default route
    this.router.navigate(['']);
  }

  initiateLogout(currentUrl: string): void {
    console.log('AuthService: initiateLogout', currentUrl);

    // Store currentUrl for later redirection
    if (currentUrl) {
      sessionStorage.setItem(this.keyRedirectUrl, currentUrl);
    }

    window.location.replace('/auth/logout?returnUrl=/auth-callback/logout_success');
  }

  private getUserDetails(): Observable<DigidUser> {
    return this.http.get<DigidUser>(this.userDetailsUrl)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
    }
    // return an observable with a user-facing error message
    return throwError(
      'Something bad happened; please try again later.');
  }

  private initLoginState(): void {
    // Try to find out if the server already accepts our cookie and considers us logged in.
    // In that case, modify client-side state accordingly.
    this.getUserDetails()
      .subscribe(user => {
        this.user = user;
        console.log(`AuthService: initLoginState - already logged in: GebruikerID=${this.user.gebruikerID}`);
        this.isLoggedIn = true;
        this.lastLoginFailed = false;
      }, err => {
        console.error('AuthService: initLoginState - not already logged in', err);
      });
  }

  private resetState(): void {
    this.isLoggedIn = false;
    this.lastLoginFailed = false;
    this.user = null;
    sessionStorage.removeItem(this.keyRedirectUrl);
  }

  private redirectIfRequired(): void {
    const redirectUrl = sessionStorage.getItem(this.keyRedirectUrl);
    if (redirectUrl !== null) {
      console.log('redirectUrl type', typeof redirectUrl);
      console.log('AuthService: navigating to ' + redirectUrl);
      this.router.navigate([redirectUrl]).then(_ => { }, err => { window.location.replace(redirectUrl); });
    }
  }
}

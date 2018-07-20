import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const currentUrl: string = state.url;

    return this.checkLogin(currentUrl);
  }

  checkLogin(currentUrl: string): boolean {
    if (this.authService.isLoggedIn) { return true; }

    this.authService.initiateLogin(currentUrl);

    return false;
  }
}

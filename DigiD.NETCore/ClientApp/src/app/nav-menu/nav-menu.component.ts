import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { ActivatedRouteSnapshot } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(public authService: AuthService) {}

  login() {
    console.log('NavMenuComponent: login');
    this.authService.initiateLogin(null);
  }

  logout() {
    console.log('NavMenuComponent: logout');
    this.authService.initiateLogout(null);
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}

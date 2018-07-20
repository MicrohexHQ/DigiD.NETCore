import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styleUrls: ['./auth-callback.component.css']
})
export class AuthCallbackComponent {

  constructor(private router: Router, private route: ActivatedRoute, private authService: AuthService) {
    this.route.params.subscribe(params => {
      console.log('AuthCallbackComponent', params);

      const status = params['status'];

      if (status) {
        if (status === 'login_success') {
          this.authService.loggedIn();
        } else if (status === 'login_failed') {
          this.authService.failedLogIn();
        } else if (status === 'logout_success') {
          this.authService.loggedOut();
        }
      }

      // Otherwise, just navigate to the root of the site
      this.router.navigate(['']);
    });
  }
}

import { Component } from '@angular/core';
import { AccountService } from 'src/app/_services/account.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent {
  constructor(private accountService: AccountService, private router: Router) {}

  get currentUser$() {
    return this.accountService.currentUser$;
  }

  returnToMembersPage(): void {
    this.router.navigate(['/members']);
  }

  goToHomePage(): void {
    this.router.navigate(['/']);
  }
}
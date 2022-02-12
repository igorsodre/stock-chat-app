import { DataStoreService } from './../../services/data-store.service';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(private dataStoreService: DataStoreService, private router: Router) {}

  userName = '';

  setUserName() {
    this.dataStoreService.setUserName(this.userName);
    this.router.navigate(['/chat']);
  }
}

import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { Nav } from "../layout/nav/nav";
import { AccountService } from '../core/services/account-service';
import { Home } from "../features/home/home";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [Nav, Home],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App implements OnInit {
  
  private http = inject(HttpClient);
  protected readonly title = 'Dating App';
  protected members = signal<any>([]);
  private accountService = inject(AccountService);

async ngOnInit() {
  this.members.set(await this.getMembers());
  this.setCurrentUser();
  }

  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) {
      return;
    }
    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }

 async getMembers() {
  try {
  return lastValueFrom(this.http.get('https://localhost:5001/api/members'));
  } catch (error) {
    console.error('Error fetching members:', error);
    throw error;
  }
}
  
}

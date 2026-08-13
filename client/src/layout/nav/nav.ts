import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './nav.html',
  styleUrls: ['./nav.css'],
})
export class Nav {
protected creds: any={}
protected accountService= inject(AccountService);


login(){
  this.accountService.login(this.creds).subscribe({
    next: (response) => {    
        console.log(response);   
       
           this.creds={};
              },
    error: (error) => {      alert(error.message);    }
  });
}

logout(){
  this.accountService.logout();
}

}

import { Component, signal } from '@angular/core';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.html',
  styleUrls: ['./home.css'],
})
export class Home {
protected registerMode = signal(false);

showRegister()
{
  this.registerMode.set(true);
}


}

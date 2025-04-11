import { Component } from '@angular/core';
import { NavBarComponent } from './core/components/nav-bar/nav-bar.component';
import { ToastComponent } from './shared/toast/toast.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  imports: [NavBarComponent, ToastComponent, RouterOutlet],
})
export class AppComponent {}

import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatTab, MatTabGroup} from '@angular/material/tabs'
import { MatToolbar } from '@angular/material/toolbar';
import { PollingComponent } from './polling/polling.component';
import { SignalrComponent } from './signalr/signalr.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,MatTab,MatTabGroup,MatToolbar,PollingComponent,SignalrComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'ngPollingEtSignalR';
}

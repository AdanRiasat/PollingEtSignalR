import { Component, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { UselessTask } from '../models/UselessTask';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-signalr',
  standalone: true,
  imports: [
    MatCardModule,
    MatFormFieldModule,
    FormsModule,
    CommonModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
  ],
  templateUrl: './signalr.component.html',
  styleUrls: ['./signalr.component.css'],
})
export class SignalrComponent implements OnInit {
  private hubConnection?: signalR.HubConnection;
  usercount = 0;
  tasks: UselessTask[] = [];
  taskname: string = '';
  

  ngOnInit(): void {
    this.connecttohub();
  }

  connecttohub() {
    // TODO On doit commencer par créer la connexion vers le Hub
    // TODO On peut commencer à écouter pour les évènements qui vont déclencher des callbacks
    // TODO On doit ensuite se connecter

    this.hubConnection = new signalR.HubConnectionBuilder().withUrl("https://localhost:7289/taskHub").build()

    this.hubConnection!.on('TaskList', (data: UselessTask[]) => {
      this.tasks = data
      console.log(data);
    })

    this.hubConnection
        .start()
        .then(() => {
            console.log('La connexion est active!');
          })
        .catch(err => console.log('Error while starting connection: ' + err));
  }

  complete(id: number) {
    // TODO On invoke la méthode pour compléter une tâche sur le serveur
    this.hubConnection!.invoke('Complete', id)

    this.hubConnection!.on('TaskList', (data: UselessTask[]) => {
      this.tasks = data
      console.log(data);
    })
  }

  addtask() {
    // TODO On invoke la méthode pour ajouter une tâche sur le serveur

    this.hubConnection!.invoke('Add', this.taskname)

    this.hubConnection!.on('TaskList', (data: UselessTask[]) => {
      this.tasks = data
      console.log(data);
    })
  }
}

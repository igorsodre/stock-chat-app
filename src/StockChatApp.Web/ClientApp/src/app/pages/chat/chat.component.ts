import { Message } from './../../models/message';
import { Component, OnInit } from '@angular/core';
import { DataStoreService } from 'src/app/services/data-store.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent implements OnInit {
  userName: string;
  messages: Message[] = [
    {
      userName: 'teste',
      content: 'Hi Aiden, how are you? How is the project coming along?',
      timestamp: new Date(),
    },
    {
      userName: 'Aiden',
      content: 'Are we meeting today?',
      timestamp: new Date(),
    },
  ];
  constructor(private dataStoreService: DataStoreService) {
    this.userName = this.dataStoreService.getUserName();
  }
  ngOnInit(): void {
    // Fetch the latest 50 messages
  }

  isItMe(userName: string) {
    return userName === this.userName;
  }
}

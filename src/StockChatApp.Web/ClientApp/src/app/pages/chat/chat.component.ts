import { CommandService } from './../../services/command.service';
import { Message } from './../../models/message';
import { Component, OnInit } from '@angular/core';
import { DataStoreService } from 'src/app/services/data-store.service';
import { ChatService } from 'src/app/services/chat.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent implements OnInit {
  userName: string;
  content = '';
  isChatAvailable = false;
  messages: Message[] = [];
  constructor(
    private dataStoreService: DataStoreService,
    private chatService: ChatService,
    private commandService: CommandService
  ) {
    this.userName = this.dataStoreService.getUserName();
  }

  ngOnInit(): void {
    this.chatService
      .startChat()
      .then(() => {
        this.isChatAvailable = true;
        this.subscribeToChat();
      })
      .catch(() => {
        alert('Could not connect to the chat room');
      });
  }

  sendMessage() {
    if (!this.content || !this.chatService.getConnectionId()) return;

    if (this.commandService.IsValidCommand(this.content)) {
      this.commandService.SendCommand(this.content, this.chatService.getConnectionId())?.subscribe(
        (_) => {},
        (_) => {
          alert('Could not execute the command');
        }
      );
    } else {
      this.chatService.sendMessage(this.userName, this.content);
    }

    this.clearContent();
  }

  isItMe(userName: string) {
    return userName === this.userName;
  }

  private clearContent() {
    this.content = '';
  }

  private subscribeToChat() {
    this.chatService.message$.subscribe((message) => {
      if (!message) return;
      this.messages.push(message);
    });
  }
}

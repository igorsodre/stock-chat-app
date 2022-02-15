import { CommandService } from './../../services/command.service';
import { IMessage } from './../../models/message';
import { AfterViewChecked, AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { DataStoreService } from 'src/app/services/data-store.service';
import { ChatService } from 'src/app/services/chat.service';
import { ChatMessageService } from 'src/app/services/chat-message.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent implements OnInit {
  userName: string;
  content = '';
  isChatAvailable = false;
  scrollOnReceiveMessage = true;
  messages: IMessage[] = [];
  private readonly MAX_CHAT_SIZE = 60;
  @ViewChild('chatHistory') private chatHistory?: ElementRef;

  constructor(
    private dataStoreService: DataStoreService,
    private chatService: ChatService,
    private chatMessageService: ChatMessageService,
    private commandService: CommandService
  ) {
    this.userName = this.dataStoreService.getUserName();
  }

  ngOnInit(): void {
    this.chatMessageService.getMessages().subscribe((reponse) => {
      this.messages = reponse;
      setTimeout(() => {
        this.scrollToBottom();
      });
    });

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
    // this.scrollToBottom();
  }

  isItMe(userName: string) {
    return userName === this.userName;
  }

  toggleScrollWhenReceiveMessage() {
    this.scrollOnReceiveMessage = !this.scrollOnReceiveMessage;
  }

  private clearContent() {
    this.content = '';
  }

  private scrollToBottom() {
    this.chatHistory?.nativeElement?.scroll({
      top: this.chatHistory.nativeElement.scrollHeight * 2,
      left: 0,
      behavior: 'smooth',
    });
  }

  private subscribeToChat() {
    this.chatService.message$.subscribe((message) => {
      if (!message) return;
      this.addMessage(message);
    });
  }

  private addMessage(message: IMessage) {
    if (this.messages.length >= this.MAX_CHAT_SIZE) {
      this.messages.shift();
    }
    this.messages.push(message);
    if (this.scrollOnReceiveMessage) {
      setTimeout(() => {
        this.scrollToBottom();
      });
    }
  }
}

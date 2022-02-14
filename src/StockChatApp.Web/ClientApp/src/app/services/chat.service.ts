import { IMessage } from './../models/message';
import { Inject, Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ChatService {
  private chatUrl = 'hubs/chat';
  private readonly _messageSource = new BehaviorSubject<IMessage | null>(null);
  readonly message$ = this._messageSource.asObservable();
  private _connection: HubConnection | undefined;
  private get endpoint() {
    return `${this.baseUrl}${this.chatUrl}`;
  }

  constructor(@Inject('BASE_URL') private baseUrl: string) {}

  async startChat() {
    if (!this._connection) {
      await this.buildConnection();
      this.addEventListeners();
    }
  }

  sendMessage(userName: string, content: string) {
    if (!this._connection) return;

    this._connection.invoke('SendMessage', userName, content);
  }

  getConnectionId() {
    return this._connection?.connectionId || '';
  }

  private async buildConnection() {
    this._connection = new HubConnectionBuilder().withUrl(this.endpoint).build();
    await this._connection.start();
  }

  private addEventListeners() {
    this._connection?.on('ReceiveMessage', (data: IMessage) => {
      this._messageSource.next(data);
    });
  }
}

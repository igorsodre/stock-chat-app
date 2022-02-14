import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { ISuccess } from '../models/api/default-response';
import { IMessage } from '../models/message';

@Injectable({
  providedIn: 'root',
})
export class ChatMessageService {
  private readonly chatMessagesUrl = 'api/chat-messages';
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {}

  getMessages() {
    const endpoint = this.baseUrl + this.chatMessagesUrl;
    return this.http.get<ISuccess<IMessage[]>>(endpoint).pipe(map((res) => res.data));
  }
}

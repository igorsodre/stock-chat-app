import { ICommandRequest } from './../models/api/command-request';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CommandService {
  private readonly commandsUrl = 'api/commands';
  private validCommands = ['/stock'];
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {}

  IsValidCommand(message: string) {
    if (!message) return false;
    const args = message.split('=');

    if (args.length !== 2) return false;

    return this.validCommands.includes(args[0]);
  }

  SendCommand(message: string, connectionId: string) {
    if (!this.IsValidCommand(message)) return;

    const endpoint = this.baseUrl + this.commandsUrl;
    const args = message.split('=');
    const body: ICommandRequest = { command: args[0], arguments: args[1], connectionId: connectionId };
    return this.http.post<string>(endpoint, body);
  }
}

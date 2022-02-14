import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class DataStoreService {
  private readonly _userNameSource = new BehaviorSubject<string>('');
  readonly userName$ = this._userNameSource.asObservable();
  constructor() {}

  getUserName() {
    return this._userNameSource.getValue();
  }

  setUserName(userName: string) {
    this._userNameSource.next(userName);
  }
}

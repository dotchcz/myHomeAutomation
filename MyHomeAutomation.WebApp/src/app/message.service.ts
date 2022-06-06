import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class MessageService {
  messages: string[] = [];

  add(message: string) {
    const date: Date = new Date();
    this.messages.unshift(date + ": " + message);
  }

  clear() {
    this.messages = [];
  }
}

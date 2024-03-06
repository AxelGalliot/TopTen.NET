import { Component, OnInit } from '@angular/core';
import { SignalrService } from '../../services/signalr.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css' 
})
export class HomeComponent {
  tabs: string[] = ["Créer une partie", "Rejoindre une partie"]
  activatedTabIndex: number = 0
  avatar: string = 'avatar01'
  name: string = ''
  group: string = ''

  constructor(public signalRService: SignalrService) { }

  tabChange(tabIndex: number) {
    this.activatedTabIndex = tabIndex
  }

  onNameValueChange(value: string) {
    this.name = value
  }

  onGroupValueChange(value: string) {
    this.group = value
  }

  onAvatarValueChange(value: string) {
    this.avatar = value
  }

  createGroup() {
    this.signalRService.createGroupInvoker(this.avatar, this.name)
  }

  joinGroup() {
    this.signalRService.joinGroupInvoker(this.avatar, this.name, this.group)
  }
}

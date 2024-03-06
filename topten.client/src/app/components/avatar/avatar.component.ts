import { Component, HostBinding, Input } from '@angular/core';

@Component({
  selector: 'app-avatar',
  templateUrl: './avatar.component.html',
  styleUrl: './avatar.component.css'
})
export class AvatarComponent {
  @Input() id: string = ''
  @Input() avatar: string = ''
  @Input() size: number = 48
  @Input() isAnswerMode: boolean = false
  @Input() isHost: boolean = false
  @Input() isCaptain: boolean = false

  @HostBinding('style.width.px')
  get width(): number {
    return this.size
  }

  gearStyle(): Object {
    return { top: this.size * 5 / 8, left: this.size * 5 / 8 }
  }
}

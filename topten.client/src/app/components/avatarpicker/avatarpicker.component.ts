import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-avatarpicker', 
  templateUrl: './avatarpicker.component.html',
  styleUrl: './avatarpicker.component.css'
})
export class AvatarpickerComponent {
  @Input() initialAvatar: string = ''
  id: number
  @Output() valueChange = new EventEmitter<string>()

  avatars: string[] = [
    "avatar01", 
    "avatar02", 
    "avatar03", 
    "avatar04", 
    "avatar05", 
    "avatar06", 
    "avatar07", 
    "avatar08", 
    "avatar09", 
    "avatar10", 
    "avatar11",
    "avatar12",
    "avatar13",
    "avatar14",
  ]

  constructor() {
    if (this.initialAvatar === '' || this.avatars.indexOf(this.initialAvatar) == -1) {
      this.id = 0
    }
    else {
      this.id = this.avatars.indexOf(this.initialAvatar)
    }
  }

  onPrevious() {
    if (this.id == 0)
      this.id = this.avatars.length - 1
    else
      this.id -= 1
    
    this.valueChange.emit(this.avatars[this.id])
  }

  onNext() {
    if (this.id == this.avatars.length - 1)
      this.id = 0
    else
      this.id += 1
    
    this.valueChange.emit(this.avatars[this.id])
  }
}
